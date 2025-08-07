Public Class Gate_Pass_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GPASS-"
    Private cbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private dgv_DrawNo As String = ""
    Private vCbo_ItmNm As String = ""
    Private vCloPic_STS As Boolean = False

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private vcbo_KeyDwnVal As Double

    Private Sub clear()

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        New_Entry = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_SendTo.Text = ""
        cbo_Transport.Text = ""
        cbo_VechileNo.Text = ""
        txt_Remarks.Text = ""

        dgv_Details.Rows.Clear()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_DrawNo = ""
        vCbo_ItmNm = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

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

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub Gate_Pass_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""

        Try

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Department.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DEPARTMENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_Department.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Item.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_Item.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Brand.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BRAND" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_Brand.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

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

    Private Sub Gate_Pass_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Gate_Pass_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                Else
                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub

                    Else
                        Close_Form()

                    End If



                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Gate_Pass_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable

        Me.Text = ""

        con.Open()

        'da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
        'da.Fill(dt1)
        'cbo_Grid_Department.DataSource = dt1
        'cbo_Grid_Department.DisplayMember = "Department_Name"

        'da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Item_AlaisHead order by Item_DisplayName", con)
        'da.Fill(dt2)
        'cbo_Grid_Item.DataSource = dt2
        'cbo_Grid_Item.DisplayMember = "Item_Name"

        'da = New SqlClient.SqlDataAdapter("select Brand_Name from Brand_Head order by Brand_Name", con)
        'da.Fill(dt3)
        'cbo_Grid_Brand.DataSource = dt3
        'cbo_Grid_Brand.DisplayMember = "Brand_Name"

        'da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head order by Unit_Name", con)
        'da.Fill(dt4)
        'cbo_Grid_Unit.DataSource = dt4
        'cbo_Grid_Unit.DisplayMember = "Unit_Name"


        'da = New SqlClient.SqlDataAdapter("select distinct(Send_To) from Gate_Pass_Head order by Send_To", con)
        'da.Fill(dt6)
        'cbo_SendTo.DataSource = dt6
        'cbo_SendTo.DisplayMember = "Send_To"

        'da = New SqlClient.SqlDataAdapter("select distinct(Send_Through) from Gate_Pass_Head order by Send_Through", con)
        'da.Fill(dt7)
        'cbo_SendThrough.DataSource = dt7
        'cbo_SendThrough.DisplayMember = "Send_Through"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SendTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VechileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SendTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VechileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Close_Form()
        Try

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

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= 8 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()

                                Else
                                    msk_Date.Focus()
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
                                txt_Remarks.Focus()

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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* , b.Ledger_Name as DelvName, c.Ledger_Name as TransportName from Gate_Pass_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo  Where a.Gate_Pass_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Gate_Pass_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Gate_Pass_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_SendTo.Text = dt1.Rows(0).Item("DelvName").ToString
                cbo_Transport.Text = dt1.Rows(0).Item("TransportName").ToString
                cbo_VechileNo.Text = dt1.Rows(0).Item("Vechile_No").ToString
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.* from Gate_Pass_Details a where a.Gate_Pass_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Dc_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Particulars").ToString

                        dgv_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("empty_Beams").ToString), "########0")
                        If Val(dgv_Details.Rows(n).Cells(3).Value) = 0 Then dgv_Details.Rows(n).Cells(3).Value = ""

                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Bales").ToString), "########0")
                        If Val(dgv_Details.Rows(n).Cells(4).Value) = 0 Then dgv_Details.Rows(n).Cells(4).Value = ""


                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("pcs").ToString), "########0")
                        If Val(dgv_Details.Rows(n).Cells(5).Value) = 0 Then dgv_Details.Rows(n).Cells(5).Value = ""

                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(6).Value) = 0 Then dgv_Details.Rows(n).Cells(6).Value = ""

                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("bags").ToString), "########0")
                        If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then dgv_Details.Rows(n).Cells(7).Value = ""

                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("weights").ToString), "########0.000")
                        If Val(dgv_Details.Rows(n).Cells(8).Value) = 0 Then dgv_Details.Rows(n).Cells(8).Value = ""

                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Entry_Id").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Entry_Code").ToString

                    Next i

                End If

                With dgv_Details_Total

                    .Rows.Clear()
                    .Rows.Add()
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Bales").ToString), "########0")
                    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Empty_Beams").ToString), "########0")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Pcs").ToString), "########0")
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Bags").ToString), "########0")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Weights").ToString), "########0.000")

                End With

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_GatePass, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_GatePass, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Update PavuYarn_Delivery_Head set PavuGate_Pass_Code = '', PavuGate_Pass_Increment = PavuGate_Pass_Increment - 1 Where PavuGate_Pass_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update PavuYarn_Delivery_Head set YarnGate_Pass_Code = '', YarnGate_Pass_Increment = YarnGate_Pass_Increment - 1 Where YarnGate_Pass_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update ClothSales_Invoice_Head set Gate_Pass_Code = '', Gate_Pass_Increment = Gate_Pass_Increment - 1 Where Gate_Pass_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update JobWork_Piece_Delivery_Head set Gate_Pass_Code = '', Gate_Pass_Increment = Gate_Pass_Increment - 1 Where Gate_Pass_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update JobWork_Empty_BeamBagCone_Delivery_Head set Gate_Pass_Code = '', Gate_Pass_Increment = Gate_Pass_Increment - 1 Where Gate_Pass_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Empty_BeamBagCone_Delivery_Head set Gate_Pass_Code = '', Gate_Pass_Increment = Gate_Pass_Increment - 1 Where Gate_Pass_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Gate_Pass_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Gate_Pass_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Item_AlaisHead order by Item_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_Item.DataSource = dt1
            cbo_Filter_Item.DisplayMember = "Item_DisplayName"

            cbo_Filter_Item.Text = ""
            cbo_Filter_Item.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_GatePass, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_GatePass, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref.No.", "FOR NEW NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Gate_Pass_No from Gate_Pass_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid REF No", "DOES NOT INSERT NEW NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Gate_Pass_No from Gate_Pass_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Gate_Pass_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Gate_Pass_No from Gate_Pass_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Gate_Pass_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Gate_Pass_No from Gate_Pass_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Gate_Pass_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Gate_Pass_No from Gate_Pass_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Gate_Pass_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Gate_Pass_Head", "Gate_Pass_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString

            da = New SqlClient.SqlDataAdapter("select top 1 * from Gate_Pass_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Gate_Pass_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Gate_Pass_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Gate_Pass_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Gate_Pass_No from Gate_Pass_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("REF No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Del_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Machine_ID As Integer = 0
        Dim Unit_ID As Integer = 0
        Dim Brand_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotbals As Single = 0
        Dim vTotembem As Single = 0
        Dim vTotpcs As Single = 0
        Dim vTotmtrs As Single = 0
        Dim vTotbgs As Single = 0
        Dim vTotwgts As Single = 0

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_GatePass, New_Entry) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        Del_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SendTo.Text)
        If Del_ID = 0 Then
            MessageBox.Show("Invalid Delivery Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SendTo.Enabled And cbo_SendTo.Visible Then cbo_SendTo.Focus()
            Exit Sub
        End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                'Dep_ID = Common_Procedures.Department_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                'If Dep_ID = 0 Then
                '    MessageBox.Show("Invalid Department Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                '    End If
                '    Exit Sub
                'End If

                'Item_ID = Common_Procedures.itemalais_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value)
                'If Item_ID = 0 Then
                '    MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                '    End If
                '    Exit Sub
                'End If

                'Brand_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value)
                'If Brand_ID = 0 Then
                '    MessageBox.Show("Invalid Brand Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                '    End If
                '    Exit Sub
                'End If

                'Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value)
                'If Unit_ID = 0 Then
                '    MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(6)
                '    End If
                '    Exit Sub
                'End If

                ''Machine_ID = Common_Procedures.Machine_NameToIdNo(con, dgv_Details.Rows(i).Cells(7).Value)
                ''If Machine_ID = 0 Then
                ''    MessageBox.Show("Invalid Machine Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ''    If dgv_Details.Enabled And dgv_Details.Visible Then
                ''        dgv_Details.Focus()
                ''        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(7)
                ''    End If
                ''    Exit Sub
                ''End If

            End If

        Next

        vTotbals = 0
        vTotembem = 0
        vTotpcs = 0
        vTotmtrs = 0
        vTotbgs = 0
        vTotwgts = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTotbals = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotembem = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotpcs = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotmtrs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotbgs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotwgts = Val(dgv_Details_Total.Rows(0).Cells(8).Value())

        End If

        If Trim(cbo_SendTo.Text) = "" Then
            MessageBox.Show("Invalid Send To", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SendTo.Enabled Then cbo_SendTo.Focus()
            Exit Sub
        End If

        'If vTotqty = 0 Then
        '    MessageBox.Show("Invalid Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dgv_Details.Enabled And dgv_Details.Visible Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '    End If
        '    Exit Sub
        'End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Gate_Pass_Head", "Gate_Pass_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@GPDate", dtp_date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Gate_Pass_Head(Gate_Pass_Code, Company_IdNo, Gate_Pass_No, for_OrderBy, Gate_Pass_Date, Ledger_Idno, Transport_IdNo, Vechile_No, Remarks, Total_Bales , Total_Empty_Beams, Total_Pcs   ,Total_Meters , Total_Bags ,Total_Weights) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @GPDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Trans_ID)) & ", '" & Trim(cbo_VechileNo.Text) & "', '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotbals)) & " ," & Str(Val(vTotembem)) & "," & Str(Val(vTotpcs)) & "," & Str(Val(vTotmtrs)) & "," & Str(Val(vTotbgs)) & "," & Str(Val(vTotwgts)) & " )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Gate_Pass_Head set Gate_Pass_Date= @GPDate , Ledger_Idno = " & Str(Val(Del_ID)) & ",Transport_IdNo = " & Str(Val(Trans_ID)) & " ,  Vechile_No = '" & Trim(cbo_VechileNo.Text) & "', Remarks =  '" & Trim(txt_Remarks.Text) & "', Total_Bales = " & Str(Val(vTotbals)) & " , Total_Empty_Beams = " & Str(Val(vTotembem)) & ", Total_Pcs = " & Str(Val(vTotpcs)) & ",Total_Meters =  " & Str(Val(vTotmtrs)) & ", Total_Bags = " & Str(Val(vTotbgs)) & ", Total_Weights = " & Str(Val(vTotwgts)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update JobWork_Piece_Delivery_Head set Gate_Pass_Code = '', Gate_Pass_Increment = Gate_Pass_Increment - 1 Where Gate_Pass_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update JobWork_Empty_BeamBagCone_Delivery_Head set Gate_Pass_Code = '', Gate_Pass_Increment = Gate_Pass_Increment - 1 Where Gate_Pass_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Empty_BeamBagCone_Delivery_Head set Gate_Pass_Code = '', Gate_Pass_Increment = Gate_Pass_Increment - 1 Where Gate_Pass_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_Invoice_Head set Gate_Pass_Code = '', Gate_Pass_Increment = Gate_Pass_Increment - 1 Where Gate_Pass_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update PavuYarn_Delivery_Head set PavuGate_Pass_Code = '', PavuGate_Pass_Increment = PavuGate_Pass_Increment - 1 Where PavuGate_Pass_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update PavuYarn_Delivery_Head set YarnGate_Pass_Code = '', YarnGate_Pass_Increment = YarnGate_Pass_Increment - 1 Where YarnGate_Pass_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update JobWork_PavuYarn_Return_Head set PavuGate_Pass_Code = '', PavuGate_Pass_Increment = PavuGate_Pass_Increment - 1 Where PavuGate_Pass_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update JobWork_PavuYarn_Return_Head set YarnGate_Pass_Code = '', YarnGate_Pass_Increment = YarnGate_Pass_Increment - 1 Where YarnGate_Pass_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If

            cmd.CommandText = "Delete from Gate_Pass_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) <> "" Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Gate_Pass_Details ( Gate_Pass_Code, Company_IdNo, Gate_Pass_No, for_OrderBy, Gate_Pass_Date, Sl_No, Dc_No , Particulars, Bales , Empty_Beams , Pcs , Meters , Bags , Weights , Entry_Id, Entry_Code) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @GPDate, " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "'," & Str(Val(.Rows(i).Cells(4).Value)) & " , " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & " , " & Str(Val(.Rows(i).Cells(7).Value)) & " , " & Str(Val(.Rows(i).Cells(8).Value)) & " ,'" & Trim(.Rows(i).Cells(9).Value) & "', '" & Trim(.Rows(i).Cells(10).Value) & "' )"
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(.Rows(i).Cells(9).Value)) = "JPDLV-" Then

                            cmd.CommandText = "Update JobWork_Piece_Delivery_Head set Gate_Pass_Code = '" & Trim(NewCode) & "', Gate_Pass_Increment = Gate_Pass_Increment + 1 Where JobWork_Piece_Delivery_Code = '" & Trim(.Rows(i).Cells(10).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf Trim(UCase(.Rows(i).Cells(9).Value)) = "JEBDC-" Then

                            cmd.CommandText = "Update JobWork_Empty_BeamBagCone_Delivery_Head set Gate_Pass_Code = '" & Trim(NewCode) & "', Gate_Pass_Increment = Gate_Pass_Increment + 1 Where JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(.Rows(i).Cells(10).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf Trim(UCase(.Rows(i).Cells(9).Value)) = "EBDLV-" Then

                            cmd.CommandText = "Update Empty_BeamBagCone_Delivery_Head set Gate_Pass_Code = '" & Trim(NewCode) & "', Gate_Pass_Increment = Gate_Pass_Increment + 1 Where Empty_BeamBagCone_Delivery_Code = '" & Trim(.Rows(i).Cells(10).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf Trim(UCase(.Rows(i).Cells(9).Value)) = "CSINV-" Then

                            cmd.CommandText = "Update ClothSales_Invoice_Head set Gate_Pass_Code = '" & Trim(NewCode) & "', Gate_Pass_Increment = Gate_Pass_Increment + 1 Where ClothSales_Invoice_Code = '" & Trim(.Rows(i).Cells(10).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf Trim(UCase(.Rows(i).Cells(9).Value)) = "PYPDL-" Then

                            cmd.CommandText = "Update PavuYarn_Delivery_Head set PavuGate_Pass_Code = '" & Trim(NewCode) & "', PavuGate_Pass_Increment = PavuGate_Pass_Increment + 1 Where PavuYarn_Delivery_Code = '" & Trim(.Rows(i).Cells(10).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf Trim(UCase(.Rows(i).Cells(9).Value)) = "PYYDL-" Then

                            cmd.CommandText = "Update PavuYarn_Delivery_Head set YarnGate_Pass_Code = '" & Trim(NewCode) & "', YarnGate_Pass_Increment = YarnGate_Pass_Increment + 1 Where PavuYarn_Delivery_Code = '" & Trim(.Rows(i).Cells(10).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf Trim(UCase(.Rows(i).Cells(9).Value)) = "JPYPD-" Then

                            cmd.CommandText = "Update JobWork_PavuYarn_Return_Head set PavuGate_Pass_Code = '" & Trim(NewCode) & "', PavuGate_Pass_Increment = PavuGate_Pass_Increment + 1 Where JobWork_PavuYarn_Return_Code = '" & Trim(.Rows(i).Cells(10).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf Trim(UCase(.Rows(i).Cells(9).Value)) = "JPYYD-" Then

                            cmd.CommandText = "Update JobWork_PavuYarn_Return_Head set YarnGate_Pass_Code = '" & Trim(NewCode) & "', YarnGate_Pass_Increment = YarnGate_Pass_Increment + 1 Where JobWork_PavuYarn_Return_Code = '" & Trim(.Rows(i).Cells(10).Value) & "'"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next

            End With

            tr.Commit()

            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            new_record()

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Then
                    TotalQuantity_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        txt_Remarks.Focus()
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub
   
    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Item_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Item_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Gate_Pass_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Gate_Pass_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Gate_Pass_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Val(Item_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Item_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.quantity, b.Entry_Id, c.item_name, d.unit_name from Gate_Pass_Head a left outer join Gate_Pass_Details b on a.Gate_Pass_Code = b.Gate_Pass_Code left outer join item_head c on b.item_idno = c.item_idno left outer join unit_head d on b.unit_idno = d.unit_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Gate_Pass_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Gate_Pass_Date, for_orderby, Gate_Pass_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Gate_Pass_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Gate_Pass_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Quantity").ToString), )
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Unit_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Entry_Id").ToString

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()
    End Sub

    Private Sub cbo_Filter_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Item.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Item, dtp_Filter_ToDate, btn_Filter_Show, "Item_AlaisHead", "Item_DisplayName", "", "(Item_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Item.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Item, btn_Filter_Show, "Item_AlaisHead", "Item_DisplayName", "", "(Item_idno = 0)")
    End Sub

    Private Sub dtp_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            e.Handled = True
            btn_Cancel.Focus()
        End If
    End Sub

    Private Sub dtp_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            btn_Filter_Show.Focus()
        End If
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            e.Handled = True
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub TotalQuantity_Calculation()
        Dim Sno As Integer
        Dim Totbls As Single = 0
        Dim Totembms As Single = 0
        Dim Totpcs As Single = 0
        Dim Totmtrs As Single = 0
        Dim Totbgs As Single = 0
        Dim Totwgt As Single = 0

        Sno = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(3).Value) <> 0 Then
                    Totbls = Totbls + Val(.Rows(i).Cells(3).Value)
                End If
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    Totembms = Totembms + Val(.Rows(i).Cells(4).Value)
                End If
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    Totpcs = Totpcs + Val(.Rows(i).Cells(5).Value)
                End If
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    Totmtrs = Totmtrs + Val(.Rows(i).Cells(6).Value)
                End If
                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                    Totbgs = Totbgs + Val(.Rows(i).Cells(7).Value)
                End If
                If Val(.Rows(i).Cells(8).Value) <> 0 Then
                    Totwgt = Totwgt + Val(.Rows(i).Cells(8).Value)
                End If

            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Format(Val(Totbls), "########0")
            .Rows(0).Cells(4).Value = Format(Val(Totembms), "########0")
            .Rows(0).Cells(5).Value = Format(Val(Totpcs), "########0")
            .Rows(0).Cells(6).Value = Format(Val(Totmtrs), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(Totbgs), "########0")
            .Rows(0).Cells(8).Value = Format(Val(Totwgt), "########0.000")

        End With

    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Gate_Pass_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Gate_Pass_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If

                Else
                    PrintDocument1.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*,tC.* , b.* , c.Ledger_Name as TransportName from Gate_Pass_Head a INNER JOIN Company_Head tC ON a.Company_IdNo = tC.Company_IdNo INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Gate_Pass_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Gate_Pass_Details a  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Gate_Pass_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Format1(e)
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String
        'Dim ItmNm1 As String, ItmNm2 As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 30
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 8

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "CSINV-" Then

            ClAr(1) = Val(50) : ClAr(2) = 60 : ClAr(3) = 230 : ClAr(4) = 130 : ClAr(5) = 130
            ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JEBDC-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "EBDLV-" Then

            ClAr(1) = Val(50) : ClAr(2) = 100 : ClAr(3) = 300 : ClAr(4) = 150
            ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPDLV-" Then

            ClAr(1) = Val(50) : ClAr(2) = 100 : ClAr(3) = 300 : ClAr(4) = 150
            ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYPDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYPD-" Then

            ClAr(1) = Val(50) : ClAr(2) = 100 : ClAr(3) = 300 : ClAr(4) = 150
            ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYYDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYYD-" Then

            ClAr(1) = Val(50) : ClAr(2) = 100 : ClAr(3) = 300 : ClAr(4) = 150
            ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        End If

        'ClAr(1) = Val(50) : ClAr(2) = 100 : ClAr(3) = 300 : ClAr(4) = 150
        'ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        TxtHgt = 19

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        'If NoofDets >= NoofItems_PerPage Then

                        '    CurY = CurY + TxtHgt

                        '    Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                        '    NoofDets = NoofDets + 1

                        '    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                        '    e.HasMorePages = True
                        '    Return

                        'End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Particulars").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 30 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Dc_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)

                        If Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "CSINV-" Then

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JEBDC-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "EBDLV-" Then

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Empty_Beams").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPDLV-" Then

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYPDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYPD-" Then

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Empty_Beams").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYYDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYYD-" Then

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weights").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                        End If


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.* from Gate_Pass_Details a  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Gate_Pass_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "GATE PASS", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = 450
        W1 = e.Graphics.MeasureString("DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("Send To :  ", pFont).Width


        CurY = CurY + TxtHgt - 10


        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "REF.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Gate_Pass_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Send To", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Gate_Pass_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        If Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "CSINV-" Then

            Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JEBDC-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "EBDLV-" Then

            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAMS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPDLV-" Then

            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYPDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYPD-" Then

            Common_Procedures.Print_To_PrintDocument(e, "BEAMS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYYDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYYD-" Then

            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHTS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)


        End If
        
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

            If Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "CSINV-" Then

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JEBDC-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "EBDLV-" Then

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Empty_Beams").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

            ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPDLV-" Then

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYPDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYPD-" Then

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Empty_Beams").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYYDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYYD-" Then

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weights").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            End If

        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        If Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "CSINV-" Then

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JEBDC-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "EBDLV-" Then

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPDLV-" Then

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYPDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYPD-" Then

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

        ElseIf Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "PYYDL-" Or Trim(prn_DetDt.Rows(0).Item("Entry_Id").ToString) = "JPYYD-" Then

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

        End If

       
        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "Vechile No. " & prn_HdDt.Rows(0).Item("Vechile_No"), LMargin + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Driver Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_SendTo, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
  
    Private Sub cbo_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SendTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SendTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SendTo, msk_Date, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SendTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SendTo, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        
    End Sub
    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SendTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SendTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_VechileNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VechileNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VechileNo, cbo_Transport, txt_Remarks, "Gate_Pass_Head", "Vechile_No", "", "")

    End Sub

    Private Sub cbo_VechileNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VechileNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VechileNo, txt_Remarks, "Gate_Pass_Head", "Vechile_No", "", "", False)
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()

            Else
                msk_Date.Focus()

            End If

            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Asc(e.KeyChar) = 13 Then
                If MessageBox.Show("Do you want to select Delivery", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()

                    Else
                        msk_Date.Focus()

                    End If

                    'btn_Save.Focus()

                End If

            End If
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SendTo.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SendTo.Enabled And cbo_SendTo.Visible Then cbo_SendTo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            '----- EmptyBeam from Empty_BeamBagCone_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.* from Empty_BeamBagCone_Delivery_Head a  where a.Gate_Pass_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Empty_BeamBagCone_Delivery_Date, a.for_orderby, a.Empty_BeamBagCone_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Empty_BeamBagCone_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = "EmptyBeams"
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Empty_Beam").ToString
                    .Rows(n).Cells(10).Value = "1"
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Empty_BeamBagCone_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "EBDLV-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- EmptyBeam from Empty_BeamBagCone_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.* from Empty_BeamBagCone_Delivery_Head a  where a.Gate_Pass_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Empty_BeamBagCone_Delivery_Date, a.for_orderby, a.Empty_BeamBagCone_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Empty_BeamBagCone_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = "EmptyBeams"
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Empty_Beam").ToString
                    .Rows(n).Cells(10).Value = ""
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Empty_BeamBagCone_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "EBDLV-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- EmptyBeam from JobWork_Empty_BeamBagCone_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.* from JobWork_Empty_BeamBagCone_Delivery_Head a  where a.Gate_Pass_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.JobWork_Empty_BeamBagCone_Delivery_Date, a.for_orderby, a.JobWork_Empty_BeamBagCone_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Empty_BeamBagCone_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = "EmptyBeams"
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Empty_Beam").ToString
                    .Rows(n).Cells(10).Value = "1"
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("JobWork_Empty_BeamBagCone_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "JEBDC-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- EmptyBeam from JobWork_Empty_BeamBagCone_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.* from JobWork_Empty_BeamBagCone_Delivery_Head a  where a.Gate_Pass_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.JobWork_Empty_BeamBagCone_Delivery_Date, a.for_orderby, a.JobWork_Empty_BeamBagCone_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Empty_BeamBagCone_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = "EmptyBeams"
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Empty_Beam").ToString
                    .Rows(n).Cells(10).Value = ""
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("JobWork_Empty_BeamBagCone_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "JEBDC-"
                Next

            End If
            Dt1.Clear()

            '----- Fabric from JobWork_Piece_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from JobWork_Piece_Delivery_Head a LEFT OUTER JOIN Cloth_Head b ON  b.cLOTH_IdNo = a.cLOTH_IdNo  where a.Gate_Pass_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.JobWork_Piece_Delivery_Date, a.for_orderby, a.JobWork_Piece_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Total_Rolls").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Total_Delivery_Meters").ToString)
                    .Rows(n).Cells(10).Value = "1"
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "JPDLV-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- Fabric from JobWork_Piece_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from JobWork_Piece_Delivery_Head a LEFT OUTER JOIN Cloth_Head b ON b.cLOTH_IdNo = a.cLOTH_IdNo  where a.Gate_Pass_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.JobWork_Piece_Delivery_Date, a.for_orderby, a.JobWork_Piece_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Total_Rolls").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Total_Delivery_Meters").ToString)
                    .Rows(n).Cells(10).Value = ""
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "JPDLV-"

                Next

            End If
            Dt1.Clear()

            '----- Fabric from ClothSales_Invoice_Head
            Da = New SqlClient.SqlDataAdapter("select a.* , c.Cloth_Name  from ClothSales_Invoice_Head a INNER JOIN ClothSales_Invoice_Details b ON A.ClothSales_Invoice_Code = B.ClothSales_Invoice_Code LEFT OUTER JOIN Cloth_Head c ON c.cLOTH_IdNo  IN (select Top 1 cLOTH_IdNo from ClothSales_Invoice_Details z1 where z1.ClothSales_Invoice_Code = a.ClothSales_Invoice_Code ) where a.Gate_Pass_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Invoice_Date, a.for_orderby, a.ClothSales_Invoice_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Invoice_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Invoice_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Total_bALES").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Pcs").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                    .Rows(n).Cells(10).Value = "1"
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("ClothSales_Invoice_Code").ToString
                    .Rows(n).Cells(13).Value = "CSINV-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- Fabric from ClothSales_Invoice_Head
            Da = New SqlClient.SqlDataAdapter("select a.* , c.Cloth_Name  from ClothSales_Invoice_Head a INNER JOIN ClothSales_Invoice_Details b ON A.ClothSales_Invoice_Code = B.ClothSales_Invoice_Code LEFT OUTER JOIN Cloth_Head c ON c.cLOTH_IdNo  IN (select Top 1 cLOTH_IdNo from ClothSales_Invoice_Details z1 where z1.ClothSales_Invoice_Code = a.ClothSales_Invoice_Code ) where a.Gate_Pass_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Invoice_Date, a.for_orderby, a.ClothSales_Invoice_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Invoice_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Invoice_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Total_bALES").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Pcs").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                    .Rows(n).Cells(10).Value = ""
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("ClothSales_Invoice_Code").ToString
                    .Rows(n).Cells(13).Value = "CSINV-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()


            '----- Pavu from PavuYarn_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.* , c.EndsCount_Name from PavuYarn_Delivery_Head a  INNER JOIN Pavu_Delivery_Details d On a.PavuYarn_Delivery_Code = d.PavuYarn_Delivery_Code INNER JOIN Stock_SizedPavu_Processing_Details b ON d.Set_Code = b.Set_Code and d.Beam_No = b.Beam_No  LEFT OUTER JOIN EndsCount_Head c ON  c.EndsCount_IdNo In (select Top 1 EndsCount_IdNo from Stock_SizedPavu_Processing_Details z1 where d.Set_Code = z1.Set_Code and d.Beam_No = z1.Beam_No ) where a.PavuGate_Pass_Code = '" & Trim(NewCode) & "' and a.DeliveryTo_Idno = " & Str(Val(LedIdNo)) & " order by a.PavuYarn_Delivery_Date, a.for_orderby, a.PavuYarn_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("PavuYarn_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Beam").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Total_Meters").ToString)
                    .Rows(n).Cells(10).Value = "1"
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("PavuYarn_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "PYPDL-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- Pavu from PavuYarn_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.*, c.EndsCount_Name from PavuYarn_Delivery_Head a  INNER JOIN Pavu_Delivery_Details d On a.PavuYarn_Delivery_Code = d.PavuYarn_Delivery_Code INNER JOIN Stock_SizedPavu_Processing_Details b ON d.Set_Code = b.Set_Code and d.Beam_No = b.Beam_No  LEFT OUTER JOIN EndsCount_Head c ON  c.EndsCount_IdNo In (select Top 1 EndsCount_IdNo from Stock_SizedPavu_Processing_Details z1 where d.Set_Code = z1.Set_Code and d.Beam_No = z1.Beam_No ) where a.PavuGate_Pass_Code = '' and a.DeliveryTo_Idno = " & Str(Val(LedIdNo)) & " order by a.PavuYarn_Delivery_Date, a.for_orderby, a.PavuYarn_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("PavuYarn_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Beam").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Total_Meters").ToString)
                    .Rows(n).Cells(10).Value = ""
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("PavuYarn_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "PYPDL-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()


            '----- Yarn from PavuYarn_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.*, c.Count_Name from PavuYarn_Delivery_Head a INNER JOIN Yarn_Delivery_Details b ON A.PavuYarn_Delivery_Code = B.PavuYarn_Delivery_Code LEFT OUTER JOIN Count_Head c ON  c.Count_IdNo IN (select Top 1 Count_IdNo from Yarn_Delivery_Details z1 where z1.PavuYarn_Delivery_Code = a.PavuYarn_Delivery_Code )  where a.YarnGate_Pass_Code = '" & Trim(NewCode) & "' and a.DeliveryTo_Idno = " & Str(Val(LedIdNo)) & " order by a.PavuYarn_Delivery_Date, a.for_orderby, a.PavuYarn_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("PavuYarn_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(8).Value = Val(Dt1.Rows(i).Item("Total_Bags").ToString)
                    .Rows(n).Cells(9).Value = Val(Dt1.Rows(i).Item("Total_Weight").ToString)
                    .Rows(n).Cells(10).Value = "1"
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("PavuYarn_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "PYYDL-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- Yarn from PavuYarn_Delivery_Head
            Da = New SqlClient.SqlDataAdapter("select a.*, c.Count_Name from PavuYarn_Delivery_Head a INNER JOIN Yarn_Delivery_Details b ON A.PavuYarn_Delivery_Code = B.PavuYarn_Delivery_Code LEFT OUTER JOIN Count_Head c ON  c.Count_IdNo IN (select Top 1 Count_IdNo from Yarn_Delivery_Details z1 where z1.PavuYarn_Delivery_Code = a.PavuYarn_Delivery_Code )  where a.YarnGate_Pass_Code = '' and a.DeliveryTo_Idno = " & Str(Val(LedIdNo)) & " order by a.PavuYarn_Delivery_Date, a.for_orderby, a.PavuYarn_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("PavuYarn_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(8).Value = Val(Dt1.Rows(i).Item("Total_Bags").ToString)
                    .Rows(n).Cells(9).Value = Val(Dt1.Rows(i).Item("Total_Weight").ToString)
                    .Rows(n).Cells(10).Value = ""
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("PavuYarn_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = "PYYDL-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- Yarn from JobWork_PavuYarn_Return_Head 
            Da = New SqlClient.SqlDataAdapter("select a.*, c.Count_Name from JobWork_PavuYarn_Return_Head a INNER JOIN JobWork_Yarn_Return_Details b ON A.JobWork_PavuYarn_Return_Code = B.JobWork_PavuYarn_Return_Code LEFT OUTER JOIN Count_Head c ON  c.Count_IdNo IN (select Top 1 Count_IdNo from JobWork_Yarn_Return_Details z1 where z1.JobWork_PavuYarn_Return_Code = a.JobWork_PavuYarn_Return_Code )  where a.YarnGate_Pass_Code = '" & Trim(NewCode) & "' and a.Ledger_IdNo = " & Str(Val(LedIdNo)) & " order by a.JobWork_PavuYarn_Return_Date, a.for_orderby, a.JobWork_PavuYarn_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_PavuYarn_Return_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_PavuYarn_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(8).Value = Val(Dt1.Rows(i).Item("Total_Bags").ToString)
                    .Rows(n).Cells(9).Value = Val(Dt1.Rows(i).Item("Total_Weight").ToString)
                    .Rows(n).Cells(10).Value = "1"
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("JobWork_PavuYarn_Return_Code").ToString
                    .Rows(n).Cells(13).Value = "JPYYD-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- Yarn from JobWork_PavuYarn_Return_Head 
            Da = New SqlClient.SqlDataAdapter("select a.*, c.Count_Name from JobWork_PavuYarn_Return_Head a INNER JOIN JobWork_Yarn_Return_Details b ON A.JobWork_PavuYarn_Return_Code = B.JobWork_PavuYarn_Return_Code LEFT OUTER JOIN Count_Head c ON  c.Count_IdNo IN (select Top 1 Count_IdNo from JobWork_Yarn_Return_Details z1 where z1.JobWork_PavuYarn_Return_Code = a.JobWork_PavuYarn_Return_Code )  where a.YarnGate_Pass_Code = '' and a.lEDGER_Idno = " & Str(Val(LedIdNo)) & " order by a.JobWork_PavuYarn_Return_Date, a.for_orderby, a.JobWork_PavuYarn_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_PavuYarn_Return_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_PavuYarn_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(8).Value = Val(Dt1.Rows(i).Item("Total_Bags").ToString)
                    .Rows(n).Cells(9).Value = Val(Dt1.Rows(i).Item("Total_Weight").ToString)
                    .Rows(n).Cells(10).Value = ""
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("JobWork_PavuYarn_Return_Code").ToString
                    .Rows(n).Cells(13).Value = "JPYYD-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- Pavu from JobWork_PavuYarn_Return_Head 
            Da = New SqlClient.SqlDataAdapter("select a.* , c.EndsCount_Name from JobWork_PavuYarn_Return_Head a INNER JOIN JobWork_Pavu_Return_Details B On a.JobWork_PavuYarn_Return_Code = B.JobWork_PavuYarn_Return_Code  LEFT OUTER JOIN EndsCount_Head c ON  c.EndsCount_IdNo In (select Top 1 EndsCount_IdNo from JobWork_Pavu_Return_Details z1 where a.JobWork_PavuYarn_Return_Code = z1.JobWork_PavuYarn_Return_Code ) where a.PavuGate_Pass_Code = '" & Trim(NewCode) & "' and a.Ledger_IdNo = " & Str(Val(LedIdNo)) & " order by a.JobWork_PavuYarn_Return_Date, a.for_orderby, a.JobWork_PavuYarn_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_PavuYarn_Return_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_PavuYarn_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Beam").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Total_Meters").ToString)
                    .Rows(n).Cells(10).Value = "1"
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("JobWork_PavuYarn_Return_Code").ToString
                    .Rows(n).Cells(13).Value = "JPYPD-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            '----- Pavu from JobWork_PavuYarn_Return_Head 
            Da = New SqlClient.SqlDataAdapter("select a.* , c.EndsCount_Name from JobWork_PavuYarn_Return_Head a INNER JOIN JobWork_Pavu_Return_Details B On a.JobWork_PavuYarn_Return_Code = B.JobWork_PavuYarn_Return_Code  LEFT OUTER JOIN EndsCount_Head c ON  c.EndsCount_IdNo In (select Top 1 EndsCount_IdNo from JobWork_Pavu_Return_Details z1 where a.JobWork_PavuYarn_Return_Code = z1.JobWork_PavuYarn_Return_Code ) where a.PavuGate_Pass_Code = '' and a.Ledger_IdNo = " & Str(Val(LedIdNo)) & " order by a.JobWork_PavuYarn_Return_Date, a.for_orderby, a.JobWork_PavuYarn_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_PavuYarn_Return_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_PavuYarn_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Beam").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Total_Meters").ToString)
                    .Rows(n).Cells(10).Value = "1"
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("JobWork_PavuYarn_Return_Code").ToString
                    .Rows(n).Cells(13).Value = "JPYPD-"

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()
        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Delivery(e.RowIndex)
    End Sub

    Private Sub Select_Delivery(ByVal RwIndx As Integer)

        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(10).Value = (Val(.Rows(RwIndx).Cells(10).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(10).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(10).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

                'Close_Delivery_Selection()

            End If

        End With
    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Delivery(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Delivery_Selection()
    End Sub

    Private Sub Close_Delivery_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(10).Value) = 1 Then

                lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(12).Value


                If dgv_Selection.Rows(i).Cells(13).Value = "CSINV-" Then
                    '--
                    Da1 = New SqlClient.SqlDataAdapter("select c.Cloth_Name , b.Bales , b.Pcs , b.Meters from ClothSales_Invoice_Details b LEFT OUTER JOIN Cloth_Head c ON c.cLOTH_IdNo = b.cLOTH_IdNo where b.ClothSales_Invoice_Code = '" & Trim(dgv_Selection.Rows(i).Cells(12).Value) & "' order by b.sl_no", con)
                    Dt1 = New DataTable
                    Da1.Fill(Dt1)

                    If Dt1.Rows.Count > 0 Then

                        For j = 0 To Dt1.Rows.Count - 1

                            n = dgv_Details.Rows.Add()
                            sno = sno + 1
                            dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                            dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                            dgv_Details.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("Cloth_Name").ToString
                            dgv_Details.Rows(n).Cells(4).Value = Dt1.Rows(j).Item("Bales").ToString
                            If Val(dgv_Details.Rows(n).Cells(4).Value) = 0 Then dgv_Details.Rows(n).Cells(4).Value = ""

                            dgv_Details.Rows(n).Cells(5).Value = Dt1.Rows(j).Item("Pcs").ToString
                            If Val(dgv_Details.Rows(n).Cells(5).Value) = 0 Then dgv_Details.Rows(n).Cells(5).Value = ""

                            dgv_Details.Rows(n).Cells(6).Value = Dt1.Rows(j).Item("Meters").ToString
                            If Val(dgv_Details.Rows(n).Cells(6).Value) = 0 Then dgv_Details.Rows(n).Cells(6).Value = ""

                            dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(13).Value
                            dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(12).Value

                        Next

                    End If
                    Dt1.Clear()

                ElseIf dgv_Selection.Rows(i).Cells(13).Value = "PYYDL-" Then
                    '--
                    Da1 = New SqlClient.SqlDataAdapter("select c.Count_Name , b.Bags , b.Weight from Yarn_Delivery_Details b LEFT OUTER JOIN Count_Head c ON c.Count_IdNo = b.Count_IdNo where b.PavuYarn_Delivery_Code = '" & Trim(dgv_Selection.Rows(i).Cells(12).Value) & "' order by b.sl_no", con)
                    Dt1 = New DataTable
                    Da1.Fill(Dt1)

                    If Dt1.Rows.Count > 0 Then

                        For j = 0 To Dt1.Rows.Count - 1

                            n = dgv_Details.Rows.Add()
                            sno = sno + 1
                            dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                            dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                            dgv_Details.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("Count_Name").ToString
                            dgv_Details.Rows(n).Cells(7).Value = Dt1.Rows(j).Item("Bags").ToString
                            If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then dgv_Details.Rows(n).Cells(7).Value = ""

                            dgv_Details.Rows(n).Cells(8).Value = Dt1.Rows(j).Item("Weight").ToString
                            If Val(dgv_Details.Rows(n).Cells(8).Value) = 0 Then dgv_Details.Rows(n).Cells(8).Value = ""

                            dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(13).Value
                            dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(12).Value

                        Next

                    End If
                    Dt1.Clear()

                ElseIf dgv_Selection.Rows(i).Cells(13).Value = "JPYYD-" Then
                    '--
                    Da1 = New SqlClient.SqlDataAdapter("select c.Count_Name , b.Bags , b.Weight from JobWork_Yarn_Return_Details b LEFT OUTER JOIN Count_Head c ON c.Count_IdNo = b.Count_IdNo where b.JobWork_PavuYarn_Return_Code = '" & Trim(dgv_Selection.Rows(i).Cells(12).Value) & "' order by b.sl_no", con)
                    Dt1 = New DataTable
                    Da1.Fill(Dt1)

                    If Dt1.Rows.Count > 0 Then

                        For j = 0 To Dt1.Rows.Count - 1

                            n = dgv_Details.Rows.Add()
                            sno = sno + 1
                            dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                            dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                            dgv_Details.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("Count_Name").ToString

                            dgv_Details.Rows(n).Cells(7).Value = Dt1.Rows(j).Item("Bags").ToString
                            If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then dgv_Details.Rows(n).Cells(7).Value = ""

                            dgv_Details.Rows(n).Cells(8).Value = Dt1.Rows(j).Item("Weight").ToString
                            If Val(dgv_Details.Rows(n).Cells(8).Value) = 0 Then dgv_Details.Rows(n).Cells(8).Value = ""

                            dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(13).Value
                            dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(12).Value

                        Next

                    End If
                    Dt1.Clear()

                Else

                    n = dgv_Details.Rows.Add()
                    sno = sno + 1
                    dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                    dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                    dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value

                    dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                    If Val(dgv_Details.Rows(n).Cells(3).Value) = 0 Then dgv_Details.Rows(n).Cells(3).Value = ""

                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                    If Val(dgv_Details.Rows(n).Cells(4).Value) = 0 Then dgv_Details.Rows(n).Cells(4).Value = ""

                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                    If Val(dgv_Details.Rows(n).Cells(5).Value) = 0 Then dgv_Details.Rows(n).Cells(5).Value = ""

                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                    If Val(dgv_Details.Rows(n).Cells(6).Value) = 0 Then dgv_Details.Rows(n).Cells(6).Value = ""

                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                    If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then dgv_Details.Rows(n).Cells(7).Value = ""

                    dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                    If Val(dgv_Details.Rows(n).Cells(8).Value) = 0 Then dgv_Details.Rows(n).Cells(8).Value = ""

                    dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(13).Value
                    dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(12).Value

                End If

            End If
            Dt1.Clear()

        Next

        TotalQuantity_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            save_record()

        Else
            dtp_date.Focus()

        End If

        'If dgv_Details.Enabled And dgv_Details.Visible Then
        '    If dgv_Details.Rows.Count > 0 Then
        '        btn_Save.Focus()
        '    Else
        '        dtp_date.Focus()
        '    End If
        'End If

    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged

        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

  
End Class
