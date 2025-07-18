Imports System.IO
Public Class Weaver_Payment_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "WEPAY-"
    Private Pk_Condition1 As String = "WEPDE-"
    Private PkCondition_WADVP As String = "WPADP-"
    Private PkCondition_WADVD As String = "WPADD-"
    Private PkCondition_WPTDS As String = "WPTDS-"
    Private PkCondition_WPFRT As String = "WPFRT-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1


    Private DeleteAll_STS As Boolean = False
    Private vSPEC_Keys As HashSet(Of Keys)()


    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_VouNo.Text = ""
        lbl_VouNo.ForeColor = Color.Black
        lbl_AdvBalance.Text = ""
        lbl_CoolyBalance.Text = ""
        lbl_Yarn.Text = ""
        lbl_Pavu.Text = ""
        lbl_EmptyBeam.Text = ""

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_Creditor.Text = ""
        Txt_ChequeNo.Text = ""
        cbo_TransferNeft.Text = ""

        txt_Tds.Text = ""
        lbl_Tds_Amount.Text = ""
        txt_Freight.Text = ""

        txt_Add_Amount.Text = ""
        txt_Less_Amount.Text = ""
        txt_DebitAmount.Text = ""


        cbo_Filter_CreditorName.Text = ""
        cbo_Filter_PartyName.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        lbl_PartyRecNo.Text = ""
        txt_PaidAmount.Text = ""
        txt_Narration.Text = ""

        txt_PartyRecNo.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CreditorName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CreditorName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        NoCalc_Status = False

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

    Private Sub Weaver_Payment_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Creditor.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Creditor.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Weaver_Payment_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Payment_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Weaver_Payment_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Me.Text = ""

        con.Open()

        dtp_Date.Text = ""
        msk_date.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then
            lbl_PaidAmount.Text = "Cooly Paid"
        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1273" Then
            lbl_PartyRecNo.Visible = False
            txt_PartyRecNo.Visible = True
        Else
            lbl_PartyRecNo.Visible = True
            txt_PartyRecNo.Visible = False
        End If


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Creditor.GotFocus, AddressOf ControlGotFocus

        AddHandler lbl_PartyRecNo.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_ChequeNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransferNeft.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PaidAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Add_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Less_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DebitAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tds.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Tds_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_CreditorName.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyRecNo.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Creditor.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DebitAmount.LostFocus, AddressOf ControlLostFocus

        AddHandler Txt_ChequeNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransferNeft.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_PartyRecNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PaidAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CreditorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Add_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Less_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tds.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Tds_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyRecNo.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PaidAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Add_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Less_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_DebitAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Tds.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Tds_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PaidAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Add_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Less_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_DebitAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tds.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Tds_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress


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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Payment_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Payment_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_VouNo.Text = dt1.Rows(0).Item("Weaver_Payment_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Payment_Date")
                msk_date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Creditor.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Creditor_IdNo").ToString))
                Txt_ChequeNo.Text = Val(dt1.Rows(0).Item("Cheque_No").ToString)
                cbo_TransferNeft.Text = Trim(dt1.Rows(0).Item("Transfer_Method").ToString)
                txt_PaidAmount.Text = Format(Val(dt1.Rows(0).Item("Paid_Amount").ToString), "#########0.00")
                txt_DebitAmount.Text = Format(Val(dt1.Rows(0).Item("Debit_Amount").ToString), "#########0.00")

                txt_Tds.Text = dt1.Rows(0).Item("Tds").ToString
                lbl_Tds_Amount.Text = dt1.Rows(0).Item("Tds_Amount").ToString
                txt_Freight.Text = dt1.Rows(0).Item("Freight").ToString

                txt_Add_Amount.Text = Format(Val(dt1.Rows(0).Item("ADD_Amount").ToString), "#########0.00")
                txt_Less_Amount.Text = Format(Val(dt1.Rows(0).Item("LESS_Amount").ToString), "#########0.00")

                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                lbl_PartyRecNo.Text = Val(dt1.Rows(0).Item("Party_Rec_No").ToString)
                txt_PartyRecNo.Text = lbl_PartyRecNo.Text
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_VouNo.Text)

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Packing, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Packing, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_payment_Entry, New_Entry, Me, con, "Weaver_Payment_Head", "Weaver_Payment_Code", NewCode, "Weaver_Payment_Date", "(Weaver_Payment_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If DeleteAll_STS <> True Then

            If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

            If New_Entry = True Then
                MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Payment_Head", "Weaver_Payment_Code", Val(lbl_Company.Tag), NewCode, lbl_VouNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_Payment_Code, Company_IdNo, for_OrderBy", trans)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVP) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVD) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPFRT) & Trim(NewCode), trans)

            'cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()


            If DeleteAll_STS <> True Then

                new_record()

                MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            End If

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable




            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CreditorName.Text = ""


            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CreditorName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Payment_No from Weaver_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Payment_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_VouNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Payment_No from Weaver_Payment_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Payment_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_VouNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Payment_No from Weaver_Payment_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Payment_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Payment_No from Weaver_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Payment_No desc", con)
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

            lbl_VouNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Payment_Head", "Weaver_Payment_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_VouNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Payment_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Weaver_Payment_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Weaver_Payment_Date").ToString
                End If
                If dt1.Rows(0).Item("TDS").ToString <> "" Then txt_Tds.Text = Format(Val(dt1.Rows(0).Item("Tds").ToString), "#########0.00")
            End If
            dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

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

            inpno = InputBox("Enter Vou.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Payment_No from Weaver_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Vou No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Packing, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Packing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_payment_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Vou No.", "FOR NEW BAG NO INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Payment_No from Weaver_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Vou No", "DOES NOT INSERT NEW VOU NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_VouNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BAG NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Crd_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim EntID As String = ""
        Dim SlAc_ID As Integer = 0
        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_VouNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cotton_Packing, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_payment_Entry, New_Entry, Me, con, "Weaver_Payment_Head", "Weaver_Payment_Code", NewCode, "Weaver_Payment_Date", "(Weaver_Payment_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Payment_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If
        lbl_UserName.Text = Common_Procedures.User.IdNo
        Crd_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Creditor.Text)

        If Trim(txt_DebitAmount.Text) = "" Then txt_DebitAmount.Text = 0
        If Trim(txt_Add_Amount.Text) = "" Then txt_Add_Amount.Text = 0
        If Trim(txt_Less_Amount.Text) = "" Then txt_Less_Amount.Text = 0
        If Trim(txt_PaidAmount.Text) = "" Then txt_PaidAmount.Text = 0

        tr = con.BeginTransaction

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1273" Then
            lbl_PartyRecNo.Text = txt_PartyRecNo.Text
        End If



        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_VouNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Payment_Head", "Weaver_Payment_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PayDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Payment_Head(Weaver_Payment_Code, Company_IdNo, Weaver_Payment_No, for_OrderBy, Weaver_Payment_Date, Ledger_IdNo, Creditor_IdNo,Paid_Amount, Party_Rec_No, Narration  , User_iDNo , ADD_Amount , LESS_Amount  ,Debit_Amount,Cheque_No,Transfer_Method  , Tds  ,   Tds_Amount  ,   Freight ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_VouNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouNo.Text))) & ", @PayDate, " & Str(Val(Led_ID)) & ",  " & Str(Val(Crd_ID)) & "," & Str(Val(txt_PaidAmount.Text)) & " , " & Str(Val(lbl_PartyRecNo.Text)) & "  , '" & Trim(txt_Narration.Text) & "' ," & Val(lbl_UserName.Text) & " ," & Str(Val(txt_Add_Amount.Text)) & " ," & Str(Val(txt_Less_Amount.Text)) & "  , " & Str(Val(txt_DebitAmount.Text)) & ", " & Str(Val(Txt_ChequeNo.Text)) & ", '" & Trim(cbo_TransferNeft.Text) & "' ," & Val(txt_Tds.Text) & "," & Val(lbl_Tds_Amount.Text) & "," & Val(txt_Freight.Text) & ")"
                cmd.ExecuteNonQuery()

            Else


                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Payment_Head", "Weaver_Payment_Code", Val(lbl_Company.Tag), NewCode, lbl_VouNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Payment_Code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update Weaver_Payment_Head set Weaver_Payment_Date = @PayDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Creditor_IdNo = " & Str(Val(Crd_ID)) & " , ADD_Amount = " & Str(Val(txt_Add_Amount.Text)) & "  ,  LESS_Amount =" & Str(Val(txt_Less_Amount.Text)) & "  ,Debit_Amount= " & Str(Val(txt_DebitAmount.Text)) & "  ,  Paid_Amount = " & Str(Val(txt_PaidAmount.Text)) & " , Party_Rec_No = " & Str(Val(lbl_PartyRecNo.Text)) & "  ,Narration = '" & Trim(txt_Narration.Text) & "' , User_IdNo = " & Val(lbl_UserName.Text) & ", Cheque_No = " & Str(Val(Txt_ChequeNo.Text)) & ", Transfer_Method = '" & Trim(cbo_TransferNeft.Text) & "' , Tds = " & Val(txt_Tds.Text) & ",Tds_Amount = " & Val(lbl_Tds_Amount.Text) & ",Freight = " & Val(txt_Freight.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Payment_Head", "Weaver_Payment_Code", Val(lbl_Company.Tag), NewCode, lbl_VouNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Payment_Code, Company_IdNo, for_OrderBy", tr)

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            vLed_IdNos = Led_ID & "|" & Crd_ID
            vVou_Amts = -1 * Val(CSng(txt_PaidAmount.Text)) & "|" & Val(CSng(txt_PaidAmount.Text))
            If Common_Procedures.Voucher_Updation(con, "Wea.Pymt", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_VouNo.Text), Convert.ToDateTime(msk_date.Text), Trim(txt_Narration.Text) & "  P.Rc.No.: " & Trim(txt_PartyRecNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            '  Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            vLed_IdNos = Led_ID & "|" & Crd_ID
            vVou_Amts = -1 * Val(CSng(txt_DebitAmount.Text)) & "|" & Val(CSng(txt_DebitAmount.Text))
            If Common_Procedures.Voucher_Updation(con, "Wea.Debit", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_VouNo.Text), Convert.ToDateTime(msk_date.Text), Trim(txt_Narration.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            If Val(txt_Add_Amount.Text) <> 0 Then
                Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), "WADVP-" & Trim(NewCode), tr)
            End If
            If Val(txt_Less_Amount.Text) <> 0 Then
                Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), "WADVD-" & Trim(NewCode), tr)
            End If

            vLed_IdNos = ""
            vVou_Amts = ""
            ErrMsg = ""

            vLed_IdNos = Val(Common_Procedures.CommonLedger.Cash_Ac) & "|" & Led_ID
            vVou_Amts = Val(txt_Add_Amount.Text) & "|" & -1 * Val(txt_Add_Amount.Text)
            If Common_Procedures.Voucher_Updation(con, "Wea.AdvPymt", Val(lbl_Company.Tag), Trim(PkCondition_WADVP) & Trim(NewCode), Trim(lbl_VouNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_VouNo.Text) & IIf(Trim(lbl_PartyRecNo.Text) <> "", " , P.Dc.No : " & Trim(lbl_PartyRecNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            vLed_IdNos = ""
            vVou_Amts = ""
            ErrMsg = ""

            vLed_IdNos = Led_ID & "|" & Val(Common_Procedures.CommonLedger.Cash_Ac)
            vVou_Amts = Val(txt_Less_Amount.Text) & "|" & -1 * Val(txt_Less_Amount.Text)
            If Common_Procedures.Voucher_Updation(con, "Wea.AdvDed", Val(lbl_Company.Tag), Trim(PkCondition_WADVD) & Trim(NewCode), Trim(lbl_VouNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_VouNo.Text) & IIf(Trim(lbl_PartyRecNo.Text) <> "", " , P.Dc.No : " & Trim(lbl_PartyRecNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), "WPTDS-" & Trim(NewCode), tr)

            vLed_IdNos = ""
            vVou_Amts = ""
            ErrMsg = ""

            vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Payable_Ac) & "|" & Led_ID
            vVou_Amts = Val(lbl_Tds_Amount.Text) & "|" & -1 * Val(lbl_Tds_Amount.Text)

            If Common_Procedures.Voucher_Updation(con, "Wea.Pymt.Tds", Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(NewCode), Trim(lbl_VouNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_VouNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If


            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), "WPFRT-" & Trim(NewCode), tr)

            vLed_IdNos = ""
            vVou_Amts = ""
            ErrMsg = ""

            vLed_IdNos = Val(Common_Procedures.CommonLedger.Freight_Charges_Ac) & "|" & Led_ID
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)

            If Common_Procedures.Voucher_Updation(con, "Wea.Pymt.Frgt", Val(lbl_Company.Tag), Trim(PkCondition_WPFRT) & Trim(NewCode), Trim(lbl_VouNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_VouNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            tr.Commit()
            Dt1.Dispose()
            Da.Dispose()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_VouNo.Text)
                End If

            Else
                move_record(lbl_VouNo.Text)

            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_type='WEAVER'  and Close_status = 0", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, cbo_Creditor, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 Or Ledger_Type = 'WEAVER' ) and Close_status = 0 ", "(Ledger_idno = 0)")
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, cbo_Creditor, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_type='WEAVER'  and Close_status = 0", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Creditor, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_type='WEAVER' and Close_status = 0", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub Cbo_Creditor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Creditor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or  AccountsGroup_IdNo = 6 )  ", "(Ledger_idno = 0)")
    End Sub

    Private Sub Cbo_Creditor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Creditor.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Creditor, cbo_PartyName, Txt_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )  ", "(Ledger_idno = 0)")
    End Sub

    Private Sub Cbo_Creditor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Creditor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Creditor, Txt_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )  ", "(Ledger_idno = 0)")
    End Sub

    Private Sub Cbo_Creditor_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Creditor.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Creditor.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub





    Private Sub txt_PaidAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PaidAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        If (e.KeyValue = 40) Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim OrdByNo As Single = 0

        OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_VouNo.Text))

        da1 = New SqlClient.SqlDataAdapter("select Cheque_No from Weaver_Payment_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cheque_No desc", con)
        dt1 = New DataTable
        da1.Fill(dt1)

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        Else
            If Asc(e.KeyChar) = 37 Then
                narration()
            End If
        End If
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub
    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim led_IdNo As Integer, Crd_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            led_IdNo = 0
            Crd_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Payment_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Payment_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Payment_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_CreditorName.Text) <> "" Then
                Crd_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_CreditorName.Text)
            End If


            If Val(led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(led_IdNo)) & ")"
            End If
            If Val(Crd_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.creditor_IdNo = " & Str(Val(Crd_IdNo)) & " )"
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ""
            End If



            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,c.Ledger_Name as Creditor_Name  from Weaver_Payment_Head a inner join Ledger_head b on a.Ledger_idno = b.Ledger_idno LEFT OUTER join Ledger_head c on a.Creditor_idno = c.Ledger_idno  where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Payment_Date, a.for_orderby, a.Weaver_Payment_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Weaver_Yarn_Delivery_Head a left outer join Weaver_Yarn_Delivery_Details b on a.Weaver_Yarn_Delivery_Code = b.Weaver_Yarn_Delivery_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Yarn_Delivery_Date, a.for_orderby, a.Weaver_Yarn_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Payment_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Payment_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Creditor_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Paid_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub




    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'WEAVER') and Close_status = 0", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CreditorName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'WEAVER') and Close_status = 0", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'WEAVER') and Close_status = 0", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CreditorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CreditorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 )  ", "(Ledger_idno = 0)")

    End Sub


    Private Sub cbo_Filter_CreditorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CreditorName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CreditorName, dtp_Filter_ToDate, cbo_Filter_PartyName, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 )  ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_CreditorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CreditorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CreditorName, cbo_Filter_PartyName, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 )  ", "(Ledger_idno = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            Pnl_Back.Enabled = True
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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim entcode As String
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_payment_Entry, New_Entry) = False Then Exit Sub

        Try

            Da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Payment_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Payment_Code = '" & Trim(entcode) & "'", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            Dt1.Dispose()
            Da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try





        For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(i)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

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
                MessageBox.Show("the printing operation failed" & vbCrLf & ex.Message, "does not print...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(800, 800)
                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("the printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "does not show print preview...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint

        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim entcode As String
        ' Dim PrnHeading As String
        PrintDocument1.DefaultPageSettings.Landscape = False


        entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt = New DataTable
        prn_PageNo = 0

        Try

            Da1 = New SqlClient.SqlDataAdapter("select a.*,co.*, b.Ledger_Name as To_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3,  b.Ledger_Address4, b.Ledger_GSTinNo, c.Ledger_Name as By_Name from Weaver_Payment_Head a, Ledger_Head b, Ledger_head c, Company_Head co  Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Payment_Code = '" & Trim(entcode) & "' and a.Ledger_IdNo = b. Ledger_IdNo and a.Creditor_IdNo = c.Ledger_IdNo and a.Company_IdNo = co.Company_IdNo", con)
            prn_HdDt = New DataTable
            Da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count <= 0 Then
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format11(e)
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs) '-----Asia Tex Veerapandi 
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single, i As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PrnHeading As String = ""
        Dim Nar1 As String = ""
        Dim Nar2 As String = ""




        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50 ' 20
            .Right = 60 ' 50
            .Top = 30
            .Bottom = 30

            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize


            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 18.5 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(525) : ClArr(2) = 100
        ClArr(3) = PageWidth - (LMargin + ClArr(1))

        'CurY = TMargin
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(1) = CurY

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

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 13, FontStyle.Bold)




        Common_Procedures.Print_To_PrintDocument(e, "PAYMENT ", LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("Voucher No  : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("To_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Ref No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Weaver_Payment_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Payment_Date")), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + 8

        Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin, CurY, 2, ClArr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, " AMOUNT  ", LMargin + ClArr(1) + 75, CurY, 2, ClArr(2), pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        CurY = CurY + 13
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(3))

        Nar1 = Trim(prn_HdDt.Rows(0).Item("Narration").ToString)
        Nar2 = ""
        If Len(Nar1) > 65 Then
            For i = 65 To 1 Step -1
                If Mid$(Trim(Nar1), i, 1) = " " Or Mid$(Trim(Nar1), i, 1) = "," Or Mid$(Trim(Nar1), i, 1) = "." Or Mid$(Trim(Nar1), i, 1) = "-" Or Mid$(Trim(Nar1), i, 1) = "/" Or Mid$(Trim(Nar1), i, 1) = "_" Or Mid$(Trim(Nar1), i, 1) = "(" Or Mid$(Trim(Nar1), i, 1) = ")" Or Mid$(Trim(Nar1), i, 1) = "\" Or Mid$(Trim(Nar1), i, 1) = "[" Or Mid$(Trim(Nar1), i, 1) = "]" Or Mid$(Trim(Nar1), i, 1) = "{" Or Mid$(Trim(Nar1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 65

            Nar2 = Microsoft.VisualBasic.Right(Trim(Nar1), Len(Nar1) - i)
            Nar1 = Microsoft.VisualBasic.Left(Trim(Nar1), i - 1)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "By " & Trim(prn_HdDt.Rows(0).Item("By_Name").ToString), LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Paid_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Narration : ", LMargin + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Trim(Nar1), LMargin + 20, CurY, 0, 0, pFont)

        If Trim(Nar2) <> "" Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Trim(Nar2), LMargin + 20, CurY, 0, 0, pFont)
            'NoofDets = NoofDets + 1
        End If
        CurY = CurY + TxtHgt + 30 ' 40
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---UNITED WEAVES
            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_VoucherAmount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Total_VoucherAmount").ToString))
            Common_Procedures.Print_To_PrintDocument(e, "Rupees  :   " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            Dim vCurr_Bal As String = ""
            Dim cmd As New SqlClient.SqlCommand
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            cmd.Connection = con
            cmd.Parameters.Clear()
            'cmd.Parameters.AddWithValue("@CompanyFromDate", Common_Procedures.Company_FromDate)
            cmd.Parameters.AddWithValue("@CompanyFromDate", Convert.ToDateTime(msk_date.Text))

            cmd.CommandText = "select sum(a.Voucher_amount) as BalAmount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("to_idno").ToString)) & " and a.voucher_date <= @CompanyFromDate "
            da = New SqlClient.SqlDataAdapter(cmd) '("select sum(a.Voucher_amount) as BalAmount from voucher_details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("to_idno").ToString)) & " a.voucher_date >= @CompanyFromDate", con)
            dt1 = New DataTable
            da.Fill(dt1)

            vCurr_Bal = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    vCurr_Bal = Val(dt1.Rows(0).Item("BalAmount").ToString)
                End If
            End If
            dt1.Clear()
            dt1.Dispose()
            da.Dispose()
            cmd.Dispose()


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + 70, CurY, LMargin + ClArr(1) + 70, LnAr(3))

            CurY = CurY + 5 ' 40
            Common_Procedures.Print_To_PrintDocument(e, "Current Balance", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + 120, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(vCurr_Bal), "########0.00"), LMargin + 130, CurY, 0, 0, pFont)
        Else
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_VoucherAmount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            'CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + 70, CurY, LMargin + ClArr(1) + 70, LnAr(3))

            CurY = CurY + 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Paid_Amount").ToString))
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Paid_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Rupees  :   " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "checked", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Signature ", PageWidth - 20, CurY, 1, 0, pFont)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(7), LMargin, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(7), PageWidth, LnAr(2))


        If 0 <= prn_HdDt.Rows.Count - 1 Then

            e.HasMorePages = False
        End If

    End Sub

    Private Sub cbo_PartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.LostFocus
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewNo As Integer = 0
        Dim Led_ID As Integer = 0
        Dim cmd As New SqlClient.SqlCommand
        Dim Dt As New DataTable
        Dim Dtbl1 As New DataTable
        Dim Bal As Decimal = 0
        Dim Amt As Double = 0, BillPend As Double = 0
        Dim count As String = ""
        Dim eNDS As String = ""

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        da = New SqlClient.SqlDataAdapter("select max(Party_Rec_No) from Weaver_Payment_Head where Ledger_idno = " & Val(Led_ID) & " and Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'", con)
        da.Fill(dt1)
        NewNo = 0
        If dt1.Rows.Count > 0 Then

            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                NewNo = Val(dt1.Rows(0)(0).ToString)
            End If
        End If

        NewNo = NewNo + 1

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1273" Then
            lbl_PartyRecNo.Text = NewNo
        Else
            lbl_PartyRecNo.Text = ""
            lbl_PartyRecNo.Text = txt_PartyRecNo.Text
        End If






        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1081" Then
            '-----------BALANCE

            da = New SqlClient.SqlDataAdapter("select  sum(a.voucher_amount) as amount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " ", con)
            Dtbl1 = New DataTable
            da.Fill(Dtbl1)

            Bal = 0
            If Dtbl1.Rows.Count > 0 Then
                For i = 0 To Dtbl1.Rows.Count - 1
                    Amt = Val(Dtbl1.Rows(i).Item("amount").ToString)
                    lbl_AdvBalance.Text = Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Cr", " Dr")

                    Amt = Val(Dtbl1.Rows(i).Item("amount").ToString)
                    lbl_AdvBalance.Text = Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Cr", " Dr")
                Next i
            End If

        Else

            '----------- Advance
            lbl_AdvBalance.Text = ""
            da = New SqlClient.SqlDataAdapter("select sum(a.voucher_amount) as amount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and (a.Entry_Identification LIKE 'OPENI-%' or a.Voucher_Code LIKE 'WADVP-%' or a.Voucher_Code LIKE 'WPADP-%' or a.Voucher_Code LIKE 'WADVD-%'  or a.Voucher_Code LIKE 'WPADD-%') ", con)
            Dtbl1 = New DataTable
            da.Fill(Dtbl1)
            Bal = 0
            If Dtbl1.Rows.Count > 0 Then
                Amt = Val(Dtbl1.Rows(0).Item("amount").ToString)
                lbl_AdvBalance.Text = "Advance : " + Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Cr", " Dr")
            End If

            '----------- Cooly
            lbl_CoolyBalance.Text = ""
            da = New SqlClient.SqlDataAdapter("select sum(a.voucher_amount) as amount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and (a.Entry_Identification NOT LIKE 'OPENI-%' AND a.Voucher_Code NOT LIKE 'WADVP-%' AND a.Voucher_Code NOT LIKE 'WPADP-%' AND a.Voucher_Code NOT LIKE 'WADVD-%' AND a.Voucher_Code NOT LIKE 'WPADD-%') ", con)
            Dtbl1 = New DataTable
            da.Fill(Dtbl1)
            Bal = 0
            If Dtbl1.Rows.Count > 0 Then
                Amt = Val(Dtbl1.Rows(0).Item("amount").ToString)
                lbl_CoolyBalance.Text = "Cooly : " + Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Cr", " Dr")
            End If

        End If
        '----------- YARN

        cmd.Connection = con

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, name1, name2, weight1) Select a.DeliveryTo_Idno, tP.Ledger_Name, c.count_name, sum(a.Weight) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON  a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and a.Weight <> 0 group by a.DeliveryTo_Idno, tP.Ledger_Name, c.count_name having sum(a.Weight) <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, name1, name2, weight1) Select a.ReceivedFrom_Idno, tP.Ledger_Name, c.count_name, -1*sum(a.Weight) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON  a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and a.Weight <> 0 group by a.ReceivedFrom_Idno, tP.Ledger_Name, c.count_name having sum(a.Weight) <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, name1, name2, weight1) Select Int1, name1, name2, sum(weight1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1, name1, name2 having sum(Weight1) <> 0"
        cmd.ExecuteNonQuery()

        lbl_Yarn.Text = ""

        da = New SqlClient.SqlDataAdapter("select Int1, name1, name2, weight1 as wgt from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
        Dtbl1 = New DataTable
        da.Fill(Dtbl1)
        count = ""
        If Dtbl1.Rows.Count > 0 Then
            For i = 0 To Dtbl1.Rows.Count - 1
                count = Trim(Dtbl1.Rows(i).Item("name2").ToString)
                lbl_Yarn.Text = Trim(lbl_Yarn.Text) & " " & Trim(count) & " : " & Format(Val(Dtbl1.Rows(i).Item("wgt").ToString), "#######0.000")
            Next i
        End If

        '-----------PAVU

        cmd.Connection = con

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, name1, name2, meters1) Select a.DeliveryTo_Idno, tP.Ledger_Name, c.endscount_name, sum(a.Meters) from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON  a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & "  and a.Meters <> 0 group by a.DeliveryTo_Idno, tP.Ledger_Name, c.endscount_name having sum(a.Meters) <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, name1, name2, meters1) Select a.ReceivedFrom_Idno, tP.Ledger_Name, c.endscount_name, -1*sum(a.Meters) from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & "  and a.Meters <> 0 group by a.ReceivedFrom_Idno, tP.Ledger_Name, c.endscount_name having sum(a.Meters) <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, name1, name3, meters1) Select Int1, name1, name2, sum(meters1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1, name1, name2 having sum(meters1) <> 0"
        cmd.ExecuteNonQuery()

        lbl_Pavu.Text = ""

        da = New SqlClient.SqlDataAdapter("select Int1, name1, name3, meters1 from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
        Dtbl1 = New DataTable
        da.Fill(Dtbl1)
        eNDS = ""
        If Dtbl1.Rows.Count > 0 Then
            For i = 0 To Dtbl1.Rows.Count - 1
                eNDS = Trim(Dtbl1.Rows(i).Item("name3").ToString)
                lbl_Pavu.Text = Trim(lbl_Pavu.Text) & " " & Trim(eNDS) & " : " & Format(Val(Dtbl1.Rows(i).Item("meters1").ToString), "#######0.00")
            Next i
        End If


        '-------- Empty Beam
        cmd.Connection = con

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, Name1, Int2) Select a.DeliveryTo_Idno, tP.Ledger_Name,  sum(a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and  (a.Empty_Beam+a.Pavu_Beam) <> 0 group by a.DeliveryTo_Idno, tP.Ledger_Name having sum(a.Empty_Beam+a.Pavu_Beam) <> 0 "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, Name1, Int2) Select a.ReceivedFrom_Idno, tP.Ledger_Name,  -1*sum(a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and (a.Empty_Beam+a.Pavu_Beam) <> 0 group by a.ReceivedFrom_Idno, tP.Ledger_Name having sum(a.Empty_Beam+a.Pavu_Beam) <> 0 "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, Name1, Int2) Select Int1, Name1,  sum(Int2) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1, Name1  having sum(Int2) <> 0 "
        cmd.ExecuteNonQuery()

        lbl_EmptyBeam.Text = ""

        da = New SqlClient.SqlDataAdapter("select Int1, name1, Int2 from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
        Dtbl1 = New DataTable
        da.Fill(Dtbl1)

        If Dtbl1.Rows.Count > 0 Then
            For i = 0 To Dtbl1.Rows.Count - 1
                lbl_EmptyBeam.Text = Val(Dtbl1.Rows(i).Item("Int2").ToString) & " Beams"
            Next i
        End If
        Dt.Dispose()
        da.Dispose()

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_PartyName.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub
    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PartyName.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Narration.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub
    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_VouNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If SaveAll_STS = True Then

            save_record()
            If Trim(UCase(LastNo)) = Trim(UCase(lbl_VouNo.Text)) Then
                Timer1.Enabled = False
                SaveAll_STS = False
                MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                movenext_record()
            End If
        ElseIf DeleteAll_STS = True Then
            delete_record()
            If Trim(UCase(LastNo)) = Trim(UCase(lbl_VouNo.Text)) Then
                Timer1.Enabled = False
                DeleteAll_STS = False
                MessageBox.Show("All entries Deleted Successfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Else
                movenext_record()

        End If
        End If
    End Sub

    Private Sub btn_WeaverLedger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_WeaverLedger.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"
        Common_Procedures.RptInputDet.ReportName = "Weaver All Stock Ledger"
        Common_Procedures.RptInputDet.ReportHeading = "Weaver Ledger"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,*W"
        f.MdiParent = MDIParent1
        f.Show()
        f.cbo_Inputs2.Text = cbo_PartyName.Text
        f.Show_Report()
    End Sub

    Private Sub Txt_ChequeNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Txt_ChequeNo.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub Txt_ChequeNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Txt_ChequeNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_TransferNeft.Visible And cbo_TransferNeft.Enabled Then
                cbo_TransferNeft.Focus()

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_date.Focus()
                End If
            End If
        End If

    End Sub

    Private Sub cbo_TransferNeft_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransferNeft.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Payment_Head", "Transfer_Method", "", "Transfer_Method")
    End Sub

    Private Sub cbo_TransferNeft_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransferNeft.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransferNeft, Txt_ChequeNo, txt_PaidAmount, "Weaver_Payment_Head", "Transfer_Method", "", "Transfer_Method")
    End Sub

    Private Sub cbo_TransferNeft_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransferNeft.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransferNeft, txt_PaidAmount, "Weaver_Payment_Head", "Transfer_Method", "", "", False)
    End Sub

    Public Sub narration()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Payment_No from Weaver_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Payment_No", con)
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

    Private Sub txt_Tds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Tds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Tds_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Tds.TextChanged
        TdsCommission_Calculation()
    End Sub

    Private Sub TdsCommission_Calculation()
        Dim tdsamt As Double = 0

        If NoCalc_Status = True Then Exit Sub

        tdsamt = Format(Val(txt_PaidAmount.Text) * Val(txt_Tds.Text) / 100, "########0")

        lbl_Tds_Amount.Text = Format(Val(tdsamt), "########0")

    End Sub

    Private Sub txt_Add_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Add_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Less_Amount.Focus()
        End If
    End Sub

    Private Sub txt_DebitAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DebitAmount.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If txt_PartyRecNo.Visible Then
                txt_PartyRecNo.Focus()
            Else
                txt_Add_Amount.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Less_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Less_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_SMS_Click(sender As System.Object, e As System.EventArgs) Handles btn_SMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", EndsCount As String = "", Cloth As String = ""
        Dim Led_IdNo As Integer = 0, Endscount_IdNo As Integer = 0, Cloth_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")


            ' If Trim(AgPNo) <> "" Then
            PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", "", "")
            ' End If

            smstxt = smstxt & "Receipt No : " & Trim(lbl_VouNo.Text) & vbCrLf
            smstxt = smstxt & " Date : " & Trim(msk_date.Text) & vbCrLf
            smstxt = smstxt & " Paid amount: " & Val(txt_PaidAmount.Text) & vbCrLf
            smstxt = smstxt & " debit amount: " & Val(txt_DebitAmount.Text) & vbCrLf
            smstxt = smstxt & " add amount: " & Val(txt_Add_Amount.Text) & vbCrLf
            smstxt = smstxt & " less amount: " & Val(txt_Less_Amount.Text) & vbCrLf

            'smstxt = smstxt & " Balnce : " & Trim(lbl_AdvBalance.Text) & vbCrLf



            smstxt = smstxt & " " & vbCrLf
            smstxt = smstxt & " Thanks! " & vbCrLf
            smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub


    Private Sub Printing_Format11(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0, i As Integer = 0

        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_StateName As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single, W2 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PrnHeading As String = ""

        Dim Nar1 As String = ""
        Dim Nar2 As String = ""
        Dim BnkDetAr() As String
        Dim BInc As Integer
        Dim BankNm1 As String


        For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(i)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 30 
            .Right = 40
            .Top = 30 ' 50 
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        TxtHgt = 17.5 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}



        ClArr(1) = Val(450) ': ClArr(2) = 100
        ClArr(2) = PageWidth - (LMargin + ClArr(1))

        PrnHeading = "WEAVER PAYMENT VOUCHER"

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_StateName = ""

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
        'If Trim(prn_HdDt.Rows(0).Item("Company_State").ToString) <> "" Then
        '    Cmp_StateName = "State : " & prn_HdDt.Rows(0).Item("Company_State").ToString
        'End If

        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 20, CurY + 5, 110, 100)

                        End If

                    End Using

                End If

            End If

        End If


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 13, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, PrnHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("Voucher No  : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("To_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Voucher No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Weaver_Payment_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "      " & prn_HdDt.Rows(0).Item("ledger_address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "      " & prn_HdDt.Rows(0).Item("ledger_address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Payment_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "      " & prn_HdDt.Rows(0).Item("ledger_address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "      " & prn_HdDt.Rows(0).Item("ledger_address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + 8

        Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin, CurY, 2, ClArr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, " AMOUNT  ", LMargin + ClArr(1) + 75, CurY, 2, ClArr(2), pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        Nar1 = Trim(prn_HdDt.Rows(0).Item("Narration").ToString)
        Nar2 = ""
        If Len(Nar1) > 65 Then
            For i = 65 To 1 Step -1
                If Mid$(Trim(Nar1), i, 1) = " " Or Mid$(Trim(Nar1), i, 1) = "," Or Mid$(Trim(Nar1), i, 1) = "." Or Mid$(Trim(Nar1), i, 1) = "-" Or Mid$(Trim(Nar1), i, 1) = "/" Or Mid$(Trim(Nar1), i, 1) = "_" Or Mid$(Trim(Nar1), i, 1) = "(" Or Mid$(Trim(Nar1), i, 1) = ")" Or Mid$(Trim(Nar1), i, 1) = "\" Or Mid$(Trim(Nar1), i, 1) = "[" Or Mid$(Trim(Nar1), i, 1) = "]" Or Mid$(Trim(Nar1), i, 1) = "{" Or Mid$(Trim(Nar1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 65

            Nar2 = Microsoft.VisualBasic.Right(Trim(Nar1), Len(Nar1) - i)
            Nar1 = Microsoft.VisualBasic.Left(Trim(Nar1), i - 1)
        End If
        CurY = CurY + 13
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(3))
        W2 = e.Graphics.MeasureString("Advance/Salary  : ", pFont).Width



        Common_Procedures.Print_To_PrintDocument(e, "By " & Trim(prn_HdDt.Rows(0).Item("by_Name").ToString), LMargin + 20, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Paid_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        ' CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "Cash/Check", LMargin + 20, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Cheque").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "Advance/Salary", LMargin + 20, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Advance_Salary").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)
        'CurY = CurY + TxtHgt

        'Common_Procedures.Print_To_PrintDocument(e, "Remarks ", LMargin + 20, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "Cheque No   : " & Trim(prn_HdDt.Rows(0).Item("Cheque_No").ToString), LMargin + 20, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Narration").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Narration     : ", LMargin + 20, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, Trim(Nar1), LMargin + 20, CurY, 0, 0, pFont)

            If Trim(Nar2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, Trim(Nar2), LMargin + 20, CurY, 0, 0, pFont)
                'NoofDets = NoofDets + 1
            End If
        End If
        CurY = CurY + TxtHgt + 30
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Paid_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(3))


        CurY = CurY + TxtHgt - 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Paid_Amount").ToString))
        BmsInWrds = Replace(Trim(UCase(BmsInWrds)), "", "")
        Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY



        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "checked", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Signature ", PageWidth - 20, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(7), LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(7), PageWidth, LnAr(1))

        e.HasMorePages = False
    End Sub

    Private Sub txt_PaidAmount_TextChanged(sender As Object, e As EventArgs) Handles txt_PaidAmount.TextChanged
        TdsCommission_Calculation()
    End Sub

    Private Sub txt_PartyRecNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_PartyRecNo.KeyDown
        If e.KeyCode = 38 Then
            txt_DebitAmount.Focus()
        End If

        If e.KeyCode = 40 Then
            txt_Add_Amount.Focus()
        End If

    End Sub

    Private Sub txt_PartyRecNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_PartyRecNo.KeyPress

        If Asc(e.KeyChar) = 13 Then
            txt_Add_Amount.Focus()
        End If

    End Sub

    Private Sub txt_Add_Amount_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Add_Amount.KeyDown
        If e.KeyCode = 38 Then

            If txt_PartyRecNo.Visible Then
                txt_PartyRecNo.Focus()
            Else
                txt_DebitAmount.Focus()
            End If

        End If

        If e.KeyCode = 40 Then
            txt_Less_Amount.Focus()
        End If
    End Sub

    Private Sub txt_DebitAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_DebitAmount.KeyDown
        If e.KeyCode = 38 Then
            txt_PaidAmount.Focus()
        End If

        If e.KeyCode = 40 Then
            If txt_PartyRecNo.Visible Then
                txt_PartyRecNo.Focus()
            Else
                txt_Add_Amount.Focus()
            End If
        End If
    End Sub

    Private Sub txt_DebitAmount_TextChanged(sender As Object, e As EventArgs) Handles txt_DebitAmount.TextChanged

    End Sub

    Private Sub Me_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        'vSPEC_KEYS.Add(e.KeyCode)
        If e.Control AndAlso e.Alt AndAlso e.KeyCode = Keys.D Then
            'MessageBox.Show("Shortcut Ctrl + Alt + N activated!")
            DeleteAll()
        End If
    End Sub

    Private Sub Me_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        'If Control.ModifierKeys AndAlso vSPEC_KEYS.Contains(Keys.A) AndAlso vSPEC_KEYS.Contains(Keys.D) Then
        '    'MessageBox.Show("Ctrl+A or Ctrl+D was pressed!")
        '    DeleteAll()
        'End If

        'vSPEC_KEYS.Remove(e.KeyCode)
        'vSPEC_KEYS.Clear()
    End Sub
    Private Sub DeleteAll()
        Dim pwd As String = ""

        If MessageBox.Show("Do you want to Delete All Data's?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSDA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        DeleteAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_VouNo.Text

        movefirst_record()
        Timer1.Enabled = True
    End Sub

End Class
