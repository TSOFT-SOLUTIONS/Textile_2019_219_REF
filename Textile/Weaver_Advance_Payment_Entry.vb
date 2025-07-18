Public Class Weaver_Advance_payment_Entry

    Implements Interface_MDIActions


    Private Structure VoucherEntry_AmountDetails
        Dim LedgerIdNo As Integer
        Dim VoucherAmount As Double
    End Structure
    Private VouAmtAr(10) As VoucherEntry_AmountDetails

    Public Advance_Opening_Entry_Status As Boolean = False
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private Pk_Condition As String = "AMREC-"
    '  Private Pk_Condition2 As String = "AMVOU-"
    Private Prec_ActCtrl As New Control

    Private vcbo_KeyDwnVal As Double

    Private prn_HdDt As New DataTable
    Private prn_PageNo As Integer

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private vmskOldText As String = ""
    Private vmskSelStrt As Integer = -1

    Public RptSubReport_Index As Integer = 0
    Public RptSubReport_CompanyShortName As String = ""
    Public RptSubReport_VouNo As String = ""
    Public RptSubReport_VouCode As String = ""

    Public Structure SubReport_Details
        Dim ReportName As String
        Dim ReportGroupName As String
        Dim ReportHeading As String
        Dim ReportInputs As String
        Dim IsGridReport As Boolean

        Dim CurrentRowVal As Integer
        Dim TopRowVal As Integer

        Dim DateInp_Value1 As Date
        Dim DateInp_Value2 As Date
        Dim CboInp_Text1 As String
        Dim CboInp_Text2 As String
        Dim CboInp_Text3 As String
        Dim CboInp_Text4 As String
        Dim CboInp_Text5 As String

    End Structure
    Public RptSubReportDet(10) As SubReport_Details

    Public Structure SubReport_InputDetails
        Dim PKey As String
        Dim TableName As String
        Dim Selection_FieldName As String
        Dim Return_FieldName As String
        Dim Condition As String
        Dim Display_Name As String
        Dim BlankFieldCondition As String
        Dim CtrlType_Cbo_OR_Txt As String
    End Structure
    Public RptSubReportInpDet(10, 10) As SubReport_InputDetails

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_filter.Visible = False
        pnl_back.Enabled = True
        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black
        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black

        cbo_PartyName.Text = ""

        txt_Remarks.Text = ""
        cbo_chequeBank.Text = ""
        msk_Date.Text = ""
        txt_ReferenceNo.Text = ""
        dtp_cheque_date.Text = ""

        Msk_Date_Voucher.Text = ""
        dtp_voucher_date.Text = ""

        cbo_PaymentMode.Text = ""
        '   cbo_DebitorAccount.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        txt_Amount.Text = ""
        dtp_cheque_date.Text = ""

        lbl_CurrentBalance.Tag = -100
        lbl_CurrentBalance.Text = "Current Balance :"

        pnl_CurrentBalance.Visible = False

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

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.ledger_Name  from Weaver_Advance_Payment_Head a LEFT OUTER JOIN Ledger_Head b ON a.ledger_IdNo = b.ledger_IdNo  where a.Weaver_Advance_Payment_Code = '" & Trim(NewCode) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RecNo.Text = dt1.Rows(0).Item("Weaver_Advance_Payment_No").ToString
                ' dtp_Date.Text = dt1.Rows(0).Item("Weaver_Advance_Payment_Date").ToString

                dtp_voucher_date.Text = dt1.Rows(0).Item("Weaver_Advance_Payment_Date").ToString
                Msk_Date_Voucher.Text = dtp_voucher_date.Text

                cbo_PartyName.Text = dt1.Rows(0).Item("ledger_Name").ToString
                txt_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "#########0.00")
                cbo_PaymentMode.Text = dt1.Rows(0).Item("Payment_Mode").ToString
                cbo_DebitorAccount.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DebitAc_IdNo").ToString))
                txt_ReferenceNo.Text = dt1.Rows(0).Item("Reference_No").ToString
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                cbo_chequeBank.Text = dt1.Rows(0).Item("ChequeBank_Name").ToString

                If Trim(cbo_PaymentMode.Text) <> "CASH" Then
                    dtp_cheque_date.Text = dt1.Rows(0).Item("Cheque_Date").ToString
                    msk_Date.Text = dtp_cheque_date.Text
                End If

            Else

                new_record()

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Msk_Date_Voucher.Visible And Msk_Date_Voucher.Enabled Then Msk_Date_Voucher.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Advance_Payment, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Advance_Payment, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



            cmd.Connection = con
            cmd.Transaction = tr

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode), tr)



            cmd.CommandText = "delete from Weaver_Advance_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Advance_Payment_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Msk_Date_Voucher.Enabled = True And Msk_Date_Voucher.Visible = True Then Msk_Date_Voucher.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or  AccountsGroup_IdNo  = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_EmployeeFilter.DataSource = dt1
            cbo_EmployeeFilter.DisplayMember = "Ledger_DisplayName"

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""
            cbo_EmployeeFilter.SelectedIndex = -1
            dgv_filter.Rows.Clear()

            da.Dispose()

        End If

        pnl_filter.Visible = True
        pnl_filter.Enabled = True
        pnl_filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Advance_Payment, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Advance_Payment, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Advance_payment, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Advance_Payment_No from Weaver_Advance_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Advance_Payment_Code = '" & Trim(Pk_Condition) & Trim(InvCode) & "' AND Weaver_Advance_Payment_Code LIKE '" & Trim(Pk_Condition) & "%'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Rec No.", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RecNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Advance_Payment_No from Weaver_Advance_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Advance_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Weaver_Advance_Payment_Code LIKE '" & Trim(Pk_Condition) & "%'  Order by for_Orderby, Weaver_Advance_Payment_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Advance_Payment_No from Weaver_Advance_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Advance_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Weaver_Advance_Payment_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Weaver_Advance_Payment_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Advance_Payment_No from Weaver_Advance_Payment_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Advance_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Weaver_Advance_Payment_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Weaver_Advance_Payment_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Advance_Payment_No from Weaver_Advance_Payment_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Advance_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  AND Weaver_Advance_Payment_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Weaver_Advance_Payment_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Dim dt2 As New DataTable
        Dim NewCode As Integer = 0
        Dim NewNo As Integer = 0

        Try
            clear()

            New_Entry = True

            NewNo = NewNo + 1

            lbl_RecNo.Text = NewNo
            lbl_RecNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Weaver_Advance_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Advance_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            dt = New DataTable
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            NewID = NewID + 1

            lbl_RecNo.Text = NewID
            lbl_RecNo.ForeColor = Color.Red

            Da1 = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Advance_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "and Weaver_Advance_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by  Weaver_Advance_Payment_No desc, For_OrderBy desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                '  dtp_Date.Text = Dt1.Rows(0).Item("Weaver_Advance_Payment_Date").ToString
                cbo_PaymentMode.Text = Dt1.Rows(0).Item("Payment_Mode").ToString
                cbo_DebitorAccount.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(0).Item("DebitAc_IdNo").ToString))
            End If
            Dt1.Clear()

            If Msk_Date_Voucher.Enabled And Msk_Date_Voucher.Visible Then Msk_Date_Voucher.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Receipt Voucher No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con

            cmd.CommandText = "select Weaver_Advance_Payment_No from Weaver_Advance_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "and Weaver_Advance_Payment_No = '" & Trim(inpno) & "' and Weaver_Advance_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'"
            'cmd.CommandText = "select Weaver_Advance_Payment_No from Weaver_Advance_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "and Weaver_Advance_Payment_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Voucher No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim entcode As String
        Dim ps As Printing.PaperSize

        entcode = Pk_Condition & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Advance_payment, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, c.ledger_name , d.ledger_name as debitor_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4 from weaver_advance_payment_head a, ledger_head c, ledger_head d where a.company_idno = " & Str(Val(lbl_Company.Tag)) & "and a.weaver_advance_payment_code = '" & Trim(entcode) & "' and a.ledger_idno = c.ledger_idno and a.DebitAc_IdNo = d.ledger_idno", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count <= 0 Then
                MessageBox.Show("this is new entry", "does not print...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "does not print...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

        entcode = Pk_Condition & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt = New DataTable
        prn_PageNo = 0

        Da1 = New SqlClient.SqlDataAdapter("select a.*, cm.*,c.ledger_name , d.ledger_name as debitor_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4 from weaver_advance_payment_head a, ledger_head c, ledger_head d,Company_Head cm where a.company_idno = " & Str(Val(lbl_Company.Tag)) & "and a.weaver_advance_payment_code = '" & Trim(entcode) & "' and a.ledger_idno = c.ledger_idno and a.DebitAc_IdNo = d.ledger_idno and  a.Company_idno = cm.Company_idno", con)
        Da1.Fill(prn_HdDt)


        If prn_HdDt.Rows.Count <= 0 Then

            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format1(e)
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
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




        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15 ' 65
            .Right = 50
            .Top = 40
            .Bottom = 50
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

        ClArr(1) = Val(550) : ClArr(2) = 100
        ClArr(3) = PageWidth - (LMargin + ClArr(1))

        'CurY = TMargin
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(1) = CurY

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
        Common_Procedures.Print_To_PrintDocument(e, "ADVANCE PAYMENT", LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 20
        p1Font = New Font("Calibri", 13, FontStyle.Bold)



        Common_Procedures.Print_To_PrintDocument(e, PrnHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight 
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY



        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("Voucher No  : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("lEDGER_NAME").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Ref No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Weaver_Advance_payment_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "      " & prn_HdDt.Rows(0).Item("Ledger_address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "      " & prn_HdDt.Rows(0).Item("Ledger_address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Advance_payment_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "      " & prn_HdDt.Rows(0).Item("Ledger_address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "      " & prn_HdDt.Rows(0).Item("Ledger_address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

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
        W2 = e.Graphics.MeasureString("Cash/Check    : ", pFont).Width

        Common_Procedures.Print_To_PrintDocument(e, "By " & Trim(prn_HdDt.Rows(0).Item("Debitor_Name").ToString), LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Cash/Check", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Payment_Mode").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Advance", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Remarks ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 30
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt

        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Amount").ToString))
        BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")
        Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(3))

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin, CurY, 2, PrintWidth, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "Authorized Signature ", PageWidth - 20, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10



        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(7), LMargin, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(7), PageWidth, LnAr(2))
        e.HasMorePages = False
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewCode1 As String = ""
        Dim NewNo As Long = 0
        Dim Mem_id As Integer = 0
        Dim CrdtAc_id As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim OnAc_id As Integer = 0
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'Weaver_Advance_Payment
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Advance_payment, New_Entry, Me, con, "weaver_advance_payment_Head", "weaver_advance_payment_Code", NewCode, "weaver_advance_payment_Date", "(weaver_advance_payment_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and weaver_advance_payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, weaver_advance_payment_No desc", dtp_voucher_date.Value.Date) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(Msk_Date_Voucher.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Msk_Date_Voucher.Enabled Then Msk_Date_Voucher.Focus()
            Exit Sub
        End If

        If IsDate(dtp_voucher_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Msk_Date_Voucher.Enabled Then Msk_Date_Voucher.Focus()
            Exit Sub
        End If

        Mem_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_PartyName.Text)
        If Mem_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        CrdtAc_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DebitorAccount.Text)
        If CrdtAc_id = 0 Then
            MessageBox.Show("Invalid Debit Account", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DebitorAccount.Enabled Then cbo_DebitorAccount.Focus()
            Exit Sub
        End If

        If Trim(cbo_PaymentMode.Text) = "" Then
            MessageBox.Show("Invalid Payment Method", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PaymentMode.Enabled Then cbo_PaymentMode.Focus()
            Exit Sub
        End If


        tr = con.BeginTransaction

        Try

            Common_Procedures.FnYearCode = Common_Procedures.FnYearCode
            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Advance_Payment_Head", "Weaver_Advance_Payment_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_voucher_date.Value.Date)


            cmd.Parameters.AddWithValue("@ChequeDate", dtp_cheque_date.Value.Date)



            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Advance_Payment_Head (        Weaver_Advance_Payment_Code   ,                 Company_IdNo     ,          Weaver_Advance_Payment_No  ,                               for_OrderBy                               , Weaver_Advance_Payment_Date,           Ledger_IdNo      ,                 Amount   ,               Payment_Mode          ,                   DebitAc_IdNo        ,               Reference_No ,        ChequeBank_Name        ,    Cheque_Date    ,              Remarks    ) " &
                                  "Values                                    ( '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & " ,     @EntryDate       , " & Str(Val(Mem_id)) & ",  " & Str(Val(txt_Amount.Text)) & "  , '" & Trim(cbo_PaymentMode.Text) & "',  " & Val(CrdtAc_id) & "  ,  '" & Trim(txt_ReferenceNo.Text) & "','" & Trim(cbo_chequeBank.Text) & "' ,  @ChequeDate  ,  '" & Trim(txt_Remarks.Text) & "' ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Weaver_Advance_Payment_Head set Weaver_Advance_Payment_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Mem_id)) & ", Amount = " & Str(Val(txt_Amount.Text)) & ",Payment_Mode = '" & Trim(cbo_PaymentMode.Text) & "', DebitAc_IdNo = " & Str(Val(CrdtAc_id)) & ", Reference_No ='" & Trim(txt_ReferenceNo.Text) & "' ,ChequeBank_Name ='" & Trim(cbo_chequeBank.Text) & "', Cheque_Date = @ChequeDate , Remarks = '" & Trim(txt_Remarks.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Advance_Payment_Code = '" & Trim(NewCode) & "'  "
                cmd.ExecuteNonQuery()

            End If



            Dim vVouNar As String = ""

            vVouNar = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" Then '---- Nachiyar Tradings
                vVouNar = "Vou.No : " & Trim(lbl_RecNo.Text)
            End If

            If Trim(UCase(cbo_PaymentMode.Text)) = "CASH" Then

                vVouNar = Trim(vVouNar) & "  -  Cash" & IIf(Trim(txt_Remarks.Text) <> "", " - ", "") & Trim(txt_Remarks.Text)

            Else

                If Trim(UCase(cbo_PaymentMode.Text)) = "CHEQUE" And Trim(txt_ReferenceNo.Text) <> "" Then
                    vVouNar = Trim(vVouNar) & "  -  Cheque No : " & Trim(txt_ReferenceNo.Text)

                Else
                    vVouNar = Trim(vVouNar) & "  -  " & Trim(cbo_PaymentMode.Text) & IIf(Trim(txt_ReferenceNo.Text) <> "", " : Ref. No. ", "") & Trim(txt_ReferenceNo.Text)

                End If

                If Trim(cbo_chequeBank.Text) <> "" Then
                    vVouNar = Trim(vVouNar) & " / " & Trim(cbo_chequeBank.Text)
                End If

                If Trim(txt_Remarks.Text) <> "" Then
                    vVouNar = Trim(vVouNar) & " / " & Trim(txt_Remarks.Text)
                End If

            End If


            vLed_IdNos = Mem_id & "|" & CrdtAc_id

            vVou_Amts = -1 * Val(txt_Amount.Text) & "|" & (Val(txt_Amount.Text))

            If Common_Procedures.Voucher_Updation(con, "Adv.Vou", Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_RecNo.Text), dtp_voucher_date.Value.Date, Trim(vVouNar) & " ", vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
                Exit Sub
            End If


            tr.Commit()

            If SaveAll_STS <> True Then

                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RecNo.Text)
                End If
            Else
                move_record(lbl_RecNo.Text)
            End If

            new_record()
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Msk_Date_Voucher.Enabled And Msk_Date_Voucher.Visible Then Msk_Date_Voucher.Focus()

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub cbo_EmployeeName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ledger_Head", "Ledger_Name", "(Ledger_Type='WEAVER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_EmployeeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, Msk_Date_Voucher, txt_Amount, "ledger_Head", "Ledger_Name", "(Ledger_Type='WEAVER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_EmployeeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "ledger_Head", "Ledger_Name", "(Ledger_Type='WEAVER')", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            txt_Amount.Focus()

            get_Ledger_CurrentBalance()

        End If
    End Sub

    Private Sub cbo_EmployeeName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub



    Private Sub btn_closefilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Emp_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Emp_IdNo = 0
            ' Itm_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Weaver_Advance_Payment_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Weaver_Advance_Payment_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Weaver_Advance_Payment_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_EmployeeFilter.Text) <> "" Then
                Emp_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_EmployeeFilter.Text)
            End If

            If Val(Emp_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Emp_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as Member_Name from Weaver_Advance_Payment_Head a INNER JOIN Ledger_Head b ON a.Ledger_Idno = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Advance_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_Advance_Payment_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()
                    dgv_filter.Rows(n).Cells(0).Value = " " & dt2.Rows(i).Item("Weaver_Advance_Payment_No").ToString
                    ' dgv_filter.Rows(n).Cells(1).Value = " " & dt2.Rows(i).Item("Voucher_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Advance_Payment_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Member_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub


    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String = ""

        Try

            If dgv_filter.Rows.Count > 0 Then

                movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

                If Val(movno) <> 0 Then

                    Filter_Status = True
                    move_record(movno)
                    pnl_back.Enabled = True
                    pnl_filter.Visible = False

                End If

            End If

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN OPEN FILTER.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub



    Private Sub cbo_CreditAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DebitorAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_idNo = 6 or AccountsGroup_idNo = 5)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_CreditAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DebitorAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DebitorAccount, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_idNo = 6 or AccountsGroup_idNo = 5)", "(Ledger_IdNo = 0)")


        If Asc(e.KeyChar) = 13 Then
            If txt_ReferenceNo.Enabled And txt_ReferenceNo.Visible = True Then
                txt_ReferenceNo.Focus()
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_CreditAccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DebitorAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DebitorAccount, cbo_PaymentMode, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_idNo = 6 or AccountsGroup_idNo = 5)", "(Ledger_IdNo = 0)") ' (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6)
        If (e.KeyValue = 38 And cbo_DebitorAccount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PaymentMode.Focus()
        End If
        If (e.KeyValue = 40 And cbo_DebitorAccount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True : e.SuppressKeyPress = True
            If txt_ReferenceNo.Enabled And txt_ReferenceNo.Visible = True Then
                txt_ReferenceNo.Focus()
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub



    Private Sub cbo_CashCheque_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMode.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMode, cbo_DebitorAccount, "", "", "", "")

        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_PaymentMode.Text) <> "CASH" Then

                Da1 = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Advance_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "and Weaver_Advance_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Payment_Mode ='CHEQUE' ", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    '  dtp_Date.Text = Dt1.Rows(0).Item("Weaver_Advance_Payment_Date").ToString
                    'cbo_PaymentMode.Text = Dt1.Rows(0).Item("Payment_Mode").ToString
                    cbo_DebitorAccount.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(0).Item("DebitAc_IdNo").ToString))

                End If
                Dt1.Clear()


            End If
        End If
    End Sub

    Private Sub cbo_CashCheque_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMode.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMode, txt_Amount, cbo_DebitorAccount, "", "", "", "")

    End Sub

    Private Sub txt_Amount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Amount.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True
            SendKeys.Send("+{TAB}")
        End If
        If e.KeyValue = 40 Then
            e.Handled = True
            cbo_PaymentMode.Focus()
        End If
    End Sub

    Private Sub txt_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            cbo_PaymentMode.Focus()
        End If
    End Sub

    Private Sub Employee_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim OrdByNo_Code As String = ""
        Dim VouCode As String = ""


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DebitorAccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DebitorAccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then

                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                If Val(RptSubReport_Index) > 0 And Trim(RptSubReport_VouCode) <> "" Then

                    Common_Procedures.CompIdNo = Val(Common_Procedures.Company_ShortNameToIdNo(con, RptSubReport_CompanyShortName))

                    If Common_Procedures.CompIdNo <> 0 Then

                        lbl_Company.Text = Common_Procedures.Company_IdNoToName(con, Common_Procedures.CompIdNo) & "  -  " & RptSubReport_CompanyShortName
                        lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                        Me.Text = lbl_Company.Text

                        OrdByNo_Code = ""
                        Da1 = New SqlClient.SqlDataAdapter("Select a.For_OrderBy from Weaver_Advance_Payment_Head a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Advance_Payment_Code = '" & Trim(RptSubReport_VouCode) & "' ", con)
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                                OrdByNo_Code = Dt1.Rows(0)(0).ToString
                            End If
                        End If
                        Dt1.Clear()

                        If Val(OrdByNo_Code) <> 0 Then
                            VouCode = Common_Procedures.OrderBy_ValueToCode(Format(Val(OrdByNo_Code), "#########0.00"))
                            move_record(VouCode)
                        End If

                    End If

                Else

                    lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                    lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                    Me.Text = lbl_Company.Text

                    new_record()

                End If

            End If

        Catch ex As Exception

            '---MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False


    End Sub

    Private Sub Employee_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next

        Open_Report()

        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name

    End Sub

    Private Sub YarnDelivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                ElseIf MessageBox.Show("Do you want to Close ?", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Close_Form()
                Else
                    Exit Sub
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Close_Form()

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            If Val(RptSubReport_Index) > 0 And Trim(RptSubReport_VouCode) <> "" And Trim(RptSubReport_CompanyShortName) <> "" Then
                Me.Close()

            Else


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

    Private Sub Open_Report()
        Dim I As Integer = 0
        Dim J As Integer = 0
        Dim RptIpDet_ReportGroupName As String = ""
        Dim RptIpDet_ReportName As String = ""
        Dim RptIpDet_ReportHeading As String = ""
        Dim RptIpDet_IsGridReport As Boolean = False
        Dim RptIpDet_ReportInputs As String = ""
        Dim vCurRow As Integer = -1
        Dim vTopRow As Integer = -1
        Dim vDateInp1 As Date
        Dim vDateInp2 As Date
        Dim vCboInpText1 As String = ""
        Dim vCboInpText2 As String = ""
        Dim vCboInpText3 As String = ""
        Dim vCboInpText4 As String = ""
        Dim vCboInpText5 As String = ""

        Try

            If Val(RptSubReport_Index) > 0 And Val(RptSubReport_VouNo) > 0 And Trim(RptSubReport_CompanyShortName) <> "" Then

                RptIpDet_ReportName = RptSubReportDet(RptSubReport_Index).ReportName
                RptIpDet_ReportGroupName = RptSubReportDet(RptSubReport_Index).ReportGroupName
                RptIpDet_ReportHeading = RptSubReportDet(RptSubReport_Index).ReportHeading
                RptIpDet_ReportInputs = RptSubReportDet(RptSubReport_Index).ReportInputs
                RptIpDet_IsGridReport = RptSubReportDet(RptSubReport_Index).IsGridReport
                vCurRow = Val(RptSubReportDet(RptSubReport_Index).CurrentRowVal)
                vTopRow = Val(RptSubReportDet(RptSubReport_Index).TopRowVal)
                vDateInp1 = RptSubReportDet(RptSubReport_Index).DateInp_Value1
                vDateInp2 = RptSubReportDet(RptSubReport_Index).DateInp_Value2
                vCboInpText1 = RptSubReportDet(RptSubReport_Index).CboInp_Text1
                vCboInpText2 = RptSubReportDet(RptSubReport_Index).CboInp_Text2
                vCboInpText3 = RptSubReportDet(RptSubReport_Index).CboInp_Text3
                vCboInpText4 = RptSubReportDet(RptSubReport_Index).CboInp_Text4
                vCboInpText5 = RptSubReportDet(RptSubReport_Index).CboInp_Text5

                RptSubReportDet(RptSubReport_Index).ReportName = ""
                RptSubReportDet(RptSubReport_Index).ReportGroupName = ""
                RptSubReportDet(RptSubReport_Index).ReportHeading = ""
                RptSubReportDet(RptSubReport_Index).ReportInputs = ""
                RptSubReportDet(RptSubReport_Index).IsGridReport = False
                RptSubReportDet(RptSubReport_Index).CurrentRowVal = -1
                RptSubReportDet(RptSubReport_Index).TopRowVal = -1
                RptSubReportDet(RptSubReport_Index).DateInp_Value1 = #1/1/1900#
                RptSubReportDet(RptSubReport_Index).DateInp_Value2 = #1/1/1900#
                RptSubReportDet(RptSubReport_Index).CboInp_Text1 = ""
                RptSubReportDet(RptSubReport_Index).CboInp_Text2 = ""
                RptSubReportDet(RptSubReport_Index).CboInp_Text3 = ""
                RptSubReportDet(RptSubReport_Index).CboInp_Text4 = ""
                RptSubReportDet(RptSubReport_Index).CboInp_Text5 = ""

                For I = 1 To 10

                    RptSubReportInpDet(RptSubReport_Index, I).PKey = ""
                    RptSubReportInpDet(RptSubReport_Index, I).TableName = ""
                    RptSubReportInpDet(RptSubReport_Index, I).Selection_FieldName = ""
                    RptSubReportInpDet(RptSubReport_Index, I).Return_FieldName = ""
                    RptSubReportInpDet(RptSubReport_Index, I).Condition = ""
                    RptSubReportInpDet(RptSubReport_Index, I).Display_Name = ""
                    RptSubReportInpDet(RptSubReport_Index, I).BlankFieldCondition = ""
                    RptSubReportInpDet(RptSubReport_Index, I).CtrlType_Cbo_OR_Txt = ""

                Next I

                RptSubReport_Index = RptSubReport_Index - 1


                Common_Procedures.RptInputDet.ReportGroupName = RptIpDet_ReportGroupName
                Common_Procedures.RptInputDet.ReportName = RptIpDet_ReportName
                Common_Procedures.RptInputDet.ReportHeading = RptIpDet_ReportHeading
                Common_Procedures.RptInputDet.IsGridReport = RptIpDet_IsGridReport
                Common_Procedures.RptInputDet.ReportInputs = RptIpDet_ReportInputs

                Dim f As New Report_Details

                f.RptSubReport_Index = RptSubReport_Index

                For I = 1 To 10

                    f.RptSubReportDet(I).ReportName = RptSubReportDet(I).ReportName
                    f.RptSubReportDet(I).ReportGroupName = RptSubReportDet(I).ReportGroupName
                    f.RptSubReportDet(I).ReportHeading = RptSubReportDet(I).ReportHeading
                    f.RptSubReportDet(I).ReportInputs = RptSubReportDet(I).ReportInputs
                    f.RptSubReportDet(I).IsGridReport = RptSubReportDet(I).IsGridReport
                    f.RptSubReportDet(I).CurrentRowVal = RptSubReportDet(I).CurrentRowVal
                    f.RptSubReportDet(I).TopRowVal = RptSubReportDet(I).TopRowVal

                    f.RptSubReportDet(I).DateInp_Value1 = RptSubReportDet(I).DateInp_Value1
                    f.RptSubReportDet(I).DateInp_Value2 = RptSubReportDet(I).DateInp_Value2
                    f.RptSubReportDet(I).CboInp_Text1 = RptSubReportDet(I).CboInp_Text1
                    f.RptSubReportDet(I).CboInp_Text2 = RptSubReportDet(I).CboInp_Text2
                    f.RptSubReportDet(I).CboInp_Text3 = RptSubReportDet(I).CboInp_Text3
                    f.RptSubReportDet(I).CboInp_Text4 = RptSubReportDet(I).CboInp_Text4
                    f.RptSubReportDet(I).CboInp_Text5 = RptSubReportDet(I).CboInp_Text5

                    For J = 1 To 10

                        f.RptSubReportInpDet(I, J).PKey = RptSubReportInpDet(I, J).PKey
                        f.RptSubReportInpDet(I, J).TableName = RptSubReportInpDet(I, J).TableName
                        f.RptSubReportInpDet(I, J).Selection_FieldName = RptSubReportInpDet(I, J).Selection_FieldName
                        f.RptSubReportInpDet(I, J).Return_FieldName = RptSubReportInpDet(I, J).Return_FieldName
                        f.RptSubReportInpDet(I, J).Condition = RptSubReportInpDet(I, J).Condition
                        f.RptSubReportInpDet(I, J).Display_Name = RptSubReportInpDet(I, J).Display_Name
                        f.RptSubReportInpDet(I, J).BlankFieldCondition = RptSubReportInpDet(I, J).BlankFieldCondition
                        f.RptSubReportInpDet(I, J).CtrlType_Cbo_OR_Txt = RptSubReportInpDet(I, J).CtrlType_Cbo_OR_Txt

                    Next J

                Next I

                f.MdiParent = MDIParent1
                f.Show()

                f.msk_FromDate.Text = vDateInp1.ToShortDateString
                f.msk_ToDate.Text = vDateInp2.ToShortDateString

                f.cbo_Inputs1.Text = vCboInpText1
                f.cbo_Inputs2.Text = vCboInpText2
                f.cbo_Inputs3.Text = vCboInpText3
                f.cbo_Inputs4.Text = vCboInpText4
                f.cbo_Inputs5.Text = vCboInpText5

                f.Show_Report()

                If vCurRow > 0 Then
                    If f.dgv_Report.Rows.Count > 0 And f.dgv_Report.Rows.Count >= vCurRow Then
                        f.dgv_Report.CurrentCell = f.dgv_Report.Rows(vCurRow).Cells(0)
                        f.dgv_Report.CurrentCell.Selected = True
                    End If
                End If
                If vTopRow > 0 Then
                    If f.dgv_Report.Rows.Count > 0 And f.dgv_Report.Rows.Count >= vTopRow Then
                        f.dgv_Report.FirstDisplayedScrollingRowIndex = vTopRow
                    End If
                End If

            End If


        Catch ex As Exception

            '-----

        End Try

    End Sub


    Private Sub Employee_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""


        con.Open()

        cbo_PaymentMode.Items.Clear()
        cbo_PaymentMode.Items.Add("")
        cbo_PaymentMode.Items.Add("CASH")
        cbo_PaymentMode.Items.Add("CHEQUE")
        cbo_PaymentMode.Items.Add("IMPS")
        cbo_PaymentMode.Items.Add("NEFT")
        cbo_PaymentMode.Items.Add("RTGS")
        cbo_PaymentMode.Items.Add("UPI")

        pnl_CurrentBalance.Visible = False

        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMode.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DebitorAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReferenceNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterFrom_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterTo_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EmployeeFilter.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_chequeBank.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler Msk_Date_Voucher.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_voucher_date.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DebitorAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReferenceNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterFrom_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterTo_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EmployeeFilter.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_chequeBank.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus

        'AddHandler cbo_MillFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        ' AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler Msk_Date_Voucher.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_voucher_date.LostFocus, AddressOf ControlLostFocus

        '  AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReferenceNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown



        '   AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReferenceNo.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub cbo_EmployeeFilter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EmployeeFilter.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_EmployeeFilter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EmployeeFilter.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EmployeeFilter, dtp_FilterTo_date, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyNameFilter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EmployeeFilter.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EmployeeFilter, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_filtershow_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub txt_Amount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Amount.LostFocus
        txt_Amount.Text = Format(Val(txt_Amount.Text), "#########0.00")
    End Sub

    Private Sub cbo_DebitAccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DebitorAccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DebitorAccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
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

        LastNo = lbl_RecNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RecNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub txt_EMI_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    'Private Sub cbo_chitgroupName_GotFocus(sender As Object, e As System.EventArgs)
    '    Dim vMem_id As Integer

    '    vMem_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_MemberName.Text)
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Chit_Group_Head", "Chit_Group_Name", "(Chit_Group_Name IN (select  sq1.Chit_Group_Name from  Chit_Group_Head sq1 , Chit_Group_Details sq2 Where sq2.Member_IdNo = " & Str(Val(vMem_id)) & " And sq1.Chit_Group_RefCode = sq2.Chit_Group_RefCode ) )", "(Chit_Group_Name = '')")
    'End Sub

    'Private Sub cbo_chitgroupName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs)
    '    Dim vMem_id As Integer

    '    vMem_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_MemberName.Text)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_chitgroupName, cbo_MemberName, cbo_CashCheque, "Chit_Group_Head", "Chit_Group_Name", "(Chit_Group_Name IN (select  sq1.Chit_Group_Name from  Chit_Group_Head sq1 , Chit_Group_Details sq2 Where sq2.Member_IdNo = " & Str(Val(vMem_id)) & " And sq1.Chit_Group_RefCode = sq2.Chit_Group_RefCode ) )", "(Chit_Group_Name = '')")
    'End Sub

    'Private Sub cbo_chitgroupName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
    '    Dim vMem_id As Integer

    '    vMem_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_MemberName.Text)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_chitgroupName, cbo_CashCheque, "Chit_Group_Head", "Chit_Group_Name", "(Chit_Group_Name IN (select  sq1.Chit_Group_Name from  Chit_Group_Head sq1 , Chit_Group_Details sq2 Where sq2.Member_IdNo = " & Str(Val(vMem_id)) & " And sq1.Chit_Group_RefCode = sq2.Chit_Group_RefCode ) )", "(Chit_Group_Name = '')")
    'End Sub

    Private Sub msk_Date_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

        If e.KeyValue = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_chequeBank.Focus()
        End If
        If e.KeyValue = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Remarks.Focus()

        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If UCase(Chr(Asc(e.KeyChar))) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            txt_Remarks.Focus()
        End If
        '  e.Handled = True : cbo_Ledger.Focus()
    End Sub

    Private Sub msk_Date_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmsRetTxt As String = ""
        Dim vmsRetvl As Integer = -1


        If IsDate(msk_Date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
                msk_Date.SelectionStart = 0
            ElseIf e.KeyCode = 109 Then
                msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
                msk_Date.SelectionStart = 0
            End If
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub txt_Remarks_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True : e.SuppressKeyPress = True

            If msk_Date.Enabled = True And msk_Date.Visible = True Then
                msk_Date.Focus()
            Else
                cbo_DebitorAccount.Focus()
            End If

        End If
        If e.KeyValue = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                Msk_Date_Voucher.Focus()
            End If
        End If

    End Sub

    Private Sub txt_Remarks_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Asc(e.KeyChar) = 13 Then
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    Msk_Date_Voucher.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_chequeBank_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_chequeBank.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Advance_Payment_Head", "ChequeBank_Name", "", "")
    End Sub

    Private Sub cbo_chequeBank_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_chequeBank.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_chequeBank, txt_ReferenceNo, msk_Date, "Weaver_Advance_Payment_Head", "ChequeBank_Name", "", "")
    End Sub

    Private Sub cbo_chequeBank_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_chequeBank.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_chequeBank, msk_Date, "Weaver_Advance_Payment_Head", "ChequeBank_Name", "", "", False)
    End Sub

    Private Sub dtp_cheque_date_Enter(sender As Object, e As System.EventArgs) Handles dtp_cheque_date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
    End Sub

    Private Sub dtp_cheque_date_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_cheque_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_cheque_date_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_cheque_date.KeyUp

        If e.KeyCode = 17 And e.Control = False And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_cheque_date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_cheque_date_TextChanged(sender As Object, e As System.EventArgs) Handles dtp_cheque_date.TextChanged
        If IsDate(dtp_cheque_date.Text) = True Then
            msk_Date.Text = dtp_cheque_date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub cbo_PaymentMode_TextChanged(sender As Object, e As System.EventArgs) Handles cbo_PaymentMode.TextChanged
        If Trim(cbo_PaymentMode.Text) = "CASH" Then
            txt_ReferenceNo.Enabled = False
            cbo_chequeBank.Enabled = False
            msk_Date.Enabled = False
            dtp_cheque_date.Enabled = False
            msk_Date.Text = ""
            cbo_DebitorAccount.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        ElseIf Trim(cbo_PaymentMode.Text) = "CHEQUE" Then
            txt_ReferenceNo.Enabled = True
            cbo_chequeBank.Enabled = True
            msk_Date.Enabled = True
            dtp_cheque_date.Enabled = True
            '  cbo_DebitorAccount.Text = ""
        Else
            txt_ReferenceNo.Enabled = True
            cbo_chequeBank.Enabled = True
            msk_Date.Enabled = True
            dtp_cheque_date.Enabled = True
            ' cbo_DebitorAccount.Text = ""
        End If
    End Sub

    Private Sub Msk_Date_Voucher_GotFocus(sender As Object, e As System.EventArgs) Handles Msk_Date_Voucher.GotFocus
        pnl_CurrentBalance.Visible = False
    End Sub

    'Private Sub dtp_Date_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue
    '    If e.KeyValue = 38 Then
    '        e.Handled = True : e.SuppressKeyPress = True
    '        txt_Remarks.Focus()
    '    End If
    '    If e.KeyValue = 40 Then
    '        e.Handled = True : e.SuppressKeyPress = True
    '        cbo_PartyName.Focus()
    '    End If

    'End Sub

    'Private Sub dtp_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
    '    If Asc(e.KeyChar) = 13 Then
    '        cbo_PartyName.Focus()
    '    End If
    'End Sub

    'Private Sub dtp_Date_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs)
    '    If e.KeyCode = 17 And e.Control = False And vcbo_KeyDwnVal = e.KeyValue Then
    '        dtp_Date.Text = Date.Today
    '    End If
    'End Sub

    Private Sub Msk_Date_Voucher_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Msk_Date_Voucher.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = Msk_Date_Voucher.Text
            vmskSelStrt = Msk_Date_Voucher.SelectionStart
        End If

        If e.KeyValue = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Remarks.Focus()
        End If
        If e.KeyValue = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PartyName.Focus()

        End If
    End Sub

    Private Sub Msk_Date_Voucher_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Msk_Date_Voucher.KeyPress
        If UCase(Chr(Asc(e.KeyChar))) = "D" Then
            Msk_Date_Voucher.Text = Date.Today
            Msk_Date_Voucher.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            cbo_PartyName.Focus()
        End If
    End Sub

    Private Sub Msk_Date_Voucher_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Msk_Date_Voucher.KeyUp
        Dim vmsRetTxt As String = ""
        Dim vmsRetvl As Integer = -1


        If IsDate(Msk_Date_Voucher.Text) = True Then
            If e.KeyCode = 107 Then
                Msk_Date_Voucher.Text = DateAdd("D", 1, Convert.ToDateTime(Msk_Date_Voucher.Text))
                Msk_Date_Voucher.SelectionStart = 0
            ElseIf e.KeyCode = 109 Then
                Msk_Date_Voucher.Text = DateAdd("D", -1, Convert.ToDateTime(Msk_Date_Voucher.Text))
                Msk_Date_Voucher.SelectionStart = 0
            End If
            'dtp_voucher_date.Text = Convert.ToDateTime(Msk_Date_Voucher.Text)
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub

    Private Sub dtp_voucher_date_Enter(sender As Object, e As System.EventArgs) Handles dtp_voucher_date.Enter
        Msk_Date_Voucher.Focus()
        Msk_Date_Voucher.SelectionStart = 0
    End Sub

    Private Sub dtp_voucher_date_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_voucher_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_voucher_date_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_voucher_date.KeyUp

        If e.KeyCode = 17 And e.Control = False And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_voucher_date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_voucher_date_TextChanged(sender As Object, e As System.EventArgs) Handles dtp_voucher_date.TextChanged
        If IsDate(dtp_voucher_date.Text) = True Then
            Msk_Date_Voucher.Text = dtp_voucher_date.Text
            Msk_Date_Voucher.SelectionStart = 0
        End If
    End Sub

    Private Sub Msk_Date_Voucher_LostFocus(sender As Object, e As System.EventArgs) Handles Msk_Date_Voucher.LostFocus

        If IsDate(Msk_Date_Voucher.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(Msk_Date_Voucher.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(Msk_Date_Voucher.Text)) <= 12 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(Msk_Date_Voucher.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(Msk_Date_Voucher.Text)) >= 2000 Then
                    dtp_voucher_date.Value = Convert.ToDateTime(Msk_Date_Voucher.Text)
                End If
            End If

        End If
    End Sub
    Private Sub get_Ledger_CurrentBalance()
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim BalAmt As Double = 0
        Dim GpCd As String = ""
        Dim Datcondt As String = ""
        Dim n As Integer = 0
        Dim I As Integer = 0
        Dim Led_ID As Integer = 0

        Try

            lbl_CurrentBalance.Text = "Current Balance :"

            '-----------BALANCE

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

            '  With dgv_Details
            '  If .Rows.Count > 0 Then

            '  n = .CurrentRow.Index

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

            If Led_ID <> 0 Then
                GpCd = Common_Procedures.get_FieldValue(con, "ledger_head", "parent_code", "(ledger_idno = " & Str(Val(Led_ID)) & ")")
                If GpCd Like "*~18~*" Then Datcondt = " and a.Voucher_date >= @companyfromdate " Else Datcondt = ""

                cmd.CommandText = "select sum(a.Voucher_amount) as BalAmount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " " & Datcondt

                da = New SqlClient.SqlDataAdapter(cmd)
                'da = New SqlClient.SqlDataAdapter("select sum(a.Voucher_amount) as BalAmount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " " & Datcondt, con)
                dt1 = New DataTable
                da.Fill(dt1)

                BalAmt = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        BalAmt = Val(dt1.Rows(0).Item("BalAmount").ToString)
                    End If
                End If
                dt1.Clear()

                dt1.Dispose()
                da.Dispose()
                cmd.Dispose()

                '   If Trim(UCase(.Rows(n).Cells(0).Value)) = "DR" Then BalAmt = BalAmt - Val(.Rows(n).Cells(2).Value)
                '  If Trim(UCase(.Rows(n).Cells(0).Value)) = "CR" Then BalAmt = BalAmt + Val(.Rows(n).Cells(3).Value)

                For I = 0 To UBound(VouAmtAr)
                    If Val(Led_ID) = Val(VouAmtAr(I).LedgerIdNo) Then BalAmt = BalAmt - Val(VouAmtAr(I).VoucherAmount)
                Next I

                lbl_CurrentBalance.Tag = n
                lbl_CurrentBalance.Text = "Current Balance : " & Trim(Common_Procedures.Currency_Format(Math.Abs(Val(BalAmt)))) & IIf(Val(BalAmt) >= 0, " Cr", " Dr")
                pnl_CurrentBalance.Visible = True

            Else
                lbl_CurrentBalance.Tag = -100
                lbl_CurrentBalance.Text = "Current Balance : "
                pnl_CurrentBalance.Visible = False

            End If

            'End If

            'End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE GETTI CURRENT BALANCE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Remarks_LostFocus(sender As Object, e As System.EventArgs) Handles txt_Remarks.LostFocus
        pnl_CurrentBalance.Visible = False
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btn_prnt.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        ' Print_PDF_Status = False
        print_record()

    End Sub
End Class