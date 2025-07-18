Public Class TDS_Amount_Receipt_Entry
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
        'SLNO                '0
        'BILLNO              '1
        'BILLAMOUNT          '2
        'BILLBALANCEAMOUNT   '3
        'RECEIPTAMOUNT       '4
        'CASH_DISC_PERC      '5
        'CASH_DISC_AMOUNT    '6
        'RATE_DIFFERENCE     '7
        'DD_COMMISSION       '8
        'OTHERS              '9
        'AGENT_COMM_PERC     '10
        'AGENT_COMM_AMOUNT   '11
        'AGENT_TDS_PERC      '12
        'AGENT_TDS_AMOUNT    '13
        'Voucher_Bill_Code   '14
        'Voucher_Bill_date   '15
        'agent_name          '16
        'gross_amount        '17
        'PARTY_TDS_PERC      '18   
        'PARTY_TDS_AMOUNT    '19
        'NETBALANCEAMOUNT    '20
        'DAYS                '21


        SLNO                        '0
        BILLNO                      '1
        BILL_DATE                   '2
        BILLAMOUNT                  '3
        PARTY_TDS_PERC              '4
        PARTY_TDS_AMOUNT            '5
        CLOTHSALES_INVOICE_CODE     '6





    End Enum

    Public Enum dgvCol_Selection As Integer
        'SLNO                '0
        'BILLNO              '1
        'BILLDATE            '2
        'BILLAMOUNT          '3
        'BILLBALANCEAMOUNT   '4
        'AGENTNAME           '5
        'STS                 '6
        'Voucher_Bill_Code   '7
        'GROSSAMOUNT         '8
        'RECEIPTAMOUNT       '9
        'CASH_DISC_AMOUNT    '10
        'RATE_DIFFERENCE     '11
        'DD_COMMISSION       '12  
        'OTHERS              '13
        'AGENT_COMM_PERC     '14
        'AGENT_COMM_AMOUNT   '15
        'AGENT_TDS_PERC      '16
        'AGENT_TDS_AMOUNT    '17
        'AGENTIDNO           '18
        'DAYS                '19
        'CASH_DISC_PERC      '20


        SLNO                        '0
        BILLNO                      '1
        BILL_DATE                   '2
        BILLAMOUNT                  '3
        PARTY_TDS_PERC              '4
        PARTY_TDS_AMOUNT            '5
        STS                         '6
        CLOTHSALES_INVOICE_CODE     '7

        'ENTRY_TYPE          '6
        'ENTRY_PK_CODE       '7


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

        txt_Remarks.Text = ""
        cbo_Filter_PartyName.Text = ""


        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()
        dgv_Details_Total.Rows.Clear()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White



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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Debtor_Name from TDS_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  Where a.Party_Receipt_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_ReceiptNo.Text = dt1.Rows(0).Item("Party_Receipt_No").ToString
                dtp_ReceiptDate.Text = dt1.Rows(0).Item("Party_Receipt_Date").ToString
                msk_ReceiptDate.Text = dtp_ReceiptDate.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString


                da2 = New SqlClient.SqlDataAdapter("Select a.*, C.Ledger_name from TDS_Receipt_Details a INNER JOIN TDS_Receipt_Head b ON a.Party_Receipt_Code = b.Party_Receipt_Code LEFT OUTER JOIN Ledger_Head c ON B.Ledger_IdNo = c.Ledger_IdNo  where a.company_idno  = " & Str(Val(lbl_Company.Tag)) & " and a.Party_Receipt_code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)

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
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.BILL_DATE).Value = dt2.Rows(i).Item("Party_Receipt_Date").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.BILLAMOUNT).Value = Format(Val(dt2.Rows(i).Item("Bill_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value = Val(dt2.Rows(i).Item("Party_Tds_Percentage").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value = Format(Val(dt2.Rows(i).Item("Party_tds_Amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(dgvCol_Detail.CLOTHSALES_INVOICE_CODE).Value = dt2.Rows(i).Item("Invoice_Code").ToString


                    Next i

                End If
                dt2.Clear()

                Total_Calculation()

                If LockSTS = True Then

                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray



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

    Private Sub TDS_Receipt_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DebtorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_DebtorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

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

    Private Sub TDS_Receipt_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim vTotCommAmt As Single

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



        'dgv_Details.Columns(dgvCol_Detail.AGENT_COMM_PERC).Visible = False
        'dgv_Details.Columns(dgvCol_Detail.AGENT_COMM_AMOUNT).Visible = False
        'dgv_Details.Columns(dgvCol_Detail.AGENT_TDS_PERC).Visible = False
        'dgv_Details.Columns(dgvCol_Detail.AGENT_TDS_AMOUNT).Visible = False

        'dgv_Details_Total.Columns(dgvCol_Detail.AGENT_COMM_PERC).Visible = False
        'dgv_Details_Total.Columns(dgvCol_Detail.AGENT_COMM_AMOUNT).Visible = False
        'dgv_Details_Total.Columns(dgvCol_Detail.AGENT_TDS_PERC).Visible = False
        'dgv_Details_Total.Columns(dgvCol_Detail.AGENT_TDS_AMOUNT).Visible = False


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then

        '    dgv_Details.Columns(dgvCol_Detail.AGENT_COMM_PERC).Visible = True
        '    dgv_Details.Columns(dgvCol_Detail.AGENT_COMM_AMOUNT).Visible = True
        '    dgv_Details.Columns(dgvCol_Detail.AGENT_TDS_PERC).Visible = True
        '    dgv_Details.Columns(dgvCol_Detail.AGENT_TDS_AMOUNT).Visible = True
        '    dgv_Details.Columns(dgvCol_Detail.PARTY_TDS_PERC).Visible = False
        '    dgv_Details.Columns(dgvCol_Detail.PARTY_TDS_AMOUNT).Visible = False

        '    dgv_Details_Total.Columns(dgvCol_Detail.AGENT_COMM_PERC).Visible = True
        '    dgv_Details_Total.Columns(dgvCol_Detail.AGENT_COMM_AMOUNT).Visible = True
        '    dgv_Details_Total.Columns(dgvCol_Detail.AGENT_TDS_PERC).Visible = True
        '    dgv_Details_Total.Columns(dgvCol_Detail.AGENT_TDS_AMOUNT).Visible = True
        '    dgv_Details_Total.Columns(dgvCol_Detail.PARTY_TDS_PERC).Visible = False
        '    dgv_Details_Total.Columns(dgvCol_Detail.PARTY_TDS_AMOUNT).Visible = False

        'End If


        AddHandler msk_ReceiptDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus




        AddHandler msk_ReceiptDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus



        AddHandler msk_ReceiptDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_ReceiptDate.KeyPress, AddressOf TextBoxControlKeyPress

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

    Private Sub TDS_Receipt_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub TDS_Receipt_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Remarks.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Detail.BILLNO)
                            End If

                            'ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.CASH_DISC_AMOUNT Then
                            '    If .Columns(.CurrentCell.ColumnIndex + 1).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 2).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 3).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 3)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 4).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 4)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 5).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 5)
                            '    End If

                            'ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_COMM_PERC Then
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                            'ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.OTHERS Then
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Detail.PARTY_TDS_PERC)
                            'ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_TDS_AMOUNT Then
                            '    If .Columns(.CurrentCell.ColumnIndex + 1).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 2).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 3).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 3)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 4).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 4)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 5).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 5)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 6).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 6)
                            '    ElseIf .Columns(.CurrentCell.ColumnIndex + 7).Visible = True Then
                            '        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 7)
                            '    End If


                            'Else

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= dgvCol_Detail.BILLNO Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Ledger.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 5)


                            End If
                            'ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.AGENT_TDS_PERC Then
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 2)
                            'ElseIf .CurrentCell.ColumnIndex = dgvCol_Detail.PARTY_TDS_PERC Then
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Detail.OTHERS)

                            'Else

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Party_Amount_Receipt_Entry, New_Entry, Me, con, "TDS_Receipt_Head", "Party_Receipt_Code", NewCode, "Party_Receipt_Date", "(Party_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Da = New SqlClient.SqlDataAdapter("select count(*) from TDS_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "' and  Cheque_Return_Code <> ''", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already Cheque Returned", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Party_Amount_Receipt_Head", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Party_Receipt_Code, Company_IdNo, for_OrderBy", trans)

            'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Party_Amount_Receipt_Details", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Party_Bill_No  , Receipt_Amount  , discount_Percentage , discount_amount  , rate_difference ,  dd_commission,   Others   , Agent_Comm_Percentage,  Agent_Comm_Amount   ,  Agent_Tds_Percentage ,Agent_tds_Amount , Voucher_Bill_Code , Agent_IdNo  , Total_Receipt_Amount ", "Sl_No", "Party_Receipt_Code, For_OrderBy, Company_IdNo, Party_Receipt_No, Party_Receipt_Date, Ledger_Idno, Party_Tds_Percentage,Party_tds_Amount", trans)


            cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.voucher_bill_code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "delete from TDS_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from TDS_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update ClothSales_Invoice_Head set TDS_Receipt_code='' where TDS_Receipt_code='" & Trim(NewCode) & "'  "
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

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_Filter_PartyName, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6)", "(Ledger_IdNo = 0)")

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

            da = New SqlClient.SqlDataAdapter("select top 1 Party_Receipt_No from TDS_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Party_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Party_Receipt_No from TDS_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Party_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Party_Receipt_No from TDS_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Party_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Party_Receipt_No from TDS_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Party_Receipt_No desc", con)
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

            lbl_ReceiptNo.Text = Common_Procedures.get_MaxCode(con, "TDS_Receipt_Head", "Party_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_ReceiptNo.ForeColor = Color.Red

            msk_ReceiptDate.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from TDS_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Party_Receipt_No desc", con)
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

            Da = New SqlClient.SqlDataAdapter("select Party_Receipt_No from TDS_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Party_Amount_Receipt_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Receipt No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Party_Receipt_No from TDS_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        Dim TotBillAmt As Single = 0
        Dim TotTdsAmt As Single = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Party_Receipt_Entry_Cheque_Cash, New_Entry) = False Then Exit Sub
        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Party_Amount_Receipt_Entry, New_Entry, Me, con, "TDS_Receipt_Head", "Party_Receipt_Code", NewCode, "Party_Receipt_Date", "(Party_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Party_Receipt_No desc", dtp_ReceiptDate.Value.Date) = False Then Exit Sub

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

        'Deb_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DebtorName.Text)
        'If Deb_ID = 0 Then
        '    MessageBox.Show("Invalid Debtor Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_DebtorName.Enabled And cbo_DebtorName.Visible Then cbo_DebtorName.Focus()
        '    Exit Sub
        'End If
        lbl_UserName.Text = Common_Procedures.User.IdNo
        acgrp_idno = Common_Procedures.get_FieldValue(con, "Ledger_Head", "AccountsGroup_IdNo", "(Ledger_idNo = " & Str(Val(Deb_ID)) & ")")

        'If Val(acgrp_idno) = 5 Then
        '    If Trim(txt_ChequeNo.Text) <> "" Then
        '        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '        DupChqNo = Common_Procedures.get_FieldValue(con, "TDS_Receipt_Head", "Party_Receipt_Code", "(company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_Idno = " & Str(Led_ID) & " and Cheque_No = '" & Trim(txt_ChequeNo.Text) & "' and Party_Receipt_Code <> '" & Trim(NewCode) & "')")

        '        'If Trim(DupChqNo) <> "" Then
        '        '    MessageBox.Show("Duplicate ChequeNo to this Party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        '    If txt_ChequeNo.Enabled And txt_ChequeNo.Visible Then txt_ChequeNo.Focus()
        '        '    Exit Sub
        '        'End If
        '    End If
        'End If

        Total_Calculation()

        'vTotRcpt = 0 : vtotCashDis = 0 : vtotRateDiff = 0 : vtotDDComm = 0 : vtotOthr = 0 : vTotAgComAmt = 0 : vTottdsAmt = 0

        'If dgv_Details_Total.RowCount > 0 Then
        '    vTotRcpt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value())
        '    vtotCashDis = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.CASH_DISC_PERC).Value())
        '    vtotRateDiff = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value())
        '    vtotDDComm = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value())
        '    vtotOthr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.DD_COMMISSION).Value())
        '    vTotAgComAmt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value())
        '    vTottdsAmt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value())

        '    vTotPartytdsAmt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value())
        'End If

        'If (vTotRcpt + vtotCashDis + vtotRateDiff + vtotDDComm + vtotOthr) = 0 Then
        '    MessageBox.Show("Invalid Receipt Amount..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dgv_Details.Rows.Count > 0 Then
        '        If dgv_Details.Enabled Then dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT)
        '    End If
        '    Exit Sub
        'End If

        TotBillAmt = 0 : TotTdsAmt = 0

        If dgv_Details_Total.RowCount > 0 Then
            TotBillAmt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.BILLAMOUNT).Value())
            TotTdsAmt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value())
        End If



        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_ReceiptNo.Text = Common_Procedures.get_MaxCode(con, "TDS_Receipt_Head", "Party_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ReceiptDate", Convert.ToDateTime(msk_ReceiptDate.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into TDS_Receipt_Head ( Party_Receipt_Code,                Company_IdNo,                    Party_Receipt_No,                                               for_OrderBy,                                                   Party_Receipt_Date,             Ledger_IdNo,             Total_Bill_Amount,                  Total_Tds_Amount,                       Remarks,                                 User_idNo         )" &
                                                          "Values('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",     '" & Trim(lbl_ReceiptNo.Text) & "',        " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text))) & ",                   @ReceiptDate,          " & Str(Val(Led_ID)) & ",  " & Str(Val(TotBillAmt)) & " ,     " & Str(Val(TotTdsAmt)) & ",    '" & Trim(txt_Remarks.Text) & "',  " & Val(lbl_UserName.Text) & "   )"
                cmd.ExecuteNonQuery()

            Else


                'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Party_Amount_Receipt_Head", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Party_Receipt_Code, Company_IdNo, for_OrderBy", tr)

                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Party_Amount_Receipt_Details", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Party_Bill_No  , Receipt_Amount  , discount_Percentage , discount_amount  , rate_difference ,  dd_commission,   Others   , Agent_Comm_Percentage,  Agent_Comm_Amount   ,  Agent_Tds_Percentage ,Agent_tds_Amount , Voucher_Bill_Code , Agent_IdNo  , Total_Receipt_Amount ", "Sl_No", "Party_Receipt_Code, For_OrderBy, Company_IdNo, Party_Receipt_No, Party_Receipt_Date, Ledger_Idno,Party_Tds_Percentage,Party_tds_Amount", tr)



                ''''''''''''


                'Da = New SqlClient.SqlDataAdapter("select count(*) from TDS_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "' and  Cheque_Return_Code <> ''", con)
                'Da.SelectCommand.Transaction = tr
                'Dt1 = New DataTable
                'Da.Fill(Dt1)
                'If Dt1.Rows.Count > 0 Then
                '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                '            Throw New ApplicationException("Already Cheque Returned")
                '            Exit Sub
                '        End If
                '    End If
                'End If

                '     Dt1.Clear()

                cmd.CommandText = "Update TDS_Receipt_Head set Party_Receipt_Date = @ReceiptDate, Ledger_IdNo = " & Val(Led_ID) & ", Remarks = '" & Trim(txt_Remarks.Text) & "',Total_Bill_Amount=" & Val(TotBillAmt) & " ,Total_Tds_Amount = " & Val(TotTdsAmt) & "  , User_idNo = " & Val(lbl_UserName.Text) & "  where  Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                cmd.CommandText = "update ClothSales_Invoice_Head set  TDS_Receipt_code ='' where TDS_Receipt_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.voucher_bill_code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "delete from voucher_bill_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()



            End If

            'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Party_Amount_Receipt_Head", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Party_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from TDS_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Party_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCol_Detail.BILLAMOUNT).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value) <> 0 Then
                        Sno = Sno + 1

                        'AgtIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(dgvCol_Detail.agent_name).Value, tr)
                        'If Val(AcPosAgIdNo) = 0 Then
                        '    AcPosAgIdNo = AgtIdNo
                        'End If



                        'RecAmt = Val(.Rows(i).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) + Val(.Rows(i).Cells(dgvCol_Detail.OTHERS).Value) + Val(.Rows(i).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value) + Val(.Rows(i).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) + Val(.Rows(i).Cells(dgvCol_Detail.DD_COMMISSION).Value) + Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value)

                        cmd.CommandText = "Insert into TDS_Receipt_Details (    Party_Receipt_Code      ,            Company_IdNo                ,             Party_Receipt_No               ,                               for_OrderBy                                               ,                      Party_receipt_Date                                    ,             Sl_No             ,                                  Party_Bill_No                        ,                        Bill_Amount                                   ,                                     Party_Tds_Percentage                      ,                       Party_Tds_Amount                         ,        Entry_Type    ,          Entry_Pk_code ,                         Invoice_Code                              ) " &
                                                                       " Values ( '" & Trim(NewCode) & "'  ,      " & Str(Val(lbl_Company.Tag)) & "  ,      '" & Trim(lbl_ReceiptNo.Text) & "'    ,   " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text))) & "     ,     '" & Trim(.Rows(i).Cells(dgvCol_Detail.BILL_DATE).Value) & "'             ,       " & Str(Val(Sno)) & "   ,       '" & Trim(.Rows(i).Cells(dgvCol_Detail.BILLNO).Value) & "'       ,       " & Val(.Rows(i).Cells(dgvCol_Detail.BILLAMOUNT).Value) & "   ,         " & Str(Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value)) & "  , " & Val(.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value) & "   ,    ''          ,          ''           , '" & Trim(.Rows(i).Cells(dgvCol_Detail.CLOTHSALES_INVOICE_CODE).Value) & "' ) "
                        cmd.ExecuteNonQuery()


                        If Trim(.Rows(i).Cells(dgvCol_Detail.CLOTHSALES_INVOICE_CODE).Value) <> "" Then
                            Nr = 0
                            cmd.CommandText = "Update ClothSales_Invoice_Head set TDS_Receipt_code = '" & Trim(NewCode) & "' Where clothsales_Invoice_Code = '" & Trim(dgv_Details.Rows(i).Cells(6).Value) & "' and Ledger_IdNo = " & Str(Val(Led_ID)) & " and TDS_Receipt_code = '' "
                            Nr = cmd.ExecuteNonQuery()


                            If Nr = 0 Then
                                MessageBox.Show("Mismatch of Party details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                tr.Rollback()
                                If txt_Remarks.Visible Then txt_Remarks.Focus()
                                Exit Sub

                            End If
                        End If



                        'If Trim(.Rows(i).Cells(dgvCol_Detail.Voucher_Bill_Code).Value) <> "" Then

                        '    Nr = 0
                        '    cmd.CommandText = "update voucher_bill_head set credit_amount = credit_amount + " & Str(Val(RecAmt)) & " where voucher_bill_code = '" & Trim(.Rows(i).Cells(dgvCol_Detail.Voucher_Bill_Code).Value) & "' and ledger_idno = " & Str(Val(Led_ID))
                        '    Nr = cmd.ExecuteNonQuery()
                        '    If Nr = 0 Then
                        '        Throw New ApplicationException("Invalid Bill Details - Bill No. " & Trim(.Rows(i).Cells(dgvCol_Detail.BILLNO).Value) & "")
                        '        Exit Sub
                        '    End If

                        '    cmd.CommandText = "insert into voucher_bill_details ( company_idno, Voucher_Bill_Code, Voucher_Bill_Date," _
                        '                        & "Ledger_Idno, entry_identification, Amount, CrDr_Type ) values ( " & Str(Val(lbl_Company.Tag)) & ", '" _
                        '                        & Trim(.Rows(i).Cells(dgvCol_Detail.Voucher_Bill_Code).Value) & "', @ReceiptDate, " _
                        '                        & Str(Led_ID) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', " _
                        '                        & Str(Val(RecAmt)) & ", 'CR' )"
                        '    cmd.ExecuteNonQuery()


                        'End If


                    End If

                Next
                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Party_Amount_Receipt_Details", "Party_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Party_Bill_No  , Receipt_Amount  , discount_Percentage , discount_amount  , rate_difference ,  dd_commission,   Others   , Agent_Comm_Percentage,  Agent_Comm_Amount   ,  Agent_Tds_Percentage ,Agent_tds_Amount , Voucher_Bill_Code , Agent_IdNo  , Total_Receipt_Amount ", "Sl_No", "Party_Receipt_Code, For_OrderBy, Company_IdNo, Party_Receipt_No, Party_Receipt_Date, Ledger_Idno, Party_Tds_Percentage, Party_tds_Amount", tr)


            End With

            '--- Accounts Posting
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

            Narr = ""

            If Trim(txt_Remarks.Text) <> "" Then Narr = Narr & "   " & Trim(txt_Remarks.Text)







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

    'Private Sub Balance_Amount_Calculation(ByVal Rw As Integer)
    '    Dim vBalAmt As Single

    '    With dgv_Details



    '        .Rows(Rw).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value = Format(Val(.Rows(Rw).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value) * Val(.Rows(Rw).Cells(dgvCol_Detail.CASH_DISC_PERC).Value) / 100, "########0.00")

    '        .Rows(Rw).Cells(dgvCol_Detail.OTHERS).Value = Format(Val(.Rows(Rw).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) * Val(.Rows(Rw).Cells(dgvCol_Detail.DD_COMMISSION).Value) / 100, "########0.00")

    '        .Rows(Rw).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value = Format(Val(.Rows(Rw).Cells(dgvCol_Detail.BILLAMOUNT).Value) * Val(.Rows(Rw).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value) / 100, "########0.00")

    '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then                   '---- Jeno Textiles (Somanur) ,sathish tex
    '            vBalAmt = Val(.Rows(Rw).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value) - (Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.OTHERS).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.DD_COMMISSION).Value))
    '        Else
    '            vBalAmt = Val(.Rows(Rw).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value) - (Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.OTHERS).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.DD_COMMISSION).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value))
    '            'vBalAmt = Val(.Rows(Rw).Cells(dgvCol_Detail.BILLAMOUNT).Value) - (Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.OTHERS).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.DD_COMMISSION).Value) + Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value))

    '        End If

    '        .Rows(Rw).Cells(dgvCol_Detail.NETBALANCEAMOUNT).Value = Format(Val(vBalAmt), "#########0.00")

    '        .Rows(Rw).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value = Format((Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.RECEIPTAMOUNT).Value) * Val(Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.AGENT_COMM_PERC).Value)) / 100), "#########0.00")

    '        .Rows(Rw).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value = Format((Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value) * Val(Val(dgv_Details.Rows(Rw).Cells(dgvCol_Detail.AGENT_TDS_PERC).Value)) / 100), "#########0.00")






    '        Total_Calculation()

    '    End With
    'End Sub

    Private Sub Total_Calculation()
        Dim vTotBal As Single, vtotCash As Single, vtotRate As Single, vtotComm As Single, vtotOtrs As Single, vTotCommAmt As Single, vTottdsAmt As Single, vPtTotdsAmt As Single, vTotBalance As Single

        Dim vTotRect As String = 0
        Dim vBlAmt As String = 0

        Dim i As Integer
        Dim Sno As Integer



        vBlAmt = 0 : vTotBal = 0 : vTotRect = 0 : vtotCash = 0 : vtotRate = 0 : vtotComm = 0 : vtotOtrs = 0 : vTotCommAmt = 0 : vTottdsAmt = 0 : vPtTotdsAmt = 0 : vTotBalance = 0 : Sno = 0

        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                Sno = Sno + 1

                .Rows(i).Cells(dgvCol_Detail.SLNO).Value = Sno

                If Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.BILLAMOUNT).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.BILLAMOUNT).Value) <> 0 Then

                    vBlAmt = vBlAmt + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.BILLAMOUNT).Value)


                    vTotRect = vTotRect + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value)


                    'vTotBal = vTotBal + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.BILLBALANCEAMOUNT).Value)
                    'vtotCash = vtotCash + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.CASH_DISC_AMOUNT).Value)
                    'vtotRate = vtotRate + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.RATE_DIFFERENCE).Value)
                    'vtotComm = vtotComm + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.DD_COMMISSION).Value)
                    'vtotOtrs = vtotOtrs + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.OTHERS).Value)
                    'vTotCommAmt = vTotCommAmt + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.AGENT_COMM_AMOUNT).Value)
                    'vTottdsAmt = vTottdsAmt + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.AGENT_TDS_AMOUNT).Value)
                    'vPtTotdsAmt = vPtTotdsAmt + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value)
                    'vTotBalance = vTotBalance + Val(dgv_Details.Rows(i).Cells(dgvCol_Detail.NETBALANCEAMOUNT).Value)
                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.BILLAMOUNT).Value = Format(Val(vBlAmt), "#########0.00")

        dgv_Details_Total.Rows(0).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value = Format(Val(vTotRect), "#########0.00")




    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_ReceiptDate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If MessageBox.Show("Do you want to select Bill Details", "FOR BILL SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_Remarks.Focus()
            End If

        End If


    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Bill Details", "FOR BILL SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_Remarks.Focus()
                '    If dgv_Details.Rows.Count > 0 Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Detail.BILLNO)

                '    Else
                '        txt_Remarks.Focus()

                '  End If

            End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

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
                If .CurrentCell.ColumnIndex >= dgvCol_Detail.BILLAMOUNT And .CurrentCell.ColumnIndex <= dgvCol_Detail.PARTY_TDS_AMOUNT Then
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
                    If .CurrentCell.ColumnIndex = dgvCol_Detail.PARTY_TDS_PERC Or .CurrentCell.ColumnIndex = dgvCol_Detail.PARTY_TDS_AMOUNT Then
                        '  Balance_Amount_Calculation(e.RowIndex)
                        'Total_Calculation()
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
                If Val(.CurrentCell.ColumnIndex) >= dgvCol_Detail.BILLAMOUNT And Val(.CurrentCell.ColumnIndex) <= dgvCol_Detail.PARTY_TDS_AMOUNT Then
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


            'If Trim(cbo_Filter_DebitorName.Text) <> "" Then
            '    Dbt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_DebitorName.Text)
            'End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If
            'If Val(Dbt_IdNo) <> 0 Then
            '    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Debtor_IdNo = " & Str(Val(Dbt_IdNo))
            'End If

            'If Val(txt_FilterChequeNo.Text) <> 0 Then
            '    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Cheque_No = '" & Trim(txt_FilterChequeNo.Text) & "'"
            'End If
            '     da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,C.Ledger_Name as Debtor_Name from TDS_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Debtor_IdNo = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Party_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Party_Receipt_No", con)
            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,C.Ledger_Name as Debtor_Name from TDS_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Party_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Party_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Party_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Party_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Total_Bill_Amount").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Tds_Amount").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Remarks").ToString

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
        ' Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, dgv_Filter_Details, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        ' Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, dgv_Filter_Details, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_Filter_PartyName_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    'Private Sub cbo_Filter_DebitorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DebitorName, cbo_Filter_PartyName, txt_FilterChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_idno = 0)")
    'End Sub

    'Private Sub cbo_Filter_DebitorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DebitorName, txt_FilterChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_idno = 0)")
    'End Sub

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

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyCode = 40 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_ReceiptDate.Focus()

            End If
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_ReceiptDate.Focus()
            End If

        End If
    End Sub

    Private Sub txt_PreparedBy_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_ReceiptDate.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_PreparedBy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_ReceiptDate.Focus()
            End If
        End If
    End Sub


    'Private Sub cbo_DebtorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6)", "(Ledger_IdNo = 0)")
    'End Sub

    'Private Sub cbo_DebtorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, txt_Remarks, Nothing, txt_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_idno = 0)")
    '    If (e.KeyValue = 38 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
    '        If dgv_Details.Rows.Count > 0 Then
    '            dgv_Details.Focus()
    '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Detail.RECEIPTAMOUNT)

    '        Else
    '            cbo_Ledger.Focus()

    '        End If
    '    End If

    'End Sub

    'Private Sub cbo_Debtorname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DebtorName, txt_ChequeNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 ) ", "(Ledger_idno = 0)")
    'End Sub

    'Private Sub cbo_DebtorName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.Control = False And e.KeyValue = 17 Then

    '        Common_Procedures.MDI_LedType = ""
    '        Dim f As New Ledger_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_DebtorName.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()

    '    End If
    'End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, k As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim dys As Integer = 0

        Dim vSELC_PKCODE As String = ""

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

                For k = 1 To 2


                    If k = 1 Then

                        vSELC_PKCODE = NewCode

                    Else

                        vSELC_PKCODE = ""

                    End If




                    ' Da = New SqlClient.SqlDataAdapter("select * from ClothSales_Invoice_Head where ledger_Idno=" & Str(Val(LedIdNo)) & " order by ClothSales_Invoice_Date, for_orderby,ClothSales_Invoice_RefNo ", con)
                    Da = New SqlClient.SqlDataAdapter("select a.* from  ClothSales_Invoice_Head a inner join Company_Head B ON a.company_idno = B.company_idno where TDS_Receipt_code = '" & Trim(vSELC_PKCODE) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Invoice_Date, a.for_orderby,a.ClothSales_Invoice_RefNo ", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)

                    If Dt1.Rows.Count > 0 Then

                        For i = 0 To Dt1.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(dgvCol_Selection.SLNO).Value = Val(SNo)
                            .Rows(n).Cells(dgvCol_Selection.BILLNO).Value = Dt1.Rows(i).Item("ClothSales_Invoice_No").ToString
                            .Rows(n).Cells(dgvCol_Selection.BILL_DATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Invoice_Date").ToString), "dd-MM-yyyy")
                            .Rows(n).Cells(dgvCol_Selection.BILLAMOUNT).Value = Format(Val(Dt1.Rows(i).Item("Total_Taxable_Amount").ToString), "#########0.00")
                            .Rows(n).Cells(dgvCol_Selection.PARTY_TDS_PERC).Value = Val(Dt1.Rows(i).Item("Tds_Percentage").ToString)
                            .Rows(n).Cells(dgvCol_Selection.PARTY_TDS_AMOUNT).Value = Val(Dt1.Rows(i).Item("tds_Amount").ToString)
                            If k = 1 Then
                                .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"
                                For j = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                Next

                            Else
                                .Rows(n).Cells(dgvCol_Selection.STS).Value = ""
                            End If

                            .Rows(n).Cells(dgvCol_Selection.CLOTHSALES_INVOICE_CODE).Value = Dt1.Rows(i).Item("ClothSales_Invoice_Code").ToString




                        Next


                    End If

                Next k

                Dt1.Clear()



            End With

            pnl_Selection.Visible = True
            pnl_Back.Enabled = False
            dgv_Selection.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID BILL SELECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
                dgv_Details.Rows(n).Cells(dgvCol_Detail.BILL_DATE).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.BILL_DATE).Value
                dgv_Details.Rows(n).Cells(dgvCol_Detail.BILLAMOUNT).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.BILLAMOUNT).Value), "#########0.00")

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.PARTY_TDS_PERC).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_PERC).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.PARTY_TDS_PERC).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.PARTY_TDS_AMOUNT).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Detail.PARTY_TDS_AMOUNT).Value = Format(Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.PARTY_TDS_AMOUNT).Value), "#########0.00")
                End If

                dgv_Details.Rows(n).Cells(dgvCol_Detail.CLOTHSALES_INVOICE_CODE).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.CLOTHSALES_INVOICE_CODE).Value

            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        txt_Remarks.Focus()



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

    Private Sub dgv_Details_Total_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details_Total.CellValueChanged
        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = dgvCol_Detail.BILLAMOUNT Or .CurrentCell.ColumnIndex = dgvCol_Detail.PARTY_TDS_AMOUNT Then
                        '  Balance_Amount_Calculation(e.RowIndex)
                        Total_Calculation()
                    End If
                End If
            End With
        Catch ex As Exception
            '------
        End Try

    End Sub

    Private Sub pnl_Selection_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Selection.Paint

    End Sub
End Class