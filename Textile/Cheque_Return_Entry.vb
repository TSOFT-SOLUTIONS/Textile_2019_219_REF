Public Class Cheque_Return_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CHQRT-"
    Private Pk_Condition2 As String = "CRAGC-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1

        lbl_ReturnNo.Text = ""
        lbl_ReturnNo.ForeColor = Color.Black
        msk_ReturnDate.Text = ""
        dtp_ReturnDate.Text = ""

        cbo_Ledger.Text = ""

        cbo_BankName.Text = ""

        cbo_ChequeNo.Text = ""
        txt_Narration.Text = ""
        cbo_Filter_PartyName.Text = ""
        cbo_Filter_BankName.Text = ""
        cbo_Filter_ChequeNo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()
        dgv_Details_Total.Rows.Clear()
        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_BankName.Text = ""
            cbo_Filter_BankName.SelectedIndex = -1
            cbo_Filter_ChequeNo.Text = ""
            cbo_Filter_ChequeNo.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
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
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectionStart = 0
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Debtor_Name from Cheque_Return_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Debtor_IdNo = c.Ledger_IdNo  Where a.Cheque_Return_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_ReturnNo.Text = dt1.Rows(0).Item("Cheque_Return_No").ToString
                dtp_ReturnDate.Text = dt1.Rows(0).Item("Cheque_Return_Date").ToString
                msk_ReturnDate.Text = dtp_ReturnDate.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_BankName.Text = dt1.Rows(0).Item("Debtor_Name").ToString
                cbo_ChequeNo.Text = Val(dt1.Rows(0).Item("Cheque_No").ToString)
                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.debit_amount, c.Ledger_name as Agent_Name from Cheque_Return_Details a INNER JOIN voucher_bill_head b ON a.Voucher_Bill_Code = b.Voucher_Bill_Code LEFT OUTER JOIN Ledger_Head c ON a.Agent_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cheque_Return_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Party_Bill_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("debit_amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Receipt_Amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(3).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(3).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("discount_amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(4).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(4).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("rate_difference").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(5).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(5).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("dd_commission").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(6).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(6).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Others").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(7).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("AgentComm_Percentage").ToString)
                        If Val(dgv_Details.Rows(n).Cells(8).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(8).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("AgentComm_Amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(9).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(9).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Agent_Tds_Percentage").ToString)
                        If Val(dgv_Details.Rows(n).Cells(10).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(10).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Agent_tds_Amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(11).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(11).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Voucher_Bill_Code").ToString
                        dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("Agent_Name").ToString

                    Next i

                End If
                dt2.Clear()

                Total_Calculation()

            End If
            dt1.Clear()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()
            If msk_ReturnDate.Visible And msk_ReturnDate.Enabled Then msk_ReturnDate.Focus()

        End Try

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub Cheque_Return_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BankName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BankName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
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
            '---MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            FrmLdSTS = False

        End Try

    End Sub

    Private Sub Cheque_Return_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Condt As String

        Me.Text = ""

        con.Open()

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_Ledger, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_BankName, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6)", "(Ledger_IdNo = 0)")

        Condt = " (Cheque_No <> '' and Cheque_Return_Code = '') "
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_ChequeNo, con, "Party_Amount_Receipt_Head", "Cheque_No", Condt, "(Cheque_No = '')")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_ReturnDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BankName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_BankName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_ChequeNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ChequeNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_ReturnDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BankName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_BankName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ChequeNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ChequeNo.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_ReturnDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_ReturnDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Cheque_Return_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Cheque_Return_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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
            'MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 7 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                cbo_BankName.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(4)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 9 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 4 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Ledger.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 7)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 11 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 2)

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult

        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ReturnNo.Text)

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cheque_Return_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cheque_Return_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Cheque_return_Entry, New_Entry, Me, con, "Cheque_Return_Head", "Cheque_Return_Code", NewCode, "Cheque_Return_Date", "(Cheque_Return_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Cheque_Return_Head", "Cheque_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_ReturnNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Cheque_Return_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Cheque_Return_Details", "Cheque_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_ReturnNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Party_Bill_No , Receipt_Amount , discount_amount  ,  rate_difference    , dd_commission , Others, AgentComm_Percentage,AgentComm_Amount ,  Agent_Tds_Percentage,Agent_tds_Amount ,  Voucher_Bill_Code ,Agent_IdNo , Total_Receipt_Amount_Amount ", "Sl_No", "Cheque_Return_Code, For_OrderBy, Company_IdNo, Cheque_Return_No, Cheque_Return_Date, Ledger_Idno", trans)

            cmd.CommandText = "update Party_Amount_Receipt_Head set Cheque_Return_Code = '', Cheque_Return_Increment = Cheque_Return_Increment - 1 where Cheque_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.voucher_bill_code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "delete from Cheque_Return_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cheque_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_ReturnDate.Enabled = True And msk_ReturnDate.Visible = True Then msk_ReturnDate.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable
            Dim condt As String
            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_Filter_BankName, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6)", "(Ledger_IdNo = 0)")

            Condt = " (Cheque_No <> '' and Cheque_Return_Code = '') "
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_Filter_ChequeNo, con, "Party_Amount_Receipt_Head", "Cheque_No", condt, "(Cheque_No = '')")

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_BankName.Text = ""

            cbo_Filter_BankName.SelectedIndex = -1
            cbo_Filter_ChequeNo.Text = ""

            cbo_Filter_ChequeNo.SelectedIndex = -1


            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        pnl_Filter.BringToFront()
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Cheque_Return_No from Cheque_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cheque_Return_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReturnNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cheque_Return_No from Cheque_Return_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cheque_Return_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReturnNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cheque_Return_No from Cheque_Return_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cheque_Return_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Cheque_Return_No from Cheque_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cheque_Return_No desc", con)
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

            lbl_ReturnNo.Text = Common_Procedures.get_MaxCode(con, "Cheque_Return_Head", "Cheque_Return_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_ReturnNo.ForeColor = Color.Red


            msk_ReturnDate.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Cheque_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cheque_Return_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Cheque_Return_Date").ToString <> "" Then msk_ReturnDate.Text = dt1.Rows(0).Item("Cheque_Return_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_ReturnDate.Enabled And msk_ReturnDate.Visible Then
                msk_ReturnDate.Focus()
                msk_ReturnDate.SelectionStart = 0
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
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Return.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cheque_Return_No from Cheque_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Return No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cheque_Return_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cheque_Return_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Cheque_return_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Return No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cheque_Return_No from Cheque_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Return No", "DOES NOT INSERT NEW RETURN ENTRY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ReturnNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RETURN ENTRY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotRcpt As Single, vtotCashDis As Single, vtotRateDiff As Single, vtotDDComm As Single, vtotOthr As Single, vTotAgComAmt As Single, vTottdsAmt As Single
        Dim i As Integer = 0
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Deb_ID As Integer = 0
        Dim AgtIdNo As Integer = 0
        Dim AcPosAgIdNo As Integer = 0
        Dim acgrp_idno As Integer = 0
        Dim DupChqNo As String = ""
        Dim Narr As String = ""
        Dim RecAmt As Single = 0
        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ReturnNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '   If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cheque_Return_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Cheque_return_Entry, New_Entry, Me, con, "Cheque_Return_Head", "Cheque_Return_Code", NewCode, "Cheque_Return_Date", "(Cheque_Return_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Cheque_Return_No desc", dtp_ReturnDate.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_ReturnDate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_ReturnDate.Enabled And msk_ReturnDate.Visible Then msk_ReturnDate.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_ReturnDate.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_ReturnDate.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_ReturnDate.Enabled And msk_ReturnDate.Visible Then msk_ReturnDate.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Trim(cbo_ChequeNo.Text) = "" Then
            MessageBox.Show("Invalid Cheque No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ChequeNo.Enabled And cbo_ChequeNo.Visible Then cbo_ChequeNo.Focus()
            Exit Sub
        End If

        If Trim(lbl_RecNo.Text) = "" Then
            MessageBox.Show("Invalid Amount Receipt Details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ChequeNo.Enabled And cbo_ChequeNo.Visible Then cbo_ChequeNo.Focus()
            Exit Sub
        End If

        Deb_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_BankName.Text)
        If Deb_ID = 0 Then
            MessageBox.Show("Invalid Bank Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_BankName.Enabled And cbo_BankName.Visible Then cbo_BankName.Focus()
            Exit Sub
        End If
        lbl_UserName.Text = Common_Procedures.User.IdNo
        Total_Calculation()

        vTotRcpt = 0 : vtotCashDis = 0 : vtotRateDiff = 0 : vtotDDComm = 0 : vtotOthr = 0 : vTotAgComAmt = 0 : vTottdsAmt = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotRcpt = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vtotCashDis = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vtotRateDiff = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vtotDDComm = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vtotOthr = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotAgComAmt = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
            vTottdsAmt = Val(dgv_Details_Total.Rows(0).Cells(11).Value())
        End If

        If (vTotRcpt + vtotCashDis + vtotRateDiff + vtotDDComm + vtotOthr) = 0 Then
            MessageBox.Show("Invalid Cheque Amount..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_Details.Rows.Count > 0 Then
                If dgv_Details.Enabled Then dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
            End If
            Exit Sub
        End If

        tr = con.BeginTransaction

        'Try

        If Insert_Entry = True Or New_Entry = False Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Else

            lbl_ReturnNo.Text = Common_Procedures.get_MaxCode(con, "Cheque_Return_Head", "Cheque_Return_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        End If

        cmd.Connection = con
        cmd.Transaction = tr

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_ReturnDate.Text))

        cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        cmd.ExecuteNonQuery()

        If New_Entry = True Then
            cmd.CommandText = "Insert into Cheque_Return_Head ( Cheque_Return_Code, Company_IdNo, Cheque_Return_No, for_OrderBy, Cheque_Return_Date, Ledger_IdNo, Debtor_IdNo, Cheque_No, Narration, Total_Receipt_Amount, Total_Discount_Amount, Total_RateDifference,Total_DDComm, Total_Others, Total_AgentComm_Amount, Total_Tds_Amount, User_Idno, Party_Receipt_Code, Party_Receipt_No ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReturnNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ReturnNo.Text))) & ", @EntryDate, " & Str(Val(Led_ID)) & ", " & Str(Val(Deb_ID)) & ", '" & Trim(cbo_ChequeNo.Text) & "',  '" & Trim(txt_Narration.Text) & "'," & Str(Val(vTotRcpt)) & " , " & Str(Val(vtotCashDis)) & ", " & Str(Val(vtotRateDiff)) & " , " & Val(vtotDDComm) & " , " & Val(vtotOthr) & " , " & Val(vTotAgComAmt) & ", " & Val(vTottdsAmt) & ", " & Str(Val(Common_Procedures.User.IdNo)) & ", '" & Trim(lbl_RecCode.Text) & "', '" & Trim(lbl_RecNo.Text) & "' )"
            cmd.ExecuteNonQuery()

        Else


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Cheque_Return_Head", "Cheque_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_ReturnNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Cheque_Return_Code, Company_IdNo, for_OrderBy", tr)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Cheque_Return_Details", "Cheque_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_ReturnNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Party_Bill_No , Receipt_Amount , discount_amount  ,  rate_difference    , dd_commission , Others, AgentComm_Percentage,AgentComm_Amount ,  Agent_Tds_Percentage,Agent_tds_Amount ,  Voucher_Bill_Code ,Agent_IdNo , Total_Receipt_Amount_Amount ", "Sl_No", "Cheque_Return_Code, For_OrderBy, Company_IdNo, Cheque_Return_No, Cheque_Return_Date, Ledger_Idno", tr)


            cmd.CommandText = "Update Cheque_Return_Head set Cheque_Return_Date = @EntryDate, Ledger_IdNo = " & Val(Led_ID) & ", Debtor_IdNo = " & Val(Deb_ID) & ", Cheque_No = '" & Trim(cbo_ChequeNo.Text) & "', Narration = '" & Trim(txt_Narration.Text) & "', Total_Receipt_Amount = " & Val(vTotRcpt) & ", Total_Discount_Amount = " & Val(vtotCashDis) & ",Total_RateDifference = " & Val(vtotRateDiff) & " , Total_DDComm = " & Val(vtotDDComm) & " ,Total_Others = " & Val(vtotOthr) & " , Total_AgentComm_Amount = " & Val(vTotAgComAmt) & " ,Total_Tds_Amount = " & Val(vTottdsAmt) & ", User_Idno = " & Str(Val(Common_Procedures.User.IdNo)) & ", Party_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "', Party_Receipt_No = '" & Trim(lbl_RecNo.Text) & "'  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Party_Amount_Receipt_Head set Cheque_Return_Code = '', Cheque_Return_Increment = Cheque_Return_Increment - 1 where Cheque_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Name2, Currency1 ) select entry_identification, voucher_bill_code, amount from voucher_bill_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

        End If

        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Cheque_Return_Head", "Cheque_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_ReturnNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Cheque_Return_Code, Company_IdNo, for_OrderBy", tr)

        Nr = 0
        cmd.CommandText = "update Party_Amount_Receipt_Head set Cheque_Return_Code = '" & Trim(NewCode) & "', Cheque_Return_Increment = Cheque_Return_Increment + 1 where Ledger_IdNo = " & Str(Val(Led_ID)) & " and Cheque_No = '" & Trim(cbo_ChequeNo.Text) & "' and Party_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
        Nr = cmd.ExecuteNonQuery()
        If Nr <> 1 Then
            Throw New ApplicationException("Invalid ChequeNo - Mismatch of Party & ChequeNo")
            Exit Sub
        End If

        cmd.CommandText = "Delete from Cheque_Return_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Return_Code = '" & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        With dgv_Details
            Sno = 0
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Or Val(.Rows(i).Cells(9).Value) <> 0 Or Val(.Rows(i).Cells(11).Value) <> 0 Then

                    Sno = Sno + 1

                    AgtIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(13).Value, tr)
                    If Val(AcPosAgIdNo) = 0 Then
                        AcPosAgIdNo = AgtIdNo
                    End If

                    RecAmt = Val(.Rows(i).Cells(3).Value) + Val(.Rows(i).Cells(4).Value) + Val(.Rows(i).Cells(5).Value) + Val(.Rows(i).Cells(6).Value) + Val(.Rows(i).Cells(7).Value)

                    cmd.CommandText = "Insert into Cheque_Return_Details ( Cheque_Return_Code,    Company_IdNo       ,             Cheque_Return_No      ,                               for_OrderBy                                  , Cheque_Return_Date,            Ledger_Idno  ,         Debtor_Idno     ,            Sl_No     ,                    Party_Bill_No       ,          Receipt_Amount             ,         discount_amount                  ,                  rate_difference    ,                  dd_commission      ,              Others                      ,                      AgentComm_Percentage,             AgentComm_Amount                  ,              Agent_Tds_Percentage         ,              Agent_tds_Amount             ,                    Voucher_Bill_Code    ,     Agent_IdNo      , Total_Receipt_Amount_Amount  ) " & _
                                                " Values ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReturnNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ReturnNo.Text))) & ",        @EntryDate   , " & Str(Val(Led_ID)) & ", " & Str(Val(Deb_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Val(.Rows(i).Cells(3).Value) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Val(.Rows(i).Cells(5).Value) & ", " & Val(.Rows(i).Cells(6).Value) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " ," & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", '" & Trim(.Rows(i).Cells(12).Value) & "', " & Val(AgtIdNo) & ", " & Val(RecAmt) & "   ) "
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "insert into voucher_bill_details ( company_idno, Voucher_Bill_Code, Voucher_Bill_Date," _
                              & "Ledger_Idno, entry_identification, Amount, CrDr_Type ) values ( " & Str(Val(lbl_Company.Tag)) & ", '" _
                              & Trim(.Rows(i).Cells(12).Value) & "', @EntryDate, " _
                              & Str(Led_ID) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', " _
                              & Str(-1 * Val(RecAmt)) & ", 'DR' )"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Update voucher_bill_head set credit_amount = credit_amount - " & Str(Val(RecAmt)) & " where voucher_bill_code = '" & Trim(.Rows(i).Cells(12).Value) & "' and ledger_idno = " & Str(Val(Led_ID))
                    Nr = cmd.ExecuteNonQuery()
                    If Nr = 0 Then
                        Throw New ApplicationException("Invalid Bill Details - Bill No. " & Trim(.Rows(i).Cells(1).Value) & "")
                        Exit Sub
                    End If

                End If

            Next

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Cheque_Return_Details", "Cheque_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_ReturnNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Party_Bill_No , Receipt_Amount , discount_amount  ,  rate_difference    , dd_commission , Others, AgentComm_Percentage,AgentComm_Amount ,  Agent_Tds_Percentage,Agent_tds_Amount ,  Voucher_Bill_Code ,Agent_IdNo , Total_Receipt_Amount_Amount ", "Sl_No", "Cheque_Return_Code, For_OrderBy, Company_IdNo, Cheque_Return_No, Cheque_Return_Date, Ledger_Idno", tr)


        End With

        '---- value is stored in negative so a.credit_amount - b.amount instead of +
        cmd.CommandText = "Update voucher_bill_head set credit_amount = a.credit_amount - b.Currency1 from voucher_bill_head a, " & Trim(Common_Procedures.EntryTempTable) & " b where b.Name1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.name2"
        cmd.ExecuteNonQuery()

        '--- Accounts Posting
        Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)
        Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

        Narr = ""
        Narr = "CHEQUE RETURN, NO : " & Trim(cbo_ChequeNo.Text)
        If Trim(txt_Narration.Text) <> "" Then Narr = Narr & "   " & Trim(txt_Narration.Text)

        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
        vLed_IdNos = Led_ID & "|" & Deb_ID & "|" & Val(Common_Procedures.CommonLedger.CASH_DISCOUNT_Ac) & "|" & Val(Common_Procedures.CommonLedger.RATE_DIFFERENCE_Ac) & "|" & Val(Common_Procedures.CommonLedger.DD_COMMISSION_Ac) & "|" & Val(Common_Procedures.CommonLedger.Discount_Ac)
        vVou_Amts = -1 * Val(vTotRcpt + vtotCashDis + vtotRateDiff + vtotDDComm + vtotOthr) & "|" & Val(vTotRcpt) & "|" & Val(vtotCashDis) & "|" & Val(vtotRateDiff) & "|" & Val(vtotDDComm) & "|" & Val(vtotOthr)

        If Common_Procedures.Voucher_Updation(con, "Chq.Ret", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_ReturnNo.Text), Convert.ToDateTime(msk_ReturnDate.Text), Trim(Narr), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
            Throw New ApplicationException(ErrMsg)
        End If

        '--Agent Commission + TDS Posting
        If Val(AcPosAgIdNo) <> 0 And Val(vTotAgComAmt) <> 0 Then
            vLed_IdNos = Val(Common_Procedures.CommonLedger.Agent_Commission_Ac) & "|" & AcPosAgIdNo & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
            vVou_Amts = Val(vTotAgComAmt) & "|" & -1 * Val(vTotAgComAmt - vTottdsAmt) & "|" & -1 * Val(vTottdsAmt)

            If Common_Procedures.Voucher_Updation(con, "AgCm.ChqR", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_ReturnNo.Text), Convert.ToDateTime(msk_ReturnDate.Text), "Cheque Return, No. " & cbo_ChequeNo.Text & ", " & cbo_Ledger.Text, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

        End If

        tr.Commit()


        MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
            If New_Entry = True Then
                new_record()
            Else
                move_record(lbl_ReturnNo.Text)
            End If
        Else
            move_record(lbl_ReturnNo.Text)
        End If


        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally
        '    cmd.Dispose()
        '    Dt1.Dispose()
        '    Da.Dispose()
        '    tr.Dispose()
        '    If msk_ReturnDate.Enabled And msk_ReturnDate.Visible Then msk_ReturnDate.Focus()

        'End Try

    End Sub

    Private Sub Total_Calculation()
        Dim vBlAmt As Single, vTotRect As Single, vtotCash As Single, vtotRate As Single, vtotComm As Single, vtotOtrs As Single, vTotCommAmt As Single, vTottdsAmt As Single
        Dim i As Integer
        Dim Sno As Integer

        vBlAmt = 0 : vTotRect = 0 : vtotCash = 0 : vtotRate = 0 : vtotComm = 0 : vtotOtrs = 0 : vTotCommAmt = 0 : vTottdsAmt = 0
        Sno = 0

        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                Sno = Sno + 1

                .Rows(i).Cells(0).Value = Sno

                If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                    vBlAmt = vBlAmt + Val(dgv_Details.Rows(i).Cells(2).Value)
                    vTotRect = vTotRect + Val(dgv_Details.Rows(i).Cells(3).Value)
                    vtotCash = vtotCash + Val(dgv_Details.Rows(i).Cells(4).Value)
                    vtotRate = vtotRate + Val(dgv_Details.Rows(i).Cells(5).Value)
                    vtotComm = vtotComm + Val(dgv_Details.Rows(i).Cells(6).Value)
                    vtotOtrs = vtotOtrs + Val(dgv_Details.Rows(i).Cells(7).Value)
                    vTotCommAmt = vTotCommAmt + Val(dgv_Details.Rows(i).Cells(9).Value)
                    vTottdsAmt = vTottdsAmt + Val(dgv_Details.Rows(i).Cells(11).Value)

                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(2).Value = Format(Val(vBlAmt), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(3).Value = Format(Val(vTotRect), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(4).Value = Format(Val(vtotCash), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(5).Value = Format(Val(vtotRate), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(6).Value = Format(Val(vtotComm), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vtotOtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(9).Value = Format(Val(vTotCommAmt), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(11).Value = Format(Val(vTottdsAmt), "#########0.00")

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_ReturnDate, cbo_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

        'If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    If dgv_Details.Rows.Count > 0 Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

        '    Else
        '        cbo_BankName.Focus()

        '    End If
        'End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        'If Asc(e.KeyChar) = 13 Then

        '    If MessageBox.Show("Do you want to select Bill Details", "FOR BILL SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '        btn_Selection_Click(sender, e)

        '    Else

        '        If dgv_Details.Rows.Count > 0 Then
        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

        '        Else
        '            cbo_BankName.Focus()

        '        End If

        '    End If

        'End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_BankName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BankName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_BankName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BankName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BankName, cbo_ChequeNo, txt_Narration, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_idno = 0)")
        'If (e.KeyValue = 38 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
        '    If dgv_Details.Rows.Count > 0 Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

        '    Else
        '        cbo_ChequeNo.Focus()

        '    End If
        'End If
    End Sub

    Private Sub cbo_BankName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BankName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BankName, txt_Narration, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_BankName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BankName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BankName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ChequeNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ChequeNo.GotFocus
        Dim Condt As String = ""
        Dim Led_ID As Integer = 0
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Condt = " (company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_No <> '' and (Cheque_Return_Code = '' or Cheque_Return_Code = '" & Trim(NewCode) & "') ) "
        Condt = " ( " & Trim(Condt) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Party_Amount_Receipt_Head", "Cheque_No", Condt, "(Cheque_No = '')")

    End Sub

    Private Sub cbo_ChequeNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ChequeNo.KeyDown
        Dim Condt As String = ""
        Dim Led_ID As Integer = 0
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Condt = " (company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_No <> '' and (Cheque_Return_Code = '' or Cheque_Return_Code = '" & Trim(NewCode) & "') ) "
        Condt = " ( " & Trim(Condt) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " )"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ChequeNo, cbo_Ledger, cbo_BankName, "Party_Amount_Receipt_Head", "Cheque_No", Condt, "(Cheque_No = '')")

    End Sub

    Private Sub cbo_ChequeNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ChequeNo.KeyPress
        Dim Condt As String = ""
        Dim Led_ID As Integer = 0
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Condt = " (company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_No <> '' and (Cheque_Return_Code = '' or Cheque_Return_Code = '" & Trim(NewCode) & "') ) "
        Condt = " ( " & Trim(Condt) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " )"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ChequeNo, cbo_BankName, "Party_Amount_Receipt_Head", "Cheque_No", Condt, "(Cheque_No = '')")

        If Asc(e.KeyChar) = 13 Then
            Receipt_Selection()
        End If

    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    If .CurrentCell.ColumnIndex >= 2 And .CurrentCell.ColumnIndex <= 11 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                        Else
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                        End If
                    End If
                End If
            End With
        Catch ex As Exception
            '-----

        End Try


    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub


            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Then
                            Total_Calculation()
                        End If
                    End If
                End If
            End With
        Catch ex As Exception
            '------
        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        print_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Bak_IdNo As Integer, Chq_No As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Chq_No = 0
            Bak_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cheque_Return_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Cheque_Return_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cheque_Return_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_BankName.Text) <> "" Then
                Bak_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_BankName.Text)
            End If
            'If Trim(cbo_Filter_ChequeNo.Text) <> "" Then
            '    Chq_No = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_ChequeNo.Text)
            'End If
            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Bak_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Debtor_IdNo = " & Str(Val(Bak_IdNo))
            End If
            If Val(cbo_Filter_ChequeNo.Text) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Cheque_No = " & Str(Val(cbo_Filter_ChequeNo.Text))
            End If
            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,C.Ledger_Name as Debtor_Name from Cheque_Return_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Debtor_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cheque_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cheque_Return_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Cheque_Return_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cheque_Return_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Debtor_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Receipt").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Narration").ToString

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


    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_BankName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_BankName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_Filter_BankName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_BankName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_BankName, cbo_Filter_PartyName, cbo_Filter_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_BankName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_BankName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_BankName, cbo_Filter_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        Try
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Receipt_Selection()
        If cbo_ChequeNo.Enabled And cbo_ChequeNo.Visible Then cbo_ChequeNo.Focus()
    End Sub

    Private Sub Receipt_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String

        Try

            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            If LedIdNo = 0 Then
                MessageBox.Show("Invalid Party Name", "DOES NOT SELECT CHEQUE DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                Exit Sub
            End If

            If Trim(cbo_ChequeNo.Text) = "" Then
                MessageBox.Show("Invalid Cheque No.", "DOES NOT SELECT CHEQUE DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_ChequeNo.Enabled And cbo_ChequeNo.Visible Then cbo_ChequeNo.Focus()
                Exit Sub
            End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            With dgv_Details

                .Rows.Clear()
                lbl_RecCode.Text = ""
                lbl_RecNo.Text = ""

                SNo = 0

                Da = New SqlClient.SqlDataAdapter("Select a.*, b.*, c.*, d.ledger_name as bank_name, e.ledger_name as agent_name from Party_Amount_Receipt_Head a INNER JOIN Party_Amount_Receipt_Details b ON a.Company_Idno = b.Company_Idno and a.Party_Receipt_Code = b.Party_Receipt_Code INNER JOIN voucher_bill_head c ON b.Voucher_Bill_Code = c.Voucher_Bill_Code INNER JOIN Ledger_Head d ON a.Debtor_Idno = d.ledger_idno LEFT OUTER JOIN Ledger_Head e ON b.agent_idno = e.ledger_idno Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(LedIdNo)) & " and a.Cheque_No = '" & Trim(cbo_ChequeNo.Text) & "' and (a.Cheque_Return_Code = '' or a.Cheque_Return_Code = '" & Trim(NewCode) & "') order by b.sl_no", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    lbl_RecCode.Text = Dt1.Rows(i).Item("Party_Receipt_code").ToString
                    lbl_RecNo.Text = Dt1.Rows(i).Item("Party_Receipt_No").ToString
                    cbo_BankName.Text = Dt1.Rows(i).Item("bank_name").ToString

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Party_Bill_No").ToString
                        .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(i).Item("debit_amount").ToString), "#########0.00")
                        .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Amount").ToString), "#########0.00")
                        If Val(.Rows(n).Cells(3).Value) = 0 Then
                            .Rows(n).Cells(3).Value = ""
                        End If
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("discount_amount").ToString), "#########0.00")
                        If Val(.Rows(n).Cells(4).Value) = 0 Then
                            .Rows(n).Cells(4).Value = ""
                        End If
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("rate_difference").ToString), "#########0.00")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then
                            .Rows(n).Cells(5).Value = ""
                        End If
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("dd_commission").ToString), "#########0.00")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then
                            .Rows(n).Cells(6).Value = ""
                        End If
                        .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Others").ToString), "#########0.00")
                        If Val(.Rows(n).Cells(7).Value) = 0 Then
                            .Rows(n).Cells(7).Value = ""
                        End If
                        .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Agent_Comm_Percentage").ToString), "#########0.00")
                        If Val(.Rows(n).Cells(8).Value) = 0 Then
                            .Rows(n).Cells(8).Value = ""
                        End If
                        .Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(i).Item("Agent_Comm_Amount").ToString), "#########0.00")
                        If Val(.Rows(n).Cells(9).Value) = 0 Then
                            .Rows(n).Cells(9).Value = ""
                        End If
                        .Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(i).Item("Agent_Tds_Percentage").ToString), "#########0.00")
                        If Val(.Rows(n).Cells(10).Value) = 0 Then
                            .Rows(n).Cells(10).Value = ""
                        End If
                        .Rows(n).Cells(11).Value = Format(Val(Dt1.Rows(i).Item("Agent_tds_Amount").ToString), "#########0.00")
                        If Val(.Rows(n).Cells(11).Value) = 0 Then
                            .Rows(n).Cells(11).Value = ""
                        End If
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Voucher_Bill_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Agent_Name").ToString

                    Next

                End If

                Dt1.Clear()

                Total_Calculation()

                Grid_Cell_DeSelect()

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID RECEIPT SELECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_ReturnDate.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_ReturnDate.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Filter_ChequeNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ChequeNo.KeyDown
        Dim Condt As String
        Condt = " (Cheque_No <> '' and Cheque_Return_Code = '') "

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ChequeNo, cbo_Filter_BankName, btn_Filter_Show, "Party_Amount_Receipt_Head", "Cheque_No", Condt, "(Cheque_No = '')")
    End Sub

    Private Sub cbo_Filter_ChequeNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ChequeNo.KeyPress
        Dim Condt As String
        Condt = " (Cheque_No <> '' and Cheque_Return_Code = '') "

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ChequeNo, btn_Filter_Show, "Party_Amount_Receipt_Head", "Cheque_No", Condt, "(Cheque_No = '')")
    End Sub

    Private Sub msk_ReturnDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_ReturnDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_ReturnDate.Text = Date.Today
            msk_ReturnDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_ReturnDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_ReturnDate.KeyUp

        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_ReturnDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_ReturnDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_ReturnDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_ReturnDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If
    End Sub
    Private Sub msk_ReturnDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_ReturnDate.LostFocus

        If IsDate(msk_ReturnDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_ReturnDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_ReturnDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_ReturnDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_ReturnDate.Text)) >= 2000 Then
                    dtp_ReturnDate.Value = Convert.ToDateTime(msk_ReturnDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_ReturnDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_ReturnDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_ReturnDate.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_ReturnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ReturnDate.TextChanged
        If IsDate(dtp_ReturnDate.Text) = True Then

            msk_ReturnDate.Text = dtp_ReturnDate.Text
            msk_ReturnDate.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_ReturnDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_ReturnDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_ReturnDate.Text
            vmskSelStrt = msk_ReturnDate.SelectionStart
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReturnNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
End Class