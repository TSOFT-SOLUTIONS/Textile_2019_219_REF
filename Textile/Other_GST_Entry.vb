Imports System.Drawing.Printing
Imports System.IO
Public Class Other_GST_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = ""
    Private PkCondition_TDS As String = "GSPTS-"
    Private Other_Condition As String = ""
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private NoCalc_Status As Boolean = False

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private vEntryType As String = ""

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private prn_HdDt_VAT As New DataTable
    Private prn_DetDt_VAT As New DataTable

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable

    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer

    Private prn_PageNo As Integer

    Private prn_DetAr(200, 10) As String
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False
    Private vEMAIL_Attachment_FileName As String
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_OriDupTri As String = ""


    Private DeleteAll_STS As Boolean = False
    Private vSPEC_KEYS As New HashSet(Of Keys)()

    Public Sub New(ByVal EntryType As String)
        vEntryType = Trim(UCase(EntryType))
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub


    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As GroupBox

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False
        EMAIL_Status = False

        lbl_EntryNo.Text = ""
        lbl_EntryNo.ForeColor = Color.Black

        pnl_back.Enabled = True
        pnl_Filter.Visible = False

        pnl_GSTTax_Details.Visible = False

        cbo_OnAc_Type.Text = ""
        txt_DiscountPerc.Text = ""
        lbl_Grid_FooterDiscPerc.Text = ""
        lbl_Grid_FooterDiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_TotalAmount.Text = ""
        txt_Hsn_Sac_Code.Text = ""
        txt_GstPerc.Text = ""
        txt_Tds_Percentage.Text = ""
        txt_AddLess.Text = ""
        lbl_Tds_Amount.Text = ""
        lbl_BillAmount.Text = ""
        Cbo_Transport.Text = ""
        txt_EWayBillNo.Text = ""
        grp_EInvoice.Visible = False
        cbo_DeliveryTo.Text = ""
        cbo_InvoiceSufixNo.Text = ""
        cbo_Agent.Text = ""
        cbo_Agent.Tag = ""
        txt_EWBNo.Text = ""
        rtbEWBResponse.Text = ""


        lbl_UserName_CreatedBy.Text = ""
        lbl_UserName_ModifiedBy.Text = ""

        chk_Tax_FullRoundOff_Status.Checked = False
        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""

            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""

            ElseIf TypeOf obj Is DateTimePicker Then
                obj.text = ""

            ElseIf TypeOf obj Is GroupBox Then
                grpbx = obj
                For Each ctrl1 In grpbx.Controls
                    If TypeOf ctrl1 Is TextBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is ComboBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is DateTimePicker Then
                        ctrl1.text = ""
                    End If
                Next

            ElseIf TypeOf obj Is Panel Then
                pnl1 = obj
                If Trim(UCase(pnl1.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                    For Each ctrl2 In pnl1.Controls
                        If TypeOf ctrl2 Is TextBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is ComboBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is DataGridView Then
                            ctrl2.Rows.Clear()
                        ElseIf TypeOf ctrl2 Is DateTimePicker Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is Panel Then
                            pnl2 = ctrl2
                            If Trim(UCase(pnl2.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                                For Each ctrl3 In pnl2.Controls
                                    If TypeOf ctrl3 Is TextBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is ComboBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is DateTimePicker Then
                                        ctrl3.text = ""
                                    End If
                                Next
                            End If

                        End If

                    Next

                End If

            End If

        Next

        lbl_GrossAmount.Text = ""

        txt_Amount.Text = ""
        lbl_TotalAmount.Text = ""

        lbl_GrossAmount.Text = ""
        txt_DiscountPerc.Text = ""
        txt_DiscountAmount.Text = ""
        txt_CashDiscPerc.Text = ""
        lbl_CashDiscAmount.Text = ""
        txt_Remarks.Text = ""

        lbl_Grid_FooterDiscAmount.Text = ""

        dgv_Details.Rows.Clear()
        lbl_TaxableValue.Text = ""


        dgv_GSTTax_Details.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Add()

        lbl_CGstAmount.Text = ""
        lbl_SGstAmount.Text = ""
        lbl_IGstAmount.Text = ""

        lbl_NetAmount.Text = ""
        cbo_Ledger.Text = ""


        cbo_EntryAcName.Text = Common_Procedures.Ledger_IdNoToName(con, 22)
        txt_SlNo.Text = "1"

        NoCalc_Status = False
        Cbo_VehicleNo.Text = ""


        grp_EInvoice.Visible = False
        pic_IRN_QRCode_Image.BackgroundImage = Nothing
        txt_IR_No.Text = ""
        txt_eInvoiceNo.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckDate.Text = ""
        txt_eInvoice_CancelStatus.Text = ""
        rtbeInvoiceResponse.Text = ""

        txt_Tcs_Name.Text = "TCS"
        txt_TcsPerc.Text = ""
        lbl_TcsAmount.Text = ""
        '  pnl_TotalSales_Amount.Visible = True
        txt_TCS_TaxableValue.Text = ""
        txt_TcsPerc.Enabled = False
        txt_TCS_TaxableValue.Enabled = False
        lbl_TotalSales_Amount_Current_Year.Text = "0.00"
        lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
        chk_TCSAmount_RoundOff_STS.Checked = True

        lbl_Invoice_Value_Before_TCS.Text = ""
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = ""

        txt_TDS_Value.Text = ""

        chk_TCS_Tax.Checked = False
        chk_TDS_Tax.Checked = False


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox

        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            Grid_Cell_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If
    End Sub

    Private Sub ControlLostFocus2(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(255, 255, 192)
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}") : e.Handled = True
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}") : e.Handled = True
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}") : e.Handled = True
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Trim(no) = "" Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Ledger_Name as Entry_Ac_Name , d.Ledger_Name as OnAccount_Name from Other_GST_Entry_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Other_GST_Entry_Ac_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.OnAccount_IdNo = d.Ledger_IdNo Where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' and " & Other_Condition, con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Other_GST_Entry_Reference_No").ToString
                txt_EntryPrefixNo.Text = dt1.Rows(0).Item("Other_GST_Entry_PrefixNo").ToString

                cbo_InvoiceSufixNo.Text = dt1.Rows(0).Item("Invoice_SuffixNo").ToString

                If Val(dt1.Rows(0).Item("created_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("created_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_CreatedBy.Text = "Created by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString)
                    Else
                        lbl_UserName_CreatedBy.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString))))
                    End If
                End If
                If Val(dt1.Rows(0).Item("Last_modified_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("Last_modified_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_ModifiedBy.Text = "Last Modified by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("Last_modified_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString)
                    End If
                End If

                lbl_EntryNo.Text = dt1.Rows(0).Item("Other_GST_Entry_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Other_GST_Entry_Date").ToString

                msk_Date.Text = dtp_Date.Text
                msk_Date.SelectionStart = 0

                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString

                Cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                cbo_EntryAcName.Text = dt1.Rows(0).Item("Entry_Ac_Name").ToString
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("Add_Less").ToString), "########0.00")
                cbo_OnAc_Type.Text = dt1.Rows(0).Item("OnAccount_Name").ToString

                If txt_BillNo.Visible Then
                    txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                End If
                If msk_BillDate.Visible Then
                    If IsDBNull(dt1.Rows(0).Item("Bill_Date")) = False Then
                        If IsDate(dt1.Rows(0).Item("Bill_Date")) Then
                            dtp_BillDate.Text = dt1.Rows(0).Item("Bill_Date").ToString
                            msk_BillDate.Text = dtp_BillDate.Text
                            msk_BillDate.SelectionStart = 0
                        End If
                    End If
                End If

                txt_IR_No.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                txt_eInvoiceNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_Date")) Then txt_eInvoiceAckDate.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_Date").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancelled_Status")) Then txt_eInvoice_CancelStatus.Text = IIf(dt1.Rows(0).Item("E_Invoice_Cancelled_Status") = True, "Cancelled", "Active")
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))

                If IsDBNull(dt1.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(dt1.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                pic_IRN_QRCode_Image.BackgroundImage = Image.FromStream(ms)

                            End If
                        End Using
                    End If
                End If

                txt_EWayBillNo.Text = dt1.Rows(0).Item("Eway_BillNo").ToString
                txt_EWBNo.Text = dt1.Rows(0).Item("Eway_BillNo").ToString

                If Not IsDBNull(dt1.Rows(0).Item("EWB_No")) Then txt_eWayBill_No.Text = Trim(dt1.Rows(0).Item("EWB_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Date")) Then txt_EWB_Date.Text = Trim(dt1.Rows(0).Item("EWB_Date").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Valid_Upto")) Then txt_EWB_ValidUpto.Text = Trim(dt1.Rows(0).Item("EWB_Valid_Upto").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Cancelled")) Then
                    If dt1.Rows(0).Item("EWB_Cancelled") = True Then
                        txt_EWB_Cancel_Status.Text = "Cancelled"
                    Else
                        txt_EWB_Cancel_Status.Text = "Active"
                    End If
                End If

                Cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Unit_Name from Other_GST_Entry_Details a LEFT OUTER JOIN Unit_Head b on a.unit_idno = b.unit_idno where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()

                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_Details.Rows.Add()

                            SNo = SNo + 1
                            dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                            dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Particulars").ToString
                            dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Unit_Name").ToString
                            dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Hsn_Sac_Code").ToString
                            dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Gst_Perc").ToString)
                            dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                            dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Discount_Perc").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Footer_Cash_Discount_Perc").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Footer_Cash_Discount_Amount").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Taxable_Value").ToString), "########0.00")

                        Next i

                    End If
                    dt2.Clear()

                    SNo = SNo + 1
                    txt_SlNo.Text = Val(SNo)

                    For i = 0 To .Rows.Count - 1
                        dgv_Details.Rows(n).Cells(0).Value = i + 1
                    Next

                    With dgv_Details_Total
                        If .RowCount = 0 Then .Rows.Add()
                        .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                        .Rows(0).Cells(7).Value = Val(dt1.Rows(0).Item("Total_Sub_Amount").ToString)
                        .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00")
                        .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                        .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Total_Footer_Cash_Discount_Amount").ToString), "########0.00")
                        .Rows(0).Cells(13).Value = Format(Val(dt1.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00")
                    End With


                    '***** GST START *****
                    da1 = New SqlClient.SqlDataAdapter("Select a.* from Other_GST_Entry_Tax_Details a Where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' ", con)
                    dt2 = New DataTable
                    da1.Fill(dt2)

                    With dgv_GSTTax_Details

                        .Rows.Clear()
                        SNo = 0

                        If dt2.Rows.Count > 0 Then

                            For i = 0 To dt2.Rows.Count - 1

                                n = .Rows.Add()

                                SNo = SNo + 1

                                .Rows(n).Cells(0).Value = SNo
                                .Rows(n).Cells(1).Value = Trim(dt2.Rows(i).Item("Hsn_Sac_Code").ToString)
                                .Rows(n).Cells(2).Value = IIf(Val(dt2.Rows(i).Item("Taxable_Amount").ToString) <> 0, Format(Val(dt2.Rows(i).Item("Taxable_Amount").ToString), "############0.00"), "")
                                .Rows(n).Cells(3).Value = IIf(Val(dt2.Rows(i).Item("CGST_Percentage").ToString) <> 0, Val(dt2.Rows(i).Item("CGST_Percentage").ToString), "")
                                .Rows(n).Cells(4).Value = IIf(Val(dt2.Rows(i).Item("CGST_Amount").ToString) <> 0, Format(Val(dt2.Rows(i).Item("CGST_Amount").ToString), "##########0.00"), "")
                                .Rows(n).Cells(5).Value = IIf(Val(dt2.Rows(i).Item("SGST_Percentage").ToString) <> 0, Val(dt2.Rows(i).Item("SGST_Percentage").ToString), "")
                                .Rows(n).Cells(6).Value = IIf(Val(dt2.Rows(i).Item("SGST_Amount").ToString) <> 0, Format(Val(dt2.Rows(i).Item("SGST_Amount").ToString), "###########0.00"), "")
                                .Rows(n).Cells(7).Value = IIf(Val(dt2.Rows(i).Item("IGST_Percentage").ToString) <> 0, Val(dt2.Rows(i).Item("IGST_Percentage").ToString), "")
                                .Rows(n).Cells(8).Value = IIf(Val(dt2.Rows(i).Item("IGST_Amount").ToString) <> 0, Format(Val(dt2.Rows(i).Item("IGST_Amount").ToString), "###########0.00"), "")

                            Next i

                        End If

                    End With


                    NoCalc_Status = False
                    Total_GSTTax_Calculation()
                    NoCalc_Status = True


                    If IsDBNull(dt1.Rows(0).Item("TaxAmount_RoundOff_Status")) = False Then
                        If Val(dt1.Rows(0).Item("TaxAmount_RoundOff_Status")) = 1 Then
                            chk_Tax_FullRoundOff_Status.Checked = True
                        End If
                    End If

                    lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                    txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                    lbl_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                    lbl_TaxableValue.Text = Format(Val(dt1.Rows(0).Item("Taxable_Value").ToString), "########0.00")
                    lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGST_Amount").ToString), "########0.00")
                    lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGST_Amount").ToString), "########0.00")
                    lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGST_AMount").ToString), "########0.00")
                    lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off_Amount").ToString), "########0.00")
                    lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                    txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                    If Val(dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False
                    txt_TCS_TaxableValue.Text = dt1.Rows(0).Item("TCS_Taxable_Value").ToString
                    If Val(dt1.Rows(0).Item("EDIT_TCS_TaxableValue").ToString) = 1 Then
                        txt_TcsPerc.Enabled = True
                        txt_TCS_TaxableValue.Enabled = True
                    End If
                    If IsDBNull(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                        If Val(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                    End If
                    txt_TcsPerc.Text = Val(dt1.Rows(0).Item("Tcs_Percentage").ToString)
                    lbl_TcsAmount.Text = dt1.Rows(0).Item("TCS_Amount").ToString

                    If Val(dt1.Rows(0).Item("TDS_Tax_Status").ToString) = 1 Then chk_TDS_Tax.Checked = True Else chk_TDS_Tax.Checked = False

                    txt_Tds_Percentage.Text = Format(Val(dt1.Rows(0).Item("Tds_Percentage").ToString), "########0.00")
                    lbl_Tds_Amount.Text = Format(Val(dt1.Rows(0).Item("Tds_Amount").ToString), "########0.00")
                    txt_TDS_Value.Text = Format(Val(dt1.Rows(0).Item("TDS_Taxable_Value").ToString), "########0.00")
                    lbl_BillAmount.Text = Format(Val(dt1.Rows(0).Item("Bill_Amount").ToString), "########0.00")
                    cbo_Reason_For_Note.Text = Trim(dt1.Rows(0).Item("Reason_For_Issuing_Note").ToString)
                    cbo_Unregister_Type.Text = Trim(dt1.Rows(0).Item("Unregister_Type").ToString)

                End With


            Else

                new_record()


            End If
            get_Ledger_TotalSales()
            dt1.Clear()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            NoCalc_Status = False

            dt1.Dispose()
            dt2.Dispose()

            da1.Dispose()
            da2.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub Other_GST_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EntryAcName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EntryAcName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Other_GST_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Me.Text = ""

        btn_JSON_Einvoice_Gen.Visible = False
        txt_BillNo.Visible = True
        lbl_BillNo_Caption.Visible = True
        lbl_BillNo_Caption_Star.Visible = True
        msk_BillDate.Visible = True
        dtp_BillDate.Visible = True
        lbl_BillDate_Caption.Visible = True
        btn_EWayBIll_Generation.Visible = False


        Pk_Condition = ""
        Other_Condition = ""
        txt_AddLess.Text = ""


        lbl_NetAmountCaption.Text = "Net Amount"
        If Trim(UCase(vEntryType)) = "PURC" Then

            Pk_Condition = "GSPUR-"

            lbl_EntryType_Heading.Text = "PURCHASE ENTRY"
            lbl_CreditNoteAc_Caption.Text = "Purchase A/C"
            lbl_RefNo_Caption.Text = "GRN No."
            lbl_RefDate_Caption.Text = "GRN Date"


            lbl_TdsPercCaptiion.Visible = True

            chk_TDS_Tax.Visible = True
            txt_TDS_Value.Visible = True

            lbl_Tds_Amount.Visible = True
            txt_Tds_Percentage.Visible = True

            lbl_OnAc_Type_Caption.Visible = True
            cbo_OnAc_Type.Visible = True
            cbo_OnAc_Type.BackColor = Color.White
            cbo_OnAc_Type.Width = cbo_Unregister_Type.Width
            cbo_Reason_For_Note.Visible = True
            lbl_Reason_For_Note_Caption.Visible = True
            cbo_Unregister_Type.Visible = False
            lbl_Unregister_Type_Caption.Visible = False

            lbl_BillAmountCaption.Visible = True
            lbl_BillAmount.Visible = True
            lbl_NetAmountCaption.Text = "Bill Amount"


            txt_Tcs_Name.Text = "TCS"
            txt_TcsPerc.Text = ""
            lbl_TcsAmount.Text = ""
            pnl_TotalSales_Amount.Visible = True
            txt_TCS_TaxableValue.Text = ""
            txt_TcsPerc.Enabled = False
            txt_TCS_TaxableValue.Enabled = False
            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            chk_TCSAmount_RoundOff_STS.Checked = True

            lbl_Invoice_Value_Before_TCS.Text = ""
            lbl_RoundOff_Invoice_Value_Before_TCS.Text = ""
            btn_EDIT_TCS_TaxableValue.Visible = True
            'chk_TCS_Tax.Checked = True
            Label42.Visible = True

            lbl_IRN_Caption.Visible = False
            txt_IR_No.Visible = False
            lbl_Einvoice_Caption.Visible = False
            pic_IRN_QRCode_Image.Visible = False
            pic_IRN_QRCode_Image_forPrinting.Visible = False
            Btn_Qr_Code_Add.Visible = False
            Btn_Qr_Code_Close.Visible = False
            btn_EInvoice_Generation.Visible = False
            txt_EWayBillNo.Visible = False
            lbl_caption_EwayBill.Visible = False

            lbl_VehicleCaption.Visible = True
            Cbo_VehicleNo.Visible = True
            lbl_Transport_Caption.Visible = True
            Cbo_Transport.Visible = True

            btn_EWayBIll_Generation.Visible = False


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1135" Then
                lbl_TdsPercCaptiion.Visible = True
                lbl_Tds_Amount.Visible = True
                txt_Tds_Percentage.Visible = True

                chk_TDS_Tax.Visible = True
                txt_TDS_Value.Visible = True
            End If

        ElseIf Trim(UCase(vEntryType)) = "SALE" Then
            Pk_Condition = "GSSAL-"
            lbl_EntryType_Heading.Text = "SALES ENTRY"
            lbl_CreditNoteAc_Caption.Text = "Sales A/C"
            lbl_RefNo_Caption.Text = "Invoice No."
            lbl_RefDate_Caption.Text = "Inv.Date"

            lbl_OnAc_Type_Caption.Visible = True
            cbo_OnAc_Type.Visible = True
            cbo_OnAc_Type.BackColor = Color.White
            cbo_OnAc_Type.Width = cbo_Unregister_Type.Width
            cbo_DeliveryTo.Visible = True
            Label6.Visible = True
            txt_BillNo.Visible = False
            lbl_BillNo_Caption.Visible = False
            lbl_BillNo_Caption_Star.Visible = False
            msk_BillDate.Visible = False
            dtp_BillDate.Visible = False
            lbl_BillDate_Caption.Visible = False

            cbo_Reason_For_Note.Visible = False
            lbl_Reason_For_Note_Caption.Visible = False
            cbo_Unregister_Type.Visible = False
            lbl_Unregister_Type_Caption.Visible = False

            txt_Tcs_Name.Text = "TCS"
            txt_TcsPerc.Text = ""
            lbl_TcsAmount.Text = ""
            pnl_TotalSales_Amount.Visible = True
            txt_TCS_TaxableValue.Text = ""
            txt_TcsPerc.Enabled = False
            txt_TCS_TaxableValue.Enabled = False
            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            chk_TCSAmount_RoundOff_STS.Checked = True
            btn_EDIT_TCS_TaxableValue.Visible = True
            lbl_Invoice_Value_Before_TCS.Text = ""
            lbl_RoundOff_Invoice_Value_Before_TCS.Text = ""

            'chk_TCS_Tax.Checked = True
            Label42.Visible = True
            Label13.Text = "Current Year Of Sales  :"
            Label71.Text = "Previous Year Of Sales :"

            lbl_IRN_Caption.Visible = True
            txt_IR_No.Visible = True
            lbl_Einvoice_Caption.Visible = True
            pic_IRN_QRCode_Image.Visible = True
            pic_IRN_QRCode_Image_forPrinting.Visible = True
            Btn_Qr_Code_Add.Visible = True
            Btn_Qr_Code_Close.Visible = True
            btn_EInvoice_Generation.Visible = True
            txt_EWayBillNo.Visible = True
            lbl_caption_EwayBill.Visible = True

            lbl_VehicleCaption.Visible = True
            Cbo_VehicleNo.Visible = True
            lbl_Transport_Caption.Visible = True
            Cbo_Transport.Visible = True

            btn_EWayBIll_Generation.Visible = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1135" Then
                lbl_TdsPercCaptiion.Visible = True
                lbl_Tds_Amount.Visible = True
                txt_Tds_Percentage.Visible = True

                chk_TDS_Tax.Visible = True
                txt_TDS_Value.Visible = True
            End If

        ElseIf Trim(UCase(vEntryType)) = "CRNT" Then

            Pk_Condition = "GSCRN-"
            lbl_EntryType_Heading.Text = "CREDIT NOTE ENTRY"
            lbl_CreditNoteAc_Caption.Text = "Credit Note A/C"
            lbl_RefNo_Caption.Text = "Credit Note No."
            lbl_RefDate_Caption.Text = "Cr.Nt Date"

            lbl_OnAc_Type_Caption.Visible = False
            cbo_OnAc_Type.Visible = False

            cbo_Reason_For_Note.Visible = False
            lbl_Reason_For_Note_Caption.Visible = True
            cbo_Unregister_Type.Visible = True
            lbl_Unregister_Type_Caption.Visible = True
            txt_Tcs_Name.Text = "TCS"
            txt_TcsPerc.Text = ""
            lbl_TcsAmount.Text = ""
            pnl_TotalSales_Amount.Visible = True
            txt_TCS_TaxableValue.Text = ""
            txt_TcsPerc.Enabled = False
            txt_TCS_TaxableValue.Enabled = False
            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            chk_TCSAmount_RoundOff_STS.Checked = True
            btn_EDIT_TCS_TaxableValue.Visible = True
            lbl_Invoice_Value_Before_TCS.Text = ""
            lbl_RoundOff_Invoice_Value_Before_TCS.Text = ""

            lbl_TdsPercCaptiion.Visible = True

            chk_TDS_Tax.Visible = True
            txt_TDS_Value.Visible = True

            lbl_Tds_Amount.Visible = True
            txt_Tds_Percentage.Visible = True

            'chk_TCS_Tax.Checked = True
            Label42.Visible = True
            Label13.Text = "Current Year Of Sales  :"
            Label71.Text = "Previous Year Of Sales :"

            lbl_IRN_Caption.Visible = True
            txt_IR_No.Visible = True
            lbl_Einvoice_Caption.Visible = True
            pic_IRN_QRCode_Image.Visible = True
            pic_IRN_QRCode_Image_forPrinting.Visible = True
            Btn_Qr_Code_Add.Visible = True
            Btn_Qr_Code_Close.Visible = True
            btn_EInvoice_Generation.Visible = True
            txt_EWayBillNo.Visible = True
            lbl_caption_EwayBill.Visible = True

            lbl_VehicleCaption.Visible = True
            Cbo_VehicleNo.Visible = True


            btn_EWayBIll_Generation.Visible = False

        ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
            Pk_Condition = "GSDBN-"
            lbl_EntryType_Heading.Text = "DEBIT NOTE ENTRY"
            lbl_CreditNoteAc_Caption.Text = "Debit Note A/C"
            lbl_RefNo_Caption.Text = "Debit Note No."
            lbl_RefDate_Caption.Text = "Dr.Nt Date"

            lbl_OnAc_Type_Caption.Visible = False
            cbo_OnAc_Type.Visible = False

            cbo_Reason_For_Note.Visible = False
            lbl_Reason_For_Note_Caption.Visible = True

            lbl_Reason_For_Note_Caption.Left = lbl_IRN_Caption.Left
            cbo_Reason_For_Note.Left = txt_IR_No.Left
            cbo_Reason_For_Note.Width = cbo_Ledger.Width

            cbo_Unregister_Type.Visible = True
            lbl_Unregister_Type_Caption.Visible = True


            txt_Tcs_Name.Text = "TCS"
            txt_TcsPerc.Text = ""
            lbl_TcsAmount.Text = ""
            pnl_TotalSales_Amount.Visible = True
            txt_TCS_TaxableValue.Text = ""
            txt_TcsPerc.Enabled = False
            txt_TCS_TaxableValue.Enabled = False
            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            chk_TCSAmount_RoundOff_STS.Checked = True
            btn_EDIT_TCS_TaxableValue.Visible = True
            lbl_Invoice_Value_Before_TCS.Text = ""
            lbl_RoundOff_Invoice_Value_Before_TCS.Text = ""

            'chk_TCS_Tax.Checked = True
            Label42.Visible = True
            Label13.Text = "Current Year Of Sales  :"
            Label71.Text = "Previous Year Of Sales :"


            lbl_IRN_Caption.Visible = False
            txt_IR_No.Visible = False
            lbl_Einvoice_Caption.Visible = False
            pic_IRN_QRCode_Image.Visible = False
            pic_IRN_QRCode_Image_forPrinting.Visible = False
            Btn_Qr_Code_Add.Visible = False
            Btn_Qr_Code_Close.Visible = False
            btn_EInvoice_Generation.Visible = False
            txt_EWayBillNo.Visible = False
            lbl_caption_EwayBill.Visible = False

            lbl_VehicleCaption.Visible = True
            Cbo_VehicleNo.Visible = True

            lbl_IRN_Caption.Visible = True ' flse
            txt_IR_No.Visible = True ' flse
            lbl_Einvoice_Caption.Visible = True
            pic_IRN_QRCode_Image.Visible = True
            pic_IRN_QRCode_Image_forPrinting.Visible = True
            Btn_Qr_Code_Add.Visible = True
            Btn_Qr_Code_Close.Visible = True
            btn_EInvoice_Generation.Visible = True
            lbl_caption_EwayBill.Visible = True '  flse
            txt_EWayBillNo.Visible = True '  flse

            btn_EWayBIll_Generation.Visible = True

        ElseIf Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then
            Pk_Condition = "GJINV-"
            lbl_EntryType_Heading.Text = "JOBWORK INVOICE"
            lbl_CreditNoteAc_Caption.Text = "Sales A/C"
            lbl_RefNo_Caption.Text = "Invoice No."
            lbl_RefDate_Caption.Text = "Inv.Date"

            lbl_OnAc_Type_Caption.Visible = True
            cbo_OnAc_Type.Visible = True
            cbo_OnAc_Type.BackColor = Color.White
            cbo_OnAc_Type.Width = cbo_Unregister_Type.Width

            txt_BillNo.Visible = False
            lbl_BillNo_Caption.Visible = False
            lbl_BillNo_Caption_Star.Visible = False

            msk_BillDate.Visible = False
            dtp_BillDate.Visible = False
            lbl_BillDate_Caption.Visible = False

            cbo_Reason_For_Note.Visible = False
            lbl_Reason_For_Note_Caption.Visible = False
            cbo_Unregister_Type.Visible = False
            lbl_Unregister_Type_Caption.Visible = False

            txt_Tcs_Name.Visible = False
            txt_TcsPerc.Visible = False
            lbl_TcsAmount.Visible = False
            pnl_TotalSales_Amount.Visible = False
            txt_TCS_TaxableValue.Visible = False
            txt_TcsPerc.Visible = False
            txt_TCS_TaxableValue.Visible = False
            lbl_TotalSales_Amount_Current_Year.Visible = False
            lbl_TotalSales_Amount_Previous_Year.Visible = False
            chk_TCSAmount_RoundOff_STS.Visible = False

            lbl_Invoice_Value_Before_TCS.Visible = False
            lbl_RoundOff_Invoice_Value_Before_TCS.Visible = False
            btn_EDIT_TCS_TaxableValue.Visible = False
            chk_TCS_Tax.Visible = False
            Label42.Visible = False


            lbl_IRN_Caption.Visible = False
            txt_IR_No.Visible = False
            lbl_Einvoice_Caption.Visible = False
            pic_IRN_QRCode_Image.Visible = False
            pic_IRN_QRCode_Image_forPrinting.Visible = False
            Btn_Qr_Code_Add.Visible = False
            Btn_Qr_Code_Close.Visible = False
            btn_EInvoice_Generation.Visible = False
            txt_EWayBillNo.Visible = False
            lbl_caption_EwayBill.Visible = False

            lbl_VehicleCaption.Visible = False
            Cbo_VehicleNo.Visible = False

        ElseIf Trim(UCase(vEntryType)) = "ADV.PYMT" Then
            Pk_Condition = "GADVP-"
            lbl_EntryType_Heading.Text = "ADVANCE PAYMENT ENTRY"
            lbl_CreditNoteAc_Caption.Text = "Bank/Cash A/C"
            lbl_RefNo_Caption.Text = "Voucher No."
            lbl_RefDate_Caption.Text = "Vou.Date"

            '************************************************************
            lbl_OnAc_Type_Caption.Visible = True
            cbo_OnAc_Type.Visible = True
            cbo_Reason_For_Note.Visible = False
            lbl_Reason_For_Note_Caption.Visible = False
            cbo_Unregister_Type.Visible = False
            lbl_Unregister_Type_Caption.Visible = False

            txt_BillNo.Visible = False
            lbl_BillNo_Caption.Visible = False
            lbl_BillNo_Caption_Star.Visible = False
            msk_BillDate.Visible = False
            dtp_BillDate.Visible = False
            lbl_BillDate_Caption.Visible = False
            txt_Tcs_Name.Visible = False
            txt_TcsPerc.Visible = False
            lbl_TcsAmount.Visible = False
            pnl_TotalSales_Amount.Visible = False
            txt_TCS_TaxableValue.Visible = False
            txt_TcsPerc.Visible = False
            txt_TCS_TaxableValue.Visible = False
            lbl_TotalSales_Amount_Current_Year.Visible = False
            lbl_TotalSales_Amount_Previous_Year.Visible = False
            chk_TCSAmount_RoundOff_STS.Visible = False

            lbl_Invoice_Value_Before_TCS.Visible = False
            lbl_RoundOff_Invoice_Value_Before_TCS.Visible = False
            btn_EDIT_TCS_TaxableValue.Visible = False
            chk_TCS_Tax.Visible = False
            Label42.Visible = False


            lbl_IRN_Caption.Visible = False
            txt_IR_No.Visible = False
            lbl_Einvoice_Caption.Visible = False
            pic_IRN_QRCode_Image.Visible = False
            pic_IRN_QRCode_Image_forPrinting.Visible = False
            Btn_Qr_Code_Add.Visible = False
            Btn_Qr_Code_Close.Visible = False
            btn_EInvoice_Generation.Visible = False
            txt_EWayBillNo.Visible = False
            lbl_caption_EwayBill.Visible = False

            lbl_VehicleCaption.Visible = False
            Cbo_VehicleNo.Visible = False

        End If

        Other_Condition = "(Other_GST_Entry_Reference_Code LIKE '" & Trim(Pk_Condition) & "%' and Other_GST_Entry_Type = '" & Trim(UCase(vEntryType)) & "')"


        con.Open()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        pnl_GSTTax_Details.Visible = False
        pnl_GSTTax_Details.Left = (Me.Width - pnl_GSTTax_Details.Width) \ 2
        pnl_GSTTax_Details.Top = ((Me.Height - pnl_GSTTax_Details.Height) \ 2) - 100
        pnl_GSTTax_Details.BringToFront()

        cbo_Reason_For_Note.Items.Clear()
        cbo_Reason_For_Note.Items.Add("01-Sales Return")
        cbo_Reason_For_Note.Items.Add("02-Post Sale Discount")
        cbo_Reason_For_Note.Items.Add("03-Deficiency in Services")
        cbo_Reason_For_Note.Items.Add("04-Correction in Invoice")
        cbo_Reason_For_Note.Items.Add("05-Chancge in POS")
        cbo_Reason_For_Note.Items.Add("06-Finalization of Provisional Assessment")
        cbo_Reason_For_Note.Items.Add("07-Others")

        cbo_Unregister_Type.Items.Clear()
        cbo_Unregister_Type.Items.Add("B2CL")
        cbo_Unregister_Type.Items.Add("Exports with Payment")
        cbo_Unregister_Type.Items.Add("Exports without Payment")

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1158-" Then
            btn_SaveAll.Visible = True
        End If


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        cbo_InvoiceSufixNo.Items.Clear()
        cbo_InvoiceSufixNo.Items.Add("")
        cbo_InvoiceSufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_InvoiceSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_InvoiceSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))
        cbo_InvoiceSufixNo.Items.Add("/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)))



        AddHandler txt_EntryPrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntryAcName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_BillDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_BillDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ItemParticulars.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Hsn_Sac_Code.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GstPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Quantity.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscountPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscountAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.Enter, AddressOf ControlGotFocus


        AddHandler btn_Print_Estimate.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_GST.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Delete.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Pdf.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tds_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Reason_For_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unregister_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OnAc_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tcs_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TcsPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TCS_TaxableValue.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IR_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EWayBillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_InvoiceSufixNo.Enter, AddressOf ControlGotFocus
        AddHandler txt_TDS_Value.GotFocus, AddressOf ControlGotFocus

        AddHandler Cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EntryPrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntryAcName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_BillDate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_BillDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ItemParticulars.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Hsn_Sac_Code.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GstPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Quantity.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscountPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscountAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tcs_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TcsPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TCS_TaxableValue.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_Print_Estimate.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_GST.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_InvoiceSufixNo.Leave, AddressOf ControlLostFocus

        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Delete.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Pdf.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tds_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Reason_For_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unregister_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OnAc_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IR_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EWayBillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.Leave, AddressOf ControlLostFocus
        AddHandler txt_TDS_Value.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Tcs_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TcsPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TCS_TaxableValue.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_BillNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Hsn_Sac_Code.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GstPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Quantity.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscountPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Tds_Percentage.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Hsn_Sac_Code.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GstPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Quantity.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscountPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TcsPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TCS_TaxableValue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tcs_Name.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Tds_Percentage.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Other_GST_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Other_GST_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_GSTTax_Details.Visible = True Then
                    btn_Close_GSTTax_Details_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.General_entry, New_Entry, Me, con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", NewCode, "Other_GST_Entry_Reference_Date", "(Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub
        If DeleteAll_STS <> True Then

            If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

            If New_Entry = True Then
                MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Other_GST_Entry_head", "Other_GST_Entry_Reference_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Other_GST_Entry_Reference_Code, Company_IdNo, for_OrderBy", tr)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Other_GST_Entry_Details", "Other_GST_Entry_Reference_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Item_Particulars,Unit_IdNo,Hsn_Sac_Code,Gst_Perc,Quantity,Rate,Amount,Discount_Perc,Discount_Amount,Total_Amount,Footer_Cash_Discount_Perc,Footer_Cash_Discount_Amount,Taxable_Value", "Sl_No", "Other_GST_Entry_Reference_Code, For_OrderBy, Company_IdNo, Other_GST_Entry_No, Other_GST_Entry_Date, Ledger_Idno", tr)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_TDS) & Trim(NewCode), tr)

            cmd.CommandText = "Delete from Other_GST_Entry_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Other_GST_Entry_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()
            If DeleteAll_STS <> True Then

                new_record()

                MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            tr.Rollback()

            Timer1.Enabled = False
            DeleteAll_STS = False

            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            tr.Dispose()
            cmd.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.ledger_idno = 0 or b.AccountsGroup_IdNo = 10or b.AccountsGroup_IdNo = 14 or b.AccountsGroup_IdNo = 6) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Item_Particulars from Other_GST_Entry_Details order by Item_Particulars", con)
            da.Fill(dt2)
            cbo_Filter_ItemName.DataSource = dt2
            cbo_Filter_ItemName.DisplayMember = "Item_Particulars"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try


            da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Other_GST_Entry_No, Other_GST_Entry_RefNo", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_RefNo from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Other_GST_Entry_RefNo", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Trim(movno) <> "" Then move_record(movno)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_EntryNo.Text))


            da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_No from Other_GST_Entry_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Other_GST_Entry_No, Other_GST_Entry_RefNo", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_RefNo from Other_GST_Entry_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Other_GST_Entry_RefNo", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_Reference_No from Other_GST_Entry_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Other_GST_Entry_Reference_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_EntryNo.Text))



            da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_No from Other_GST_Entry_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Other_GST_Entry_No desc, Other_GST_Entry_RefNo desc", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_RefNo from Other_GST_Entry_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Other_GST_Entry_RefNo desc", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_Reference_No from Other_GST_Entry_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Other_GST_Entry_Reference_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

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



            da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Other_GST_Entry_No desc, Other_GST_Entry_RefNo desc", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_RefNo from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Other_GST_Entry_RefNo desc", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Other_GST_Entry_Reference_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Other_GST_Entry_RefNo desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try

            clear()

            New_Entry = True

            'lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "ForOrderBy_ReferenceCode", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            'lbl_RefNo.ForeColor = Color.Red

            If Common_Procedures.settings.Cloth_Yarn_General_Sales_Invoice_ContinousNo_Status = 1 Then
                If Pk_Condition = "GSSAL-" Then
                    lbl_EntryNo.Text = Common_Procedures.get_CloYarn_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
                Else

                    lbl_EntryNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
                End If
            ElseIf Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then

                If Pk_Condition = "GSCRN-" Then
                    lbl_EntryNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "CRNT")
                ElseIf Pk_Condition = "GSDBN-" Then
                    lbl_EntryNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "DRNT")
                Else

                    lbl_EntryNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
                End If
            Else
                lbl_EntryNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            End If


            lbl_EntryNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select top 1 a.*, b.ledger_name as GST_EntryAc_Name from Other_GST_Entry_Head a LEFT OUTER JOIN Ledger_Head b ON a.Other_GST_Entry_Ac_IdNo = b.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Other_GST_Entry_Reference_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by a.for_Orderby desc, a.Other_GST_Entry_Reference_No desc", con)
            dt2 = New DataTable
            da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                If dt2.Rows(0).Item("GST_EntryAc_Name").ToString <> "" Then cbo_EntryAcName.Text = dt2.Rows(0).Item("GST_EntryAc_Name").ToString
                If dt2.Rows(0).Item("Other_GST_Entry_PrefixNo").ToString <> "" Then txt_EntryPrefixNo.Text = dt2.Rows(0).Item("Other_GST_Entry_PrefixNo").ToString

                If IsDBNull(dt2.Rows(0).Item("Invoice_SuffixNo").ToString) = False Then
                    If dt2.Rows(0).Item("Invoice_SuffixNo").ToString <> "" Then cbo_InvoiceSufixNo.Text = dt2.Rows(0).Item("Invoice_SuffixNo").ToString
                End If

                If IsDBNull(dt2.Rows(0).Item("Tcs_Tax_Status").ToString) = False Then


                    If Val(dt2.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False

                End If

                If IsDBNull(dt2.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt2.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If

                If IsDBNull(dt2.Rows(0).Item("TDS_Tax_Status").ToString) = False Then
                    If Val(dt2.Rows(0).Item("TDS_Tax_Status").ToString) = 1 Then
                        chk_TDS_Tax.Checked = True

                        If dt2.Rows(0).Item("TDS_Percentage").ToString <> "" Then txt_Tds_Percentage.Text = dt2.Rows(0).Item("TDS_Percentage").ToString

                    Else
                        chk_TDS_Tax.Checked = False
                        txt_Tds_Percentage.Text = ""
                    End If
                End If

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            dt2.Dispose()
            da.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim inpno As String = ""
        Dim vCSMovNo As String = ""
        Dim vCSInvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""
        Dim vCode As String = ""
        Dim vJWmovCode As String = ""
        Dim vJWmovNo As String = ""
        Dim vMovNo As String = ""
        Try

            inpno = InputBox("Enter " & lbl_RefNo_Caption.Text, "FOR FINDING...")


            Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Other_GST_Entry_No = '" & Trim(inpno) & "' and " & Other_Condition, con)
            'Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_RefNo from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Other_GST_Entry_No = '" & Trim(inpno) & "' and " & Other_Condition, con)
            'Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_Reference_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Other_GST_Entry_No = '" & Trim(inpno) & "' and " & Other_Condition, con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            vCSMovNo = ""
            If Common_Procedures.settings.Cloth_Yarn_General_Sales_Invoice_ContinousNo_Status = 1 Then

                vCSInvCode = "GCINV-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select ClothSales_Invoice_RefNo from ClothSales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Invoice_Code = '" & Trim(vCSInvCode) & "' and ClothSales_Invoice_Code LIKE 'GCINV-%' ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                vCSMovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vCSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()


                vYSInvCode = "GYNSL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_No from Yarn_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(vYSInvCode) & "' and Yarn_Sales_Code LIKE 'GYNSL-%' ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                vYSMovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vYSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()


                vJWmovCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(vJWmovCode) & "'  ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                vJWmovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vJWmovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()
                If Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then
                    If (Pk_Condition = "GSCRN-") Then
                        vCode = "GCLSR-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                        Da = New SqlClient.SqlDataAdapter("select ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(vCode) & "'", con)
                        Dt = New DataTable
                        Da.Fill(Dt)

                    ElseIf (Pk_Condition = "GSDBN-") Then
                        vCode = "GYPRT-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                        Da = New SqlClient.SqlDataAdapter("select Yarn_Purchase_Return_No from Yarn_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code = '" & Trim(vCode) & "'", con)
                        Dt = New DataTable
                        Da.Fill(Dt)
                    End If
                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            vMovNo = Trim(Dt.Rows(0)(0).ToString)
                            '  MessageBox.Show("Already Enterd ", "DOES NOT OPEN INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)

                        End If
                        Dt.Clear()
                    End If
                End If


            End If

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vCSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Cloth Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            ElseIf Val(vMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Return", "DOES NOT FIND....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            ElseIf Val(vYSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Yarn Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vJWmovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Jobwork Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show("Invoice No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim vCSInvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""
        Dim vCSMovNo As String = ""
        Dim CInvCode As String = ""
        Dim ClMovNo As String = ""
        Dim vJWmovCode As String = ""
        Dim vJWmovNo As String = ""
        Dim vMovNo As String = ""
        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.General_entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New " & lbl_RefNo_Caption.Text, "FOR NEW NUMBER INSERTION...")



            Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Other_GST_Entry_No = '" & Trim(inpno) & "' and " & Other_Condition, con)
            '  Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_Reference_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Other_GST_Entry_No = '" & Trim(inpno) & "' and " & Other_Condition, con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            vCSMovNo = ""
            If Common_Procedures.settings.Cloth_Yarn_General_Sales_Invoice_ContinousNo_Status = 1 Then

                vCSInvCode = "GCINV-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select ClothSales_Invoice_RefNo from ClothSales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Invoice_Code = '" & Trim(vCSInvCode) & "' and ClothSales_Invoice_Code LIKE 'GCINV-%' ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                vCSMovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vCSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()


                vYSInvCode = "GYNSL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_No from Yarn_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(vYSInvCode) & "' and Yarn_Sales_Code LIKE 'GYNSL-%' ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                vYSMovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vYSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

                vJWmovCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(vJWmovCode) & "'  ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                vJWmovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vJWmovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()
            ElseIf Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then
                If (Pk_Condition = "GSCRN-") Then
                    CInvCode = "GCLSR-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                    Da = New SqlClient.SqlDataAdapter("select ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code ='" & Trim(CInvCode) & "' ", con)
                    Dt = New DataTable
                    Da.Fill(Dt)
                ElseIf (Pk_Condition = "GSDBN-") Then

                    CInvCode = "GYPRT-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                    Da = New SqlClient.SqlDataAdapter("select Yarn_Purchase_Return_No from Yarn_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code = '" & Trim(CInvCode) & "' and Entry_VAT_GST_Type = 'GST' ", con)
                    Dt = New DataTable
                    Da.Fill(Dt)

                End If
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vMovNo = Trim(Dt.Rows(0)(0).ToString)
                        '  MessageBox.Show("Already Enterd ", "DOES NOT OPEN INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)

                    End If
                    Dt.Clear()
                End If
            End If



            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vCSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Cloth Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            ElseIf Val(vYSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Yarn Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vJWmovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Jobwork Invoice", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vMovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid " & lbl_RefNo_Caption.Text, "DOES NOT INSERT NEW NUMBER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Else
                    new_record()
                    Insert_Entry = True
                    lbl_EntryNo.Text = Trim(UCase(inpno))

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

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim vEnt_Ac_IdNo As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_ID As Integer = 0
        Dim vEntRefNo As String = ""
        Dim vRefCd_forOrdby As String = ""
        Dim vforOrdby As String = 0
        Dim vTot_DetQty As String = 0
        Dim vTot_DetAmt As String = 0
        Dim vTot_DetDiscAmt As String = 0
        Dim vTot_DetTtAmt As String = 0
        Dim vTot_DetFtrDiscAmt As String = 0
        Dim vTot_DetTxblAmt As String = 0
        Dim vBillDt As String = ""
        Dim vTxAmt_RndOff_STS As Integer = 0
        Dim vGSTPerc As String = 0
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
        Dim vVouBil As String = ""
        Dim vVouType As String = ""
        Dim vVou_CRDR_BillType As String = ""
        Dim vPBillNo As String = ""
        Dim Led_GSTTIN As String = ""
        Dim Ent_Bill_Amt As String = ""
        Dim NtAmt As String = 0
        Dim Nr As Integer = 0
        Dim vOnAc_ID As Integer = 0
        Dim vVouPos_LedID As Integer = 0

        Dim da, da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As DataTable
        Dim vEInvAckDate As String = ""
        Dim vOrdByNo As String = ""
        Dim vTCS_AssVal_EditSTS As Integer = 0
        Dim vTCS_Tax_Sts As Integer = 0
        Dim vTCSAmtRndOff_STS As Integer = 0
        Dim vDelvTo_IdNo As Integer = 0
        Dim Agt_Idno As Integer = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        Dim Trans_ID As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Val(Common_Procedures.User.IdNo) = 0 Then
            MessageBox.Show("Invalid User Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Cloth_Invoice_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.General_entry, New_Entry, Me, con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", NewCode, "Other_GST_Entry_Reference_Date", "(Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Other_GST_Entry_Reference_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        vEnt_Ac_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_EntryAcName.Text)
        If vEnt_Ac_IdNo = 0 And Val(CDbl(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid " & lbl_CreditNoteAc_Caption.Text, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EntryAcName.Enabled Then cbo_EntryAcName.Focus()
            Exit Sub
        End If

        vOnAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_OnAc_Type.Text)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Transport.Text)
        vDelvTo_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        If Trim(txt_BillNo.Text) <> "" Then
            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da = New SqlClient.SqlDataAdapter("select * from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " and  Bill_No = '" & Trim(txt_BillNo.Text) & "' and Other_GST_Entry_Reference_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Other_GST_Entry_Reference_Code <> '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Bill No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
                Exit Sub
            End If
            dt1.Clear()
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Item Particulars", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(7).Value) = 0 Then
                        MessageBox.Show("Invalid Amount", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(7)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        NoCalc_Status = False
        TotalAmount_Calculation()

        vTot_DetQty = 0
        vTot_DetAmt = 0
        vTot_DetDiscAmt = 0
        vTot_DetTtAmt = 0
        vTot_DetFtrDiscAmt = 0
        vTot_DetTxblAmt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTot_DetQty = Val(dgv_Details_Total.Rows(0).Cells(5).Value)
            vTot_DetAmt = Val(dgv_Details_Total.Rows(0).Cells(7).Value)
            vTot_DetDiscAmt = Val(dgv_Details_Total.Rows(0).Cells(9).Value)
            vTot_DetTtAmt = Val(dgv_Details_Total.Rows(0).Cells(10).Value)
            vTot_DetFtrDiscAmt = Val(dgv_Details_Total.Rows(0).Cells(12).Value)
            vTot_DetTxblAmt = Val(dgv_Details_Total.Rows(0).Cells(13).Value)
        End If

        vBillDt = ""
        If Not Trim(msk_BillDate.Text) = "-  -" Then
            If IsDate(msk_BillDate.Text) Then
                vBillDt = Trim(msk_BillDate.Text)
            End If
        End If

        vTxAmt_RndOff_STS = 0
        If chk_Tax_FullRoundOff_Status.Checked = True Then vTxAmt_RndOff_STS = 1
        vTCS_Tax_Sts = 0
        If chk_TCS_Tax.Checked = True Then vTCS_Tax_Sts = 1
        vTCS_AssVal_EditSTS = 0
        If txt_TCS_TaxableValue.Enabled = True Then vTCS_AssVal_EditSTS = 1
        vTCSAmtRndOff_STS = 0
        If chk_TCSAmount_RoundOff_STS.Checked = True Then vTCSAmtRndOff_STS = 1
        Dim vTDS_Tax_Sts = 0
        If chk_TDS_Tax.Checked = True Then vTDS_Tax_Sts = 1

        If Val(lbl_NetAmount.Text) = 0 Then lbl_NetAmount.Text = "0"
        If Val(lbl_BillAmount.Text) = 0 Then lbl_BillAmount.Text = "0"

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@EntryDate", CDate(msk_Date.Text))


        Dim ms As New MemoryStream()
        If IsNothing(pic_IRN_QRCode_Image.BackgroundImage) = False Then
            Dim bitmp As New Bitmap(pic_IRN_QRCode_Image.BackgroundImage)
            bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
        End If
        Dim data As Byte() = ms.GetBuffer()
        Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
        p.Value = data
        cmd.Parameters.Add(p)
        ms.Dispose()

        vEInvAckDate = ""
        If Trim(txt_eInvoiceAckDate.Text) <> "" Then
            If IsDate(txt_eInvoiceAckDate.Text) = True Then
                If Year(CDate(txt_eInvoiceAckDate.Text)) <> 1900 Then
                    vEInvAckDate = Trim(txt_eInvoiceAckDate.Text)
                End If

            End If
        End If
        If Trim(vEInvAckDate) <> "" Then
            cmd.Parameters.AddWithValue("@EInvoiceAckDate", Convert.ToDateTime(vEInvAckDate))
        End If


        Dim eiCancel As String = "0"
        If txt_eInvoice_CancelStatus.Text = "Cancelled" Then
            eiCancel = "1"
        End If

        Dim EWBCancel As String = "0"
        If txt_EWB_Cancel_Status.Text = "Cancelled" Then
            EWBCancel = "1"
        End If

        tr = con.BeginTransaction

        Try


            If New_Entry = False Then
                '---Nothing

            Else
                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "ForOrderBy_ReferenceCode", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            End If


            If Insert_Entry = True Or New_Entry = False Then
                '---Nothing

            Else
                If Common_Procedures.settings.Cloth_Yarn_General_Sales_Invoice_ContinousNo_Status = 1 Then
                    If Pk_Condition = "GSSAL-" Then
                        lbl_EntryNo.Text = Common_Procedures.get_CloYarn_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                        'ElseIf Pk_Condition = "GSCRN-" Then
                        '    lbl_EntryNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "CRNT", tr)
                        'ElseIf Pk_Condition = "GSDBN-" Then
                        '    lbl_EntryNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "DRNT", tr)

                        '    lbl_RefNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                        ' lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "ForOrderBy_ReferenceCode", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                    Else
                        lbl_EntryNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                    End If
                ElseIf Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then
                    If Pk_Condition = "GSCRN-" Then
                        lbl_EntryNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "CRNT", tr)
                    ElseIf Pk_Condition = "GSDBN-" Then
                        lbl_EntryNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "DRNT", tr)
                    Else
                        lbl_EntryNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                    End If
                Else
                    lbl_EntryNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                End If
                'lbl_EntryNo.Text = Common_Procedures.get_MaxCode(con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            End If


            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr



            If Trim(vBillDt) <> "" Then
                cmd.Parameters.AddWithValue("@BillDate", CDate(vBillDt))
            End If

            vRefCd_forOrdby = Val(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text)))
            vforOrdby = Val(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_EntryNo.Text)))

            vEntRefNo = Trim(UCase(txt_EntryPrefixNo.Text)) & Trim(UCase(lbl_EntryNo.Text)) & Trim(cbo_InvoiceSufixNo.Text)

            Dim vCREATED_DTTM_TXT As String = ""
            Dim vMODIFIED_DTTM_TXT As String = ""

            vCREATED_DTTM_TXT = ""
            vMODIFIED_DTTM_TXT = ""

            vCREATED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@createddatetime", Now)


            vMODIFIED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@modifieddatetime", Now)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Other_GST_Entry_Head (  Other_GST_Entry_Reference_Code  ,                 Company_IdNo     ,      Other_GST_Entry_Reference_No ,   ForOrderBy_ReferenceCode       ,  Other_GST_Entry_Type     ,         Other_GST_Entry_PrefixNo       ,        Other_GST_Entry_No       ,   Other_GST_Entry_RefNo   ,           for_OrderBy      , Other_GST_Entry_Date,       Ledger_IdNo       ,               Bill_No          ,                                Bill_Date                 ,   Other_GST_Entry_Ac_IdNo     ,                  Gross_Amount          ,           CashDiscount_Perc            ,              CashDiscount_Amount         ,                 Taxable_Value          ,                  CGST_Amount          ,              SGST_Amount             ,                 IGST_AMount          ,  Chess_Amount,             Round_Off_Amount       ,                  Net_Amount                ,     TaxAmount_RoundOff_Status      ,         Total_Quantity       ,       Total_Sub_Amount       ,        Total_DiscountAmount       ,           Total_Amount         ,     Total_Footer_Cash_Discount_Amount,        Total_Taxable_Value      ,                 Remarks           ,                               User_Idno    , Tds_Percentage                   , Tds_Amount                           ,Bill_Amount                            ,Reason_For_Issuing_Note                 ,Unregister_Type                         ,  Add_Less                    ,         OnAccount_IdNo     , Vehicle_no, Tcs_Name_caption           ,              Tcs_percentage       ,                    Tcs_Amount    ,                     TCS_Taxable_Value,                            EDIT_TCS_TaxableValue ,             Tcs_Tax_Status,             TCSAmount_RoundOff_Status,                         Invoice_Value_Before_TCS ,                            RoundOff_Invoice_Value_Before_TCS                           ,           E_Invoice_IRNO  ,           E_Invoice_QR_Image  ,                 Eway_BillNo              ,               Transport_IdNo ,    DeliveryTo_IdNo                 ,               Invoice_SuffixNo            ,       Agent_idno                      , TDS_Tax_Status                   ,        TDS_Taxable_Value                             ,   created_useridno           ,   created_DateTime,          created_DateTime_Text    , Last_modified_useridno, Last_modified_DateTime, Last_modified_DateTime_Text  ) " &
                                "          Values               (  '" & Trim(NewCode) & "'         , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'    , " & Str(Val(vRefCd_forOrdby)) & ", '" & Trim(vEntryType) & "',  '" & Trim(txt_EntryPrefixNo.Text) & "', '" & Trim(lbl_EntryNo.Text) & "',  '" & Trim(vEntRefNo) & "', " & Str(Val(vforOrdby)) & ",     @EntryDate      , " & Str(Val(Led_ID)) & ", '" & Trim(txt_BillNo.Text) & "',  " & IIf(IsDate(vBillDt) = True, "@BillDate", "Null") & ", " & Str(Val(vEnt_Ac_IdNo)) & ",  " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(lbl_CashDiscAmount.Text)) & ", " & Str(Val(lbl_TaxableValue.Text)) & ",  " & Str(Val(lbl_CGstAmount.Text)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", " & Str(Val(lbl_IGstAmount.Text)) & ",       0      , " & Str(Val(lbl_RoundOff.Text)) & ",  " & Str(Val(CDbl(lbl_NetAmount.Text))) & ", " & Str(Val(vTxAmt_RndOff_STS)) & ", " & Str(Val(vTot_DetQty)) & ", " & Str(Val(vTot_DetAmt)) & ",  " & Str(Val(vTot_DetDiscAmt)) & ", " & Str(Val(vTot_DetTtAmt)) & ", " & Str(Val(vTot_DetFtrDiscAmt)) & ", " & Str(Val(vTot_DetTxblAmt)) & ", '" & Trim(txt_Remarks.Text) & "', " & Str(Val(Common_Procedures.User.IdNo)) & ", " & Str(Val(txt_Tds_Percentage.Text)) & "   ," & Str(Val(lbl_Tds_Amount.Text)) & " ," & Str(Val(lbl_BillAmount.Text)) & "  ,'" & Trim(cbo_Reason_For_Note.Text) & "','" & Trim(cbo_Unregister_Type.Text) & "', " & Val(txt_AddLess.Text) & ", " & Str(Val(vOnAc_ID)) & " ,'" & Trim(Cbo_VehicleNo.Text) & "', '" & Trim(txt_Tcs_Name.Text) & "',       " & Str(Val(txt_TcsPerc.Text)) & ",    " & Str(Val(lbl_TcsAmount.Text)) & " ,  " & Str(Val(txt_TCS_TaxableValue.Text)) & ", " & Str(Val(vTCS_AssVal_EditSTS)) & ", " & Str(Val(vTCS_Tax_Sts)) & ", " & Str(Val(vTCSAmtRndOff_STS)) & ", " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & " ,  '" & Trim(txt_IR_No.Text) & "' ,     @QrCode          , '" & Trim(txt_EWayBillNo.Text) & "'  ,    " & Str(Val(Trans_ID)) & " , " & Str(Val(vDelvTo_IdNo)) & " , '" & Trim(cbo_InvoiceSufixNo.Text) & "' , " & Str(Val(Agt_Idno)) & "   , " & Str(Val(vTDS_Tax_Sts)) & "  ," & Str(Val(txt_TDS_Value.Text)) & " ,       " & Str(Val(Common_Procedures.User.IdNo)) & ",  @createddatetime ,  '" & Trim(vCREATED_DTTM_TXT) & "',              0        ,     NUll              ,          ''       ) "
                cmd.ExecuteNonQuery()

            Else


                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Other_GST_Entry_head", "Other_GST_Entry_Reference_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Other_GST_Entry_Reference_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Other_GST_Entry_Details", "Other_GST_Entry_Reference_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_Particulars,Unit_IdNo,Hsn_Sac_Code,Gst_Perc,Quantity,Rate,Amount,Discount_Perc,Discount_Amount,Total_Amount,Footer_Cash_Discount_Perc,Footer_Cash_Discount_Amount,Taxable_Value", "Sl_No", "Other_GST_Entry_Reference_Code, For_OrderBy, Company_IdNo, Other_GST_Entry_No, Other_GST_Entry_Date, Ledger_Idno", tr)


                Nr = 0
                cmd.CommandText = "Update Other_GST_Entry_Head set  Other_GST_Entry_PrefixNo = '" & Trim(txt_EntryPrefixNo.Text) & "', Other_GST_Entry_RefNo = '" & Trim(vEntRefNo) & "',  Other_GST_Entry_Date = @EntryDate ,  Ledger_IdNo = " & Str(Val(Led_ID)) & " ,  Bill_No  = '" & Trim(txt_BillNo.Text) & "',  Bill_Date = " & IIf(IsDate(vBillDt) = True, "@BillDate", "Null") & " ,   Other_GST_Entry_Ac_IdNo   = " & Str(Val(vEnt_Ac_IdNo)) & " ,  Gross_Amount   =  " & Str(Val(lbl_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(lbl_CashDiscAmount.Text)) & " ,  Taxable_Value = " & Str(Val(lbl_TaxableValue.Text)) & ",  CGST_Amount = " & Str(Val(lbl_CGstAmount.Text)) & " ,  SGST_Amount = " & Str(Val(lbl_SGstAmount.Text)) & " ,  IGST_AMount =  " & Str(Val(lbl_IGstAmount.Text)) & " ,  Chess_Amount = 0, Round_Off_Amount  =  " & Str(Val(lbl_RoundOff.Text)) & " ,  Net_Amount = " & Str(Val(CDbl(lbl_NetAmount.Text))) & ", TaxAmount_RoundOff_Status  = " & Str(Val(vTxAmt_RndOff_STS)) & "  ,  Total_Quantity = " & Str(Val(vTot_DetQty)) & "  ,  Total_Sub_Amount  = " & Str(Val(vTot_DetAmt)) & " , Total_DiscountAmount  =  " & Str(Val(vTot_DetDiscAmt)) & " ,  Total_Amount  = " & Str(Val(vTot_DetTtAmt)) & ",  Total_Footer_Cash_Discount_Amount = " & Str(Val(vTot_DetFtrDiscAmt)) & ",  Total_Taxable_Value = " & Str(Val(vTot_DetTxblAmt)) & " , Remarks = '" & Trim(txt_Remarks.Text) & "' ,  User_Idno = " & Str(Val(Common_Procedures.User.IdNo)) & " ,Vehicle_no='" & Trim(Cbo_VehicleNo.Text) & "', Tds_Percentage =" & Val(txt_Tds_Percentage.Text) & " , Tds_Amount = " & Val(lbl_Tds_Amount.Text) & "  ,Bill_Amount= " & Val(lbl_BillAmount.Text) & " ,Reason_For_Issuing_Note = '" & Trim(cbo_Reason_For_Note.Text) & "',Unregister_Type='" & Trim(cbo_Unregister_Type.Text) & "', Add_Less = " & Val(txt_AddLess.Text) & "  , OnAccount_IdNo = " & Str(Val(vOnAc_ID)) & " ,  Tcs_Name_caption = '" & Trim(txt_Tcs_Name.Text) & "', Tcs_percentage=" & Str(Val(txt_TcsPerc.Text)) & ",Tcs_Amount= " & Str(Val(lbl_TcsAmount.Text)) & " , TCS_Taxable_Value = " & Str(Val(txt_TCS_TaxableValue.Text)) & ", EDIT_TCS_TaxableValue = " & Str(Val(vTCS_AssVal_EditSTS)) & " , Tcs_Tax_Status = " & Str(Val(vTCS_Tax_Sts)) & " , TCSAmount_RoundOff_Status = " & Str(Val(vTCSAmtRndOff_STS)) & " , Invoice_Value_Before_TCS = " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", RoundOff_Invoice_Value_Before_TCS = " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & "  ,  E_Invoice_IRNO = '" & Trim(txt_IR_No.Text) & "' , E_Invoice_QR_Image =  @QrCode  , E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & "   , E_Invoice_Cancelled_Status = " & eiCancel.ToString & "  ,  Eway_BillNo = '" & Trim(txt_EWayBillNo.Text) & "' , Transport_IdNo = " & Str(Val(Trans_ID)) & " ,  EWB_No = '" & txt_eWayBill_No.Text & "',EWB_Date = '" & txt_EWB_Date.Text & "',EWB_Valid_Upto = '" & txt_EWB_ValidUpto.Text & "' , EWB_Cancelled = " & EWBCancel.ToString & " , EWBCancellation_Reason = '" & txt_EWB_Canellation_Reason.Text & "', DeliveryTo_IdNo =" & Str(Val(vDelvTo_IdNo)) & " , Invoice_SuffixNo = '" & Trim(cbo_InvoiceSufixNo.Text) & "' ,  Agent_idno = " & Str(Val(Agt_Idno)) & " , TDS_Tax_Status = " & Str(Val(vTDS_Tax_Sts)) & " ,TDS_Taxable_Value= " & Str(Val(txt_TDS_Value.Text)) & " , Last_modified_useridno = " & Str(Val(Common_Procedures.User.IdNo)) & ", Last_modified_DateTime = @modifieddatetime, Last_modified_DateTime_Text = '" & Trim(vMODIFIED_DTTM_TXT) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
                Nr = cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Other_GST_Entry_head", "Other_GST_Entry_Reference_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Other_GST_Entry_Reference_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from Other_GST_Entry_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                For i = 0 To .Rows.Count - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        unt_id = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Other_GST_Entry_Details (  Other_GST_Entry_Reference_Code ,                 Company_IdNo     ,   Other_GST_Entry_Reference_No  ,   ForOrderBy_ReferenceCode       ,  Other_GST_Entry_Type     ,         Other_GST_Entry_PrefixNo       ,        Other_GST_Entry_No       ,   Other_GST_Entry_RefNo   ,           for_OrderBy      , Other_GST_Entry_Date,       Ledger_IdNo       ,             Sl_No     ,              Item_Particulars          ,          Unit_IdNo      ,                    Hsn_Sac_Code         ,                      Gst_Perc            ,                      Quantity            ,                      Rate                ,                       Amount             ,                      Discount_Perc       ,                      Discount_Amount      ,                      Total_Amount          ,         Footer_Cash_Discount_Perc          ,        Footer_Cash_Discount_Amount        ,                      Taxable_Value         ) " &
                                            "          Values                  (     '" & Trim(NewCode) & "'     , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'  , " & Str(Val(vRefCd_forOrdby)) & ", '" & Trim(vEntryType) & "',  '" & Trim(txt_EntryPrefixNo.Text) & "', '" & Trim(lbl_EntryNo.Text) & "',  '" & Trim(vEntRefNo) & "', " & Str(Val(vforOrdby)) & ",     @EntryDate      , " & Str(Val(Led_ID)) & ",  " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(unt_id)) & ", '" & Trim(.Rows(i).Cells(3).Value) & "' , " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & " , " & Str(Val(.Rows(i).Cells(10).Value)) & " , " & Str(Val(.Rows(i).Cells(11).Value)) & " , " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next i
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Other_GST_Entry_Details", "Other_GST_Entry_Reference_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_Particulars,Unit_IdNo,Hsn_Sac_Code,Gst_Perc,Quantity,Rate,Amount,Discount_Perc,Discount_Amount,Total_Amount,Footer_Cash_Discount_Perc,Footer_Cash_Discount_Amount,Taxable_Value", "Sl_No", "Other_GST_Entry_Reference_Code, For_OrderBy, Company_IdNo, Other_GST_Entry_No, Other_GST_Entry_Date, Ledger_Idno", tr)

            End With


            '---Tax Details
            cmd.CommandText = "Delete from Other_GST_Entry_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_GSTTax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        vGSTPerc = 0
                        If Val(.Rows(i).Cells(8).Value) <> 0 Then
                            vGSTPerc = Val(.Rows(i).Cells(7).Value)
                        Else
                            vGSTPerc = Format(Val(.Rows(i).Cells(3).Value) + Val(.Rows(i).Cells(5).Value), "########0.00")
                        End If

                        cmd.CommandText = "Insert into Other_GST_Entry_Tax_Details  ( Other_GST_Entry_Reference_Code ,                 Company_IdNo    ,   Other_GST_Entry_Reference_No  ,   ForOrderBy_ReferenceCode       ,  Other_GST_Entry_Type     ,         Other_GST_Entry_PrefixNo       ,        Other_GST_Entry_No       ,   Other_GST_Entry_RefNo   ,           for_OrderBy      , Other_GST_Entry_Date,       Ledger_IdNo       ,              Sl_No    ,                    HSN_SAC_Code        ,                      Taxable_Amount      ,         GST_Percentage    ,                      CGST_Percentage     ,                      CGST_Amount         ,                      SGST_Percentage      ,                      SGST_Amount         ,                      IGST_Percentage     ,                      IGST_Amount          ) " &
                                            "          Values                       (   '" & Trim(NewCode) & "'     , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'  , " & Str(Val(vRefCd_forOrdby)) & ", '" & Trim(vEntryType) & "',  '" & Trim(txt_EntryPrefixNo.Text) & "', '" & Trim(lbl_EntryNo.Text) & "',  '" & Trim(vEntRefNo) & "', " & Str(Val(vforOrdby)) & ",     @EntryDate      , " & Str(Val(Led_ID)) & ",  " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(vGSTPerc)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            Ac_ID = Led_ID

            '-----A/c Posting

            '-----Getting GST account details
            cmd.CommandText = "truncate table Entry_GST_Tax_Details_Temp"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "insert into Entry_GST_Tax_Details_Temp(GST_Percentage, CGST_Amount, SGST_Amount, IGST_Amount) select (CGST_Percentage + SGST_Percentage + IGST_Percentage), sum(CGST_Amount), sum(SGST_Amount), sum(IGST_Amount) from Other_GST_Entry_Tax_Details where Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' and (CGST_Percentage <> 0 or SGST_Percentage <> 0 or IGST_Percentage <> 0) and (CGST_Amount <> 0 or SGST_Amount <> 0 or IGST_Amount <> 0) Group by CGST_Percentage,  SGST_Percentage,  IGST_Percentage Having sum(CGST_Amount) <> 0 or sum(SGST_Amount) <> 0 or sum(IGST_Amount) <> 0"
            cmd.ExecuteNonQuery()




            If vOnAc_ID <> 0 Then
                vVouPos_LedID = vOnAc_ID
            Else
                vVouPos_LedID = Led_ID
            End If

            vVouType = ""
            vPBillNo = ""
            vVou_CRDR_BillType = ""

            NtAmt = Val(CDbl(lbl_NetAmount.Text))
            If Trim(UCase(vEntryType)) = "PURC" Then
                vVouType = "Gen.Gst.Purc"
                vPBillNo = "Bill No : " & Trim(txt_BillNo.Text)
                vVou_CRDR_BillType = "CR"


            ElseIf Trim(UCase(vEntryType)) = "SALE" Then
                vVouType = "Gen.Gst.Sale"
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
                    vPBillNo = Trim(vEntRefNo)
                Else
                    vPBillNo = "Invoice No : " & Trim(vEntRefNo)
                End If


                vVou_CRDR_BillType = "DR"

            ElseIf Trim(UCase(vEntryType)) = "CRNT" Then    ' Sales return
                vVouType = "Gst.CrNt"
                vPBillNo = "Bill No : " & Trim(txt_BillNo.Text)
                vVou_CRDR_BillType = "CR"

            ElseIf Trim(UCase(vEntryType)) = "DRNT" Then  ' Purchase Return
                vVouType = "Gst.DbNt"
                vPBillNo = "Bill No : " & Trim(txt_BillNo.Text)
                vVou_CRDR_BillType = "DR"

            ElseIf Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then
                vVouType = "Gen.JbWrk.Inv"
                vPBillNo = "Invoice No : " & Trim(vEntRefNo)

                vVou_CRDR_BillType = "DR"

            ElseIf Trim(UCase(vEntryType)) = "ADV.PYMT" Then
                vVouType = "Gst.Adv.Pymt"
                vPBillNo = "ADVANCE"

                vVou_CRDR_BillType = "DR"

            End If


            Led_GSTTIN = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_GSTinNo", "(Ledger_IdNo = " & Str(Val(vVouPos_LedID)) & ")", 0, tr)

            Dim vTaxVal As String = ""
            Dim vVOUPOS_GSTAC_IDNOS As String = "", vVOUPOS_GST_AMTS As String = ""

            vTaxVal = Format(Val(lbl_GrossAmount.Text) - Val(lbl_CashDiscAmount.Text), "##############0.00")

            If Trim(UCase(vVou_CRDR_BillType)) = "CR" Then

                Common_Procedures.get_GST_AC_IDNOS_for_AC_POSTING(con, "DR", vVOUPOS_GSTAC_IDNOS, vVOUPOS_GST_AMTS, tr)

                vLed_IdNos = vVouPos_LedID & "|" & vEnt_Ac_IdNo & "|" & Trim(vVOUPOS_GSTAC_IDNOS) & "|" & Common_Procedures.CommonLedger.TCS_RECEIVABLE_AC & "|" & Common_Procedures.CommonLedger.ADDLESS_AMOUNT_AC & "|" & Common_Procedures.CommonLedger.ROUNDOFF_AC
                vVou_Amts = Val(NtAmt) & "|" & -1 * (Val(vTaxVal)) & "|" & Trim(vVOUPOS_GST_AMTS) & "|" & -1 * Val(lbl_TcsAmount.Text) & "|" & -1 * Val(txt_AddLess.Text) & "|" & -1 * (Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text) + Val(lbl_RoundOff.Text))

                'vLed_IdNos = vVouPos_LedID & "|" & vEnt_Ac_IdNo & "|24|25|26|33|30"
                'vVou_Amts = Val(NtAmt) & "|" & -1 * (Val(vTaxVal)) & "|" & -1 * Val(lbl_CGstAmount.Text) & "|" & -1 * Val(lbl_SGstAmount.Text) & "|" & -1 * Val(lbl_IGstAmount.Text) & "|" & -1 * Val(lbl_TcsAmount.Text) & "|" & -1 * (Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text) + Val(lbl_RoundOff.Text))

            Else

                Common_Procedures.get_GST_AC_IDNOS_for_AC_POSTING(con, "CR", vVOUPOS_GSTAC_IDNOS, vVOUPOS_GST_AMTS, tr)

                vLed_IdNos = vVouPos_LedID & "|" & vEnt_Ac_IdNo & "|" & Trim(vVOUPOS_GSTAC_IDNOS) & "|" & Common_Procedures.CommonLedger.TCS_RECEIVABLE_AC & "|" & Common_Procedures.CommonLedger.ADDLESS_AMOUNT_AC & "|" & Common_Procedures.CommonLedger.ROUNDOFF_AC
                vVou_Amts = -1 * Val(NtAmt) & "|" & (Val(vTaxVal)) & "|" & Trim(vVOUPOS_GST_AMTS) & "|" & Val(lbl_TcsAmount.Text) & "|" & Val(txt_AddLess.Text) & "|" & (Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text) + Val(lbl_RoundOff.Text))

                'vLed_IdNos = vVouPos_LedID & "|" & vEnt_Ac_IdNo & "|24|25|26|32|30"
                'vVou_Amts = -1 * Val(NtAmt) & "|" & (Val(vTaxVal)) & "|" & Val(lbl_CGstAmount.Text) & "|" & Val(lbl_SGstAmount.Text) & "|" & Val(lbl_IGstAmount.Text) & "|" & Val(lbl_TcsAmount.Text) & "|" & (Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text) + Val(lbl_RoundOff.Text))

            End If


            If Common_Procedures.Voucher_Updation(con, vVouType, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_EntryNo.Text), Convert.ToDateTime(msk_Date.Text), vPBillNo, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_TDS) & Trim(NewCode), tr)
            If lbl_Tds_Amount.Visible = True Then
                vLed_IdNos = ""
                vVou_Amts = ""
                ErrMsg = ""

                vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Payable_Ac) & "|" & vVouPos_LedID
                vVou_Amts = Val(lbl_Tds_Amount.Text) & "|" & -1 * Val(lbl_Tds_Amount.Text)

                If Common_Procedures.Voucher_Updation(con, "Gen.Gst.Tds", Val(lbl_Company.Tag), Trim(PkCondition_TDS) & Trim(NewCode), Trim(lbl_EntryNo.Text), Convert.ToDateTime(msk_Date.Text), vPBillNo, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If

            End If


            '-----Bill Posting
            vVouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(msk_Date.Text), vVouPos_LedID, Trim(vPBillNo), Agt_Idno, Val(CSng(lbl_BillAmount.Text)), vVou_CRDR_BillType, Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software, SaveAll_STS)
            If Trim(UCase(vVouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If


            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If


            move_record(lbl_EntryNo.Text)

        Catch ex As Exception
            tr.Rollback()
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()


            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()


        End Try


    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer = 0
        Dim MtchSTS As Boolean = False

        If Trim(txt_ItemParticulars.Text) = "" Then
            MessageBox.Show("Invalid Item particulars", "DOES NOT ADD...", MessageBoxButtons.OKCancel)
            If txt_ItemParticulars.Enabled Then txt_ItemParticulars.Focus()
            Exit Sub
        End If

        If Val(txt_Amount.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT ADD...", MessageBoxButtons.OKCancel)
            If txt_Amount.Enabled Then txt_Amount.Focus()
            Exit Sub
        End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1

                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    n = i

                    '.Rows(i).Selected = True

                    MtchSTS = True

                    'If i >= 10 Then .FirstDisplayedScrollingRowIndex = i - 9

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()

            End If

            .Rows(n).Cells(0).Value = txt_SlNo.Text
            .Rows(n).Cells(1).Value = txt_ItemParticulars.Text
            .Rows(n).Cells(2).Value = cbo_Unit.Text
            .Rows(n).Cells(3).Value = txt_Hsn_Sac_Code.Text
            .Rows(n).Cells(4).Value = Val(txt_GstPerc.Text)
            .Rows(n).Cells(5).Value = Val(txt_Quantity.Text)
            .Rows(n).Cells(6).Value = Format(Val(txt_Rate.Text), "########0.00")
            .Rows(n).Cells(7).Value = Format(Val(txt_Amount.Text), "########0.00")
            .Rows(n).Cells(8).Value = Format(Val(txt_DiscountPerc.Text), "########0.00")
            .Rows(n).Cells(9).Value = Format(Val(txt_DiscountAmount.Text), "########0.00")
            .Rows(n).Cells(10).Value = Format(Val(lbl_TotalAmount.Text), "########0.00")

            .Rows(n).Cells(11).Value = Format(Val(lbl_Grid_FooterDiscPerc.Text), "########0.00")
            .Rows(n).Cells(12).Value = Format(Val(lbl_Grid_FooterDiscAmount.Text), "########0.00")

            .Rows(n).Cells(13).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")


        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        txt_ItemParticulars.Text = ""
        cbo_Unit.Text = ""
        txt_Hsn_Sac_Code.Text = ""
        txt_GstPerc.Text = ""
        txt_Quantity.Text = ""
        txt_Rate.Text = ""
        txt_Amount.Text = ""
        txt_DiscountPerc.Text = ""
        txt_DiscountAmount.Text = ""
        lbl_TotalAmount.Text = ""
        lbl_Grid_FooterDiscPerc.Text = ""
        lbl_Grid_FooterDiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""

        Grid_Cell_DeSelect()

        If txt_ItemParticulars.Enabled And txt_ItemParticulars.Visible Then txt_ItemParticulars.Focus()

    End Sub

    Private Sub txt_Quantity_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Quantity.KeyDown
        txt_Quantity.Tag = txt_Quantity.Text
    End Sub

    Private Sub txt_Quantity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Quantity.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Quantity_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Quantity.KeyUp
        If Val(txt_Quantity.Tag) <> Val(txt_Quantity.Text) Then
            Call Amount_Calculation(1)
        End If
    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        txt_Rate.Tag = txt_Rate.Text
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        If Val(txt_Rate.Tag) <> Val(txt_Rate.Text) Then
            Call Amount_Calculation(1)
        End If
    End Sub

    Private Sub txt_Amount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Amount.KeyDown
        txt_Amount.Tag = txt_Amount.Text
    End Sub

    Private Sub txt_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Amount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Amount.KeyUp
        If Val(txt_Amount.Tag) <> Val(txt_Amount.Text) Then
            Call Amount_Calculation(0)
        End If
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then ''-----united
            If Trim(UCase(vEntryType)) = "CRNT" Or Trim(UCase(vEntryType)) = "DRNT" Then
                Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1 ) and Close_status = 0  )", "(Ledger_IdNo = 0)")
            Else
                Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
            End If
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If

        cbo_Ledger.Tag = cbo_Ledger.Text
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then ''-----united
                If Trim(UCase(vEntryType)) = "CRNT" Or Trim(UCase(vEntryType)) = "DRNT" Then
                    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1 ) and Close_status = 0  )", "(Ledger_IdNo = 0)")
                Else
                    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
                End If
            Else

                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
            End If

            If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                If txt_BillNo.Visible Then
                    txt_BillNo.Focus()
                ElseIf cbo_DeliveryTo.Visible = True Then
                    cbo_DeliveryTo.Focus()
                Else
                    cbo_EntryAcName.Focus()
                End If
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress

        Try
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then ''-----united
                If Trim(UCase(vEntryType)) = "CRNT" Or Trim(UCase(vEntryType)) = "DRNT" Then
                    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1 ) and Close_status = 0  )", "(Ledger_IdNo = 0)")
                Else
                    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
                End If
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
            End If

            If Asc(e.KeyChar) = 13 Then
                e.Handled = True
                If txt_BillNo.Visible Then
                    txt_BillNo.Focus()
                ElseIf cbo_DeliveryTo.Visible = True Then
                    cbo_DeliveryTo.Focus()
                Else
                    cbo_EntryAcName.Focus()
                End If
                If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                    cbo_Ledger.Tag = cbo_Ledger.Text
                    get_Ledger_City_Name()

                    Amount_Calculation(True)

                End If

                get_Ledger_TotalSales()
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
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

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            get_Ledger_City_Name()

            Amount_Calculation(True)
        End If
    End Sub

    Private Sub txt_CashDiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscPerc.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : txt_ItemParticulars.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CashDiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_AddLess.Focus()
        End If
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_ItemParticulars.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            If txt_EWayBillNo.Visible Then
                txt_EWayBillNo.Focus()

            ElseIf txt_IR_No.Visible Then
                txt_IR_No.Focus()

            ElseIf cbo_Reason_For_Note.Visible Then
                cbo_Reason_For_Note.Focus()
            ElseIf Cbo_VehicleNo.Visible Then
                Cbo_VehicleNo.Focus()
            ElseIf msk_BillDate.Visible Then
                msk_BillDate.Focus()
            Else
                cbo_EntryAcName.Focus()
            End If
        End If
    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                For i = 0 To .Rows.Count - 1

                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        Move_ItemDetails_From_Grid_To_InputPanel(i)

                        Exit For

                    End If

                Next

            End With

            If Val(txt_SlNo.Text) = 0 Then
                txt_SlNo.Text = dgv_Details.Rows.Count + 1
                txt_CashDiscPerc.Focus()
            Else
                txt_ItemParticulars.Focus()
            End If

        End If

    End Sub

    Private Sub txt_ItemParticulars_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ItemParticulars.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            If Trim(txt_ItemParticulars.Text) = "" Then
                txt_CashDiscPerc.Focus()
            Else
                cbo_Unit.Focus()
            End If
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_SlNo.Focus()
        End If

    End Sub

    Private Sub txt_ItemParticulars_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ItemParticulars.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(txt_ItemParticulars.Text) = "" Then
                txt_CashDiscPerc.Focus()
            Else
                cbo_Unit.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index
                .Rows.RemoveAt(n)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            TotalAmount_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            txt_ItemParticulars.Text = ""
            cbo_Unit.Text = ""
            txt_Hsn_Sac_Code.Text = ""
            txt_GstPerc.Text = ""
            txt_Quantity.Text = ""
            txt_Rate.Text = ""
            txt_Amount.Text = ""
            txt_DiscountPerc.Text = ""
            txt_DiscountAmount.Text = ""
            lbl_TotalAmount.Text = ""
            lbl_Grid_FooterDiscPerc.Text = ""
            lbl_Grid_FooterDiscAmount.Text = ""
            lbl_Grid_AssessableValue.Text = ""

            If txt_ItemParticulars.Enabled And txt_ItemParticulars.Visible Then txt_ItemParticulars.Focus()

        End If

    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

            Move_ItemDetails_From_Grid_To_InputPanel(dgv_Details.CurrentRow.Index)

            If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub Move_ItemDetails_From_Grid_To_InputPanel(ByVal RowID As Integer)

        With dgv_Details

            If RowID <= .Rows.Count - 1 Then

                txt_SlNo.Text = Trim(.Rows(RowID).Cells(0).Value)
                txt_ItemParticulars.Text = Trim(.Rows(RowID).Cells(1).Value)
                cbo_Unit.Text = Trim(.Rows(RowID).Cells(2).Value)
                txt_Hsn_Sac_Code.Text = Trim(.Rows(RowID).Cells(3).Value)
                txt_GstPerc.Text = Val(.Rows(RowID).Cells(4).Value)
                txt_Quantity.Text = Format(Val(.Rows(RowID).Cells(5).Value), "########0.00")
                txt_Rate.Text = Format(Val(.Rows(RowID).Cells(6).Value), "########0.00")

                txt_Amount.Text = Format(Val(.Rows(RowID).Cells(7).Value), "########0.00")
                txt_DiscountPerc.Text = Format(Val(.Rows(RowID).Cells(8).Value), "########0.00")
                txt_DiscountAmount.Text = Format(Val(.Rows(RowID).Cells(9).Value), "########0.00")

                lbl_TotalAmount.Text = Format(Val(.Rows(RowID).Cells(10).Value), "########0.00")

                lbl_Grid_FooterDiscPerc.Text = Format(Val(.Rows(RowID).Cells(11).Value), "########0.00")
                lbl_Grid_FooterDiscAmount.Text = Format(Val(.Rows(RowID).Cells(12).Value), "########0.00")

                lbl_Grid_AssessableValue.Text = Format(Val(.Rows(RowID).Cells(13).Value), "########0.00")

            End If

        End With

    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "unit_head", "unit_Name", "", "(unit_idno = 0)")
    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, txt_ItemParticulars, txt_Hsn_Sac_Code, "unit_head", "unit_Name", "", "(unit_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, txt_Hsn_Sac_Code, "unit_head", "unit_Name", "", "(unit_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub cbo_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub chk_Tax_FullRoundOff_Status_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_Tax_FullRoundOff_Status.CheckedChanged
        TotalAmount_Calculation()
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Other_GST_Entry_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Other_GST_Entry_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Other_GST_Entry_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                Itm_IdNo = Common_Procedures.Item_NameToIdNo(con, cbo_Filter_ItemName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Other_GST_Entry_Reference_Code IN (select z.Other_GST_Entry_Reference_Code from Other_GST_Entry_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            'da = New SqlClient.SqlDataAdapter("select a.Other_GST_Entry_Reference_No, a.Other_GST_Entry_Date, a.Total_Quantity, a.Net_Amount, b.Ledger_Name from Other_GST_Entry_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Other_GST_Entry_Reference_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Other_GST_Entry_Reference_No", con)

            da = New SqlClient.SqlDataAdapter("select a.Other_GST_Entry_No, a.Other_GST_Entry_Date, a.Total_Quantity, a.Net_Amount, b.Ledger_Name from Other_GST_Entry_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Other_GST_Entry_Reference_CODE LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Other_GST_Entry_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    '    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Other_GST_Entry_Reference_No").ToString

                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Other_GST_Entry_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Other_GST_Entry_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Quantity").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            dt2.Dispose()
            da.Dispose()

        End Try

        If dgv_Filter_Details.Rows.Count > 0 Then
            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()
        Else
            dtp_Filter_Fromdate.Focus()
        End If

    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemName.GotFocus
        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Other_GST_Entry_Details", "Item_Particulars", "", "(Item_Particulars = '')")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "Other_GST_Entry_Details", "Item_Particulars", "", "(Item_Particulars = '')")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, Nothing, "Other_GST_Entry_Details", "Item_Particulars", "", "(Item_Particulars = '')")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        If dgv_Filter_Details.Rows.Count > 0 Then
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_back.Enabled = True
                pnl_Filter.Visible = False
            End If

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

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows.RemoveAt(i)

                    MtchSTS = True

                    Exit For

                End If

            Next

            If MtchSTS = True Then
                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next
            End If

        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        txt_ItemParticulars.Text = ""
        cbo_Unit.Text = ""
        txt_Hsn_Sac_Code.Text = ""
        txt_GstPerc.Text = ""
        txt_Quantity.Text = ""
        txt_Rate.Text = ""
        txt_Amount.Text = ""
        txt_DiscountPerc.Text = ""
        txt_DiscountAmount.Text = ""
        lbl_TotalAmount.Text = ""
        lbl_Grid_FooterDiscPerc.Text = ""
        lbl_Grid_FooterDiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""

        If txt_ItemParticulars.Enabled And txt_ItemParticulars.Visible Then txt_ItemParticulars.Focus()

    End Sub


    Private Sub txt_DiscountPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscountPerc.KeyDown
        txt_DiscountPerc.Tag = txt_DiscountPerc.Text
    End Sub

    Private Sub txt_DiscountPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscountPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscountPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscountPerc.KeyUp
        If Val(txt_DiscountPerc.Tag) <> Val(txt_DiscountPerc.Text) Then
            txt_DiscountPerc.Tag = txt_DiscountPerc.Text
            Call Amount_Calculation(2)
        End If
    End Sub

    Private Sub txt_DiscountAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscountAmount.KeyDown
        txt_DiscountAmount.Tag = txt_DiscountAmount.Text
        If e.KeyCode = 40 Then
            btn_Add_Click(sender, e)
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If Val(txt_DiscountPerc.Text) <> 0 And e.KeyCode <> 13 Then e.Handled = True : e.SuppressKeyPress = True
    End Sub

    Private Sub txt_DiscountAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscountAmount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            btn_Add_Click(sender, e)
        Else
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        End If
    End Sub

    Private Sub txt_DiscountAmount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscountAmount.KeyUp
        If Val(txt_DiscountAmount.Tag) <> Val(txt_DiscountAmount.Text) Then
            txt_DiscountAmount.Tag = txt_DiscountAmount.Text
            Call Amount_Calculation(0)
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_Ledger.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Remarks.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            dtp_Date.Text = Date.Today
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Ledger.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
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

    'Private Sub msk_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.TextChanged
    '    msk_Date_LostFocus(sender, e)
    'End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then e.Handled = True : e.SuppressKeyPress = True : cbo_Ledger.Focus()
        If e.KeyCode = 38 Then e.Handled = True : e.SuppressKeyPress = True : msk_Date.Focus()
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            e.Handled = True
            dtp_Date.Text = Date.Today
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Ledger.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            e.Handled = True
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If

    End Sub

    Private Sub msk_BillDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BillDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_EntryAcName.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_BillNo.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_BillDate.Text
            vmskSelStrt = msk_BillDate.SelectionStart
        End If

    End Sub

    Private Sub msk_BillDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_BillDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            dtp_BillDate.Text = Date.Today
            msk_BillDate.Text = dtp_BillDate.Text
            msk_BillDate.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_EntryAcName.Focus()
        End If
    End Sub

    Private Sub msk_BillDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BillDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    dtp_BillDate.Text = Date.Today
        '    msk_BillDate.Text = dtp_BillDate.Text
        '    msk_BillDate.SelectionStart = 0
        'End If
        If e.KeyCode = 107 Then
            msk_BillDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_BillDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_BillDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_BillDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub msk_BillDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_BillDate.LostFocus
        If IsDate(msk_BillDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_BillDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_BillDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_BillDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_BillDate.Text)) >= 2000 Then
                    dtp_BillDate.Value = Convert.ToDateTime(msk_BillDate.Text)
                End If
            End If
        End If
    End Sub

    Private Sub msk_BillDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_BillDate.TextChanged
        msk_BillDate_LostFocus(sender, e)
    End Sub

    Private Sub dtp_BillDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_BillDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then e.Handled = True : e.SuppressKeyPress = True : txt_SlNo.Focus()
        If e.KeyCode = 38 Then e.Handled = True : e.SuppressKeyPress = True : cbo_EntryAcName.Focus()
    End Sub

    Private Sub dtp_BillDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_BillDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            e.Handled = True
            dtp_BillDate.Text = Date.Today
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_SlNo.Focus()
        End If
    End Sub

    Private Sub dtp_BillDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_BillDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            e.Handled = True
            dtp_BillDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_BillDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_BillDate.TextChanged
        If IsDate(dtp_BillDate.Text) = True Then
            msk_BillDate.Text = dtp_BillDate.Text
            msk_BillDate.SelectionStart = 0
        End If
    End Sub

    Private Sub cbo_EntryAcName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntryAcName.GotFocus
        Dim vCondt As String = ""

        If Trim(UCase(vEntryType)) = "PURC" Or Trim(UCase(vEntryType)) = "DRNT" Then
            vCondt = "(AccountsGroup_IdNo = 27 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 )"

        ElseIf Trim(UCase(vEntryType)) = "SALE" Or Trim(UCase(vEntryType)) = "CRNT" Or Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then
            vCondt = "(AccountsGroup_IdNo = 28 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 )"

        ElseIf Trim(UCase(vEntryType)) = "ADV.PYMT" Then
            vCondt = "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 23 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 )"

        Else
            vCondt = "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 or AccountsGroup_IdNo = 23 or AccountsGroup_IdNo = 27 or AccountsGroup_IdNo = 28)"

        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_EntryAcName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntryAcName.KeyDown
        Dim vCondt As String = ""

        vcbo_KeyDwnVal = e.KeyValue

        If Trim(UCase(vEntryType)) = "PURC" Or Trim(UCase(vEntryType)) = "DRNT" Then
            vCondt = "(AccountsGroup_IdNo = 27 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 )"

        ElseIf Trim(UCase(vEntryType)) = "SALE" Or Trim(UCase(vEntryType)) = "CRNT" Or Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then
            vCondt = "(AccountsGroup_IdNo = 28 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 )"

        ElseIf Trim(UCase(vEntryType)) = "ADV.PYMT" Then
            vCondt = "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 23 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 )"

        Else
            vCondt = "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 or AccountsGroup_IdNo = 23 or AccountsGroup_IdNo = 27 or AccountsGroup_IdNo = 28)"

        End If


        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntryAcName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", vCondt, "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_EntryAcName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True : e.SuppressKeyPress = True

            If cbo_OnAc_Type.Visible Then
                cbo_OnAc_Type.Focus()
            ElseIf cbo_Unregister_Type.Visible Then
                cbo_Unregister_Type.Focus()
            ElseIf Cbo_Transport.Visible Then
                Cbo_Transport.Focus()
            ElseIf Cbo_VehicleNo.Visible Then
                Cbo_VehicleNo.Focus()
            ElseIf cbo_Reason_For_Note.Visible Then
                cbo_Reason_For_Note.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If

        If (e.KeyValue = 38 And cbo_EntryAcName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            e.Handled = True : e.SuppressKeyPress = True
            If msk_BillDate.Visible Then
                msk_BillDate.Focus()
            ElseIf cbo_DeliveryTo.Visible = True Then
                cbo_DeliveryTo.Focus()
            Else
                cbo_Ledger.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_EntryAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntryAcName.KeyPress
        Dim vCondt As String = ""


        If Trim(UCase(vEntryType)) = "PURC" Or Trim(UCase(vEntryType)) = "DRNT" Then
            vCondt = "(AccountsGroup_IdNo = 27 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 )"

        ElseIf Trim(UCase(vEntryType)) = "SALE" Or Trim(UCase(vEntryType)) = "CRNT" Or Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then
            vCondt = "(AccountsGroup_IdNo = 28 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 )"

        ElseIf Trim(UCase(vEntryType)) = "ADV.PYMT" Then
            vCondt = "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 23 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 )"

        Else
            vCondt = "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 or AccountsGroup_IdNo = 23 or AccountsGroup_IdNo = 27 or AccountsGroup_IdNo = 28)"

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1105" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntryAcName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)", False)
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntryAcName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", vCondt, "(Ledger_IdNo = 0)", False)
        End If


        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If cbo_OnAc_Type.Visible Then
                cbo_OnAc_Type.Focus()
            ElseIf cbo_Unregister_Type.Visible Then
                cbo_Unregister_Type.Focus()
            ElseIf Cbo_Transport.Visible Then
                Cbo_Transport.Focus()
            ElseIf Cbo_VehicleNo.Visible Then
                Cbo_VehicleNo.Focus()
            ElseIf cbo_Reason_For_Note.Visible Then
                cbo_Reason_For_Note.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_EntryAcName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntryAcName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EntryAcName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_EntryPrefixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EntryPrefixNo.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Remarks.Focus()
        End If
    End Sub

    Private Sub txt_EntryPrefixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EntryPrefixNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : btn_save.Focus()
        If e.KeyCode = 38 Then
            'e.Handled = True
            'SendKeys.Send("+{TAB}")
            cbo_Agent.Focus()

        End If
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        EMAIL_Status = False

        print_record()
    End Sub

    Private Sub btn_Pdf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Pdf.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        EMAIL_Status = False

        print_record()
        Print_PDF_Status = False
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub Amount_Calculation(ByVal CalcType As Integer)

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If CalcType = 1 Then
            txt_Amount.Text = Format(Val(txt_Quantity.Text) * Val(txt_Rate.Text), "#########0.00")
        End If
        If Val(txt_DiscountPerc.Text) <> 0 Or CalcType = 2 Then
            txt_DiscountAmount.Text = Format(Val(txt_Amount.Text) * Val(txt_DiscountPerc.Text) / 100, "#########0.00")
        End If
        lbl_TotalAmount.Text = Format(Val(txt_Amount.Text) - Val(txt_DiscountAmount.Text), "#########0.00")
        lbl_Grid_FooterDiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
        lbl_Grid_FooterDiscAmount.Text = Format(Val(lbl_TotalAmount.Text) * Val(lbl_Grid_FooterDiscPerc.Text) / 100, "#########0.00")
        lbl_Grid_AssessableValue.Text = Format(Val(lbl_TotalAmount.Text) - Val(lbl_Grid_FooterDiscAmount.Text), "#########0.00")
    End Sub

    Private Sub Amount_Calculation(ByVal GridAll_Row_STS As Boolean)
        Dim i As Integer = 0

        '***** GST START *****

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If GridAll_Row_STS = True Then

            With dgv_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        '.Rows(i).Cells(7).Value = Format(Val(.Rows(i).Cells(5).Value) * Val(.Rows(i).Cells(6).Value), "#########0.00")

                        If Val(.Rows(i).Cells(8).Value) <> 0 Then
                            .Rows(i).Cells(9).Value = Format(Val(.Rows(i).Cells(7).Value) * Val(.Rows(i).Cells(8).Value) / 100, "#########0.00")
                        End If
                        .Rows(i).Cells(10).Value = Format(Val(.Rows(i).Cells(7).Value) - Val(.Rows(i).Cells(9).Value), "#########0.00")

                        .Rows(i).Cells(11).Value = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
                        .Rows(i).Cells(12).Value = Format(Val(.Rows(i).Cells(10).Value) * Val(.Rows(i).Cells(11).Value) / 100, "#########0.00")

                        .Rows(i).Cells(13).Value = Format(Val(.Rows(i).Cells(10).Value) - Val(.Rows(i).Cells(12).Value), "#########0.00")

                    End If

                Next

            End With

            TotalAmount_Calculation()

        Else

            'txt_Amount.Text = Format(Val(txt_NoofItems.Text) * Val(txt_Rate.Text), "#########0.00")
            If Val(txt_DiscountPerc.Text) <> 0 Then
                txt_DiscountAmount.Text = Format(Val(txt_Amount.Text) * Val(txt_DiscountPerc.Text) / 100, "#########0.00")
            End If
            lbl_TotalAmount.Text = Format(Val(txt_Amount.Text) - Val(txt_DiscountAmount.Text), "#########0.00")
            lbl_Grid_FooterDiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
            lbl_Grid_FooterDiscAmount.Text = Format(Val(lbl_TotalAmount.Text) * Val(lbl_Grid_FooterDiscPerc.Text) / 100, "#########0.00")
            lbl_Grid_AssessableValue.Text = Format(Val(lbl_TotalAmount.Text) - Val(lbl_Grid_FooterDiscAmount.Text), "#########0.00")

        End If

        '***** GST END *****

    End Sub

    Private Sub TotalAmount_Calculation()
        Dim Sno As Integer = 0
        Dim TotQty As String = 0
        Dim TotAmt As String = 0
        Dim TotDiscAmt As String = 0
        Dim Tot_TtAmt As String = 0
        Dim TotFtrDiscAmt As String = 0
        Dim Tot_Grd_AssAmt As String = 0
        Dim Tot_TxbleAmt As String = 0
        Dim TotCGstAmt As String = 0
        Dim TotSGstAmt As String = 0
        Dim TotIGstAmt As String = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotQty = 0
        TotAmt = 0
        TotDiscAmt = 0
        Tot_TtAmt = 0
        TotFtrDiscAmt = 0
        Tot_Grd_AssAmt = 0

        For i = 0 To dgv_Details.RowCount - 1

            Sno = Sno + 1

            dgv_Details.Rows(i).Cells(0).Value = Sno

            If Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then
                TotQty = Val(TotQty) + Val(dgv_Details.Rows(i).Cells(5).Value)
                TotAmt = Format(Val(TotAmt) + Val(dgv_Details.Rows(i).Cells(7).Value), "###########0.00")

                TotDiscAmt = Format(Val(TotDiscAmt) + Val(dgv_Details.Rows(i).Cells(9).Value), "###########0.00")
                Tot_TtAmt = Format(Val(Tot_TtAmt) + Val(dgv_Details.Rows(i).Cells(10).Value), "###########0.00")

                TotFtrDiscAmt = Format(Val(TotFtrDiscAmt) + Val(dgv_Details.Rows(i).Cells(12).Value), "###########0.00")
                Tot_Grd_AssAmt = Format(Val(Tot_Grd_AssAmt) + Val(dgv_Details.Rows(i).Cells(13).Value), "###########0.00")

            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotQty)
            .Rows(0).Cells(7).Value = Format(Val(TotAmt), "###########0.00")

            .Rows(0).Cells(9).Value = Format(Val(TotDiscAmt), "##########0.00")
            .Rows(0).Cells(10).Value = Format(Val(Tot_TtAmt), "##########0.00")

            .Rows(0).Cells(12).Value = Format(Val(TotFtrDiscAmt), "##########0.00")
            .Rows(0).Cells(13).Value = Format(Val(Tot_Grd_AssAmt), "##########0.00")

        End With

        lbl_GrossAmount.Text = Format(Val(Tot_TtAmt), "###########0.00")
        lbl_CashDiscAmount.Text = Format(Val(TotFtrDiscAmt), "###########0.00")

        Get_HSN_CodeWise_GSTTax_Details()

        Tot_TxbleAmt = 0
        TotCGstAmt = 0
        TotSGstAmt = 0
        TotIGstAmt = 0
        With dgv_GSTTax_Details_Total
            If .RowCount > 0 Then
                Tot_TxbleAmt = Val(.Rows(0).Cells(2).Value)
                TotCGstAmt = Val(.Rows(0).Cells(4).Value)
                TotSGstAmt = Val(.Rows(0).Cells(6).Value)
                TotIGstAmt = Val(.Rows(0).Cells(8).Value)
            End If
        End With

        'OLD
        '  lbl_TaxableValue.Text = Format(Val(Tot_TxbleAmt), "########0.00")

        lbl_TaxableValue.Text = Format(Val(Tot_TtAmt) - Val(TotFtrDiscAmt), "########0.00")
        lbl_CGstAmount.Text = Format(Val(TotCGstAmt), "########0.00")
        lbl_SGstAmount.Text = Format(Val(TotSGstAmt), "########0.00")
        lbl_IGstAmount.Text = Format(Val(TotIGstAmt), "########0.00")

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As String = 0
        Dim vBlAmt As String = 0
        Dim TDS As String = 0
        Dim vTCS_AssVal As String = 0
        Dim vTOT_SalAmt As String = 0
        Dim vTCS_Amt As String = 0
        Dim vInvAmt_Bfr_TCS As String = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub


        Dim vTCS_StartDate As Date = #9/30/2020#
        Dim vMIN_TCS_assval As String = "5000000"
        Dim vTDS_StartDate As Date = #6/30/2021#
        Dim Led_ID = 0
        Dim vTDS_AssVal As String, vTDS_Amt As String
        Dim vTCS_Comp_STS = 0
        Dim vTCS_DED_STS As Boolean = False
        Dim vTCS_Led_STS As String = 0

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If chk_TCS_Tax.Checked = True Then

            If DateDiff("d", vTCS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                vTCS_Comp_STS = Common_Procedures.get_FieldValue(con, "company_head", "TCS_Company_Status", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")")

                If Val(vTCS_Comp_STS) = 1 Then

                    vTCS_DED_STS = True

                    If DateDiff("d", vTDS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                        Dim vLED_TCS_FIELD_NAME = ""


                        If Trim(vEntryType) = "PURC" Or Trim(vEntryType) = "DRNT" Then
                            vLED_TCS_FIELD_NAME = "TCS_PURCHASE_Status"
                            'ElseIf Trim(vEntryType) = "SALE" Or Trim(vEntryType) = "CRNT" Then
                        Else
                            vLED_TCS_FIELD_NAME = "TCS_Sales_Status"
                        End If

                        vTCS_Led_STS = Common_Procedures.get_FieldValue(con, "ledger_head", " " & Trim(vLED_TCS_FIELD_NAME) & " ", "(ledger_idno = " & Str(Val(Led_ID)) & ")")

                        vTCS_DED_STS = False
                        If Val(vTCS_Led_STS) = 1 Then
                            vTCS_DED_STS = True
                        End If

                    End If
                    If vTCS_DED_STS = True Then

                        If txt_TCS_TaxableValue.Enabled = False Then

                            vTOT_SalAmt = Format(Val(lbl_TaxableValue.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text), "###########0")

                            vTCS_AssVal = 0

                            If Val(CDbl(lbl_TotalSales_Amount_Previous_Year.Text)) > Val(vMIN_TCS_assval) Then

                                vTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                            ElseIf Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) > Val(vMIN_TCS_assval) Then
                                vTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                            ElseIf (Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt)) > Val(vMIN_TCS_assval) Then
                                vTCS_AssVal = Format(Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt) - Val(vMIN_TCS_assval), "############0")

                            End If
                            txt_TCS_TaxableValue.Text = Format(Val(vTCS_AssVal), "############0.00")

                            If Val(txt_TCS_TaxableValue.Text) > 0 Then
                                If Val(txt_TcsPerc.Text) = 0 Then
                                    txt_TcsPerc.Text = "0.1"
                                End If
                            End If

                        End If

                        vTCS_Amt = Format(Val(txt_TCS_TaxableValue.Text) * Val(txt_TcsPerc.Text) / 100, "##########0.00")
                        If chk_TCSAmount_RoundOff_STS.Checked = True Then
                            vTCS_Amt = Format(Val(vTCS_Amt), "##########0")
                        End If
                        lbl_TcsAmount.Text = Format(Val(vTCS_Amt), "##########0.00")

                    Else

                        txt_TCS_TaxableValue.Text = ""
                        txt_TcsPerc.Text = ""
                        lbl_TcsAmount.Text = ""

                    End If
                Else

                    txt_TCS_TaxableValue.Text = ""
                    txt_TcsPerc.Text = ""
                    lbl_TcsAmount.Text = ""

                End If
            Else

                txt_TCS_TaxableValue.Text = ""
                txt_TcsPerc.Text = ""
                lbl_TcsAmount.Text = ""

            End If

        Else
            txt_TCS_TaxableValue.Text = ""
            txt_TcsPerc.Text = ""
            lbl_TcsAmount.Text = ""

        End If

        vInvAmt_Bfr_TCS = Format(Val(lbl_GrossAmount.Text) - Val(lbl_CashDiscAmount.Text) + Val(txt_AddLess.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text), "##########0.00")
        'vInvAmt_Bfr_TCS = Format(Val(lbl_TaxableValue.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text), "###########0.00")
        lbl_Invoice_Value_Before_TCS.Text = Format(Val(vInvAmt_Bfr_TCS), "###########0")
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = Format(Val(lbl_Invoice_Value_Before_TCS.Text) - Val(vInvAmt_Bfr_TCS), "###########0.00")

        NtAmt = Format(Val(lbl_Invoice_Value_Before_TCS.Text) + Val(lbl_TcsAmount.Text), "############0.00")

        lbl_NetAmount.Text = Format(Val(NtAmt), "###########0")

        lbl_RoundOff.Text = Format(Val(lbl_NetAmount.Text) - Val(NtAmt), "#########0.00")
        lbl_NetAmount.Text = Format(Val(lbl_NetAmount.Text), "###########0.00")




        ' ---- TDS AMOUNT 


        If chk_TDS_Tax.Checked = True Then

            If DateDiff("d", vTDS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                Dim vTDS_Led_STS As String = 0
                Dim vLED_TDS_FIELD_NAME = ""


                If Trim(vEntryType) = "PURC" Or Trim(vEntryType) = "DRNT" Then
                    vLED_TDS_FIELD_NAME = "Purchase_TDS_Deduction_Status"
                    'ElseIf Trim(vEntryType) = "SALE" Or Trim(vEntryType) = "CRNT" Then
                Else
                    vLED_TDS_FIELD_NAME = "Sales_TDS_Deduction_Status"
                End If


                vTDS_Led_STS = Common_Procedures.get_FieldValue(con, "ledger_head", "" & Trim(vLED_TDS_FIELD_NAME) & "", "(ledger_idno = " & Str(Val(Led_ID)) & ")")

                If Val(vTDS_Led_STS) = 1 Then
                    If lbl_Tds_Amount.Enabled = False Then

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1530" Then ' ---  RAJAMOHAN 
                            txt_Tds_Percentage.Text = Common_Procedures.get_FieldValue(con, "ledger_head", "Tds_Perc", "(ledger_idno = " & Str(Val(Led_ID)) & ")")
                        End If

                        vTDS_Amt = Format(Val(txt_TDS_Value.Text) * Val(txt_Tds_Percentage.Text) / 100, "##########0")
                        'If Val(lbl_TaxableValue.Text) <> 0 Then
                        '    vTDS_Amt = Format(Val(lbl_TaxableValue.Text) * Val(txt_Tds_Percentage.Text) / 100, "############0")
                        'Else
                        '    vTDS_Amt = Format(Val(lbl_GrossAmount.Text) * Val(txt_Tds_Percentage.Text) / 100, "############0")
                        'End If
                        lbl_Tds_Amount.Text = Format(Val(vTDS_Amt), "##########0.00")
                    End If
                Else
                    txt_Tds_Percentage.Text = ""
                    lbl_Tds_Amount.Text = ""
                    txt_TDS_Value.Text = ""
                End If

            Else

                txt_Tds_Percentage.Text = ""
                lbl_Tds_Amount.Text = ""
                txt_TDS_Value.Text = ""

            End If

        Else

            txt_Tds_Percentage.Text = ""
            lbl_Tds_Amount.Text = ""
            txt_TDS_Value.Text = ""

        End If


        ' ------

        'If Val(lbl_TaxableValue.Text) <> 0 Then
        '    TDS = Format(Val(lbl_TaxableValue.Text) * Val(txt_Tds_Percentage.Text) / 100, "############0")
        'Else
        '    TDS = Format(Val(lbl_GrossAmount.Text) * Val(txt_Tds_Percentage.Text) / 100, "############0")
        'End If
        'lbl_Tds_Amount.Text = Format(Val(TDS), "###########0.00")

        ' 'lbl_BillAmount.Text = Format(Val(lbl_NetAmount.Text) - Val(lbl_Tds_Amount.Text), "#############0.00")


        vBlAmt = Format(Val(lbl_NetAmount.Text) - Val(lbl_Tds_Amount.Text), "#############0.00")

        lbl_BillAmount.Text = Format(Val(vBlAmt), "###########0")

        lbl_BillAmount.Text = Format(Val(lbl_BillAmount.Text), "###########0.00")

    End Sub

    Private Sub Get_HSN_CodeWise_GSTTax_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim ItmIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

            cmd.Connection = con


            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            With dgv_Details

                If .Rows.Count > 0 Then
                    For i = 0 To .Rows.Count - 1

                        If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(4).Value) <> 0 And Val(.Rows(i).Cells(7).Value) <> 0 Then

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1               ,                      Currency1           ,                      Currency2            ) " &
                                              "            Values    ( '" & Trim(.Rows(i).Cells(3).Value) & "', " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & " ) "
                            cmd.ExecuteNonQuery()

                        End If

                    Next
                End If
            End With

            With dgv_GSTTax_Details

                .Rows.Clear()
                Sno = 0

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as Assessable_Value from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 order by name1, Currency1", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1

                        .Rows(n).Cells(0).Value = Sno
                        .Rows(n).Cells(1).Value = dt.Rows(i).Item("HSN_Code").ToString

                        .Rows(n).Cells(2).Value = Format(Val(dt.Rows(i).Item("Assessable_Value").ToString), "############0.00")
                        If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""

                        .Rows(n).Cells(3).Value = ""
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(7).Value = ""
                        If InterStateStatus = True Then
                            .Rows(n).Cells(7).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString), "######0.00")
                            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                        Else
                            .Rows(n).Cells(3).Value = Format(Val(dt.Rows(i)("GST_Percentage").ToString) / 2, "#########0.00")
                            .Rows(n).Cells(5).Value = Format(Val(dt.Rows(i)("GST_Percentage").ToString) / 2, "#########0.00")

                        End If

                        .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "############0.00")
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                        .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "############0.00")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                        .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "############0.00")
                        If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                        If chk_Tax_FullRoundOff_Status.Checked = True Then
                            .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(4).Value), "############0")
                            .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(4).Value), "############0.00")

                            .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(6).Value), "############0")
                            .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(6).Value), "############0.00")

                            .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(8).Value), "############0")
                            .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(8).Value), "############0.00")
                        End If

                    Next i

                End If

                dt.Clear()
                dt.Dispose()
                da.Dispose()

            End With

            Total_GSTTax_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Private Sub Total_GSTTax_Calculation()
        Dim Sno As Integer = 0
        Dim TotAss_Val As String = 0
        Dim TotCGST_amt As String = 0
        Dim TotSGST_amt As String = 0
        Dim TotIGST_amt As String = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotAss_Val = 0 : TotCGST_amt = 0 : TotSGST_amt = 0 : TotIGST_amt = 0

        With dgv_GSTTax_Details

            For i = 0 To .RowCount - 1

                Sno = Sno + 1

                .Rows(i).Cells(0).Value = Sno

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotAss_Val = Format(Val(TotAss_Val) + Val(.Rows(i).Cells(2).Value), "###########0.00")
                    TotCGST_amt = Format(Val(TotCGST_amt) + Val(.Rows(i).Cells(4).Value), "###########0.00")
                    TotSGST_amt = Format(Val(TotSGST_amt) + Val(.Rows(i).Cells(6).Value), "###########0.00")
                    TotIGST_amt = Format(Val(TotIGST_amt) + Val(.Rows(i).Cells(8).Value), "###########0.00")

                End If

            Next i

        End With

        With dgv_GSTTax_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(TotAss_Val), "###########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotCGST_amt), "##########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotSGST_amt), "##########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotIGST_amt), "##########0.00")
        End With

    End Sub

    Private Sub btn_GSTTax_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_GSTTax_Details.Click
        pnl_GSTTax_Details.Visible = True
        pnl_back.Enabled = False
        pnl_GSTTax_Details.Focus()
    End Sub

    Private Sub btn_Close_GSTTax_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_GSTTax_Details.Click
        pnl_back.Enabled = True
        pnl_GSTTax_Details.Visible = False
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_GST.Click
        prn_Status = 1
        print_Invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Estimate.Click
        prn_Status = 2
        print_Invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1171" Then
        '    pnl_Print.Visible = True
        '    pnl_Back.Enabled = False
        '    If btn_Print_Estimate.Enabled And btn_Print_Estimate.Visible Then
        '        btn_Print_Estimate.Focus()
        '    End If
        'Else
        print_Invoice()
        ' End If
    End Sub

    Public Sub print_Invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim NewCode As String
        Dim vFILNm As String = ""
        Dim vFLPATH As String = ""
        Dim vPDFFLPATH_and_NAME As String = ""
        Dim vPRNTRNAME As String
        Dim vPARTYNM As String = ""
        Dim Def_PrntrNm As String = ""



        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.General_entry, New_Entry) = False Then Exit Sub

        Try

            vPARTYNM = ""
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_MainName, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Other_GST_Entry_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            Else
                vPARTYNM = dt1.Rows(0).Item("Ledger_MainName").ToString
            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_InpOpts = ""
        If EMAIL_Status = True Or WHATSAPP_Status = True Then
            prn_InpOpts = "1"
        Else
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "12")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")
        End If

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then

                    vFLPATH = ""
                    vPRNTRNAME = Common_Procedures.get_PDF_PrinterName(EMAIL_Status, WHATSAPP_Status, vFLPATH)

                    If Trim(vPRNTRNAME) = "" Then
                        Exit Sub
                    End If

                    Def_PrntrNm = PrintDocument1.PrinterSettings.PrinterName

                    vPARTYNM = Common_Procedures.Replace_SpecialCharacters_With_UnderScore(vPARTYNM)

                    Dim vPrntHeading As String = ""

                    If Trim(UCase(vEntryType)) = "PURC" Then
                        vPrntHeading = "Purchase_"

                    ElseIf Trim(UCase(vEntryType)) = "SALE" Then
                        vPrntHeading = "Invoice_"

                    ElseIf Trim(UCase(vEntryType)) = "CRNT" Then
                        vPrntHeading = "CreditNote_"

                    ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                        vPrntHeading = "DebitNote_"

                    ElseIf Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then
                        vPrntHeading = "JobWorkInvoice_"

                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION)
                        vFILNm = Trim(Trim(vPrntHeading) & Trim(lbl_EntryNo.Text) & "_" & Trim(vPARTYNM) & ".pdf")
                    Else
                        vFILNm = Trim(Trim(vPrntHeading) & Trim(lbl_EntryNo.Text) & ".pdf")
                    End If

                    vFILNm = StrConv(vFILNm, vbProperCase)
                    vPDFFLPATH_and_NAME = Trim(vFLPATH) & "\" & Trim(vFILNm)
                    vEMAIL_Attachment_FileName = Trim(vPDFFLPATH_and_NAME)

                    PrintDocument1.DocumentName = Trim(vFILNm)
                    PrintDocument1.PrinterSettings.PrinterName = Trim(vPRNTRNAME)    ' "Microsoft Print to PDF"
                    'PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintToFile = True
                    PrintDocument1.PrinterSettings.PrintFileName = Trim(vPDFFLPATH_and_NAME)
                    PrintDocument1.Print()

                    'Debug.Print(PrintDocument1.PrinterSettings.PrintFileName)

                    PrintDocument1.PrinterSettings.PrinterName = Trim(Def_PrntrNm)
                    Print_PDF_Status = False

                    '--PrintDocument1.DocumentName = "Invoice"
                    '--PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    '--PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    '--PrintDocument1.Print()


                Else

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument1.Print()
                        End If

                    Else
                        PrintDocument1.Print()

                    End If

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        Print_PDF_Status = False

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0 '1
        DetSNo = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_Count = 0

        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Erase prn_DetAr

        prn_DetAr = New String(200, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* ,Lsh.State_Name as Ledger_state_name , Lsh.State_Code as Ledger_State_Code , Csh.State_Name as Company_State_Name ,Csh.State_Code as Company_State_Code, Ah.Ledger_MainName as AgentName , f.Ledger_MainName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Pan_No as DeliveryTo_PanNo, Dsh.State_Name as DeliveryTo_State_Name, Csh.State_Code as DeliveryTo_State_Code  from Other_GST_Entry_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON b.Ledger_State_IdNo = lsh.State_Idno LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else 0 end) = f.Ledger_IdNo LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = dsh.State_Idno INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = Csh.State_Idno LEFT OUTER JOIN Ledger_HEad Ah ON a.Agent_IdNo = Ah.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, c.Unit_Name from Other_GST_Entry_Details a  LEFT OUTER JOIN Unit_Head c on A.unit_idno = c.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name from Other_GST_Entry_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                'If prn_DetDt.Rows.Count > 0 Then

                '    prn_DetMxIndx = 0
                '    For I = 0 To prn_DetDt.Rows.Count - 1

                '        ItmNm1 = Trim(prn_DetDt.Rows(I).Item("Item_Particulars").ToString)
                '        ItmNm2 = ""
                '        If Len(ItmNm1) > 30 Then
                '            For K = 30 To 1 Step -1
                '                If Mid$(Trim(ItmNm1), K, 1) = " " Or Mid$(Trim(ItmNm1), K, 1) = "," Or Mid$(Trim(ItmNm1), K, 1) = "." Or Mid$(Trim(ItmNm1), K, 1) = "-" Or Mid$(Trim(ItmNm1), K, 1) = "/" Or Mid$(Trim(ItmNm1), K, 1) = "_" Or Mid$(Trim(ItmNm1), K, 1) = "(" Or Mid$(Trim(ItmNm1), K, 1) = ")" Or Mid$(Trim(ItmNm1), K, 1) = "\" Or Mid$(Trim(ItmNm1), K, 1) = "[" Or Mid$(Trim(ItmNm1), K, 1) = "]" Or Mid$(Trim(ItmNm1), K, 1) = "{" Or Mid$(Trim(ItmNm1), K, 1) = "}" Then Exit For
                '            Next K
                '            If K = 0 Then K = 30
                '            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - K)
                '            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), K - 1)
                '        End If

                '        prn_DetMxIndx = prn_DetMxIndx + 1
                '        prn_DetAr(prn_DetMxIndx, 1) = Trim(Val(I) + 1)
                '        prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm1)
                '        prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt.Rows(I).Item("Quantity").ToString)
                '        prn_DetAr(prn_DetMxIndx, 4) = prn_DetDt.Rows(I).Item("Unit_Name").ToString
                '        prn_DetAr(prn_DetMxIndx, 5) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Rate").ToString), "########0.00"))
                '        prn_DetAr(prn_DetMxIndx, 6) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Discount_Amount").ToString), "########0.00"))
                '        prn_DetAr(prn_DetMxIndx, 7) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Amount").ToString), "########0.00"))
                '        '    prn_DetAr(prn_DetMxIndx, 9) = Trim(Format(Val(prn_DetDt.Rows(I).Item("MRP_Rate").ToString), "########0.00"))
                '        ' prn_DetAr(prn_DetMxIndx, 10) = Trim(Format(Val(prn_DetDt.Rows(I).Item("MRP_Amount").ToString), "########0.00"))

                '    Next I

                'End If

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            da2.Dispose()

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim cmp_type As String = ""

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Printing_Format_GST_1186(e)
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1539" Then
            If Trim(UCase(vEntryType)) = "SALE" Then
                Printing_GST_Format_1539(e)
            Else
                Printing_Format_GST1(e)
            End If

        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1370" Then
            If Trim(UCase(vEntryType)) = "SALE" Then
                Printing_Format_GST_1370(e)
            Else
                Printing_Format_GST1(e)
            End If
        Else
            Printing_Format_GST1(e)

        End If
        'RoundOff_Invoice_Value_Before_TCS

    End Sub


    Private Sub Printing_Format_GST1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim vNoofHsnCodes As Integer = 0
        Dim vFontName As String = ""
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim ItmNm3 As String = ""
        Dim ItmNm4 As String = ""
        Dim ItmNm5 As String = ""
        Dim ItmNm6 As String = ""

        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            PrintDocument1.DefaultPageSettings.PaperSize = ps
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        End If


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30 ' 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then '----Star Fabric Mills (Thekkalur)
            vFontName = "Cambria"
        Else
            vFontName = "Calibri"
        End If
        pFont = New Font(vFontName, 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.6 ' 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height
        If Trim(UCase(vEntryType)) = "SALE" Then
            NoofItems_PerPage = 3 ' 6 '10  ' 19  
        Else
            NoofItems_PerPage = 3 ' 10  ' 19
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 45 : ClArr(2) = 190 : ClArr(3) = 80 : ClArr(4) = 50 : ClArr(5) = 75 : ClArr(6) = 40 : ClArr(7) = 90
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                '***** GST START *****
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                '  If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 4
                Else
                    If Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                    If Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString) = Val(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString) Then
                        NoofItems_PerPage = NoofItems_PerPage + 1
                    End If
                End If

                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 3
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                '***** GST END *****

                Printing_Format_GST1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, vFontName)

                Try

                    NoofDets = 0

                    '    CurY = CurY + TxtHgt

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_Format_GST1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False, vFontName)
                                e.HasMorePages = True

                                Return

                            End If

                            CurY = CurY + TxtHgt


                            ItmNm1 = prn_DetDt.Rows(DetIndx).Item("Item_Particulars").ToString
                            ItmNm2 = ""
                            ItmNm3 = ""
                            If Len(ItmNm1) > 20 Then
                                For I = 20 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 20
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Sl_No").ToString, LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Sac_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Gst_Perc").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Quantity").ToString), "##########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "#############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1
                            If Trim(ItmNm2) <> "" Then

                                If Len(ItmNm2) > 20 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 20
                                    ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                    ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I) '- 1)
                                End If

                                CurY = CurY + TxtHgt - 3
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1

                                If Len(ItmNm3) > 20 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 20
                                    ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                                    ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I) '- 1)
                                End If

                                If Trim(ItmNm3) <> "" Then
                                    CurY = CurY + TxtHgt - 3
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                If Len(ItmNm4) > 20 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmNm4), I, 1) = " " Or Mid$(Trim(ItmNm4), I, 1) = "," Or Mid$(Trim(ItmNm4), I, 1) = "." Or Mid$(Trim(ItmNm4), I, 1) = "-" Or Mid$(Trim(ItmNm4), I, 1) = "/" Or Mid$(Trim(ItmNm4), I, 1) = "_" Or Mid$(Trim(ItmNm4), I, 1) = "(" Or Mid$(Trim(ItmNm4), I, 1) = ")" Or Mid$(Trim(ItmNm4), I, 1) = "\" Or Mid$(Trim(ItmNm4), I, 1) = "[" Or Mid$(Trim(ItmNm4), I, 1) = "]" Or Mid$(Trim(ItmNm4), I, 1) = "{" Or Mid$(Trim(ItmNm4), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 20
                                    ItmNm5 = Microsoft.VisualBasic.Right(Trim(ItmNm4), Len(ItmNm4) - I)
                                    ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm4), I)
                                End If

                                If Trim(ItmNm4) <> "" Then
                                    CurY = CurY + TxtHgt - 3
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If


                                If Len(ItmNm5) > 20 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmNm5), I, 1) = " " Or Mid$(Trim(ItmNm5), I, 1) = "," Or Mid$(Trim(ItmNm5), I, 1) = "." Or Mid$(Trim(ItmNm5), I, 1) = "-" Or Mid$(Trim(ItmNm5), I, 1) = "/" Or Mid$(Trim(ItmNm5), I, 1) = "_" Or Mid$(Trim(ItmNm5), I, 1) = "(" Or Mid$(Trim(ItmNm5), I, 1) = ")" Or Mid$(Trim(ItmNm5), I, 1) = "\" Or Mid$(Trim(ItmNm5), I, 1) = "[" Or Mid$(Trim(ItmNm5), I, 1) = "]" Or Mid$(Trim(ItmNm5), I, 1) = "{" Or Mid$(Trim(ItmNm5), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 20
                                    ItmNm6 = Microsoft.VisualBasic.Right(Trim(ItmNm5), Len(ItmNm5) - I)
                                    ItmNm5 = Microsoft.VisualBasic.Left(Trim(ItmNm5), I) '-1)
                                End If

                                If Trim(ItmNm5) <> "" Then
                                    CurY = CurY + TxtHgt - 3
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm5), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If
                                If Trim(ItmNm6) <> "" Then
                                    CurY = CurY + TxtHgt - 3
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm6), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                            End If
                            If Val(prn_DetDt.Rows(DetIndx).Item("Discount_Amount").ToString) <> 0 Then
                                CurY = CurY + TxtHgt
                                p1Font = New Font("vFontName", 9, FontStyle.Italic)
                                Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Format(Val(prn_DetDt.Rows(DetIndx).Item("Discount_Perc").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) - 10, CurY, 1, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, Format(-1 * Val(prn_DetDt.Rows(DetIndx).Item("Discount_Amount").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format_GST1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True, vFontName)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 0
                            prn_PageNo = 0
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If
                    End If


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_GST1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vFontName As String)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim I As Integer = 0
        Dim CurY2 As Single = 0
        Dim Cmp_UAMNO As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, c.Unit_Name from Other_GST_Entry_Details a LEFT OUTER JOIN Unit_Head c on a.unit_idno = c.unit_idno where a.Other_GST_Entry_Reference_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                    PrintDocument1.DefaultPageSettings.Color = True
                    PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
                    e.PageSettings.Color = True
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        Dim vPrntHeading As String = ""

        If Trim(UCase(vEntryType)) = "PURC" Then
            vPrntHeading = "PURCHASE VOUCHER"

        ElseIf Trim(UCase(vEntryType)) = "SALE" Then
            vPrntHeading = "INVOICE"

        ElseIf Trim(UCase(vEntryType)) = "CRNT" Then
            vPrntHeading = "CREDIT NOTE"

        ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
            vPrntHeading = "DEBIT NOTE"

        ElseIf Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then
            vPrntHeading = "JOBWORK INVOICE"

        ElseIf Trim(UCase(vEntryType)) = "ADV.PYMT" Then
            vPrntHeading = "ADVANCE PAYMENT"

        End If

        p1Font = New Font(vFontName, 12, FontStyle.Regular)
        If Common_Procedures.settings.CustomerCode = "1214" Then
            If Trim(UCase(vEntryType)) = "SALE" Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Vinayakar_cholatx, Drawing.Image), LMargin + 10, CurY + 10, 90, 80)

            End If
        End If
        Common_Procedures.Print_To_PrintDocument(e, Trim(UCase(vPrntHeading)), LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)


        'CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
        Cmp_UAMNO = ""
        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If
        Else
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        End If

        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
        If Trim(Cmp_Add2) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            Else
                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            End If
        Else
            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_UAM_No").ToString) <> "" Then
            Cmp_UAMNO = "UDYAM No. : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
            p1Font = New Font(vFontName, 20, FontStyle.Bold)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
            p1Font = New Font("Elephant", 22, FontStyle.Bold)
        Else
            p1Font = New Font(vFontName, 18, FontStyle.Bold)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then '---- KRG TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KRG_Logo, Drawing.Image), LMargin + 10, CurY - 5, 90, 90)
        Else
            If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                '.BackgroundImage = Image.FromStream(ms)

                                ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 15, CurY + 10, 100, 100)

                            End If

                        End Using

                    End If

                End If

            End If
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 100, CurY - 35, 85, 85)

                        End If

                    End Using

                End If

            End If

        End If



        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

        If Trim(Cmp_UAMNO) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, pFont)
        End If

        '***** GST END *****




        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For I = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If




            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + Cen1 + 10, CurY, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 10, CurY, 1, 0, p1Font)

                If Trim(ItmNm2) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "          " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 10, CurY, 1, 0, p1Font)
                End If
            End If

        End If



        CurY = CurY + TxtHgt + 2
        If Trim(UCase(vEntryType)) = "SALE" Then
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            p1Font = New Font(vFontName, 14, FontStyle.Bold)
            If Trim(UCase(vEntryType)) = "CRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Credit No.", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Debit No.", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                Common_Procedures.Print_To_PrintDocument(e, "GRN No.", LMargin + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + 10, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_GST_Entry_PrefixNo").ToString & " " & prn_HdDt.Rows(0).Item("Other_GST_Entry_No").ToString & " " & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + 120, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt

            If Trim(UCase(vEntryType)) = "CRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Credit Date.", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Debit Date.", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                Common_Procedures.Print_To_PrintDocument(e, "GRN Date.", LMargin + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Invoice Date.", LMargin + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Other_GST_Entry_Date").ToString), "dd-MM-yyyy"), LMargin + 120, CurY, 0, 0, pFont)

        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            'If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
            '    PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

            '    If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
            '    If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
            '    If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
            '    If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
            '    If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
            '    '***** GST START *****
            '    If UBound(PnAr) >= 5 Then Led_State = Trim(PnAr(5))
            '    If UBound(PnAr) >= 6 Then Led_PhNo = Trim(PnAr(6))
            '    If UBound(PnAr) >= 7 Then Led_GSTTinNo = Trim(PnAr(7))
            '    '***** GST END *****

            'Else

            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            '***** GST START *****
            Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

            Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
            '***** GST END *****

            '  End If

            Erase LedNmAr
            LedNmAr = New String(10) {}
            LInc = 0

            LInc = LInc + 1
            LedNmAr(LInc) = Led_Name

            If Trim(Led_Add1) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add1
            End If

            If Trim(Led_Add2) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add2
            End If

            If Trim(Led_Add3) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add3
            End If

            'If Trim(Led_Add4) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_Add4
            'End If
            '***** GST START *****
            If Trim(Led_State) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_State
            End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_PhNo
            End If

            If Trim(Led_GSTTinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_GSTTinNo
            End If

            'If Trim(Led_TinNo) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_TinNo
            'End If
            '***** GST END *****

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY


            'If Trim(UCase(vEntryType)) = "SALE" Then
            '    '***** GST START *****
            '    

            'Else
            If Trim(UCase(vEntryType)) = "SALE" Then
                '***** GST START *****
                Common_Procedures.Print_To_PrintDocument(e, " TO :", LMargin + 10, CurY, 0, 0, pFont)
                p1Font = New Font(vFontName, 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                '***** GST END *****

                'CurY2 = CurY

                CurY2 = BlockInvNoY
                If Trim(UCase(vEntryType)) = "SALE" Then

                    'CurY2 = CurY2 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " DELIVERY TO : ", LMargin + Cen1 + 10, CurY2, 0, 0, p1Font)
                    p1Font = New Font(vFontName, 11, FontStyle.Bold)

                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                        strHeight = e.Graphics.MeasureString("A", p1Font).Height
                        CurY2 = CurY2 + TxtHgt
                        p1Font = New Font("Calibri", 11, FontStyle.Bold)
                        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, p1Font)
                    End If

                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If
                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If

                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If

                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If


                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If


                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    Else
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)

                    End If
                    'If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                    '    strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                    '    CurX = LMargin + Cen1 + W1 + 10 + strWidth
                    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont)
                    'End If
                End If

                CurY2 = CurY2 + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, CurY2 + 10, PageWidth, CurY2 + 10)

                CurY2 = CurY2 + TxtHgt + 10
                If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 10, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 40, CurY2, 0, 0, pFont)
                End If


                If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then

                    CurY2 = CurY2 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Eway BillNo", LMargin + Cen1 + 10, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + Cen1 + W1 + 40, CurY2, 0, 0, pFont)
                End If


                If Trim(prn_HdDt.Rows(0).Item("AgentName").ToString) <> "" Then

                    CurY2 = CurY2 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "AgentName", LMargin + Cen1 + 10, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AgentName").ToString, LMargin + Cen1 + W1 + 40, CurY2, 0, 0, pFont)
                End If

                CurY = IIf(CurY2 > CurY, CurY2, CurY)

                'BlockInvNoY = BlockInvNoY + TxtHgt
                'If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 40, CurY, 0, 0, pFont)
                'End If


                'If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
                '    BlockInvNoY = BlockInvNoY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "Eway BillNo", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + Cen1 + W1 + 40, CurY, 0, 0, pFont)
                'End If


            Else


                Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
                p1Font = New Font(vFontName, 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
                '***** GST END *****


                '------------------- Invoice No Block

                '***** GST START *****

                p1Font = New Font(vFontName, 14, FontStyle.Bold)

                If Trim(UCase(vEntryType)) = "CRNT" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Credit No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Debit No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GRN No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_GST_Entry_PrefixNo").ToString & " " & prn_HdDt.Rows(0).Item("Other_GST_Entry_No").ToString & " " & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

                BlockInvNoY = BlockInvNoY + TxtHgt

                If Trim(UCase(vEntryType)) = "CRNT" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Credit Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Debit Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GRN Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Invoice Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                End If

                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Other_GST_Entry_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)


                BlockInvNoY = BlockInvNoY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, BlockInvNoY + 10, PageWidth, BlockInvNoY + 10)

                BlockInvNoY = BlockInvNoY + TxtHgt

                CurY = CurY + TxtHgt + 10
                If Trim(prn_HdDt.Rows(0).Item("Bill_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                End If


                BlockInvNoY = BlockInvNoY + TxtHgt
                If msk_BillDate.Visible = True Then
                    If Trim(prn_HdDt.Rows(0).Item("Bill_Date").ToString) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, "Bill Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bill_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                    End If
                    'End If

                Else
                    '    If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
                    '        Common_Procedures.Print_To_PrintDocument(e, "Dc No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                    '    End If

                    'End If

                    '   BlockInvNoY = BlockInvNoY + TxtHgt
                    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "2002" Then
                    '    If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                    '        Common_Procedures.Print_To_PrintDocument(e, "Dc Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                    '    




                End If

                BlockInvNoY = BlockInvNoY + TxtHgt
                If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                End If


                If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then

                    BlockInvNoY = BlockInvNoY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Eway BillNo", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt.Rows(0).Item("AgentName").ToString) <> "" Then

                    BlockInvNoY = BlockInvNoY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AgentName").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                End If
            End If





            'BlockInvNoY = BlockInvNoY + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString) <> "" Then
            '    p1Font = New Font(vFontName, 10, FontStyle.Regular)
            '    Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Issue", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY, 0, 0, p1Font)
            'End If

            '***** GST END *****

            '----------------------------



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))

            '***** GST START *****
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            '***** GST END *****

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_GST1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vFontName As String)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim vTaxPerc As Single = 0
        Dim Yax As Single
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim remks As String
        Dim remks1 As String = ""
        Dim vNoofHsnCodes As Integer = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1162" Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            End If


            If is_LastPage = True Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1162" Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Sub_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

            Erase BnkDetAr

            If is_LastPage = True Then

                If Trim(UCase(vEntryType)) = "SALE" Or Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then

                    If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                        BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                        BInc = -1
                        Yax = CurY

                        Yax = Yax + TxtHgt - 10
                        'If Val(prn_PageNo) = 1 Then
                        p1Font = New Font(vFontName, 12, FontStyle.Bold Or FontStyle.Underline)
                        Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                        'End If

                        p1Font = New Font(vFontName, 11, FontStyle.Bold)
                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                    End If

                End If

            End If


            CurY = CurY - 10

            '***** GST START *****
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(-1 * Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    If is_LastPage = True Then

            '        If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
            '            Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Else
            '            Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            '    End If
            'End If

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font(vFontName, 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
                End If

            End If




            remks = Trim(prn_HdDt.Rows(0).Item("Remarks").ToString)

            'remks1 = ""
            'If Len(remks) > 40 Then
            '    For I = 40 To 1 Step -1
            '        If Mid$(Trim(remks), I, 1) = " " Or Mid$(Trim(remks), I, 1) = "," Or Mid$(Trim(remks), I, 1) = "." Or Mid$(Trim(remks), I, 1) = "-" Or Mid$(Trim(remks), I, 1) = "/" Or Mid$(Trim(remks), I, 1) = "_" Or Mid$(Trim(remks), I, 1) = "(" Or Mid$(Trim(remks), I, 1) = ")" Or Mid$(Trim(remks), I, 1) = "\" Or Mid$(Trim(remks), I, 1) = "[" Or Mid$(Trim(remks), I, 1) = "]" Or Mid$(Trim(remks), I, 1) = "{" Or Mid$(Trim(remks), I, 1) = "}" Then Exit For
            '    Next I
            '    If I = 0 Then I = 40
            '    remks1 = Microsoft.VisualBasic.Right(Trim(remks), Len(remks) - I)
            '    remks = Microsoft.VisualBasic.Left(Trim(remks), I - 1)
            'End If


            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1333" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then
            '    If Trim(remks) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Remarks :" & remks, LMargin + 10, CurY, 0, 0, pFont)
            '    End If

            'End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(Format(Val(vTaxPerc), "#######0.00")) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            'If Trim(remks1) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(remks1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            '    'NoofDets = NoofDets + 1
            'End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then

                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(Format(Val(vTaxPerc), "#######0.00")) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(Format(Val(vTaxPerc), "#######0.00")) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            '***** GST END *****



            If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Add_Less").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            Dim rndoff As Double





            'If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString) <> 0 Then

                    rndoff = Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString)
                    If Val(rndoff) <> 0 Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 1, 0, pFont)
                        If Val(rndoff) >= 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If

                    'CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                End If

            If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt + 2
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
                CurY = CurY - 15 + 2

                p1Font = New Font(vFontName, 11, FontStyle.Bold)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL INVOICE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)

                CurY = CurY + 5

                If Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString) <> Val(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString) Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "TCS TAXABLE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TCs_name_caption").ToString & "  @ " & Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Round_Off_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font(vFontName, 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font(vFontName, 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                If Len(Rup1) > 80 Then
                    For I = 80 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 80
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If
            End If

            '***** GST START *****
            CurY = CurY + TxtHgt - 12
            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY
            CurY = CurY + 2
            Common_Procedures.Print_To_PrintDocument(e, "Remarks :" & remks, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY
            '***** GST START *****
            '=============GST SUMMARY============
            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            If vNoofHsnCodes <> 0 Then
                Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            End If
            '==========================
            '***** GST END *****


            CurY = CurY + TxtHgt - 10



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 12, FontStyle.Bold)
            Else
                p1Font = New Font(vFontName, 12, FontStyle.Bold)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            If Trim(UCase(vEntryType)) = "SALE" Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Krs_Sign, Drawing.Image), PageWidth - 120, CurY + 5, 80, 50)
                    'CurY = CurY + TxtHgt + 23
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" And Print_PDF_Status = True Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.vijay_tex_Sign2, Drawing.Image), PageWidth - 110, CurY, 90, 55)


                End If
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We Declare that this Invoice Shows the actual price of the goods", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font(vFontName, 9, FontStyle.Regular)

            'Jurs = Common_Procedures.settings.Jurisdiction
            'If Trim(Jurs) = "" Then Jurs = "Tirupur"

            Dim vJURISDICTN As String
            vJURISDICTN = Trim(prn_HdDt.Rows(0).Item("Jurisdiction").ToString)
            If Trim(vJURISDICTN) = "" Then
                vJURISDICTN = Common_Procedures.settings.Jurisdiction
            End If
            If Trim(vJURISDICTN) = "" Then vJURISDICTN = "Tirupur"

            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(vJURISDICTN) & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)

            If Print_PDF_Status = True Then
                CurY = CurY + TxtHgt - 15
                p1Font = New Font(vFontName, 9, FontStyle.Regular)

                Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)



            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_HSN_Details_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim I As Integer, NoofDets As Integer
        Dim p1Font As Font
        Dim p2Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer = 0
        Dim NoofItems_Increment As Integer
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String

        Try

            TxtHgt = TxtHgt - 1

            p2Font = New Font("Calibri", 9, FontStyle.Regular)

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 140 : SubClAr(2) = 130 : SubClAr(3) = 60 : SubClAr(4) = 95 : SubClAr(5) = 60 : SubClAr(6) = 90 : SubClAr(7) = 60
            SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Da = New SqlClient.SqlDataAdapter("Select * from Other_GST_Entry_Tax_Details Where Other_GST_Entry_Reference_Code = '" & Trim(EntryCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0

                CurY = CurY - 20

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

                    ItmNm2 = ""
                    If Len(ItmNm1) > 40 Then
                        For I = 35 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 40
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If



                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)
                    prn_DetIndx = prn_DetIndx + 1
                Loop

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            BmsInWrds = ""
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")


            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount (In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub Printing_Format_Gst2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim vNoofHsnCodes As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 65
            .Right = 50
            .Top = 40 ' 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 20 '14  ' 19  

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 45 : ClArr(2) = 230 : ClArr(3) = 70 : ClArr(4) = 50 : ClArr(5) = 75 : ClArr(6) = 50 : ClArr(7) = 75
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                '***** GST START *****
                'If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 7
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                '***** GST END *****

                Printing_Format_Gst2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0

                    CurY = CurY - TxtHgt - 10
                    If prn_Count <> 1 Then
                        CurY = CurY + TxtHgt
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
                        Common_Procedures.Print_To_PrintDocument(e, "100 % HOSIERY GOODS", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    End If

                    If prn_DetMxIndx > 0 Then

                        Do While DetIndx <= prn_DetMxIndx

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_Format_Gst2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                e.HasMorePages = True

                                Return

                            End If

                            CurY = CurY + TxtHgt

                            '***** GST START *****
                            'If DetIndx <> 1 And Val(prn_DetAr(DetIndx, 1)) <> 0 Then
                            '    CurY = CurY + 2
                            'End If
                            '***** GST END *****

                            If Trim(prn_DetAr(DetIndx, 2)) <> "" And Trim(prn_DetAr(DetIndx, 9)) = "SERIALNO" Then
                                CurY = CurY - 3
                                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 25, CurY, 0, 0, p1Font)

                            ElseIf Trim(prn_DetAr(DetIndx, 2)) <> "" And Trim(prn_DetAr(DetIndx, 9)) = "ITEM_2ND_LINE" Then
                                CurY = CurY - 3
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 25, CurY, 0, 0, pFont)
                            Else
                                '***** GST START *****
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 3), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 5), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                '***** GST END *****
                            End If

                            NoofDets = NoofDets + 1

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format_Gst2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 1
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If
                    End If


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_Gst2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String
        Dim PnAr() As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Other_GST_Entry_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Other_GST_Entry_Reference_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                    PrintDocument1.DefaultPageSettings.Color = True
                    PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
                    e.PageSettings.Color = True
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                End If

            End If
        End If

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        p1Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        'CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If
        Else
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        End If

        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
        If Trim(Cmp_Add2) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            Else
                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            End If
        Else
            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
            p1Font = New Font("Calibri", 20, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST END *****

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
                PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

                If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
                If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
                If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
                If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
                If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
                '***** GST START *****
                If UBound(PnAr) >= 5 Then Led_State = Trim(PnAr(5))
                If UBound(PnAr) >= 6 Then Led_PhNo = Trim(PnAr(6))
                If UBound(PnAr) >= 7 Then Led_GSTTinNo = Trim(PnAr(7))
                '***** GST END *****

            Else

                Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                '***** GST START *****
                Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

                Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
                '***** GST END *****

            End If

            Erase LedNmAr
            LedNmAr = New String(10) {}
            LInc = 0

            LInc = LInc + 1
            LedNmAr(LInc) = Led_Name

            If Trim(Led_Add1) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add1
            End If

            If Trim(Led_Add2) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add2
            End If

            If Trim(Led_Add3) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add3
            End If

            'If Trim(Led_Add4) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_Add4
            'End If
            '***** GST START *****
            If Trim(Led_State) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_State
            End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_PhNo
            End If

            If Trim(Led_GSTTinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_GSTTinNo
            End If

            'If Trim(Led_TinNo) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_TinNo
            'End If
            '***** GST END *****

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY

            '***** GST START *****
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '***** GST END *****


            '------------------- Invoice No Block

            '***** GST START *****
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_GST_Entry_Reference_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt + 2

            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Other_GST_Entry_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)


            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Electronic Ref.No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            End If


            BlockInvNoY = BlockInvNoY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, BlockInvNoY + 10, PageWidth, BlockInvNoY + 10)

            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
                Common_Procedures.Print_To_PrintDocument(e, "No.of Articals", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            Else
                If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Dc No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                End If

            End If

            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "2002" Then
                If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Dc Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                End If
            End If


            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString) <> "" Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Issue", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY, 0, 0, p1Font)
            End If

            '***** GST END *****

            '----------------------------


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))

            '***** GST START *****
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            '***** GST END *****

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_Gst2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim vTaxPerc As Single = 0
        Dim Yax As Single
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))


            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    'Yax = Yax + TxtHgt - 10
                    ''If Val(prn_PageNo) = 1 Then
                    'p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                    ''  Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                    ''Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    ''End If

                    'p1Font = New Font("Calibri", 11, FontStyle.Bold)
                    'BInc = BInc + 1
                    'If UBound(BnkDetAr) >= BInc Then
                    '    Yax = Yax + TxtHgt
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    'End If

                    'BInc = BInc + 1
                    'If UBound(BnkDetAr) >= BInc Then
                    '    Yax = Yax + TxtHgt - 3
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    'End If

                    'BInc = BInc + 1
                    'If UBound(BnkDetAr) >= BInc Then
                    '    Yax = Yax + TxtHgt - 3
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    'End If

                    'BInc = BInc + 1
                    'If UBound(BnkDetAr) >= BInc Then
                    '    Yax = Yax + TxtHgt - 3
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    'End If

                End If

            End If


            CurY = CurY - 10

            '***** GST START *****
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N) : N", LMargin + 15, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then

                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                End If
            End If

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then


                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
                End If
            End If


            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 20, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                If Len(Rup1) > 80 Then
                    For I = 80 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 80
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If
            End If

            '***** GST START *****
            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If

            '***** GST END *****

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            '***** GST START *****
            '=============GST SUMMARY============

            'vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            'If vNoofHsnCodes <> 0 Then
            '    Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            'End If

            '==========================
            '***** GST END *****

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "1. We Declare that this Invoice Shows the actual price of the goods", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "2. The particulars to be contained in the credit or debit note is prepared as per the Rules on Tax Invoice, ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "    Credit and Debit Notes finalised by GST Council on 18.05.2017.", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            Jurs = Common_Procedures.settings.Jurisdiction
            If Trim(Jurs) = "" Then Jurs = "Tirupur"

            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Jurs & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)

            If Print_PDF_Status = True Then
                CurY = CurY + TxtHgt - 15
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Other_GST_Entry_Tax_Details Where Other_GST_Entry_Reference_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            NoofHsnCodes = Dt1.Rows.Count
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da.Dispose()

        get_GST_Noof_HSN_Codes_For_Printing = NoofHsnCodes

    End Function


    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Other_GST_Entry_Tax_Details Where Other_GST_Entry_Reference_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Other_GST_Entry_Tax_Details Where Other_GST_Entry_Reference_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If Val(Dt2.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                        TaxPerc = Val(Dt2.Rows(0).Item("IGST_Percentage").ToString)
                    Else
                        TaxPerc = Val(Dt2.Rows(0).Item("CGST_Percentage").ToString)
                    End If
                End If
                Dt2.Clear()

            End If
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Dt2.Dispose()
        Da.Dispose()

        get_GST_Tax_Percentage_For_Printing = Format(Val(TaxPerc), "#########0.00")

    End Function

    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim I As Integer, NoofDets As Integer
        Dim p1Font As Font
        Dim p2Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer = 0
        Dim NoofItems_Increment As Integer
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String

        Try

            TxtHgt = TxtHgt - 1

            p2Font = New Font("Calibri", 9, FontStyle.Regular)

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 140 : SubClAr(2) = 130 : SubClAr(3) = 60 : SubClAr(4) = 95 : SubClAr(5) = 60 : SubClAr(6) = 90 : SubClAr(7) = 60
            SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Da = New SqlClient.SqlDataAdapter("Select * from Other_GST_Entry_Tax_Details Where Other_GST_Entry_Reference_Code = '" & Trim(EntryCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0

                CurY = CurY - 20

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

                    ItmNm2 = ""
                    If Len(ItmNm1) > 40 Then
                        For I = 35 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 40
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If



                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)
                    prn_DetIndx = prn_DetIndx + 1
                Loop

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            BmsInWrds = ""
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")


            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount (In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Tds_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Tds_Percentage.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_Reason_For_Note_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Reason_For_Note.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Reason_For_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reason_For_Note.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Reason_For_Note, Nothing, Nothing, "", "", "", "")

        If (e.KeyValue = 40 And cbo_Reason_For_Note.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            'e.Handled = True : e.SuppressKeyPress = True
            'txt_SlNo.Focus()
            If txt_IR_No.Visible Then
                txt_IR_No.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If

        If (e.KeyValue = 38 And cbo_Reason_For_Note.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            e.Handled = True : e.SuppressKeyPress = True

            If Cbo_VehicleNo.Visible Then
                Cbo_VehicleNo.Focus()
            ElseIf Cbo_Transport.Visible Then
                Cbo_Transport.Focus()
            ElseIf cbo_EntryAcName.Visible Then
                cbo_EntryAcName.Focus()
            Else
                cbo_Ledger.Focus()
            End If
        End If


    End Sub
    Private Sub cbo_Reason_For_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Reason_For_Note.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Reason_For_Note, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True

            If txt_IR_No.Visible Then
                txt_IR_No.Focus()
            Else
                txt_SlNo.Focus()
            End If

        End If
    End Sub
    Private Sub cbo_Unregister_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unregister_Type.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Unregister_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unregister_Type.KeyDown


        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unregister_Type, Nothing, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And cbo_Unregister_Type.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True : e.SuppressKeyPress = True

            If Cbo_Transport.Visible Then
                Cbo_Transport.Focus()
            ElseIf cbo_Reason_For_Note.Visible Then
                cbo_Reason_For_Note.Focus()
            End If

        End If
        If (e.KeyValue = 38 And cbo_Unregister_Type.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            e.Handled = True : e.SuppressKeyPress = True

            If cbo_EntryAcName.Visible Then
                cbo_EntryAcName.Focus()
            Else
                cbo_Ledger.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Unregister_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unregister_Type.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unregister_Type, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True

            If Cbo_Transport.Visible Then
                Cbo_Transport.Focus()
            ElseIf cbo_Reason_For_Note.Visible Then
                cbo_Reason_For_Note.Focus()
            End If


        End If
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_OnAc_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OnAc_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_OnAc_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAc_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OnAc_Type, cbo_EntryAcName, Cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_OnAc_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OnAc_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OnAc_Type, Cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_VehicleNo_GotFocus(sender As Object, e As System.EventArgs) Handles Cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Other_GST_Entry_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub Cbo_VehicleNo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_VehicleNo, Nothing, Nothing, "Other_GST_Entry_Head", "Vehicle_No", "", "")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If Cbo_Transport.Visible Then
                Cbo_Transport.Focus()
            ElseIf cbo_Unregister_Type.Visible Then
                txt_Tds_Percentage.Focus()
            ElseIf cbo_EntryAcName.Visible Then

                cbo_EntryAcName.Focus()
            End If
        End If

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Reason_For_Note.Visible Then
                cbo_Reason_For_Note.Focus()
            ElseIf txt_IR_No.Visible Then
                txt_IR_No.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If
    End Sub

    Private Sub Cbo_VehicleNo_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_VehicleNo, Nothing, "Other_GST_Entry_Head", "Vehicle_No", "", "", False)
        If Asc(e.KeyChar) = 13 Then
            If cbo_Reason_For_Note.Visible Then
                cbo_Reason_For_Note.Focus()
            ElseIf txt_IR_No.Visible Then
                txt_IR_No.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub


    Private Sub txt_TCS_TaxableValue_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_TCS_TaxableValue.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_TotalSales_Amount_Current_Year_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_TotalSales_Amount_Current_Year.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_TotalSales_Amount_Previous_Year_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_TotalSales_Amount_Previous_Year.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub btn_EDIT_TCS_TaxableValue_Click(sender As System.Object, e As System.EventArgs) Handles btn_EDIT_TCS_TaxableValue.Click
        txt_TCS_TaxableValue.Enabled = Not txt_TCS_TaxableValue.Enabled
        txt_TcsPerc.Enabled = Not txt_TcsPerc.Enabled

        If txt_TCS_TaxableValue.Enabled Then
            txt_TCS_TaxableValue.Focus()
            txt_TcsPerc.Text = "0.1"
            txt_TCS_TaxableValue.Text = lbl_Invoice_Value_Before_TCS.Text

        Else
            ' txt_addless.Focus()
            btn_save.Focus()

        End If
    End Sub

    Private Sub chk_TCS_Tax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCS_Tax.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub chk_TCSAmount_RoundOff_STS_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCSAmount_RoundOff_STS.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub msk_Date_TextChanged(sender As Object, e As System.EventArgs) Handles msk_Date.TextChanged
        msk_Date_LostFocus(sender, e)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TcsPerc_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TcsPerc.TextChanged
        NetAmount_Calculation()
    End Sub


    'GSPUR
    'GSSAL

    Private Sub get_Ledger_TotalSales()
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim TtSalAmt_CurrYr As String = 0
        Dim TtSalAmt_PrevYr As String = 0
        Dim GpCd As String = ""
        Dim Datcondt As String = ""
        Dim n As Integer = 0
        Dim I As Integer = 0
        Dim Led_ID As Integer = 0
        Dim vPrevYrCode As String = ""
        Dim NewCode As String = ""
        Dim vOrdbyNo As String


        Try



            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            vOrdbyNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_EntryNo.Text))
            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"


            '-----------TOTAL SALES

            cmd.Connection = Con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@entrydate", dtp_Date.Value.Date)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            If Led_ID <> 0 Then
                If Trim(UCase(vEntryType)) = "PURC" Then
                    If Pk_Condition = "GSPUR-" Then
                        cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount > 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCLPR-%' OR a.Voucher_Code LIKE 'GPAVP-%' OR a.Voucher_Code LIKE 'GCOPU-%'  OR a.Voucher_Code LIKE 'GYPUR-%'  OR a.Voucher_Code LIKE 'GSPUR-%' OR a.Voucher_Code LIKE 'EBPUR-%' OR a.Voucher_Code LIKE 'GITPU-%') "
                        'cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%') "
                        da = New SqlClient.SqlDataAdapter(cmd)
                        dt1 = New DataTable
                        da.Fill(dt1)


                        cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount > 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCLPR-%' OR a.Voucher_Code LIKE 'GPAVP-%' OR a.Voucher_Code LIKE 'GCOPU-%'  OR a.Voucher_Code LIKE 'GYPUR-%'  OR a.Voucher_Code LIKE 'GSPUR-%' OR a.Voucher_Code LIKE 'EBPUR-%' OR a.Voucher_Code LIKE 'GITPU-%') "
                        'cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%') "
                        da = New SqlClient.SqlDataAdapter(cmd)
                        dt1 = New DataTable
                        da.Fill(dt1)

                        TtSalAmt_CurrYr = 0
                        If dt1.Rows.Count > 0 Then
                            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                                TtSalAmt_CurrYr = Val(dt1.Rows(0).Item("BalAmount").ToString)
                            End If
                        End If
                        dt1.Clear()


                        vPrevYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
                        vPrevYrCode = Trim(Format(Val(vPrevYrCode) - 1, "00")) & "-" & Trim(Format(Val(vPrevYrCode), "00"))

                        cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount > 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(vPrevYrCode) & "' and (a.Voucher_Code LIKE 'GCLPR-%' OR a.Voucher_Code LIKE 'GPAVP-%' OR a.Voucher_Code LIKE 'GCOPU-%'  OR a.Voucher_Code LIKE 'GYPUR-%'  OR a.Voucher_Code LIKE 'GSPUR-%' OR a.Voucher_Code LIKE 'EBPUR-%' OR a.Voucher_Code LIKE 'GITPU-%') "
                        da = New SqlClient.SqlDataAdapter(cmd)
                        dt1 = New DataTable
                        da.Fill(dt1)

                        TtSalAmt_PrevYr = 0
                        If dt1.Rows.Count > 0 Then
                            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                                TtSalAmt_PrevYr = Val(dt1.Rows(0).Item("BalAmount").ToString)
                            End If
                        End If
                        dt1.Clear()

                        dt1.Dispose()
                        da.Dispose()
                        cmd.Dispose()

                        lbl_TotalSales_Amount_Current_Year.Text = Trim(Common_Procedures.Currency_Format(Math.Abs(Val(TtSalAmt_CurrYr))))
                        lbl_TotalSales_Amount_Previous_Year.Text = Trim(Common_Procedures.Currency_Format(Math.Abs(Val(TtSalAmt_PrevYr))))

                    End If

                ElseIf Trim(UCase(vEntryType)) = "SALE" Then
                    If Pk_Condition = "GSSAL-" Then

                        Common_Procedures.get_TotalSales_Value_of_Party(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, Pk_Condition, NewCode, Led_ID, vOrdbyNo, dtp_Date, lbl_TotalSales_Amount_Current_Year, lbl_TotalSales_Amount_Previous_Year)

                        'cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%' OR a.Voucher_Code LIKE 'GYPSL-%') "
                        ''cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%') "
                        'da = New SqlClient.SqlDataAdapter(cmd)
                        'dt1 = New DataTable
                        'da.Fill(dt1)

                        'TtSalAmt_CurrYr = 0
                        'If dt1.Rows.Count > 0 Then
                        '    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        '        TtSalAmt_CurrYr = Val(dt1.Rows(0).Item("BalAmount").ToString)
                        '    End If
                        'End If
                        'dt1.Clear()


                        'vPrevYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
                        'vPrevYrCode = Trim(Format(Val(vPrevYrCode) - 1, "00")) & "-" & Trim(Format(Val(vPrevYrCode), "00"))

                        'cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(vPrevYrCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%' OR a.Voucher_Code LIKE 'GYPSL-%') "
                        'da = New SqlClient.SqlDataAdapter(cmd)
                        'dt1 = New DataTable
                        'da.Fill(dt1)

                        'TtSalAmt_PrevYr = 0
                        'If dt1.Rows.Count > 0 Then
                        '    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        '        TtSalAmt_PrevYr = Val(dt1.Rows(0).Item("BalAmount").ToString)
                        '    End If
                        'End If
                        'dt1.Clear()

                        'dt1.Dispose()
                        'da.Dispose()
                        'cmd.Dispose()

                        'lbl_TotalSales_Amount_Current_Year.Text = Trim(Common_Procedures.Currency_Format(Math.Abs(Val(TtSalAmt_CurrYr))))
                        'lbl_TotalSales_Amount_Previous_Year.Text = Trim(Common_Procedures.Currency_Format(Math.Abs(Val(TtSalAmt_PrevYr))))

                    End If
                End If



            End If


        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE GETTIG TOTAL SALES....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub Printing_Format_GST_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim vNoofHsnCodes As Integer = 0
        Dim vFontName As String = ""
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim ItmNm3 As String = ""
        Dim ItmNm4 As String = ""

        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            PrintDocument1.DefaultPageSettings.PaperSize = ps
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        End If


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30 ' 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then '----Star Fabric Mills (Thekkalur)
            vFontName = "Cambria"
        Else
            vFontName = "Calibri"
        End If
        pFont = New Font(vFontName, 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18 '18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 7 '10  ' 19  

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 45 : ClArr(3) = 80 : ClArr(4) = 50 : ClArr(5) = 75 : ClArr(6) = 40 : ClArr(7) = 90 : ClArr(8) = 120
        ClArr(2) = PageWidth - (LMargin + ClArr(1) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        'ClArr(0) = 0
        'ClArr(1) = 45 : ClArr(2) = 190 : ClArr(3) = 80 : ClArr(4) = 50 : ClArr(5) = 75 : ClArr(6) = 40 : ClArr(7) = 90
        'ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                '***** GST START *****
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                '  If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 7
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                '***** GST END *****

                Printing_Format_GST_1186_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, vFontName)

                Try

                    NoofDets = 0

                    '    CurY = CurY + TxtHgt

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_Format_GST_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False, vFontName)
                                e.HasMorePages = True

                                Return

                            End If

                            CurY = CurY + TxtHgt


                            ItmNm1 = prn_DetDt.Rows(DetIndx).Item("Item_Particulars").ToString
                            ItmNm2 = ""
                            ItmNm3 = ""
                            ItmNm4 = ""
                            If Len(ItmNm1) > 30 Then
                                For I = 30 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 30
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Sl_No").ToString, LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Sac_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Gst_Perc").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Quantity").ToString), "##########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "#############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1
                            If Trim(ItmNm2) <> "" Then

                                If Len(ItmNm2) > 30 Then
                                    For I = 30 To 1 Step -1
                                        If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 30
                                    ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                    ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                                End If

                                If Len(ItmNm3) > 30 Then
                                    For I = 30 To 1 Step -1
                                        If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 30
                                    ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                                    ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                                End If

                                CurY = CurY + TxtHgt - 3
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1

                                If Trim(ItmNm3) <> "" Then
                                    CurY = CurY + TxtHgt - 3
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                If Trim(ItmNm4) <> "" Then
                                    CurY = CurY + TxtHgt - 3
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                            End If
                            If Val(prn_DetDt.Rows(DetIndx).Item("Discount_Amount").ToString) <> 0 Then
                                CurY = CurY + TxtHgt
                                p1Font = New Font("vFontName", 9, FontStyle.Italic)
                                Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Format(Val(prn_DetDt.Rows(DetIndx).Item("Discount_Perc").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) - 10, CurY, 1, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, Format(-1 * Val(prn_DetDt.Rows(DetIndx).Item("Discount_Amount").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format_GST_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True, vFontName)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 0
                            prn_PageNo = 0
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If
                    End If


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_GST_1186_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vFontName As String)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_Add3 As String, city As String = ""
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim DelvLed_Name As String, DelvLed_Add1 As String, DelvLed_Add2 As String, DelvLed_Add3 As String, DelvLed_Add4 As String, DelvLed_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String
        Dim DelvLed_GSTTinNo As String, DelvLed_State As String
        Dim LedNmAr(10) As String

        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim DelvLed_PhNo As String
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0

        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim I As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, c.Unit_Name from Other_GST_Entry_Details a LEFT OUTER JOIN Unit_Head c on a.unit_idno = c.unit_idno where a.Other_GST_Entry_Reference_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                    PrintDocument1.DefaultPageSettings.Color = True
                    PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
                    e.PageSettings.Color = True
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        Dim vPrntHeading As String = ""

        If Trim(UCase(vEntryType)) = "PURC" Then
            vPrntHeading = "PURCHASE VOUCHER"

        ElseIf Trim(UCase(vEntryType)) = "SALE" Then
            vPrntHeading = "INVOICE"

        ElseIf Trim(UCase(vEntryType)) = "CRNT" Then
            vPrntHeading = "CREDIT NOTE"

        ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
            vPrntHeading = "DEBIT NOTE"
        ElseIf Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then


            vPrntHeading = "JOBWORK INVOICE"

        ElseIf Trim(UCase(vEntryType)) = "ADV.PYMT" Then
            vPrntHeading = "ADVANCE PAYMENT"

        End If

        p1Font = New Font(vFontName, 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, Trim(UCase(vPrntHeading)), LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)


        'CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""


        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            City = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            If InStr(1, Trim(UCase(Cmp_Name)), "UNITED") > 0 And InStr(1, Trim(UCase(Cmp_Name)), "WEAVES") > 0 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)
            Else
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Gounder_Traders_Logo, Drawing.Image), PageWidth - 150, CurY, 120, 100)
            End If
        End If



        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 300, CurY + 10, 90, 90)

                        End If

                    End Using

                End If

            End If

        End If



        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & "," & City, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "/" & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "/ " & Cmp_Email, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)




        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For I = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + Cen1 + 10, CurY, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString) <> "" Then

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 10, CurY, 1, 0, p1Font)

                If Trim(ItmNm2) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "          " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 10, CurY, 1, 0, p1Font)
                End If
            End If

        End If



        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            DelvLed_Name = "" : DelvLed_Add1 = "" : DelvLed_Add2 = "" : DelvLed_Add3 = "" : DelvLed_Add4 = "" : DelvLed_TinNo = "" : DelvLed_PhNo = "" : DelvLed_GSTTinNo = "" : DelvLed_State = ""

            'If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
            '    PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

            '    If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
            '    If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
            '    If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
            '    If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
            '    If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
            '    '***** GST START *****
            '    If UBound(PnAr) >= 5 Then Led_State = Trim(PnAr(5))
            '    If UBound(PnAr) >= 6 Then Led_PhNo = Trim(PnAr(6))
            '    If UBound(PnAr) >= 7 Then Led_GSTTinNo = Trim(PnAr(7))
            '    '***** GST END *****

            'Else

            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            '***** GST START *****
            Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

            Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
            '***** GST END *****

            '  End If


            DelvLed_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString)

            DelvLed_Add1 = Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString)
            DelvLed_Add2 = Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString)
            DelvLed_Add3 = Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString)

            ' DelvLed_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_TinNo").ToString)
            '  If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PhoneNo").ToString) <> "" Then DelvLed_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PhoneNo").ToString)

            DelvLed_State = Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString)
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then DelvLed_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString)




            Erase LedNmAr
            LedNmAr = New String(10) {}
            LInc = 0

            LInc = LInc + 1
            LedNmAr(LInc) = Led_Name

            If Trim(Led_Add1) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add1
            End If

            If Trim(Led_Add2) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add2
            End If

            If Trim(Led_Add3) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add3
            End If

            'If Trim(Led_Add4) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_Add4
            'End If
            '***** GST START *****
            If Trim(Led_State) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_State
            End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_PhNo
            End If

            If Trim(Led_GSTTinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_GSTTinNo
            End If

            'If Trim(Led_TinNo) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_TinNo
            'End If
            '***** GST END *****

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY

            '***** GST START *****
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font(vFontName, 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '***** GST END *****


            '------------------- Invoice No Block

            '***** GST START *****
            p1Font = New Font(vFontName, 14, FontStyle.Bold)

            If Trim(UCase(vEntryType)) = "CRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Credit No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Debit No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                Common_Procedures.Print_To_PrintDocument(e, "GRN No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            End If


            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_GST_Entry_PrefixNo").ToString & " " & prn_HdDt.Rows(0).Item("Other_GST_Entry_No").ToString & " " & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY - 2, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt

            If Trim(UCase(vEntryType)) = "CRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Credit Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Debit Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                Common_Procedures.Print_To_PrintDocument(e, "GRN Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Invoice Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Other_GST_Entry_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY + 2, 0, 0, pFont)



            'BlockInvNoY = BlockInvNoY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, BlockInvNoY + 10, PageWidth, BlockInvNoY + 10)



            'If Trim(prn_HdDt.Rows(0).Item("Bill_No").ToString) <> "" Then
            '    BlockInvNoY = BlockInvNoY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)

            '    If Trim(prn_HdDt.Rows(0).Item("Bill_Date").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Bill Date", PageWidth - 300, BlockInvNoY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", PageWidth - 250, BlockInvNoY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bill_Date").ToString), "dd-MM-yyyy"), PageWidth - 260, BlockInvNoY, 0, 0, pFont)
            '    End If
            'End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then


                BlockInvNoY = BlockInvNoY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, BlockInvNoY + 10, PageWidth, BlockInvNoY + 10)

                BlockInvNoY = BlockInvNoY + TxtHgt

                Erase LedNmAr
                LedNmAr = New String(10) {}
                LInc = 0

                LInc = LInc + 1
                LedNmAr(LInc) = DelvLed_Name

                If Trim(Led_Add1) <> "" Then
                    LInc = LInc + 1
                    LedNmAr(LInc) = DelvLed_Add1
                End If

                If Trim(Led_Add2) <> "" Then
                    LInc = LInc + 1
                    LedNmAr(LInc) = DelvLed_Add2
                End If

                If Trim(Led_Add3) <> "" Then
                    LInc = LInc + 1
                    LedNmAr(LInc) = DelvLed_Add3
                End If

                If Trim(Led_State) <> "" Then
                    LInc = LInc + 1
                    LedNmAr(LInc) = DelvLed_State
                End If

                'If Trim(Led_PhNo) <> "" Then
                '    LInc = LInc + 1
                '    LedNmAr(LInc) = DelvLed_PhNo
                'End If

                If Trim(Led_GSTTinNo) <> "" Then
                    LInc = LInc + 1
                    LedNmAr(LInc) = DelvLed_GSTTinNo
                End If

                Common_Procedures.Print_To_PrintDocument(e, "SHIPPED TO : ", LMargin + Cen1 + 5, BlockInvNoY, 0, 0, pFont)
                p1Font = New Font(vFontName, 11, FontStyle.Bold)
                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)

                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)

                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)

                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)

                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)

                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)


                'CurY = CurY + TxtHgt
                ''e.Graphics.DrawLine(Pens.Red, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), PageWidth, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), BlockInvNoY)
                'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin, CurY)
                '    Common_Procedures.Print_To_PrintDocument(e, "Delivery To", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)

            End If

            If Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then

                CurY = CurY + TxtHgt
                'e.Graphics.DrawLine(Pens.Red, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), PageWidth, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), BlockInvNoY)
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin, CurY)

                CurY = CurY + TxtHgt - 5
                Dim VTrans_Name As String = ""
                VTrans_Name = Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)

                Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + W2, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(VTrans_Name), LMargin + W1 + W2 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + 10, CurY, 0, 0, pFont)
                '   Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + W2, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + W2 + 10, CurY, 0, 0, pFont)
            End If

            'LMargin +W2 + 10, CurY, 0, 0, pFont)
            'txt_EWBNo.Text = dt1.Rows(0).Item("Eway_BillNo").ToString



            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Eway BillNo ", LMargin + 10, CurY, 0, 0, pFont)
                '   Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + W2, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + W1 + W2 + 10, CurY, 0, 0, pFont)
            End If


            'BlockInvNoY = BlockInvNoY + TxtHgt

            'If Trim(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> "" Then
            '    Dim VTrans_Name As String = ""
            '    VTrans_Name = Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)

            '    Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(VTrans_Name), LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            'End If

            'If msk_BillDate.Visible = True Then
            '    If Trim(prn_HdDt.Rows(0).Item("Bill_Date").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Bill Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bill_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            '    End If
            'End If



            'Else
            '    If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Dc No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            '    End If

            'End If

            '   BlockInvNoY = BlockInvNoY + TxtHgt
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "2002" Then
            '    If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Dc Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            '    End If
            'End If

            'BlockInvNoY = BlockInvNoY + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            '    '   Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            'End If


            'BlockInvNoY = BlockInvNoY + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString) <> "" Then
            '    p1Font = New Font(vFontName, 10, FontStyle.Regular)
            '    Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Issue", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY, 0, 0, p1Font)
            'End If

            '***** GST END *****

            '----------------------------


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))

            '***** GST START *****
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            '***** GST END *****

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_GST_1186_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vFontName As String)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim vTaxPerc As Single = 0
        Dim Yax As Single
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim remks As String
        Dim remks1 As String = ""
        Dim vNoofHsnCodes As Integer = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1162" Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            End If


            If is_LastPage = True Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1162" Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Sub_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

            Erase BnkDetAr

            If is_LastPage = True Then

                If Trim(UCase(vEntryType)) = "SALE" Or Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then

                    If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                        BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                        BInc = -1
                        Yax = CurY

                        Yax = Yax + TxtHgt - 10
                        'If Val(prn_PageNo) = 1 Then
                        p1Font = New Font(vFontName, 12, FontStyle.Bold Or FontStyle.Underline)
                        Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                        'End If

                        p1Font = New Font(vFontName, 11, FontStyle.Bold)
                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                    End If

                End If

            End If


            CurY = CurY - 10

            '***** GST START *****
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(-1 * Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    If is_LastPage = True Then

            '        If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
            '            Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Else
            '            Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            '    End If
            'End If

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font(vFontName, 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
                End If

            End If




            remks = Trim(prn_HdDt.Rows(0).Item("Remarks").ToString)

            'remks1 = ""
            'If Len(remks) > 40 Then
            '    For I = 40 To 1 Step -1
            '        If Mid$(Trim(remks), I, 1) = " " Or Mid$(Trim(remks), I, 1) = "," Or Mid$(Trim(remks), I, 1) = "." Or Mid$(Trim(remks), I, 1) = "-" Or Mid$(Trim(remks), I, 1) = "/" Or Mid$(Trim(remks), I, 1) = "_" Or Mid$(Trim(remks), I, 1) = "(" Or Mid$(Trim(remks), I, 1) = ")" Or Mid$(Trim(remks), I, 1) = "\" Or Mid$(Trim(remks), I, 1) = "[" Or Mid$(Trim(remks), I, 1) = "]" Or Mid$(Trim(remks), I, 1) = "{" Or Mid$(Trim(remks), I, 1) = "}" Then Exit For
            '    Next I
            '    If I = 0 Then I = 40
            '    remks1 = Microsoft.VisualBasic.Right(Trim(remks), Len(remks) - I)
            '    remks = Microsoft.VisualBasic.Left(Trim(remks), I - 1)
            'End If


            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1333" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then
            '    If Trim(remks) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Remarks :" & remks, LMargin + 10, CurY, 0, 0, pFont)
            '    End If

            'End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            'If Trim(remks1) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(remks1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            '    'NoofDets = NoofDets + 1
            'End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then

                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            '***** GST END *****



            If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Add_Less").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            Dim rndoff As Double


            If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString) <> 0 Then

                    rndoff = Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString)
                    If Val(rndoff) <> 0 Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 1, 0, pFont)
                        If Val(rndoff) >= 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If

                    'CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                End If

                CurY = CurY + TxtHgt + 2
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
                CurY = CurY - 15 + 2

                p1Font = New Font(vFontName, 11, FontStyle.Bold)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL INVOICE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)

                CurY = CurY + 5

                If Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString) <> Val(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString) Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "TCS TAXABLE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TCs_name_caption").ToString & "  @ " & Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Round_Off_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font(vFontName, 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font(vFontName, 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                If Len(Rup1) > 80 Then
                    For I = 80 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 80
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If
            End If

            '***** GST START *****
            CurY = CurY + TxtHgt - 12
            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY
            CurY = CurY + 2
            Common_Procedures.Print_To_PrintDocument(e, "Remarks :" & remks, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY
            '***** GST START *****
            '=============GST SUMMARY============
            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            If vNoofHsnCodes <> 0 Then
                Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            End If
            '==========================
            '***** GST END *****


            CurY = CurY + TxtHgt - 10



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 12, FontStyle.Bold)
            Else
                p1Font = New Font(vFontName, 12, FontStyle.Bold)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            If Trim(UCase(vEntryType)) = "SALE" Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Krs_Sign, Drawing.Image), PageWidth - 120, CurY + 5, 80, 50)
                    'CurY = CurY + TxtHgt + 23


                End If
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "1. We Declare that this Invoice Shows the actual price of the goods.", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            If Trim(UCase(vEntryType)) = "CRNT" Or Trim(UCase(vEntryType)) = "DRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "2. The particulars to be contained in the credit or debit note is prepared  ", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "as per the Rules on Tax Invoice, Credit and Debit Notes finalised by GST Council on 18.05.2017.", LMargin + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, pFont)

            End If

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font(vFontName, 9, FontStyle.Regular)

            Jurs = Common_Procedures.settings.Jurisdiction
            If Trim(Jurs) = "" Then Jurs = "Tirupur"

            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Jurs & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)

            If Print_PDF_Status = True Then
                CurY = CurY + TxtHgt - 15
                p1Font = New Font(vFontName, 9, FontStyle.Regular)

                Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)



            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If SaveAll_STS = True Then
            save_record()
            If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
                Timer1.Enabled = False
                SaveAll_STS = False
                MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Else
                movenext_record()

            End If
        ElseIf DeleteAll_STS = True Then
            delete_record()
            If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
                Timer1.Enabled = False
                DeleteAll_STS = False
                new_record()
                MessageBox.Show("All entries Deleted Successfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Else
                movenext_record()

            End If
        End If
    End Sub

    Private Sub cbo_OnAc_Type_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_OnAc_Type.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_OnAc_Type.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Btn_Qr_Code_Add_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Add.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pic_IRN_QRCode_Image.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub Btn_Qr_Code_Close_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Close.Click
        pic_IRN_QRCode_Image.BackgroundImage = Nothing
    End Sub

    Private Sub btn_EInvoice_Generation_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click
        'rtbeInvoiceResponse.Text = ""
        'txt_EWBNo.Text = txt_Electronic_RefNo.Text
        grp_EInvoice.Visible = True
        grp_EInvoice.BringToFront()
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2
        btn_CheckConnectivity1.Enabled = False
        btn_CheckConnectivity1.Visible = False
    End Sub

    Private Sub btn_CheckConnectivity1_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity1.Click

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GetAuthToken(rtbeInvoiceResponse)
        'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg
    End Sub

    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Other_GST_Entry_Head Where Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Other_GST_Entry_Head Where Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) > 0"
        c = Cmd.ExecuteScalar

        If c > 0 Then
            Dim k As Integer = MsgBox("An IRN Has been Generated already for this Invoice. Do you want to Delete the Previous IRN ?", vbYesNo, "IRN Generated")
            If k = vbNo Then
                MsgBox("Cannot Create a New IRN When there is an IRN generated already !", vbOKOnly, "Duplicate IRN ")
                Exit Sub
            Else

            End If
        End If

        Dim tr As SqlClient.SqlTransaction

        tr = con.BeginTransaction
        Cmd.Transaction = tr

        Try

            Cmd.CommandText = "Delete from e_Invoice_Head  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Delete from e_Invoice_Details  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Head (e_Invoice_No ,        e_Invoice_date ,           Buyer_IdNo ,    Consignee_IdNo ,     Assessable_Value  ,   CGST ,           SGST  ,     IGST   ,  Cess   ,   State_Cess ,   Round_Off        ,  Nett_Invoice_Value  ,   Ref_Sales_Code      ,                        Other_Charges ,                         Dispatcher_Idno ) " &
                                            "Select  Other_GST_Entry_RefNo ,    Other_GST_Entry_Date,       Ledger_IdNo,    Ledger_IdNo,         Total_Taxable_Value, CGST_Amount, SGST_Amount, IGST_Amount ,   0,          0,          Round_Off_Amount,        Net_Amount,     '" & Trim(NewCode) & "',( ISNULL(TCS_Amount,0) + ISNULL(Add_Less,0) ) as OtherCharges , 0   from Other_GST_Entry_Head where Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Details  ( Sl_No,     IsService   ,       Product_Description                          ,   HSN_Code     ,       Batch_Details,   Quantity    ,   Unit        ,   Unit_Price  ,        Total_Amount,                                       Discount                                ,      Assessable_Amount  ,   GST_Rate,  SGST_Amount , IGST_Amount , CGST_Amount , Cess_rate ,  Cess_Amount  , CessNonAdvlAmount ,  State_Cess_Rate , State_Cess_Amount , StateCessNonAdvlAmount , Other_Charge , Total_Item_Value ,   AttributesDetails      ,           Ref_Sales_Code) " &
                                                  " Select    a.Sl_No,     0 as IsServc,         (a.Item_Particulars ) as producDescription , a.Hsn_Sac_Code ,  '' as batchdetails,    a.Quantity ,   'MTR' as UOM,      a.Rate   ,      a.Amount      ,         (a.Discount_Amount + a.Footer_Cash_Discount_Amount ) as DiscountAmount,         a.Taxable_Value , a.Gst_Perc,  0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt,              0      ,  0 as TotItemVal ,   '' as AttributesDetails, '" & Trim(NewCode) & "'  " &
                               " from Other_GST_Entry_Details a INNER JOIN Other_GST_Entry_Head b  ON a.Other_GST_Entry_Reference_Code =  b.Other_GST_Entry_Reference_Code" &
                                " Where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()


            'Cmd.CommandText = "Insert into e_Invoice_Details  ( Sl_No,     IsService   ,       Product_Description                          ,   HSN_Code     ,       Batch_Details,   Quantity    ,   Unit        ,   Unit_Price  ,       Total_Amount                  ,                                       Discount                                                 ,      Assessable_Amount  ,   GST_Rate,  SGST_Amount , IGST_Amount , CGST_Amount , Cess_rate ,  Cess_Amount  , CessNonAdvlAmount ,  State_Cess_Rate , State_Cess_Amount , StateCessNonAdvlAmount ,                                         Other_Charge ,                                   Total_Item_Value ,   AttributesDetails ,           Ref_Sales_Code) " &
            '                                      " Select    a.Sl_No,     0 as IsServc,         (a.Item_Particulars ) as producDescription , a.Hsn_Sac_Code ,  '' as batchdetails,    a.Quantity ,   'MTR' as UOM,      a.Rate   ,     ( a.Amount - a.Discount_Amount) ,         (b.Total_DiscountAmount + a.Discount_Amount + b.CashDiscount_Amount ) as DiscountAmount,         a.Taxable_Value , a.Gst_Perc,  0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt,              0,                 0 as TotItemVal,       '' as AttributesDetails, '" & Trim(NewCode) & "'  " &
            '                   " from Other_GST_Entry_Details a INNER JOIN Other_GST_Entry_Head b  ON a.Other_GST_Entry_Reference_Code =  b.Other_GST_Entry_Reference_Code" &
            '                    " Where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()


            'Cmd.CommandText = "Insert into e_Invoice_Details  ( Sl_No     ,     IsService       ,       Product_Description     ,                HSN_Code        ,       Batch_Details       ,   Quantity    ,   Unit    ,   Unit_Price  ,       Total_Amount               ,                                       Discount  ,                                               Assessable_Amount ,         GST_Rate ,  SGST_Amount , IGST_Amount , CGST_Amount , Cess_rate ,  Cess_Amount  , CessNonAdvlAmount ,  State_Cess_Rate , State_Cess_Amount , StateCessNonAdvlAmount ,                                         Other_Charge ,                                   Total_Item_Value ,   AttributesDetails ,           Ref_Sales_Code) " &
            '                                      " Select a.Sl_No,           0 as IsServc,         (a.Item_Particulars ) as producDescription , a.Hsn_Sac_Code ,  '' as batchdetails,    a.Quantity  ,      'MTR' as UOM,      a.Rate,     ( a.Amount - a.Discount_Amount) ,         (b.Total_DiscountAmount + a.Discount_Amount + b.CashDiscount_Amount ) as DiscountAmount,         a.Taxable_Value ,              a.Gst_Perc,  0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt,   (CASE WHEN a.sl_no = 1 then b.Add_Less else 0 end ) as OthChrg,       0 as TotItemVal,       '' as AttributesDetails, '" & Trim(NewCode) & "'  " &
            '                   " from Other_GST_Entry_Details a INNER JOIN Other_GST_Entry_Head b  ON a.Other_GST_Entry_Reference_Code =  b.Other_GST_Entry_Reference_Code" &
            '                    " Where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()

            'Cmd.CommandText = "Insert into e_Invoice_Details  ( Sl_No     ,     IsService       ,       Product_Description     ,                HSN_Code        ,       Batch_Details       ,   Quantity    ,   Unit    ,   Unit_Price  ,              Total_Amount ,                                                                       Discount  ,                                                                               Assessable_Amount ,                                                            GST_Rate , SGST_Amount , IGST_Amount , CGST_Amount , Cess_rate ,  Cess_Amount  , CessNonAdvlAmount ,  State_Cess_Rate , State_Cess_Amount , StateCessNonAdvlAmount ,                  Other_Charge ,                                   Total_Item_Value ,   AttributesDetails ,           Ref_Sales_Code) " &
            '                                      " Select a.Sl_No,           0 as IsServc,         (a.Item_Particulars ) as producDescription , a.Hsn_Sac_Code ,  '' as batchdetails,    a.Quantity  ,      'MTR' as UOM,      a.Rate,       (CASE WHEN a.sl_no = 1 then (a.Amount + b.Add_less ) ELSE 0 END ) , (CASE WHEN a.sl_no = 1 then (b.Total_DiscountAmount + a.Discount_Amount ) ELSE 0 END ) as DiscountAmount ,      (CASE WHEN a.sl_no = 1 then (a.Amount + a.Taxable_Value  ) ELSE 0 END )          , a.Gst_Perc, 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt,   (CASE WHEN a.sl_no =1 then b.Add_Less else 0 end ) as OthChrg,       0 as TotItemVal,       '' as AttributesDetails, '" & Trim(NewCode) & "'  " &
            '                   " from Other_GST_Entry_Details a INNER JOIN Other_GST_Entry_Head b  ON a.Other_GST_Entry_Reference_Code =  b.Other_GST_Entry_Reference_Code" &
            '                    " Where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub
        End Try

        Dim vType As String = ""

        If Trim(UCase(vEntryType)) = "CRNT" Then
            vType = "CRN"
        ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
            vType = "DBN"
        Else
            vType = "INV"
        End If

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", Trim(Pk_Condition), vType)

    End Sub

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)

    End Sub

    Private Sub btn_Refresh_eInvoice_Info_Click(sender As Object, e As EventArgs) Handles btn_refresh.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select E_Invoice_QR_Image,E_Invoice_IRNO,E_Invoice_ACK_No,E_Invoice_ACK_Date,E_Invoice_Cancelled_Status FROM Other_GST_Entry_Head WHERE Other_GST_Entry_Reference_Code = '" & NewCode & "'", con)

        Dim DT As New DataTable

        da.Fill(DT)

        If DT.Rows.Count > 0 Then

            pic_IRN_QRCode_Image.BackgroundImage = Nothing
            txt_eInvoiceAckNo.Text = ""
            txt_eInvoiceAckDate.Text = ""
            txt_eInvoice_CancelStatus.Text = ""

            txt_eInvoiceNo.Text = Trim(DT.Rows(0).Item("E_Invoice_IRNO").ToString)

            If Not IsDBNull(DT.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(DT.Rows(0).Item("E_Invoice_ACK_No").ToString)
            If Not IsDBNull(DT.Rows(0).Item("E_Invoice_ACK_Date")) Then txt_eInvoiceAckDate.Text = Trim(DT.Rows(0).Item("E_Invoice_ACK_Date").ToString)
            If Not IsDBNull(DT.Rows(0).Item("E_Invoice_Cancelled_Status")) Then txt_eInvoice_CancelStatus.Text = IIf(DT.Rows(0).Item("E_Invoice_Cancelled_Status") = True, "Cancelled", "Active")

            If IsDBNull(DT.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(DT.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)
                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image.BackgroundImage = Image.FromStream(ms)

                        End If

                    End Using
                End If
            End If

        End If

    End Sub

    Private Sub txt_eInvoiceNo_TextChanged(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.TextChanged
        txt_IR_No.Text = txt_eInvoiceNo.Text
    End Sub

    Private Sub btn_Get_QR_Code_Click(sender As Object, e As EventArgs) Handles btn_Get_QR_Code.Click
        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con

        CMD.CommandText = "DELETE FROM " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_refresh where IRN = '" & txt_eInvoiceNo.Text & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = " INSERT INTO " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_Refresh ([IRN] ,[ACK_No] , [DOC_No] , [SEARCH_BY]  , [COMPANY_IDNO],[Update_Table] ,[Update_table_Unique_Code] ) VALUES " &
                          "('" & txt_eInvoiceNo.Text & "' ,'','','I'," & Val(Common_Procedures.CompGroupIdNo).ToString & ",'Other_GST_Entry_Head', 'E_Invoice_IRNO')"
        CMD.ExecuteNonQuery()

        Shell(Application.StartupPath & "\Refresh_IRN.EXE")

        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'Dim einv1 As New eInvoice1(Val(lbl_Company.Tag))
        'einv1.RefresheInvoiceInfoByIRN(txt_IR_No.Text, NewCode, Con, rtbeInvoiceResponse, pb_IRNQRC, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "ClothSales_Invoice_Head", "ClothSales_Invoice_Code")
    End Sub

    Private Sub txt_IR_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_IR_No.KeyDown
        If e.KeyCode = 38 Then
            If cbo_Reason_For_Note.Visible = True Then
                cbo_Reason_For_Note.Focus()
            Else
                Cbo_VehicleNo.Focus()
            End If
        End If
        If e.KeyCode = 40 Then
            'txt_SlNo.Focus()
            txt_EWayBillNo.Focus()
        End If
    End Sub

    Private Sub txt_IR_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_IR_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_EWayBillNo.Focus()
        End If
    End Sub
    Private Sub txt_EWayBillNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_EWayBillNo.KeyDown
        If e.KeyCode = 38 Then
            txt_IR_No.Focus()
        End If
        If e.KeyCode = 40 Then
            txt_SlNo.Focus()
        End If
    End Sub

    Private Sub txt_EWayBillNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_EWayBillNo.KeyPress

        If Asc(e.KeyChar) = 13 Then
            txt_SlNo.Focus()
        End If
    End Sub

    Private Sub btn_Generate_EWB_IRN_Click(sender As Object, e As EventArgs) Handles btn_Generate_EWB_IRN.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Other_GST_Entry_Details Where Other_GST_Entry_Reference_Code = '" & NewCode & "'"
        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Other_GST_Entry_Head Where Other_GST_Entry_Reference_Code = '" & NewCode & "' and (Len(Eway_BillNo) >0 or Len(E_Invoice_IRNO) = 0 OR E_Invoice_IRNO IS NULL )"
        c = Cmd.ExecuteScalar

        If c > 0 Then
            'Dim k As Integer = MsgBox("EWB Has been Generated already for this Invoice. Do you want to Delete the Previous IRN ?", vbYesNo, "IRN Generated")
            'If k = vbNo Then
            MsgBox("Cannot Create a New EWB When there is an EWB generated already and/or an IRN has not been generated!", vbOKOnly, "Duplicate EWB ")
            Exit Sub
            'Else
            'End If
        End If

        Dim tr As SqlClient.SqlTransaction

        tr = con.BeginTransaction
        Cmd.Transaction = tr

        Try

            Cmd.CommandText = "Delete from EWB_By_IRN  where InvCode = '" & NewCode & "'"
            Cmd.ExecuteNonQuery()


            Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]         ,     	[TransID]        ,	            [TransMode]  ,	[TransDocNo]    , [TransDocDate] ,	        [VehicleNo]        ,                [Distance]       ,	        [VehType] ,	           [TransName]         ,    [InvCode]           ,         Company_Idno ,     Company_Pincode,            Shipped_To_Idno     ,     Shipped_To_Pincode               ) " &
                                " Select                A.E_Invoice_IRNO  ,  ISNULL(t.Ledger_GSTINNo, '' ) ,        '1'    ,            ''   ,           Null         ,       a.Vehicle_No ,                       L.Distance  ,                   'R'    ,        T.Ledger_Mainname ,        '" & Trim(NewCode) & "' ,    tZ.Company_IdNo,       tZ.Company_PinCode,      a.Ledger_IdNo ,                  L.Pincode    " &
                                    " from Other_GST_Entry_Head a INNER JOIN Company_Head tZ on a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo  Where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "'"

            Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()

            MessageBox.Show(ex.Message + " Cannot Generate IRN.", "ERROR WHILE GENERATING E-WAY BILL BY IRN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Exit Sub

        End Try


        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateEWBByIRN(NewCode, rtbeInvoiceResponse, txt_eWayBill_No, txt_EWB_Date, txt_EWB_ValidUpto, con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", txt_EWB_Canellation_Reason, txt_EWB_Cancel_Status, Pk_Condition)

        Cmd.CommandText = "DELETE FROM EWB_By_IRN WHERE INVCODE = '" & NewCode & "'"
        Cmd.ExecuteNonQuery()


    End Sub

    Private Sub btn_Print_EWB_Click(sender As Object, e As EventArgs) Handles btn_Print_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_eWayBill_No.Text, rtbeInvoiceResponse, 0)
    End Sub

    Private Sub btn_Cancel_EWB_Click(sender As Object, e As EventArgs) Handles btn_Cancel_EWB.Click
        If Len(Trim(txt_EWB_Canellation_Reason.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))

        einv.Cancel_EWB_IRN(NewCode, txt_eWayBill_No.Text, rtbeInvoiceResponse, txt_eInvoice_CancelStatus, con, "Other_GST_Entry_Head", "Other_GST_Entry_Reference_Code", txt_EWB_Canellation_Reason.Text)
    End Sub
    Private Sub txt_eWayBill_No_TextChanged(sender As Object, e As EventArgs) Handles txt_eWayBill_No.TextChanged
        'txt_eWayBill_No.Text = txt_eWayBill_No.Text


        txt_EWayBillNo.Text = txt_eWayBill_No.Text
        txt_EWBNo.Text = txt_eWayBill_No.Text
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Transport, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_OnAc_Type.Visible Then
                cbo_OnAc_Type.Focus()
            ElseIf cbo_Unregister_Type.Visible Then
                cbo_Unregister_Type.Focus()
            Else
                cbo_EntryAcName.Focus()
            End If

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Cbo_VehicleNo.Visible Then
                Cbo_VehicleNo.Focus()
            ElseIf cbo_Reason_For_Note.Visible Then
                cbo_Reason_For_Note.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Transport, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Cbo_VehicleNo.Visible Then
                Cbo_VehicleNo.Focus()
            ElseIf cbo_Reason_For_Note.Visible Then
                cbo_Reason_For_Note.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(sender As Object, e As EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_Ledger, cbo_EntryAcName, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_EntryAcName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 ) ) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_DeliveryTo.Tag)) <> Trim(UCase(cbo_DeliveryTo.Text)) Then
                cbo_DeliveryTo.Tag = cbo_DeliveryTo.Text
                get_Ledger_City_Name()
            End If
        End If
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_InvoiceSufixNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_InvoiceSufixNo, txt_Remarks, msk_Date, "", "", "", "")
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_InvoiceSufixNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_InvoiceSufixNo, msk_Date, "", "", "", "", False)
    End Sub


    Private Sub cbo_Agent_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.Enter
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
        cbo_Agent.Tag = cbo_Agent.Text
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_AddLess, txt_Remarks, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            txt_Remarks.Focus()
        End If
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "AGENT"
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub



    Private Sub txt_AddLess_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_Reason_For_Note_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Reason_For_Note.SelectedIndexChanged

    End Sub

    Private Sub Cbo_Transport_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_Transport.SelectedIndexChanged

    End Sub

    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click

        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - grp_EInvoice.Width) / 2
        Grp_EWB.Top = (Me.Height - grp_EInvoice.Height) / 2 + 200

    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()


        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select EWB_NO from Other_GST_Entry_Head where Other_GST_Entry_Reference_Code = '" & NewCode & "'", con)
        Dim dt As New DataTable

        da.Fill(dt)

        If dt.Rows.Count = 0 Then
            MessageBox.Show("Please Save the Invoice before proceeding to generate EWB", "Please SAVE", MessageBoxButtons.OKCancel)
            dt.Clear()
            Exit Sub
        End If

        If Not IsDBNull(dt.Rows(0).Item(0)) Then
            If Len(Trim(dt.Rows(0).Item(0))) > 0 Then
                MessageBox.Show("EWB has been generated for this invoice already", "Redundant Request", MessageBoxButtons.OKCancel)
                dt.Clear()
                Exit Sub
            End If
        End If

        dt.Clear()

        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con

        CMD.CommandText = "Delete from EWB_Head Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()


        If Len(Trim(cbo_DeliveryTo.Text)) = 0 Then

            'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
            '             "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
            '             "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
            '             "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
            '             "[VehicleNo]      ,[VehicleType]   , [InvCode]) " &
            '             " " &
            '             " " &
            '             "  SELECT               'O'              , '1'             ,   ''              ,    'INV'    , a.Other_GST_Entry_RefNo ,a.Other_GST_Entry_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
            '             " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo  ,L.Ledger_Name,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,L.City_Town,L.Pincode, TS.State_Code,TS.State_Code," &
            '             " 1                     ,a.AddLess_Amount + a.RoundOff_Amount, A.Total_Taxable_Value    , A.Total_CGST_Amount  ,  A.Total_SGST_Amount , A.Total_IGST_Amount   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
            '             " a.LR_No        ,         a.Lr_Date            , a.Net_Amount         ,     CASE    WHEN a.Transport_Mode = 'Rail' THEN '2'  WHEN a.Transport_Mode = 'Air' THEN '3'  WHEN a.Transport_Mode = 'Ship' THEN '4'    ELSE '1' END AS TrMode ," &
            '             " a.Vehicle_No,'R','" & NewCode & "' from Other_GST_Entry_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
            '             " Inner Join Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Left Outer Join State_Head FS On " &
            '             " C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  where a.Other_GST_Entry_Reference_Code = '" & NewCode & "'"


            CMD.CommandText = "Insert into EWB_Head ([SupplyType]   , [SubSupplyType], [SubSupplyDesc], [DocType] ,     [EWBGenDocNo]       ,        [EWBDocDate]       , [FromGSTIN]           ,   [FromTradeName]     ,              [FromAddress1]         ,        [FromAddress2],                 [FromPlace],            [FromPINCode],            [FromStateCode], [ActualFromStateCode],        [ToGSTIN],      [ToTradeName],              [ToAddress1],                [ToAddress2],                           [ToPlace],      [ToPINCode],       [ToStateCode],  [ActualToStateCode], [TransactionType],                      [OtherValue],           [Total_value],         [CGST_Value],   [SGST_Value],   [IGST_Value],       [CessValue], [CessNonAdvolValue], [TransporterID], [TransporterName], [TransportDOCNo],         [TransportDOCDate]  , [TotalInvValue]       , [TransMode]   ,   [VehicleNo] , [VehicleType],        [InvCode]) " &
                                         "Select       'O'        ,      '1'         ,         ''     ,    'INV'  , a.Other_GST_Entry_RefNo  , a.Other_GST_Entry_Date   , C.Company_GSTINNo     ,   C.Company_Name      ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,	C.Company_City		, C.Company_PinCode    , FS.State_Code           ,FS.State_Code		   ,L.Ledger_GSTINNo  ,		L.Ledger_Name   ,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4 ,     L.City_Town	,	L.Pincode	,  TS.State_Code	,  TS.State_Code    ,           1         ,	  a.Add_Less + a.Round_Off_Amount , A.Total_Taxable_Value  , A.CGST_Amount  ,  A.SGST_Amount  ,  A.IGST_Amount   ,       0     ,          0        , t.Ledger_GSTINNo   , t.Ledger_Name   , '' AS LR_No        ,      NULL AS  Lr_Date  ,    a.Net_Amount      ,   '1' AS TrMode  , a.Vehicle_No ,       'R'       ,'" & NewCode & "'  " &
                                        "From Other_GST_Entry_Head a " &
                                        "inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                                        "Inner Join Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo  " &
                                        "Left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                                        "Left Outer Join State_Head FS On  C.Company_State_IdNo = fs.State_IdNo " &
                                        "Left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo " &
                                        "where a.Other_GST_Entry_Reference_Code =  '" & NewCode & "' "




        Else



            CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,  [DocType]   ,	   [EWBGenDocNo]     ,      [EWBDocDate]        ,   [FromGSTIN]    ,  [FromTradeName]  ,              [FromAddress1]         ,               [FromAddress2]         ,  [FromPlace] ,   [FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,  [ToGSTIN]       , [ToTradeName]  ,          [ToAddress1]           ,                 [ToAddress2]       ,      [ToPlace]  ,  [ToPINCode]  ,  [ToStateCode] ,   [ActualToStateCode] ,[TransactionType],              [OtherValue]           ,	[Total_value]       ,	[CGST_Value]   ,  [SGST_Value]  ,  [IGST_Value]   ,	[CessValue] ,[CessNonAdvolValue], [TransporterID]    ,[TransporterName]  ,[TransportDOCNo] , [TransportDOCDate]    ,[TotalInvValue]    ,      [TransMode]   ,  [VehicleNo]       ,  [VehicleType]   , [InvCode])  " &
                               "Select               'O'           , '1'             ,   ''              ,    'INV'    ,  a.Other_GST_Entry_RefNo , a.Other_GST_Entry_Date  , C.Company_GSTINNo , C.Company_Name   ,C.Company_Address1+C.Company_Address2, c.Company_Address3+C.Company_Address4,C.Company_City , C.Company_PinCode  ,   FS.State_Code  ,    FS.State_Code     ,L.Ledger_GSTINNo  ,L.Ledger_Name  ,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,    L.City_Town  ,   L.Pincode   ,    TS.State_Code,        TS.State_Code   ,        1      ,  a.Add_Less + a.Round_Off_Amount   , A.Total_Taxable_Value ,   A.CGST_Amount  ,  A.SGST_Amount , A.IGST_Amount   ,   0         ,          0        , t.Ledger_GSTINNo  , t.Ledger_Name ,      '' AS LR_No      , NULL AS Lr_Date      ,    a.Net_Amount   ,    '1'  AS TrMode ,    a.Vehicle_No   ,         'R'      ,'" & NewCode & "'  " &
                                "from Other_GST_Entry_Head a  " &
                                "inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo   " &
                                "Inner Join Ledger_Head L on a.DeliveryTo_IdNo = L.Ledger_IdNo " &
                                "Left Outer Join Ledger_Head T On a.Transport_IdNo = T.Ledger_IdNo  " &
                                "Left Outer Join State_Head FS On  C.Company_State_IdNo = fs.State_IdNo " &
                                "left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  " &
                                "where a.Other_GST_Entry_Reference_Code = '" & NewCode & "' "


        End If

        CMD.ExecuteNonQuery()

        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim dt1 As New DataTable


        da = New SqlClient.SqlDataAdapter("SELECT a.Sl_No,a.Item_Particulars, a.hsn_sac_code,a.Quantity,b.Unit_Name,a.Gst_Perc,a.Taxable_Value  FROM Other_GST_Entry_Details a  LEFT OUTER JOIN UNIT_HEAD  b on a.Unit_IdNo =b.Unit_IdNo  Where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' ", con)

        dt1 = New DataTable
        da.Fill(dt1)

        For I = 0 To dt1.Rows.Count - 1

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]             ,	  [Product_Description]        ,   	[HSNCode]                 ,	    [Quantity]                   ,        [QuantityUnit]                   ,           Tax_Perc                       ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode) " &
                              " values                 (" & dt1.Rows(I).Item(0).ToString & ",'" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(3).ToString & ", '" & dt1.Rows(I).Item(4).ToString & "'   ," & dt1.Rows(I).Item(5).ToString & "      ,        0               ,        0           ," & dt1.Rows(I).Item(6) & ",'" & Trim(NewCode) & "')"

            CMD.ExecuteNonQuery()

        Next

        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Other_GST_Entry_Head", "EWB_NO", "Other_GST_Entry_Reference_Code", Pk_Condition)


    End Sub
    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GetAuthToken(rtbEWBResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub
    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_EWayBillNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Other_GST_Entry_Head", "EWB_NO", "Other_GST_Entry_Reference_Code")

    End Sub
    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWayBillNo.Text, rtbEWBResponse, 0)
    End Sub
    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWayBillNo.Text, rtbEWBResponse, 1, Trim(txt_IR_No.Text))
    End Sub
    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_EWayBillNo.Text = txt_EWBNo.Text
        txt_eWayBill_No.Text = txt_EWBNo.Text
    End Sub

    Private Sub btn_Detail_PRINT_EWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINT_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbeInvoiceResponse, 1)
    End Sub

    Private Sub txt_EWayBillNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWayBillNo.TextChanged
        txt_eWayBill_No.Text = txt_EWayBillNo.Text
        txt_EWBNo.Text = txt_EWayBillNo.Text
    End Sub

    Private Sub btn_Close_EWB_Click_1(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub
    Private Sub Printing_GST_Format_1539(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim vNoofHsnCodes As Integer = 0
        Dim vFontName As String = ""
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim ItmNm3 As String = ""
        Dim vMorePages_STS As Boolean

        Dim vLine_Pen As Pen

        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            PrintDocument1.DefaultPageSettings.PaperSize = ps
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        End If


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30 ' 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        vLine_Pen = New Pen(Color.Black, 2)

        vFontName = "Calibri"

        pFont = New Font(vFontName, 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 16.5 '18.6 ' 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 3


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 45 : ClArr(2) = 250 : ClArr(3) = 80 : ClArr(4) = 50 : ClArr(5) = 75 : ClArr(6) = 50 : ClArr(7) = 60
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                '***** GST START *****
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                '  If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 4
                Else
                    If Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                    If Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString) = Val(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString) Then
                        NoofItems_PerPage = NoofItems_PerPage + 1
                    End If
                End If

                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 3
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                '***** GST END *****

                Printing_GST_Format_1539_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, vLine_Pen, vFontName)

                '  Printing_GST_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen, vFontName)
                '
                Try

                    NoofDets = 0

                    '    CurY = CurY + TxtHgt

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_GST_Format_1539_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False, vLine_Pen, vFontName, vMorePages_STS)

                                ' Printing_GST_Format_1539_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False, vLine_Pen, vFontName, vMorePages_STS)
                                '
                                e.HasMorePages = True

                                Return

                            End If

                            CurY = CurY + TxtHgt


                            ItmNm1 = prn_DetDt.Rows(DetIndx).Item("Item_Particulars").ToString
                            ItmNm2 = ""
                            ItmNm3 = ""
                            If Len(ItmNm1) > 30 Then
                                For I = 30 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 30
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Sl_No").ToString, LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Sac_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Gst_Perc").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Quantity").ToString), "##########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "#############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1
                            If Trim(ItmNm2) <> "" Then

                                If Len(ItmNm2) > 30 Then
                                    For I = 30 To 1 Step -1
                                        If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 30
                                    ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                    ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                                End If

                                CurY = CurY + TxtHgt - 3
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1

                                If Trim(ItmNm3) <> "" Then
                                    CurY = CurY + TxtHgt - 3
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                            End If
                            If Val(prn_DetDt.Rows(DetIndx).Item("Discount_Amount").ToString) <> 0 Then
                                CurY = CurY + TxtHgt
                                p1Font = New Font("vFontName", 9, FontStyle.Italic)
                                Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Format(Val(prn_DetDt.Rows(DetIndx).Item("Discount_Perc").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) - 10, CurY, 1, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, Format(-1 * Val(prn_DetDt.Rows(DetIndx).Item("Discount_Amount").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format_1539_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True, vLine_Pen, vFontName, vMorePages_STS)
                    'Printing_GST_Format_1539_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArR, NoofDets, True, vLine_Pen, vFontName, vMorePages_STS)


                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 0
                            prn_PageNo = 0
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If
                    End If


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format_1539_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByRef NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen, ByVal vFontName As String)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_CIN As String, Cmp_UAMNO As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim Rate_PCMETER As String = ""
        Dim vPgNo_TXT As String = ""
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim I As Integer = 0
        Dim br2 As SolidBrush                  '--COMMON BRUSH FOR ALL DETAILS 
        Dim vbr_CmpName As SolidBrush          '--COMPANY TITTLE
        Dim vbr_CmpDets As SolidBrush          '--COMPANY DETAILS

        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String
        Dim Cen1 As Single = 0
        Dim LInc As Integer = 0
        ' Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim BlockInvNoY As Single = 0


        PageNo = PageNo + 1

        CurY = TMargin


        If PageNo <= 1 Then
            prn_Count = prn_Count + 1
        End If

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)


                If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL FOR BUYER"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "DUPLICATE FOR TRANSPORT"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE FOR ASSESSE"
                    ElseIf Val(S) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If

                    End If



                End If
        End If

        vLine_Pen = New Pen(Color.Black, 2)

        Dim br = New SolidBrush(Color.FromArgb(255, 0, 0))

        p1Font = New Font(vFontName, 14, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font, br2)

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont, br2)
        End If

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

        LnAr(1) = CurY



        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = "" : Cmp_UAMNO = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_CIN = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "Phone : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO : " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO : " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CinNo").ToString) <> "" Then

            Cmp_CIN = "CIN : " & prn_HdDt.Rows(0).Item("Company_CinNo").ToString

        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_UAM_No").ToString) <> "" Then
            Cmp_UAMNO = "UDYAM No. : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
        End If

        CurY = CurY + TxtHgt - 5
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1539" Then

        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Shree_Sakthi_Tex_Logo, Drawing.Image), LMargin + 10, CurY + 5, 130, 80)

        'Else

        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                '.BackgroundImage = Image.FromStream(ms)


                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 5, 100, 100)

                                'e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 20, CurY + 5, 110, 100)
                                'e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)

                            End If

                        End Using

                    End If

                End If

            End If

            ' End If


            '**************************


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then      '--------JEEVITHA TEXTILE

            If Trim(UCase(Cmp_GSTIN_No)) = "33EDJPS1112P1ZB" Then
                '   br2 = New SolidBrush(Color.Green)                        
                vbr_CmpName = New SolidBrush(Color.Red)                        '---COMPANY TITTLE NAME
                vbr_CmpDets = New SolidBrush(Color.Green)                      '---COMPANY DETATILS 
            ElseIf Trim(UCase(Cmp_GSTIN_No)) = "33CNNPK7602N2ZK" Then

                '  br2 = New SolidBrush(Color.Black)
                vbr_CmpName = New SolidBrush(Color.Black)                       '---COMPANY TITTLE NAME
                vbr_CmpDets = New SolidBrush(Color.Black)                       '---COMPANY DETATILS 

            End If

        End If

        '********************


        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 95, CurY - 5, 90, 90)

                        End If

                    End Using

                End If

            End If

        End If

        p1Font = New Font(vFontName, 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpName)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 10
        If Desc <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        CurY = CurY + TxtHgt

        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        If Trim(Cmp_UAMNO) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, pFont)
        End If



        '-------------

        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
        If PrintWidth > strWidth Then
            If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                    CurX = LMargin + ((PrintWidth - strWidth) / 2) - 75

                Else
                    CurX = LMargin + (PrintWidth - strWidth) / 2

                End If

            Else
                CurX = LMargin + (PrintWidth - strWidth) / 2

            End If




        Else
            CurX = LMargin
        End If

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For I = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If

            CurY = CurY + TxtHgt + 2

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin, CurY, 0, 0, p1Font, br2)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PageWidth - 20, CurY, 1, 0, p1Font, br2)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "          " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font, br2)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PageWidth - 20, CurY, 1, 0, p1Font, br2)
            End If

        End If

        CurY = CurY + TxtHgt - 5
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            If Val(ClAr(6)) > 0 Then
                'C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
                C1 = ClAr(1) + ClAr(2) + ClAr(3)
            Else
                C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            End If


            W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
            S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

            W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
            S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

            W3 = e.Graphics.MeasureString("INVOICE   DATE", pFont).Width
            S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width

            CurY = CurY + 10
            p1Font = New Font(vFontName, 12, FontStyle.Bold)



            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_GST_Entry_PrefixNo").ToString & " " & prn_HdDt.Rows(0).Item("Other_GST_Entry_No").ToString & " " & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + W3 + 30, CurY - 2, 0, 0, p1Font, br2)

            Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont, br2)

            CurY = CurY + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont, br2)
            p1Font = New Font(vFontName, 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Other_GST_Entry_Date").ToString), "dd-MM-yyyy"), LMargin + W3 + 30, CurY - 2, 0, 0, p1Font, br2)

            If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO. ", LMargin + C1 + 10, CurY, 0, 0, pFont, br2)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont, br2)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont, br2)


            End If



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            'LnAr(2) = CurY

            CurY1 = CurY
            CurY2 = CurY

            '---left side

            CurY1 = CurY1 + 10

            p1Font = New Font(vFontName, 10, FontStyle.Bold)


            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font, br2)

            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY1 = CurY1 + strHeight

            p1Font = New Font(vFontName, 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font, br2, True, LMargin + C1)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, br2)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, br2)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, br2)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, br2)
            End If

            CurY1 = CurY1 + TxtHgt


            CurY2 = CurY2 + 10

            p1Font = New Font(vFontName, 10, FontStyle.Bold)



            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font, br2)
                strHeight = e.Graphics.MeasureString("A", p1Font).Height
                CurY2 = CurY2 + strHeight

                p1Font = New Font(vFontName, 11, FontStyle.Bold)



                ' CurY2 = CurY2 + TxtHgt
                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString)

                ItmNm2 = ""
            If Len(ItmNm1) > 30 Then
                For I = 30 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 30

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If
            If (prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & Trim(ItmNm1), LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font, br2, True, PrintWidth)
                If Trim(ItmNm2) <> "" Then
                    CurY2 = CurY2 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(ItmNm2), LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font, br2, True, PrintWidth)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font, br2, True, PrintWidth)

            End If

            CurY2 = CurY2 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, br2)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, br2)

            End If
            CurY2 = CurY2 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, br2)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, br2)

            End If
            CurY2 = CurY2 + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, br2)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, br2)
            End If
            CurY2 = CurY2 + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)

            End If

            CurY1 = IIf(CurY1 > CurY2, CurY1, CurY2)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1342" Then
                CurY1 = CurY1 + TxtHgt
                e.Graphics.DrawLine(vLine_Pen, LMargin, CurY1, PageWidth, CurY1)
            End If
            CurY1 = CurY1 + TxtHgt - 15


            p1Font = New Font(vFontName, 10, FontStyle.Bold)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font, br2)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p1Font).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, p1Font, br2)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, p1Font, br2)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, p1Font, br2)

            End If
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, p1Font).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY1, 0, PrintWidth, p1Font, br2)
            Else
                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p1Font).Width
                    CurX = LMargin + C1 + S1 + 10 + strWidth
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, p1Font, br2)
                End If
            End If

            CurY1 = CurY1 + TxtHgt
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1342" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1159" Then
                e.Graphics.DrawLine(vLine_Pen, LMargin, CurY1, PageWidth, CurY1)
                LnAr(10) = CurY1
            End If


            CurY1 = CurY1 + TxtHgt - 5   ' 15
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1159" Then
                    Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, "CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C1 - 100, CurY1, 0, 0, pFont, br2)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "STATE CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C1 - 100, CurY1, 0, 0, pFont, br2)
                End If
            End If
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1159" Then
                    Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, " CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, PageWidth - 100, CurY1, 0, 0, pFont, br2)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, " STATE CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, PageWidth - 100, CurY1, 0, 0, pFont, br2)
                End If
            Else
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1159" Then
                    Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, " CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, PageWidth - 100, CurY1, 0, 0, pFont, br2)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, " STATE CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, PageWidth - 100, CurY1, 0, 0, pFont, br2)
                End If

            End If

            CurY = IIf(CurY1 > CurY2, CurY1, CurY2)

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
            LnAr(3) = CurY

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1342" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1159" Then
                e.Graphics.DrawLine(vLine_Pen, LMargin + C1 - 110, LnAr(10), LMargin + C1 - 110, LnAr(3))
                e.Graphics.DrawLine(vLine_Pen, PageWidth - 110, LnAr(10), PageWidth - 110, LnAr(3))
            End If

            W2 = e.Graphics.MeasureString("AGENT NAME   :", pFont).Width

            W1 = e.Graphics.MeasureString("DOCUMENT THROUGH   : ", pFont).Width

            S2 = e.Graphics.MeasureString("DATE & TIME OF SUPPLY  : ", pFont).Width

            '---Right Side
            CurY = CurY + 10


            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + 10, CurY, 0, 0, pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 30, CurY, 0, 0, pFont, br2)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)).ToString, LMargin + W2 + 40, CurY, 0, 0, pFont, br2)

            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont, br2)


            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + 10, CurY, 0, 0, pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 30, CurY, 0, 0, pFont, br2)

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + W2 + 40, CurY, 0, 0, pFont, br2)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + W2 + 40, CurY, 0, 0, pFont, br2)
            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(3))
            LnAr(4) = CurY

            '---Table Headings


            CurY = CurY + 10

            'Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY + (TxtHgt \ 2), 2, ClAr(1), pFont, br2)

            'Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY + (TxtHgt \ 2), 2, ClAr(2), pFont, br2)

            'Common_Procedures.Print_To_PrintDocument(e, "HSN", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont, br2)

            'Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont, br2)

            'Common_Procedures.Print_To_PrintDocument(e, "GST", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont, br2)

            'Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont, br2)

            'Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + (TxtHgt \ 2), 2, ClAr(5), pFont, br2)

            'Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + (TxtHgt \ 2), 2, ClAr(6), pFont, br2)

            'Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, pFont, br2)

            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont, br2)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont, br2)


            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            p1Font = New Font(vFontName, 8, FontStyle.Bold)




        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format_1539_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vLine_Pen As Pen, ByVal vFontName As String, ByRef vIS_HasMorePages As Boolean)
        Dim p1Font As Font, p2Font As Font, p3Font As Font
        Dim rndoff As String, TtAmt As Double
        Dim I As Integer
        Dim BInc As Integer
        Dim pFont1 As Font
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim BmsInWrds As String = ""
        Dim vprn_BlNos As String = ""
        Dim BLNo1 As String = "", BLNo2 As String = "", BLNo3 As String = "", BLNo4 As String = "", BLNo5 As String = "", BLNo6 As String = "", BLNo7 As String = ""
        Dim Tot_Mtr As String = ""
        Dim BnkDetAr() As String, BnkAcDet2Ar() As String
        Dim BnkAcAr2() As String
        Dim BankNm1 As String = "", BankAc2Nm1 As String = ""
        Dim BankNm2 As String = "", BankAc2Nm2 As String = ""
        Dim BankNm3 As String = "", BankAc2Nm3 As String = ""
        Dim BankNm4 As String = "", BankAc2Nm4 As String = ""
        Dim CurY1 As Single = 0
        Dim Curx As Single = 0
        Dim strWidth As Single = 0
        Dim strHEIGHT As Single = 0
        Dim vBNKWidth As Single = 0
        Dim vBNK_TOP_Yaxs As Single = 0
        Dim SubClAr(15) As Single
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim tot_pcs As Integer = 0
        Dim Vpcsmtr As String = ""
        Dim len1 As Single = 0
        Dim br2 As SolidBrush
        Dim vDUEDATE_TERMS As String = ""
        Dim Y1 As Single = 0, Y2 As Single = 0

        Dim vOpen_Bal_Amt As String = ""
        Dim vReceived_Amt As String = ""
        Dim vInvoice_Amt As String = ""
        Dim vBal_Amt As String = ""

        Dim vOpen_Bal_Amt_cap As String = ""
        Dim vReceived_Amt_cap As String = ""
        Dim vInvoice_Amt_cap As String = ""
        Dim vBal_Amt_cap As String = ""

        Dim VLINE As Single = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            vLine_Pen = New Pen(Color.Black, 2)

            CurY = CurY + TxtHgt + 7
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            ' TOTAL PRINTING
            CurY = CurY + 5 '+ TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 30, CurY, 0, 0, pFont, br2)
            '5,6,7


            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, 0, pFont)


            Dim vJURISDICTN As String
            vJURISDICTN = Trim(prn_HdDt.Rows(0).Item("Jurisdiction").ToString)
            If Trim(vJURISDICTN) = "" Then
                vJURISDICTN = Common_Procedures.settings.Jurisdiction
            End If


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            'End If

            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            If Val(ClAr(4)) > 0 Then
                e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            End If
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            If Val(ClAr(6)) > 0 Then
                e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            End If

            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            vIS_HasMorePages = False



            CurY1 = CurY

            Erase BnkAcAr2
            Erase BnkDetAr
            Erase BnkAcDet2Ar
            If is_LastPage = True Then

                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then

                    BnkAcAr2 = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), "~")

                    If UBound(BnkAcAr2) >= 0 Then
                        BnkDetAr = Split(Trim(BnkAcAr2(0)), ",")
                        If UBound(BnkAcAr2) >= 1 Then
                            BnkAcDet2Ar = Split(Trim(BnkAcAr2(1)), ",")
                        End If

                    Else
                        BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    End If

                    If IsNothing(BnkDetAr) = False Then

                        BInc = -1

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            BankNm1 = Trim(BnkDetAr(BInc))
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            BankNm2 = Trim(BnkDetAr(BInc))
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            BankNm3 = Trim(BnkDetAr(BInc))
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            BankNm4 = Trim(BnkDetAr(BInc))
                        End If

                    End If


                    If IsNothing(BnkAcDet2Ar) = False Then

                        BInc = -1

                        BInc = BInc + 1
                        If UBound(BnkAcDet2Ar) >= BInc Then
                            BankAc2Nm1 = Trim(BnkAcDet2Ar(BInc))
                        End If

                        BInc = BInc + 1
                        If UBound(BnkAcDet2Ar) >= BInc Then
                            BankAc2Nm2 = Trim(BnkAcDet2Ar(BInc))
                        End If

                        BInc = BInc + 1
                        If UBound(BnkAcDet2Ar) >= BInc Then
                            BankAc2Nm3 = Trim(BnkAcDet2Ar(BInc))
                        End If

                        BInc = BInc + 1
                        If UBound(BnkAcDet2Ar) >= BInc Then
                            BankAc2Nm4 = Trim(BnkAcDet2Ar(BInc))
                        End If

                    End If




                End If




                p1Font = New Font(vFontName, 9, FontStyle.Bold)

                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BLNo2, LMargin + 30, CurY1, 0, 0, p1Font, br2)

                If Trim(BLNo3) <> "" Then
                    CurY1 = CurY1 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, BLNo3, LMargin + 30, CurY1, 0, 0, p1Font, br2)
                End If

                If Trim(BLNo4) <> "" Then
                    CurY1 = CurY1 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, BLNo4, LMargin + 30, CurY1, 0, 0, p1Font, br2)
                End If
                If Trim(BLNo5) <> "" Then
                    CurY1 = CurY1 + TxtHgt - 3
                    Common_Procedures.Print_To_PrintDocument(e, BLNo5, LMargin + 30, CurY1, 0, 0, p1Font, br2)
                End If
                If Trim(BLNo6) <> "" Then
                    CurY1 = CurY1 + TxtHgt - 3
                    Common_Procedures.Print_To_PrintDocument(e, BLNo6, LMargin + 30, CurY1, 0, 0, p1Font, br2)
                End If
                pFont = New Font(vFontName, 9, FontStyle.Bold)
                p1Font = New Font(vFontName, 10, FontStyle.Bold)



                CurY1 = CurY1 + 5

                p3Font = New Font(vFontName, 10, FontStyle.Bold)



                p1Font = New Font(vFontName, 15, FontStyle.Underline Or FontStyle.Bold)

                p3Font = New Font(vFontName, 12, FontStyle.Bold)

                strHEIGHT = e.Graphics.MeasureString(Trim("BANK"), p3Font).Height


                If Trim(BankNm1) <> "" Then
                    CurY1 = CurY1 + TxtHgt
                    vBNK_TOP_Yaxs = CurY1 + strHEIGHT + 3
                    If Trim(BankAc2Nm1) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY1, 2, ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), p1Font, br2)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY1, 0, 0, p1Font, br2)
                    End If
                End If


                vBNKWidth = e.Graphics.MeasureString(Trim("BANK NAME      :  " & BankNm1), p3Font).Width

                strWidth = e.Graphics.MeasureString(Trim("BRANCH NAME :  " & BankNm2), p3Font).Width
                If strWidth > vBNKWidth Then
                    vBNKWidth = strWidth
                End If
                strWidth = e.Graphics.MeasureString(Trim("ACCOUNT NO    :  " & BankNm3), p3Font).Width
                If strWidth > vBNKWidth Then
                    vBNKWidth = strWidth
                End If

                W1 = e.Graphics.MeasureString(Trim("BRANCH NAME :"), p3Font).Width
                W2 = vBNKWidth + 25


                CurY1 = CurY1 + 2
                CurY1 = CurY1 + strHEIGHT + 1 '  + TxtHgt + 3
                If Trim(BankNm1) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "BANK NAME", LMargin + 10, CurY1, 0, 0, p3Font, br2)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 10 + W1, CurY1, 0, 0, p3Font, br2)
                    Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10 + W1 + 15, CurY1, 0, 0, p3Font, br2)
                End If
                If Trim(BankAc2Nm1) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(BankAc2Nm1), LMargin + W2 + 10, CurY1, 0, 0, p3Font, br2)
                End If

                CurY1 = CurY1 + strHEIGHT + 0.5 ' + TxtHgt + 3
                If BankNm2 <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "BRANCH NAME", LMargin + 10, CurY1, 0, 0, p3Font, br2)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 10 + W1, CurY1, 0, 0, p3Font, br2)
                    Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10 + W1 + 15, CurY1, 0, 0, p3Font, br2)
                End If
                If Trim(BankAc2Nm2) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(BankAc2Nm2), LMargin + W2 + 10, CurY1, 0, 0, p3Font, br2)
                End If


                CurY1 = CurY1 + strHEIGHT + 0.5  ' + TxtHgt + 3
                If BankNm3 <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT NO", LMargin + 10, CurY1, 0, 0, p3Font, br2)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 10 + W1, CurY1, 0, 0, p3Font, br2)
                    Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10 + W1 + 15, CurY1, 0, 0, p3Font, br2)
                End If
                If Trim(BankAc2Nm3) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(BankAc2Nm3), LMargin + W2 + 10, CurY1, 0, 0, p3Font, br2)
                End If

                CurY1 = CurY1 + strHEIGHT + 0.5  '+ TxtHgt + 3
                If BankNm4 <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE", LMargin + 10, CurY1, 0, 0, p3Font, br2)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 10 + W1, CurY1, 0, 0, p3Font, br2)
                    Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10 + W1 + 15, CurY1, 0, 0, p3Font, br2)
                End If
                If Trim(BankAc2Nm4) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(BankAc2Nm4), LMargin + W2 + 10, CurY1, 0, 0, p3Font, br2)
                End If

                If Trim(BankAc2Nm1) <> "" Or Trim(BankAc2Nm2) <> "" Or Trim(BankAc2Nm3) <> "" Or Trim(BankAc2Nm4) <> "" Then
                    e.Graphics.DrawLine(vLine_Pen, LMargin + W2, CurY1 + strHEIGHT, LMargin + W2, vBNK_TOP_Yaxs)
                End If

            End If


            '---Right Side
            CurY = CurY - 5
            CurY = CurY + TxtHgt
            VLINE = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15
            If is_LastPage = True Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + VLINE, CurY, 1, 0, pFont, br2)
                    'Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
                    '   Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(-1 * Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, br2)
                End If

                vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

                CurY = CurY + TxtHgt
                If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then


                    p1Font = New Font(vFontName, 10, FontStyle.Bold)


                    If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "TAXABLE VALUE", LMargin + VLINE, CurY, 1, 0, p1Font, br2)
                        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, p1Font, br2)
                        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font, br2)
                    End If
                End If


                '----Gst
                CurY = CurY + TxtHgt
                If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ ", LMargin + VLINE - 20, CurY, 1, 0, pFont, br2)
                    If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(vTaxPerc) & " %", LMargin + VLINE - 20, CurY, 0, 0, pFont, br2)
                    End If
                    'Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, br2)


                End If
                CurY = CurY + TxtHgt
                If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ ", LMargin + VLINE - 20, CurY, 1, 0, pFont, br2)
                    If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(vTaxPerc) & " %", LMargin + VLINE - 20, CurY, 0, 0, pFont, br2)
                    End If
                    '   Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, br2)


                End If

                CurY = CurY + TxtHgt
                If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ ", LMargin + VLINE - 20, CurY, 1, 0, pFont, br2)
                    If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(vTaxPerc) & " %", LMargin + VLINE - 20, CurY, 0, 0, pFont, br2)
                    End If
                    '   Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, br2)
                End If
                If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then

                    If Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString) <> 0 Then

                        rndoff = Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString)
                        If Val(rndoff) <> 0 Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + VLINE, CurY, 1, 0, pFont, br2)
                            If Val(rndoff) >= 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 0, 0, pFont, br2)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 0, 0, pFont, br2)
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, br2)
                        End If

                        'CurY = CurY + TxtHgt
                        'Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                    End If

                    CurY = CurY + TxtHgt + 2
                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                    CurY = CurY - 15 + 2

                    p1Font = New Font(vFontName, 11, FontStyle.Bold)

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "TOTAL INVOICE VALUE", LMargin + VLINE, CurY, 1, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font, br2)

                    CurY = CurY + 5

                    If Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString) <> Val(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString) Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "TCS TAXABLE VALUE", LMargin + VLINE, CurY, 1, 0, pFont, br2)
                        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, br2)
                    End If

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TCs_name_caption").ToString & "  @ " & Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + VLINE, CurY, 1, 0, pFont, br2)
                    '   Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, br2)

                End If


                If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Add Less", LMargin + VLINE, CurY, 1, 0, pFont, br2)
                    ' Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont, br2)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Add_Less").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, br2)
                End If

                If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then
                    TtAmt = Format(Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString) + Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Add_Less").ToString), "#########0.00")

                Else


                    TtAmt = Format(Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) - -Val(prn_HdDt.Rows(0).Item("Add_Less").ToString), "#########0.00")
                    rndoff = 0
                    rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

                End If

                If (CurY + TxtHgt + TxtHgt + TxtHgt) < CurY1 Then
                    CurY = CurY1 - TxtHgt - TxtHgt
                End If

                CurY = CurY + TxtHgt

                If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) = 0 Then
                    rndoff = Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString) + Val(prn_HdDt.Rows(0).Item("Round_Off_Amount").ToString), "#######0.00")
                Else
                    rndoff = Format(Val(prn_HdDt.Rows(0).Item("Round_Off_Amount").ToString), "#######0.00")
                End If

                If Val(rndoff) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + VLINE, CurY, 1, 0, pFont, br2)
                    If Val(rndoff) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont, br2)
                    ElseIf Val(rndoff) < 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont, br2)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, br2)
                End If

                'End If


            End If

            If CurY1 > CurY Then CurY = CurY1

            If CurY < 690 Then CurY = 690 ' 731

            CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY + 5, PageWidth, CurY + 5)
            LnAr(8) = CurY


            If is_LastPage = True Then

                p1Font = New Font(vFontName, 11, FontStyle.Bold)

                CurY = CurY + TxtHgt - 10

                Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font, br2)
                p1Font = New Font(vFontName, 11, FontStyle.Bold)

                Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + VLINE, CurY, 1, 0, p1Font, br2)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font, br2)

            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            'e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

            CurY = CurY + 5
            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                BmsInWrds = Replace(Trim(BmsInWrds), "", "")


                p1Font = New Font(vFontName, 10, FontStyle.Bold)

                Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font, br2)

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
                LnAr(10) = CurY




                CurY = CurY + TxtHgt - 15

                p1Font = New Font(vFontName, 9, FontStyle.Underline Or FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font, br2)

                CurY = CurY + TxtHgt

                p2Font = New Font("Webdings", 8, FontStyle.Bold)
                p1Font = New Font(vFontName, 8, FontStyle.Bold)

                '1

                Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font, br2)

                Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from the invoice date ", LMargin + 25, CurY, 0, 0, p1Font, br2)

                '3
                '
                Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font, br2)


                Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", PrintWidth / 2 + 25, CurY, 0, 0, p1Font, br2)

                '2
                CurY = CurY + TxtHgt

                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1162" Then
                CurY = CurY + 2 ' + TxtHgt
                'End If
                Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font, br2)


                Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 25, CurY, 0, 0, p1Font, br2)

                Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font, br2)
                Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(vJURISDICTN) & " jurisdiction ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font, br2)

                'CurY = CurY + TxtHgt
                'p1Font = New Font(vFontName, 9, FontStyle.Underline Or FontStyle.Bold)
                'Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, p1Font)
                'CurY = CurY + TxtHgt
                'p1Font = New Font(vFontName, 8, FontStyle.Bold)
                'Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
                'Common_Procedures.Print_To_PrintDocument(e, "We Declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.", LMargin + 25, CurY, 0, 0, p1Font)



                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
                LnAr(10) = CurY



                CurY = CurY + 5
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                p1Font = New Font(vFontName, 7, FontStyle.Bold)

                Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font, br2)

                CurY = CurY - TxtHgt




                Dim br = New SolidBrush(Color.FromArgb(255, 0, 0))

                CurY = CurY + TxtHgt + 10
                p1Font = New Font(vFontName, 12, FontStyle.Bold)

                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, br2)

                '-------SIGNATURE-------'

                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt


                '-------END-SIGNATURE-------'

                ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1116" AND Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1120" Then
                If Val(prn_HdDt.Rows(0).Item("User_IdNo").ToString) <> 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("User_IdNo").ToString))) & ")", LMargin + 25, CurY, 0, 0, pFont, br2)
                End If
                ' End If

                br = New SolidBrush(Color.FromArgb(0, 150, 0))
                CurY = CurY + TxtHgt





                Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont, br2)
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 200, CurY, 0, 0, pFont, br2)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont, br2)




            CurY = CurY + TxtHgt + 10


            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(vLine_Pen, PageWidth, LnAr(1), PageWidth, CurY)

            If Val(prn_PageNo) > 1 Or is_LastPage = False Then
                CurY = CurY + 10
                p1Font = New Font(vFontName, 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Page No. : " & prn_PageNo, LMargin, CurY, 2, PageWidth, p1Font, br2)
            End If


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub get_Ledger_City_Name()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Led_ID As Integer
        Dim vDELV_ID As Integer
        Dim vDESP_LEDID As Integer
        Dim vDESPTO As String
        Dim vTDSDED_STS As String
        Dim vTCSDED_STS As String

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If Led_ID = 0 Then Exit Sub

        vDELV_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)

        If vDELV_ID <> 0 Then
            vDESP_LEDID = vDELV_ID
        Else
            vDESP_LEDID = Led_ID
        End If
        vDESPTO = ""
        vTCSDED_STS = ""
        vTDSDED_STS = ""
        Dim vLED_TDS_FIELD_NAME = ""
        Dim vLED_TCS_FIELD_NAME = ""


        If Trim(vEntryType) = "PURC" Or Trim(vEntryType) = "DRNT" Then
            vLED_TCS_FIELD_NAME = "TCS_Purchase_Status"
            vLED_TDS_FIELD_NAME = "Purchase_TDS_Deduction_Status"
            'ElseIf Trim(vEntryType) = "SALE" Or Trim(vEntryType) = "CRNT" Then
            '    vLED_TCS_FIELD_NAME = "TCS_Sales_Status"
            '    vLED_TDS_FIELD_NAME = "Sales_TDS_Deduction_Status"
        Else
            vLED_TCS_FIELD_NAME = "TCS_Sales_Status"
            vLED_TDS_FIELD_NAME = "Sales_TDS_Deduction_Status"

        End If


        da1 = New SqlClient.SqlDataAdapter("Select a.* from Ledger_Head a Where a.Ledger_IdNo = " & Str(Val(Led_ID)), con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)(vLED_TCS_FIELD_NAME).ToString) = False Then
                vTCSDED_STS = dt1.Rows(0)(vLED_TCS_FIELD_NAME).ToString
            End If
            If IsDBNull(dt1.Rows(0)(vLED_TDS_FIELD_NAME).ToString) = False Then
                vTDSDED_STS = dt1.Rows(0)(vLED_TDS_FIELD_NAME).ToString
            End If
        End If
        dt1.Clear()

        vDESPTO = ""
        da1 = New SqlClient.SqlDataAdapter("Select a.*, b.area_name from Ledger_Head a LEFT OUTER JOIN Area_Head b ON a.area_idno = b.area_idno Where a.Ledger_IdNo = " & Str(Val(vDESP_LEDID)), con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            vDESPTO = dt1.Rows(0)("City_Town").ToString
            If Trim(vDESPTO) = "" Then
                If IsDBNull(dt1.Rows(0)("area_name").ToString) = False Then
                    vDESPTO = dt1.Rows(0)("area_name").ToString
                End If
            End If
        End If
        dt1.Clear()

        dt1.Dispose()
        da1.Dispose()

        If Val(vTCSDED_STS) = 1 Then
            chk_TCS_Tax.Checked = True
        Else
            chk_TCS_Tax.Checked = False
        End If

        If Val(vTDSDED_STS) = 1 Then
            chk_TDS_Tax.Checked = True
        Else
            chk_TDS_Tax.Checked = False
        End If

        'If Trim(vDESPTO) <> "" Then
        '    cbo_DespTo.Text = Trim(vDESPTO)
        'End If

        cbo_Ledger.Tag = cbo_Ledger.Text
        cbo_DeliveryTo.Tag = cbo_DeliveryTo.Text
    End Sub

    Private Sub txt_TDS_Value_TextChanged(sender As Object, e As EventArgs) Handles txt_TDS_Value.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TDS_Value_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_TDS_Value.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If txt_Tds_Percentage.Enabled = True Then
                txt_Tds_Percentage.Focus()
            Else
                cbo_Agent.Focus()
            End If


        End If
    End Sub

    Private Sub txt_TDS_Value_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_TDS_Value.KeyDown
        If e.KeyCode = 38 Then

            txt_AddLess.Focus()

        End If
        If e.KeyCode = 40 Then

            If txt_Tds_Percentage.Enabled = True Then
                txt_Tds_Percentage.Focus()
            Else
                cbo_Agent.Focus()
            End If

        End If
    End Sub

    Private Sub chk_TDS_Tax_CheckedChanged(sender As Object, e As EventArgs) Handles chk_TDS_Tax.CheckedChanged
        If chk_TDS_Tax.Checked = True Then
            txt_Tds_Percentage.Text = "0.1"
            txt_Tds_Percentage.Enabled = True
            '  txt_Tds_Percentage.Focus()
        Else
            txt_Tds_Percentage.Text = ""
            txt_Tds_Percentage.Enabled = False
            ' txt_AddLess.Focus()
        End If
    End Sub
    Private Sub Printing_Format_GST_1370(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim vNoofHsnCodes As Integer = 0
        Dim vFontName As String = ""
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim ItmNm3 As String = ""

        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            PrintDocument1.DefaultPageSettings.PaperSize = ps
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        End If


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30 ' 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then '----Star Fabric Mills (Thekkalur)
            vFontName = "Cambria"
        Else
            vFontName = "Calibri"
        End If
        pFont = New Font(vFontName, 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 19.6 '18.6 ' 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height
        If Trim(UCase(vEntryType)) = "SALE" Then
            NoofItems_PerPage = 3 ' 6 '10  ' 19  
        Else
            NoofItems_PerPage = 3 ' 10  ' 19
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 45 : ClArr(2) = 250 : ClArr(3) = 80 : ClArr(4) = 50 : ClArr(5) = 75 : ClArr(6) = 40 : ClArr(7) = 90
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                '***** GST START *****
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                '  If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 4
                Else
                    If Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                    If Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString) = Val(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString) Then
                        NoofItems_PerPage = NoofItems_PerPage + 1
                    End If
                End If

                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 3
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                '***** GST END *****

                Printing_Format_GST_1370_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, vFontName)

                Try

                    NoofDets = 0

                    '    CurY = CurY + TxtHgt

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_Format_GST_1370_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False, vFontName)
                                e.HasMorePages = True

                                Return

                            End If

                            CurY = CurY + TxtHgt


                            ItmNm1 = prn_DetDt.Rows(DetIndx).Item("Item_Particulars").ToString
                            ItmNm2 = ""
                            ItmNm3 = ""
                            If Len(ItmNm1) > 20 Then
                                For I = 20 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 20
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Sl_No").ToString, LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Sac_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Gst_Perc").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Quantity").ToString), "##########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "#############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1
                            If Trim(ItmNm2) <> "" Then

                                If Len(ItmNm2) > 20 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 20
                                    ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                    ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                                End If

                                CurY = CurY + TxtHgt - 3
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1

                                If Trim(ItmNm3) <> "" Then
                                    CurY = CurY + TxtHgt - 3
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                            End If
                            If Val(prn_DetDt.Rows(DetIndx).Item("Discount_Amount").ToString) <> 0 Then
                                CurY = CurY + TxtHgt
                                p1Font = New Font("vFontName", 9, FontStyle.Italic)
                                Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Format(Val(prn_DetDt.Rows(DetIndx).Item("Discount_Perc").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) - 10, CurY, 1, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, Format(-1 * Val(prn_DetDt.Rows(DetIndx).Item("Discount_Amount").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format_GST_1370_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True, vFontName)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 0
                            prn_PageNo = 0
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If
                    End If


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_GST_1370_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vFontName As String)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim I As Integer = 0
        Dim CurY2 As Single = 0
        Dim Cmp_UAMNO As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, c.Unit_Name from Other_GST_Entry_Details a LEFT OUTER JOIN Unit_Head c on a.unit_idno = c.unit_idno where a.Other_GST_Entry_Reference_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                    PrintDocument1.DefaultPageSettings.Color = True
                    PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
                    e.PageSettings.Color = True
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        Dim vPrntHeading As String = ""

        If Trim(UCase(vEntryType)) = "PURC" Then
            vPrntHeading = "PURCHASE VOUCHER"

        ElseIf Trim(UCase(vEntryType)) = "SALE" Then
            vPrntHeading = "INVOICE"

        ElseIf Trim(UCase(vEntryType)) = "CRNT" Then
            vPrntHeading = "CREDIT NOTE"

        ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
            vPrntHeading = "DEBIT NOTE"

        ElseIf Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then
            vPrntHeading = "JOBWORK INVOICE"

        ElseIf Trim(UCase(vEntryType)) = "ADV.PYMT" Then
            vPrntHeading = "ADVANCE PAYMENT"

        End If

        p1Font = New Font(vFontName, 12, FontStyle.Regular)
        If Common_Procedures.settings.CustomerCode = "1214" Then
            If Trim(UCase(vEntryType)) = "SALE" Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Vinayakar_cholatx, Drawing.Image), LMargin + 10, CurY + 10, 90, 80)

            End If
        End If
        Common_Procedures.Print_To_PrintDocument(e, Trim(UCase(vPrntHeading)), LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)


        'CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
        Cmp_UAMNO = ""
        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If
        Else
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        End If

        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
        If Trim(Cmp_Add2) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            Else
                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            End If
        Else
            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_UAM_No").ToString) <> "" Then
            Cmp_UAMNO = "UDYAM No. : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
            p1Font = New Font(vFontName, 20, FontStyle.Bold)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
            p1Font = New Font("Elephant", 22, FontStyle.Bold)
        Else
            p1Font = New Font(vFontName, 18, FontStyle.Bold)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then '---- KRG TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KRG_Logo, Drawing.Image), LMargin + 10, CurY - 5, 90, 90)
        Else
            If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                '.BackgroundImage = Image.FromStream(ms)

                                ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 15, CurY + 10, 100, 100)

                            End If

                        End Using

                    End If

                End If

            End If
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 100, CurY - 35, 85, 85)

                        End If

                    End Using

                End If

            End If

        End If



        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

        If Trim(Cmp_UAMNO) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, pFont)
        End If

        '***** GST END *****




        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For I = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If




            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + Cen1 + 10, CurY, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 10, CurY, 1, 0, p1Font)

                If Trim(ItmNm2) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "          " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 10, CurY, 1, 0, p1Font)
                End If
            End If

        End If



        CurY = CurY + TxtHgt + 2
        If Trim(UCase(vEntryType)) = "SALE" Then
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            p1Font = New Font(vFontName, 14, FontStyle.Bold)
            If Trim(UCase(vEntryType)) = "CRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Credit No.", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Debit No.", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                Common_Procedures.Print_To_PrintDocument(e, "GRN No.", LMargin + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + 10, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_GST_Entry_PrefixNo").ToString & " " & prn_HdDt.Rows(0).Item("Other_GST_Entry_No").ToString & " " & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + 120, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt

            If Trim(UCase(vEntryType)) = "CRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Credit Date.", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                Common_Procedures.Print_To_PrintDocument(e, "Debit Date.", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                Common_Procedures.Print_To_PrintDocument(e, "GRN Date.", LMargin + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Invoice Date.", LMargin + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Other_GST_Entry_Date").ToString), "dd-MM-yyyy"), LMargin + 120, CurY, 0, 0, pFont)

        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            'If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
            '    PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

            '    If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
            '    If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
            '    If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
            '    If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
            '    If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
            '    '***** GST START *****
            '    If UBound(PnAr) >= 5 Then Led_State = Trim(PnAr(5))
            '    If UBound(PnAr) >= 6 Then Led_PhNo = Trim(PnAr(6))
            '    If UBound(PnAr) >= 7 Then Led_GSTTinNo = Trim(PnAr(7))
            '    '***** GST END *****

            'Else

            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            '***** GST START *****
            Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

            Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
            '***** GST END *****

            '  End If

            Erase LedNmAr
            LedNmAr = New String(10) {}
            LInc = 0

            LInc = LInc + 1
            LedNmAr(LInc) = Led_Name

            If Trim(Led_Add1) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add1
            End If

            If Trim(Led_Add2) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add2
            End If

            If Trim(Led_Add3) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add3
            End If

            'If Trim(Led_Add4) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_Add4
            'End If
            '***** GST START *****
            If Trim(Led_State) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_State
            End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_PhNo
            End If

            If Trim(Led_GSTTinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_GSTTinNo
            End If

            'If Trim(Led_TinNo) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_TinNo
            'End If
            '***** GST END *****

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3)
            'Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY


            'If Trim(UCase(vEntryType)) = "SALE" Then
            '    '***** GST START *****
            '    

            'Else
            If Trim(UCase(vEntryType)) = "SALE" Then
                '***** GST START *****
                Common_Procedures.Print_To_PrintDocument(e, " TO :", LMargin + 10, CurY, 0, 0, pFont)
                p1Font = New Font(vFontName, 11, FontStyle.Bold)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                '***** GST END *****

                'CurY2 = CurY

                CurY2 = BlockInvNoY
                If Trim(UCase(vEntryType)) = "SALE" Then

                    'CurY2 = CurY2 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " DELIVERY TO : ", LMargin + Cen1 + 10, CurY2, 0, 0, p1Font)
                    p1Font = New Font(vFontName, 11, FontStyle.Bold)

                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                        strHeight = e.Graphics.MeasureString("A", p1Font).Height
                        CurY2 = CurY2 + TxtHgt
                        p1Font = New Font("Calibri", 11, FontStyle.Bold)
                        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, p1Font)
                    End If

                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If
                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If

                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If

                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If


                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    End If


                    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)
                    Else
                        CurY2 = CurY2 + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, LMargin + Cen1 + 40, CurY2, 0, 0, pFont)

                    End If
                    'If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                    '    strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                    '    CurX = LMargin + Cen1 + W1 + 10 + strWidth
                    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont)
                    'End If
                End If

                CurY2 = CurY2 + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, CurY2 + 10, PageWidth, CurY2 + 10)

                CurY2 = CurY2 + TxtHgt + 10
                If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 10, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 40, CurY2, 0, 0, pFont)
                End If


                If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then

                    CurY2 = CurY2 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Eway BillNo", LMargin + Cen1 + 10, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + Cen1 + W1 + 40, CurY2, 0, 0, pFont)
                End If


                If Trim(prn_HdDt.Rows(0).Item("AgentName").ToString) <> "" Then

                    CurY2 = CurY2 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "AgentName", LMargin + Cen1 + 10, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AgentName").ToString, LMargin + Cen1 + W1 + 40, CurY2, 0, 0, pFont)
                End If

                CurY = IIf(CurY2 > CurY, CurY2, CurY)

                'BlockInvNoY = BlockInvNoY + TxtHgt
                'If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 40, CurY, 0, 0, pFont)
                'End If


                'If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
                '    BlockInvNoY = BlockInvNoY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "Eway BillNo", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + Cen1 + W1 + 40, CurY, 0, 0, pFont)
                'End If


            Else


                Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
                p1Font = New Font(vFontName, 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
                '***** GST END *****


                '------------------- Invoice No Block

                '***** GST START *****

                p1Font = New Font(vFontName, 14, FontStyle.Bold)

                If Trim(UCase(vEntryType)) = "CRNT" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Credit No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Debit No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GRN No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_GST_Entry_PrefixNo").ToString & " " & prn_HdDt.Rows(0).Item("Other_GST_Entry_No").ToString & " " & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

                BlockInvNoY = BlockInvNoY + TxtHgt

                If Trim(UCase(vEntryType)) = "CRNT" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Credit Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Debit Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                ElseIf Trim(UCase(vEntryType)) = "PURC" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GRN Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Invoice Date.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                End If

                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Other_GST_Entry_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)


                BlockInvNoY = BlockInvNoY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, BlockInvNoY + 10, PageWidth, BlockInvNoY + 10)

                BlockInvNoY = BlockInvNoY + TxtHgt

                CurY = CurY + TxtHgt + 10
                If Trim(prn_HdDt.Rows(0).Item("Bill_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                End If


                BlockInvNoY = BlockInvNoY + TxtHgt
                If msk_BillDate.Visible = True Then
                    If Trim(prn_HdDt.Rows(0).Item("Bill_Date").ToString) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, "Bill Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bill_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                    End If
                    'End If

                Else
                    '    If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
                    '        Common_Procedures.Print_To_PrintDocument(e, "Dc No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                    '    End If

                    'End If

                    '   BlockInvNoY = BlockInvNoY + TxtHgt
                    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "2002" Then
                    '    If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                    '        Common_Procedures.Print_To_PrintDocument(e, "Dc Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
                    '    




                End If

                BlockInvNoY = BlockInvNoY + TxtHgt
                If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                End If


                If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then

                    BlockInvNoY = BlockInvNoY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Eway BillNo", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt.Rows(0).Item("AgentName").ToString) <> "" Then

                    BlockInvNoY = BlockInvNoY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AgentName").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                End If
            End If





            'BlockInvNoY = BlockInvNoY + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString) <> "" Then
            '    p1Font = New Font(vFontName, 10, FontStyle.Regular)
            '    Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Issue", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY, 0, 0, p1Font)
            'End If

            '***** GST END *****

            '----------------------------



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))

            '***** GST START *****
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            '***** GST END *****

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_GST_1370_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vFontName As String)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim vTaxPerc As Single = 0
        Dim Yax As Single
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim remks As String
        Dim remks1 As String = ""
        Dim vNoofHsnCodes As Integer = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1162" Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            End If


            If is_LastPage = True Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1162" Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Sub_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

            Erase BnkDetAr

            If is_LastPage = True Then

                If Trim(UCase(vEntryType)) = "SALE" Or Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then

                    If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                        BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                        BInc = -1
                        Yax = CurY

                        Yax = Yax + TxtHgt - 10
                        'If Val(prn_PageNo) = 1 Then
                        p1Font = New Font(vFontName, 12, FontStyle.Bold Or FontStyle.Underline)
                        Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                        'End If

                        p1Font = New Font(vFontName, 11, FontStyle.Bold)
                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                        End If

                    End If

                End If

            End If


            CurY = CurY - 10

            '***** GST START *****
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(-1 * Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    If is_LastPage = True Then

            '        If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
            '            Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Else
            '            Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            '    End If
            'End If

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font(vFontName, 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
                End If

            End If




            remks = Trim(prn_HdDt.Rows(0).Item("Remarks").ToString)

            'remks1 = ""
            'If Len(remks) > 40 Then
            '    For I = 40 To 1 Step -1
            '        If Mid$(Trim(remks), I, 1) = " " Or Mid$(Trim(remks), I, 1) = "," Or Mid$(Trim(remks), I, 1) = "." Or Mid$(Trim(remks), I, 1) = "-" Or Mid$(Trim(remks), I, 1) = "/" Or Mid$(Trim(remks), I, 1) = "_" Or Mid$(Trim(remks), I, 1) = "(" Or Mid$(Trim(remks), I, 1) = ")" Or Mid$(Trim(remks), I, 1) = "\" Or Mid$(Trim(remks), I, 1) = "[" Or Mid$(Trim(remks), I, 1) = "]" Or Mid$(Trim(remks), I, 1) = "{" Or Mid$(Trim(remks), I, 1) = "}" Then Exit For
            '    Next I
            '    If I = 0 Then I = 40
            '    remks1 = Microsoft.VisualBasic.Right(Trim(remks), Len(remks) - I)
            '    remks = Microsoft.VisualBasic.Left(Trim(remks), I - 1)
            'End If


            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1333" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then
            '    If Trim(remks) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Remarks :" & remks, LMargin + 10, CurY, 0, 0, pFont)
            '    End If

            'End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            'If Trim(remks1) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(remks1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            '    'NoofDets = NoofDets + 1
            'End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then

                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            '***** GST END *****



            If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Add_Less").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            Dim rndoff As Double





            'If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString) <> 0 Then

                rndoff = Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString)
                If Val(rndoff) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 1, 0, pFont)
                    If Val(rndoff) >= 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            End If

            If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then

                CurY = CurY + TxtHgt + 2
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
                CurY = CurY - 15 + 2

                p1Font = New Font(vFontName, 11, FontStyle.Bold)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL INVOICE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)

                CurY = CurY + 5

                If Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString) <> Val(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString) Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "TCS TAXABLE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TCs_name_caption").ToString & "  @ " & Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Round_Off_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font(vFontName, 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font(vFontName, 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                If Len(Rup1) > 80 Then
                    For I = 80 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 80
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If
            End If

            '***** GST START *****
            CurY = CurY + TxtHgt - 12
            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY
            CurY = CurY + 2
            Common_Procedures.Print_To_PrintDocument(e, "Remarks :" & remks, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY
            '***** GST START *****
            '=============GST SUMMARY============
            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            If vNoofHsnCodes <> 0 Then
                Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            End If
            '==========================
            '***** GST END *****


            CurY = CurY + TxtHgt - 10



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 12, FontStyle.Bold)
            Else
                p1Font = New Font(vFontName, 12, FontStyle.Bold)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            If Trim(UCase(vEntryType)) = "SALE" Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Krs_Sign, Drawing.Image), PageWidth - 120, CurY + 5, 80, 50)
                    'CurY = CurY + TxtHgt + 23
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" And Print_PDF_Status = True Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.vijay_tex_Sign2, Drawing.Image), PageWidth - 110, CurY, 90, 55)


                End If
            End If
            'CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We Declare that this Invoice Shows the actual price of the goods", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY + 10, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font(vFontName, 9, FontStyle.Regular)

            'Jurs = Common_Procedures.settings.Jurisdiction
            'If Trim(Jurs) = "" Then Jurs = "Tirupur"

            Dim vJURISDICTN As String
            vJURISDICTN = Trim(prn_HdDt.Rows(0).Item("Jurisdiction").ToString)
            If Trim(vJURISDICTN) = "" Then
                vJURISDICTN = Common_Procedures.settings.Jurisdiction
            End If
            If Trim(vJURISDICTN) = "" Then vJURISDICTN = "Tirupur"

            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(vJURISDICTN) & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)

            If Print_PDF_Status = True Then
                CurY = CurY + TxtHgt - 15
                p1Font = New Font(vFontName, 9, FontStyle.Regular)

                Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)



            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Other_GST_Entry_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        'vSPEC_KEYS.Add(e.KeyCode)
        If e.Control AndAlso e.Alt AndAlso e.KeyCode = Keys.D Then
            'MessageBox.Show("Shortcut Ctrl + Alt + N activated!")
            DeleteAll()
        End If
    End Sub
    Private Sub Other_GST_Entry_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
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

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True
    End Sub
    Private Sub btn_EMail_Click(sender As Object, e As EventArgs) Handles btn_EMail.Click
        Dim Led_IdNo As Integer
        Dim MailTxt As String
        Dim vEntry_Type = ""
        Dim vEntry_Type_heading = ""

        Try


            Common_Procedures.Print_OR_Preview_Status = 1
            Print_PDF_Status = True
            EMAIL_Status = True
            WHATSAPP_Status = False
            print_Invoice()


            If Trim(UCase(vEntryType)) = "PURC" Then

                vEntry_Type = "Purchase No - "
                vEntry_Type_heading = "GENERAL PURCHASE "

            ElseIf Trim(UCase(vEntryType)) = "SALE" Then
                vEntry_Type_heading = "GENERAL SALES "
                vEntry_Type = "Invoice No - "


            ElseIf Trim(UCase(vEntryType)) = "CRNT" Then

                vEntry_Type_heading = "CREDIT NOTE "
                vEntry_Type = "Credit Note No - "

            ElseIf Trim(UCase(vEntryType)) = "DRNT" Then

                vEntry_Type_heading = "DEBIT NOTE "
                vEntry_Type = "Debit Note No - "

            ElseIf Trim(UCase(vEntryType)) = "JOBWORKINVOICE" Then

                vEntry_Type_heading = "JOBWORK INVOICE"
                vEntry_Type = "Invoice No - "


            ElseIf Trim(UCase(vEntryType)) = "ADV.PYMT" Then

                vEntry_Type_heading = "ADVANCE PAYMENT "
                vEntry_Type = "Voucher No - "

            End If



            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            MailTxt = vEntry_Type_heading & vbCrLf & vbCrLf

            MailTxt = MailTxt & vEntry_Type & Trim(lbl_EntryNo.Text) & vbCrLf & "Date-" & Trim(msk_Date.Text)


            If System.IO.File.Exists(vEMAIL_Attachment_FileName) = True Then
                MailTxt = MailTxt & vbCrLf
                MailTxt = MailTxt & vbCrLf
                MailTxt = MailTxt & "Please find the following attachment(s):"
                MailTxt = MailTxt & "        " & Trim(Path.GetFileName(vEMAIL_Attachment_FileName))
            End If

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            EMAIL_Entry.vSubJect = vEntry_Type & Trim(lbl_EntryNo.Text)

            EMAIL_Entry.vMessage = Trim(MailTxt)
            EMAIL_Entry.vAttchFilepath = ""
            If System.IO.File.Exists(vEMAIL_Attachment_FileName) = True Then
                EMAIL_Entry.vAttchFilepath = Trim(vEMAIL_Attachment_FileName)
            End If

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_JSON_Einvoice_Gen.Click
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim I As Integer
        Dim NewCode As String

        Dim docTyp As String = ""
        Dim docNo As String = ""
        Dim docDate As Date

        Dim sellerGstin As String = ""
        Dim sellerLglNm As String = ""
        Dim sellerTrdNm As String = ""
        Dim sellerAddr1 As String = ""
        Dim sellerAddr2 As String = ""
        Dim sellerLoc As String = ""
        Dim sellerPin As String = ""
        Dim sellerStcd As String = ""

        Dim buyerGstin As String = ""
        Dim buyerLglNm As String = ""
        Dim buyerTrdNm As String = ""
        Dim buyerAddr1 As String = ""
        Dim buyerAddr2 As String = ""
        Dim buyerLoc As String = ""
        Dim buyerPin As String = ""
        Dim buyerStcd As String = ""

        Dim vShiptoIDNO As String = 0
        Dim ShipGstin As String = ""
        Dim ShipLglNm As String = ""
        Dim ShipTrdNm As String = ""
        Dim ShipAddr1 As String = ""
        Dim ShipAddr2 As String = ""
        Dim ShipLoc As String = ""
        Dim ShipPin As String = ""
        Dim ShipStcd As String = ""

        Dim eInvitems As New List(Of EInvoiceItem)()

        Dim vSNo As Integer = 0
        Dim item_SlNo As String = 0
        Dim item_Desc As String = ""
        Dim item_IsServc As String = "N"
        Dim item_HsnCd As String = ""
        Dim item_Qty As String = 0
        Dim item_Unit As String = ""
        Dim item_UnitPrice As String = 0
        Dim item_TotAmt As String = 0
        Dim item_Discount As String = 0
        Dim item_PreTaxVal As String = 0
        Dim item_AssAmt As String = 0
        Dim item_GstRt As String = 0
        Dim item_IgstAmt As String = 0
        Dim item_CgstAmt As String = 0
        Dim item_SgstAmt As String = 0
        Dim item_TotItemVal As String = 0

        Dim vGSTAMT As String = 0

        Dim Total_AssVal As String = 0
        Dim Total_CgstVal As String = 0
        Dim Total_SgstVal As String = 0
        Dim Total_IgstVal As String = 0
        Dim Total_TotInvVal As String = 0

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EntryNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da1 = New SqlClient.SqlDataAdapter("select a.* from Other_GST_Entry_Head a Where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' and " & Other_Condition, con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then

                If Trim(UCase(vEntryType)) = "CRNT" Then
                    docTyp = "CRN"
                ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
                    docTyp = "DBN"
                Else
                    docTyp = "INV"
                End If
                docNo = Trim(dt1.Rows(0).Item("Other_GST_Entry_RefNo").ToString)
                docDate = dt1.Rows(0).Item("Other_GST_Entry_Date")


                '----SELLER DETAILS
                da1 = New SqlClient.SqlDataAdapter("select a.*, b.State_Code from Company_Head a INNER JOIN State_Head b ON a.Company_State_IdNo = b.State_IdNo Where a.Company_IdNo = " & Str(Val(dt1.Rows(0).Item("Company_IdNo").ToString)), con)
                dt2 = New DataTable
                da1.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    sellerGstin = Trim(dt2.Rows(0).Item("Company_GSTinNo").ToString)
                    sellerLglNm = Trim(dt2.Rows(0).Item("Legal_Nameof_Business").ToString)
                    sellerTrdNm = Trim(dt2.Rows(0).Item("Company_Name").ToString)
                    If Trim(sellerLglNm) = "" Then sellerLglNm = sellerTrdNm
                    sellerAddr1 = Trim(dt2.Rows(0).Item("Company_Address1").ToString) & " " & Trim(dt2.Rows(0).Item("Company_Address2").ToString)
                    sellerAddr2 = Trim(dt2.Rows(0).Item("Company_Address3").ToString) & " " & Trim(dt2.Rows(0).Item("Company_Address4").ToString)
                    sellerLoc = Trim(dt2.Rows(0).Item("Company_City").ToString)
                    sellerPin = Trim(dt2.Rows(0).Item("Company_PinCode").ToString)
                    sellerStcd = Trim(dt2.Rows(0).Item("State_Code").ToString)
                End If
                dt2.Clear()


                '----BUYER DETAILS
                da1 = New SqlClient.SqlDataAdapter("select a.*, b.State_Code from Ledger_Head a INNER JOIN State_Head b ON a.Ledger_State_IdNo = b.State_IdNo Where a.Ledger_IdNo = " & Str(Val(dt1.Rows(0).Item("Ledger_IdNo").ToString)), con)
                dt2 = New DataTable
                da1.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    buyerGstin = Trim(dt2.Rows(0).Item("Ledger_GSTinNo").ToString)
                    buyerLglNm = Trim(dt2.Rows(0).Item("Legal_Nameof_Business").ToString)
                    buyerTrdNm = Trim(dt2.Rows(0).Item("Ledger_MainName").ToString)
                    If Trim(buyerLglNm) = "" Then buyerLglNm = buyerTrdNm
                    buyerAddr1 = Trim(dt2.Rows(0).Item("Ledger_Address1").ToString) & " " & Trim(dt2.Rows(0).Item("Ledger_Address2").ToString)
                    buyerAddr2 = Trim(dt2.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(dt2.Rows(0).Item("Ledger_Address4").ToString)
                    buyerLoc = Trim(dt2.Rows(0).Item("City_Town").ToString)
                    buyerPin = Trim(dt2.Rows(0).Item("Pincode").ToString)
                    buyerStcd = Trim(dt2.Rows(0).Item("State_Code").ToString)
                End If
                dt2.Clear()


                '----SHIP DETAILS
                If Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                    vShiptoIDNO = Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString)
                Else
                    vShiptoIDNO = Val(dt1.Rows(0).Item("Ledger_IdNo").ToString)
                End If

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.State_Code from Ledger_Head a INNER JOIN State_Head b ON a.Ledger_State_IdNo = b.State_IdNo Where a.Ledger_IdNo = " & Str(Val(vShiptoIDNO)), con)
                dt2 = New DataTable
                da1.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    ShipGstin = Trim(dt2.Rows(0).Item("Ledger_GSTinNo").ToString)
                    ShipLglNm = Trim(dt2.Rows(0).Item("Legal_Nameof_Business").ToString)
                    ShipTrdNm = Trim(dt2.Rows(0).Item("Ledger_MainName").ToString)
                    If Trim(ShipLglNm) = "" Then ShipLglNm = ShipTrdNm
                    ShipAddr1 = Trim(dt2.Rows(0).Item("Ledger_Address1").ToString) & " " & Trim(dt2.Rows(0).Item("Ledger_Address2").ToString)
                    ShipAddr2 = Trim(dt2.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(dt2.Rows(0).Item("Ledger_Address4").ToString)
                    ShipLoc = Trim(dt2.Rows(0).Item("City_Town").ToString)
                    ShipPin = Trim(dt2.Rows(0).Item("Pincode").ToString)
                    ShipStcd = Trim(dt2.Rows(0).Item("State_Code").ToString)
                End If
                dt2.Clear()


                '----ITEM DETAILS

                Total_AssVal = 0
                Total_CgstVal = 0
                Total_SgstVal = 0
                Total_IgstVal = 0
                Total_TotInvVal = 0


                vSNo = 0
                da1 = New SqlClient.SqlDataAdapter("select a.*, b.Unit_Name from Other_GST_Entry_Details a LEFT OUTER JOIN Unit_Head b on a.unit_idno = b.unit_idno where a.Other_GST_Entry_Reference_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da1.Fill(dt2)
                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        vSNo = vSNo + 1
                        item_SlNo = vSNo
                        item_Desc = Trim(dt2.Rows(I).Item("Item_Particulars").ToString)
                        item_HsnCd = Trim(dt2.Rows(I).Item("Hsn_Sac_Code").ToString)
                        If Trim(Microsoft.VisualBasic.Left(item_HsnCd, 2)) = "99" Then
                            item_IsServc = "Y"
                        Else
                            item_IsServc = "N"
                        End If

                        item_Qty = Val(dt2.Rows(I).Item("Quantity").ToString)
                        item_Unit = Trim(dt2.Rows(I).Item("Unit_Name").ToString)
                        item_UnitPrice = Format(Val(dt2.Rows(I).Item("Rate").ToString), "#########0.00")
                        item_TotAmt = Format(Val(dt2.Rows(I).Item("Amount").ToString), "#########0.00")
                        item_Discount = Format(Val(dt2.Rows(I).Item("Discount_Amount").ToString), "#########0.00")
                        item_PreTaxVal = Format(Val(dt2.Rows(I).Item("Total_Amount").ToString), "#########0.00")
                        item_AssAmt = Format(Val(dt2.Rows(I).Item("Total_Amount").ToString), "#########0.00")
                        item_GstRt = Format(Val(dt2.Rows(I).Item("Gst_Perc").ToString), "#########0.00")
                        vGSTAMT = Format(Val(item_AssAmt) * Val(item_GstRt) / 100, "#########0.00")
                        If Trim(UCase(sellerStcd)) = Trim(UCase(buyerStcd)) Then
                            item_IgstAmt = 0
                            item_CgstAmt = Format(Val(vGSTAMT) / 2, "#########0.00")
                            item_SgstAmt = Format(Val(vGSTAMT) / 2, "#########0.00")

                        Else
                            item_IgstAmt = Format(Val(vGSTAMT), "#########0.00")
                            item_CgstAmt = 0
                            item_SgstAmt = 0

                        End If
                        item_TotItemVal = Format(Val(item_AssAmt) + Val(item_IgstAmt) + Val(item_CgstAmt) + Val(item_SgstAmt), "#########0.00")

                        ' Add item to list
                        Dim newItem As New EInvoiceItem With {
                                                        .SlNo = vSNo.ToString(),
                                                        .PrdDesc = item_Desc,
                                                        .IsServc = item_IsServc,
                                                        .HsnCd = item_HsnCd,
                                                        .Qty = item_Qty,
                                                        .Unit = item_Unit,
                                                        .UnitPrice = item_UnitPrice,
                                                        .TotAmt = item_TotAmt,
                                                        .Discount = item_Discount,
                                                        .PreTaxVal = item_PreTaxVal,
                                                        .AssAmt = item_AssAmt,
                                                        .GstRt = item_GstRt,
                                                        .IgstAmt = item_IgstAmt,
                                                        .CgstAmt = item_CgstAmt,
                                                        .SgstAmt = item_SgstAmt,
                                                        .TotItemVal = item_TotItemVal
                                                    }

                        eInvitems.Add(newItem)


                        Total_AssVal = Format(Val(Total_AssVal) + Val(item_AssAmt), "#########0.00")
                        Total_IgstVal = Format(Val(Total_IgstVal) + Val(item_IgstAmt), "#########0.00")
                        Total_CgstVal = Format(Val(Total_CgstVal) + Val(item_CgstAmt), "#########0.00")
                        Total_SgstVal = Format(Val(Total_SgstVal) + Val(item_SgstAmt), "#########0.00")
                        Total_TotInvVal = Format(Val(Total_TotInvVal) + Val(item_TotItemVal), "#########0.00")

                    Next I



                End If
                dt2.Clear()

            End If
            dt1.Clear()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT GETTING DATA...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



        Try

            EInvoice_JSON_Generator.GenerateEInvoice(docTyp, docNo, docDate,
                    sellerGstin, sellerLglNm, sellerTrdNm, sellerAddr1, sellerAddr2, sellerLoc, sellerPin, sellerStcd,
                    buyerGstin, buyerLglNm, buyerTrdNm, buyerAddr1, buyerAddr2, buyerLoc, buyerPin, buyerStcd,
                    vShiptoIDNO, ShipGstin, ShipLglNm, ShipTrdNm, ShipAddr1, ShipAddr2, ShipLoc, ShipPin, ShipStcd,
                    eInvitems,
                    Total_AssVal, Total_CgstVal, Total_SgstVal, Total_IgstVal, Total_TotInvVal)

            'EInvoice_JSON_Generator.GenerateEInvoice(docTyp, docNo, docDate,
            '                    sellerGstin, sellerLglNm, sellerTrdNm, sellerAddr1, sellerAddr2, sellerLoc, sellerPin, sellerStcd,
            '                    buyerGstin, buyerLglNm, buyerTrdNm, buyerAddr1, buyerAddr2, buyerLoc, buyerPin, buyerStcd,
            '                    vShiptoIDNO, ShipGstin, ShipLglNm, ShipTrdNm, ShipAddr1, ShipAddr2, ShipLoc, ShipPin, ShipStcd,
            '                    item_SlNo, item_Desc, item_IsServc, item_HsnCd, item_Qty, item_Unit, item_UnitPrice, item_TotAmt, item_Discount, item_PreTaxVal, item_AssAmt, item_GstRt, item_IgstAmt, item_CgstAmt, item_SgstAmt, item_TotItemVal,
            '                    Total_AssVal, Total_CgstVal, Total_SgstVal, Total_IgstVal, Total_TotInvVal)


        Catch ex As Exception
            MessageBox.Show(ex.Message, "WHILE JSON GENERATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try




        MessageBox.Show("E-Invoice JSON generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)


    End Sub

    Private Sub txt_Tds_Percentage_KeyUp(sender As Object, e As KeyEventArgs) Handles txt_Tds_Percentage.KeyUp

    End Sub

    Private Sub txt_Tds_Percentage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Tds_Percentage.KeyPress

        If Asc(e.KeyChar) = 13 Then
            cbo_Agent.Focus()
        End If

    End Sub

    Private Sub txt_Tds_Percentage_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Tds_Percentage.KeyDown
        If e.KeyCode = 40 Then
            cbo_Agent.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_TDS_Value.Focus()
        End If


    End Sub

    Private Sub txt_Remarks_TextChanged(sender As Object, e As EventArgs) Handles txt_Remarks.TextChanged

    End Sub

    Private Sub cbo_Agent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Agent.SelectedIndexChanged

    End Sub
End Class
