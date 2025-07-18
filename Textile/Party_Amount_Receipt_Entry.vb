Public Class Party_Amount_Receipt_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "AMTRC-"
    Private Pk_Condition2 As String = "AGCOM-"
    Private Pk_Condition3 As String = "PTTDS"
    Private Prec_ActCtrl As New Control

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private vcbo_KeyDwnVal As Double

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String

    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Dim vTotCommAmt As Single

    Public Enum dgvCol_Detail As Integer
        SLNO                '0
        BILLNO              '1
        BILLAMOUNT          '2
        BILLBALANCEAMOUNT   '3
        RECEIPTAMOUNT       '4
        CASH_DISC_PERC      '5
        CASH_DISC_AMOUNT    '6
        RATE_DIFFERENCE     '7
        DD_COMMISSION       '8
        OTHERS              '9
        AGENT_COMM_PERC     '10
        AGENT_COMM_AMOUNT   '11
        AGENT_TDS_PERC      '12
        AGENT_TDS_AMOUNT    '13
        Voucher_Bill_Code   '14
        Voucher_Bill_date   '15
        agent_name          '16
        gross_amount        '17
        PARTY_TDS_PERC      '18   
        PARTY_TDS_AMOUNT    '19
        NETBALANCEAMOUNT    '20
        DAYS                '21
        INVNO               '22
    End Enum

    Public Enum dgvCol_Selection As Integer
        SLNO                '0
        BILLNO              '1
        BILLDATE            '2
        BILLAMOUNT          '3
        BILLBALANCEAMOUNT   '4
        AGENTNAME           '5
        STS                 '6
        Voucher_Bill_Code   '7
        GROSSAMOUNT         '8
        RECEIPTAMOUNT       '9
        CASH_DISC_AMOUNT    '10
        RATE_DIFFERENCE     '11
        DD_COMMISSION       '12  
        OTHERS              '13
        AGENT_COMM_PERC     '14
        AGENT_COMM_AMOUNT   '15
        AGENT_TDS_PERC      '16
        AGENT_TDS_AMOUNT    '17
        AGENTIDNO           '18
        DAYS                '19
        CASH_DISC_PERC      '20
        ORDERNO             '21
        INVNO               '22
    End Enum

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_ReceiptNo.Text = ""
        lbl_ReceiptNo.ForeColor = Color.Black

        dtp_ReceiptDate.Text = ""
        msk_ReceiptDate.Text = ""
        cbo_Ledger.Text = ""

        cbo_DebtorName.Text = ""
        txt_PreparedBy.Text = ""

        txt_ChequeNo.Text = ""
        txt_Narration.Text = ""
        cbo_Filter_PartyName.Text = ""
        cbo_Filter_DebitorName.Text = ""
        txt_FilterChequeNo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()
        dgv_Details_Total.Rows.Clear()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_DebitorName.Text = ""
            cbo_Filter_DebitorName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_DebtorName.Enabled = True
        cbo_DebtorName.BackColor = Color.White

        txt_ChequeNo.Enabled = True
        txt_ChequeNo.BackColor = Color.White

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
        Dim LockSTS As Boolean = False
        Dim vdys As Long = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Debtor_Name from Party_Amount_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Debtor_IdNo = c.Ledger_IdNo  Where a.Party_Receipt_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_ReceiptNo.Text = dt1.Rows(0).Item("Party_Receipt_No").ToString
                dtp_ReceiptDate.Text = dt1.Rows(0).Item("Party_Receipt_Date").ToString
                msk_ReceiptDate.Text = dtp_ReceiptDate.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_DebtorName.Text = dt1.Rows(0).Item("Debtor_Name").ToString
                txt_ChequeNo.Text = dt1.Rows(0).Item("Cheque_No").ToString
                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                If IsDBNull(dt1.Rows(0).Item("Cheque_Return_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Cheque_Return_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                txt_PreparedBy.Text = dt1.Rows(0).Item("PreparedBy").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Voucher_Bill_Date, b.debit_amount, (b.debit_amount - b.credit_amount + a.receipt_amount + a.discount_amount + a.rate_difference + a.dd_commission + a.others+a.Party_tds_Amount) as bill_balance, (b.debit_amount - b.credit_amount) as receipt_balance, c.Ledger_name as Agent_Name from Party_Amount_Receipt_Details a INNER JOIN voucher_bill_head b ON a.Voucher_Bill_Code = b.Voucher_Bill_Code LEFT OUTER JOIN Ledger_Head c ON a.Agent_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Party_Receipt_code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.SLNO).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.BILLNO).Value = dt2.Rows(i).Item("Party_Bill_No").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.BILLAMOUNT).Value = Format(Val(dt2.Rows(i).Item("debit_amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value = Format(Val(dt2.Rows(i).Item("bill_balance").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value = Format(Val(dt2.Rows(i).Item("Receipt_Amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.CASH_DISC_PERC).Value = Format(Val(dt2.Rows(i).Item("discount_Percentage").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.CASH_DISC_PERC).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.CASH_DISC_PERC).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value = Format(Val(dt2.Rows(i).Item("discount_amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value = Format(Val(dt2.Rows(i).Item("rate_difference").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.DD_COMMISSION).Value = Format(Val(dt2.Rows(i).Item("dd_commission").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.DD_COMMISSION).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.DD_COMMISSION).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.OTHERS).Value = Format(Val(dt2.Rows(i).Item("Others").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.OTHERS).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.OTHERS).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value = Val(dt2.Rows(i).Item("Agent_Comm_Percentage").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value = Format(Val(dt2.Rows(i).Item("Agent_Comm_Amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value = Val(dt2.Rows(i).Item("Agent_Tds_Percentage").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value = Format(Val(dt2.Rows(i).Item("Agent_tds_Amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value = ""
                        End If
                        If Common_Procedures.settings.CustomerCode = "1005" Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.NETBALANCEAMOUNT).Value = Format(Val(dt2.Rows(i).Item("bill_balance").ToString), "########0.00")
                        Else
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.NETBALANCEAMOUNT).Value = Format(Val(dt2.Rows(i).Item("Receipt_balance").ToString), "########0.00")
                        End If

                        dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value = Val(dt2.Rows(i).Item("Party_Tds_Percentage").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value = ""
                        End If

                        dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value = Format(Val(dt2.Rows(i).Item("Party_tds_Amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value = ""
                        End If

                        dgv_Details.Rows(n).Cells(dgvCol_Detail.Voucher_Bill_Code).Value = dt2.Rows(i).Item("Voucher_Bill_Code").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.Voucher_Bill_date).Value = dt2.Rows(i).Item("Voucher_Bill_Date").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.agent_name).Value = dt2.Rows(i).Item("Agent_Name").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.gross_amount).Value = ""



                        vdys = DateDiff(DateInterval.Day, dt2.Rows(i).Item("Voucher_Bill_Date"), Convert.ToDateTime(msk_ReceiptDate.Text))

                        dgv_Details.Rows(n).Cells(dgvCol_Detail.DAYS).Value = vdys

                        dgv_Details.Rows(n).Cells(dgvCol_Detail.INVNO).Value = dt2.Rows(i).Item("Party_Invoice_No").ToString

                    Next i

                End If
                dt2.Clear()

                Total_Calculation()

                If LockSTS = True Then

                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray

                    cbo_DebtorName.Enabled = False
                    cbo_DebtorName.BackColor = Color.LightGray

                    txt_ChequeNo.Enabled = False
                    txt_ChequeNo.BackColor = Color.LightGray

                End If

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
            If msk_ReceiptDate.Visible And msk_ReceiptDate.Enabled Then msk_ReceiptDate.Focus()

        End Try



    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Amount_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DebtorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DebtorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Amount_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim vTotCommAmt As Single = 0

        Me.Text = ""

        con.Open()

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_Filter_PartyName, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        dgv_Details.Columns(dgvCol_Detail.AGENT_COMM_PERC).Visible = False
        dgv_Details.Columns(dgvCol_Detail.AGENT_COMM_AMOUNT).Visible = False
        dgv_Details.Columns(dgvCol_Detail.AGENT_TDS_PERC).Visible = False
        dgv_Details.Columns(dgvCol_Detail.AGENT_TDS_AMOUNT).Visible = False

        dgv_Details_Total.Columns(dgvCol_Detail.AGENT_COMM_PERC).Visible = False
        dgv_Details_Total.Columns(dgvCol_Detail.AGENT_COMM_AMOUNT).Visible = False
        dgv_Details_Total.Columns(dgvCol_Detail.AGENT_TDS_PERC).Visible = False
        dgv_Details_Total.Columns(dgvCol_Detail.AGENT_TDS_AMOUNT).Visible = False


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then

            dgv_Details.Columns(dgvCol_Detail.AGENT_COMM_PERC).Visible = True
            dgv_Details.Columns(dgvCol_Detail.AGENT_COMM_AMOUNT).Visible = True
            dgv_Details.Columns(dgvCol_Detail.AGENT_TDS_PERC).Visible = True
            dgv_Details.Columns(dgvCol_Detail.AGENT_TDS_AMOUNT).Visible = True
            dgv_Details.Columns(dgvCol_Detail.PARTY_TDS_PERC).Visible = True ' False
            dgv_Details.Columns(dgvCol_Detail.PARTY_TDS_AMOUNT).Visible = True ' False

            dgv_Details_Total.Columns(dgvCol_Detail.AGENT_COMM_PERC).Visible = True
            dgv_Details_Total.Columns(dgvCol_Detail.AGENT_COMM_AMOUNT).Visible = True
            dgv_Details_Total.Columns(dgvCol_Detail.AGENT_TDS_PERC).Visible = True
            dgv_Details_Total.Columns(dgvCol_Detail.AGENT_TDS_AMOUNT).Visible = True
            dgv_Details_Total.Columns(dgvCol_Detail.PARTY_TDS_PERC).Visible = True ' False
            dgv_Details_Total.Columns(dgvCol_Detail.PARTY_TDS_AMOUNT).Visible = True 'False

        End If

        dgv_Selection.Columns(dgvCol_Selection.ORDERNO).Visible = False
        dgv_Selection.Columns(dgvCol_Selection.INVNO).Visible = False
        dgv_Details.Columns(dgvCol_Detail.INVNO).Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1360" Then '---- Ashoka Textile (63.Velampalayam - Palladam)

            dgv_Details.Columns(dgvCol_Detail.INVNO).Visible = True

            dgv_Details.Columns(dgvCol_Detail.BILLNO).Width = dgv_Details.Columns(dgvCol_Detail.BILLNO).Width + 150
            dgv_Details_Total.Columns(dgvCol_Detail.BILLNO).Width = dgv_Details.Columns(dgvCol_Detail.BILLNO).Width

            dgv_Selection.Columns(dgvCol_Selection.BILLNO).Width = dgv_Selection.Columns(dgvCol_Selection.BILLNO).Width + 100
            dgv_Selection.Columns(dgvCol_Selection.AGENTNAME).Width = dgv_Selection.Columns(dgvCol_Selection.AGENTNAME).Width - 125

            dgv_Selection.Columns(dgvCol_Selection.INVNO).Visible = True
            dgv_Selection.Columns(dgvCol_Selection.BILLAMOUNT).Width = dgv_Selection.Columns(dgvCol_Selection.BILLAMOUNT).Width - 25
            dgv_Selection.Columns(dgvCol_Selection.BILLBALANCEAMOUNT).Width = dgv_Selection.Columns(dgvCol_Selection.BILLBALANCEAMOUNT).Width - 25

        End If


        AddHandler msk_ReceiptDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DebtorName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_DebitorName.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_ChequeNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FilterChequeNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PreparedBy.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_ReceiptDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DebtorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_DebitorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ChequeNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FilterChequeNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PreparedBy.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_ReceiptDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ChequeNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FilterChequeNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_FilterChequeNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_ReceiptDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ChequeNo.KeyPress, AddressOf TextBoxControlKeyPress

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

    Private Sub Amount_Receipt_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Amount_Receipt_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 8 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                cbo_DebtorName.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Detail.RECEIPTAMOUNT)
                            End If

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.CASH_DISC_AMOUNT Then
                            If .Columns(.CurrentCell.ColumnIndex + 1).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 2).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 3).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 3)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 4).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 4)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 5).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 5)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Detail.PARTY_TDS_PERC)
                            End If

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_COMM_PERC Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.OTHERS Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Detail.PARTY_TDS_PERC)
                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_TDS_PERC Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Detail.PARTY_TDS_PERC)
                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_TDS_AMOUNT Then
                            If .Columns(.CurrentCell.ColumnIndex + 1).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 2).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 3).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 3)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 4).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 4)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 5).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 5)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 6).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 6)
                            ElseIf .Columns(.CurrentCell.ColumnIndex + 7).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 7)
                            End If


                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= dgvCol_Detail.RECEIPTAMOUNT Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Ledger.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 8)


                            End If
                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_COMM_PERC Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Detail.CASH_DISC_AMOUNT)
                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_TDS_PERC Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Detail.AGENT_COMM_PERC)
                            '.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 2)
                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.PARTY_TDS_PERC Then
                            If .Columns(dgvCol_Detail.OTHERS).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Detail.OTHERS)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Detail.AGENT_TDS_PERC)
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
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text)

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Party_Receipt_Entry_Cheque_Cash, "~L~") = 0 And InStr(Common_Procedures.UR.Party_Receipt_Entry_Cheque_Cash, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Party_Amount_Receipt_Entry, New_Entry, Me, con, "Party_Amount_Receipt_Head", "Party_Receipt_Code", NewCode, "Party_Receipt_Date", "(Party_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Party_Amount_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "' and  Cheque_Return_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Cheque Returned", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Party_Amount_Receipt_Head", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Party_Receipt_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Party_Amount_Receipt_Details", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Party_Bill_No  , Receipt_Amount  , discount_Percentage , discount_amount  , rate_difference ,  dd_commission,   Others   , Agent_Comm_Percentage,  Agent_Comm_Amount   ,  Agent_Tds_Percentage ,Agent_tds_Amount , Voucher_Bill_Code , Agent_IdNo  , Total_Receipt_Amount ", "Sl_No", "Party_Receipt_Code, For_OrderBy, Company_IdNo, Party_Receipt_No, Party_Receipt_Date, Ledger_Idno, Party_Tds_Percentage,Party_tds_Amount", trans)


            cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.voucher_bill_code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "delete from Party_Amount_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Party_Amount_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "'"
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

            If msk_ReceiptDate.Enabled = True And msk_ReceiptDate.Visible = True Then msk_ReceiptDate.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_Filter_DebitorName, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6)", "(Ledger_IdNo = 0)")

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

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

            da = New SqlClient.SqlDataAdapter("select top 1 Party_Receipt_No from Party_Amount_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Party_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Party_Receipt_No from Party_Amount_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Party_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Party_Receipt_No from Party_Amount_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Party_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Party_Receipt_No from Party_Amount_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Party_Receipt_No desc", con)
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

            lbl_ReceiptNo.Text = Common_Procedures.get_MaxCode(con, "Party_Amount_Receipt_Head", "Party_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_ReceiptNo.ForeColor = Color.Red

            msk_ReceiptDate.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Party_Amount_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Party_Receipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Party_Receipt_Date").ToString <> "" Then msk_ReceiptDate.Text = dt1.Rows(0).Item("Party_Receipt_Date").ToString
                End If
            End If
            dt1.Clear()


            If msk_ReceiptDate.Enabled And msk_ReceiptDate.Visible Then msk_ReceiptDate.Focus() : msk_ReceiptDate.SelectionStart = 0

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

            inpno = InputBox("Enter Receipt.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Party_Receipt_No from Party_Amount_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Receipt No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Party_Receipt_Entry_Cheque_Cash, "~L~") = 0 And InStr(Common_Procedures.UR.Party_Receipt_Entry_Cheque_Cash, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Party_Amount_Receipt_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Receipt No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Party_Receipt_No from Party_Amount_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Receipt No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ReceiptNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim vTotRcpt As Single, vtotCashDis As Single, vtotRateDiff As Single, vtotDDComm As Single, vtotOthr As Single, vTotAgComAmt As Single, vTottdsAmt As Single, vTotPartytdsAmt As Single
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
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Party_Receipt_Entry_Cheque_Cash, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Party_Amount_Receipt_Entry, New_Entry, Me, con, "Party_Amount_Receipt_Head", "Party_Receipt_Code", NewCode, "Party_Receipt_Date", "(Party_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Party_Receipt_No desc", dtp_ReceiptDate.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_ReceiptDate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_ReceiptDate.Enabled And msk_ReceiptDate.Visible Then msk_ReceiptDate.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_ReceiptDate.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_ReceiptDate.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_ReceiptDate.Enabled And msk_ReceiptDate.Visible Then msk_ReceiptDate.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Deb_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DebtorName.Text)
        If Deb_ID = 0 Then
            MessageBox.Show("Invalid Debtor Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DebtorName.Enabled And cbo_DebtorName.Visible Then cbo_DebtorName.Focus()
            Exit Sub
        End If
        lbl_UserName.Text = Common_Procedures.User.IdNo
        acgrp_idno = Common_Procedures.get_FieldValue(con, "Ledger_Head", "AccountsGroup_IdNo", "(Ledger_idNo = " & Str(Val(Deb_ID)) & ")")

        If Val(acgrp_idno) = 5 Then
            If Trim(txt_ChequeNo.Text) <> "" Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                DupChqNo = Common_Procedures.get_FieldValue(con, "Party_Amount_Receipt_Head", "Party_Receipt_Code", "(company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_Idno = " & Str(Led_ID) & " and Cheque_No = '" & Trim(txt_ChequeNo.Text) & "' and Party_Receipt_Code <> '" & Trim(NewCode) & "')")

                If Trim(DupChqNo) <> "" Then
                    MessageBox.Show("Duplicate ChequeNo to this Party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If txt_ChequeNo.Enabled And txt_ChequeNo.Visible Then txt_ChequeNo.Focus()
                    Exit Sub
                End If
            End If
        End If

        Total_Calculation()

        vTotRcpt = 0 : vtotCashDis = 0 : vtotRateDiff = 0 : vtotDDComm = 0 : vtotOthr = 0 : vTotAgComAmt = 0 : vTottdsAmt = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotRcpt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value())
            vtotCashDis = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.CASH_DISC_PERC).Value())
            vtotRateDiff = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value())
            vtotDDComm = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value())
            vtotOthr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.DD_COMMISSION).Value())
            vTotAgComAmt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value())
            vTottdsAmt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value())

            vTotPartytdsAmt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value())
        End If

        If (vTotRcpt + vtotCashDis + vtotRateDiff + vtotDDComm + vtotOthr) = 0 Then
            MessageBox.Show("Invalid Receipt Amount..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_Details.Rows.Count > 0 Then
                If dgv_Details.Enabled Then dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT)
            End If
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_ReceiptNo.Text = Common_Procedures.get_MaxCode(con, "Party_Amount_Receipt_Head", "Party_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ReceiptDate", Convert.ToDateTime(msk_ReceiptDate.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Party_Amount_Receipt_Head ( Party_Receipt_Code, Company_IdNo, Party_Receipt_No, for_OrderBy, Party_Receipt_Date, Ledger_IdNo, Debtor_IdNo, Cheque_No, Narration, Total_Receipt, Total_Discount, Total_RateDifference,Total_DDComm,Total_Others,Total_Agent_Comm_Amount,Total_Tds_Amount   , User_idNo ,PreparedBy   ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text))) & ", @ReceiptDate, " & Str(Val(Led_ID)) & ", " & Str(Val(Deb_ID)) & ", '" & Trim(txt_ChequeNo.Text) & "',  '" & Trim(txt_Narration.Text) & "'," & Str(Val(vTotRcpt)) & " , " & Str(Val(vtotCashDis)) & ", " & Str(Val(vtotRateDiff)) & " , " & Val(vtotDDComm) & " , " & Val(vtotOthr) & " , " & Val(vTotAgComAmt) & ", " & Val(vTottdsAmt) & "," & Val(lbl_UserName.Text) & " ,'" & Trim(txt_PreparedBy.Text) & "')"
                cmd.ExecuteNonQuery()

            Else


                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Party_Amount_Receipt_Head", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Party_Receipt_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Party_Amount_Receipt_Details", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Party_Bill_No  , Receipt_Amount  , discount_Percentage , discount_amount  , rate_difference ,  dd_commission,   Others   , Agent_Comm_Percentage,  Agent_Comm_Amount   ,  Agent_Tds_Percentage ,Agent_tds_Amount , Voucher_Bill_Code , Agent_IdNo  , Total_Receipt_Amount ", "Sl_No", "Party_Receipt_Code, For_OrderBy, Company_IdNo, Party_Receipt_No, Party_Receipt_Date, Ledger_Idno,Party_Tds_Percentage,Party_tds_Amount", tr)



                Da = New SqlClient.SqlDataAdapter("select count(*) from Party_Amount_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "' and  Cheque_Return_Code <> ''", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                            Throw New ApplicationException("Already Cheque Returned")
                            Exit Sub
                        End If
                    End If
                End If
                Dt1.Clear()

                cmd.CommandText = "Update Party_Amount_Receipt_Head set Party_Receipt_Date = @ReceiptDate, Ledger_IdNo = " & Val(Led_ID) & ", Debtor_IdNo = " & Val(Deb_ID) & ", Cheque_No = '" & Trim(txt_ChequeNo.Text) & "', Narration = '" & Trim(txt_Narration.Text) & "', Total_Receipt = " & Val(vTotRcpt) & ", Total_Discount = " & Val(vtotCashDis) & ",Total_RateDifference = " & Val(vtotRateDiff) & " , Total_DDComm = " & Val(vtotDDComm) & " ,Total_Others = " & Val(vtotOthr) & " , Total_Agent_Comm_Amount = " & Val(vTotAgComAmt) & " ,Total_Tds_Amount = " & Val(vTottdsAmt) & " , User_idNo = " & Val(lbl_UserName.Text) & ",PreparedBy = '" & Trim(txt_PreparedBy.Text) & "'  where  Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.voucher_bill_code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "delete from voucher_bill_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Party_Amount_Receipt_Head", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Party_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from Party_Amount_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.CASH_DISC_PERC).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.DD_COMMISSION).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.OTHERS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value) <> 0 Then
                        Sno = Sno + 1

                        AgtIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(dgvCol_Detail.agent_name).Value, tr)
                        If Val(AcPosAgIdNo) = 0 Then
                            AcPosAgIdNo = AgtIdNo
                        End If


                        RecAmt = Val(.Rows(i).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) + Val(.Rows(i).Cells(dgvCol_Detail.OTHERS).Value) + Val(.Rows(i).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value) + Val(.Rows(i).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) + Val(.Rows(i).Cells(dgvCol_Detail.DD_COMMISSION).Value) + Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value)

                        cmd.CommandText = "Insert into Party_Amount_Receipt_Details ( Party_Receipt_Code,  Company_IdNo  ,             Party_Receipt_No      ,                               for_OrderBy                                  , Party_receipt_Date,            Sl_No     ,                    Party_Bill_No       ,                                                Receipt_Amount             ,                                     discount_Percentage                  ,                       discount_amount                  ,                  rate_difference    ,                                                                 dd_commission      ,                                                        Others                      ,                      Agent_Comm_Percentage,                                                                          Agent_Comm_Amount            ,              Agent_Tds_Percentage         ,                                                                Agent_tds_Amount                   ,                                Voucher_Bill_Code             ,      Agent_IdNo      , Total_Receipt_Amount ,                    Party_Tds_Percentage,                                                                    Party_tds_Amount                     ,                           Party_Invoice_No                ) " &
                                                    " Values ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text))) & ",    @ReceiptDate   , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Detail.BILLNO).Value) & "', " & Val(.Rows(i).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Detail.CASH_DISC_PERC).Value)) & ", " & Val(.Rows(i).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value) & ", " & Val(.Rows(i).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Detail.DD_COMMISSION).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Detail.OTHERS).Value)) & " ," & Str(Val(.Rows(i).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value)) & ",  '" & Trim(.Rows(i).Cells(dgvCol_Detail.Voucher_Bill_Code).Value) & "', " & Val(AgtIdNo) & " ,  " & Val(RecAmt) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Detail.INVNO).Value) & "' ) "
                        cmd.ExecuteNonQuery()


                        If Trim(.Rows(i).Cells(dgvCol_Detail.Voucher_Bill_Code).Value) <> "" Then

                            Nr = 0
                            cmd.CommandText = "update voucher_bill_head set credit_amount = credit_amount + " & Str(Val(RecAmt)) & " where voucher_bill_code = '" & Trim(.Rows(i).Cells(dgvCol_Detail.Voucher_Bill_Code).Value) & "' and ledger_idno = " & Str(Val(Led_ID))
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Invalid Bill Details - Bill No. " & Trim(.Rows(i).Cells(dgvCol_Detail.BILLNO).Value) & "")
                                Exit Sub
                            End If

                            cmd.CommandText = "insert into voucher_bill_details ( company_idno, Voucher_Bill_Code, Voucher_Bill_Date," _
                                                & "Ledger_Idno, entry_identification, Amount, CrDr_Type ) values ( " & Str(Val(lbl_Company.Tag)) & ", '" _
                                                & Trim(.Rows(i).Cells(dgvCol_Detail.Voucher_Bill_Code).Value) & "', @ReceiptDate, " _
                                                & Str(Led_ID) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', " _
                                                & Str(Val(RecAmt)) & ", 'CR' )"
                            cmd.ExecuteNonQuery()


                        End If


                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Party_Amount_Receipt_Details", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Party_Bill_No  , Receipt_Amount  , discount_Percentage , discount_amount  , rate_difference ,  dd_commission,   Others   , Agent_Comm_Percentage,  Agent_Comm_Amount   ,  Agent_Tds_Percentage ,Agent_tds_Amount , Voucher_Bill_Code , Agent_IdNo  , Total_Receipt_Amount ", "Sl_No", "Party_Receipt_Code, For_OrderBy, Company_IdNo, Party_Receipt_No, Party_Receipt_Date, Ledger_Idno, Party_Tds_Percentage, Party_tds_Amount", tr)


            End With

            '--- Accounts Posting
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

            Narr = ""
            If Trim(txt_ChequeNo.Text) <> "" Then Narr = "CHEQUE NO : " & Trim(txt_ChequeNo.Text)
            If Trim(txt_Narration.Text) <> "" Then Narr = Narr & "   " & Trim(txt_Narration.Text)

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            vLed_IdNos = Led_ID & "|" & Deb_ID & "|" & Val(Common_Procedures.CommonLedger.CASH_DISCOUNT_Ac) & "|" & Val(Common_Procedures.CommonLedger.RATE_DIFFERENCE_Ac) & "|" & Val(Common_Procedures.CommonLedger.DD_COMMISSION_Ac) & "|" & Val(Common_Procedures.CommonLedger.Discount_Ac)
            vVou_Amts = Format(Val(vTotRcpt) + Val(vtotCashDis) + Val(vtotRateDiff) + Val(vtotDDComm) + Val(vtotOthr), "##########0.00") & "|" & -1 * Format(Val(vTotRcpt), "##########0.00") & "|" & -1 * Format(Val(vtotCashDis), "##########0.00") & "|" & -1 * Format(Val(vtotRateDiff), "##########0.00") & "|" & -1 * Format(Val(vtotDDComm), "##########0.00") & "|" & -1 * Format(Val(vtotOthr), "##########0.00")

            If Common_Procedures.Voucher_Updation(con, "Amt.Rcpt", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_ReceiptNo.Text), Convert.ToDateTime(msk_ReceiptDate.Text), Trim(Narr), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
                Exit Sub
            End If

            '--Agent Commission + TDS Posting
            If dgv_Details.Columns(dgvCol_Detail.AGENT_COMM_AMOUNT).Visible = True Then

                If Val(AcPosAgIdNo) <> 0 And Val(vTotAgComAmt) <> 0 Then
                    vLed_IdNos = Val(Common_Procedures.CommonLedger.Agent_Commission_Ac) & "|" & AcPosAgIdNo & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                    vVou_Amts = -1 * Format(Val(vTotAgComAmt), "##########0.00") & "|" & Format(Val(vTotAgComAmt) - Val(vTottdsAmt), "##########0.00") & "|" & Format(Val(vTottdsAmt), "##########0.00")
                    'vVou_Amts = -1 * Val(vTotAgComAmt) & "|" & Val(vTotAgComAmt - vTottdsAmt) & "|" & Val(vTottdsAmt)

                    If Common_Procedures.Voucher_Updation(con, "Rcpt.AgComm", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_ReceiptNo.Text), Convert.ToDateTime(msk_ReceiptDate.Text), "Rec.No : " & Trim(lbl_ReceiptNo.Text) & ", " & Trim(cbo_Ledger.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                        Throw New ApplicationException(ErrMsg)
                    End If

                End If

            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition3) & Trim(NewCode), tr)

            vLed_IdNos = ""
            vVou_Amts = ""
            ErrMsg = ""

            vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Payable_Ac) & "|" & Led_ID
            vVou_Amts = Val(CSng(vTotPartytdsAmt)) & "|" & -1 * Val(CSng(vTotPartytdsAmt))

            If Common_Procedures.Voucher_Updation(con, "PartyRec.Tds", Val(lbl_Company.Tag), Trim(Pk_Condition3) & Trim(NewCode), Trim(lbl_ReceiptNo.Text), Convert.ToDateTime(msk_ReceiptDate.Text), "Party Name : " & Trim(cbo_Ledger.Text) & " , PartyAmtRec.No : " & Trim(lbl_ReceiptNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_ReceiptNo.Text)
                End If

            Else
                move_record(lbl_ReceiptNo.Text)

            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()
            If msk_ReceiptDate.Enabled And msk_ReceiptDate.Visible Then msk_ReceiptDate.Focus()

        End Try

    End Sub

    Private Sub Balance_Amount_Calculation(ByVal Rw As Integer)
        Dim vBalAmt As Single

        With dgv_Details


            If Val(.Rows(Rw).Cells(dgvCol_Detail.CASH_DISC_PERC).Value) <> 0 Then
                .Rows(Rw).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value = Format(Val(.Rows(Rw).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value) * Val(.Rows(Rw).Cells(dgvCol_Detail.CASH_DISC_PERC).Value) / 100, "########0.00")
            End If

            .Rows(Rw).Cells(dgvCol_Detail.OTHERS).Value = Format(Val(.Rows(Rw).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) * Val(.Rows(Rw).Cells(dgvCol_Detail.DD_COMMISSION).Value) / 100, "########0.00")

            .Rows(Rw).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value = Format(Val(.Rows(Rw).Cells(dgvCol_Detail.BILLAMOUNT).Value) * Val(.Rows(Rw).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value) / 100, "########0.00")

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then                   '---- Jeno Textiles (Somanur) ,sathish tex
                vBalAmt = Val(.Rows(Rw).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value) - (Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.OTHERS).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.DD_COMMISSION).Value))
            Else
                vBalAmt = Val(.Rows(Rw).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value) - (Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.OTHERS).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.DD_COMMISSION).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value))
                'vBalAmt = Val(.Rows(Rw).Cells(dgvCol_Detail.BILLAMOUNT).Value) - (Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.OTHERS).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.DD_COMMISSION).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value))
            End If

            .Rows(Rw).Cells(dgvCol_Detail.NETBALANCEAMOUNT).Value = Format(Val(vBalAmt), "#########0.00")

            .Rows(Rw).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value = Format((Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) * Val(Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value)) / 100), "#########0.00")

            .Rows(Rw).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value = Format((Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value) * Val(Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value)) / 100), "#########0.00")






            Total_Calculation()

        End With
    End Sub

    Private Sub Total_Calculation()
        Dim vBlAmt As Single, vTotBal As Single, vTotRect As Single, vtotCash As Single, vtotRate As Single, vtotComm As Single, vtotOtrs As Single, vTotCommAmt As Single, vTottdsAmt As Single, vPtTotdsAmt As Single, vTotBalance As Single
        Dim i As Integer
        Dim Sno As Integer

        vBlAmt = 0 : vTotBal = 0 : vTotRect = 0 : vtotCash = 0 : vtotRate = 0 : vtotComm = 0 : vtotOtrs = 0 : vTotCommAmt = 0 : vTottdsAmt = 0 : vPtTotdsAmt = 0 : vTotBalance = 0 : Sno = 0

        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                Sno = Sno + 1

                .Rows(i).Cells(dgvCol_Detail.SLNO).Value = Sno

                If Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.BILLAMOUNT).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) <> 0 Then

                    vBlAmt = vBlAmt + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.BILLAMOUNT).Value)
                    vTotBal = vTotBal + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value)
                    vTotRect = vTotRect + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value)
                    vtotCash = vtotCash + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value)
                    vtotRate = vtotRate + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value)
                    vtotComm = vtotComm + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.DD_COMMISSION).Value)
                    vtotOtrs = vtotOtrs + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.OTHERS).Value)
                    vTotCommAmt = vTotCommAmt + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value)
                    vTottdsAmt = vTottdsAmt + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value)
                    vPtTotdsAmt = vPtTotdsAmt + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value)
                    vTotBalance = vTotBalance + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.NETBALANCEAMOUNT).Value)
                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.BILLAMOUNT).Value = Format(Val(vBlAmt), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value = Format(Val(vTotBal), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value = Format(Val(vTotRect), "#########0.00")
        'dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.CASH_DISC_PERC).Value = Format(Val(vtotCash), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value = Format(Val(vtotCash), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value = Format(Val(vtotRate), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.DD_COMMISSION).Value = Format(Val(vtotComm), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value = Format(Val(vTotCommAmt), "#########0.00")
        '
        'dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value = Format(Val(vTottdsAmt), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value = Format(Val(vTottdsAmt), "#########0.00")
        'dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value = Format(Val(vPtTotdsAmt), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value = Format(Val(vPtTotdsAmt), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.NETBALANCEAMOUNT).Value = Format(Val(vTotBalance), "#########0.00")

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_ReceiptDate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT)

            Else
                cbo_DebtorName.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Bill Details", "FOR BILL SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT)

                Else
                    cbo_DebtorName.Focus()

                End If

            End If

        End If
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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
        'Balance_Amount_Calculation(e.RowIndex)
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .CurrentCell.ColumnIndex >= dgvCol_Detail.BILLAMOUNT And .CurrentCell.ColumnIndex <= dgvCol_Detail.NETBALANCEAMOUNT Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                        'Balance_Amount_Calculation(e.RowIndex)
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If
            End With
        Catch ex As Exception
            '-----

        End Try

        'Balance_Amount_Calculation(e.RowIndex)
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = dgvCol_Detail.BILLBALANCEAMOUNT Or .CurrentCell.ColumnIndex = dgvCol_Detail.RECEIPTAMOUNT Or .CurrentCell.ColumnIndex = dgvCol_Detail.CASH_DISC_PERC Or .CurrentCell.ColumnIndex = dgvCol_Detail.CASH_DISC_AMOUNT Or .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_TDS_PERC Or .CurrentCell.ColumnIndex = dgvCol_Detail.CASH_DISC_AMOUNT Or .CurrentCell.ColumnIndex = dgvCol_Detail.RATE_DIFFERENCE Or .CurrentCell.ColumnIndex = dgvCol_Detail.DD_COMMISSION Or .CurrentCell.ColumnIndex = dgvCol_Detail.OTHERS Or .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_COMM_AMOUNT Or .CurrentCell.ColumnIndex = dgvCol_Detail.PARTY_TDS_PERC Or .CurrentCell.ColumnIndex = dgvCol_Detail.PARTY_TDS_AMOUNT Then
                        Balance_Amount_Calculation(e.RowIndex)
                    End If
                End If
            End With
        Catch ex As Exception
            '------
        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgv_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        Try
            With dgv_Details
                If Val(.CurrentCell.ColumnIndex) >= dgvCol_Detail.BILLAMOUNT And Val(.CurrentCell.ColumnIndex) <= dgvCol_Detail.NETBALANCEAMOUNT Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End With
        Catch ex As Exception
            '-----
        End Try
        Total_Calculation()

    End Sub

    Private Sub dgtxt_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyUp
        dgv_Details_KeyUp(sender, e)
        Total_Calculation()

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try


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

                    Total_Calculation()


                End With

            End If

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
            .Rows(n - 1).Cells(dgvCol_Detail.SLNO).Value = Val(n)
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
        Dim Led_IdNo As Integer
        Dim Condt As String = ""
        Dim Dbt_IdNo As Integer

        Try

            Condt = ""
            Led_IdNo = 0
            Dbt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Party_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Party_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Party_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Trim(cbo_Filter_DebitorName.Text) <> "" Then
                Dbt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_DebitorName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If
            If Val(Dbt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Debtor_IdNo = " & Str(Val(Dbt_IdNo))
            End If

            If Val(txt_FilterChequeNo.Text) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Cheque_No = '" & Trim(txt_FilterChequeNo.Text) & "'"
            End If
            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,C.Ledger_Name as Debtor_Name from Party_Amount_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Debtor_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Party_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Party_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Party_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Party_Receipt_Date").ToString), "dd-MM-yyyy")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_DebitorName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_DebitorName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_DebitorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DebitorName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DebitorName, cbo_Filter_PartyName, txt_FilterChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_DebitorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DebitorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DebitorName, txt_FilterChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_idno = 0)")
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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        If e.KeyCode = 40 Then
            txt_PreparedBy.Focus()
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_PreparedBy.Focus()
        End If
    End Sub

    Private Sub txt_PreparedBy_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PreparedBy.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_ReceiptDate.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_PreparedBy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PreparedBy.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_ReceiptDate.Focus()
            End If
        End If
    End Sub


    Private Sub cbo_DebtorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DebtorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 23)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DebtorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DebtorName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DebtorName, Nothing, txt_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6  or AccountsGroup_IdNo = 23)", "(Ledger_idno = 0)")
        If (e.KeyValue = 38 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT)

            Else
                cbo_Ledger.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_Debtorname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DebtorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DebtorName, txt_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6  or AccountsGroup_IdNo = 23) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DebtorName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DebtorName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DebtorName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim dys As Integer = 0
        Dim vBILLNO As String = ""
        Dim vINVNO As String = ""

        Try

            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            If LedIdNo = 0 Then
                MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                Exit Sub
            End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
            If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 1 And Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) <> 1 Then
                CompIDCondt = ""
            End If


            cmd.Connection = con
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@uptodate", Convert.ToDateTime(msk_ReceiptDate.Text))

            With dgv_Selection

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("Select FH.Discount_Percentage as discper ,FH.Discount_Amount as Dis_amt, a.*, b.*, (b.debit_amount - b.credit_amount + a.receipt_amount + a.discount_amount + a.rate_difference + a.dd_commission + a.others) as ent_bill_balance, c.ledger_name as agent_name from Party_Amount_Receipt_Details a INNER JOIN voucher_bill_head b ON a.Voucher_Bill_Code = b.Voucher_Bill_Code LEFT OUTER JOIN Ledger_Head c ON a.agent_idno = c.ledger_idno LEFT OUTER JOIN FinishedProduct_Invoice_Head FH ON  FH.FinishedProduct_Invoice_Code = b.Entry_Identification Where " & Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " b.ledger_idno = " & Str(Val(LedIdNo)) & " and a.Party_Receipt_code = '" & Trim(NewCode) & "' order by a.sl_no", con)
                'Da = New SqlClient.SqlDataAdapter("Select FH.Discount_Percentage as discper ,FH.Discount_Amount as Dis_amt, a.*, b.*, (b.debit_amount - b.credit_amount + a.receipt_amount + a.discount_amount + a.rate_difference + a.dd_commission + a.others) as ent_bill_balance, c.ledger_name as agent_name from Party_Amount_Receipt_Details a INNER JOIN voucher_bill_head b ON a.Voucher_Bill_Code = b.Voucher_Bill_Code LEFT OUTER JOIN Ledger_Head c ON a.agent_idno = c.ledger_idno  LEFT OUTER JOIN FinishedProduct_Invoice_Head FH ON  FH.FinishedProduct_Invoice_Code = b.Entry_Identification OR FH.FinishedProduct_Invoice_Code = b.Entry_Identification where " & Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.agent_idno = " & Str(Val(LedIdNo)) & " and a.Party_Receipt_code = '" & Trim(NewCode) & "' order by a.sl_no", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SLNO).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_Selection.BILLNO).Value = Dt1.Rows(i).Item("Party_Bill_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.BILLDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Voucher_Bill_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(dgvCol_Selection.BILLAMOUNT).Value = Format(Val(Dt1.Rows(i).Item("debit_amount").ToString), "#########0.00")
                        .Rows(n).Cells(dgvCol_Selection.BILLBALANCEAMOUNT).Value = Format(Val(Dt1.Rows(i).Item("ent_bill_balance").ToString), "#########0.00")
                        .Rows(n).Cells(dgvCol_Selection.AGENTNAME).Value = Dt1.Rows(i).Item("Agent_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"
                        .Rows(n).Cells(dgvCol_Selection.Voucher_Bill_Code).Value = Dt1.Rows(i).Item("Voucher_Bill_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.GROSSAMOUNT).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.RECEIPTAMOUNT).Value = Val(Dt1.Rows(i).Item("Receipt_Amount").ToString)
                        .Rows(n).Cells(dgvCol_Selection.CASH_DISC_AMOUNT).Value = Val(Dt1.Rows(i).Item("discount_amount").ToString)
                        .Rows(n).Cells(dgvCol_Selection.RATE_DIFFERENCE).Value = Val(Dt1.Rows(i).Item("rate_difference").ToString)
                        .Rows(n).Cells(dgvCol_Selection.DD_COMMISSION).Value = Val(Dt1.Rows(i).Item("dd_commission").ToString)
                        .Rows(n).Cells(dgvCol_Selection.OTHERS).Value = Val(Dt1.Rows(i).Item("Others").ToString)
                        .Rows(n).Cells(dgvCol_Selection.AGENT_COMM_PERC).Value = Val(Dt1.Rows(i).Item("Agent_Comm_Percentage").ToString)
                        .Rows(n).Cells(dgvCol_Selection.AGENT_COMM_AMOUNT).Value = Val(Dt1.Rows(i).Item("Agent_Comm_Amount").ToString)
                        .Rows(n).Cells(dgvCol_Selection.AGENT_TDS_PERC).Value = Val(Dt1.Rows(i).Item("Agent_Tds_Percentage").ToString)
                        .Rows(n).Cells(dgvCol_Selection.AGENT_TDS_AMOUNT).Value = Val(Dt1.Rows(i).Item("Agent_tds_Amount").ToString)
                        .Rows(n).Cells(dgvCol_Selection.AGENTIDNO).Value = Val(Dt1.Rows(i).Item("Agent_IdNo").ToString)



                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then                   '---- Jeno Textiles (Somanur) ,sathish tex
                            .Rows(n).Cells(dgvCol_Selection.CASH_DISC_PERC).Value = Val(Dt1.Rows(i).Item("discper").ToString)
                            .Rows(n).Cells(dgvCol_Selection.CASH_DISC_AMOUNT).Value = Dt1.Rows(i).Item("Dis_amt").ToString
                        Else
                            .Rows(n).Cells(dgvCol_Selection.CASH_DISC_PERC).Value = Val(Dt1.Rows(i).Item("discount_Percentage").ToString)
                        End If


                        dys = DateDiff(DateInterval.Day, Dt1.Rows(i).Item("Voucher_Bill_Date"), Convert.ToDateTime(msk_ReceiptDate.Text))

                        .Rows(n).Cells(dgvCol_Selection.DAYS).Value = dys

                        vBILLNO = Dt1.Rows(i).Item("Party_Bill_No").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1360" Then '---- Ashoka Textile (63.Velampalayam - Palladam)
                            If Trim(UCase(Dt1.Rows(i).Item("Entry_Identification").ToString)) Like "CSINV-*" Or Trim(UCase(Dt1.Rows(i).Item("Entry_Identification").ToString)) Like "GCINV-*" Then
                                Da = New SqlClient.SqlDataAdapter("Select z1.ClothSales_Invoice_No, z1.ClothSales_Invoice_RefNo from ClothSales_Invoice_Head z1 where 'CSINV-' + z1.ClothSales_Invoice_Code = '" & Trim(Dt1.Rows(i).Item("Entry_Identification").ToString) & "' OR z1.ClothSales_Invoice_Code = '" & Trim(Dt1.Rows(i).Item("Entry_Identification").ToString) & "' ", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then

                                    vBILLNO = Trim(Dt2.Rows(0).Item("ClothSales_Invoice_No").ToString)

                                End If
                                Dt2.Clear()
                            End If
                        End If

                        .Rows(n).Cells(dgvCol_Selection.INVNO).Value = vBILLNO


                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("Select FH.Discount_Percentage as discper, FH.Discount_Amount as Dis_amt, a.*, b.ledger_name as agent_name ,b.Tds_Percentage,b.Cloth_Comm_Percentage, datediff(day, a.voucher_bill_date, getdate()) as noof_days From voucher_bill_head a LEFT OUTER JOIN ledger_head b ON a.agent_idno = b.ledger_idno LEFT OUTER JOIN FinishedProduct_Invoice_Head FH ON  FH.FinishedProduct_Invoice_Code = a.Entry_Identification OR FH.FinishedProduct_Invoice_Code = a.Entry_Identification Where " & Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_idno = " & Str(Val(LedIdNo)) & " and a.debit_amount > a.credit_amount and a.Voucher_Bill_Code NOT IN ( Select z1.Voucher_Bill_Code from Party_Amount_Receipt_Details z1, Party_Amount_Receipt_Head z2 where z1.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z2.ledger_idno = " & Str(Val(LedIdNo)) & " and z1.Party_Receipt_code = '" & Trim(NewCode) & "' and z1.Party_Receipt_code = z2.Party_Receipt_code) order by a.voucher_bill_date, a.For_OrderBy, a.Voucher_Bill_No ", con)
                'Da = New SqlClient.SqlDataAdapter("Select FH.Discount_Percentage as discper, FH.Discount_Amount as Dis_amt, a.*, b.ledger_name as agent_name ,b.Tds_Percentage,b.Cloth_Comm_Percentage, datediff(day, a.voucher_bill_date, getdate()) as noof_days From voucher_bill_head a LEFT OUTER JOIN ledger_head b ON a.agent_idno = b.ledger_idno LEFT OUTER JOIN FinishedProduct_Invoice_Head FH ON  FH.FinishedProduct_Invoice_Code = a.Entry_Identification OR FH.FinishedProduct_Invoice_Code = a.Entry_Identification Where " & Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_idno = " & Str(Val(LedIdNo)) & " and a.debit_amount > a.credit_amount and a.Voucher_Bill_Code NOT IN ( Select z.Voucher_Bill_Code from Party_Amount_Receipt_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.agent_idno = " & Str(Val(LedIdNo)) & " and z.Party_Receipt_code = '" & Trim(NewCode) & "' ) order by a.voucher_bill_date, a.For_OrderBy, a.Voucher_Bill_No ", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(dgvCol_Selection.SLNO).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_Selection.BILLNO).Value = Dt1.Rows(i).Item("Party_Bill_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.BILLDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Voucher_Bill_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(dgvCol_Selection.BILLAMOUNT).Value = Format(Val(Dt1.Rows(i).Item("debit_amount").ToString), "#########0.00")
                        .Rows(n).Cells(dgvCol_Selection.BILLBALANCEAMOUNT).Value = Format(Val(Dt1.Rows(i).Item("debit_amount").ToString) - Val(Dt1.Rows(i).Item("Credit_Amount").ToString), "#########0.00")
                        .Rows(n).Cells(dgvCol_Selection.AGENTNAME).Value = Dt1.Rows(i).Item("Agent_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Voucher_Bill_Code).Value = Dt1.Rows(i).Item("Voucher_Bill_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.GROSSAMOUNT).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.RECEIPTAMOUNT).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.CASH_DISC_AMOUNT).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.RATE_DIFFERENCE).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.DD_COMMISSION).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.OTHERS).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.AGENT_COMM_PERC).Value = Format(Val(Dt1.Rows(i).Item("Cloth_Comm_Percentage").ToString), "#########0.00")
                        .Rows(n).Cells(dgvCol_Selection.AGENT_COMM_AMOUNT).Value = "" 'Format((Val(Dt1.Rows(i).Item("debit_amount").ToString) * Val(Dt1.Rows(i).Item("Cloth_Comm_Percentage").ToString)) / 100, "#########0.00")
                        .Rows(n).Cells(dgvCol_Selection.AGENT_TDS_PERC).Value = Format(Val(Dt1.Rows(i).Item("Tds_Percentage").ToString), "#########0.00")
                        .Rows(n).Cells(dgvCol_Selection.AGENT_TDS_AMOUNT).Value = "" 'Format((Val(Dt1.Rows(i).Item("debit_amount").ToString) * Val(Dt1.Rows(i).Item("Tds_Percentage").ToString)) / 100, "#########0.00")

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then                   '---- Jeno Textiles (Somanur) ,sathish tex
                            .Rows(n).Cells(dgvCol_Selection.CASH_DISC_PERC).Value = Dt1.Rows(i).Item("discper").ToString
                            .Rows(n).Cells(dgvCol_Selection.CASH_DISC_AMOUNT).Value = Dt1.Rows(i).Item("Dis_amt").ToString
                        Else
                            .Rows(n).Cells(dgvCol_Selection.CASH_DISC_PERC).Value = ""
                        End If

                        dys = DateDiff(DateInterval.Day, Dt1.Rows(i).Item("Voucher_Bill_Date"), Convert.ToDateTime(msk_ReceiptDate.Text))

                        .Rows(n).Cells(dgvCol_Selection.DAYS).Value = dys
                        '.Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("noof_days").ToString)

                        vBILLNO = Dt1.Rows(i).Item("Party_Bill_No").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1360" Then '---- Ashoka Textile (63.Velampalayam - Palladam)
                            If Trim(UCase(Dt1.Rows(i).Item("Entry_Identification").ToString)) Like "CSINV-*" Or Trim(UCase(Dt1.Rows(i).Item("Entry_Identification").ToString)) Like "GCINV-*" Then
                                Da = New SqlClient.SqlDataAdapter("Select z1.ClothSales_Invoice_No, z1.ClothSales_Invoice_RefNo from ClothSales_Invoice_Head z1 where 'CSINV-' + z1.ClothSales_Invoice_Code = '" & Trim(Dt1.Rows(i).Item("Entry_Identification").ToString) & "' OR z1.ClothSales_Invoice_Code = '" & Trim(Dt1.Rows(i).Item("Entry_Identification").ToString) & "' ", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    vBILLNO = Trim(Dt2.Rows(0).Item("ClothSales_Invoice_No").ToString)
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(dgvCol_Selection.INVNO).Value = vBILLNO

                    Next

                End If
                Dt1.Clear()

            End With

            pnl_Selection.Visible = True
            pnl_Back.Enabled = False
            dgv_Selection.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID BILL SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_BILL(e.RowIndex)
    End Sub

    Private Sub Select_BILL(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(dgvCol_Selection.STS).Value = (Val(.Rows(RwIndx).Cells(dgvCol_Selection.STS).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(dgvCol_Selection.STS).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(dgvCol_Selection.STS).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_BILL(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Bill_Selection()
    End Sub

    Private Sub Bill_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim BalAmt As Single = 0

        pnl_Back.Enabled = True
        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.STS).Value) = 1 Then

                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(dgvCol_Detail.SLNO).Value = Val(sno)
                dgv_Details.Rows(n).Cells(dgvCol_Detail.BILLNO).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.BILLNO).Value
                dgv_Details.Rows(n).Cells(dgvCol_Detail.BILLAMOUNT).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.BILLAMOUNT).Value), "#########0.00")
                dgv_Details.Rows(n).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.BILLBALANCEAMOUNT).Value), "#########0.00")

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.RECEIPTAMOUNT).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.RECEIPTAMOUNT).Value), "#########0.00")
                End If
                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.CASH_DISC_PERC).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.CASH_DISC_PERC).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.CASH_DISC_PERC).Value), "#########0.00")
                End If

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.CASH_DISC_AMOUNT).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.CASH_DISC_AMOUNT).Value), "#########0.00")
                End If

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.RATE_DIFFERENCE).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.RATE_DIFFERENCE).Value), "#########0.00")
                End If

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.DD_COMMISSION).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.DD_COMMISSION).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.DD_COMMISSION).Value), "#########0.00")
                End If

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.DD_COMMISSION).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.OTHERS).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.OTHERS).Value), "#########0.00")
                End If

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENT_COMM_PERC).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENT_COMM_PERC).Value), "#########0.00")
                End If

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENT_COMM_AMOUNT).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENT_COMM_AMOUNT).Value), "#########0.00")
                End If

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENT_TDS_PERC).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENT_TDS_PERC).Value), "#########0.00")
                End If

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENT_TDS_AMOUNT).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENT_TDS_AMOUNT).Value), "#########0.00")
                End If





                BalAmt = Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.BILLBALANCEAMOUNT).Value) - (Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENT_COMM_PERC).Value) + Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.CASH_DISC_AMOUNT).Value) + Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.RATE_DIFFERENCE).Value) + Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.DD_COMMISSION).Value) + Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.OTHERS).Value))

                dgv_Details.Rows(n).Cells(dgvCol_Detail.NETBALANCEAMOUNT).Value = Format(Val(BalAmt), "#########0.00")

                dgv_Details.Rows(n).Cells(dgvCol_Detail.Voucher_Bill_Code).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Voucher_Bill_Code).Value
                dgv_Details.Rows(n).Cells(dgvCol_Detail.Voucher_Bill_date).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.BILLDATE).Value
                dgv_Details.Rows(n).Cells(dgvCol_Detail.agent_name).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.AGENTNAME).Value
                dgv_Details.Rows(n).Cells(dgvCol_Detail.gross_amount).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.GROSSAMOUNT).Value

                dgv_Details.Rows(n).Cells(dgvCol_Detail.DAYS).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.DAYS).Value
                dgv_Details.Rows(n).Cells(dgvCol_Detail.INVNO).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.INVNO).Value

            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT)

        Else
            cbo_DebtorName.Focus()

        End If

    End Sub

    Private Sub msk_ReceiptDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_ReceiptDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_ReceiptDate.Text
            vmskSelStrt = msk_ReceiptDate.SelectionStart
        End If

    End Sub

    Private Sub msk_ReceiptDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_ReceiptDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_ReceiptDate.Text = Date.Today
            msk_ReceiptDate.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_ReceiptDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_ReceiptDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_ReceiptDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_ReceiptDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_ReceiptDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_ReceiptDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub dtp_ReceiptDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ReceiptDate.TextChanged

        If IsDate(dtp_ReceiptDate.Text) = True Then

            msk_ReceiptDate.Text = dtp_ReceiptDate.Text
            msk_ReceiptDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_ReceiptDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_ReceiptDate.LostFocus

        If IsDate(msk_ReceiptDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_ReceiptDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_ReceiptDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_ReceiptDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_ReceiptDate.Text)) >= 2000 Then
                    dtp_ReceiptDate.Value = Convert.ToDateTime(msk_ReceiptDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_ReceiptDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_ReceiptDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_ReceiptDate.Text = Date.Today
        End If
    End Sub

    Private Sub dgtxt_details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_details.Text)
                End If
            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub msk_ReceiptDate_MaskInputRejected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MaskInputRejectedEventArgs) Handles msk_ReceiptDate.MaskInputRejected

    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_DebtorName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_DebtorName.SelectedIndexChanged

    End Sub
End Class