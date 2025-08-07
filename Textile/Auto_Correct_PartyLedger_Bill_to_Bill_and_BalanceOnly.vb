Public Class Auto_Correct_PartyLedger_Bill_to_Bill_and_BalanceOnly
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Enum DgvCol_Details
        SNo '0
        PartyIdNo '1
        PartyName '2
        BalanceOnly_Amount '3
        BilltoBill_Amount '4
        SelectionStatus '5
    End Enum

    Private Sub clear()
        pnl_Back.Enabled = True
        dgv_Details.Rows.Clear()
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub LoomNo_Production_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        FrmLdSTS = False

        Try

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            FrmLdSTS = False

            new_record()

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub LoomNo_Production_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        FrmLdSTS = True
        Me.Text = ""
        con.Open()
        FrmLdSTS = True
    End Sub

    Private Sub LoomNo_Production_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub LoomNo_Production_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then

                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()

                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '----
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Exit Sub
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Exit Sub
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        get_LedgerName_List()
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        get_LedgerName_List()
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        get_LedgerName_List()
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        get_LedgerName_List()
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        get_LedgerName_List()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        get_LedgerName_List()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub



    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Private Sub btn_REFERESH_Click(sender As Object, e As EventArgs) Handles btn_REFERESH.Click
        get_LedgerName_List()
    End Sub


    Private Sub get_LedgerName_List()
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim n As Integer
        Dim SNo As Integer

        Cmd.Connection = con

        'Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Int1) select sur_name, count(*) from Ledger_Head Where sur_name <> '' group by sur_name having count(*) > 1"
        'Cmd.ExecuteNonQuery()

        Cmd.CommandText = "select Ledger_IdNo, Ledger_Name, Ledger_Type from Ledger_Head Where Bill_Type = 'BILL TO BILL' Order by Ledger_Name, Ledger_IdNo"
        'Cmd.CommandText = "select Ledger_IdNo, Ledger_Name, Ledger_Type from Ledger_Head Where sur_name IN (Select sq1.Name1 from " & Trim(Common_Procedures.EntryTempTable) & " sq1) Order by Ledger_Name, Ledger_IdNo"
        da1 = New SqlClient.SqlDataAdapter(Cmd)
        dt1 = New DataTable
        da1.Fill(dt1)

        With dgv_Details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(DgvCol_Details.SNo).Value = Val(SNo)
                    .Rows(n).Cells(DgvCol_Details.PartyIdNo).Value = dt1.Rows(i).Item("Ledger_IdNo").ToString
                    .Rows(n).Cells(DgvCol_Details.PartyName).Value = dt1.Rows(i).Item("Ledger_Name").ToString

                Next i

            End If

            Grid_Cell_DeSelect()

        End With
    End Sub

    Private Sub chk_SelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_SelectAll.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1
                .Rows(J).Cells(DgvCol_Details.SelectionStatus).Value = STS
            Next

        End With
    End Sub

    Private Sub cbo_LedgerName_Search_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LedgerName_Search.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_AlaisHead.Ledger_IdNo IN (select sq1.Ledger_IdNo from Ledger_Head sq1 where sq1.Bill_Type = 'BILL TO BILL') )", "(Ledger_IdNo = 0)")
        cbo_LedgerName_Search.SelectAll()
        cbo_LedgerName_Search.BackColor = Color.Lime
        cbo_LedgerName_Search.ForeColor = Color.Blue
    End Sub

    Private Sub cbo_LedgerName_Search_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LedgerName_Search.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LedgerName_Search, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_AlaisHead.Ledger_IdNo IN (select sq1.Ledger_IdNo from Ledger_Head sq1 where sq1.Bill_Type = 'BILL TO BILL') )", "(Ledger_IdNo = 0)")

        If ((e.KeyValue = 38 Or e.KeyValue = 40) And sender.DroppedDown = False) Or (e.Control = True And (e.KeyValue = 38 Or e.KeyValue = 40)) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)

            Else
                btn_AutoCorrect_Selected.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_LedgerName_Search_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LedgerName_Search.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LedgerName_Search, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_AlaisHead.Ledger_IdNo IN (select sq1.Ledger_IdNo from Ledger_Head sq1 where sq1.Bill_Type = 'BILL TO BILL') )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then



            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)

            Else
                btn_AutoCorrect_Selected.Focus()

            End If

            btn_Search_LedgerName_Click(sender, e)

        End If
    End Sub

    Private Sub cbo_LedgerName_Search_Leave(sender As Object, e As EventArgs) Handles cbo_LedgerName_Search.Leave
        cbo_LedgerName_Search.BackColor = Color.White
        cbo_LedgerName_Search.ForeColor = Color.Black
    End Sub

    Private Sub btn_Search_LedgerName_Click(sender As Object, e As EventArgs) Handles btn_Search_LedgerName.Click
        Dim Led_ID As Integer = 0
        Dim i As Integer

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_LedgerName_Search.Text)

        If Led_ID = 0 Then
            If cbo_LedgerName_Search.Enabled And cbo_LedgerName_Search.Visible Then cbo_LedgerName_Search.Focus()
            Exit Sub
        End If

        Grid_Cell_DeSelect()

        For i = 0 To dgv_Details.Rows.Count - 1

            If Val(Led_ID) = Val(dgv_Details.Rows(i).Cells(1).Value) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                dgv_Details.FirstDisplayedCell = dgv_Details.Rows(i).Cells(2)
                Exit Sub

            End If

        Next

    End Sub

    Private Sub btn_AutoCorrect_Selected_Click(sender As Object, e As EventArgs) Handles btn_AutoCorrect_Selected.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim i As Integer
        Dim Led_IDno As Integer
        Dim vPKCONDT_OF_PYMT_RCPT As String
        Dim Nr As Long
        Dim vCrDr_Type As String
        Dim vou_bil_code As String, vou_bil_no As String, Ent_Idn As String
        Dim Comp_IdNo As Integer
        Dim Vou_Bil_Date As Date
        Dim Par_Bil_No As String
        Dim Agt_Idno As Integer
        Dim Bil_Amt As String
        Dim vSOFTMOD_IDNo As Integer
        Dim Posting_Column As String
        Dim Adjust_Column As String
        Dim adj_amt As String = 0
        Dim acgrp_idno As Integer = 0
        Dim Parnt_CD As String = ""
        Dim vAmtSignfor_bills As String, vAmtSignfor_pymts_rcpts As String
        Dim vCRDR_for_bills As String, vCRDR_for_pymts_rcpts As String
        Dim vBILLS_VouCode As String = ""
        Dim vVOU_PAIDRCPT_Amt As String = 0
        Dim vBILL_BALAmt As String = 0
        Dim vVOUAmt As String = 0



        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        cmd.Connection = con

        tr = con.BeginTransaction

        Try

            cmd.Transaction = tr

            For rwindx = 0 To dgv_Details.Rows.Count - 1

                Led_IDno = Val(dgv_Details.Rows(rwindx).Cells(1).Value)

                If Val(Led_IDno) <> 0 Then

                    If dgv_Details.Rows(rwindx).Cells(DgvCol_Details.SelectionStatus).Value = True Then


                        '-----STEP-1 - check it is debtor or creditor

                        acgrp_idno = Common_Procedures.get_FieldValue(con, "Ledger_Head", "AccountsGroup_IdNo", "(Ledger_IdNo = " & Str(Val(Led_IDno)) & ")",, tr)
                        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno, tr)

                        If Val(acgrp_idno) = 10 Or InStr(1, Trim(LCase(Parnt_CD)) = "~10~") > 0 Then
                            vAmtSignfor_bills = " < "
                            vAmtSignfor_pymts_rcpts = " > "

                            vCRDR_for_bills = "DR"
                            vCRDR_for_pymts_rcpts = "CR"

                        Else
                            vAmtSignfor_bills = " > "
                            vAmtSignfor_pymts_rcpts = " < "
                            vCRDR_for_bills = "CR"
                            vCRDR_for_pymts_rcpts = "DR"

                        End If

                        '********************************************************************************************************************************

                        '-----STEP-2 - DELETE {voucher_bill_head} & {voucher_bill_details}

                        cmd.CommandText = "delete from voucher_bill_head where ledger_idno = " & Str(Val(Led_IDno))
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "delete from voucher_bill_details where ledger_idno = " & Str(Val(Led_IDno))
                        cmd.ExecuteNonQuery()

                        '********************************************************************************************************************************

                        '-----STEP-3 - ReSave all bill entries to generate {voucher_bill_head}

                        For k = 1 To 2

                            vPKCONDT_OF_PYMT_RCPT = " and ( a.Entry_Identification NOT LIKE 'TDSYP-%' and a.Entry_Identification NOT LIKE 'GSPTS-%'  and a.Entry_Identification NOT LIKE 'TDSCS-%'  and a.Entry_Identification NOT LIKE 'CLCTD-%'  and a.Entry_Identification NOT LIKE 'TDSSS-%'  and a.Entry_Identification NOT LIKE 'TDSCP-%' and a.Entry_Identification NOT LIKE 'TDSYS-%') "
                            If k = 1 Then
                                Da = New SqlClient.SqlDataAdapter("Select a.*, 0 as agent_idno from Voucher_Details a Where a.voucher_amount " & vAmtSignfor_bills & " 0 and a.Entry_Identification LIKE 'OPENI-%' and a.ledger_Idno = " & Str(Val(Led_IDno)) & vPKCONDT_OF_PYMT_RCPT & " Order by a.Voucher_Date, a.For_OrderBy, a.voucher_code", con)
                            Else
                                Da = New SqlClient.SqlDataAdapter("Select a.*, b.* from Voucher_Head a INNER JOIN Voucher_Details b ON a.voucher_code = b.voucher_code and a.Entry_Identification = b.Entry_Identification Where b.voucher_amount " & vAmtSignfor_bills & " 0 and b.Entry_Identification NOT LIKE 'OPENI-%' and b.ledger_Idno = " & Str(Val(Led_IDno)) & vPKCONDT_OF_PYMT_RCPT & " Order by b.Voucher_Date, b.For_OrderBy, b.voucher_code", con)
                            End If

                            Da.SelectCommand.Transaction = tr
                            Dt1 = New DataTable
                            Da.Fill(Dt1)

                            If Dt1.Rows.Count > 0 Then

                                For i = 0 To Dt1.Rows.Count - 1

                                    Comp_IdNo = Val(Dt1.Rows(i).Item("company_idno").ToString)
                                    Ent_Idn = Dt1.Rows(i).Item("Entry_Identification").ToString
                                    vou_bil_code = Ent_Idn
                                    vou_bil_no = Dt1.Rows(i).Item("Voucher_NO").ToString
                                    Vou_Bil_Date = Dt1.Rows(i).Item("Voucher_Date")
                                    Bil_Amt = Math.Abs(Val(Dt1.Rows(i).Item("voucher_amount").ToString))
                                    Par_Bil_No = vou_bil_no
                                    Agt_Idno = Val(Dt1.Rows(i).Item("agent_idno").ToString)

                                    If InStr(1, Trim(UCase(vou_bil_code)), "OPENI-") > 0 Then
                                        Par_Bil_No = "Opening"
                                        Agt_Idno = 0

                                    ElseIf InStr(1, Trim(UCase(vou_bil_code)), "GCINV-") > 0 Then
                                        Da = New SqlClient.SqlDataAdapter("Select a.ClothSales_Invoice_No, a.Agent_IdNo, a.Invoice_Amount_Receivable from ClothSales_Invoice_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.ClothSales_Invoice_Code = '" & Trim(vou_bil_code) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            Par_Bil_No = Dt2.Rows(0).Item("ClothSales_Invoice_No").ToString
                                            Agt_Idno = Val(Dt2.Rows(0).Item("Agent_IdNo").ToString)
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Invoice_Amount_Receivable").ToString))
                                        End If
                                        Dt2.Clear()

                                    ElseIf InStr(1, Trim(UCase(vou_bil_code)), "GYNSL-") > 0 Then

                                        Da = New SqlClient.SqlDataAdapter("Select a.Yarn_Sales_No, a.Agent_IdNo, a.Invoice_Amount_Receivable from Yarn_Sales_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.Yarn_Sales_Code = '" & Trim(vou_bil_code) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            Par_Bil_No = Dt2.Rows(0).Item("Yarn_Sales_No").ToString
                                            Agt_Idno = Val(Dt2.Rows(0).Item("Agent_IdNo").ToString)
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Invoice_Amount_Receivable").ToString))
                                        End If
                                        Dt2.Clear()

                                    ElseIf InStr(1, Trim(UCase(vou_bil_code)), "GYPUR-") > 0 Then
                                        Da = New SqlClient.SqlDataAdapter("Select a.Bill_No, a.Agent_IdNo, a.Bill_Amount, a.Net_Amount from Yarn_Purchase_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.Yarn_Purchase_Code = '" & Trim(vou_bil_code) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            Par_Bil_No = Dt2.Rows(0).Item("Bill_No").ToString
                                            Agt_Idno = Val(Dt2.Rows(0).Item("Agent_IdNo").ToString)
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Net_Amount").ToString))
                                        End If
                                        Dt2.Clear()

                                    ElseIf InStr(1, Trim(UCase(vou_bil_code)), "GCLPR-") > 0 Then
                                        Da = New SqlClient.SqlDataAdapter("Select a.Bill_No, a.Agent_IdNo, a.Net_Amount from Cloth_Purchase_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.Cloth_Purchase_Code = '" & Trim(vou_bil_code) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            Par_Bil_No = Dt2.Rows(0).Item("Bill_No").ToString
                                            Agt_Idno = Val(Dt2.Rows(0).Item("Agent_IdNo").ToString)
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Net_Amount").ToString))
                                        End If
                                        Dt2.Clear()

                                    ElseIf InStr(1, Trim(UCase(vou_bil_code)), "GSPUR-") > 0 Or InStr(1, Trim(UCase(vou_bil_code)), "GSSAL-") > 0 Then
                                        Da = New SqlClient.SqlDataAdapter("Select a.Other_GST_Entry_RefNo, a.Bill_No, a.Agent_IdNo, a.Bill_Amount from Other_GST_Entry_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.Other_GST_Entry_Reference_Code = '" & Microsoft.VisualBasic.Right(Trim(vou_bil_code), Len(Trim(vou_bil_code)) - 6) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            If InStr(1, Trim(UCase(vou_bil_code)), "GSPUR-") > 0 Then
                                                Par_Bil_No = Dt2.Rows(0).Item("Bill_No").ToString
                                            Else
                                                Par_Bil_No = Dt2.Rows(0).Item("Other_GST_Entry_RefNo").ToString
                                            End If
                                            Agt_Idno = Val(Dt2.Rows(0).Item("Agent_IdNo").ToString)
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Bill_Amount").ToString))
                                        End If
                                        Dt2.Clear()


                                    End If


                                    vSOFTMOD_IDNo = Val(Dt1.Rows(i).Item("software_module_idno").ToString)
                                    adj_amt = 0

                                    'If Val(Bil_Amt) = 30996 Then
                                    '    Debug.Print(Bil_Amt)
                                    'End If

                                    vCrDr_Type = "CR"
                                    Posting_Column = ""
                                    Adjust_Column = ""
                                    If Val(Dt1.Rows(i).Item("voucher_amount").ToString) < 0 Then
                                        vCrDr_Type = "DR"
                                    End If

                                    Posting_Column = IIf(Trim(UCase(vCrDr_Type)) = "CR", "Credit", "Debit")
                                    Adjust_Column = IIf(Trim(UCase(vCrDr_Type)) = "CR", "Debit", "Credit")


                                    cmd.Parameters.Clear()
                                    cmd.Parameters.AddWithValue("@VouchDate", Vou_Bil_Date)



                                    cmd.CommandText = "Insert into voucher_bill_head ( voucher_bill_code,           company_idno     ,        voucher_bill_no    ,            for_orderby      , voucher_bill_date,        ledger_idno   ,        party_bill_no      ,        agent_idno    ,      bill_amount         , " & Trim(Posting_Column) & "_amount, " & Trim(Adjust_Column) & "_amount,         crdr_type         ,        entry_identification         ,      Software_Module_IdNo      ) " _
                                                            & "  Values ( '" & Trim(vou_bil_code) & "'  , " & Str(Val(Comp_IdNo)) & ", '" & Trim(vou_bil_no) & "', " & Str(Val(vou_bil_no)) & ",     @VouchDate   , " & Str(Led_IDno) & ", '" & Trim(Par_Bil_No) & "', " & Str(Agt_Idno) & ", " & Str(Val(Bil_Amt)) & ", " & Str(Val(Bil_Amt)) & "          , " & Str(Val(adj_amt)) & "         , '" & Trim(vCrDr_Type) & "', '" & Trim(UCase(vou_bil_code)) & "' , " & Str(Val(vSOFTMOD_IDNo)) & ")"
                                    Nr = cmd.ExecuteNonQuery


                                Next i

                            End If

                        Next k

                        '********************************************************************************************************************************

                        '-----STEP-4 - ReSave all Payments/Receipts entries to generate {voucher_bill_details} and update to {voucher_bill_head}



                        For k = 1 To 2

                            vPKCONDT_OF_PYMT_RCPT = " and ( a.Entry_Identification NOT LIKE 'TDSYP-%' and a.Entry_Identification NOT LIKE 'GSPTS-%'  and a.Entry_Identification NOT LIKE 'TDSCS-%'  and a.Entry_Identification NOT LIKE 'CLCTD-%'  and a.Entry_Identification NOT LIKE 'TDSSS-%'  and a.Entry_Identification NOT LIKE 'TDSCP-%' and a.Entry_Identification NOT LIKE 'TDSYS-%') "
                            If k = 1 Then
                                Da = New SqlClient.SqlDataAdapter("Select a.*, 0 as agent_idno from Voucher_Details a Where a.voucher_amount " & vAmtSignfor_pymts_rcpts & " 0 and a.Entry_Identification LIKE 'OPENI-%' and a.ledger_Idno = " & Str(Val(Led_IDno)) & vPKCONDT_OF_PYMT_RCPT & " Order by a.Voucher_Date, a.For_OrderBy, a.voucher_code", con)
                            Else
                                Da = New SqlClient.SqlDataAdapter("Select a.*, b.* from Voucher_Head a INNER JOIN Voucher_Details b ON a.voucher_code = b.voucher_code and a.Entry_Identification = b.Entry_Identification Where b.voucher_amount " & vAmtSignfor_pymts_rcpts & " 0 and b.Entry_Identification NOT LIKE 'OPENI-%' and b.ledger_Idno = " & Str(Val(Led_IDno)) & vPKCONDT_OF_PYMT_RCPT & " Order by b.Voucher_Date, b.For_OrderBy, b.voucher_code", con)
                            End If
                            Da.SelectCommand.Transaction = tr
                            Dt1 = New DataTable
                            Da.Fill(Dt1)
                            If Dt1.Rows.Count > 0 Then

                                For i = 0 To Dt1.Rows.Count - 1

                                    Comp_IdNo = Val(Dt1.Rows(i).Item("company_idno").ToString)
                                    Ent_Idn = Dt1.Rows(i).Item("Entry_Identification").ToString
                                    vou_bil_code = Ent_Idn
                                    vou_bil_no = Dt1.Rows(i).Item("Voucher_NO").ToString
                                    Vou_Bil_Date = Dt1.Rows(i).Item("Voucher_Date")
                                    Bil_Amt = Math.Abs(Val(Dt1.Rows(i).Item("voucher_amount").ToString))
                                    Par_Bil_No = vou_bil_no
                                    Agt_Idno = Val(Dt1.Rows(i).Item("agent_idno").ToString)

                                    vSOFTMOD_IDNo = Val(Dt1.Rows(i).Item("software_module_idno").ToString)
                                    adj_amt = 0

                                    'If Val(Bil_Amt) = 30996 Then
                                    '    Debug.Print(Bil_Amt)
                                    'End If

                                    If InStr(1, Trim(UCase(vou_bil_code)), "GCINV-") > 0 Then
                                        Da = New SqlClient.SqlDataAdapter("Select a.ClothSales_Invoice_No, a.Agent_IdNo, a.Invoice_Amount_Receivable from ClothSales_Invoice_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.ClothSales_Invoice_Code = '" & Trim(vou_bil_code) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            Par_Bil_No = Dt2.Rows(0).Item("ClothSales_Invoice_No").ToString
                                            Agt_Idno = Val(Dt2.Rows(0).Item("Agent_IdNo").ToString)
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Invoice_Amount_Receivable").ToString))
                                        End If
                                        Dt2.Clear()

                                    ElseIf InStr(1, Trim(UCase(vou_bil_code)), "GYNSL-") > 0 Then

                                        Da = New SqlClient.SqlDataAdapter("Select a.Yarn_Sales_No, a.Agent_IdNo, a.Invoice_Amount_Receivable from Yarn_Sales_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.Yarn_Sales_Code = '" & Trim(vou_bil_code) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Invoice_Amount_Receivable").ToString))
                                        End If
                                        Dt2.Clear()

                                    ElseIf InStr(1, Trim(UCase(vou_bil_code)), "GYPUR-") > 0 Then
                                        Da = New SqlClient.SqlDataAdapter("Select a.Bill_No, a.Agent_IdNo, a.Bill_Amount, a.Net_Amount from Yarn_Purchase_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.Yarn_Purchase_Code = '" & Trim(vou_bil_code) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Net_Amount").ToString))
                                        End If
                                        Dt2.Clear()

                                    ElseIf InStr(1, Trim(UCase(vou_bil_code)), "GCLPR-") > 0 Then
                                        Da = New SqlClient.SqlDataAdapter("Select a.Bill_No, a.Agent_IdNo, a.Net_Amount from Cloth_Purchase_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.Cloth_Purchase_Code = '" & Trim(vou_bil_code) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Net_Amount").ToString))
                                        End If
                                        Dt2.Clear()

                                    ElseIf InStr(1, Trim(UCase(vou_bil_code)), "GSPUR-") > 0 Or InStr(1, Trim(UCase(vou_bil_code)), "GSSAL-") > 0 Then
                                        Da = New SqlClient.SqlDataAdapter("Select a.Other_GST_Entry_RefNo, a.Bill_No, a.Agent_IdNo, a.Bill_Amount from Other_GST_Entry_Head a Where a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.Other_GST_Entry_Reference_Code = '" & Microsoft.VisualBasic.Right(Trim(vou_bil_code), Len(Trim(vou_bil_code)) - 6) & "'", con)
                                        Da.SelectCommand.Transaction = tr
                                        Dt2 = New DataTable
                                        Da.Fill(Dt2)
                                        If Dt2.Rows.Count > 0 Then
                                            Bil_Amt = Math.Abs(Val(Dt2.Rows(0).Item("Bill_Amount").ToString))
                                        End If
                                        Dt2.Clear()


                                    End If


                                    vCrDr_Type = "CR"
                                    Posting_Column = ""
                                    Adjust_Column = ""
                                    If Val(Dt1.Rows(i).Item("voucher_amount").ToString) < 0 Then
                                        vCrDr_Type = "DR"
                                    End If

                                    Posting_Column = IIf(Trim(UCase(vCrDr_Type)) = "CR", "Credit", "Debit")
                                    Adjust_Column = IIf(Trim(UCase(vCrDr_Type)) = "CR", "Debit", "Credit")

                                    cmd.Parameters.Clear()
                                    cmd.Parameters.AddWithValue("@VouchDate", Vou_Bil_Date)


                                    '-----STEP-4.1 - list all pending bills {voucher_bill_head}


                                    vVOU_PAIDRCPT_Amt = Val(Bil_Amt)
                                    vBILL_BALAmt = 0
                                    vVOUAmt = 0

                                    Da = New SqlClient.SqlDataAdapter("Select a.* from voucher_bill_head a Where a.Company_Idno = " & Str(Val(Comp_IdNo)) & " and a.ledger_Idno = " & Str(Val(Led_IDno)) & " and a.crdr_type <> '" & Trim(vCrDr_Type) & "' and a.crdr_type <> '' and a.bill_amount <> 0 and (a.bill_amount - (CASE WHEN a.crdr_type = 'DR' THEN a.Credit_Amount ELSE a.Debit_Amount END)  ) > 0 Order by a.voucher_bill_date, a.for_orderby, a.voucher_bill_code", con)
                                    Da.SelectCommand.Transaction = tr
                                    Dt3 = New DataTable
                                    Da.Fill(Dt3)
                                    If Dt3.Rows.Count > 0 Then
                                        For j = 0 To Dt3.Rows.Count - 1

                                            vBILLS_VouCode = Dt3.Rows(j).Item("voucher_bill_code").ToString
                                            vBILL_BALAmt = Format(Math.Abs(Val(Dt3.Rows(j).Item("Credit_Amount").ToString) - Val(Dt3.Rows(j).Item("Debit_Amount").ToString)), "##########0.00")

                                            If Val(vVOU_PAIDRCPT_Amt) > Val(vBILL_BALAmt) Then
                                                vVOUAmt = Val(vBILL_BALAmt)
                                            Else
                                                vVOUAmt = vVOU_PAIDRCPT_Amt
                                            End If
                                            vVOU_PAIDRCPT_Amt = Format(Val(vVOU_PAIDRCPT_Amt) - Val(vVOUAmt), "##########0.00")

                                            If Val(vVOUAmt) > 0 Then

                                                cmd.CommandText = "Insert into voucher_bill_details (         Voucher_Bill_Code    ,           Company_Idno     , Voucher_Bill_Date,        Ledger_Idno        ,   entry_identification ,            Amount        ,            CrDr_Type       ) " &
                                                                    "           Values              ( '" & Trim(vBILLS_VouCode) & "', " & Str(Val(Comp_IdNo)) & ",      @VouchDate  , " & Str(Val(Led_IDno)) & ", '" & Trim(Ent_Idn) & "', " & Str(Val(vVOUAmt)) & ", '" & Trim(vCrDr_Type) & "' )"
                                                cmd.ExecuteNonQuery()

                                                Nr = 0
                                                cmd.CommandText = "Update voucher_bill_head set " & IIf(Trim(UCase(vCrDr_Type)) = "CR", "Credit_Amount", "Debit_Amount") & " = " & IIf(Trim(UCase(vCrDr_Type)) = "CR", "Credit_Amount", "Debit_Amount") & " + " & Str(Val(vVOUAmt)) & " where Company_Idno = " & Str(Val(Comp_IdNo)) & " and ledger_idno = " & Str(Val(Led_IDno)) & " and voucher_bill_code = '" & Trim(vBILLS_VouCode) & "'"
                                                'cmd.CommandText = "Update voucher_bill_head set " & IIf(Trim(UCase(vCrDr_Type)) = "CR", "Debit_Amount", "Credit_Amount") & " = " & IIf(Trim(UCase(vCrDr_Type)) = "CR", "Debit_Amount", "Credit_Amount") & " + " & Str(Val(vVOUAmt)) & " where Company_Idno = " & Str(Val(Comp_IdNo)) & " and ledger_idno = " & Str(Val(Led_IDno)) & " and voucher_bill_code = '" & Trim(vBILLS_VouCode) & "'"
                                                Nr = cmd.ExecuteNonQuery()

                                            End If


                                            If Val(vVOU_PAIDRCPT_Amt) <= 0 Then
                                                Exit For
                                            End If

                                        Next j

                                    End If



                                    '-----STEP-4.2 - save balance paid_rcpt amount as advance in {voucher_bill_details}

                                    Dim New_BillNo As String, New_BillCode As String, vFNYR As String

                                    If Val(vVOU_PAIDRCPT_Amt) > 0 Then

                                        vFNYR = Microsoft.VisualBasic.Right(Ent_Idn, 5)

                                        New_BillNo = Common_Procedures.get_MaxCode(con, "Voucher_Bill_Head", "Voucher_Bill_Code", "For_OrderBy", "", Val(Comp_IdNo), vFNYR, tr)
                                        New_BillCode = Trim(Val(Comp_IdNo)) & "-" & Trim(New_BillNo) & "/" & Trim(vFNYR)
                                        Par_Bil_No = "Advance"

                                        cmd.CommandText = "Insert into voucher_bill_head (         voucher_bill_code     ,           company_idno     ,        voucher_bill_no    ,            for_orderby      , voucher_bill_date,        ledger_idno   ,        party_bill_no      ,        agent_idno    ,      bill_amount                   , " & Trim(Posting_Column) & "_amount,  " & Trim(Adjust_Column) & "_amount ,         crdr_type         ,        entry_identification    ,      Software_Module_IdNo       ) " _
                                                            & "         Values           ( '" & Trim(New_BillCode) & "'  , " & Str(Val(Comp_IdNo)) & ", '" & Trim(New_BillNo) & "', " & Str(Val(New_BillNo)) & ",     @VouchDate   , " & Str(Led_IDno) & ", '" & Trim(Par_Bil_No) & "', " & Str(Agt_Idno) & ", " & Str(Val(vVOU_PAIDRCPT_Amt)) & ", " & Str(Val(vVOU_PAIDRCPT_Amt)) & ",  " & Str(Val(vVOU_PAIDRCPT_Amt)) & ", '" & Trim(vCrDr_Type) & "', '" & Trim(UCase(Ent_Idn)) & "' , " & Str(Val(vSOFTMOD_IDNo)) & " ) "
                                        cmd.ExecuteNonQuery()

                                        vVOU_PAIDRCPT_Amt = 0

                                    End If


                                Next i


                            End If

                        Next k

LOOP1:
                        '********************************************************************************************************************************

                    End If

                End If

            Next rwindx

            tr.Commit()

            MessageBox.Show("Sucessfully done.", "FOR AUTO CORRECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT AUTO CORRECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



    End Sub
End Class