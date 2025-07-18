Public Class Item_OpeningStock
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private vCbo_ItmNm As String
    Private vOp_YrCode As String
    Private New_Entry As Boolean
    Private Prec_ActCtrl As New Control
    Private cbo_KeyDwnVal As Double
    Private vCbo_DrawNo As String
    Private vCbo_DepNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private Sub clear()

        pnl_Back.Enabled = True

        vCbo_ItmNm = ""
        vCbo_DrawNo = ""
        vCbo_DepNm = ""

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        cbo_Department.Text = ""
        cbo_DrawingNo.Text = ""
        cbo_ItemName.Text = ""
        lbl_Unit.Text = ""

        dgv_Details.Rows.Clear()

        cbo_Grid_Brand.Visible = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Brand.Name Then
            cbo_Grid_Brand.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        'dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal idno As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer = 0
        Dim Sno As Integer = 0

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(idno)) & "/" & Trim(vOp_YrCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Department_Name, c.Unit_Name from Stores_item_head a LEFT OUTER JOIN Department_Head b ON a.Department_IDNo = b.Department_IDNo LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo where a.Item_IdNo = " & Str(Val(idno)), con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_IdNo.Text = dt1.Rows(0).Item("Item_IdNo").ToString

                cbo_Department.Text = dt1.Rows(0).Item("Department_Name").ToString
                cbo_DrawingNo.Text = dt1.Rows(0).Item("Drawing_No").ToString

                cbo_ItemName.Text = dt1.Rows(0).Item("Item_Name").ToString
                lbl_Unit.Text = dt1.Rows(0).Item("Unit_Name").ToString

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.brand_name from Stores_Stock_Item_Processing_Details a INNER JOIN Brand_Head b ON a.Brand_IdNo = b.Brand_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_IdNo = " & Str(Val(idno)) & " and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Reference_Date, a.For_OrderBy, a.Reference_No, a.sl_no", con)
                dt2 = New DataTable
                da1.Fill(dt2)

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        Sno = Sno + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(Sno)

                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Brand_name").ToString
                        If Val(dt2.Rows(i).Item("Quantity_New").ToString) <> 0 Then dgv_Details.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Quantity_New").ToString)
                        If Val(dt2.Rows(i).Item("Quantity_Old_Usable").ToString) <> 0 Then dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Quantity_Old_Usable").ToString)
                        If Val(dt2.Rows(i).Item("Quantity_Old_Scrap").ToString) <> 0 Then dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Quantity_Old_Scrap").ToString)

                    Next i


                End If

                dt2.Clear()

                Sno = 0
                For i = 0 To dgv_Details.Rows.Count - 1
                    Sno = Sno + 1
                    dgv_Details.Rows(i).Cells(0).Value = Val(Sno)
                Next

            End If

            dt1.Clear()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            dt2.Dispose()
            da1.Dispose()

            If cbo_Department.Visible And cbo_Department.Enabled Then cbo_Department.Focus()

        End Try



    End Sub

    Private Sub Item_OpeningStock_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Department.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DEPARTMENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Department.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Brand.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BRAND" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Brand.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then
                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Item_OpeningStock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable

        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select item_name from Stores_item_head order by item_name", con)
        Da.Fill(Dt1)
        cbo_ItemName.DataSource = Dt1
        cbo_ItemName.DisplayMember = "item_name"

        Da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
        Da.Fill(Dt2)
        cbo_Department.DataSource = Dt2
        cbo_Department.DisplayMember = "Department_Name"

        Da = New SqlClient.SqlDataAdapter("select distinct(Drawing_No) from Stores_item_head order by Drawing_No", con)
        Da.Fill(Dt3)
        cbo_DrawingNo.DataSource = Dt3
        cbo_DrawingNo.DisplayMember = "Drawing_No"

        AddHandler cbo_Department.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DrawingNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Brand.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btnClose.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Department.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DrawingNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Brand.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Save.LostFocus, AddressOf ControlLostFocus
        AddHandler btnClose.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        vOp_YrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        vOp_YrCode = Trim(Mid(Val(vOp_YrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(vOp_YrCode, 2))

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Purchase_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Purchase_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                lbl_Company.Tag = 0
                lbl_Company.Text = ""
                Me.Text = ""
                Common_Procedures.CompIdNo = 0


                lbl_Company.Text = Common_Procedures.Show_CompanySelection_On_FormClose(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)
                Me.Text = lbl_Company.Text
                If Val(Common_Procedures.CompIdNo) = 0 Then

                    Me.Close()

                Else

                    new_record()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = dgv_Details

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()

                                Else
                                    cbo_Department.Focus()
                                    Return True
                                    Exit Function

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_ItemName.Focus()

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

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Itm_ID As Integer
        Dim Nr As Integer
        Dim NewCode As String

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Master_Opening_Stock, "~L~") = 0 And InStr(Common_Procedures.UR.Master_Opening_Stock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        Try

            Itm_ID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

            If Val(lbl_IdNo.Text) = 0 Or Val(Itm_ID) = 0 Or Trim(UCase(lbl_IdNo.Text)) = "NEW" Or Val(Itm_ID) <> Val(lbl_IdNo.Text) Then
                MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(vOp_YrCode)

            cmd.Connection = con

            cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            If Nr = 0 Then
                MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else

                new_record()

                MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()

            If cbo_ItemName.Enabled = True And cbo_ItemName.Visible = True Then cbo_ItemName.Focus()

        End Try


    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Item_IdNo) from Stores_item_head Where Item_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(Item_IdNo) from Stores_item_head Where Item_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Item_IdNo) from Stores_item_head Where Item_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and Item_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(Item_IdNo) from Stores_item_head Where Item_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Item_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        clear()

        New_Entry = True

        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = "NEW"  ' Common_Procedures.get_MaxIdNo(con, "Stores_item_head", "Item_IdNo", "")

        'If Val(lbl_IdNo.Text) <= 100 Then lbl_IdNo.Text = 101

        If cbo_Department.Enabled And cbo_Department.Visible Then cbo_Department.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '----
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim nr As Long = 0
        Dim Itm_ID As Integer = 0
        Dim Brnd_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim OpDate As Date

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Master_Opening_Stock, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Itm_ID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

        If Val(lbl_IdNo.Text) = 0 Or Val(Itm_ID) = 0 Or Trim(UCase(lbl_IdNo.Text)) = "NEW" Or Val(Itm_ID) <> Val(lbl_IdNo.Text) Then
            MessageBox.Show("Invalid Item Name", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
            Exit Sub
        End If

        Unt_ID = Common_Procedures.Unit_NameToIdNo(con, lbl_Unit.Text)


        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                Brnd_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                If Brnd_ID = 0 Then
                    MessageBox.Show("Invalid Brand Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                    End If
                    Exit Sub
                End If

            End If

        Next


        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(vOp_YrCode)

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate.Date)

            cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        Brnd_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Stores_Stock_Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Entry_ID, Party_Bill_No, Particulars, Sl_No, Item_IdNo, Unit_IdNo, Brand_IdNo, Quantity_New, Quantity_Old_Usable, Quantity_Old_Scrap) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, 0, '', '', '', " & Str(Val(Sno)) & ", " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Unt_ID)) & ", " & Str(Val(Brnd_ID)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With


            tr.Commit()

            move_record(Val(lbl_IdNo.Text))

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()
            If cbo_Department.Enabled And cbo_Department.Visible Then cbo_Department.Focus()

        End Try

    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        Dim dep_idno As Integer = 0
        Dim Condt As String

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Department.Text))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        vCbo_ItmNm = Trim(cbo_ItemName.Text)
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        Dim dep_idno As Integer = 0
        Dim Condt As String

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Department.Text))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, cbo_DrawingNo, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(1)
            End If

        End With

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dno_nm As String
        Dim Unt_nm As String
        Dim Dep_nm As String
        Dim dep_idno As Integer = 0
        Dim Itm_idno As Integer = 0
        Dim Condt As String

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Department.Text))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(cbo_Department.Text) = "" Or Trim(cbo_DrawingNo.Text) = "" Or Trim(lbl_Unit.Text) = "" Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then

                Itm_idno = Common_Procedures.itemalais_NameToIdNo(con, Trim(cbo_ItemName.Text))

                da = New SqlClient.SqlDataAdapter("select a.Drawing_No, b.unit_name, c.department_name from Stores_item_head a left outer join unit_head b on a.unit_idno = b.unit_idno left outer join Department_Head c ON a.Department_IdNo = c.Department_IdNo Where a.item_IdNo = " & Str(Val(Itm_idno)), con)
                da.Fill(dt)

                Dep_nm = ""
                dno_nm = ""
                Unt_nm = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        Dep_nm = Trim(dt.Rows(0).Item("department_name").ToString)
                        dno_nm = Trim(dt.Rows(0).Item("Drawing_No").ToString)
                        Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)
                    End If
                End If

                dt.Dispose()
                da.Dispose()

                cbo_Department.Text = Trim(Dep_nm)
                cbo_DrawingNo.Text = Trim(dno_nm)
                lbl_Unit.Text = Trim(Unt_nm)

                move_record(Val(Itm_idno))

            End If

            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

        End If

    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Stores_Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Department_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Department.GotFocus
        vCbo_DepNm = cbo_Department.Text
    End Sub

    Private Sub cbo_Department_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Department.KeyDown
        cbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Department, Nothing, cbo_DrawingNo, "Department_Head", "Department_name", "", "(Department_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(1)
            End If

        End With

    End Sub

    Private Sub cbo_Department_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Department.KeyPress
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dep_idno As Integer = 0
        Dim Condt As String

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Department, cbo_DrawingNo, "Department_Head", "Department_name", "", "(Department_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(vCbo_DepNm)) <> Trim(UCase(cbo_Department.Text)) Then

                dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Department.Text))

                Condt = ""
                If dep_idno <> 0 And dep_idno <> 1 Then Condt = " Where (Department_idno = " & Str(Val(dep_idno)) & ")"

                Da = New SqlClient.SqlDataAdapter("select item_name from Stores_item_head " & Condt & " order by item_name", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                cbo_ItemName.DataSource = Dt1
                cbo_ItemName.DisplayMember = "item_name"

                Condt = " Where (Department_idno = " & Str(Val(dep_idno)) & ")"

                Da = New SqlClient.SqlDataAdapter("select distinct(Drawing_No) from Stores_item_head " & Condt & " order by Drawing_No", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                cbo_DrawingNo.DataSource = Dt2
                cbo_DrawingNo.DisplayMember = "Drawing_No"

                new_record()

                cbo_Department.Text = Common_Procedures.Department_IdNoToName(con, dep_idno)
                cbo_DrawingNo.Focus()

            End If

        End If

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

    Private Sub cbo_DrawingNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DrawingNo.GotFocus
        vCbo_DrawNo = cbo_DrawingNo.Text
    End Sub

    Private Sub cbo_DrawingNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DrawingNo.KeyDown
        Dim dep_idno As Integer = 0
        Dim Condt As String

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Department.Text))

        'Condt = ""
        'If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"
        Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DrawingNo, cbo_Department, cbo_ItemName, "Stores_item_head", "Drawing_No", Condt, "(Item_idno = 0)")

    End Sub

    Private Sub cbo_DrawingNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DrawingNo.KeyPress
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Itm_idno As Integer = 0
        Dim item_nm As String = ""
        Dim Unt_nm As String = ""
        Dim dno As String = ""
        Dim dep_idno As Integer = 0
        Dim Condt As String = ""

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Department.Text))

        'Condt = ""
        'If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"
        Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DrawingNo, cbo_ItemName, "Stores_item_head", "Drawing_No", Condt, "(Item_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(cbo_ItemName.Text) = "" Or Trim(UCase(vCbo_DrawNo)) <> Trim(UCase(cbo_DrawingNo.Text)) Then

                dep_idno = Common_Procedures.Department_NameToIdNo(con, cbo_Department.Text)
                dno = cbo_DrawingNo.Text

                Da = New SqlClient.SqlDataAdapter("select a.Item_IdNo, a.Item_name, b.unit_name from Stores_item_head a left outer join unit_head b on a.unit_idno = b.unit_idno where a.department_idno = " & Str(Val(dep_idno)) & " and a.drawing_no = '" & Trim(dno) & "'", con)
                Da.Fill(Dt)

                Itm_idno = 0
                item_nm = ""
                Unt_nm = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Itm_idno = Val(Dt.Rows(0).Item("Item_idno").ToString)
                        item_nm = Trim(Dt.Rows(0).Item("Item_name").ToString)
                        Unt_nm = Trim(Dt.Rows(0).Item("unit_name").ToString)
                    End If
                End If

                Dt.Dispose()
                Da.Dispose()

                cbo_ItemName.Text = Trim(item_nm)
                lbl_Unit.Text = Trim(Unt_nm)

                move_record(Val(Itm_idno))

                cbo_ItemName.Focus()

            End If

        End If

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim rect As Rectangle
        Dim dep_idno As Integer = 0

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_Brand.Visible = False Or Val(cbo_Grid_Brand.Tag) <> e.RowIndex Then

                    cbo_Grid_Brand.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Brand_Name from Brand_Head order by Brand_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_Brand.DataSource = Dt3
                    cbo_Grid_Brand.DisplayMember = "Brand_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Brand.Left = .Left + rect.Left
                    cbo_Grid_Brand.Top = .Top + rect.Top

                    cbo_Grid_Brand.Width = rect.Width
                    cbo_Grid_Brand.Height = rect.Height
                    cbo_Grid_Brand.Text = .CurrentCell.Value

                    cbo_Grid_Brand.Tag = Val(e.RowIndex)
                    cbo_Grid_Brand.Visible = True

                    cbo_Grid_Brand.BringToFront()
                    cbo_Grid_Brand.Focus()

                End If

            Else
                cbo_Grid_Brand.Visible = False

            End If



        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        cbo_ItemName.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
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

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Grid_Brand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Brand.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Brand, Nothing, Nothing, "Brand_Head", "Brandname", "", "(Brand_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                e.Handled = True
                If .CurrentCell.RowIndex <= 0 Then
                    cbo_ItemName.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)

                End If


            End If

            If (e.KeyValue = 40 And cbo_Grid_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                e.Handled = True
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_Department.Focus()
                    End If

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Brand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Brand.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Brand, Nothing, "Brand_Head", "Brand_name", "", "(Brand_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_Department.Focus()
                    End If

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_Brand_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Brand.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Brand_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Brand.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Brand_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Brand.TextChanged
        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            If cbo_Grid_Brand.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Brand.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Brand.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_ItemName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ItemName.SelectedIndexChanged

    End Sub
End Class