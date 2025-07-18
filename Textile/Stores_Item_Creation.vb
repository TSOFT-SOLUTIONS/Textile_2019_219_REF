Imports System.IO
Public Class Stores_Item_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private Prec_ActCtrl As New Control
    Dim New_Entry As Boolean
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl

    Private dgv_LevColNo As Integer


    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        grp_Open.Visible = False

        lbl_IdNo.ForeColor = Color.Black
        lbl_IdNo.Text = ""

        txt_Name.Text = ""
        txt_Code.Text = ""
        txt_ItemType.Text = ""
        cbo_Department.Text = ""
        txt_DrawingNo.Text = ""
        cbo_ReedCount.Text = ""
        cbo_ReedWidth.Text = ""
        cbo_Unit.Text = ""
        txt_MinimumStock.Text = ""
        txt_ReOrderQty.Text = ""
        txt_TaxPerc.Text = ""
        cbo_ItemGroup.Text = ""
        cbo_RackNo.Text = ""
        txt_catelog_pageno.Text = ""
        cbo_motion_type.Text = ""
        cbo_Filter_Brand.Text = ""
        cbo_Filter_Department.Text = ""
        cbo_Filter_RackNo.Text = ""



        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing

        dgv_Details.Rows.Clear()
        dgv_Filter.Rows.Clear()



        Grid_DeSelect()

        cbo_Brand.Visible = False
        cbo_Brand.Tag = -1
        cbo_Brand.Text = ""


        dgv_Details.Tag = ""
        dgv_LevColNo = -1

    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub
    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False

    End Sub
    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da = New SqlClient.SqlDataAdapter("select a.*, c.Unit_Name, d.Department_Name, e.Count_Name, f.ReedWidth_Name from Stores_item_head a LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo  LEFT OUTER JOIN Department_Head d ON a.Department_IDNo = d.Department_IDNo LEFT OUTER JOIN Count_head e ON a.reedCount_IdNo = e.Count_IdNo LEFT OUTER JOIN ReedWidth_Head f ON a.ReedWidth_IdNo = f.ReedWidth_IdNo   where a.Item_IdNo = " & Str(Val(idno)), con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                lbl_IdNo.Text = dt.Rows(0).Item("Item_IdNo").ToString
                txt_Name.Text = dt.Rows(0).Item("Item_DisplayName").ToString
                txt_Code.Text = dt.Rows(0).Item("Item_Code").ToString
                txt_ItemType.Text = dt.Rows(0).Item("Item_Type").ToString
                cbo_Department.Text = dt.Rows(0).Item("Department_Name").ToString
                txt_DrawingNo.Text = dt.Rows(0).Item("Drawing_No").ToString
                cbo_ReedCount.Text = dt.Rows(0).Item("Count_Name").ToString
                cbo_ReedWidth.Text = dt.Rows(0).Item("ReedWidth_Name").ToString
                cbo_Unit.Text = dt.Rows(0).Item("Unit_Name").ToString

                cbo_ItemGroup.Text = Common_Procedures.ItemGroup_IdNoToName(con, Val(dt.Rows(0).Item("ItemGroup_IdNo").ToString))

                cbo_RackNo.Text = Common_Procedures.Rack_IdNoToNo(con, Val(dt.Rows(0).Item("Rack_IdNo").ToString))

                If Val(dt.Rows(0).Item("Minimum_Stock").ToString) <> 0 Then txt_MinimumStock.Text = Val(dt.Rows(0).Item("Minimum_Stock").ToString)
                If Val(dt.Rows(0).Item("ReOrder_Quantity").ToString) <> 0 Then txt_ReOrderQty.Text = Val(dt.Rows(0).Item("ReOrder_Quantity").ToString)
                If Val(dt.Rows(0).Item("Tax_Percentage").ToString) <> 0 Then txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString



                txt_catelog_pageno.Text = dt.Rows(0).Item("Catelog_pageno").ToString
                cbo_motion_type.Text = Common_Procedures.Motion_Type_IdNoToName(con, Val(dt.Rows(0).Item("Motion_Type_IdNo").ToString))

                'If Val(dt.Rows(0).Item("Rate").ToString) <> 0 Then txt_Rate_New.Text = dt.Rows(0).Item("Rate").ToString
                'If Val(dt.Rows(0).Item("Rate_Old").ToString) <> 0 Then txt_Rate_Old.Text = dt.Rows(0).Item("Rate_Old").ToString
                'If Val(dt.Rows(0).Item("Rate_Scrap").ToString) <> 0 Then txt_Rate_Scrap.Text = dt.Rows(0).Item("Rate_Scrap").ToString

                If IsDBNull(dt.Rows(0).Item("Item_Image1")) = False Then
                    Dim imageData As Byte() = DirectCast(dt.Rows(0).Item("Item_Image1"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                PictureBox1.Image = Image.FromStream(ms)
                                'PictureBox1.BackgroundImage = Image.FromStream(ms)

                            End If
                        End Using
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Item_Image2")) = False Then
                    Dim imageData As Byte() = DirectCast(dt.Rows(0).Item("Item_Image2"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                PictureBox2.Image = Image.FromStream(ms)
                                'PictureBox1.BackgroundImage = Image.FromStream(ms)

                            End If
                        End Using
                    End If
                End If


                da2 = New SqlClient.SqlDataAdapter("select a.* ,b.Brand_Name  from Stores_item_Details a INNER JOIN Brand_Head b ON  b.Brand_IdNo = a.Brand_Idno  where a.Item_IdNo = " & Val(idno) & " Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Brand_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Rate_Old").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Rate_Scrap").ToString), "########0.00")



                    Next i

                End If


            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

        End Try



    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Sur As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Master_Stores_Item_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Master_Stores_Item_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to delete?", "FOR DELETING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from Stores_Item_AlaisHead where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stores_item_head where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stores_item_Details where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            trans.Commit()
            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()

    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next


        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        '  .Rows.Add()
                        If .CurrentCell.RowIndex = .RowCount - 1 Then

                            btnSave.Focus()
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            btnSave.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            txt_Name.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                        End If

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                    End If

                    Return True

                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function
    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        'con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        'con.Open()

        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable

        'da = New SqlClient.SqlDataAdapter("select a.item_idno, a.item_name, b.DEPARTMENT_name, a.drawing_no from Stores_item_head a, DEPARTMENT_head b where a.item_idno <> 0 and a.Department_Idno <> 0 Order by a.Item_IdNo", con)
        'dt = New DataTable
        'da.Fill(dt)

        'dgv_Filter.Columns.Clear()
        ''dgv_Filter.DataSource = dt
        ''dgv_Filter.RowHeadersVisible = False

        ''dgv_Filter.Columns(0).HeaderText = "IDNO"
        ''dgv_Filter.Columns(1).HeaderText = "ITEM NAME"
        ''dgv_Filter.Columns(2).HeaderText = "DEPARTMENT"
        ''dgv_Filter.Columns(3).HeaderText = "DRAWING NO"

        ''dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ''dgv_Filter.Columns(0).FillWeight = 40
        ''dgv_Filter.Columns(1).FillWeight = 210
        ''dgv_Filter.Columns(2).FillWeight = 80
        ''dgv_Filter.Columns(3).FillWeight = 50

        'pnl_Back.Enabled = False
        'grp_Filter.Visible = True

        'dgv_Filter.BringToFront()
        'dgv_Filter.Focus()

        'dgv_Filter.Columns.Clear()

        'da.Dispose()


        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        da = New SqlClient.SqlDataAdapter("select rack_no  from Rack_Head order by rack_no", con)
        da.Fill(dt1)
        cbo_Filter_RackNo.DataSource = dt1
        cbo_Filter_RackNo.DisplayMember = "rack_no"

        cbo_Filter_RackNo.Text = ""
        cbo_Filter_RackNo.SelectedIndex = -1

        da = New SqlClient.SqlDataAdapter("select brand_name  from brand_head order by brand_name", con)
        da.Fill(dt2)
        cbo_Filter_Brand.DataSource = dt2
        cbo_Filter_Brand.DisplayMember = "brand_name"

        cbo_Filter_Brand.Text = ""
        cbo_Filter_Brand.SelectedIndex = -1


        da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
        da.Fill(dt3)
        cbo_Filter_Department.DataSource = dt3
        cbo_Filter_Department.DisplayMember = "Department_Name"

        cbo_Filter_Department.Text = ""
        cbo_Filter_Department.SelectedIndex = -1

        dt1.Clear()
        dt2.Clear()
        dt3.Clear()

        dgv_Filter.Rows.Clear()



        grp_Filter.Visible = True
        pnl_Back.Enabled = False
        If cbo_Filter_Department.Enabled And cbo_Filter_Department.Visible Then cbo_Filter_Department.Focus()



    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(item_idno) from Stores_item_head where item_idno <> 0"
            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select max(item_idno) from Stores_item_head where item_idno <> 0", con)
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(item_idno) from Stores_item_head where item_idno > " & Str(Val(lbl_IdNo.Text)) & " and item_idno <> 0", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(item_idno) from Stores_item_head where item_idno < " & Str(Val(lbl_IdNo.Text)) & " and item_idno <> 0"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If
            dr.Close()
            If Val(movid) <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim newid As Integer = 0

        clear()

        New_Entry = True

        lbl_IdNo.ForeColor = Color.Red
        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Stores_item_head", "item_idno", "")

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select item_displayname from Stores_Item_AlaisHead order by item_idno", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "item_displayname"

        grp_Open.Visible = True
        pnl_Back.Enabled = False
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt2 As New DataTable
        Dim nr As Long = 0
        Dim unt_id As Integer = 0
        Dim Dep_ID As Integer = 0
        Dim recnt_id As Integer = 0
        Dim rewth_id As Integer = 0
        Dim Dep_IDno As Integer = 0
        Dim ItmName As String = ""
        Dim ItmAlsName As String = ""
        Dim Sur As String = ""
        Dim brnd_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim ItemGrp_IDno As Integer = 0
        Dim MotTyp_id As Integer = 0

        Dim Rack_Id As Integer = 0

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Master_Stores_Item_Creation, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1007" Then
            If Trim(txt_Code.Text) = "" Then
                MessageBox.Show("Invalid Itemcode Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_Code.Enabled Then txt_Code.Focus()
                Exit Sub
            End If
        End If
        If Common_Procedures.settings.CustomerCode <> "1391" Then
            If Trim(txt_DrawingNo.Text) = "" Then
                MessageBox.Show("Invalid DrawingNo Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_DrawingNo.Enabled Then txt_DrawingNo.Focus()
                Exit Sub
            End If

        End If




        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Brand Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)

                        End If
                        Exit Sub
                    End If



                    If Val(dgv_Details.Rows(i).Cells(2).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub

                    End If

                End If

            Next
        End With

        dt.Clear()

        Dep_ID = Common_Procedures.Department_NameToIdNo(con, cbo_Department.Text)

        unt_id = Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text)


        recnt_id = Common_Procedures.reedcount_NameToIdNo(con, cbo_ReedCount.Text)

        rewth_id = Common_Procedures.reedwidth_NameToIdNo(con, cbo_ReedWidth.Text)

        ItemGrp_IDno = Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroup.Text)

        Rack_Id = Common_Procedures.Rack_NoToIdNo(con, cbo_RackNo.Text)

        MotTyp_id = Common_Procedures.Motion_Type_NameToIdNo(con, cbo_motion_type.Text)

        If Val(Dep_ID) = 0 Then
            MessageBox.Show("Invalid Department", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Department.Enabled Then cbo_Department.Focus()
            Exit Sub
        End If
        If Val(unt_id) = 0 Then
            MessageBox.Show("Invalid Unit", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        ItmName = Trim(txt_Name.Text)
        If Trim(txt_Code.Text) <> "" Then
            ItmName = Trim(ItmName) & " - " & Trim(txt_Code.Text)
        End If
        If Trim(txt_ItemType.Text) <> "" Then
            ItmName = Trim(ItmName) & " - " & Trim(txt_ItemType.Text)
        End If
        If Trim(cbo_ReedCount.Text) <> "" Then
            ItmName = Trim(ItmName) & " - " & Trim(cbo_ReedCount.Text)
        End If
        If Trim(cbo_ReedWidth.Text) <> "" Then
            ItmName = Trim(ItmName) & " - " & Trim(cbo_ReedWidth.Text)
        End If

        Sur = Common_Procedures.Remove_NonCharacters(ItmName)

        If Common_Procedures.Check_Duplicate_Stores_Item(con, Val(lbl_IdNo.Text), Sur) = True Then
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If


        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr


            Dim ms As New MemoryStream()
            If IsNothing(PictureBox1.Image) = False Then
                Dim bitmp As New Bitmap(PictureBox1.Image)
                bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            'If IsNothing(PictureBox1.BackgroundImage) = False Then
            '    Dim bitmp As New Bitmap(PictureBox1.BackgroundImage)
            '    bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
            'End If
            Dim data As Byte() = ms.GetBuffer()
            Dim p As New SqlClient.SqlParameter("@photo1", SqlDbType.Image)
            p.Value = data
            cmd.Parameters.Add(p)
            ms.Dispose()

            Dim ms1 As New MemoryStream()
            If IsNothing(PictureBox2.Image) = False Then
                Dim bitmp1 As New Bitmap(PictureBox2.Image)
                bitmp1.Save(ms1, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            'If IsNothing(PictureBox1.BackgroundImage) = False Then
            '    Dim bitmp As New Bitmap(PictureBox1.BackgroundImage)
            '    bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
            'End If
            Dim data1 As Byte() = ms1.GetBuffer()
            Dim p1 As New SqlClient.SqlParameter("@photo2", SqlDbType.Image)
            p1.Value = data1
            cmd.Parameters.Add(p1)
            ms1.Dispose()


            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Stores_item_head", "Item_idno", "", tr)

                cmd.CommandText = "Insert into Stores_item_head(Item_IdNo, Item_Name, Sur_Name,  Item_DisplayName,  Item_Code, Item_Type, Department_IDNo, Drawing_No, ReedCount_IdNo, ReedWidth_IdNo, Unit_IdNo, Minimum_Stock, ReOrder_Quantity, Tax_Percentage,ItemGroup_Idno,Item_Image1,Item_Image2 , Rack_IdNo, Catelog_pageno,Motion_Type_IdNo) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(ItmName) & "', '" & Trim(Sur) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_Code.Text) & "', '" & Trim(txt_ItemType.Text) & "', " & Str(Val(Dep_ID)) & ", '" & Trim(txt_DrawingNo.Text) & "', " & Str(Val(recnt_id)) & ", " & Str(Val(rewth_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(txt_MinimumStock.Text)) & ", " & Str(Val(txt_ReOrderQty.Text)) & ", " & Str(Val(txt_TaxPerc.Text)) & "," & Val(ItemGrp_IDno) & " ,@photo1 ,@photo2 , " & Str(Val(Rack_Id)) & ", '" & Trim(txt_catelog_pageno.Text) & "', " & Str(Val(MotTyp_id)) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "update Stores_item_head set Item_Name = '" & Trim(ItmName) & "', Sur_Name = '" & Trim(Sur) & "', Item_DisplayName = '" & Trim(txt_Name.Text) & "', Item_Code = '" & Trim(txt_Code.Text) & "', Item_Type = '" & Trim(txt_ItemType.Text) & "', Department_IDNo = " & Str(Val(Dep_ID)) & ", Drawing_No = '" & Trim(txt_DrawingNo.Text) & "', ReedCount_IdNo = " & Str(Val(recnt_id)) & ", ReedWidth_IdNo = " & Str(Val(rewth_id)) & ", Unit_IdNo = " & Str(Val(unt_id)) & ", Minimum_Stock = " & Str(Val(txt_MinimumStock.Text)) & ", ReOrder_Quantity = " & Str(Val(txt_ReOrderQty.Text)) & ", Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & " ,ItemGroup_Idno=" & Val(ItemGrp_IDno) & " ,Item_Image1 = @photo1 ,Item_Image2 = @photo2 , Rack_IdNo = " & Str(Val(Rack_Id)) & " , Catelog_pageno ='" & Trim(txt_catelog_pageno.Text) & "', Motion_Type_IdNo = " & Str(Val(MotTyp_id)) & "  Where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Stores_Item_AlaisHead where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_item_Details Where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then
                        Sno = Sno + 1
                        brnd_ID = Common_Procedures.Brand_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Stores_item_Details(Item_IdNo, Item_Name, Sur_Name ,Sl_No, Brand_IdNo, Rate, Rate_Old, Rate_Scrap  ) Values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(ItmName) & "', '" & Trim(Sur) & "'  ," & Str(Val(Sno)) & ", " & Str(Val(brnd_ID)) & " , " & Str(Val(.Rows(i).Cells(2).Value)) & " , " & Val(.Rows(i).Cells(3).Value) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            cmd.CommandText = "Insert into Stores_Item_AlaisHead(Item_IdNo, Sl_No, Item_DisplayName, Sur_Name, Department_IdNo, Drawing_No ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 1, '" & Trim(ItmName) & "', '" & Trim(Sur) & "', " & Str(Val(Dep_ID)) & ", '" & Trim(txt_DrawingNo.Text) & "' )"
            cmd.ExecuteNonQuery()

            If Trim(txt_Code.Text) <> "" Then

                ItmAlsName = Trim(txt_Code.Text)
                If Trim(txt_Name.Text) <> "" Then
                    ItmAlsName = Trim(ItmAlsName) & " - " & Trim(txt_Name.Text)
                End If
                If Trim(txt_ItemType.Text) <> "" Then
                    ItmAlsName = Trim(ItmAlsName) & " - " & Trim(txt_ItemType.Text)
                End If
                If Trim(cbo_ReedCount.Text) <> "" Then
                    ItmAlsName = Trim(ItmAlsName) & " - " & Trim(cbo_ReedCount.Text)
                End If
                If Trim(cbo_ReedWidth.Text) <> "" Then
                    ItmAlsName = Trim(ItmAlsName) & " - " & Trim(cbo_ReedWidth.Text)
                End If

                Sur = Common_Procedures.Remove_NonCharacters(ItmAlsName)

                cmd.CommandText = "Insert into Stores_Item_AlaisHead(Item_IdNo, Sl_No, Item_DisplayName, Sur_Name, Department_IdNo, Drawing_No ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 2, '" & Trim(ItmAlsName) & "', '" & Trim(Sur) & "', " & Str(Val(Dep_ID)) & ", '" & Trim(txt_DrawingNo.Text) & "' )"
                cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(ItmName)
            Common_Procedures.Master_Return.Master_Type = "ITEM"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "duplicate_itemhead_name") > 0 Or InStr(1, Trim(LCase(ex.Message)), "duplicate_Stores_Item_AlaisHead_name") > 0 Then
                MessageBox.Show("Duplicate Item Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            ElseIf InStr(1, Trim(LCase(ex.Message)), "duplicate_itemhead_drawingno") > 0 Or InStr(1, Trim(LCase(ex.Message)), "duplicate_Stores_Item_AlaisHead_drawingno") > 0 Then
                MessageBox.Show("Duplicate Drawing No. to this department", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub Item_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Department.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DEPARTMENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Department.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ReedCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ReedCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ReedWidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "REEDWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ReedWidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_motion_type.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MOTION" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_motion_type.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Item_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                Call btn_CloseFilter_Click(sender, e)
                Exit Sub

            ElseIf grp_Open.Visible Then
                Call btnClose_Click(sender, e)
                Exit Sub

            Else
                Me.Close()

            End If
        End If
    End Sub

    Private Sub Item_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Department_name from Department_head where Department_IdNo <> 1 order by Department_name", con)
        da.Fill(dt2)
        cbo_Department.DataSource = dt2
        cbo_Department.DisplayMember = "Department_name"

        da = New SqlClient.SqlDataAdapter("select Count_name from Count_head order by Count_name", con)
        da.Fill(dt3)
        cbo_ReedCount.DataSource = dt3
        cbo_ReedCount.DisplayMember = "Count_name"

        da = New SqlClient.SqlDataAdapter("select ReedWidth_name from ReedWidth_head order by ReedWidth_name", con)
        da.Fill(dt4)
        cbo_ReedWidth.DataSource = dt4
        cbo_ReedWidth.DisplayMember = "ReedWidth_name"

        da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)
        da.Fill(dt5)
        cbo_Unit.DataSource = dt5
        cbo_Unit.DisplayMember = "unit_name"

        da = New SqlClient.SqlDataAdapter("select ItemGroup_Name from ItemGroup_Head order by ItemGroup_Name", con)
        da.Fill(dt6)
        cbo_ItemGroup.DataSource = dt6
        cbo_ItemGroup.DisplayMember = "ItemGroup_Name"

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Department.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ItemType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ReedCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ReedWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Code.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DrawingNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MinimumStock.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReOrderQty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Brand.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RackNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_catelog_pageno.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_motion_type.GotFocus, AddressOf ControlGotFocus



        AddHandler cbo_Department.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ItemType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ReedCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ReedWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Code.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DrawingNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MinimumStock.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReOrderQty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Brand.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RackNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_catelog_pageno.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_motion_type.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Code.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ItemType.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_DrawingNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_MinimumStock.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReOrderQty.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Code.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DrawingNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ItemType.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_MinimumStock.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReOrderQty.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_Filter_Brand.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Department.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_RackNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_Brand.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Department.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_RackNo.LostFocus, AddressOf ControlLostFocus

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) - 50
        grp_Open.Top = (Me.Height - grp_Open.Height) - 50
        grp_Open.BringToFront()

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 25
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 55
        grp_Filter.BringToFront()


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
        '    cbo_ItemGroup.Width = 180
        '    lbl_RackNo_Caption.Visible = True
        '    cbo_RackNo.Visible = True
        'Else
        '    lbl_RackNo_Caption.Visible = False
        '    cbo_RackNo.Visible = False
        'End If


        new_record()

    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "Stores_Item_AlaisHead", "Item_Displayname", "", "(Item_idno = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Stores_Item_AlaisHead", "Item_Displayname", "", "(Item_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Find_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim itemalais_id As Integer = 0
        Try
            itemalais_id = Common_Procedures.itemalais_NameToIdNo(con, cbo_Open.Text)

            If itemalais_id <> 0 Then
                move_record(itemalais_id)
                btnClose_Click(sender, e)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR FINDING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        pnl_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub cbo_Department_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Department.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Department, txt_ItemType, cbo_motion_type, "Department_HEAD", "Department_name", "(Department_IdNo <> 1)", "( Department_IdNo = 0 )")
    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, txt_DrawingNo, cbo_ReedCount, "Unit_HEAD", "Unit_name", "", "")
    End Sub

    Private Sub txt_TaxPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyDown
        If (e.KeyValue = 40) Then
            With dgv_Details

                ' If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()

                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

                ' End If
            End With

        End If
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                '   If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True


                '  End If
            End With


        End If
    End Sub

    Private Sub btn_OpenFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OpenFilter.Click
        Dim movid As Integer = 0

        Try
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)

            If Val(movid) <> 0 Then
                move_record(movid)
                pnl_Back.Enabled = True
                grp_Filter.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        pnl_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgv_Filter.DoubleClick
        Call btn_OpenFilter_Click(sender, e)
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, cbo_ReedCount, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        Me.ActiveControl.BackColor = Color.PaleGreen
        Me.ActiveControl.ForeColor = Color.Blue

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Prec_ActCtrl = Me.ActiveControl
    End Sub
    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If

    End Sub

    Private Sub txt_MinimumStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinimumStock.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            txt_ReOrderQty.Focus()
        End If

    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub cbo_ReedCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ReedCount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ReedCount, cbo_Unit, cbo_ReedWidth, "Count_head", "Count_name", "", "(Count_idno = 0)")
    End Sub

    Private Sub cbo_ReedCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ReedCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ReedCount, cbo_ReedWidth, "Count_head", "Count_name", "", "(Count_idno = 0)")
    End Sub

    Private Sub cbo_ReedWidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ReedWidth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ReedWidth, cbo_ReedCount, txt_catelog_pageno, "ReedWidth_Head", "ReedWidth_name", "", "(ReedWidth_idno = 0)")
    End Sub

    Private Sub cbo_ReedWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ReedWidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ReedWidth, txt_catelog_pageno, "ReedWidth_Head", "ReedWidth_name", "", "(ReedWidth_idno = 0)")
    End Sub

    Private Sub cbo_Department_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Department.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Department, cbo_motion_type, "Department_HEAD", "Department_name", "( Department_IdNo <> 1 )", "( Department_IdNo = 0 )")
    End Sub

    Private Sub cbo_Department_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Department.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Stores_Department_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Department.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_ReedCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ReedCount.KeyUp
        'If e.Control = False And e.KeyValue = 17 Then
        '    Dim f As New Stores_ReedCount_Creation
        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_ReedCount.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""
        '    f.MdiParent = MDIParent1
        '    f.Show()
        'End If
    End Sub

    Private Sub cbo_ReedWidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ReedWidth.KeyUp
        'If e.Control = False And e.KeyValue = 17 Then
        '    Dim f As New Stores_ReedWidth_Creation
        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_ReedWidth.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""
        '    f.MdiParent = MDIParent1
        '    f.Show()
        'End If
    End Sub

    Private Sub cbo_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_DrawingNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DrawingNo.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txt_ItemType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ItemType.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Code_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Code.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Rate_New_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Rate_Old_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Rate_Scrap_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_Scrap_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If

    End Sub
    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Brand.Visible = False Or Val(cbo_Brand.Tag) <> e.RowIndex Then

                    cbo_Brand.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Brand_Name from Brand_Head  order by Brand_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Brand.DataSource = Dt1
                    cbo_Brand.DisplayMember = "Brand_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Brand.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Brand.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Brand.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Brand.Height = rect.Height  ' rect.Height
                    cbo_Brand.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Brand.Tag = Val(e.RowIndex)
                    cbo_Brand.Visible = True

                    cbo_Brand.BringToFront()
                    cbo_Brand.Focus()


                End If


            Else

                cbo_Brand.Visible = False


            End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            'If .CurrentCell.ColumnIndex = 9 Then
            '    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
            '        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
            '    End If
            'End If
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    'Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
    '    Dim i As Integer
    '    Dim vTotMtrs As Single
    '    Dim Chees_Wgt_Calculation As Double = 0
    '    Dim no_of_Chees As Integer = 0

    '    On Error Resume Next
    '    With dgv_Details
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
    '            End If

    '        End If
    '    End With
    'End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
        End With
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress

        With dgv_Details
            'If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(12).Value) <> 0 Then
            '    e.Handled = True
            'End If

            'If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 12 Or .CurrentCell.ColumnIndex = 13 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
            'End If
        End With
    End Sub
    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub


    Private Sub cbo_Brand_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Brand.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Brand_Head", "Brand_Name", "", "(Brand_idno = 0)")

    End Sub

    Private Sub cbo_Brand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Brand.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Brand, Nothing, Nothing, "Brand_Head", "Brand_Name", "", "(Brand_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                txt_Name.Focus()
            End If

            If (e.KeyValue = 40 And cbo_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    btnSave.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Brand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Brand.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Brand, Nothing, "Brand_Head", "Brand_Name", "", "(Brand_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Brand.Text)
                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                        btnSave.Focus()
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If

                End If
            End With
        End If
    End Sub

    Private Sub cbo_Brand_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Brand.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Brand_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Brand.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Brand_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Brand.TextChanged
        Try
            If cbo_Brand.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Brand.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Brand.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_ItemGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ItemGroup_Head", "ItemGroup_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemGroup, cbo_motion_type, Nothing, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")

        If e.KeyCode = 40 Then

            If cbo_RackNo.Visible Then
                cbo_RackNo.Focus()
            Else
                txt_DrawingNo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, Nothing, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then


            If cbo_RackNo.Visible Then
                cbo_RackNo.Focus()
            Else
                txt_DrawingNo.Focus()
                End If
            End If

    End Sub

    Private Sub cbo_ItemGroup_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New ItemGroup_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemGroup.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_BrowsePhoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BrowsePhoto.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
            'PictureBox1.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_EnLargePhoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnLargePhoto.Click

        If IsNothing(PictureBox1.Image) = False Then

            EnlargePicture.Text = "IMAGE   -   " & txt_Name.Text
            EnlargePicture.PictureBox2.ClientSize = PictureBox1.Image.Size
            EnlargePicture.PictureBox2.Image = CType(PictureBox1.Image.Clone, Image)
            EnlargePicture.ShowDialog()

        End If
    End Sub
    Private Sub btn_BrowsePhoto2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BrowsePhoto2.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox2.Image = Image.FromFile(OpenFileDialog1.FileName)
            'PictureBox1.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_EnLargePhoto2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnLargePhoto2.Click

        If IsNothing(PictureBox2.Image) = False Then

            EnlargePicture.Text = "IMAGE   -   " & txt_Name.Text
            EnlargePicture.PictureBox2.ClientSize = PictureBox2.Image.Size
            EnlargePicture.PictureBox2.Image = CType(PictureBox2.Image.Clone, Image)
            EnlargePicture.ShowDialog()

        End If
    End Sub

    Private Sub cbo_RackNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_RackNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Rack_head", "Rack_No", "", "(Rack_idno = 0)")
    End Sub

    Private Sub cbo_RackNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_RackNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RackNo, cbo_ItemGroup, txt_DrawingNo, "Rack_head", "Rack_No", "", "(Rack_idno = 0)")
    End Sub

    Private Sub cbo_RackNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_RackNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RackNo, txt_DrawingNo, "Rack_head", "Rack_No", "", "(Rack_idno = 0)")
    End Sub

    Private Sub cbo_RackNo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_RackNo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New RackNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RackNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_DrawingNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_DrawingNo.KeyDown
        If e.KeyCode = 38 Then
            If cbo_RackNo.Visible Then
                cbo_RackNo.Focus()
            Else
                cbo_ItemGroup.Focus()

            End If
        End If
        If e.KeyCode = 40 Then
            cbo_Unit.Focus()
        End If
    End Sub

    Private Sub txt_catelog_pageno_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_catelog_pageno.KeyDown
        If e.KeyCode = 40 Then
            txt_MinimumStock.Focus()
        End If
        If e.KeyCode = 38 Then
            cbo_ReedWidth.Focus()
        End If
    End Sub

    Private Sub txt_catelog_pageno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_catelog_pageno.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_MinimumStock.Focus()
        End If
    End Sub

    Private Sub cbo_motion_type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_motion_type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_motion_type, cbo_Department, cbo_ItemGroup, "Motion_Type_Head", "Motion_Type_Name", "", "(Motion_Type_IdNo = 0)")
    End Sub


    Private Sub cbo_motion_type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_motion_type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_motion_type, cbo_ItemGroup, "Motion_Type_Head", "Motion_Type_Name", "", "(Motion_Type_IdNo = 0)")
    End Sub

    Private Sub cbo_motion_type_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_motion_type.KeyUp
        'If e.Control = False And e.KeyValue = 17 Then
        '    Dim f As New Motion_Type_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_motion_type.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()
        'End If
    End Sub

    Private Sub txt_MinimumStock_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_MinimumStock.KeyDown
        If e.KeyCode = 38 Then
            txt_catelog_pageno.Focus()
        End If

        If e.KeyCode = 40 Then
            txt_ReOrderQty.Focus()
        End If
    End Sub

    Private Sub cbo_motion_type_GotFocus(sender As Object, e As EventArgs) Handles cbo_motion_type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Motion_Type_Head", "Motion_Type_Name", "", "(Motion_Type_IdNo = 0)")
    End Sub
    Private Sub btn_Filter_Close_Click(sender As Object, e As EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_Filter_Show_Click(sender As Object, e As EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Depart_Idno As Integer, Brand_Idno As Integer, Rack_Idno As Integer
        Dim Condt As String = ""
        Dim Condt2 As String = ""
        Dim MainCondt As String = ""

        dgv_Filter.Rows.Clear()

        Try
            Condt = ""
            Depart_Idno = 0
            Rack_Idno = 0
            Brand_Idno = 0

            If Trim(cbo_Filter_Department.Text) <> "" Then
                Depart_Idno = Common_Procedures.Department_NameToIdNo(con, cbo_Filter_Department.Text)
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.department_Idno = " & Str(Val(Depart_Idno))
            End If

            If Trim(cbo_Filter_RackNo.Text) <> "" Then
                Rack_Idno = Common_Procedures.Rack_NoToIdNo(con, cbo_Filter_RackNo.Text)
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Rack_IdNo = " & Str(Val(Rack_Idno))
            End If

            If Trim(cbo_Filter_Brand.Text) <> "" Then
                Brand_Idno = Common_Procedures.Brand_NameToIdNo(con, cbo_Filter_Brand.Text)
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " d.brand_Idno = " & Str(Val(Brand_Idno))
            End If

            If Trim(Condt) <> "" Then

                Condt = Condt & IIf(Trim(Condt) <> "", " ", "")
            Else
                Condt = "a.item_Idno<>0"
            End If


            da = New SqlClient.SqlDataAdapter(" Select  A.item_Idno,a.Item_Name ,b.department_Name,c.Brand_Name ,e.Rack_No ,a.drawing_no  From Stores_Item_Head a inner Join Department_Head b on a.department_Idno =b.department_Idno inner Join Stores_Item_Details d on d.item_Idno=a.item_Idno   inner Join brand_head c on  d.brand_Idno = c.brand_idno     inner Join  Rack_Head e on a.Rack_IdNo= e.Rack_IdNo where " & Condt & " order by  item_idno asc ", con)

            'da = New SqlClient.SqlDataAdapter(" Select  A.item_Idno,a.Item_Name ,b.department_Name,c.Brand_Name ,e.Rack_No ,a.drawing_no  From Stores_Item_Head a inner Join Department_Head b on a.department_Idno =b.department_Idno inner Join Stores_Item_Details d on d.item_Idno=a.item_Idno   inner Join brand_head c on  d.brand_Idno = c.brand_idno     inner Join  Rack_Head e on a.Rack_IdNo= e.Rack_IdNo where " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " ", con)

            ' da = New SqlClient.SqlDataAdapter(" Select  A.item_Idno,a.Item_Name ,b.department_Name,c.Brand_Name ,e.Rack_No ,a.drawing_no  From Stores_Item_Head a inner Join Department_Head b on a.department_Idno =b.department_Idno inner Join Stores_Item_Details d on d.item_Idno=a.item_Idno   inner Join brand_head c on  d.brand_Idno = c.brand_idno     inner Join  Rack_Head e on a.Rack_IdNo= e.Rack_IdNo where b.department_idno=" & Str(Val(Depart_Idno)) & " AND c.Brand_idno=" & Str(Val(Brand_Idno)) & " AND e.Rack_idno=" & Str(Val(Rack_Idno)) & "  ", con)
            dt1 = New DataTable
            da.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    n = dgv_Filter.Rows.Add()

                    dgv_Filter.Rows(n).Cells(0).Value = dt1.Rows(i).Item("item_Idno").ToString
                    dgv_Filter.Rows(n).Cells(1).Value = dt1.Rows(i).Item("Item_Name").ToString
                    dgv_Filter.Rows(n).Cells(2).Value = dt1.Rows(i).Item("department_Name").ToString
                    dgv_Filter.Rows(n).Cells(3).Value = dt1.Rows(i).Item("drawing_no").ToString

                Next i
            End If
            dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
        da.Dispose()

        If dgv_Filter.Visible And dgv_Filter.Enabled Then dgv_Filter.Focus()

        End Try
    End Sub
    Private Sub cbo_Filter_Department_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_Department.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Department_Head", "Department_Name", "", "(Department_Idno = 0)")
    End Sub
    Private Sub cbo_Filter_Department_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Filter_Department.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Department, Nothing, cbo_Filter_RackNo, "Department_Head", "Department_Name", "", "(Department_Idno = 0)")
    End Sub
    Private Sub cbo_Filter_Department_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Filter_Department.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Department, cbo_Filter_RackNo, "Department_Head", "Department_Name", "", "(Department_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_RackNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_RackNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Rack_head", "Rack_No", "", "(Rack_idno = 0)")
    End Sub
    Private Sub cbo_Filter_RackNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Filter_RackNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_RackNo, cbo_Filter_Department, cbo_Filter_Brand, "Rack_head", "Rack_No", "", "(Rack_idno = 0)")
    End Sub
    Private Sub cbo_Filter_RackNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Filter_RackNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_RackNo, cbo_Filter_Brand, "Rack_head", "Rack_No", "", "(Rack_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Brand_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Brand.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Brand_Head", "Brand_Name", "", "(Brand_idno = 0)")

    End Sub
    Private Sub cbo_Filter_Brand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Brand.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Brand, cbo_Filter_RackNo, btn_Filter_Show, "Brand_Head", "Brand_Name", "", "(Brand_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Brand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Brand.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Brand, btn_Filter_Show, "Brand_Head", "Brand_Name", "", "(Brand_idno = 0)")

    End Sub

    Private Sub cbo_Brand_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Brand.SelectedIndexChanged

    End Sub

    Private Sub Btn_Close_Img_Click(sender As Object, e As EventArgs) Handles Btn_Close_Img1.Click

        PictureBox1.Image = Nothing

    End Sub

    Private Sub Btn_Close_Img2_Click(sender As Object, e As EventArgs) Handles Btn_Close_Img2.Click

        PictureBox2.Image = Nothing

    End Sub


End Class