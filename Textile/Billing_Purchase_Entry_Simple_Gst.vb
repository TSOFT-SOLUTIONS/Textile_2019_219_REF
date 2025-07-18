Public Class Billing_Purchase_Entry_Simple_Gst
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GPURS-"
    Private Pk_Condition2 As String = "CSPYM-"
    Private PkCondition3_TDSBP As String = "TDSBP-"
    Private NoCalc_Status As Boolean = False
    Private Mov_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private cmbItmNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private prn_DetIndx As Integer
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

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As Panel

        New_Entry = False
        Insert_Entry = False
        Mov_Status = False

        lbl_PurchaseNo.Text = ""
        lbl_PurchaseNo.ForeColor = Color.Black
        pnl_GSTTax_Details.Visible = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_AvailableStock.Tag = 0
        lbl_AvailableStock.Text = ""

        lbl_TotalTaxAmount.Text = ""

        txt_Freight.Text = ""
        txt_AddLess_BeforeTax.Text = ""
        cbo_OrderNo.Text = ""
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""
        txt_ExchangeRate.Text = ""
        cbo_Currency.Text = ""
        txt_TDS_TaxableValue.Text = ""
        txt_TDS_TaxableValue.Enabled = False
        chk_TDS_Tax.Checked = True

        txt_TdsPerc.Text = ""
        txt_TdsPerc.Enabled = False
        lbl_TdsAmount.Text = ""

        Chk_Acc_Yes.Text = ""
        Chk_Acc_Yes.Checked = False

        pnl_Bill_Rate.Visible = False


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            txt_Filter_BillNo.Text = ""
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

            ElseIf TypeOf obj Is MaskedTextBox Then
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
                    ElseIf TypeOf ctrl1 Is MaskedTextBox Then
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
                        ElseIf TypeOf ctrl2 Is MaskedTextBox Then
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
                                    ElseIf TypeOf ctrl3 Is MaskedTextBox Then
                                        ctrl3.text = ""
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
            End If

        Next

        txt_VehicleNo.Text = ""

        dgv_Details.Rows.Clear()
        dgv_GSTTax_Details.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Add()

        cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, 21)
        cbo_PaymentMethod.Text = "CREDIT"
        '  cbo_TaxType.Text = "NO TAX"
        txt_SlNo.Text = "1"

        cbo_TaxType.Text = "GST"

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
        chk_TCS_Tax.Checked = True

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim dtp As DateTimePicker
        Dim msk As MaskedTextBox

        On Error Resume Next
        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub
        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is DateTimePicker Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is DateTimePicker Then
            dtp = Me.ActiveControl
            dtp.Select()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msk = Me.ActiveControl
            msk.SelectionStart = 0
        End If

        'If Me.ActiveControl.Name <> cbo_ItemName.Name Then
        '    cbo_ItemName.Visible = False
        'End If
        'If Me.ActiveControl.Name <> cbo_Unit.Name Then
        '    cbo_Unit.Visible = False
        'End If




        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False

        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim vCmpSurNm As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        Mov_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Ledger_Name as PurchaseAcName from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.PurchaseAc_IdNo = c.Ledger_IdNo where a.Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Entry_VAT_GST_Type = 'GST'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_PurchaseNo.Text = dt1.Rows(0).Item("Purchase_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Purchase_Date").ToString
                msk_Date.Text = dtp_Date.Text

                cbo_PaymentMethod.Text = dt1.Rows(0).Item("Payment_Method").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                cbo_PurchaseAc.Text = dt1.Rows(0).Item("PurchaseAcName").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                txt_TotalQty.Text = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                txt_SubTotal.Text = Format(Val(dt1.Rows(0).Item("SubTotal_Amount").ToString), "########0.00")
                txt_TotalDiscAmount.Text = Format(Val(dt1.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00")
                lbl_TotalTaxAmount.Text = Format(Val(dt1.Rows(0).Item("Total_TaxAmount").ToString), "########0.00")
                txt_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                txt_AddLess_AfterTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                txt_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString)) 'Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                cbo_OrderNo.Text = Trim(dt1.Rows(0).Item("Sales_Order_Selection_Code").ToString)
                txt_place_Supply.Text = dt1.Rows(0).Item("Place_Of_Supply").ToString
                txt_Electronic_RefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                txt_Due_Days.Text = dt1.Rows(0).Item("Due_Days").ToString
                cbo_TransportMode.Text = dt1.Rows(0).Item("Transportation_Mode").ToString
                txt_DateTime_Of_Supply.Text = dt1.Rows(0).Item("Date_Time_Of_Supply").ToString
                txt_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_vehicle.Text = Common_Procedures.Vehicle_IdNoToName(con, Val(dt1.Rows(0).Item("Vehicle_idNo").ToString))
                cbo_TaxType.Text = dt1.Rows(0).Item("Entry_GST_Tax_Type").ToString
                lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGst_Amount").ToString), "########0.00")
                lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGst_Amount").ToString), "########0.00")

                lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGst_Amount").ToString), "########0.00")

                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                txt_AddLess_BeforeTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), "########0.00")
                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                cbo_Currency.Text = Common_Procedures.Currency_IdNoToName(con, Val(dt1.Rows(0).Item("Currency_idNo").ToString))
                txt_ExchangeRate.Text = Format(Val(dt1.Rows(0).Item("Exchange_Rate").ToString), "########0.00")


                '----TCS

                If Val(dt1.Rows(0).Item("Chk_AccBill_Yes").ToString) = 1 Then
                    Chk_Acc_Yes.Checked = True
                Else
                    Chk_Acc_Yes.Checked = False
                End If

                'If Val(dt1.Rows(0).Item("Chk_AccBill_No").ToString) = 1 Then
                '    Chk_Acc_No.Checked = True
                'Else
                '    Chk_Acc_No.Checked = False
                'End If
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



                ''''''''''''''Tds
                If IsDBNull(dt1.Rows(0).Item("Tds_Tax_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Tds_Tax_Status").ToString) = 1 Then chk_TDS_Tax.Checked = True Else chk_TDS_Tax.Checked = False
                End If
                If Val(dt1.Rows(0).Item("TDS_Tax_Status").ToString) = 1 Then chk_TDS_Tax.Checked = True Else chk_TDS_Tax.Checked = False
                txt_TDS_TaxableValue.Text = dt1.Rows(0).Item("TDS_Taxable_Value").ToString
                If Val(dt1.Rows(0).Item("EDIT_TDS_TaxableValue").ToString) = 1 Then
                    txt_TdsPerc.Enabled = True
                    txt_TDS_TaxableValue.Enabled = True
                End If
                txt_TdsPerc.Text = Val(dt1.Rows(0).Item("TDS_Percentage").ToString)
                lbl_TdsAmount.Text = dt1.Rows(0).Item("TDS_Amount").ToString






                vCmpSurNm = ""
                da3 = New SqlClient.SqlDataAdapter("Select ch.company_Name from Company_Head ch where ch.company_idno = " & Str(Val(lbl_Company.Tag)), con)
                dt3 = New DataTable
                da3.Fill(dt3)
                If dt3.Rows.Count > 0 Then
                    vCmpSurNm = Common_Procedures.Remove_NonCharacters(dt3.Rows(0).Item("Company_Name").ToString)
                End If
                dt3.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Trim(dt2.Rows(i).Item("Description").ToString)
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Unit_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1142" Then  '---  SLP
                            dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.000")
                        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then  '---  AADHARSH
                            dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00000")
                        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" And Trim(vCmpSurNm) = "NACHIYARACCESSORIEESINC" Then  'Nachiyar
                            dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Rate").ToString)
                        Else
                            dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        End If

                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Tax_Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Discount_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Tax_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Perc_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Amount_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("Assessable_Value").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(16).Value = dt2.Rows(i).Item("HSN_Code").ToString
                        dgv_Details.Rows(n).Cells(17).Value = Format(Val(dt2.Rows(i).Item("GST_Percentage").ToString), "########0.00")

                    Next i

                End If


                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

                da1 = New SqlClient.SqlDataAdapter("Select a.* from Purchase_GST_Tax_Details a Where a.Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
                dt4 = New DataTable
                da1.Fill(dt4)

                With dgv_GSTTax_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = SNo
                            .Rows(n).Cells(1).Value = Trim(dt4.Rows(i).Item("HSN_Code").ToString)
                            .Rows(n).Cells(2).Value = IIf(Val(dt4.Rows(i).Item("Taxable_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("Taxable_Amount").ToString), "############0.00"), "")
                            .Rows(n).Cells(3).Value = IIf(Val(dt4.Rows(i).Item("CGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("CGST_Percentage").ToString), "")
                            .Rows(n).Cells(4).Value = IIf(Val(dt4.Rows(i).Item("CGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("CGST_Amount").ToString), "##########0.00"), "")
                            .Rows(n).Cells(5).Value = IIf(Val(dt4.Rows(i).Item("SGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("SGST_Percentage").ToString), "")
                            .Rows(n).Cells(6).Value = IIf(Val(dt4.Rows(i).Item("SGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("SGST_Amount").ToString), "###########0.00"), "")
                            .Rows(n).Cells(7).Value = IIf(Val(dt4.Rows(i).Item("IGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("IGST_Percentage").ToString), "")
                            .Rows(n).Cells(8).Value = IIf(Val(dt4.Rows(i).Item("IGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("IGST_Amount").ToString), "###########0.00"), "")
                        Next i

                    End If

                End With
                get_Ledger_TotalSales()


                '''''''''''''''Tds
                'If IsDBNull(dt1.Rows(0).Item("Tds_Tax_Status").ToString) = False Then
                '    If Val(dt1.Rows(0).Item("Tds_Tax_Status").ToString) = 1 Then chk_TDS_Tax.Checked = True Else chk_TDS_Tax.Checked = False
                'End If
                'If Val(dt1.Rows(0).Item("TDS_Tax_Status").ToString) = 1 Then chk_TDS_Tax.Checked = True Else chk_TDS_Tax.Checked = False
                'txt_TDS_TaxableValue.Text = dt1.Rows(0).Item("TDS_Taxable_Value").ToString
                'If Val(dt1.Rows(0).Item("EDIT_TDS_TaxableValue").ToString) = 1 Then
                '    txt_TdsPerc.Enabled = True
                '    txt_TDS_TaxableValue.Enabled = True
                'End If
                'txt_TdsPerc.Text = Val(dt1.Rows(0).Item("TDS_Percentage").ToString)

                'lbl_TdsAmount.Text = dt1.Rows(0).Item("TDS_Amount").ToString

                Mov_Status = False
                TotalAmount_Calculation()
                NetAmount_Calculation()

            End If

            Grid_Cell_DeSelect()

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Mov_Status = False

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Purchase_Entry_Simple_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Dim OrdByNo_Code As String = ""
        Dim VouCode As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Currency.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CURRENCY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Currency.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
                        Da1 = New SqlClient.SqlDataAdapter("Select a.For_OrderBy from Purchase_Head a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Purchase_Code = '" & Trim(RptSubReport_VouCode) & "'  and a.Entry_VAT_GST_Type = 'GST'", con)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Purchase_Entry_Simple_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Me.Text = ""

        con.Open()

        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Else
            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        End If

        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt2)
        cbo_ItemName.DataSource = dt2
        cbo_ItemName.DisplayMember = "item_name"

        da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)
        da.Fill(dt3)
        cbo_Unit.DataSource = dt3
        cbo_Unit.DisplayMember = "unit_name"

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 27) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_PurchaseAc.DataSource = dt4
        cbo_PurchaseAc.DisplayMember = "Ledger_DisplayName"

        lbl_Freight.Visible = False
        txt_Freight.Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1071" Then
            lbl_Freight.Visible = True
            txt_Freight.Visible = True
        End If



        If (Common_Procedures.settings.CustomerCode = "1186") Then
            lbl_caption_duedays.Visible = True
            txt_Due_Days.Visible = True
        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("NO TAX")
        cbo_TaxType.Items.Add("GST")

        pnl_GSTTax_Details.Visible = False
        pnl_GSTTax_Details.Left = (Me.Width - pnl_GSTTax_Details.Width) \ 2
        pnl_GSTTax_Details.Top = ((Me.Height - pnl_GSTTax_Details.Height) \ 2) - 100
        pnl_GSTTax_Details.BringToFront()

        pnl_Bill_Rate.Visible = False
        pnl_Bill_Rate.Left = pnl_Back.Left
        pnl_Bill_Rate.Top = pnl_Back.Top + pnl_Back.Height - pnl_Bill_Rate.Height
        pnl_Bill_Rate.BringToFront()


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Electronic_RefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateTime_Of_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Due_Days.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_place_Supply.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofItems.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Description.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tcs_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TcsPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TCS_TaxableValue.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TDS_TaxableValue.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TdsPerc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscountAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalQty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SubTotal.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler dgv_Details.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_AfterTax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RoundOff.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NetAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vehicle.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_BeforeTax.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Delete.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Currency.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ExchangeRate.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_place_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_BeforeTax.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Electronic_RefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateTime_Of_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Due_Days.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Description.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoofItems.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vehicle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscountAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalQty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SubTotal.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler dgv_Details.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_AfterTax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RoundOff.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NetAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Delete.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Currency.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ExchangeRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tcs_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TcsPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TCS_TaxableValue.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TDS_TaxableValue.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TdsPerc.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_BeforeTax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Description.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Electronic_RefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_place_Supply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalQty.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SubTotal.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_Due_Days.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_AfterTax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RoundOff.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NetAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ExchangeRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TDS_TaxableValue.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TdsPerc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_ExchangeRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Electronic_RefNo.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_NoofItems.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_place_Supply.KeyPress, AddressOf TextBoxControlKeyPress


        '  AddHandler txt_Due_Days.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_TDS_TaxableValue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalQty.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SubTotal.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_AfterTax.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RoundOff.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NetAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_BeforeTax.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TcsPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TCS_TaxableValue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tcs_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TdsPerc.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Tcs_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TcsPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TCS_TaxableValue.KeyDown, AddressOf TextBoxControlKeyDown
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2001" Then '---- Demo - Elpro Chem for Vasanth by Deva (Chennai)
            lbl_OrderNo.Visible = True
            cbo_OrderNo.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1355" Then
            cbo_vehicle.Visible = True
            txt_VehicleNo.Visible = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then 'Aadharsh
            lbl_caption_duedays.Visible = False
            lbl_caption_duedays.Tag = "INVISIBLE"

            txt_Due_Days.Visible = False
            txt_Due_Days.Tag = "INVISIBLE"

            lbl_caption_vehicleno.Visible = False
            lbl_caption_vehicleno.Tag = "INVISIBLE"

            txt_VehicleNo.Visible = False
            txt_VehicleNo.Tag = "INVISIBLE"

            cbo_vehicle.Visible = False
            cbo_vehicle.Tag = "INVISIBLE"

            lbl_Caption_Currency.Visible = True
            lbl_Caption_Currency.Tag = ""

            cbo_Currency.Visible = True
            cbo_Currency.Tag = ""

            lbl_ExchangeRate.Visible = True
            lbl_ExchangeRate.Tag = ""

            txt_ExchangeRate.Visible = True
            txt_ExchangeRate.Tag = ""

            lbl_IGstAmount.Enabled = True



        End If

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub

    Private Sub Purchase_Entry_Simple_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next

        Open_Report()
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Purchase_Entry_Simple_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_GSTTax_Details.Visible = True Then
                    btn_Close_GSTTax_Details_Click(sender, e)
                    Exit Sub
                Else
                    Close_Form()

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim NewCode2 As String = ""


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            NewCode2 = Trim((Pk_Condition2)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode2), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition3_TDSBP) & Trim(NewCode), tr)


            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Reference_Code, Reference_Date, Company_Idno, Item_IdNo ) " & _
                                  " Select                               Reference_Code, Reference_Date, Company_IdNo, Item_IdNo from Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Purchase_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Purchase_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then
                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub
            End If

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
            da.Fill(dt2)
            cbo_Filter_ItemName.DataSource = dt2
            cbo_Filter_ItemName.DisplayMember = "item_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            txt_Filter_BillNo.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Purchase_No"
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

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_PurchaseNo.Text))

            da = New SqlClient.SqlDataAdapter("select Purchase_No from Purchase_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Purchase_No", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select Purchase_No from Purchase_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Purchase_No desc"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If
            dr.Close()

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Purchase_No desc", con)
        Dim dt As New DataTable
        Dim movno As String

        Try
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If movno <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0
        Dim Dt1 As New DataTable


        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_PurchaseNo.Text = NewID
            lbl_PurchaseNo.ForeColor = Color.Red


            da = New SqlClient.SqlDataAdapter("select a.*, b.ledger_name as PurchaseAcName from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Purchase_No desc", con)
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                cbo_PaymentMethod.Text = dt2.Rows(0).Item("Payment_Method").ToString
                cbo_PurchaseAc.Text = dt2.Rows(0).Item("PurchaseAcName").ToString
                If dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString
                txt_ExchangeRate.Text = Format(Val(dt2.Rows(0).Item("Exchange_Rate").ToString), "########0.00")

                If IsDBNull(dt2.Rows(0).Item("Tcs_Tax_Status").ToString) = False Then


                    If Val(dt2.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False

                End If

                If IsDBNull(dt2.Rows(0).Item("Tds_Tax_Status").ToString) = False Then
                    If Val(dt2.Rows(0).Item("Tds_Tax_Status").ToString) = 1 Then chk_TDS_Tax.Checked = True Else chk_TDS_Tax.Checked = False
                End If

                If IsDBNull(dt2.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt2.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If
            End If

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Purchase No.", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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
                MessageBox.Show("Purchase No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String
        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Purchase_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Purchase No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Purchase No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_PurchaseNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewCode2 As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim L_id As Integer = 0
        Dim purcac_id As Integer = 0
        Dim TxAc_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim Amt As Single = 0
        Dim TxAmt_Diff As Single = 0, TotTxAmt As Single = 0
        Dim vLed_IdNos As String = "", vVou_Amts As String = ""
        Dim VouBil As String = ""
        Dim Vehicle_id As String = ""
        Dim Curr_id As Integer = 0
        Dim vTDS_AssVal_EditSTS As Integer = 0
        Dim vTDS_Tax_Sts As Integer = 0


        Dim vTCS_AssVal_EditSTS As Integer = 0
        Dim vTCS_Tax_Sts As Integer = 0
        Dim vTCSAmtRndOff_STS As Integer = 0

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Purchase_Entry, New_Entry) = False Then Exit Sub

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If
        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 And Trim(UCase(cbo_PaymentMethod.Text)) = "CREDIT" Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        purcac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)

        If purcac_id = 0 And Val(txt_NetAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If

        If Val(lbl_TcsAmount.Text) <> 0 And Val(lbl_TdsAmount.Text) <> 0 Then
            MessageBox.Show("Invalid TCS/TDS Amount" & Chr(13) & "Bothe TCS and TDS cannot done at same time", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If chk_TCS_Tax.Enabled And chk_TCS_Tax.Visible Then
                chk_TCS_Tax.Focus()
            ElseIf chk_TDS_Tax.Enabled And chk_TDS_Tax.Visible Then
                chk_TDS_Tax.Focus()
            End If
            Exit Sub
        End If

        If Val(txt_NetAmount.Text) = 0 Then txt_NetAmount.Text = 0

        If Trim(txt_BillNo.Text) = "" Then
            MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_BillNo.Enabled Then txt_BillNo.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            da = New SqlClient.SqlDataAdapter("select * from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Ledger_IdNo = " & Str(Val(led_id)) & " and Bill_No = '" & Trim(txt_BillNo.Text) & "' and Purchase_Code <> '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_VAT_GST_Type = 'GST'", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Bill No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
                Exit Sub
            End If
            dt1.Clear()
        End If
        Amount_Calculation(True)

        Dim Vehicle_No As String = ""
        If txt_VehicleNo.Visible = True Then
            Vehicle_No = txt_VehicleNo.Text
        End If
        Vehicle_id = Common_Procedures.Vehicle_NameToIdNo(con, cbo_vehicle.Text)


        Curr_id = Common_Procedures.Currency_NameToIdNo(con, cbo_Currency.Text)

        Dim chk_AccBill_yes As Integer, chk_AccBill_No As Integer
        chk_AccBill_yes = 0
        If Chk_Acc_Yes.Checked = True Then
            chk_AccBill_yes = 1
        End If
        vTCS_Tax_Sts = 0
        If chk_TCS_Tax.Checked = True Then vTCS_Tax_Sts = 1
        vTCS_AssVal_EditSTS = 0
        If txt_TCS_TaxableValue.Enabled = True Then vTCS_AssVal_EditSTS = 1
        vTCSAmtRndOff_STS = 0
        If chk_TCSAmount_RoundOff_STS.Checked = True Then vTCSAmtRndOff_STS = 1


        vTDS_Tax_Sts = 0
        If chk_TDS_Tax.Checked = True Then vTDS_Tax_Sts = 1
        vTDS_AssVal_EditSTS = 0
        If txt_TDS_TaxableValue.Enabled = True Then vTDS_AssVal_EditSTS = 1


        ''''''''''''''Tds
        Debug.Print(chk_TDS_Tax.Checked)
        Debug.Print(txt_TDS_TaxableValue.Text)
        Debug.Print(txt_TdsPerc.Text)
        Debug.Print(lbl_TdsAmount.Text)



        'chk_AccBill_No = 0
        'If Chk_Acc_No.Checked = True Then
        '    chk_AccBill_No = 1
        'End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' ", con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt4)

                NewNo = 0
                If dt4.Rows.Count > 0 Then
                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                        NewNo = Int(Val(dt4.Rows(0)(0).ToString))
                        NewNo = Val(NewNo) + 1
                    End If
                End If
                dt4.Clear()
                If Trim(NewNo) = "" Then NewNo = Trim(lbl_PurchaseNo.Text)

                lbl_PurchaseNo.Text = Trim(NewNo)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PurchaseDate", Convert.ToDateTime(msk_Date.Text))

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()



            If New_Entry = True Then
                If Trim(txt_DateTime_Of_Supply.Text) = "" Then txt_DateTime_Of_Supply.Text = Format(Now, "dd-MM-yyyy hh:mm tt")

                cmd.CommandText = "Insert into Purchase_Head( Entry_VAT_GST_Type  ,             Purchase_Code,                     Company_IdNo,                      Purchase_No,                                     for_OrderBy,                                           Purchase_Date,              Payment_Method,                  Ledger_IdNo,                   PurchaseAc_IdNo,  Tax_Type,            Narration,                     Total_Qty,                        SubTotal_Amount,                           Total_DiscountAmount,                        Total_TaxAmount,                           Gross_Amount,                           CashDiscount_Perc,                 CashDiscount_Amount,                               AddLess_Amount,                         Round_Off,                                Net_Amount,                          Bill_No ,                        Freight_Amount  ,             Sales_Order_Selection_Code,     Electronic_Reference_No   ,                      Transportation_Mode     ,                    Date_Time_Of_Supply   ,                   Entry_GST_Tax_Type ,             CGst_Amount  ,                                SGst_Amount   ,                IGst_Amount,                         Place_Of_Supply,                              Assessable_Value ,               Vehicle_No,                      Due_Days,                  AddLess_BeforeTax_Amount,                       Vehicle_idNo,                    Chk_AccBill_Yes ,               Currency_Idno ,                  Exchange_Rate ,                       Tcs_Name_caption           ,        Tcs_percentage       ,                    Tcs_Amount    ,                     TCS_Taxable_Value,                            EDIT_TCS_TaxableValue ,             Tcs_Tax_Status,             TCSAmount_RoundOff_Status,                         Invoice_Value_Before_TCS ,       RoundOff_Invoice_Value_Before_TCS ,                                           TDS_Taxable_Value ,                         TDS_Percentage,                       TDS_Amount,                            EDIT_TDS_TaxableValue , Tds_Tax_Status) 
                                                          Values ( 'GST '   ,'" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(purcac_id)) & ", 'VAT', '" & Trim(txt_Narration.Text) & "', " & Str(Val(txt_TotalQty.Text)) & ", " & Str(Val(txt_SubTotal.Text)) & ", " & Str(Val(txt_TotalDiscAmount.Text)) & ", " & Str(Val(lbl_TotalTaxAmount.Text)) & ", " & Str(Val(txt_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(txt_AddLess_AfterTax.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(CDbl(txt_NetAmount.Text))) & " , '" & Trim(txt_BillNo.Text) & "' , " & Str(Val(txt_Freight.Text)) & " ,'" & Trim(cbo_OrderNo.Text) & "', '" & Trim(txt_Electronic_RefNo.Text) & "', '" & Trim(cbo_TransportMode.Text) & "', '" & Trim(txt_DateTime_Of_Supply.Text) & "', '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(lbl_CGstAmount.Text)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", " & Str(Val(lbl_IGstAmount.Text)) & ",'" & Trim(txt_place_Supply.Text) & "', " & Str(Val(lbl_Assessable.Text)) & " , '" & Trim(Vehicle_No) & "','" & Trim(txt_Due_Days.Text) & "'," & Str(Val(txt_AddLess_BeforeTax.Text)) & "," & Str(Val(Vehicle_id)) & ", " & Str(Val(chk_AccBill_yes)) & " , " & Str(Val(Curr_id)) & ", " & Str(Val(txt_ExchangeRate.Text)) & ",'" & Trim(txt_Tcs_Name.Text) & "',       " & Str(Val(txt_TcsPerc.Text)) & ",    " & Str(Val(lbl_TcsAmount.Text)) & " ,  " & Str(Val(txt_TCS_TaxableValue.Text)) & ", " & Str(Val(vTCS_AssVal_EditSTS)) & ", " & Str(Val(vTCS_Tax_Sts)) & ", " & Str(Val(vTCSAmtRndOff_STS)) & ", " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & " , " & Str(Val(txt_TDS_TaxableValue.Text)) & ",   " & Str(Val(txt_TdsPerc.Text)) & ", " & Str(Val(lbl_TdsAmount.Text)) & ", " & Str(Val(vTDS_AssVal_EditSTS)) & ", " & Str(Val(vTDS_Tax_Sts)) & " ) "

                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Purchase_Head set Entry_VAT_GST_Type = 'GST' , Purchase_Date = @PurchaseDate, Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", PurchaseAc_IdNo = " & Str(Val(purcac_id)) & ", Tax_Type = 'VAT', Narration = '" & Trim(txt_Narration.Text) & "',Freight_Amount = " & Str(Val(txt_Freight.Text)) & "  ,  Total_Qty = " & Str(Val(txt_TotalQty.Text)) & ", SubTotal_Amount = " & Str(Val(txt_SubTotal.Text)) & ", Total_DiscountAmount = " & Str(Val(txt_TotalDiscAmount.Text)) & ", Total_TaxAmount = " & Str(Val(lbl_TotalTaxAmount.Text)) & ", Gross_Amount = " & Str(Val(txt_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess_AfterTax.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CDbl(txt_NetAmount.Text))) & " ,Bill_No = '" & Trim(txt_BillNo.Text) & "',Sales_Order_Selection_Code ='" & Trim(cbo_OrderNo.Text) & "', Electronic_Reference_No = '" & Trim(txt_Electronic_RefNo.Text) & "' ,  Transportation_Mode = '" & Trim(cbo_TransportMode.Text) & "'  ,  Date_Time_Of_Supply = '" & Trim(txt_DateTime_Of_Supply.Text) & "'  , Entry_GST_Tax_Type = '" & Trim(cbo_TaxType.Text) & "',  CGst_Amount = " & Str(Val(lbl_CGstAmount.Text)) & " , SGst_Amount = " & Str(Val(lbl_SGstAmount.Text)) & " , IGst_Amount = " & Str(Val(lbl_IGstAmount.Text)) & ",Place_Of_Supply = '" & Trim(txt_place_Supply.Text) & "', Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Vehicle_No = '" & Trim(Vehicle_No) & "',Due_Days= '" & Trim(txt_Due_Days.Text) & "' ,AddLess_BeforeTax_Amount=" & Str(Val(txt_AddLess_BeforeTax.Text)) & " ,Vehicle_idNo = '" & Trim(Vehicle_id) & "', Chk_AccBill_Yes =" & Str(Val(chk_AccBill_yes)) & " , Currency_Idno =" & Str(Val(Curr_id)) & " , Exchange_Rate =" & Str(Val(txt_ExchangeRate.Text)) & " ,  Tcs_Name_caption = '" & Trim(txt_Tcs_Name.Text) & "', Tcs_percentage=" & Str(Val(txt_TcsPerc.Text)) & ",Tcs_Amount= " & Str(Val(lbl_TcsAmount.Text)) & " , TCS_Taxable_Value = " & Str(Val(txt_TCS_TaxableValue.Text)) & ", EDIT_TCS_TaxableValue = " & Str(Val(vTCS_AssVal_EditSTS)) & " , Tcs_Tax_Status = " & Str(Val(vTCS_Tax_Sts)) & " , TCSAmount_RoundOff_Status = " & Str(Val(vTCSAmtRndOff_STS)) & " , Invoice_Value_Before_TCS = " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", RoundOff_Invoice_Value_Before_TCS = " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & " ,  TDS_Taxable_Value =  " & Str(Val(txt_TDS_TaxableValue.Text)) & ",    TDS_Percentage =  " & Str(Val(txt_TdsPerc.Text)) & " , TDS_Amount = " & Str(Val(lbl_TdsAmount.Text)) & "  , EDIT_TDS_TaxableValue =  " & Str(Val(vTDS_AssVal_EditSTS)) & " , Tds_Tax_Status = " & Str(Val(vTDS_Tax_Sts)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Reference_Code, Reference_Date, Company_Idno, Item_IdNo ) " & _
                                      " Select                               Reference_Code, Reference_Date, Company_IdNo, Item_IdNo from Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            TxAmt_Diff = 0

            TotTxAmt = 0
            For i = 0 To dgv_Details.RowCount - 1

                If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then
                    TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(11).Value)
                End If

            Next

            TxAmt_Diff = Format(Val(lbl_TotalTaxAmount.Text) - Val(TotTxAmt), "#########0.00")

            cmd.CommandText = "Delete from Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            For i = 0 To dgv_Details.RowCount - 1

                itm_id = 0
                unt_id = 0

                da = New SqlClient.SqlDataAdapter("select item_idno from item_head where item_name = '" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "'", con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt3)

                If dt3.Rows.Count > 0 Then
                    If IsDBNull(dt3.Rows(0)(0).ToString) = False Then
                        itm_id = Val(dt3.Rows(0)(0).ToString)
                    End If
                End If

                dt3.Clear()

                If itm_id <> 0 And Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 And Val(dgv_Details.Rows(i).Cells(12).Value) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select unit_idno from unit_head where unit_name = '" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "'", con)
                    da.SelectCommand.Transaction = tr
                    da.Fill(dt5)

                    If dt5.Rows.Count > 0 Then
                        If IsDBNull(dt5.Rows(0)(0).ToString) = False Then
                            unt_id = Val(dt5.Rows(0)(0).ToString)
                        End If
                    End If

                    dt5.Clear()

                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Purchase_Details(Purchase_Code, Company_IdNo, Purchase_No, for_OrderBy, Purchase_Date, Ledger_IdNo, SL_No, Item_IdNo, Unit_IdNo, Noof_Items, Rate, Tax_Rate, Amount, Discount_Perc, Discount_Amount, Tax_Perc, Tax_Amount, Total_Amount, TaxAmount_Difference, Cash_Discount_Perc_For_All_Item    ,       Cash_Discount_Amount_For_All_Item  ,    Assessable_Value  ,      HSN_Code        ,      GST_Percentage,Description) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(10).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(11).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(12).Value)) & ", " & Str(Val(TxAmt_Diff)) & "," & Str(Val(dgv_Details.Rows(i).Cells(13).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & ", '" & Trim(dgv_Details.Rows(i).Cells(16).Value) & "', " & Str(Val(dgv_Details.Rows(i).Cells(17).Value)) & " ,'" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "' )"
                    cmd.ExecuteNonQuery()

                    If Chk_Acc_Yes.Checked = False Then
                        cmd.CommandText = "Insert into Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Party_Bill_No, SL_No, Item_IdNo, Unit_IdNo, Quantity) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, " & Str(Val(led_id)) & ", '" & Trim(txt_BillNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " )"
                        cmd.ExecuteNonQuery()
                    End If

                    TxAmt_Diff = 0

                End If

            Next

            cmd.CommandText = "Delete from Purchase_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_GSTTax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Purchase_GST_Tax_Details   (        Purchase_Code      ,               Company_IdNo       ,                Purchase_No           ,                               for_OrderBy                                  , Purchase_Date ,         Ledger_IdNo     ,            Sl_No     ,                    HSN_Code            ,                      Taxable_Amount      ,                      CGST_Percentage     ,                      CGST_Amount         ,                      SGST_Percentage      ,                      SGST_Amount         ,                      IGST_Percentage     ,                      IGST_Amount          ) " & _
                                            "          Values                  ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate , " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            Dim vVouPos_IdNos As String = "", vVouPos_Amts As String = "", vVouPos_ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0
            Dim vVouPos_Narr As String = ""
            Dim ErrMsg As String = ""

            'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
            '    AcPos_ID = 1
            'Else
            AcPos_ID = led_id
            'End If
            vVouPos_IdNos = AcPos_ID & "|" & purcac_id & "|25|26|27|" & Common_Procedures.CommonLedger.TCS_PAYABLE_AC

            vVouPos_Amts = (Val(CDbl(txt_NetAmount.Text)) + Val(lbl_TdsAmount.Text)) & "|" & -1 * ((Val(CDbl(txt_NetAmount.Text))) - Val(lbl_CGstAmount.Text) - Val(lbl_SGstAmount.Text) - Val(lbl_IGstAmount.Text) - Val(lbl_TcsAmount.Text) + Val(lbl_TdsAmount.Text)) & "|" & -1 * Val(lbl_CGstAmount.Text) & "|" & -1 * Val(lbl_SGstAmount.Text) & "|" & -1 * Val(lbl_IGstAmount.Text) & "|" & -1 * Val(lbl_TcsAmount.Text)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES
                vVouPos_Narr = "Bill No : " & Trim(txt_BillNo.Text) & "   " & Trim(txt_Narration.Text)
            Else
                vVouPos_Narr = "Bill No : " & Trim(txt_BillNo.Text)
            End If

            If Common_Procedures.Voucher_Updation(con, "Gst.Purc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_PurchaseNo.Text), Trim(msk_Date.Text), vVouPos_Narr, vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVouPos_ErrMsg)
            End If


            'vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Payable_Ac) & "|" & led_id
            'vVou_Amts = Val(lbl_TdsAmount.Text) & "|" & -1 * Val(lbl_TdsAmount.Text)

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If


            NewCode2 = Trim((Pk_Condition2)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode2), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition3_TDSBP) & Trim(NewCode), tr)

            vLed_IdNos = ""
            vVou_Amts = ""
            ErrMsg = ""


            vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Payable_Ac) & "|" & led_id
            vVou_Amts = Val(lbl_TdsAmount.Text) & "|" & -1 * Val(lbl_TdsAmount.Text)

            If Common_Procedures.Voucher_Updation(con, "Purc.Tds", Val(lbl_Company.Tag), Trim(PkCondition3_TDSBP) & Trim(NewCode), Trim(lbl_PurchaseNo.Text), Convert.ToDateTime(msk_Date.Text), "Bill No : " & Trim(txt_BillNo.Text) & " , Purc.No : " & Trim(lbl_PurchaseNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then

                vVouPos_IdNos = AcPos_ID & "|1"
                vVouPos_Amts = -1 * Val(CDbl(txt_NetAmount.Text)) & "|" & Val(CDbl(txt_NetAmount.Text))

                If Common_Procedures.Voucher_Updation(con, "Cash.Pymt", Val(lbl_Company.Tag), Trim(NewCode2), Trim(lbl_PurchaseNo.Text), dtp_Date.Value.Date, "Bill No. : " & Trim(txt_BillNo.Text) & " - Cash Payment", vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                    Throw New ApplicationException(vVouPos_ErrMsg)
                End If
            Else
                ' Bill(Posting)
                VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Trim(msk_Date.Text), led_id, Trim(txt_BillNo.Text), 0, Val(CDbl(txt_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
                If Trim(UCase(VouBil)) = "ERROR" Then
                    Throw New ApplicationException("Error on Voucher Bill Posting")
                End If

            End If



            tr.Commit()

            If Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_PurchaseNo.Text)
                End If
            Else
                move_record(lbl_PurchaseNo.Text)

            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        With cbo_ItemName

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "item_head", "item_Name", "", "(item_idno = 0)")
            cbo_ItemName.Tag = cbo_ItemName.Text
        End With
        Show_Item_CurrentStock()
    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean
        Dim itm_id As Integer = 0

        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim vCmpSurNm As String = ""


        If Trim(cbo_ItemName.Text) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If


        If Trim(cbo_Unit.Text) = "" Then
            MessageBox.Show("Invalid Unit", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If
        If Val(txt_NoofItems.Text) = 0 Then
            MessageBox.Show("Invalid No.of Items", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_NoofItems.Enabled Then txt_NoofItems.Focus()
            Exit Sub
        End If

        If Val(txt_Rate.Text) = 0 And Val(txt_TaxRate.Text) = 0 Then
            MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Rate.Enabled Then txt_Rate.Focus()
            Exit Sub
        End If




        If Val(txt_Amount.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Amount.Enabled Then txt_Amount.Focus()
            Exit Sub
        End If

        vCmpSurNm = ""
        da3 = New SqlClient.SqlDataAdapter("Select ch.company_Name from Company_Head ch where ch.company_idno = " & Str(Val(lbl_Company.Tag)), con)
        dt3 = New DataTable
        da3.Fill(dt3)
        If dt3.Rows.Count > 0 Then
            vCmpSurNm = Common_Procedures.Remove_NonCharacters(dt3.Rows(0).Item("Company_Name").ToString)
        End If
        dt3.Clear()


        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_ItemName.Text
                    .Rows(i).Cells(2).Value = Trim(txt_Description.Text)

                    .Rows(i).Cells(3).Value = cbo_Unit.Text

                    .Rows(i).Cells(4).Value = Val(txt_NoofItems.Text)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1142" Then  '---  SLP
                        .Rows(i).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.000")
                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then 'AADHARSH
                        .Rows(i).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00000")
                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" And Trim(vCmpSurNm) = "NACHIYARACCESSORIEESINC" Then  'Nachiyar
                        .Rows(i).Cells(5).Value = Val(txt_Rate.Text)
                    Else
                        .Rows(i).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                    End If

                    .Rows(i).Cells(6).Value = Format(Val(txt_TaxRate.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(txt_SubAmount.Text), "########0.00")
                    .Rows(i).Cells(8).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(9).Value = Format(Val(txt_DiscountAmount.Text), "########0.00")
                    .Rows(i).Cells(10).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                    .Rows(i).Cells(11).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                    .Rows(i).Cells(12).Value = Format(Val(txt_Amount.Text), "########0.00")
                    .Rows(i).Cells(13).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(14).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")
                    .Rows(i).Cells(15).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")
                    .Rows(i).Cells(16).Value = lbl_Grid_HsnCode.Text
                    .Rows(i).Cells(17).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")
                    '  .Rows(i).Selected = True

                    MtchSTS = True

                    ' If i >= 10 Then .FirstDisplayedScrollingRowIndex = i - 9

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(2).Value = Trim(txt_Description.Text)
                .Rows(n).Cells(3).Value = cbo_Unit.Text
                .Rows(n).Cells(4).Value = Val(txt_NoofItems.Text)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1142" Then  '---  SLP
                    .Rows(n).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.000")
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then 'AADHARSH
                    .Rows(n).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00000")
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" And Trim(vCmpSurNm) = "NACHIYARACCESSORIEESINC" Then
                    .Rows(n).Cells(5).Value = Val(txt_Rate.Text)
                Else
                    .Rows(n).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                End If

                '.Rows(n).Cells(5).Value = Format(Val(txt_TaxRate.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(txt_SubAmount.Text), "########0.00")
                .Rows(n).Cells(8).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(9).Value = Format(Val(txt_DiscountAmount.Text), "########0.00")
                .Rows(n).Cells(10).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                .Rows(n).Cells(11).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                .Rows(n).Cells(12).Value = Format(Val(txt_Amount.Text), "########0.00")
                .Rows(n).Cells(13).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(14).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")
                .Rows(n).Cells(15).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")
                .Rows(n).Cells(16).Value = lbl_Grid_HsnCode.Text
                .Rows(n).Cells(17).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")
                '.Rows(n).Selected = True

                ' If n >= 10 Then .FirstDisplayedScrollingRowIndex = n - 9

            End If

        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_Description.Text = ""

        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        txt_SubAmount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscountAmount.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_Amount.Text = ""
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub txt_NoofItems_Enter(sender As Object, e As System.EventArgs) Handles txt_NoofItems.Enter
        pnl_Bill_Rate.Visible = True
        Bill_RateDetails()
    End Sub

    Private Sub txt_NoofItems_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoofItems.GotFocus
        Show_Item_CurrentStock()
    End Sub

    Private Sub txt_NoofItems_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_NoofItems.KeyDown
        If e.KeyValue = 38 Then
            cbo_Unit.Focus()
        End If
        If e.KeyValue = 40 Then
            txt_Rate.Focus()

        End If
    End Sub

    Private Sub txt_NoofItems_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofItems.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Rate.Focus()
            pnl_Bill_Rate.Visible = True
        End If
    End Sub

    Private Sub txt_NoofItems_Leave(sender As Object, e As System.EventArgs) Handles txt_NoofItems.Leave
        pnl_Bill_Rate.Visible = False
    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoofItems.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub txt_Rate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.GotFocus
        Show_Item_CurrentStock()
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub



    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
        Call Amount_Calculation(False)
    End Sub

    Private Sub txt_Rate_Leave(sender As Object, e As System.EventArgs) Handles txt_Rate.Leave
        pnl_Bill_Rate.Visible = False
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        'If Val(txt_ExchangeRate.Text) <> 0 Then
        '    txt_Rate.Text = txt_ExchangeRate.Text
        'End If

        Call Amount_Calculation(False)
    End Sub



    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        Call Amount_Calculation(False)
    End Sub



    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If

    End Sub

    Private Sub txt_TaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
        Call Amount_Calculation(False)
    End Sub





    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub get_Item_Unit_Rate_TaxPerc(ByVal vItmName As String, ByVal vItmGrpNm As String)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da2 As SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable

        Dim GstTxPer As String = ""
        Dim Led_id As Integer = 0
        Dim vItemID As Integer = 0
        Dim vItemGpId As Integer = 0
        Dim vCmpSurNm As String = ""

        If Trim(vItmName) = "" And Trim(vItmGrpNm) = "" Then Exit Sub


        Led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        vItemID = Val(Common_Procedures.Item_NameToIdNo1(con, Trim(vItmName)))

        vItemGpId = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(vItmGrpNm)))


        If vItemID = 0 And vItemGpId = 0 Then Exit Sub

        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then

            cbo_ItemName.Tag = cbo_ItemName.Text
            da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)("unit_name").ToString) = False Then
                    cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                End If
                If IsDBNull(dt.Rows(0)("CostRate_Excl_Tax").ToString) = False Then
                    txt_Rate.Text = dt.Rows(0)("CostRate_Excl_Tax").ToString
                    'txt_Rate.Text = dt.Rows(0)("Cost_Rate").ToString
                    'If Common_Procedures.settings.CustomerCode = "1247" Then
                    '    GstTxPer = dt.Rows(0)("GST_Percentage").ToString
                    '    txt_Rate.Text = Val(txt_Rate.Text) * 100 \ (100 + GstTxPer)
                    'End If
                End If

                If IsDBNull(dt.Rows(0)("Sale_TaxRate").ToString) = False Then
                    txt_TaxRate.Text = dt.Rows(0)("Sale_TaxRate").ToString
                End If
                If IsDBNull(dt.Rows(0)("Tax_Percentage").ToString) = False Then
                    txt_TaxPerc.Text = dt.Rows(0)("Tax_Percentage").ToString
                End If



            End If
            dt.Dispose()
            da.Dispose()


            vCmpSurNm = ""
            da3 = New SqlClient.SqlDataAdapter("Select ch.company_Name from Company_Head ch where ch.company_idno = " & Str(Val(lbl_Company.Tag)), con)
            dt3 = New DataTable
            da3.Fill(dt3)
            If dt3.Rows.Count > 0 Then
                vCmpSurNm = Common_Procedures.Remove_NonCharacters(dt3.Rows(0).Item("Company_Name").ToString)
            End If
            dt3.Clear()


            Dim vItemRate As String = ""

            vItemRate = ""

            If Trim(Common_Procedures.settings.CustomerCode) = "1365" And Trim(vCmpSurNm) = "NACHIYARACCESSORIEESINC" Then  'Nachiyar
                vItemRate = " a.Item_idNo = " & Str(Val(vItemID))
            Else
                vItemRate = " b.ItemGroup_idNo = " & Str(Val(vItemGpId))
            End If

            da2 = New SqlClient.SqlDataAdapter("select top 1 a.Rate  from Purchase_Details a inner join Item_Head b on a.Item_IdNo = b.item_idno  where a.Ledger_Idno =" & Str(Val(Led_id)) & " and   " & vItemRate & "  Order by a.Purchase_Date desc, a.for_OrderBy desc, a.Purchase_No desc ", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If Trim(Common_Procedures.settings.CustomerCode) = "1365" Then
                    txt_Rate.Text = dt2.Rows(0)("Rate").ToString
                Else
                    txt_Rate.Text = dt.Rows(0)("CostRate_Excl_Tax").ToString
                End If

            Else
                If IsDBNull(dt.Rows(0)("CostRate_Excl_Tax").ToString) = False Then
                    txt_Rate.Text = dt.Rows(0)("CostRate_Excl_Tax").ToString
                End If

            End If
            dt2.Dispose()
            da2.Dispose()

            get_Item_Tax(False)

        End If

    End Sub

    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus
        'If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
        get_Item_Unit_Rate_TaxPerc(cbo_ItemName.Text, "")
        'End If
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
    End Sub

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.GotFocus
        Show_Item_CurrentStock()
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then

            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)

        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Or Trim(Common_Procedures.settings.CustomerCode) = "1133" Or Trim(Common_Procedures.settings.CustomerCode) = "1232" Or Trim(Common_Procedures.settings.CustomerCode) = "1186" Then

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

        Else

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

        End If
        cbo_Ledger.Tag = cbo_Ledger.Text

    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus

        Show_Item_CurrentStock()
        cbo_TaxType.Tag = cbo_TaxType.Text

    End Sub

    Private Sub txt_AddLessAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_AfterTax.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess_AfterTax.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_CashDiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_GrossAmount.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_SlNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SlNo.GotFocus
        Show_Item_CurrentStock()
    End Sub



    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim vCmpSurNm As String = ""


        vCmpSurNm = ""
        da3 = New SqlClient.SqlDataAdapter("Select ch.company_Name from Company_Head ch where ch.company_idno = " & Str(Val(lbl_Company.Tag)), con)
        dt3 = New DataTable
        da3.Fill(dt3)
        If dt3.Rows.Count > 0 Then
            vCmpSurNm = Common_Procedures.Remove_NonCharacters(dt3.Rows(0).Item("Company_Name").ToString)
        End If
        dt3.Clear()

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        cbo_ItemName.Text = Trim(.Rows(i).Cells(1).Value)
                        txt_Description.Text = Trim(.Rows(i).Cells(2).Value)
                        cbo_Unit.Text = Trim(.Rows(i).Cells(3).Value)
                        txt_NoofItems.Text = Val(.Rows(i).Cells(4).Value)

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" And Trim(vCmpSurNm) = "NACHIYARACCESSORIEESINC" Then
                            txt_Rate.Text = Val(txt_Rate.Text)
                        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1142" Then  '---  SLP
                            txt_Rate.Text = Format(Val(txt_Rate.Text), "########0.000")
                        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then 'AADHARSH
                            txt_Rate.Text = Format(Val(txt_Rate.Text), "########0.00000")
                        Else
                            txt_Rate.Text = Format(Val(.Rows(i).Cells(5).Value), "########0.00")
                        End If

                        txt_TaxRate.Text = Format(Val(.Rows(i).Cells(6).Value), "########0.00")
                        txt_SubAmount.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.00")
                        txt_DiscPerc.Text = Format(Val(.Rows(i).Cells(8).Value), "########0.00")
                        txt_DiscountAmount.Text = Format(Val(.Rows(i).Cells(9).Value), "########0.00")
                        txt_TaxPerc.Text = Format(Val(.Rows(i).Cells(10).Value), "########0.00")
                        txt_TaxAmount.Text = Format(Val(.Rows(i).Cells(11).Value), "########0.00")
                        txt_Amount.Text = Format(Val(.Rows(i).Cells(12).Value), "########0.00")
                        lbl_Grid_DiscPerc.Text = Format(Val(.Rows(i).Cells(13).Value), "########0.00")
                        lbl_Grid_DiscAmount.Text = Format(Val(.Rows(i).Cells(14).Value), "########0.00")
                        lbl_Grid_AssessableValue.Text = Format(Val(.Rows(i).Cells(15).Value), "########0.00")
                        lbl_Grid_HsnCode.Text = Trim(.Rows(i).Cells(16).Value)
                        lbl_Grid_GstPerc.Text = Trim(.Rows(i).Cells(17).Value)
                        Exit For

                    End If

                Next

            End With

            SendKeys.Send("{TAB}")

        End If
    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus

        Show_Item_CurrentStock()
    End Sub





    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyUp
        txt_Rate.Text = Format(Val(txt_TaxRate.Text) * (100 / (100 + Val(txt_TaxPerc.Text))), "#########0.00")
        Amount_Calculation(True)
    End Sub

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        'If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
    End Sub



    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Purchase_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument1.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , C.*,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo where a.Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                da2.Fill(prn_DetDt)

                da2.Dispose()

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "" Then
        Printing_Format1(e)
        'End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim vNoofHsnCodes As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'PageSetupDialog1.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 65
            .Right = 50
            .Top = 40
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
        TxtHgt = 17.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 16

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr(0) = 0
        ClArr(1) = 45 : ClArr(2) = 205 : ClArr(3) = 80 : ClArr(4) = 50 : ClArr(5) = 75 : ClArr(6) = 50 : ClArr(7) = 75
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        'Cmp_Name = "TSOFT SOLUTIONS"
        'Cmp_Add1 = "4, IIIrd floor, R.A Tower"
        'Cmp_Add2 = "P.N Road, Tirupur - 2."
        'Cmp_PhNo = "PHONE : 96293 37417"
        'Cmp_TinNo = "TIN NO. : 33554488556"
        'Cmp_CstNo = "CST NO. : 998875 Dt. 01-04-2015"

        'If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
        '    Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
        '    Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        'End If


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                'If Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Total_TaxAmount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                ' If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
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

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            DetSNo = DetSNo + 1


                            ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Item_Name").ToString) & "-" & Trim(prn_DetDt.Rows(DetIndx).Item("Description").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 30 Then
                                For I = 30 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 30
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If


                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 25, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("GST_Percentage").ToString) & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim strHeight As Single
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String
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
        Dim LedNmAr(10) As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        p1Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "PURCHASE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
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

            ' End If

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
            Common_Procedures.Print_To_PrintDocument(e, "Purchase No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Purchase_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt


            BlockInvNoY = BlockInvNoY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Purchase_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)

            BlockInvNoY = BlockInvNoY + TxtHgt
            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Electronic Ref.No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            End If
            BlockInvNoY = BlockInvNoY + TxtHgt
            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString) <> "" Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Issue", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY, 0, 0, p1Font)
            End If
            BlockInvNoY = BlockInvNoY + TxtHgt
            ' e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, BlockInvNoY + 10, PageWidth, BlockInvNoY + 10)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
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
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N) : Y", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Sub Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then

                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

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
            Dim rndoff As Double = 0

            If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString) <> 0 Then

                    rndoff = Val(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString)
                    If Val(rndoff) <> 0 Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, 0, 0, pFont)
                        If Val(rndoff) >= 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    End If

                    'CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                End If

                CurY = CurY + TxtHgt + 2
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                CurY = CurY - 15 + 2

                p1Font = New Font("CAlibiri", 11, FontStyle.Bold)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL INVOICE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString))), PageWidth - 10, CurY, 1, 0, p1Font)

                CurY = CurY + 5

                If Val(prn_HdDt.Rows(0).Item("Invoice_Value_Before_TCS").ToString) <> Val(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString) Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "TCS TAXABLE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("TCS_Taxable_Value").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TCs_name_caption").ToString & "  @ " & Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            End If

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

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            If vNoofHsnCodes <> 0 Then
                Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            End If

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & (prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Purchase_GST_Tax_Details Where Purchase_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
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

        Da = New SqlClient.SqlDataAdapter("Select * from Purchase_GST_Tax_Details Where Purchase_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Purchase_GST_Tax_Details Where Purchase_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
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
    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
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
                Condt = "a.Purchase_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Purchase_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Purchase_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                da = New SqlClient.SqlDataAdapter("select item_idno from item_head where item_name = '" & Trim(cbo_Filter_ItemName.Text) & "'", con)
                da.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                        Itm_IdNo = Val(dt2.Rows(0)(0).ToString)
                    End If
                End If

                dt2.Clear()
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Purchase_Code IN (select z.Purchase_Code from Purchase_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            If Trim(txt_Filter_BillNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Bill_No = '" & Trim(txt_Filter_BillNo.Text) & "' "
            End If

            da = New SqlClient.SqlDataAdapter("select a.Purchase_No, a.Purchase_Date, a.Bill_No, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Purchase_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Purchase_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Purchase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Purchase_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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

    Private Sub Open_FilterEntry()
        Dim movno As String

        If dgv_Filter_Details.Rows.Count > 0 Then
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
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

    Private Sub cbo_PaymentMethod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentMethod.LostFocus

        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If

    End Sub

    Private Sub cbo_PurchaseAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
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

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick


        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim vCmpSurNm As String = ""

        vCmpSurNm = ""
        da3 = New SqlClient.SqlDataAdapter("Select ch.company_Name from Company_Head ch where ch.company_idno = " & Str(Val(lbl_Company.Tag)), con)
        dt3 = New DataTable
        da3.Fill(dt3)
        If dt3.Rows.Count > 0 Then
            vCmpSurNm = Common_Procedures.Remove_NonCharacters(dt3.Rows(0).Item("Company_Name").ToString)
        End If
        dt3.Clear()

        Try
            If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

                txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                txt_Description.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)

                cbo_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
                txt_NoofItems.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1142" Then  '---  SLP
                    txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.000")
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then 'AADHARSH
                    txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00000")
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" And Trim(vCmpSurNm) = "NACHIYARACCESSORIEESINC" Then
                    txt_Rate.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
                Else
                    txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                End If

                txt_TaxRate.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
                txt_SubAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
                txt_DiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
                txt_DiscountAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
                txt_TaxPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
                txt_TaxAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(11).Value), "########0.00")

                txt_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(12).Value), "########0.00")
                lbl_Grid_DiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0.00")
                lbl_Grid_DiscAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(14).Value), "########0.00")
                lbl_Grid_AssessableValue.Text = Format(Val(dgv_Details.CurrentRow.Cells(15).Value), "########0.00")
                lbl_Grid_HsnCode.Text = Trim(dgv_Details.CurrentRow.Cells(16).Value)
                lbl_Grid_GstPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(17).Value), "########0.00")
                If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()


            End If

        Catch ex As Exception
            '--

        End Try



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
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_Description.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        txt_SubAmount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscountAmount.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_Amount.Text = ""
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_GstPerc.Text = ""
        lbl_Grid_HsnCode.Text = ""
        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .Rows.Count - 1
                        .Rows(n).Cells(0).Value = i + 1
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

            TotalAmount_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            cbo_ItemName.Text = ""
            cbo_Unit.Text = ""
            txt_Description.Text = ""

            txt_NoofItems.Text = ""
            txt_Rate.Text = ""
            txt_TaxRate.Text = ""
            txt_SubAmount.Text = ""
            txt_DiscPerc.Text = ""
            txt_DiscountAmount.Text = ""
            txt_TaxPerc.Text = ""
            txt_TaxAmount.Text = ""
            txt_Amount.Text = ""
            lbl_Grid_DiscPerc.Text = ""
            lbl_Grid_DiscAmount.Text = ""
            lbl_Grid_AssessableValue.Text = ""
            lbl_Grid_GstPerc.Text = ""
            lbl_Grid_HsnCode.Text = ""
            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

        End If

    End Sub


    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        Dim SubAmt As Single
        Dim DiscAmt As Single
        Dim TxPerc As Single
        Dim TxAmt As Single
        Dim TotAmt As Single

        For i = 0 To dgv_Details.RowCount - 1
            SubAmt = Val(dgv_Details.Rows(i).Cells(7).Value)
            DiscAmt = Val(dgv_Details.Rows(i).Cells(9).Value)
            TxPerc = Val(dgv_Details.Rows(i).Cells(10).Value)

            TxAmt = 0
            If Trim(cbo_TaxType.Text) <> "GST" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
                TxAmt = Format((Val(SubAmt) - Val(DiscAmt)) * Val(TxPerc) / 100, "#########0.00")
            End If

            TotAmt = Val(SubAmt) - Val(DiscAmt) + Val(TxAmt)

            dgv_Details.Rows(i).Cells(11).Value = Trim(Format(Val(TxAmt), "#########0.00"))
            dgv_Details.Rows(i).Cells(12).Value = Trim(Format(Val(TotAmt), "#########0.00"))

        Next

        TotalAmount_Calculation()
    End Sub

    Private Sub Show_Item_CurrentStock()
        Dim vItemID As Integer
        Dim CurStk As Decimal

        If Trim(cbo_ItemName.Text) <> "" Then
            vItemID = Common_Procedures.Item_NameToIdNo1(con, cbo_ItemName.Text)
            If Val(lbl_AvailableStock.Tag) <> Val(vItemID) Then
                lbl_AvailableStock.Tag = 0
                lbl_AvailableStock.Text = ""
                If Val(vItemID) <> 0 Then
                    CurStk = Common_Procedures.get_Item_CurrentStock(con, Val(lbl_Company.Tag), vItemID)
                    lbl_AvailableStock.Tag = vItemID
                    lbl_AvailableStock.Text = CurStk
                End If
            End If

        Else
            lbl_AvailableStock.Tag = 0
            lbl_AvailableStock.Text = ""

        End If
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Or Trim(Common_Procedures.settings.CustomerCode) = "1133" Or Trim(Common_Procedures.settings.CustomerCode) = "1232" Or Trim(Common_Procedures.settings.CustomerCode) = "1186" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Or Trim(Common_Procedures.settings.CustomerCode) = "1133" Or Trim(Common_Procedures.settings.CustomerCode) = "1232" Or Trim(Common_Procedures.settings.CustomerCode) = "1186" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        End If
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                cbo_Ledger.Tag = cbo_Ledger.Text
                Amount_Calculation(True)
            End If
            get_Ledger_TotalSales()

        End If
    End Sub

    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, cbo_Ledger, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_PaymentMethod, txt_Electronic_RefNo, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_Electronic_RefNo, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
                cbo_TaxType.Tag = cbo_TaxType.Text
                Amount_Calculation(True)
            End If
        End If
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, txt_SlNo, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_ItemName.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_BillNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        If Asc(e.KeyChar) = 13 Then
            Show_Item_CurrentStock()
            If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
                get_Item_Unit_Rate_TaxPerc(cbo_ItemName.Text, "")
            End If
            If Trim(cbo_ItemName.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_BillNo.Focus()
            End If
            get_Ledger_TotalSales()

        End If

    End Sub


    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, cbo_ItemName, txt_NoofItems, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, txt_NoofItems, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")
    End Sub

    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, cbo_PurchaseAc, cbo_TaxType, "", "", "", "")
    End Sub

    Private Sub cbo_paymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, cbo_TaxType, "", "", "", "")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, txt_Filter_BillNo, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, txt_Filter_BillNo, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_ItemName.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub lbl_TotalTaxAmount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_TotalTaxAmount.Click
        Dim VtAmt As String = ""

        VtAmt = InputBox("Enter vat Amount :", "FOR VAT AMOUNT ALTERATION....", Val(lbl_TotalTaxAmount.Text))

        If Trim(VtAmt) <> "" Then
            If Val(VtAmt) <> 0 Then
                lbl_TotalTaxAmount.Text = Format(Val(VtAmt), "#########0.00")

                txt_GrossAmount.Text = Format(Val(txt_SubTotal.Text) - Val(txt_TotalDiscAmount.Text) + Val(lbl_TotalTaxAmount.Text), "########0.00")

                NetAmount_Calculation()
            End If
        End If

        If txt_CashDiscPerc.Visible And txt_CashDiscPerc.Enabled Then txt_CashDiscPerc.Focus()

    End Sub

    Private Sub txt_Filter_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Filter_BillNo.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_Filter_ItemName.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : btn_Filter_Show.Focus()  ' SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Filter_BillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_BillNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If

    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub cbo_OrderNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OrderNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Order_Selection_Code_Head", "Order_Selection_Code", "", "")

    End Sub

    Private Sub cbo_OrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OrderNo.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OrderNo, cbo_TaxType, txt_SlNo, "Order_Selection_Code_Head", "Order_Selection_Code", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_OrderNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OrderNo.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OrderNo, txt_SlNo, "Order_Selection_Code_Head", "Order_Selection_Code", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportMode, txt_Electronic_RefNo, txt_place_Supply, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportMode.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportMode, txt_place_Supply, "", "", "", "", False)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            Amount_Calculation(True)
        End If
    End Sub

    Private Sub get_Item_Tax(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

            lbl_Grid_GstPerc.Text = ""
            lbl_Grid_HsnCode.Text = ""

            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

                ItmIdNo = Common_Procedures.Item_NameToIdNo1(con, cbo_ItemName.Text)

                lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")

                da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                        lbl_Grid_HsnCode.Text = dt.Rows(0)("Item_HSN_Code").ToString
                    End If
                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                        lbl_Grid_GstPerc.Text = dt.Rows(0)("Item_GST_Percentage").ToString
                    End If
                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

            End If

            Amount_Calculation(False)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT GET ITEM TAX DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub



    Private Sub cbo_TaxType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.LostFocus
        If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
            cbo_TaxType.Tag = cbo_TaxType.Text
            Amount_Calculation(True)
        End If
    End Sub

    Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
        Amount_Calculation(True)
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub Amount_Calculation(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0

        '***** GST START *****

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        If GridAll_Row_STS = True Then

            With dgv_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        ItmIdNo = Common_Procedures.Item_NameToIdNo1(con, .Rows(i).Cells(1).Value)
                        If ItmIdNo <> 0 Then

                            .Rows(i).Cells(16).Value = ""
                            .Rows(i).Cells(17).Value = ""

                            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                                da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                                dt = New DataTable
                                da.Fill(dt)
                                If dt.Rows.Count > 0 Then

                                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                                        .Rows(i).Cells(16).Value = dt.Rows(0)("Item_HSN_Code").ToString
                                    End If
                                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                                        .Rows(i).Cells(17).Value = Format(Val(dt.Rows(0)("Item_GST_Percentage").ToString), "#########0.00")
                                    End If

                                End If
                                dt.Clear()

                            End If


                            .Rows(i).Cells(7).Value = Format(Val(.Rows(i).Cells(4).Value) * Val(.Rows(i).Cells(5).Value), "#########0.00")
                            .Rows(i).Cells(9).Value = Format(Val(.Rows(i).Cells(7).Value) * Val(.Rows(i).Cells(8).Value) / 100, "#########0.00")
                            .Rows(i).Cells(12).Value = Format(Val(.Rows(i).Cells(7).Value) - Val(.Rows(i).Cells(9).Value) + Val(.Rows(i).Cells(11).Value), "#########0.00")
                            .Rows(i).Cells(13).Value = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
                            .Rows(i).Cells(14).Value = Format(Val(.Rows(i).Cells(7).Value) * Val(.Rows(i).Cells(13).Value) / 100, "#########0.00")
                            .Rows(i).Cells(15).Value = Format(Val(.Rows(i).Cells(7).Value) - Val(.Rows(i).Cells(14).Value) - Val(.Rows(i).Cells(9).Value), "#########0.00")

                        End If

                    End If

                Next

            End With

            TotalAmount_Calculation()

        Else

            ' txt_SubAmount.Text = Val(txt_NoofItems.Text) * Val(txt_Rate.Text)

            If Val(txt_ExchangeRate.Text) <> 0 Then
                txt_SubAmount.Text = Val(txt_NoofItems.Text) * Val(txt_Rate.Text) * Val(txt_ExchangeRate.Text)
            Else
                txt_SubAmount.Text = Val(txt_NoofItems.Text) * Val(txt_Rate.Text)
            End If

            txt_DiscountAmount.Text = Format(Val(txt_SubAmount.Text) * Val(txt_DiscPerc.Text) / 100, "#########0.00")
            txt_Amount.Text = Format(Val(txt_SubAmount.Text) - Val(txt_DiscountAmount.Text) + Val(txt_TaxAmount.Text), "########0.00")


            lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
            lbl_Grid_DiscAmount.Text = Format(Val(txt_SubAmount.Text) * Val(lbl_Grid_DiscPerc.Text) / 100, "#########0.00")

            lbl_Grid_AssessableValue.Text = Format(Val(txt_SubAmount.Text) - Val(lbl_Grid_DiscAmount.Text), "#########0.00")

        End If



    End Sub

    Private Sub TotalAmount_Calculation()
        Dim Sno As Integer = 0
        Dim TotQty As Decimal = 0
        Dim TotGrsAmt As Decimal = 0
        Dim TotDiscAmt As Decimal = 0
        Dim TotAssval As Decimal = 0
        Dim TotCGstAmt As Decimal = 0
        Dim TotSGstAmt As Decimal = 0
        Dim TotIGstAmt As Decimal = 0
        Dim TotFotDisAmt As Decimal = 0
        Dim TotSubAmt As Decimal, TotTxAmt As Decimal, TotAmt As Decimal

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        Sno = 0
        TotQty = 0
        TotSubAmt = 0
        TotDiscAmt = 0
        TotTxAmt = 0
        TotAmt = 0
        TotGrsAmt = 0
        TotFotDisAmt = 0
        TotAssval = 0
        For i = 0 To dgv_Details.RowCount - 1
            Sno = Sno + 1
            dgv_Details.Rows(i).Cells(0).Value = Sno

            TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(4).Value)
            TotSubAmt = TotSubAmt + Val(dgv_Details.Rows(i).Cells(7).Value)
            TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(9).Value)
            TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(11).Value)
            TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(12).Value)
            TotFotDisAmt = TotFotDisAmt + Val(dgv_Details.Rows(i).Cells(14).Value)
            TotAssval = TotAssval + Val(dgv_Details.Rows(i).Cells(15).Value)
        Next


        txt_TotalQty.Text = Val(TotQty)
        txt_SubTotal.Text = Format(TotSubAmt, "########0.00")
        txt_TotalDiscAmount.Text = Format(TotDiscAmt, "########0.00")
        lbl_TotalTaxAmount.Text = Format(TotTxAmt, "########0.00")
        txt_GrossAmount.Text = Format(TotAmt, "########0.00")
        txt_CashDiscAmount.Text = Format(TotFotDisAmt, "########0.00")

        Get_HSN_CodeWise_GSTTax_Details()

        TotAssval = 0
        TotCGstAmt = 0
        TotSGstAmt = 0
        TotIGstAmt = 0
        With dgv_GSTTax_Details_Total
            If .RowCount > 0 Then
                TotAssval = Val(.Rows(0).Cells(2).Value)
                TotCGstAmt = Val(.Rows(0).Cells(4).Value)
                TotSGstAmt = Val(.Rows(0).Cells(6).Value)
                TotIGstAmt = Val(.Rows(0).Cells(8).Value)
            End If
        End With

        lbl_Assessable.Text = Format(TotAssval, "########0.00")


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1209" Then '------AADHARSH INTERNATIONAL
            lbl_IGstAmount.Text = Format(TotIGstAmt, "########0.00")
            lbl_CGstAmount.Text = Format(TotCGstAmt, "########0.00")
            lbl_SGstAmount.Text = Format(TotSGstAmt, "########0.00")
        End If

        Gross_Discount_Tax_Amount_Calculation()

    End Sub

    Private Sub Gross_Discount_Tax_Amount_Calculation()
        '***** GST START *****
        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        txt_TaxAmount.Text = "0.00"
        If Trim(cbo_TaxType.Text) <> "GST" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
            txt_TaxAmount.Text = Format((Val(txt_SubAmount.Text) - Val(txt_DiscountAmount.Text)) * Val(txt_TaxPerc.Text) / 100, "#########0.00")
        End If
        NetAmount_Calculation()
        '***** GST END *****
    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As String = ""

        Dim vTCS_AssVal As String = 0
        Dim vTOT_SalAmt As String = 0
        Dim vTCS_Amt As String = 0
        Dim vInvAmt_Bfr_TCS As String = 0
        Dim Tax_Amt As Double = 0
        Dim vTDS_Amt As String = 0
        Dim vTDS_AssVal As String = 0

        Dim vTCS_StartDate As Date = #9/30/2020#
        Dim vMIN_TCS_assval As String = "5000000"

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        Tax_Amt = Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text)
        If chk_TCS_Tax.Checked = True Then

            If DateDiff("d", vTCS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                If txt_TCS_TaxableValue.Enabled = False Then

                    vTOT_SalAmt = Format(Val(lbl_Assessable.Text) + Val(Tax_Amt), "###########0")

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
                            txt_TcsPerc.Text = "0.075"
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

        vInvAmt_Bfr_TCS = Format(Val(lbl_Assessable.Text) + Val(Tax_Amt), "###########0.00")
        lbl_Invoice_Value_Before_TCS.Text = Format(Val(vInvAmt_Bfr_TCS), "###########0")
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = Format(Val(lbl_Invoice_Value_Before_TCS.Text) - Val(vInvAmt_Bfr_TCS), "###########0.00")


        txt_CashDiscAmount.Text = Format(Val(txt_SubTotal.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")


        NtAmt = Format(Val(txt_Freight.Text) + Val(txt_SubTotal.Text) - Val(txt_TotalDiscAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_AddLess_AfterTax.Text) + Val(txt_AddLess_BeforeTax.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(lbl_TcsAmount.Text), "########0.00")

        'NtAmt = Format(Val(txt_Freight.Text) + Val(txt_SubTotal.Text) - Val(txt_TotalDiscAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_AddLess_AfterTax.Text) + Val(txt_AddLess_BeforeTax.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_RoundOff.Text), "########0.00")
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1011" Then '---- Chellam Batteries (Thekkalur)
        '    txt_NetAmount.Text = Format(Val(NtAmt), "#########0.00")
        '    txt_RoundOff.Text = 0

        'Else
        txt_NetAmount.Text = Format(Val(NtAmt), "#########0")
        txt_RoundOff.Text = Format(Format(Val(NtAmt), "#########0") - Val(NtAmt), "#########0.00")

        'End If

        Dim vTDS_StartDate As Date = #6/30/2021#

        If chk_TDS_Tax.Checked = True Then

            If DateDiff("d", vTDS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                If txt_TDS_TaxableValue.Enabled = False Then

                    vTOT_SalAmt = Format(Val(lbl_Assessable.Text), "###########0")

                    vTDS_AssVal = 0

                    If Val(CDbl(lbl_TotalSales_Amount_Previous_Year.Text)) > Val(vMIN_TCS_assval) Then
                        vTDS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) > Val(vMIN_TCS_assval) Then
                        vTDS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf (Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt)) > Val(vMIN_TCS_assval) Then
                        vTDS_AssVal = Format(Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt) - Val(vMIN_TCS_assval), "############0")

                    End If

                    If Common_Procedures.settings.CustomerCode = "1087" Then ' Kalaimagal Palladam
                        txt_TDS_TaxableValue.Text = Format(Val(txt_GrossAmount.Text), "############0.00")
                    Else
                        txt_TDS_TaxableValue.Text = Format(Val(vTDS_AssVal), "############0.00")
                    End If


                    If Val(txt_TDS_TaxableValue.Text) > 0 Then
                        If Val(txt_TdsPerc.Text) = 0 Then
                            txt_TdsPerc.Text = "0.1"
                        End If
                    End If

                End If

                If Common_Procedures.settings.CustomerCode = "1087" Then ' Kalaimagal Palladam
                    vTDS_Amt = Format(Val(txt_GrossAmount.Text) * Val(txt_TdsPerc.Text) / 100, "##########0")
                Else
                    vTDS_Amt = Format(Val(txt_TDS_TaxableValue.Text) * Val(txt_TdsPerc.Text) / 100, "##########0")
                End If

                lbl_TdsAmount.Text = Format(Val(vTDS_Amt), "##########0.00")

            Else

                txt_TDS_TaxableValue.Text = ""
                txt_TdsPerc.Text = ""
                lbl_TdsAmount.Text = ""

            End If

        Else

            txt_TDS_TaxableValue.Text = ""
            txt_TdsPerc.Text = ""
            lbl_TdsAmount.Text = ""

        End If

        NtAmt = Format(Val(CDbl(txt_NetAmount.Text)) - Val(lbl_TdsAmount.Text), "##########0.00")

        txt_NetAmount.Text = Format(Val(NtAmt), "##########0")
        txt_NetAmount.Text = Common_Procedures.Currency_Format(Val(CDbl(txt_NetAmount.Text)))

        'lbl_AmountInWords.Text = "Rupees  :  "
        'If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
        '    lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        'End If



    End Sub

    Private Sub btn_GSTTax_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_GSTTax_Details.Click
        pnl_GSTTax_Details.Visible = True
        pnl_Back.Enabled = False
        pnl_GSTTax_Details.Focus()
    End Sub

    Private Sub btn_Close_GSTTax_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_GSTTax_Details.Click
        pnl_Back.Enabled = True
        pnl_GSTTax_Details.Visible = False
    End Sub

    Private Sub Get_HSN_CodeWise_GSTTax_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim AssVal_Frgt_Othr_Charges As Double = 0
        Dim LedIdNo As Integer = 0
        Dim ItmIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

            LedIdNo = 0
            InterStateStatus = False
            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

            End If

            AssVal_Frgt_Othr_Charges = Val(txt_Freight.Text) + Val(txt_AddLess_BeforeTax.Text)

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            With dgv_Details

                If .Rows.Count > 0 Then
                    For i = 0 To .Rows.Count - 1

                        If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(17).Value) <> 0 Then
                            '  If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(15).Value) <> "" And Val(.Rows(i).Cells(16).Value) <> 0 Then
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1                ,                   Currency1            ,                       Currency2                                      ) " & _
                                              "            Values    ( '" & Trim(.Rows(i).Cells(16).Value) & "', " & (Val(.Rows(i).Cells(17).Value)) & ", " & Str(Val(.Rows(i).Cells(15).Value) + AssVal_Frgt_Othr_Charges) & " ) "
                            cmd.ExecuteNonQuery()

                            AssVal_Frgt_Othr_Charges = 0

                        End If

                    Next
                End If
            End With

            With dgv_GSTTax_Details

                .Rows.Clear()
                Sno = 0

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as Assessable_Value from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 ", con)
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

                    Next i

                End If

                dt.Clear()
                dt.Dispose()
                da.Dispose()

            End With

            Total_GSTTax_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Private Sub Total_GSTTax_Calculation()
        Dim Sno As Integer
        Dim TotAss_Val As Single
        Dim TotCGST_amt As Single
        Dim TotSGST_amt As Double
        Dim TotIGST_amt As Double



        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        Sno = 0
        TotAss_Val = 0 : TotCGST_amt = 0 : TotSGST_amt = 0 : TotIGST_amt = 0

        With dgv_GSTTax_Details

            For i = 0 To .RowCount - 1

                Sno = Sno + 1

                .Rows(i).Cells(0).Value = Sno

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotAss_Val = TotAss_Val + Val(.Rows(i).Cells(2).Value())
                    TotCGST_amt = TotCGST_amt + Val(.Rows(i).Cells(4).Value())
                    TotSGST_amt = TotSGST_amt + Val(.Rows(i).Cells(6).Value())
                    TotIGST_amt = TotIGST_amt + Val(.Rows(i).Cells(8).Value())

                End If

            Next i

        End With


        With dgv_GSTTax_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(TotAss_Val), "########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotCGST_amt), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotSGST_amt), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotIGST_amt), "########0.00")
        End With


    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DateTime_Of_Supply.KeyDown
        If e.KeyCode = 40 Then
            If cbo_OrderNo.Visible = True Then
                cbo_OrderNo.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If
        If e.KeyCode = 38 Then
            txt_place_Supply.Focus()
        End If


    End Sub

    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 38 Then
            If cbo_OrderNo.Visible = True Then
                cbo_OrderNo.Focus()
            ElseIf txt_ExchangeRate.Visible = True Then
                txt_ExchangeRate.Focus()
            Else
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1355" Then
                    cbo_vehicle.Focus()
                Else
                    txt_VehicleNo.Focus()
                    'txt_DateTime_Of_Supply.Focus()
                End If

            End If
        End If
        If e.KeyCode = 40 Then
            cbo_ItemName.Focus()
        End If


    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DateTime_Of_Supply.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_OrderNo.Visible = True Then
                cbo_OrderNo.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If
    End Sub

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

            Da = New SqlClient.SqlDataAdapter("Select * from Purchase_GST_Tax_Details Where Purchase_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
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
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If Asc(e.KeyCode) = 13 Then e.Handled = True : msk_Date.Focus()
    End Sub

    Private Sub dtp_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.LostFocus
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vmskOldText = ""
        vmskSelStrt = -1
        If IsDate(msk_Date.Text) = True Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If
    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If UCase(Chr(Asc(e.KeyChar))) = "D" Then
            msk_Date.Text = Date.Today
        End If
        If Asc(e.KeyChar) = 13 Then e.Handled = True : cbo_Ledger.Focus()
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
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

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus
        If IsDate(dtp_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(msk_Date.Text) <= 31 And Microsoft.VisualBasic.DateAndTime.Month(msk_Date.Text) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(msk_Date.Text) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(msk_Date.Text) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If
        End If
    End Sub

    Private Sub txt_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VehicleNo.KeyDown
        If e.KeyCode = 40 Then
            If cbo_OrderNo.Visible = True Then
                cbo_OrderNo.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If
        If e.KeyCode = 38 Then
            txt_place_Supply.Focus()
        End If


    End Sub

    Private Sub txt_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VehicleNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_OrderNo.Visible = True Then
                cbo_OrderNo.Focus()
            Else
                txt_SlNo.Focus()
            End If
        End If
    End Sub

    Private Sub lbl_SGstAmount_DoubleClick(sender As Object, e As System.EventArgs) Handles lbl_SGstAmount.DoubleClick
        Try
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then '------AADHARSH INTERNATIONAL
                lbl_SGstAmount.Text = InputBox("Enter sgst Value", "FOR SGST VALUE", Val(lbl_SGstAmount.Text))
            End If
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub lbl_SGstAmount_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_SGstAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    'Private Sub lbl_Assessable_DoubleClick(sender As Object, e As System.EventArgs) Handles lbl_Assessable.DoubleClick
    '    Try
    '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then '------AADHARSH INTERNATIONAL
    '            lbl_Assessable.Text = InputBox("Enter Assessable Value", "FOR ASSESSABLE VALUE", Val(lbl_Assessable.Text))
    '        End If
    '    Catch ex As Exception
    '        '----
    '    End Try
    'End Sub

    Private Sub lbl_Assessable_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_Assessable.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_IGstAmount_DoubleClick(sender As Object, e As System.EventArgs) Handles lbl_IGstAmount.DoubleClick
        Try
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then '------AADHARSH INTERNATIONAL
                lbl_IGstAmount.Text = InputBox("Enter IGST Value", "FOR IGST VALUE", Val(lbl_IGstAmount.Text))
            End If
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub lbl_IGstAmount_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_IGstAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_CGstAmount_DoubleClick(sender As Object, e As System.EventArgs) Handles lbl_CGstAmount.DoubleClick
        Try
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then '------AADHARSH INTERNATIONAL
                lbl_CGstAmount.Text = InputBox("Enter CGST Value", "FOR CGST VALUE", Val(lbl_CGstAmount.Text))
            End If
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub lbl_CGstAmount_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_CGstAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_AddLess_BeforeTax_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_BeforeTax.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AddLess_BeforeTax_TextChanged(sender As Object, e As System.EventArgs) Handles txt_AddLess_BeforeTax.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub cbo_vehicle_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_vehicle.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "vehicle_head", "vehicle_No", "", "")
    End Sub

    Private Sub cbo_vehicle_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vehicle, txt_Due_Days, txt_SlNo, "vehicle_head", "vehicle_No", "", "")
    End Sub

    Private Sub cbo_vehicle_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vehicle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vehicle, txt_SlNo, "vehicle_head", "vehicle_No", "", "")
    End Sub

    Private Sub cbo_vehicle_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicle.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New VehicleNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_vehicle.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub txt_Due_Days_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_Due_Days.KeyDown
        If e.KeyCode = 38 Then
            txt_place_Supply.Focus()
        End If
        If e.KeyCode = 40 Then
            cbo_vehicle.Focus()
        End If
    End Sub

    Private Sub txt_Due_Days_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Due_Days.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1355" Then
                cbo_vehicle.Focus()
            Else
                txt_VehicleNo.Focus()
            End If
        End If
    End Sub

    'Private Sub Chk_Acc_Yes_CheckedChanged(sender As Object, e As System.EventArgs) Handles Chk_Acc_Yes.CheckedChanged
    '    If Chk_Acc_Yes.Checked = True Then
    '        Chk_Acc_No.Checked = False
    '    End If
    'End Sub

    'Private Sub Chk_Acc_No_CheckedChanged(sender As Object, e As System.EventArgs)
    '    If Chk_Acc_No.Checked = True Then
    '        Chk_Acc_Yes.Checked = False
    '    End If
    'End Sub

    Private Sub cbo_Currency_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Currency.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "currency_head", "currency_Name", "", "")
    End Sub

    Private Sub cbo_Currency_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Currency.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Currency, txt_place_Supply, txt_ExchangeRate, "currency_head", "currency_Name", "", "")
    End Sub

    Private Sub cbo_Currency_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Currency.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Currency, txt_ExchangeRate, "currency_head", "currency_Name", "", "")
    End Sub

    Private Sub cbo_Currency_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Currency.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Currency_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Currency.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub txt_ExchangeRate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ExchangeRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_ExchangeRate_TextChanged(sender As Object, e As System.EventArgs) Handles txt_ExchangeRate.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub txt_SubAmount_TextChanged(sender As Object, e As System.EventArgs) Handles txt_SubAmount.TextChanged
        Call Amount_Calculation(False)
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

    Private Sub lbl_billrate_close_Click(sender As Object, e As System.EventArgs) Handles lbl_billrate_close.Click
        pnl_Bill_Rate.Visible = False
    End Sub

    Private Sub Bill_RateDetails()

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim OrdByNo As Single = 0
        Dim Led_id As Integer = 0
        Dim Item_id As Integer = 0, itemGp_id As Integer = 0
        Dim n As Integer, Sno As Integer

        Dim da2 As SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim da3 As SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim vCmpSurNm As String = ""


        OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_PurchaseNo.Text))
        Led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        Item_id = Common_Procedures.Item_NameToIdNo1(con, cbo_ItemName.Text)
        '      itemGp_id = Common_Procedures.ItemGroup_NameToIdNo(con, cbo_itemGroup.Text)


        da2 = New SqlClient.SqlDataAdapter("Select a.item_idno , b.ItemGroup_IdNo from Purchase_Details a inner join Item_Head b on a.Item_IdNo = b.item_idno  where a.Ledger_Idno =" & Str(Val(Led_id)) & " and a.item_idno = " & Str(Val(Item_id)) & " ", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        If dt2.Rows.Count > 0 Then
            itemGp_id = dt2.Rows(n).Item("ItemGroup_IdNo").ToString
        End If


        vCmpSurNm = ""
        da3 = New SqlClient.SqlDataAdapter("Select ch.company_Name from Company_Head ch where ch.company_idno = " & Str(Val(lbl_Company.Tag)), con)
        dt3 = New DataTable
        da3.Fill(dt3)
        If dt3.Rows.Count > 0 Then
            vCmpSurNm = Common_Procedures.Remove_NonCharacters(dt3.Rows(0).Item("Company_Name").ToString)
        End If
        dt3.Clear()

        Try

            Dim vbillrate As String = ""

            vbillrate = ""

            If Trim(Common_Procedures.settings.CustomerCode) = "1365" And Trim(vCmpSurNm) = "NACHIYARACCESSORIEESINC" Then
                vbillrate = " a.Item_idno = " & Str(Val(Item_id))
            Else
                vbillrate = " b.ItemGroup_idNo = " & Str(Val(itemGp_id))
            End If


            da = New SqlClient.SqlDataAdapter("Select top 3 a.Purchase_no, a.Purchase_Date,a.Rate from Purchase_Details a inner join Item_Head b on a.Item_IdNo = b.item_idno  where a.for_orderby < " & Str(Val(OrdByNo)) & " and a.Ledger_Idno =" & Str(Val(Led_id)) & " and " & vbillrate & " Order by a.for_Orderby desc, a.Purchase_No desc ", con)
            da.Fill(dt)

            With dgv_BillRate
                .Rows.Clear()

                Sno = 0

                If dt.Rows.Count > 0 Then

                    For i = 0 To dt.Rows.Count - 1
                        n = .Rows.Add()

                        Sno = Sno + 1
                        .Rows(n).Cells(0).Value = Val(Sno)
                        .Rows(n).Cells(1).Value = dt.Rows(n).Item("Purchase_no").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt.Rows(i).Item("Purchase_Date").ToString), "dd-MM-yyyy") 'dt.Rows(n).Item("Sales_Date").ToString
                        .Rows(n).Cells(3).Value = dt.Rows(n).Item("Rate").ToString

                    Next i

                End If
            End With

            dt.Clear()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Description_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Description.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")


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

            txt_TCS_TaxableValue.Text = lbl_Invoice_Value_Before_TCS.Text

            txt_TcsPerc.Text = "0.1"

            txt_TCS_TaxableValue.Focus()

            txt_TCS_TaxableValue.Focus()

        Else
            txt_AddLess_AfterTax.Focus()

        End If
    End Sub

    Private Sub chk_TCS_Tax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCS_Tax.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub chk_TCSAmount_RoundOff_STS_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCSAmount_RoundOff_STS.CheckedChanged
        NetAmount_Calculation()
    End Sub



    Private Sub txt_TcsPerc_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TcsPerc.TextChanged
        NetAmount_Calculation()
    End Sub


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


        Try


            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            '-----------TOTAL SALES

            cmd.Connection = Con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@entrydate", dtp_Date.Value.Date)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            If Led_ID <> 0 Then

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount > 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "'  and (a.Voucher_Code LIKE 'GPURS-%' OR a.Voucher_Code LIKE 'SPPUR-%' OR a.Voucher_Code LIKE 'PURGM-%'  OR a.Voucher_Code LIKE 'DELSA-%'  OR a.Voucher_Code LIKE 'GGMPR-%' OR a.Voucher_Code LIKE 'PUGM1-%'  OR a.Voucher_Code LIKE 'CBREC-%' OR a.Voucher_Code LIKE 'PUREC-%' OR a.Voucher_Code LIKE 'CSPYM-%') "
                'cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'CSPYM-%') "
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

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount > 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(vPrevYrCode) & "' and (a.Voucher_Code LIKE 'GPURS-%' OR a.Voucher_Code LIKE 'SPPUR-%' OR a.Voucher_Code LIKE 'PURGM-%'  OR a.Voucher_Code LIKE 'DELSA-%'  OR a.Voucher_Code LIKE 'GGMPR-%' OR a.Voucher_Code LIKE 'PUGM1-%'  OR a.Voucher_Code LIKE 'CBREC-%' OR a.Voucher_Code LIKE 'PUREC-%' OR a.Voucher_Code LIKE 'CSPYM-%') "
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


        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE GETTIG TOTAL SALES....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_TDS_TaxableValue_TextChanged(sender As Object, e As EventArgs) Handles txt_TDS_TaxableValue.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TdsPerc_TextChanged(sender As Object, e As EventArgs) Handles txt_TdsPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub chk_TDS_Tax_CheckedChanged(sender As Object, e As EventArgs) Handles chk_TDS_Tax.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub btn_EDIT_TDS_TaxableValue_Click(sender As Object, e As EventArgs) Handles btn_EDIT_TDS_TaxableValue.Click
        txt_TDS_TaxableValue.Enabled = Not txt_TDS_TaxableValue.Enabled

        txt_TdsPerc.Enabled = Not txt_TdsPerc.Enabled

        If txt_TDS_TaxableValue.Enabled Then

            If Common_Procedures.settings.CustomerCode = "1087" Then
                txt_TDS_TaxableValue.Text = txt_GrossAmount.Text
            Else
                txt_TDS_TaxableValue.Text = lbl_Assessable.Text
            End If

            txt_TdsPerc.Text = "0.1"

            txt_TDS_TaxableValue.Focus()

        Else
            chk_TDS_Tax.Focus()

        End If

    End Sub


End Class