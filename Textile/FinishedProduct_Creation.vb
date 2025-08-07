Imports System.IO

Public Class FinishedProduct_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private Verified_STS As Integer = 0

    Private Sub clear()

        'Insert_Entry = False
        grp_Back.Enabled = True
        grp_Filter.Visible = False
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        lbl_DisplaySlNo.Text = ""
        lbl_DisplaySlNo.ForeColor = Color.Black
        txt_Code.Text = ""
        txt_CostRate.Text = ""
        txt_TaxPerc.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        txt_Meter_Qty.Text = ""
        txt_Weight_Piece.Text = ""
        txt_Width.Text = ""
        cbo_Company.Text = ""
        txt_MinimumStock.Text = ""
        cbo_ItemGroup.Text = ""
        cbo_Unit.Text = ""
        txt_Name.Text = ""
        cbo_Product_Sales_Name.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()

        PictureBox1.BackgroundImage = Nothing

        cbo_Company.Visible = False
        cbo_Product_Sales_Name.Visible = False
        chk_Verified_Status.Checked = False
        chk_shirt.Checked = False
        Grid_DeSelect()

        New_Entry = False

        grp_Open.Visible = False
        cbo_Company.Text = ""
        cbo_Company.Visible = False
        cbo_Product_Sales_Name.Text = ""
        cbo_Product_Sales_Name.Visible = False
        New_Entry = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        Me.ActiveControl.BackColor = Color.Lime   'Color.FromArgb(128, 128, 255)  ' Color.LightBlue  ' Color.lime
        Me.ActiveControl.ForeColor = Color.Blue

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Company.Name Then
            cbo_Company.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Product_Sales_Name.Name Then
            cbo_Product_Sales_Name.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If

        'Me.ActiveControl.BackColor = Color.White
        'Me.ActiveControl.ForeColor = Color.Black
    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim slno, n As Integer

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da = New SqlClient.SqlDataAdapter("select a.*, b.ItemGroup_Name, c.Unit_Name from Processed_Item_Head a LEFT OUTER JOIN ItemGroup_Head b ON a.Processed_ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo where a.Processed_Item_IdNo = " & Str(Val(idno)) & " and Processed_Item_Type = 'FP'", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0).Item("Processed_Item_IdNo").ToString) = False Then
                    lbl_IdNo.Text = dt.Rows(0).Item("Processed_Item_IdNo").ToString
                    lbl_DisplaySlNo.Text = dt.Rows(0).Item("Processed_Item_DisplaySlNo").ToString
                    txt_Name.Text = dt.Rows(0).Item("Processed_Item_Nm").ToString
                    txt_Code.Text = dt.Rows(0).Item("Processed_Item_Code").ToString

                    cbo_ItemGroup.Text = dt.Rows(0).Item("ItemGroup_Name").ToString
                    cbo_Unit.Text = dt.Rows(0).Item("Unit_Name").ToString
                    If Val(dt.Rows(0).Item("Meter_Qty").ToString) <> 0 Then
                        txt_Meter_Qty.Text = Format(Val(dt.Rows(0).Item("Meter_Qty").ToString), "#######0.00")
                    End If
                    ' txt_Meter_Qty.Text = dt.Rows(0).Item("Meter_Qty").ToString
                    If Val(dt.Rows(0).Item("Weight_Piece").ToString) <> 0 Then
                        txt_Weight_Piece.Text = dt.Rows(0).Item("Weight_Piece").ToString
                    End If
                    If Val(dt.Rows(0).Item("Width").ToString) <> 0 Then
                        txt_Width.Text = dt.Rows(0).Item("Width").ToString
                    End If
                    If Val(dt.Rows(0).Item("Minimum_Stock").ToString) <> 0 Then
                        txt_MinimumStock.Text = dt.Rows(0).Item("Minimum_Stock").ToString
                    End If
                    If Val(dt.Rows(0).Item("Tax_Percentage").ToString) <> 0 Then
                        txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
                    End If
                    If Val(dt.Rows(0).Item("Sale_TaxRate").ToString) <> 0 Then
                        txt_TaxRate.Text = dt.Rows(0).Item("Sale_TaxRate").ToString
                    End If
                    If Val(dt.Rows(0).Item("Sales_Rate").ToString) <> 0 Then
                        txt_Rate.Text = dt.Rows(0).Item("Sales_Rate").ToString
                    End If
                    If Val(dt.Rows(0).Item("Cost_Rate").ToString) <> 0 Then
                        txt_CostRate.Text = dt.Rows(0).Item("Cost_Rate").ToString

                    End If
                    If Val(dt.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True
                    If Val(dt.Rows(0).Item("Chk_sts_shirt").ToString) = 1 Then chk_shirt.Checked = True


                    If IsDBNull(dt.Rows(0).Item("Processed_Item_Image")) = False Then
                        Dim imageData As Byte() = DirectCast(dt.Rows(0).Item("Processed_Item_Image"), Byte())
                        If Not imageData Is Nothing Then
                            Using ms As New MemoryStream(imageData, 0, imageData.Length)
                                ms.Write(imageData, 0, imageData.Length)
                                If imageData.Length > 0 Then

                                    PictureBox1.BackgroundImage = Image.FromStream(ms)

                                    'If IO.File.Exists("c:\tmpimg.png") Then IO.File.Delete("c:\tmpimg.png")
                                    'Using img As Image = Image.FromStream(ms)
                                    '    img.Save("c:\tmpimg.png", Imaging.ImageFormat.Png)
                                    'End Using
                                    'PictureBox1.BackgroundImage = Image.FromFile("c:\tmpimg.png")


                                    'Using img As Image = Image.FromStream(ms)
                                    '    PictureBox1.BackgroundImage = img
                                    'End Using

                                    'If IO.File.Exists("c:\tmpimg.png") Then IO.File.Delete("c:\tmpimg.png")

                                    'Using img As Image = Image.FromStream(ms)
                                    '    'img.Save("C:\Users\Jonathan\Desktop\e\tmp.png", Imaging.ImageFormat.Png)
                                    '    'Image.FromStream(ms).Save("c:\tmpimg.png", Imaging.ImageFormat.Png)
                                    'End Using


                                    'Image.FromStream(ms).Save("c:\tmpimg.png", Imaging.ImageFormat.Png)
                                    'PictureBox1.BackgroundImage = Image.FromFile("c:\tmpimg.png")

                                    'PictureBox1.BackgroundImage = Image.FromStream(ms)

                                    'OpenFileDialog1.FileName = ""
                                    'If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                                    '    PictureBox1.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
                                    'End If

                                End If
                            End Using
                        End If
                    End If

                    da = New SqlClient.SqlDataAdapter("select a.*, b.Company_ShortName,c.Processed_Item_SalesName from Processed_Item_SalesName_Details a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_Idno LEFT OUTER JOIN Processed_Item_SalesName_Head c ON a.Processed_Item_SalesIdNo = c.Processed_Item_SalesIdNo where a.Processed_Item_IdNo = " & Val(idno) & " ", con)
                    da.Fill(dt2)

                    dgv_Details.Rows.Clear()
                    slno = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_Details.Rows.Add()

                            slno = slno + 1
                            dgv_Details.Rows(n).Cells(0).Value = Val(slno)
                            dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Company_ShortName").ToString
                            dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Processed_Item_SalesName").ToString

                        Next i

                        For i = 0 To dgv_Details.RowCount - 1
                            dgv_Details.Rows(i).Cells(0).Value = Val(i) + 1
                        Next
                    End If
                    dt2.Clear()
                    dt2.Dispose()

                    dgv_Details.Rows.Add()

                End If
            End If
            dt.Clear()
            dt.Dispose()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        cbo_Company.Visible = False
        cbo_Product_Sales_Name.Visible = False

        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        '' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.FinishedProduct_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.FinishedProduct_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_Finished_Product_Creation, New_Entry, Me) = False Then Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Item_Processing_Details where Item_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from FinishedProduct_Order_Details where FinishedProduct_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from FinishedProduct_Invoice_Details where FinishedProduct_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from Ledger_ItemName_Details where Item_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from Processed_Item_Details where Finished_Product_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            cmd.Connection = con
            cmd.CommandText = "delete from Processed_Item_Head where Processed_Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select a.Processed_Item_IdNo, a.Processed_Item_Name, b.unit_name, a.Sale_TaxRate from Processed_Item_Head a, unit_head b where a.Processed_Item_Type = 'FP' and a.unit_idno = b.unit_idno Order by a.Processed_Item_IdNo", con)
        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt

        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "ITEM NAME"
        dgv_Filter.Columns(2).HeaderText = "UNIT"
        dgv_Filter.Columns(3).HeaderText = "SALES RATE"

        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 40
        dgv_Filter.Columns(1).FillWeight = 240
        dgv_Filter.Columns(2).FillWeight = 60
        dgv_Filter.Columns(3).FillWeight = 60

        grp_Back.Enabled = False
        grp_Filter.Visible = True

        dgv_Filter.BringToFront()
        dgv_Filter.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_Type = 'FP' ORDER BY Processed_Item_DisplaySlNo"
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_Type = 'FP' ORDER BY Processed_Item_DisplaySlNo desc"
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

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_DisplaySlNo > " & Val(lbl_DisplaySlNo.Text) & " and  Processed_Item_Type = 'FP' ORDER BY Processed_Item_DisplaySlNo"
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

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_DisplaySlNo < " & Val(lbl_DisplaySlNo.Text) & " and  Processed_Item_Type = 'FP' ORDER BY Processed_Item_DisplaySlNo desc"
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

    Public Sub new_record() Implements Interface_MDIActions.new_record

        clear()

        Try

            New_Entry = True

            lbl_IdNo.Text = Val(Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_IdNo", ""))
            lbl_IdNo.ForeColor = Color.Red

            lbl_DisplaySlNo.Text = Val(Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_DisplaySlNo", "(Processed_Item_Type= 'FP')"))
            lbl_DisplaySlNo.ForeColor = Color.Red

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_IdNo = 0 or Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
        da.Fill(dt)

        'cbo_Open.Items.Clear()

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Processed_Item_Name"

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '---
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim ProdName As String = ""
        Dim Sur As String = ""
        Dim Nr As Long = 0
        Dim ItmGrp_ID As Integer = 0
        Dim Unt_Id As Integer = 0
        Dim PSalNm_ID As Integer = 0
        Dim Cmp_ID As Integer = 0
        Dim SlNo As Integer = 0
        Dim Shirt_STS As Integer = 0

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.FinishedProduct_Creation, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_Finished_Product_Creation, New_Entry, Me) = False Then Exit Sub



        If grp_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))
        If Trim(Sur) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        ItmGrp_ID = Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroup.Text)
        If Val(ItmGrp_ID) = 0 Then
            MessageBox.Show("Invalid ItemGroup", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        If Trim(txt_Code.Text) = "" Then
            MessageBox.Show("Invalid Item Code", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Code.Enabled Then txt_Code.Focus()
            Exit Sub
        End If

        Unt_Id = Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text)
        If Val(Unt_Id) = 0 Then
            MessageBox.Show("Invalid Unit", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        With dgv_Details
            For i = 0 To .RowCount - 1
                If Trim(.Rows(i).Cells(1).Value) <> "" Or Trim(.Rows(i).Cells(2).Value) <> "" Then

                    Cmp_ID = Common_Procedures.Company_ShortNameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Val(Cmp_ID) = 0 Then
                        MessageBox.Show("Invalid Company Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                            dgv_Details.CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    PSalNm_ID = Common_Procedures.Processed_Item_SalesNameToIdNo(con, .Rows(i).Cells(2).Value)
                    If Val(PSalNm_ID) = 0 Then
                        MessageBox.Show("Invalid Product Sales Name.", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                            dgv_Details.CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If
            Next
        End With

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        Shirt_STS = 0
        If chk_shirt.Checked = True Then Shirt_STS = 1

        ProdName = Trim(txt_Code.Text) & "-" & Trim(txt_Name.Text)

        tr = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = tr

            Dim ms As New MemoryStream()
            If IsNothing(PictureBox1.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox1.BackgroundImage)
                bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                'PictureBox1.BackgroundImage.Save(ms, PictureBox1.BackgroundImage.RawFormat)
            End If
            Dim data As Byte() = ms.GetBuffer()
            Dim p As New SqlClient.SqlParameter("@photo", SqlDbType.Image)
            p.Value = data
            cmd.Parameters.Add(p)
            ms.Dispose()

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_IdNo", "", tr)

                lbl_DisplaySlNo.Text = Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_DisplaySlNo", "(Processed_Item_Type= 'FP')", tr)

                cmd.CommandText = "Insert into Processed_Item_Head ( Processed_Item_IdNo, Processed_Item_DisplaySlNo, Processed_Item_Type, Processed_Item_Name, Processed_Item_Nm, Sur_Name, Processed_Item_Code, Processed_ItemGroup_IdNo, Unit_IdNo, Tax_Percentage, Sale_TaxRate, Sales_Rate, Cost_Rate, Minimum_Stock, Meter_Qty, Weight_Piece, Width, Processed_Item_Image,Verified_Status, Chk_sts_shirt ) values (" & Str(Val(lbl_IdNo.Text)) & ", " & Val(lbl_DisplaySlNo.Text) & ", 'FP', '" & Trim(ProdName) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "', '" & Trim(txt_Code.Text) & "', " & Str(Val(ItmGrp_ID)) & ", " & Str(Val(Unt_Id)) & ", " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(txt_TaxRate.Text)) & ", " & Str(Val(txt_Rate.Text)) & ", " & Str(Val(txt_CostRate.Text)) & ", " & Str(Val(txt_MinimumStock.Text)) & "," & Val(txt_Meter_Qty.Text) & "," & Val(txt_Weight_Piece.Text) & "," & Val(txt_Width.Text) & ", @photo," & Val(Verified_STS) & "," & Str(Val(Shirt_STS)) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Processed_Item_Head set Processed_Item_Name = '" & Trim(ProdName) & "', Processed_Item_Nm = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(Sur) & "', Processed_Item_Code = '" & Trim(txt_Code.Text) & "', Processed_ItemGroup_IdNo = " & Str(Val(ItmGrp_ID)) & ", Unit_IdNo = " & Str(Val(Unt_Id)) & ", Meter_Qty = " & Val(txt_Meter_Qty.Text) & ", Weight_Piece = " & Val(txt_Weight_Piece.Text) & ", Width = " & Val(txt_Width.Text) & ", Minimum_Stock = " & Str(Val(txt_MinimumStock.Text)) & ", Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Sale_TaxRate = " & Str(Val(txt_TaxRate.Text)) & ", Sales_Rate = " & Str(Val(txt_Rate.Text)) & ", Cost_Rate = " & Str(Val(txt_CostRate.Text)) & ", Processed_Item_Image = @photo,Verified_Status = " & Val(Verified_STS) & " ,  Chk_sts_shirt =" & Str(Val(Shirt_STS)) & " Where Processed_Item_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Processed_Item_SalesName_details where Processed_Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_Details
                SlNo = 0
                For i = 0 To .RowCount - 1
                    Cmp_ID = Common_Procedures.Company_ShortNameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    PSalNm_ID = Common_Procedures.Processed_Item_SalesNameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                    If Val(Cmp_ID) <> 0 And Val(PSalNm_ID) <> 0 Then

                        SlNo = SlNo + 1

                        cmd.CommandText = "Insert into Processed_Item_SalesName_Details(Processed_Item_IdNo, sl_No, Company_IdNo, Processed_Item_SalesIdNo) values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(SlNo)) & ", " & Str(Val(Cmp_ID)) & ", " & Str(Val(PSalNm_ID)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(ProdName)       ' Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "FINISHEDPRODUCT"


            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_IdNo.Text)
                End If
            Else
                move_record(lbl_IdNo.Text)
            End If



        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_processed_item_head") > 0 Then
                MessageBox.Show("Duplicate Finished Product", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub FinishedProduct_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemGroup.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEMGROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemGroup.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Product_Sales_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PRODUCTSALESNAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Product_Sales_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Item_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim CompCondt As String

        Me.Text = ""

        '  If Val(Common_Procedures.User.IdNo) <> 1 And Common_Procedures.UR.Finished_Product_Verifition = "" Then chk_Verified_Status.Enabled = False

        con.Open()

        da = New SqlClient.SqlDataAdapter("select itemgroup_name from itemgroup_head order by itemgroup_name", con)
        da.Fill(dt1)
        cbo_ItemGroup.DataSource = dt1
        cbo_ItemGroup.DisplayMember = "itemgroup_name"

        da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)
        da.Fill(dt2)
        cbo_Unit.DataSource = dt2
        cbo_Unit.DisplayMember = "unit_name"


        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
            CompCondt = "Where (Company_Type <> 'UNACCOUNT')"
        End If

        da = New SqlClient.SqlDataAdapter("select Company_ShortName from Company_Head " & CompCondt & " order by Company_ShortName", con)
        da.Fill(dt3)
        cbo_Company.DataSource = dt3
        cbo_Company.DisplayMember = "Company_ShortName"
        cbo_Company.Visible = False
        cbo_Company.Text = ""

        da = New SqlClient.SqlDataAdapter("select Processed_Item_SalesName from Processed_Item_SalesName_Head order by Processed_Item_SalesName", con)
        da.Fill(dt4)
        cbo_Product_Sales_Name.DataSource = dt4
        cbo_Product_Sales_Name.DisplayMember = "Processed_Item_SalesName"
        cbo_Product_Sales_Name.Visible = False
        cbo_Product_Sales_Name.Text = ""

        da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_IdNo = 0 or Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
        da.Fill(dt5)
        cbo_FPNames.DataSource = dt5
        cbo_FPNames.DisplayMember = "Processed_Item_Name"

        'grp_Open.Visible = False
        'grp_Open.Left = (Me.Width - grp_Open.Width) - 10
        'grp_Open.Top = (Me.Height - grp_Open.Height) - 35  ' 20
        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) \ 2
        grp_Open.Top = ((Me.Height - grp_Open.Height) \ 2) + 10

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) \ 2
        grp_Filter.Top = ((Me.Height - grp_Filter.Height) \ 2) + 10

        grp_Pictures.Visible = False
        grp_Pictures.Left = (Me.Width - grp_Pictures.Width) \ 2
        grp_Pictures.Top = ((Me.Height - grp_Pictures.Height) \ 2)

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Code.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meter_Qty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Piece.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MinimumStock.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CostRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxRate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Company.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Product_Sales_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_FPNames.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Code.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meter_Qty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Piece.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MinimumStock.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CostRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxRate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Company.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Product_Sales_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_FPNames.LostFocus, AddressOf ControlLostFocus

        'AddHandler txt_Name.Validated, AddressOf ControlLostFocus
        'AddHandler cbo_ItemGroup.Validated, AddressOf ControlLostFocus
        'AddHandler txt_Code.Validated, AddressOf ControlLostFocus
        'AddHandler cbo_Unit.Validated, AddressOf ControlLostFocus
        'AddHandler txt_Meter_Qty.Validated, AddressOf ControlLostFocus
        'AddHandler txt_Weight_Piece.Validated, AddressOf ControlLostFocus
        'AddHandler txt_Width.Validated, AddressOf ControlLostFocus
        'AddHandler txt_MinimumStock.Validated, AddressOf ControlLostFocus
        'AddHandler txt_TaxPerc.Validated, AddressOf ControlLostFocus
        'AddHandler txt_CostRate.Validated, AddressOf ControlLostFocus
        'AddHandler txt_Rate.Validated, AddressOf ControlLostFocus
        'AddHandler txt_TaxRate.Validated, AddressOf ControlLostFocus

        Prec_ActCtrl = Nothing

        new_record()

    End Sub

    Private Sub Item_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                Call btn_CloseFilter_Click(sender, e)
                Exit Sub

            ElseIf grp_Open.Visible Then
                Call btnClose_Click(sender, e)
                Exit Sub

            ElseIf grp_Pictures.Visible Then
                Call btn_ClosePicture_Click(sender, e)
                Exit Sub

            Else
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()
                End If

            End If

        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If keyData = Keys.Enter Then

            On Error Resume Next

            If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

                dgv1 = dgv_Details

                With dgv1

                    If .CurrentCell.ColumnIndex = .ColumnCount - 1 Then

                        If .CurrentCell.RowIndex = .RowCount - 1 Then

                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()

                            Else
                                txt_Name.Focus()
                                Return True
                                Exit Function

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And (Trim(.CurrentRow.Cells(1).Value) = "" And Trim(.CurrentRow.Cells(2).Value) = "") Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                txt_Name.Focus()
                                Return True
                                Exit Function
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If


                    End If

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)
                'SendKeys.Send("{TAB}")

            End If

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If


    End Function

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        With cbo_Open
            '.BackColor = Color.lime
            '.ForeColor = Color.Blue
            .DroppedDown = True
        End With
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim Indx As Integer
        Dim FindStr As String

        If Asc(e.KeyChar) = 13 Then
            btn_Find_Click(sender, e)
        End If

        If Asc(e.KeyChar) = 8 Then
            If cbo_Open.SelectionStart <= 1 Then
                cbo_Open.Text = ""
                Exit Sub
            End If

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.Text.Length - 1)
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart - 1)
            End If

        Else

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text & e.KeyChar
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart) & e.KeyChar
            End If

        End If

        Indx = cbo_Open.FindString(FindStr)

        If Indx <> -1 Then
            cbo_Open.SelectedText = ""
            cbo_Open.SelectedIndex = Indx
            cbo_Open.SelectionStart = FindStr.Length
            cbo_Open.SelectionLength = cbo_Open.Text.Length
        End If

        e.Handled = True

    End Sub

    Private Sub txt_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.GotFocus
        'With txt_Name
        '    .BackColor = Color.lime
        '    .ForeColor = Color.Blue
        '    .SelectAll()
        'End With
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select Processed_Item_IdNo from Processed_Item_Head where Processed_Item_Name = '" & Trim(cbo_Open.Text) & "'", con)
            da.Fill(dt)
            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then
                move_record(movid)
                btnClose_Click(sender, e)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR FINDING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        grp_Back.Enabled = True
        grp_Open.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub cbo_ItemGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "itemgroup_head", "itemgroup_name", "", "")

    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemGroup, txt_Name, txt_Code, "itemgroup_head", "itemgroup_name", "", "")

    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, txt_Code, "itemgroup_head", "itemgroup_name", "", "")


    End Sub

    Private Sub cbo_ItemGroup_KeyPress1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Dim Indx As Integer = -1
        'Dim strFindStr As String = ""

        'Try
        '    If Asc(e.KeyChar) = 8 Then
        '        If cbo_ItemGroup.SelectionStart <= 1 Then
        '            cbo_ItemGroup.Text = ""
        '            Exit Sub
        '        End If
        '        If cbo_ItemGroup.SelectionLength = 0 Then
        '            strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.Text.Length - 1)
        '        Else
        '            strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.SelectionStart - 1)
        '        End If

        '    Else

        '        If cbo_ItemGroup.SelectionLength = 0 Then
        '            strFindStr = cbo_ItemGroup.Text & e.KeyChar
        '        Else
        '            strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.SelectionStart) & e.KeyChar
        '        End If

        '    End If

        '    Indx = cbo_ItemGroup.FindString(strFindStr)

        '    If Indx <> -1 Then
        '        cbo_ItemGroup.SelectedText = ""
        '        cbo_ItemGroup.SelectedIndex = Indx
        '        cbo_ItemGroup.SelectionStart = strFindStr.Length
        '        cbo_ItemGroup.SelectionLength = cbo_ItemGroup.Text.Length
        '        e.Handled = True
        '    Else
        '        e.Handled = True

        '    End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        'If Asc(e.KeyChar) = 13 Then
        '    SendKeys.Send("{TAB}")
        'End If

    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_Head", "Unit_Name", "", "")

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, txt_Code, txt_Meter_Qty, "Unit_Head", "Unit_Name", "", "")

    End Sub
    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, txt_Meter_Qty, "Unit_Head", "Unit_Name", "", "")

    End Sub
    Private Sub txt_TaxPerc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.GotFocus
        'With txt_TaxPerc
        '    .BackColor = Color.lime
        '    .ForeColor = Color.Blue
        '    .SelectAll()
        'End With
    End Sub

    Private Sub txt_VatPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btn_OpenFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OpenFilter.Click
        Dim movid As Integer = 0

        Try
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)

            If Val(movid) <> 0 Then
                move_record(movid)
                grp_Back.Enabled = True
                grp_Filter.Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        grp_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub dgv_Filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter.CellDoubleClick
        Call btn_OpenFilter_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Rate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.GotFocus
        'With txt_Rate
        '    .BackColor = Color.lime
        '    .ForeColor = Color.Blue
        '    .SelectAll()
        'End With
    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
    End Sub

    Private Sub txt_TaxRate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxRate.GotFocus
        'With txt_TaxRate
        '    .BackColor = Color.lime
        '    .ForeColor = Color.Blue
        '    .SelectAll()
        'End With
    End Sub

    Private Sub txt_TaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyUp
        txt_Rate.Text = Format(Val(txt_TaxRate.Text) * (100 / (100 + Val(txt_TaxPerc.Text))), "#########0.00")
    End Sub

    Private Sub txt_TaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "########0.00")
    End Sub

    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxRate.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            'SendKeys.Send("{TAB}")
        End If
    End Sub



    Private Sub txt_Code_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Code.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Code_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Code.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub



    Private Sub txt_CostRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CostRate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CostRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CostRate.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub



    Private Sub txt_TaxRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            'SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")

    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.LostFocus
        'With txt_Name
        '    .BackColor = Color.White
        '    .ForeColor = Color.Black
        'End With
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

    Private Sub cbo_ItemGroup_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.LostFocus
        'With cbo_ItemGroup
        '    .BackColor = Color.White
        '    .ForeColor = Color.Black
        'End With
    End Sub

    Private Sub cbo_Open_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.LostFocus
        'With cbo_Open
        '    .BackColor = Color.White
        '    .ForeColor = Color.Black
        'End With
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

    Private Sub cbo_Unit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.LostFocus
        'cbo_Unit.BackColor = Color.White
        'cbo_Unit.ForeColor = Color.Black
    End Sub

    Private Sub txt_Code_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Code.LostFocus
        'txt_Code.BackColor = Color.White
        'txt_Code.ForeColor = Color.Black
    End Sub

    Private Sub txt_CostRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CostRate.LostFocus
        'txt_CostRate.BackColor = Color.White
        'txt_CostRate.ForeColor = Color.Black
    End Sub

    Private Sub txt_Rate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.LostFocus
        'txt_Rate.BackColor = Color.White
        'txt_Rate.ForeColor = Color.Black
    End Sub

    Private Sub txt_TaxPerc_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.LostFocus
        'txt_TaxPerc.BackColor = Color.White
        'txt_TaxPerc.ForeColor = Color.Black
    End Sub

    Private Sub txt_TaxRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxRate.LostFocus
        'txt_TaxRate.BackColor = Color.White
        'txt_TaxRate.ForeColor = Color.Black
    End Sub

    Private Sub txt_MinimumStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MinimumStock.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_MinimumStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinimumStock.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_Company_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Company_Head", "Company_ShortName", "", "")

    End Sub

    Private Sub cbo_Company_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Company.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Company, Nothing, Nothing, "Company_Head", "Company_ShortName", "", "")
        Try
            With cbo_Company
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    If Val(dgv_Details.CurrentCell.RowIndex) <= 0 Then
                        txt_TaxRate.Focus()
                        .Visible = False

                    Else
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex - 1).Cells(2)
                        '.Visible = False

                    End If

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgv_Details.CurrentCell.ColumnIndex + 1)
                    '.Visible = False

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Company_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Company.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Company, Nothing, "Company_Head", "Company_ShortName", "", "")


        With cbo_Company

            If Asc(e.KeyChar) = 13 Then


                If dgv_Details.CurrentRow.Index = dgv_Details.RowCount - 1 And dgv_Details.CurrentCell.ColumnIndex >= 1 And Trim(dgv_Details.CurrentRow.Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_Name.Focus()
                    End If

                Else
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgv_Details.CurrentCell.ColumnIndex + 1)

                End If
                'cbo_Company.Visible = False



            End If

        End With


    End Sub

    Private Sub cbo_Company_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.LostFocus
        'With cbo_Company
        '    .BackColor = Color.White
        '    .ForeColor = Color.Black
        '    '.Visible = False
        '    '.Text = ""
        'End With
    End Sub

    Private Sub cbo_Company_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.TextChanged
        Try
            If cbo_Company.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                With dgv_Details
                    If Val(cbo_Company.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Company.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "TSOFT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Product_Sales_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Product_Sales_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_SalesName_Head", "Processed_Item_SalesName", "", "")

    End Sub

    Private Sub cbo_Product_Sales_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Product_Sales_Name.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Product_Sales_Name, Nothing, Nothing, "Processed_Item_SalesName_Head", "Processed_Item_SalesName", "", "")


        Try
            With cbo_Product_Sales_Name
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1)

                    '.Visible = False

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    If dgv_Details.CurrentCell.RowIndex = dgv_Details.Rows.Count - 1 Then
                        btn_save.Focus()
                        .Visible = False

                    Else
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex + 1).Cells(1)
                        '.Visible = False

                    End If

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Product_Sales_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Product_Sales_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Product_Sales_Name, Nothing, "Processed_Item_SalesName_Head", "Processed_Item_SalesName", "", "")

        With cbo_Product_Sales_Name

            If Asc(e.KeyChar) = 13 Then

                If dgv_Details.CurrentRow.Index = dgv_Details.RowCount - 1 And dgv_Details.CurrentCell.ColumnIndex >= 2 And Trim(dgv_Details.CurrentRow.Cells(1).Value) = "" And Trim(dgv_Details.CurrentRow.Cells(2).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_Name.Focus()
                    End If

                Else
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex + 1).Cells(1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Product_Sales_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Product_Sales_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Processed_Item_SalesName

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Product_Sales_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Product_Sales_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Product_Sales_Name.LostFocus
        'With cbo_Product_Sales_Name
        '    .BackColor = Color.White
        '    .ForeColor = Color.Black
        '    '.Visible = False
        '    '.Text = ""
        'End With
    End Sub

    Private Sub cbo_Product_Sales_Name_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Product_Sales_Name.TextChanged
        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            If cbo_Product_Sales_Name.Visible Then
                With dgv_Details
                    If Val(cbo_Product_Sales_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Product_Sales_Name.Text)
                    End If
                End With
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "TSOFT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim CompCondt As String

        With dgv_Details
            If e.ColumnIndex = 1 Then

                If cbo_Company.Visible = False Or Val(cbo_Company.Tag) <> e.RowIndex Then

                    cbo_Company.Tag = -100

                    CompCondt = ""
                    If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                        CompCondt = "Where (Company_Type <> 'UNACCOUNT')"
                    End If

                    Da = New SqlClient.SqlDataAdapter("select Company_ShortName from Company_Head " & CompCondt & " order by Company_ShortName", con)
                    Da.Fill(Dt1)
                    cbo_Company.DataSource = Dt1
                    cbo_Company.DisplayMember = "Company_ShortName"

                    cbo_Company.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Company.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Company.Width = .CurrentCell.Size.Width
                    cbo_Company.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Company.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Company.Visible = True

                    cbo_Company.BringToFront()
                    cbo_Company.Focus()

                End If


            Else
                cbo_Company.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If cbo_Product_Sales_Name.Visible = False Or Val(cbo_Product_Sales_Name.Tag) <> e.RowIndex Then

                    cbo_Product_Sales_Name.Tag = -100

                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_SalesName from Processed_Item_SalesName_Head order by Processed_Item_SalesName", con)
                    Da.Fill(Dt2)
                    cbo_Product_Sales_Name.DataSource = Dt2
                    cbo_Product_Sales_Name.DisplayMember = "Processed_Item_SalesName"

                    cbo_Product_Sales_Name.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Product_Sales_Name.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Product_Sales_Name.Width = .CurrentCell.Size.Width
                    cbo_Product_Sales_Name.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Product_Sales_Name.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Product_Sales_Name.Visible = True

                    cbo_Product_Sales_Name.BringToFront()
                    cbo_Product_Sales_Name.Focus()

                End If

            Else
                cbo_Product_Sales_Name.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        'If e.ColumnIndex <> 1 Then
        '    cbo_Company.Visible = False
        '    cbo_Company.Text = ""
        'End If
        'If e.ColumnIndex <> 2 Then
        '    cbo_Product_Sales_Name.Visible = False
        '    cbo_Product_Sales_Name.Text = ""
        'End If
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Try
            With dgv_Details
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                If (.CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2) And Trim(.CurrentCell.Value) <> "" Then
                    If .CurrentRow.Index = .Rows.Count - 1 Then
                        .Rows.Add()
                    End If
                End If
            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        On Error Resume Next

        With dgv_Details
            If e.KeyCode = Keys.Up Then
                If .CurrentRow.Index <= 0 Then
                    txt_TaxRate.Focus()
                End If
            End If

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex <= 0 And .CurrentCell.ColumnIndex <= 1 Then
                    txt_TaxRate.Focus()
                End If
            End If

        End With
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details
                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .ColumnCount - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    n = .CurrentRow.Index
                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        Grid_DeSelect()
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        With dgv_Details
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub txt_Width_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Width.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Width.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_Weight_Piece_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight_Piece.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Weight_Piece_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Piece.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Meter_Qty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meter_Qty.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Meter_Qty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meter_Qty.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_Unit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Unit.SelectedIndexChanged

    End Sub

    Private Sub btn_BrowsePhoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BrowsePhoto.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox1.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_ViewAll_Pictures_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ViewAll_Pictures.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_IdNo = 0 or Processed_Item_Type = 'FP'  and datalength(Processed_Item_Image) > 0 order by Processed_Item_Name", con)
        da.Fill(Dt2)
        cbo_FPNames.DataSource = Dt2
        cbo_FPNames.DisplayMember = "Processed_Item_Name"

        With dgv_Pictures

            dgv_Pictures.RowHeadersVisible = False

            .RowTemplate.Height = 120
            .RowTemplate.MinimumHeight = 20

            dgv_Pictures.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            dgv_Pictures.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            da = New SqlClient.SqlDataAdapter("select Processed_Item_Name, Processed_Item_Image from Processed_Item_Head Where Processed_Item_Type = 'FP' and datalength(Processed_Item_Image) > 0 Order by Processed_Item_Name, Processed_Item_Nm", con)
            Dt1 = New DataTable
            da.Fill(Dt1)
            'dgv_Pictures.Columns.Clear()
            dgv_Pictures.DataSource = Dt1

            dgv_Pictures.Columns(0).HeaderText = "PRODUCT NAME"
            dgv_Pictures.Columns(1).HeaderText = "IMAGE"

            dgv_Pictures.Columns(0).FillWeight = 100
            dgv_Pictures.Columns(1).FillWeight = 100


            DirectCast(dgv_Pictures.Columns(1), DataGridViewImageColumn).ImageLayout = DataGridViewImageCellLayout.Stretch

        End With

        'Dim img As New DataGridViewImageColumn
        'img.HeaderText = "Image"
        'dgv_Pictures.Columns.Insert(6, img)
        'dgv_Pictures.MaximumSiz()

        grp_Pictures.Visible = True
        grp_Pictures.BringToFront()
        grp_Back.Enabled = False
        If cbo_FPNames.Enabled And cbo_FPNames.Visible Then cbo_FPNames.Focus()

    End Sub

    Private Sub btn_ClosePicture_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ClosePicture.Click
        grp_Back.Enabled = True
        grp_Pictures.Visible = False
    End Sub

    Private Sub dgv_Pictures_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgv_Pictures.CellFormatting

        'image column = column 1

        If e.ColumnIndex = 1 Then
            'ignore new row
            If dgv_Pictures.Rows(e.RowIndex).IsNewRow Then Return

            'resize bitmap displayed

            Dim imagedata As Byte() = DirectCast(e.Value, Byte())

            Using ms As New MemoryStream(imagedata, 0, imagedata.Length)
                ms.Write(imagedata, 0, imagedata.Length)

                'picturebox1.backgroundimage = image.fromstream(ms, true)

                'dim ms as new memorystream(e.value)
                e.Value = New Bitmap(Image.FromStream(ms), 235, 100)
                e.FormattingApplied = True

            End Using

        End If

    End Sub

    Private Sub dgv_Pictures_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgv_Pictures.CellMouseClick
        Dim ItmID As Integer

        ItmID = Val(Common_Procedures.get_FieldValue(con, "Processed_Item_Head", "Processed_Item_IdNo", "(Processed_Item_Name = '" & Trim(dgv_Pictures.CurrentRow.Cells(0).Value()) & "')"))

        If ItmID <> 0 Then
            move_record(ItmID)
        End If

        grp_Back.Enabled = True
        grp_Pictures.Visible = False

    End Sub

    Private Sub cbo_FPNames_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FPNames.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP')", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_FPNames_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FPNames.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FPNames, Nothing, Nothing, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP')", "(Processed_Item_idno = 0)")

        If e.KeyValue = 38 And cbo_FPNames.DroppedDown = False Then
            e.Handled = True
            dgv_Pictures.Focus()
            If dgv_Pictures.Rows.Count > 0 Then
                dgv_Pictures.CurrentCell = dgv_Pictures.Rows(0).Cells(0)
                dgv_Pictures.CurrentCell.Selected = True
            End If
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_FPNames.DroppedDown = False Then
            e.Handled = True
            dgv_Pictures.Focus()
            If dgv_Pictures.Rows.Count > 0 Then
                dgv_Pictures.CurrentCell = dgv_Pictures.Rows(0).Cells(0)
                dgv_Pictures.CurrentCell.Selected = True
            End If
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_FPNames.DroppedDown = False Then
            cbo_FPNames.DroppedDown = True

        End If
    End Sub

    Private Sub cbo_FPNames_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FPNames.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FPNames, Nothing, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP')", "(Processed_Item_idno = 0)")

        With cbo_FPNames
            If Asc(e.KeyChar) = 13 Then
                dgv_Pictures.Focus()
                If dgv_Pictures.Rows.Count > 0 Then
                    dgv_Pictures.CurrentCell = dgv_Pictures.Rows(0).Cells(0)
                    dgv_Pictures.CurrentCell.Selected = True

                    For i = 0 To dgv_Pictures.Rows.Count - 1
                        If Trim(UCase(cbo_FPNames.Text)) = Trim(UCase(dgv_Pictures.Rows(i).Cells(0).Value)) Then
                            dgv_Pictures.CurrentCell = dgv_Pictures.Rows(i).Cells(0)
                            dgv_Pictures.CurrentCell.Selected = True
                            Exit For
                        End If
                    Next

                End If
            End If

        End With
    End Sub


   
End Class