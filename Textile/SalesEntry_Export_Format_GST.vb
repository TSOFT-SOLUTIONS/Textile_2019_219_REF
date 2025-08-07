Imports System.Drawing.Printing
Imports System.IO
Imports System.Messaging

Public Class FinishedProduct_Export_Invoice
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPDIV-"
    Private Pk_Condition2 As String = "SLFGT-"
    Private Pk_condition3 As String = "CASRE-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_HdDt_New As New DataTable
    Private prn_DetDt_New As New DataTable
    Private prn_DetSNo As Integer

    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetIndx1 As Integer
    Private prn_DetMxIndx As Integer
    Private prn_HsnIndx As Integer
    Private prn_HdAr(1000, 20) As String
    Private prn_DetAr(200, 20) As String
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private prn_DupHsnCode As String = ""
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Private prn_OriDupTri As String = ""
    Public CHk_Details_Cnt As Integer = 0
    Private prn_TotalMtrs As String = ""
    Public vmskLrText As String = ""
    Public vmskLrStrt As Integer = -1
    Private vmskOldText As String = ""
    Private vmskSelStrt As Integer = -1
    Private vEntryType As String = ""
    Private vCmpTyp As String = ""
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer

    Dim Vchk_shirt_bill As Integer = 0

    Private Format_2_Status As Integer = 0

    Public RptSubReport_Index As Integer = 0
    Public RptSubReport_CompanyShortName As String = ""
    Public RptSubReport_VouNo As String = ""
    Public RptSubReport_VouCode As String = ""
    Private NoCalc_Status As Boolean = False
    Private Mov_Status As Boolean = False
    Private WithEvents dgtxt_ItemSet_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private vItem_Set_Details_STS As Boolean = False
    Private vDgv_Double_Click_STS As Boolean = False
    Private VCheck_ArticleNo As Boolean = False

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

        'vEntryType = Trim(UCase(EntryType))
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Enum DgvCol_Details As Integer

        SL_NO
        ITEM_NAME
        UNIT
        SIZE
        COLOUR
        ORDER_NO
        HSN_CODE
        GST_PERC
        NO_OF_PACKS
        NO_OF_PCS_PER_PACKS
        QUANTITY
        NET_WEIGHT
        GROSS_WEIGHT
        RATE_QTY
        AMOUNT
        DESCRIBTION
        PACKAGE_NO
        FOOTER_CSH_DISC_PER
        FOOTER_CSH_DISC_AMT
        ASSESSABLE_VALUE
        LOT_NO
        GST_RATE
        ITEM_SET_IDNO
        PROFORMA_INV_CODE
        SALES_DELV_CODE
        SALES_DELV_SLNO


    End Enum
    Private Enum DgvCol_ItemSet_Details As Integer
        SLNO = 0
        ITEM_NAME = 1
        UNIT = 2
        SIZE = 3
        COLOUR = 4
        HSN_CODE = 5
        GST_PERCEN = 6
        QTY = 7
        NO_OF_PACKS = 8
        RATE = 9
        GST_RATE = 10
        AMOUNT = 11
        SET_ITEM_IDNO = 12


    End Enum
    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As GroupBox

        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False
        NoCalc_Status = True
        Mov_Status = False
        Grp_EWB.Visible = False
        pnl_Selection.Visible = False
        pnl_EXport_Inv_Port_Details.Visible = False
        pnl_Terms.Visible = False

        lbl_AvailableStock.Tag = 0
        lbl_AvailableStock.Text = ""

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        txt_InvoicePrefixNo.Text = ""
        cbo_InvoiceSufixNo.Text = "/" & Common_Procedures.FnYearCode

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        txt_LrNo.Text = ""
        msk_Lr_Date.Text = ""



        '***** GST START *****
        pnl_GSTTax_Details.Visible = False
        '***** GST END *****

        chk_LabourBill.Checked = False
        Chk_shirt_bill.Checked = False

        chk_GSTTax_Invocie.Checked = True
        cbo_OrderNo.Text = ""
        vItem_Set_Details_STS = False
        vDgv_Double_Click_STS = False

        pnl_Courier_Details.Visible = False


        '***** GST START *****
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_SerialNo.Text = ""

        txt_Package_No.Text = ""
        txt_Net_wgt.Text = ""
        txt_Gross_Wgt.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        txt_GSTRate.Text = "0.00"
        cbo_DispatcherName.Text = ""
        txt_ExchangeRate.Text = ""
        cbo_Currency.Text = ""
        cbo_Ledger.Text = ""
        cbo_Ledger.Tag = cbo_Ledger.Text

        pnl_Bill_Rate.Visible = False

        txt_cashDisc_amt.Text = ""
        cbo_TransportMode.Text = "By Road"
        '***** GST END *****


        grp_EInvoice.Visible = False
        pic_IRN_QRCode_Image.BackgroundImage = Nothing
        txt_IR_No.Text = ""

        txt_eInvoiceNo.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckDate.Text = ""
        txt_eInvoice_CancelStatus.Text = ""
        txt_EInvoiceCancellationReson.Text = ""

        txt_eWayBill_No.Text = ""
        txt_EWB_Date.Text = ""
        txt_EWB_ValidUpto.Text = ""
        txt_EWB_Cancel_Status.Text = ""
        txt_EWB_Canellation_Reason.Text = ""
        rtbeInvoiceResponse.Text = ""

        txt_Ledger_Details.Text = ""
        Pnl_ledger_Deatils.Visible = False
        pnl_ItemSet_Details.Visible = False

        lbl_Assessable.Text = ""
        lbl_CGstAmount.Text = ""
        lbl_SGstAmount.Text = ""
        lbl_IGstAmount.Text = ""
        lbl_NetAmount.Text = ""
        lbl_RoundOff.Text = ""

        txt_EWBNo.Text = ""
        txt_cash_Party_name.Text = ""

        txt_CourierName_Cap.Text = "Courier Name"
        txt_CourierName.Text = ""

        txt_CourierNo_Cap.Text = "Courier No"
        txt_Courier_No.Text = ""

        txt_CourierDate_Cap.Text = "Courier Date"
        txt_courier_date.Text = ""

        txt_Courier_Noof_Caption_Cap.Text = "No Of Box"
        txt_Courier_Noof_Box.Text = ""

        txt_PoNo.Text = ""

        txt_Inco_Term.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
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

        lbl_GrossAmount.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_GSTTax_Details.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Add()

        dgv_ItemSet_Details.Rows.Clear()
        dgv_ItemSet_Details.Rows.Add()


        cbo_PaymentMethod.Text = "CREDIT"
        'cbo_EntType.Text = "DIRECT"

        'cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, 22)
        'cbo_TaxAc.Text = Common_Procedures.Ledger_IdNoToName(con, 20)
        txt_SlNo.Text = "1"

        txt_Tcs_Name.Text = "TCS"
        txt_TcsPerc.Text = ""
        lbl_TcsAmount.Text = ""

        cbo_Pre_Carriage_by.Text = ""
        txt_Place_Of_Receipt_By_Pre_carrier.Text = ""
        cbo_Vessal_Flight_No.Text = ""
        txt_Port_Of_Loading.Text = ""
        txt_Port_Of_Discharge.Text = ""
        txt_Final_destination.Text = ""
        txt_Other_Reference.Text = ""

        txt_Exports_Ref.Text = ""
        txt_Terms_Delivery_Payment_1.Text = ""
        txt_Terms_Delivery_Payment_2.Text = ""
        txt_Terms_Delivery_Payment_3.Text = ""



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

        lbl_CurrentBalance.Tag = -100
        lbl_CurrentBalance.Text = "Current Balance :"
        pnl_CurrentBalance.Visible = True

        NoCalc_Status = False
        Mov_Status = False

        Check_Combo_Cash_Party_Name()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim dtp As DateTimePicker
        Dim msk As MaskedTextBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

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

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_Cell_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_ItemSet_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            pnl_ItemSet_Details.Visible = False
            pnl_ItemSet_Details.Visible = False
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is DateTimePicker Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim Chk_Lab As Integer = 0


        If Val(no) = 0 Then Exit Sub

        clear()


        Mov_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)


        Try


            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName, c.Ledger_Name as SalesAcName, d.Ledger_Name as TaxAcName from FinishedProduct_Invoice_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.SalesAc_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.TaxAc_IdNo = d.Ledger_IdNo where a.FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' AND FinishedProduct_Invoice_Code LIKE '" & Trim(Pk_Condition) & "%' ", con)
            da1 = New SqlClient.SqlDataAdapter("select a.* from FinishedProduct_Invoice_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)


            If dt1.Rows.Count > 0 Then

                'lbl_InvoiceNo.Text = dt1.Rows(0).Item("Sales_No").ToString
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("FinishedProduct_Invoice_RefNo").ToString
                dtp_Date.Text = dt1.Rows(0).Item("FinishedProduct_Invoice_Date")
                msk_Date.Text = dtp_Date.Text
                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("FinishedProduct_Invoice_PrefixNo").ToString
                cbo_InvoiceSufixNo.Text = dt1.Rows(0).Item("FinishedProduct_Invoice_SuffixNo").ToString

                cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))

                cbo_PaymentMethod.Text = dt1.Rows(0).Item("Payment_Method").ToString

                'If IsDBNull(dt1.Rows(0).Item("LedgerName").ToString) = False Then

                '    If Trim(dt1.Rows(0).Item("LedgerName").ToString) <> "" Then
                '        If Val(dt1.Rows(0).Item("Ledger_IdNo").ToString) <> 1 Then
                '            cbo_Ledger.Text = dt1.Rows(0).Item("LedgerName").ToString
                '        Else
                '            'cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString+
                '            txt_cash_Party_name.Text = dt1.Rows(0).Item("Cash_PartyName").ToString
                '        End If

                '    Else
                '        '  cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString
                '        txt_cash_Party_name.Text = dt1.Rows(0).Item("Cash_PartyName").ToString

                '    End If

                'Else

                ' cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString
                txt_cash_Party_name.Text = dt1.Rows(0).Item("Cash_PartyName").ToString
                'End If
                cbo_Ledger.Tag = cbo_Ledger.Text

                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                cbo_OrderNo.Text = Trim(dt1.Rows(0).Item("Sales_Order_Selection_Code").ToString)
                txt_Due_Days.Text = dt1.Rows(0).Item("Due_Days").ToString
                txt_OrderDate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_DcNo.Text = dt1.Rows(0).Item("Dc_No").ToString
                txt_DcDate.Text = dt1.Rows(0).Item("Dc_Date").ToString
                'cbo_SalesAc.Text = dt1.Rows(0).Item("SalesAcName").ToString
                'cbo_TaxAc.Text = dt1.Rows(0).Item("TaxAcName").ToString

                '***** GST START *****

                txt_Electronic_RefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                cbo_TransportMode.Text = dt1.Rows(0).Item("Transportation_Mode").ToString
                txt_DateTime_Of_Supply.Text = dt1.Rows(0).Item("Date_Time_Of_Supply").ToString
                txt_Place_Of_Supply.Text = dt1.Rows(0).Item("Place_Of_Supply").ToString
                'cbo_TaxType.Text = dt1.Rows(0).Item("Entry_GST_Tax_Type").ToString
                '***** GST END *****

                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                txt_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_PaymentTerms.Text = dt1.Rows(0).Item("Payment_Terms").ToString

                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                msk_Lr_Date.Text = dt1.Rows(0).Item("Lr_Date").ToString

                Chk_Lab = dt1.Rows(0).Item("Labour_Charge").ToString
                If Chk_Lab = 1 Then chk_LabourBill.Checked = True

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                lbl_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")

                txt_cashDisc_amt.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")

                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                txt_TaxPerc.Text = Format(Val(dt1.Rows(0).Item("Tax_Perc").ToString), "########0.00")
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "########0.00")

                '***** GST START *****
                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False
                lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGst_Amount").ToString), "########0.00")
                lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGst_Amount").ToString), "########0.00")
                lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGst_Amount").ToString), "########0.00")
                '***** GST END ********
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))

                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")

                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Trans_Freight.Text = Format(Val(dt1.Rows(0).Item("Trans_Freight_Amt").ToString), "########0.00")

                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                If Val(dt1.Rows(0)("Shirt_Bill_Status").ToString) <> 0 Then
                    Chk_shirt_bill.Checked = True
                End If
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

                cbo_Currency.Text = Common_Procedures.Currency_IdNoToName(con, Val(dt1.Rows(0).Item("Currency_idNo").ToString))
                txt_ExchangeRate.Text = Format(Val(dt1.Rows(0).Item("Exchange_Rate").ToString), "########0.00")


                txt_IR_No.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                txt_eInvoiceNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_Date")) Then txt_eInvoiceAckDate.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_Date").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancelled_Status")) Then txt_eInvoice_CancelStatus.Text = IIf(dt1.Rows(0).Item("E_Invoice_Cancelled_Status") = True, "Cancelled", "Active")

                txt_Inco_Term.Text = dt1.Rows(0).Item("Inco_Term").ToString

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

                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EInvoiceCancellationReson.Text = Trim(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)

                If Not IsDBNull(dt1.Rows(0).Item("EWB_No")) Then txt_eWayBill_No.Text = Trim(dt1.Rows(0).Item("EWB_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_No")) Then txt_EWBNo.Text = Trim(dt1.Rows(0).Item("EWB_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Date")) Then txt_EWB_Date.Text = Trim(dt1.Rows(0).Item("EWB_Date").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Valid_Upto")) Then txt_EWB_ValidUpto.Text = Trim(dt1.Rows(0).Item("EWB_Valid_Upto").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Cancelled")) Then
                    If dt1.Rows(0).Item("EWB_Cancelled") = True Then
                        txt_EWB_Cancel_Status.Text = "Cancelled"
                    Else
                        txt_EWB_Cancel_Status.Text = "Active"
                    End If
                End If


                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EWB_Canellation_Reason.Text = Trim(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)

                If Not IsDBNull(dt1.Rows(0).Item("Dispatcher_IdNo")) Then
                    cbo_DispatcherName.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("Dispatcher_IdNo"))
                End If

                lbl_Invoice_Value_Before_TCS.Text = dt1.Rows(0).Item("Invoice_Value_Before_TCS").ToString

                lbl_RoundOff_Invoice_Value_Before_TCS.Text = dt1.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString



                txt_CourierName_Cap.Text = dt1.Rows(0).Item("Courier_Name_Caption").ToString
                txt_CourierName.Text = dt1.Rows(0).Item("Courier_Name").ToString

                txt_CourierNo_Cap.Text = dt1.Rows(0).Item("Courier_No_Caption").ToString
                txt_Courier_No.Text = dt1.Rows(0).Item("Courier_No").ToString

                txt_CourierDate_Cap.Text = dt1.Rows(0).Item("Courier_Date_Caption").ToString
                txt_courier_date.Text = dt1.Rows(0).Item("Courier_Date").ToString

                txt_Courier_Noof_Caption_Cap.Text = dt1.Rows(0).Item("Courier_Noof_Box_Caption").ToString
                txt_Courier_Noof_Box.Text = Val(dt1.Rows(0).Item("Courier_Noof_Box").ToString)

                txt_PoNo.Text = dt1.Rows(0).Item("Po_No").ToString
                'cbo_EntType.Text = dt1.Rows(0).Item("Entry_Type").ToString


                cbo_Pre_Carriage_by.Text = dt1.Rows(0).Item("pre_Carriage_by").ToString
                txt_Place_Of_Receipt_By_Pre_carrier.Text = dt1.Rows(0).Item("Place_of_receipt_by_Pre_Carrier").ToString
                cbo_Vessal_Flight_No.Text = dt1.Rows(0).Item("Vessal_Flight_No").ToString
                txt_Port_Of_Loading.Text = dt1.Rows(0).Item("Port_Of_Loading").ToString
                txt_Port_Of_Discharge.Text = dt1.Rows(0).Item("Port_Of_Discharge").ToString
                txt_Final_destination.Text = dt1.Rows(0).Item("Final_Destination").ToString
                txt_Other_Reference.Text = dt1.Rows(0).Item("Other_Reference").ToString

                txt_Exports_Ref.Text = dt1.Rows(0).Item("Exporters_Reference").ToString
                txt_Terms_Delivery_Payment_1.Text = dt1.Rows(0).Item("Terms_Payment_Delivery_Detail_1").ToString
                txt_Terms_Delivery_Payment_2.Text = dt1.Rows(0).Item("Terms_Payment_Delivery_Detail_2").ToString
                txt_Terms_Delivery_Payment_3.Text = dt1.Rows(0).Item("Terms_Payment_Delivery_Detail_3").ToString



                'da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Item_Name, c.Unit_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on a.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)


                da2 = New SqlClient.SqlDataAdapter("select a.* from FinishedProduct_Invoice_Details a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()

                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_Details.Rows.Add()
                            SNo = SNo + 1
                            dgv_Details.Rows(n).Cells(DgvCol_Details.SL_NO).Value = Val(SNo)
                            dgv_Details.Rows(n).Cells(DgvCol_Details.ITEM_NAME).Value = Common_Procedures.Processed_Item_IdNoToName(con, Val(dt2.Rows(i).Item("Item_IdNo").ToString))
                            dgv_Details.Rows(n).Cells(DgvCol_Details.UNIT).Value = Common_Procedures.Unit_IdNoToName(con, Val(dt2.Rows(i).Item("Unit_IdNo").ToString))
                            dgv_Details.Rows(n).Cells(DgvCol_Details.SIZE).Value = Common_Procedures.Size_IdNoToName(con, Val(dt2.Rows(i).Item("Size_Idno").ToString))
                            dgv_Details.Rows(n).Cells(DgvCol_Details.COLOUR).Value = Common_Procedures.Colour_IdNoToName(con, Val(dt2.Rows(i).Item("Colour_Idno").ToString))
                            dgv_Details.Rows(n).Cells(DgvCol_Details.ORDER_NO).Value = Trim(dt2.Rows(i).Item("ORDER_NO").ToString)
                            dgv_Details.Rows(n).Cells(DgvCol_Details.HSN_CODE).Value = dt2.Rows(i).Item("HSN_Code").ToString
                            dgv_Details.Rows(n).Cells(DgvCol_Details.GST_PERC).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(DgvCol_Details.NO_OF_PACKS).Value = Trim(dt2.Rows(i).Item("NO_OF_PACKS").ToString)
                            dgv_Details.Rows(n).Cells(DgvCol_Details.NO_OF_PCS_PER_PACKS).Value = Trim(dt2.Rows(i).Item("NO_OF_PCS_PER_PACKS").ToString)
                            dgv_Details.Rows(n).Cells(DgvCol_Details.QUANTITY).Value = Trim(dt2.Rows(i).Item("No_oF_items").ToString)
                            dgv_Details.Rows(n).Cells(DgvCol_Details.GROSS_WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Gross_Weight").ToString), "#########0.000")
                            dgv_Details.Rows(n).Cells(DgvCol_Details.NET_WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "#########0.000")
                            dgv_Details.Rows(n).Cells(DgvCol_Details.RATE_QTY).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(DgvCol_Details.DESCRIBTION).Value = dt2.Rows(i).Item("Serial_No").ToString
                            dgv_Details.Rows(n).Cells(DgvCol_Details.PACKAGE_NO).Value = dt2.Rows(i).Item("Package_No").ToString
                            dgv_Details.Rows(n).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value = dt2.Rows(i).Item("Assessable_Value").ToString




                            'dgv_Details.Rows(n).Cells(DgvCol_Details.QUANTITY).Value = Val(dt2.Rows(i).Item("No_of_Items").ToString)
                            ''If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then  '---  AADHARSH
                            ''    dgv_Details.Rows(n).Cells(DgvCol_Details.RATE_QTY).Value = Val(dt2.Rows(i).Item("Rate").ToString)
                            ''Else
                            '
                            ''End If



                            ''***** GST START *****
                            'dgv_Details.Rows(n).Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Perc_For_All_Item").ToString), "########0.00")
                            'dgv_Details.Rows(n).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Amount_For_All_Item").ToString), "########0.00")
                            'dgv_Details.Rows(n).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value = Format(Val(dt2.Rows(i).Item("Assessable_Value").ToString), "########0.00")

                            'dgv_Details.Rows(n).Cells(DgvCol_Details.LOT_NO).Value = Common_Procedures.LotNo_IdNoToName(con, Val(dt2.Rows(i).Item("LotNo_Idno").ToString))

                            'dgv_Details.Rows(n).Cells(DgvCol_Details.GST_RATE).Value = Format(Val(dt2.Rows(i).Item("RateWithTax").ToString), "########0.00")

                            'dgv_Details.Rows(n).Cells(DgvCol_Details.ITEM_SET_IDNO).Value = Val(dt2.Rows(i).Item("Item_Set_IdNo").ToString)

                            'dgv_Details.Rows(n).Cells(DgvCol_Details.PROFORMA_INV_CODE).Value = Trim(dt2.Rows(i).Item("Sales_proforma_Code").ToString)

                            'dgv_Details.Rows(n).Cells(DgvCol_Details.SALES_DELV_CODE).Value = Trim(dt2.Rows(i).Item("Sales_Delivery_Code").ToString)
                            'dgv_Details.Rows(n).Cells(DgvCol_Details.SALES_DELV_SLNO).Value = Trim(dt2.Rows(i).Item("Sales_Delivery_Detail_SlNo").ToString)


                            '***** GST END *****

                        Next i

                    End If

                    For i = 0 To .Rows.Count - 1
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SL_NO).Value = i + 1
                    Next

                End With
                dt2.Clear()
                Mov_Status = False

                TotalAmount_Calculation()
                Mov_Status = True

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)


                '***** GST START *****
                da1 = New SqlClient.SqlDataAdapter("Select a.* from Sales_GST_Tax_Details a Where a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
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
                '***** GST END *****
                NoCalc_Status = False
                Mov_Status = False

                get_Ledger_TotalSales()
                get_Ledger_CurrentBalance()

                NoCalc_Status = True
                Mov_Status = True

            Else
                Me.new_record()


            End If

            dt1.Clear()

            Grid_Cell_DeSelect()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            dt2.Dispose()

            da1.Dispose()
            da2.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

        NoCalc_Status = False
        Mov_Status = False


    End Sub

    Private Sub SalesEntry_Simple2_GST_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim OrdByNo_Code As String = ""
        Dim VouCode As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Currency.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CURRENCY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Currency.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Lot_No.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOTNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Lot_No.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DispatcherName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DispatcherName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
                        Da1 = New SqlClient.SqlDataAdapter("Select a.For_OrderBy from Sales_Head a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(RptSubReport_VouCode) & "'", con)
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

                    Call new_record()

                End If


            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub SalesEntry_Simple2_GST_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter

        'Dim vCmpTyp As String = ""

        Me.Text = ""

        con.Open()





        'Other_Condition = ""

        'Other_Condition = "(Sales_Code LIKE '" & Trim(Pk_Condition) & "%' and Other_GST_Entry_Type = '" & Trim(UCase(vEntryType)) & "')"

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
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

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 28 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_SalesAc.DataSource = dt4
        cbo_SalesAc.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select LotNo_name from LotNo_head order by LotNo_name", con)
        'da.Fill(dt5)
        'Cbo_Lot_No.DataSource = dt5
        'Cbo_Lot_No.DisplayMember = "LotNo_name"

        cbo_PaymentMethod.Items.Clear()
        cbo_PaymentMethod.Items.Add("")
        cbo_PaymentMethod.Items.Add("CASH")
        cbo_PaymentMethod.Items.Add("CREDIT")

        'cbo_TaxType.Items.Clear()
        'cbo_TaxType.Items.Add("NO TAX")
        'cbo_TaxType.Items.Add("GST")


        cbo_InvoiceSufixNo.Items.Clear()
        cbo_InvoiceSufixNo.Items.Add("")
        cbo_InvoiceSufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_InvoiceSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))

        'cbo_EntType.Items.Clear()
        'cbo_EntType.Items.Add("")
        'cbo_EntType.Items.Add("DIRECT")
        'cbo_EntType.Items.Add("DELIVERY")
        'cbo_EntType.Items.Add("ORDER")
        'cbo_EntType.Items.Add("PROFORMA")




        'If Trim(UCase(vEntryType)) = "EXPORT" Then
        '    Pk_Condition = "GSAEX-"
        '    lbl_Header_Caption.Text = "EXPORT INVOICE - GST"


        'ElseIf Trim(UCase(vEntryType)) = "NOTAX" Then

        '    Pk_Condition = "SINNT-"
        '    lbl_Header_Caption.Text = "FINISHED PRODUCT INVOICE - WITHOUT TAX"
        '    cbo_TaxType.Text = "NO TAX"

        'ElseIf Trim(UCase(vEntryType)) = "" Then

        '    Pk_Condition = "FPDIV-"
        '    lbl_Header_Caption.Text = "FINISHED PRODUCT INVOICE"
        '    cbo_TaxType.Text = "GST"

        'End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1403" Then           '----------NEW WAY GARMENTS
        '    If Trim(UCase(vEntryType)) = "NOTAX" Then

        '        lbl_Header_Caption.Text = "SALES ENTRY - WITHOUT TAX"
        '        cbo_TaxType.Text = "NO TAX"

        '        Pk_Condition = "SINNT-"

        '    Else

        '        lbl_Header_Caption.Text = "SALES ENTRY - GST"
        '        cbo_TaxType.Text = "GST"

        '    End If
        'End If

        '************************************************

        '======= PUT  CONTROLS  VISIBILITY HERE ========== START 


        chk_LabourBill.Visible = False
        Pnl_ledger_Deatils.Visible = False
        pnl_ItemSet_Details.Visible = False

        'lbl_Dispatcher.Visible = False
        'cbo_DispatcherName.Visible = False

        txt_IR_No.BackColor = Color.White

        btn_Courier_details.Visible = False
        lbl_PoNo.Visible = False
        txt_PoNo.Visible = False

        'Lbl_Type.Visible = False
        'cbo_EntType.Visible = False

        lbl_caption_proformano.Visible = False
        txt_proformaNo.Visible = False


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Bill_Rate.Visible = False
        pnl_Bill_Rate.Left = pnl_Back.Left
        pnl_Bill_Rate.Top = pnl_Back.Top + pnl_Back.Height - pnl_Bill_Rate.Height
        pnl_Bill_Rate.BringToFront()

        '***** GST START *****
        pnl_GSTTax_Details.Visible = False
        pnl_GSTTax_Details.Left = (Me.Width - pnl_GSTTax_Details.Width) \ 2
        pnl_GSTTax_Details.Top = ((Me.Height - pnl_GSTTax_Details.Height) \ 2) - 100
        pnl_GSTTax_Details.BringToFront()
        '***** GST END *****       

        '***** ITEM SET DETAILS START *****
        pnl_ItemSet_Details.Visible = False
        pnl_ItemSet_Details.Left = (Me.Width - pnl_ItemSet_Details.Width) \ 2 - 10
        pnl_ItemSet_Details.Top = ((Me.Height - pnl_ItemSet_Details.Height) \ 2) + 150
        pnl_ItemSet_Details.BringToFront()
        '***** ITEM SET DETAILS END *****

        '***** COURIER DETAILS START *****
        pnl_Courier_Details.Visible = False
        pnl_Courier_Details.Left = (Me.Width - pnl_Courier_Details.Width) \ 2
        pnl_Courier_Details.Top = (Me.Height - pnl_Courier_Details.Height) \ 2
        pnl_Courier_Details.BringToFront()
        '***** COURIER DETAILS END *****

        '***** EXPORT INVOICE PORT DETAILS START *****
        pnl_EXport_Inv_Port_Details.Visible = False
        pnl_EXport_Inv_Port_Details.Left = (Me.Width - pnl_EXport_Inv_Port_Details.Width) \ 2
        pnl_EXport_Inv_Port_Details.Top = (Me.Height - pnl_EXport_Inv_Port_Details.Height) \ 2
        pnl_EXport_Inv_Port_Details.BringToFront()
        '***** EXPORT INVOICE PORT DETAILS END *****

        '***** EXPORT INVOICE TERMS DELIVERY AND PAYMENT START *****
        pnl_Terms.Visible = False
        pnl_Terms.Left = (Me.Width - pnl_Terms.Width) \ 2
        pnl_Terms.Top = (Me.Height - pnl_Terms.Height) \ 2
        pnl_Terms.BringToFront()

        '***** EXPORT INVOICE TERMS DELIVERY AND PAYMENT  END *****


        pnl_proforma_selection.Visible = False
        pnl_proforma_selection.Left = (Me.Width - pnl_proforma_selection.Width) \ 2
        pnl_proforma_selection.Top = (Me.Height - pnl_proforma_selection.Height) \ 2
        pnl_proforma_selection.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()


        '======= PUT VISIBILITY CONTROLS  HERE ========== END 

        '======= PUT CC CONDITION'S  HERE ========== START 

        If Common_Procedures.settings.EInvoice_API_TotalCredits_Per_Year <> 0 Then

            lbl_Dispatcher.Visible = True
            cbo_DispatcherName.Visible = True

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Then
            lbl_dc_No.Visible = False
            txt_DcNo.Visible = False
            lbl_PoNo.Visible = False
            txt_PoNo.Visible = False
            lbl_dc_date.Visible = False
            txt_DcDate.Visible = False
            'btn_Selection.Visible = True
            'btn_ExportInv_Port_details.Left = cbo_TaxType.Left
            'btn_ExportInv_Port_details.Width = cbo_TaxType.Width
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1039" Then '---- Senthil Kumar Industries (Coimbatore)
            chk_LabourBill.Visible = True
        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If (Common_Procedures.settings.CustomerCode = "1186") Then
            lbl_Due_Days_Caption.Visible = True
            txt_Due_Days.Visible = True
        End If
        If (Common_Procedures.settings.CustomerCode = "1244") Then
            Chk_shirt_bill.Visible = True

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1366" Then  '---sowmiya traders
            lbl_VehicleNo_Caption.Visible = False
            txt_VehicleNo.Visible = False
            lbl_PaymentTerms_Caption.Visible = False
            txt_PaymentTerms.Visible = False
            lbl_Due_Days_Caption.Visible = False
            txt_Due_Days.Visible = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1304-" Then
            btn_SaveAll.Visible = True
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2001" Then '---- Demo - Elpro Chem for Vasanth by Deva (Chennai)
            cbo_OrderNo.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then 'Aadharsh
            lbl_transport.Visible = True
            lbl_transport.Tag = ""

            cbo_Transport.Visible = True
            cbo_Transport.Tag = ""

            Lbl_Trans_Freight.Visible = True
            Lbl_Trans_Freight.Tag = ""

            txt_Trans_Freight.Visible = True
            txt_Trans_Freight.Tag = ""


            lbl_Caption_Currency.Visible = True
            lbl_Caption_Currency.Tag = ""

            cbo_Currency.Visible = True
            cbo_Currency.Tag = ""

            lbl_ExchangeRate.Visible = True
            lbl_ExchangeRate.Tag = ""

            txt_ExchangeRate.Visible = True
            txt_ExchangeRate.Tag = ""


            'lbl_Lot_No.Visible = True
            'Cbo_Lot_No.Visible = True
            'dgv_Details.Columns(DgvCol_Details.LOT_NO).Visible = True

            'Cbo_Lot_No.BackColor = Color.White
            If Cbo_Lot_No.Visible = True Then
                txt_SerialNo.Size = New Size(644, 23) '520, 23
            End If


            'dgv_Details.Columns(DgvCol_Details.UNIT).Width = 45     'UNIT
            '    dgv_Details.Columns(DgvCol_Details.DESCRIBTION).Width = 150    'SERIAL NO
            '    dgv_Details.Columns(DgvCol_Details.QUANTITY).Width = 68     'QUANTITY
            '    dgv_Details.Columns(DgvCol_Details.RATE).Width = 50     'RATE
            '    dgv_Details.Columns(DgvCol_Details.AMOUNT).Width = 90     'AMOUNT
            'dgv_Details.Columns(DgvCol_Details.GST_PERC).Width = 48    'GST



            dgv_Details_Total.Columns(DgvCol_Details.UNIT).Width = dgv_Details.Columns(DgvCol_Details.UNIT).Width
            dgv_Details_Total.Columns(DgvCol_Details.DESCRIBTION).Width = dgv_Details.Columns(DgvCol_Details.DESCRIBTION).Width
            dgv_Details_Total.Columns(DgvCol_Details.QUANTITY).Width = dgv_Details.Columns(DgvCol_Details.QUANTITY).Width
            dgv_Details_Total.Columns(DgvCol_Details.RATE_QTY).Width = dgv_Details.Columns(DgvCol_Details.RATE_QTY).Width
            dgv_Details_Total.Columns(DgvCol_Details.AMOUNT).Width = dgv_Details.Columns(DgvCol_Details.AMOUNT).Width
            dgv_Details_Total.Columns(DgvCol_Details.GST_PERC).Width = dgv_Details.Columns(DgvCol_Details.GST_PERC).Width


            If Trim(UCase(vEntryType)) = "EXPORT" Then
                btn_ExportInv_Port_details.Visible = True
                btn_ExportInv_Port_details.BackColor = Color.RoyalBlue
                Label38.Visible = False
                txt_DateTime_Of_Supply.Visible = False
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then  '--- GAJAKHARNAA TRADERRS

            'btn_Selection.Visible = True
            Pnl_ledger_Deatils.Visible = True

            cbo_Ledger.Width = cbo_Ledger.Width - 45
        Else
            'btn_Selection.Visible = False
            Pnl_ledger_Deatils.Visible = False
            'cbo_Ledger.Size = New Size(385, 23)
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1545" Then  ' ---  BAGAVAN TEX ( LEBELS AND TAG )

            'cbo_EntType.Items.Clear()
            'cbo_EntType.Items.Add("")
            'cbo_EntType.Items.Add("DIRECT")
            'cbo_EntType.Items.Add("PROFORMA")

            btn_Courier_details.Visible = True
            lbl_PoNo.Visible = True
            txt_PoNo.Visible = True
            txt_PoNo.BackColor = Color.White

            lbl_PoNo.Left = lbl_dc_No.Left
            txt_PoNo.Left = txt_DcNo.Left
            txt_PoNo.Width = txt_Place_Of_Supply.Width

            txt_OrderNo.Width = cbo_DeliveryTo.Width

            txt_OrderDate.Visible = False
            Label25.Visible = False

            lbl_dc_No.Visible = False
            txt_DcNo.Visible = False
            lbl_dc_date.Visible = False
            txt_DcDate.Visible = False

            'Lbl_Type.Visible = True
            'cbo_EntType.Visible = True

            lbl_caption_proformano.Visible = True
            txt_proformaNo.Visible = True

            'lbl_caption_proformano.Left = lbl_taxtype.Left
            txt_proformaNo.Left = cbo_TransportMode.Left
            txt_proformaNo.Width = cbo_TransportMode.Width

            txt_proformaNo.BackColor = Color.White

            'btn_Selection.Visible = True
            'cbo_Ledger.Width = cbo_Ledger.Width - (btn_Selection.Width)

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1556" Then  ' ---  HI TEC TRADINGS
            'cbo_EntType.Items.Clear()
            'cbo_EntType.Items.Add("")
            'cbo_EntType.Items.Add("DIRECT")
            'cbo_EntType.Items.Add("DELIVERY")

            'Lbl_Type.Visible = True
            'cbo_EntType.Visible = True

            'btn_Selection.Visible = True
            'cbo_Ledger.Width = cbo_Ledger.Width - (btn_Selection.Width)

        End If



        '======= PUT CC CONDITION'S  HERE ==========  END

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        '***** GST START *****
        AddHandler txt_Electronic_RefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateTime_Of_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Due_Days.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Place_Of_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        '***** GST END *****
        AddHandler txt_InvoicePrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofItems.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SerialNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PaymentTerms.GotFocus, AddressOf ControlGotFocus
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
        AddHandler chk_LabourBill.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Trans_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Gross_Wgt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Net_wgt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Package_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Lr_Date.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Inco_Term.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Tcs_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TcsPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TCS_TaxableValue.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Currency.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ExchangeRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_cashDisc_amt.Enter, AddressOf ControlGotFocus
        AddHandler cbo_InvoiceSufixNo.Enter, AddressOf ControlGotFocus
        AddHandler txt_IR_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GSTRate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DispatcherName.Enter, AddressOf ControlGotFocus
        AddHandler txt_cash_Party_name.Enter, AddressOf ControlGotFocus
        AddHandler txt_CourierName.Enter, AddressOf ControlGotFocus
        AddHandler txt_Courier_No.Enter, AddressOf ControlGotFocus
        AddHandler txt_courier_date.Enter, AddressOf ControlGotFocus
        AddHandler txt_Courier_Noof_Box.Enter, AddressOf ControlGotFocus
        AddHandler txt_PoNo.Enter, AddressOf ControlGotFocus
        'AddHandler cbo_EntType.Enter, AddressOf ControlGotFocus
        AddHandler txt_Order_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_No_of_Packs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_No_Of_Pcs_Per_Packs.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_InvoicePrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GSTRate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Trans_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tcs_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TcsPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TCS_TaxableValue.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_cashDisc_amt.Leave, AddressOf ControlLostFocus
        AddHandler txt_Order_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_No_of_Packs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_No_Of_Pcs_Per_Packs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Lr_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Inco_Term.LostFocus, AddressOf ControlLostFocus

        '***** GST START *****
        AddHandler txt_Due_Days.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Electronic_RefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateTime_Of_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Place_Of_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        '***** GST END *****

        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoofItems.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SerialNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PaymentTerms.LostFocus, AddressOf ControlLostFocus
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
        AddHandler chk_LabourBill.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Currency.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ExchangeRate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_InvoiceSufixNo.Leave, AddressOf ControlLostFocus
        AddHandler txt_IR_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DispatcherName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_cash_Party_name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Colour.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_CourierName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Courier_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_courier_date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Courier_Noof_Box.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PoNo.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_EntType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Net_wgt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Gross_Wgt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Package_No.LostFocus, AddressOf ControlLostFocus

        '***** GST START *****
        'AddHandler txt_Electronic_RefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Due_Days.KeyDown, AddressOf TextBoxControlKeyDown
        '***** GST END *****
        ' AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_DcDate.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler txt_NoofItems.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_VehicleNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_LabourBill.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Place_Of_Supply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Trans_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_cashDisc_amt.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Gross_Wgt.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Net_wgt.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Container_No.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        '***** GST START *****
        AddHandler txt_Due_Days.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Electronic_RefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateTime_Of_Supply.KeyPress, AddressOf TextBoxControlKeyPress
        '***** GST END *****
        'AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_OrderDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_DcDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_NoofItems.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_VehicleNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_LabourBill.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Place_Of_Supply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Trans_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TcsPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TCS_TaxableValue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tcs_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_cashDisc_amt.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Net_wgt.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Gross_Wgt.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Container_No.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_Tcs_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TcsPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TCS_TaxableValue.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler Cbo_Lot_No.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Lot_No.LostFocus, AddressOf ControlLostFocus



        'Dim combinedControls As New List(Of Control)

        'combinedControls.AddRange(Panel4.Controls.OfType(Of Control)())
        'combinedControls.AddRange(Panel3.Controls.OfType(Of Control)())


        'For Each ctrl As Control In combinedControls
        '    If TypeOf ctrl Is TextBox Or TypeOf ctrl Is ComboBox Then

        '        AddHandler ctrl.GotFocus, AddressOf ControlGotFocus
        '        AddHandler ctrl.LostFocus, AddressOf ControlLostFocus

        '        If TypeOf ctrl Is TextBox Then
        '            AddHandler ctrl.KeyPress, AddressOf TextBoxControlKeyPress
        '            AddHandler ctrl.KeyDown, AddressOf TextBoxControlKeyDown
        '        End If

        '    End If
        'Next

        For Each ctrl As Control In Panel3.Controls
            If TypeOf ctrl Is TextBox Or TypeOf ctrl Is ComboBox Then

                AddHandler ctrl.GotFocus, AddressOf ControlGotFocus
                AddHandler ctrl.LostFocus, AddressOf ControlLostFocus

                If TypeOf ctrl Is TextBox Then
                    AddHandler ctrl.KeyPress, AddressOf TextBoxControlKeyPress
                    AddHandler ctrl.KeyDown, AddressOf TextBoxControlKeyDown
                End If

            End If
        Next

        For Each ctrl As Control In Panel4.Controls
            If TypeOf ctrl Is TextBox Or TypeOf ctrl Is ComboBox Then

                AddHandler ctrl.GotFocus, AddressOf ControlGotFocus
                AddHandler ctrl.LostFocus, AddressOf ControlLostFocus

                If TypeOf ctrl Is TextBox Then
                    AddHandler ctrl.KeyPress, AddressOf TextBoxControlKeyPress
                    AddHandler ctrl.KeyDown, AddressOf TextBoxControlKeyDown
                End If

            End If
        Next


        '  --- DONT DISTURB THIS CODE

        If cbo_Currency.Visible = False And txt_ExchangeRate.Visible = False And cbo_DispatcherName.Visible = False Then

            lbl_IRn.Left = Label2.Left
            txt_IR_No.Left = cbo_DeliveryTo.Left
            txt_IR_No.Width = txt_SerialNo.Width



        ElseIf cbo_Currency.Visible = True And txt_ExchangeRate.Visible = True And cbo_DispatcherName.Visible = False Then

            lbl_IRn.Left = Label2.Left
            txt_IR_No.Left = cbo_DeliveryTo.Left
            txt_IR_No.Width = cbo_DeliveryTo.Width


            lbl_Caption_Currency.Left = Label40.Left
            cbo_Currency.Left = txt_Electronic_RefNo.Left
            cbo_Currency.Width = txt_Electronic_RefNo.Width

        ElseIf cbo_Currency.Visible = True And txt_ExchangeRate.Visible = True And cbo_DispatcherName.Visible = True Then

            txt_IR_No.Width = txt_DcNo.Width

        End If

        'If cbo_EntType.Visible = False And Lbl_Type.Visible = False Then

        '    lbl_ledger.Left = Lbl_Type.Left

        '    cbo_Ledger.Left = cbo_EntType.Left + 2
        '    If btn_Selection.Visible = False Then
        '        cbo_Ledger.Size = New Size(520, 23) '(493, 23)
        '    Else
        '        cbo_Ledger.Size = New Size(493, 23)
        '    End If

        'End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1545" Then
            txt_IR_No.Width = (txt_SerialNo.Width - lbl_ExchangeRate.Width) - 2

        End If

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()



    End Sub

    Private Sub SalesEntry_Simple2_GST_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        Open_Report()
        con.Close()
        con.Dispose()
    End Sub

    Private Sub SalesEntry_Simple2_GST_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_GSTTax_Details.Visible = True Then
                    btn_Close_GSTTax_Details_Click(sender, e)
                    Exit Sub
                ElseIf Pnl_ledger_Deatils.Visible = True Then
                    btn_Led_Details_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_ItemSet_Details.Visible = True Then
                    btn_Close_pnl_ItemSet_Details_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Courier_Details.Visible = True Then
                    btn_Close_Pnl_Courier_Details_Click(sender, e)
                    Exit Sub
                ElseIf pnl_proforma_selection.Visible = True Then
                    btn_Close_proforma_Selection_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Delivery_Selection_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Terms.Visible = True Then
                    btn_terms_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_EXport_Inv_Port_Details.Visible = True Then
                    btn_Close_ExportInv_Port_details_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Print.Visible = True Then
                    btn_Close_Print_Click(sender, e)
                    Exit Sub
                Else
                    Close_Form()

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim Newcode2 As String = ""
        Dim vOrdByNo As String = ""
        Dim tr As SqlClient.SqlTransaction

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Newcode2 = Trim((Pk_condition3)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Sales_Head", "Sales_Code", Val(lbl_Company.Tag), Trim(Pk_Condition) & NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Sales_Code, Company_IdNo, for_OrderBy", tr)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Sales_Details", "Sales_Code", Val(lbl_Company.Tag), Trim(Pk_Condition) & NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Item_IdNo,Unit_IdNo,Serial_No,Noof_Items,Rate,Amount,Total_Amount,Cash_Discount_Perc_For_All_Item,Cash_Discount_Amount_For_All_Item,Assessable_Value,HSN_Code,Tax_Perc", "Sl_No", "Sales_Code, For_OrderBy, Company_IdNo, Sales_No, Sales_Date, Ledger_Idno", tr)


            cmd.Connection = con
            cmd.Transaction = tr

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Newcode2), tr)



            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1150" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1376" Then  '---  SRIKA DESIGNS (TIRUPUR) 
                cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If

            'If Trim(UCase(cbo_EntType.Text)) = "PROFORMA" Then
            '    cmd.CommandText = "Update Sales_Proforma_Details set Sales_Proforma_code=''  from Sales_Proforma_Details a, Sales_Details b Where a.Sales_Proforma_code = '" & Trim(NewCode) & "' " 'and  a.Sales_Proforma_code = b.Sales_code"
            '    cmd.ExecuteNonQuery()
            'End If

            'cmd.CommandText = "Update Sales_Delivery_Details Set Receipt_Quantity = a.Receipt_Quantity - b.Noof_Items from Sales_dELIVERY_Details a, Sales_Details b where b.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and b.Entry_Type = 'DELIVERY' and a.Sales_Delivery_Code = b.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = b.Sales_Delivery_Detail_SlNo"
            'cmd.ExecuteNonQuery()

            '***** GST START *****
            'cmd.CommandText = "Delete from Sales_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            '***** GST END *****

            cmd.CommandText = "delete from FinishedProduct_Invoice_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()

        End Try


        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.ledger_idno = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_head order by Processed_Item_Name", con)
            da.Fill(dt2)
            cbo_Filter_ItemName.DataSource = dt2
            cbo_Filter_ItemName.DisplayMember = "Processed_Item_Name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 FinishedProduct_Invoice_RefNo from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND FinishedProduct_Invoice_Code LIKE '" & Trim(Pk_Condition) & "%'  Order by for_Orderby, FinishedProduct_Invoice_RefNo", con)
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


    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 FinishedProduct_Invoice_RefNo from FinishedProduct_Invoice_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND FinishedProduct_Invoice_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, FinishedProduct_Invoice_RefNo", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 FinishedProduct_Invoice_RefNo from FinishedProduct_Invoice_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  AND FinishedProduct_Invoice_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, FinishedProduct_Invoice_RefNo desc", con)
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


    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 FinishedProduct_Invoice_RefNo from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND FinishedProduct_Invoice_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, FinishedProduct_Invoice_RefNo desc", con)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            clear()

            New_Entry = True

            '---------------------------

            da1 = New SqlClient.SqlDataAdapter("Select ch.Company_Type from Company_Head ch where ch.company_idno = " & Str(Val(lbl_Company.Tag)), con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                vCmpTyp = Common_Procedures.Remove_NonCharacters(dt1.Rows(0).Item("Company_Type").ToString)
            End If
            dt1.Clear()

            '--------------------------------


            'If Common_Procedures.settings.Invoice_ContinousNo_Status = 1 Then

            'If Trim(UCase(vEntryType)) = "NOTAX" Then

            '    lbl_InvoiceNo.Text = Common_Procedures.get_All_invoice_NoTax_maxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            'Else

            '    lbl_InvoiceNo.Text = Common_Procedures.get_All_invoice_maxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            'End If

            'Else

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "FinishedProduct_Invoice_Head", "FinishedProduct_Invoice_Code", "For_OrderBy", "FinishedProduct_Invoice_Code LIKE '" & Trim(Pk_Condition) & "%' AND Entry_VAT_GST_Type  = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            'End If
            lbl_InvoiceNo.ForeColor = Color.Red
            '   msk_Date.Text = Date.Today

            'da = New SqlClient.SqlDataAdapter("select top 1 a.*, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from FinishedProduct_Invoice_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.FinishedProduct_Invoice_RefNo desc", con)

            da = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as SalesAcName from FinishedProduct_Invoice_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " AND  a.FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.FinishedProduct_Invoice_RefNo desc", con)
            dt2 = New DataTable
            da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                '***** GST START *****

                'If dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString
                If Val(dt2.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False
                If IsDBNull(dt2.Rows(0).Item("FinishedProduct_Invoice_SuffixNo").ToString) = False Then
                    cbo_InvoiceSufixNo.Text = dt2.Rows(0).Item("FinishedProduct_Invoice_SuffixNo").ToString
                End If

                If IsDBNull(dt2.Rows(0).Item("FinishedProduct_Invoice_PrefixNo").ToString) = False Then
                    txt_InvoicePrefixNo.Text = dt2.Rows(0).Item("FinishedProduct_Invoice_PrefixNo").ToString
                End If

                '***** GST END *****
                If dt2.Rows(0).Item("Payment_Method").ToString <> "" Then cbo_PaymentMethod.Text = dt2.Rows(0).Item("Payment_Method").ToString
                If dt2.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAc.Text = dt2.Rows(0).Item("SalesAcName").ToString
                'If dt2.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_TaxAc.Text = dt2.Rows(0).Item("TaxAcName").ToString
                If dt2.Rows(0).Item("Tax_Perc").ToString <> "" Then txt_TaxPerc.Text = Val(dt2.Rows(0).Item("Tax_Perc").ToString)
                If dt2.Rows(0).Item("Transportation_Mode").ToString <> "" Then cbo_TransportMode.Text = dt2.Rows(0).Item("Transportation_Mode").ToString


                If IsDBNull(dt2.Rows(0).Item("Tcs_Tax_Status").ToString) = False Then


                    If Val(dt2.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False

                End If

                If IsDBNull(dt2.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt2.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If

                txt_ExchangeRate.Text = Format(Val(dt2.Rows(0).Item("Exchange_Rate").ToString), "########0.00").Empty

                If IsDBNull(dt2.Rows(0).Item("Courier_Name_Caption").ToString) = False Then txt_CourierName_Cap.Text = dt2.Rows(0).Item("Courier_Name_Caption").ToString
                If IsDBNull(dt2.Rows(0).Item("Courier_No_Caption").ToString) = False Then txt_CourierNo_Cap.Text = dt2.Rows(0).Item("Courier_No_Caption").ToString
                If IsDBNull(dt2.Rows(0).Item("Courier_Date_Caption").ToString) = False Then txt_courier_date.Text = dt2.Rows(0).Item("Courier_Date_Caption").ToString
                If IsDBNull(dt2.Rows(0).Item("Courier_Noof_Box_Caption").ToString) = False Then txt_Courier_Noof_Caption_Cap.Text = dt2.Rows(0).Item("Courier_Noof_Box_Caption").ToString

                'If dt2.Rows(0).Item("Entry_Type").ToString <> "" Then cbo_EntType.Text = dt2.Rows(0).Item("Entry_Type").ToString


            End If
            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            dt2.Dispose()
            da.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            msk_Date.SelectionStart = 0
        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String
        Dim vCSMovNo As String
        Dim InvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""
        Dim vOSmovCode As String = ""
        Dim vOSmovNo As String = ""
        Dim vJWmovCode As String = ""
        Dim vJWmovNo As String = ""

        Dim vUpvcMovCode As String = ""
        Dim vUbvcMovNo As String = ""

        Dim vInNt_MovNo As String = ""
        Dim vInNt_MovCode As String = ""

        Dim vRInNt_MovNo As String = ""
        Dim vRInNt_MovCode As String = ""

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            RefCode = Trim(Trim(Pk_Condition) & "" & Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select FinishedProduct_Invoice_RefNo from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()




            'If Common_Procedures.settings.Invoice_ContinousNo_Status = 1 Then



            '    If Trim(UCase(vEntryType)) = "NOTAX" Then

            '        vRInNt_MovNo = ""

            '        vRInNt_MovCode = "RINNT-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            '        Da = New SqlClient.SqlDataAdapter("select sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(vRInNt_MovCode) & "'", con)
            '        Dt = New DataTable
            '        Da.Fill(Dt)
            '        If Dt.Rows.Count > 0 Then
            '            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '                vRInNt_MovNo = Trim(Dt.Rows(0)(0).ToString)
            '            End If
            '        End If
            '        Dt.Clear()

            '    Else

            '        vYSMovNo = ""

            '        vYSInvCode = "GRINV-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            '        Da = New SqlClient.SqlDataAdapter("select sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(vYSInvCode) & "'", con)
            '        Dt = New DataTable
            '        Da.Fill(Dt)
            '        If Dt.Rows.Count > 0 Then
            '            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '                vYSMovNo = Trim(Dt.Rows(0)(0).ToString)
            '            End If
            '        End If
            '        Dt.Clear()

            '        vOSmovCode = "GSSAL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            '        Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_Reference_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Other_GST_Entry_No = '" & Trim(inpno) & "' and Other_GST_Entry_Reference_Code LIKE 'GSSAL-%' ", con)
            '        Dt = New DataTable
            '        Da.Fill(Dt)

            '        vOSmovNo = ""
            '        If Dt.Rows.Count > 0 Then
            '            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '                vOSmovNo = Trim(Dt.Rows(0)(0).ToString)
            '            End If
            '        End If

            '        Dt.Clear()

            '        '*************************UPVC SALES 
            '        vUpvcMovCode = "GSUPV-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            '        Da = New SqlClient.SqlDataAdapter("select sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(vUpvcMovCode) & "'", con)
            '        Dt = New DataTable
            '        Da.Fill(Dt)
            '        If Dt.Rows.Count > 0 Then
            '            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '                vUbvcMovNo = Trim(Dt.Rows(0)(0).ToString)
            '            End If
            '        End If
            '        Dt.Clear()

            '    End If

            'End If

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vYSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Rental Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vOSmovNo) <> 0 Then
                MessageBox.Show("This Invoice No.  Is  in Other Sales Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vUbvcMovNo) <> 0 Then
                MessageBox.Show("This Invoice No.  Is  in UPVC Sales Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vRInNt_MovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Rental Invoice With Out Tax", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Invocie No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim InvCode As String

        Dim vCSMovNo As String
        Dim vCSInvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""
        Dim vOSmovCode As String = ""
        Dim vOSmovNo As String = ""

        Dim vJWmovCode As String = ""
        Dim vJWmovNo As String = ""

        Dim vUpvcMovCode As String = ""
        Dim vUbvcMovNo As String = ""


        Dim vInNt_MovNo As String = ""
        Dim vInNt_MovCode As String = ""

        Dim vRInNt_MovNo As String = ""
        Dim vRInNt_MovCode As String = ""



        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sales_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Invocie No.", "FOR NEW INVOICE INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select FinishedProduct_Invoice_RefNo from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()





            'If Common_Procedures.settings.Invoice_ContinousNo_Status = 1 Then


            '    If Trim(UCase(vEntryType)) = "NOTAX" Then

            '        vRInNt_MovNo = ""

            '        vRInNt_MovCode = "RINNT-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            '        Da = New SqlClient.SqlDataAdapter("select sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(vRInNt_MovCode) & "'", con)
            '        Dt = New DataTable
            '        Da.Fill(Dt)
            '        If Dt.Rows.Count > 0 Then
            '            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '                vRInNt_MovNo = Trim(Dt.Rows(0)(0).ToString)
            '            End If
            '        End If

            '        Dt.Clear()

            '    Else


            '        vYSMovNo = ""

            '        vYSInvCode = "GRINV-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            '        Da = New SqlClient.SqlDataAdapter("select sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(vYSInvCode) & "'", con)
            '        Dt = New DataTable
            '        Da.Fill(Dt)
            '        If Dt.Rows.Count > 0 Then
            '            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '                vYSMovNo = Trim(Dt.Rows(0)(0).ToString)
            '            End If
            '        End If

            '        Dt.Clear()
            '        vOSmovCode = "GSSAL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            '        Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_Reference_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Other_GST_Entry_No = '" & Trim(inpno) & "' and Other_GST_Entry_Reference_Code LIKE 'GSSAL-%' ", con)
            '        Dt = New DataTable
            '        Da.Fill(Dt)

            '        vOSmovNo = ""
            '        If Dt.Rows.Count > 0 Then
            '            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '                vOSmovNo = Trim(Dt.Rows(0)(0).ToString)
            '            End If
            '        End If

            '        Dt.Clear()


            '        vUpvcMovCode = "GSUPV-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            '        Da = New SqlClient.SqlDataAdapter("select sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(vUpvcMovCode) & "'", con)
            '        Dt = New DataTable
            '        Da.Fill(Dt)
            '        If Dt.Rows.Count > 0 Then
            '            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '                vUbvcMovNo = Trim(Dt.Rows(0)(0).ToString)
            '            End If
            '        End If

            '        Dt.Clear()

            '    End If


            'End If

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vYSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Rental Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vOSmovNo) <> 0 Then
                MessageBox.Show("This Invoice No.  Is  in Other Sales Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vUbvcMovNo) <> 0 Then
                MessageBox.Show("This Invoice No.  Is  in UPVC Sales Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vRInNt_MovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Rental Invoice With Out Tax", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Invoice No.", "DOES NOT INSERT NEW INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvoiceNo.Text = Trim(UCase(inpno))

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
        Dim da1 As SqlClient.SqlDataAdapter
        Dim dt1 As DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim led_id As Integer = 0
        Dim saleac_id As Integer = 0
        Dim txac_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim CsParNm As String
        Dim vTotQty As Single = 0
        Dim vforOrdby As Single = 0
        Dim Amt As Single = 0
        Dim L_ID As Integer = 0
        Dim chk_Lab As Integer = 0
        Dim VouBil As String = ""
        Dim vDelvTo_IdNo As Integer = 0
        Dim Trans_id As Integer = 0
        Dim vLed_IdNos As String = ""
        Dim vTCS_AssVal_EditSTS As Integer = 0
        Dim vTCS_Tax_Sts As Integer = 0
        Dim vTCSAmtRndOff_STS As Integer = 0
        Dim Curr_id As Integer = 0
        Dim NewCode2 As String = ""
        Dim vOrdByNo As String = ""
        Dim vInvoNo As String = ""
        Dim vEInvAckDate As String = ""
        Dim vGST_Tax_Inv_Sts As Integer = 0
        Dim lckdt As Date = #12/12/2100#
        Dim dat As Date = Now
        Dim Nr As Integer = 0
        Dim vSize_Idno As Integer = 0
        Dim vColour_Idno As Integer = 0
        Dim LotNo_ID As Integer = 0
        Dim VWarehouse_idno As Integer = 0
        Dim Item_Set_ID As Integer = 0
        Dim Dispatch_Led_ID As Integer = 0
        Dim vtot_grss_wgt = ""
        Dim vtot_Net_wgt = ""
        Dim vtot_No_Of_Packs = ""
        Dim vLrDt As String = ""
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sales_Entry, New_Entry) = False Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1304" Then  '---  SANTHOSH BLUE METALS (KARANAMPETTAI)    and   KMT BLUE METAL (KARANAMPETTAI)

            If Insert_Entry = True Or New_Entry = True Then

                lckdt = #1/1/2025#

                If IsDate(Common_Procedures.settings.Sdd) = True Then
                    dat = Common_Procedures.settings.Sdd
                End If

                If DateDiff("d", lckdt.ToShortDateString, dat.ToShortDateString) > 0 Then

                    Dim vTOT_NOOFINV As Integer = 0
                    Dim Yr1 As Integer
                    Dim Yr2 As Integer
                    Dim vNOOFYRS As Integer
                    Dim vAVG_INV As Integer

                    vTOT_NOOFINV = 0
                    da1 = New SqlClient.SqlDataAdapter("select sum(tni) from Settings_Head", con)
                    dt1 = New DataTable
                    da1.Fill(dt1)
                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            vTOT_NOOFINV = Val(dt1.Rows(0)(0).ToString)
                        End If
                    End If
                    dt1.Clear()

                    Yr1 = Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
                    Yr2 = Val(Microsoft.VisualBasic.Right(Trim(Common_Procedures.FnRange), 4))

                    vNOOFYRS = Yr2 - Yr1
                    If vNOOFYRS = 0 Then vNOOFYRS = 1

                    vAVG_INV = (vTOT_NOOFINV \ vNOOFYRS) - 50

                    If Val(lbl_InvoiceNo.Text) > vAVG_INV Then
                        MessageBox.Show("Run-time error '6' : " & Chr(13) & Chr(13) & "Overflow", "DOES NOT SAVE", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                        Me.Close()
                        Application.Exit()
                    End If

                    'MessageBox.Show("Run-time error '6': " & Chr(13) & Chr(13) & "Overflow", "DOES NOT SAVE", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                    'Me.Close()
                    'Application.Exit()

                End If

            End If

        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        'If cbo_EntType.Visible And cbo_EntType.Text = "" Then
        '    If (Trim(UCase(cbo_EntType.Text)) <> "DIRECT" And Trim(UCase(cbo_EntType.Text)) <> "DELIVERY" And Trim(UCase(cbo_EntType.Text)) <> "ORDER" And Trim(UCase(cbo_EntType.Text)) <> "PROFORMA") Then
        '        MessageBox.Show("Invalid Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
        '        Exit Sub
        '    End If
        'End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        CsParNm = ""
        If led_id = 0 Then
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CREDIT" Then
                MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
                Exit Sub

            Else

                led_id = 1
                CsParNm = Trim(txt_cash_Party_name.Text)

                'led_id = 1
                'CsParNm = Trim(cbo_Ledger.Text)

            End If
        End If

        'If led_id = 1 And Trim(CsParNm) = "" Then
        '    CsParNm = "Cash"
        'End If

        ' ---------


        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Val(led_id) <> 1 Then

            CsParNm = Common_Procedures.Ledger_IdNoToName(con, led_id)

        End If

        If led_id = 1 And Trim(CsParNm) = "" Then
            CsParNm = "Cash"
        End If


        ' --------


        Vchk_shirt_bill = 0
        If Chk_shirt_bill.Checked = True Then Vchk_shirt_bill = 1

        saleac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        If saleac_id = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            saleac_id = 22
            'MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If

        txac_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_TaxAc.Text)

        Trans_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Transport.Text)

        If txac_id = 0 And Val(lbl_TaxAmount.Text) <> 0 Then
            txac_id = 20
            'MessageBox.Show("Invalid Tax A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If
        vDelvTo_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_DeliveryTo.Text)

        Curr_id = Common_Procedures.Currency_NameToIdNo(con, cbo_Currency.Text)

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(DgvCol_Details.QUANTITY).Value) <> 0 Then

                    itm_id = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(DgvCol_Details.ITEM_NAME).Value)
                    If itm_id = 0 Then
                        MessageBox.Show("Invalid iTEM Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(DgvCol_Details.ITEM_NAME)
                        End If
                        Exit Sub
                    End If

                    unt_id = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(DgvCol_Details.UNIT).Value)
                    If unt_id = 0 Then
                        MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(DgvCol_Details.UNIT)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(DgvCol_Details.RATE_QTY).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(DgvCol_Details.RATE_QTY)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With
        vTCS_Tax_Sts = 0
        If chk_TCS_Tax.Checked = True Then vTCS_Tax_Sts = 1
        vTCS_AssVal_EditSTS = 0
        If txt_TCS_TaxableValue.Enabled = True Then vTCS_AssVal_EditSTS = 1
        vTCSAmtRndOff_STS = 0
        If chk_TCSAmount_RoundOff_STS.Checked = True Then vTCSAmtRndOff_STS = 1
        vGST_Tax_Inv_Sts = 0
        If chk_GSTTax_Invocie.Checked = True Then vGST_Tax_Inv_Sts = 1

        '***** GST START *****
        NoCalc_Status = False

        Amount_Calculation(True)


        'TotalAmount_Calculation()
        'NetAmount_Calculation()
        '***** GST END *****

        vtot_grss_wgt = 0
        vtot_Net_wgt = 0
        vTotQty = 0
        vtot_No_Of_Packs = 0
        If dgv_Details_Total.RowCount > 0 Then
            vtot_grss_wgt = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.GROSS_WEIGHT).Value())
            vtot_Net_wgt = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.NET_WEIGHT).Value())
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.QUANTITY).Value())
            vtot_No_Of_Packs = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.NO_OF_PACKS).Value())
        End If

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@FinishedProductInvoiceDate", Convert.ToDateTime(msk_Date.Text))

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

        vLrDt = ""
        If Trim(msk_Lr_Date.Text) <> "" Then
            If IsDate(msk_Lr_Date.Text) = True Then
                vLrDt = Trim(msk_Lr_Date.Text)
            End If
        End If

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

        If chk_LabourBill.Checked = True Then chk_Lab = 1

        Dispatch_Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DispatcherName.Text)

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                'If Common_Procedures.settings.Invoice_ContinousNo_Status = 1 Then

                '    If Trim(UCase(vEntryType)) = "NOTAX" Then

                '        lbl_InvoiceNo.Text = Common_Procedures.get_All_invoice_NoTax_maxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                '    Else
                '        lbl_InvoiceNo.Text = Common_Procedures.get_All_invoice_maxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                '    End If

                'Else

                '    lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "Sales_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)


                'End If

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            vInvoNo = Trim(txt_InvoicePrefixNo.Text) & Trim(lbl_InvoiceNo.Text) & Trim(cbo_InvoiceSufixNo.Text)

            cmd.Connection = con
            cmd.Transaction = tr



            vforOrdby = Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))

            If New_Entry = True Then

                If Trim(txt_DateTime_Of_Supply.Text) = "" Then txt_DateTime_Of_Supply.Text = Format(Now, "dd-MM-yyyy hh:mm tt")

                '***** GST START *****
                cmd.CommandText = "Insert into FinishedProduct_Invoice_Head ( Entry_VAT_GST_Type,           FinishedProduct_Invoice_Code      ,              Company_IdNo        ,       FinishedProduct_Invoice_No    ,             for_OrderBy       ,              FinishedProduct_Invoice_RefNo     ,    FinishedProduct_Invoice_Date ,               Payment_Method          ,          Ledger_IdNo    ,        Cash_PartyName  ,             Order_No            ,              Order_Date           ,      Dc_No                  ,              Dc_Date           ,           SalesAc_IdNo      ,  Tax_Type,           TaxAc_IdNo     ,               Narration           ,           Total_Qty      ,              SubTotal_Amount          , Total_DiscountAmount, Total_TaxAmount,              Gross_Amount             ,                 CashDiscount_Perc      ,              CashDiscount_Amount         ,             Assessable_Value         ,              Tax_Perc             ,                Tax_Amount           ,              Freight_Amount       ,              AddLess_Amount       ,              Round_Off             ,             Net_Amount                    ,               Vehicle_No          ,              Payment_Terms            , Labour_Charge            ,    Sales_Order_Selection_Code   ,                Electronic_Reference_No   ,               Transportation_Mode     ,               Date_Time_Of_Supply         ,                 CGst_Amount          ,                SGst_Amount           ,               IGst_Amount             ,           DeliveryTo_IdNo    ,              Place_Of_Supply               ,                      Due_Days           ,Shirt_Bill_Status         ,      Transport_IdNo  ,        Trans_Freight_Amt                        ,       Tcs_Name_caption           ,                    Tcs_percentage       ,                    Tcs_Amount    ,                     TCS_Taxable_Value,                            EDIT_TCS_TaxableValue ,             Tcs_Tax_Status,             TCSAmount_RoundOff_Status,                         Invoice_Value_Before_TCS ,                       RoundOff_Invoice_Value_Before_TCS             ,        Currency_Idno     ,                    Exchange_Rate        ,       FinishedProduct_Invoice_SuffixNo     ,              E_Invoice_IRNO               ,    E_Invoice_QR_Image      ,           Dispatcher_IdNo     ,            Courier_Name_Caption         ,                    Courier_Name       ,               Courier_No_Caption       ,                     Courier_No         ,             Courier_Date_Caption        ,               Courier_Date                ,          Courier_Noof_Box_Caption            ,               Courier_Noof_Box        ,           Po_No     , FinishedProduct_Invoice_PrefixNo        ,           pre_Carriage_by             ,                   Place_of_receipt_by_Pre_Carrier         ,               Vessal_Flight_No             ,               Port_Of_Loading            ,                 Port_Of_Discharge             ,               Final_Destination           ,           Other_Reference             ,             Exporters_Reference       ,           Terms_Payment_Delivery_Detail_1     ,           Terms_Payment_Delivery_Detail_2   ,             Terms_Payment_Delivery_Detail_3         ,        Total_Gross_Weight              ,           Total_Weight        ,            GST_Tax_Invoice_Status            ,            Total_No_of_Packs        ,                Lr_No          ,          Lr_Date       ,             Inco_term               ) " &
                                  "            Values     (                           'GST'     , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",        '" & Trim(vInvoNo) & "'      ,    " & Str(Val(vforOrdby)) & ",            '" & Trim(lbl_InvoiceNo.Text) & "'  ,    @FinishedProductInvoiceDate  , '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", '" & Trim(CsParNm) & "', '" & Trim(txt_OrderNo.Text) & "', '" & Trim(txt_OrderDate.Text) & "', '" & Trim(txt_DcNo.Text) & "', '" & Trim(txt_DcDate.Text) & "', " & Str(Val(saleac_id)) & ",    'VAT' , " & Str(Val(txac_id)) & ", '" & Trim(txt_Narration.Text) & "', " & Str(Val(vTotQty)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ",           0         ,       0         , " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_cashDisc_amt.Text)) & ", " & Str(Val(lbl_Assessable.Text)) & ", " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", '" & Trim(txt_VehicleNo.Text) & "', '" & Trim(txt_PaymentTerms.Text) & "' , " & Str(Val(chk_Lab)) & ", '" & Trim(cbo_OrderNo.Text) & "', '" & Trim(txt_Electronic_RefNo.Text) & "', '" & Trim(cbo_TransportMode.Text) & "', '" & Trim(txt_DateTime_Of_Supply.Text) & "', " & Str(Val(lbl_CGstAmount.Text)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", " & Str(Val(lbl_IGstAmount.Text)) & " ," & Str(Val(vDelvTo_IdNo)) & ", '" & Trim(txt_Place_Of_Supply.Text) & "'   ,      '" & Trim(txt_Due_Days.Text) & "' ," & Val(Vchk_shirt_bill) & ",  " & Str(Val(Trans_id)) & " , " & Str(Val(txt_Trans_Freight.Text)) & ", '" & Trim(txt_Tcs_Name.Text) & "',       " & Str(Val(txt_TcsPerc.Text)) & ",    " & Str(Val(lbl_TcsAmount.Text)) & " ,  " & Str(Val(txt_TCS_TaxableValue.Text)) & ", " & Str(Val(vTCS_AssVal_EditSTS)) & ", " & Str(Val(vTCS_Tax_Sts)) & ", " & Str(Val(vTCSAmtRndOff_STS)) & ", " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & "  , " & Str(Val(Curr_id)) & ", " & Str(Val(txt_ExchangeRate.Text)) & " ,   '" & Trim(cbo_InvoiceSufixNo.Text) & "'  ,       '" & Trim(txt_eInvoiceNo.Text) & "' ,              @QrCode   ,  " & Val(Dispatch_Led_ID) & "  , '" & Trim(txt_CourierName_Cap.Text) & "', '" & Trim(txt_CourierName.Text) & "'  , '" & Trim(txt_CourierNo_Cap.Text) & "' , '" & Trim(txt_Courier_No.Text) & "' , '" & Trim(txt_CourierDate_Cap.Text) & "' , '" & Trim(txt_courier_date.Text) & "'   ,'" & Trim(txt_Courier_Noof_Caption_Cap.Text) & "' ," & Val(txt_Courier_Noof_Box.Text) & " ,'" & Trim(txt_PoNo.Text) & "', '" & Trim(txt_InvoicePrefixNo.Text) & "','" & Trim(cbo_Pre_Carriage_by.Text) & "'  , '" & Trim(txt_Place_Of_Receipt_By_Pre_carrier.Text) & "'   , '" & Trim(cbo_Vessal_Flight_No.Text) & "'  , '" & Trim(txt_Port_Of_Loading.Text) & "'  , '" & Trim(txt_Port_Of_Discharge.Text) & "'   ,'" & Trim(txt_Final_destination.Text) & "' ,'" & Trim(txt_Other_Reference.Text) & "','" & Trim(txt_Exports_Ref.Text) & "','" & Trim(txt_Terms_Delivery_Payment_1.Text) & "','" & Trim(txt_Terms_Delivery_Payment_2.Text) & "','" & Trim(txt_Terms_Delivery_Payment_3.Text) & "',   " & Str(Val(vtot_grss_wgt)) & "  ," & Str(Val(vtot_Net_wgt)) & " ,      " & Str(Val(vGST_Tax_Inv_Sts)) & " ,  " & Str(Val(vtot_No_Of_Packs)) & " , '" & Trim(txt_LrNo.Text) & "' ,  '" & Trim(vLrDt) & "' , '" & Trim(txt_Inco_Term.Text) & "'  ) "
                cmd.ExecuteNonQuery()
                '***** GST END *****

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "FinishedProduct_Invoice_Head", "FinishedProduct_Invoice_Code", Val(lbl_Company.Tag), Trim(Pk_Condition) & NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "FinishedProduct_Invoice_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "FinishedProduct_Invoice_Details", "FinishedProduct_Invoice_Code", Val(lbl_Company.Tag), Trim(Pk_Condition) & NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo,Unit_IdNo,Colour_Idno,No_Of_Packs,No_of_Pcs_Per_Packs,Serial_No,Noof_Items,Rate,Amount,Total_Amount,Cash_Discount_Perc_For_All_Item,Cash_Discount_Amount_For_All_Item,Assessable_Value,HSN_Code,Tax_Perc,Order_No", "Sl_No", "FinishedProduct_Invoice_Code, For_OrderBy, Company_IdNo,  FinishedProduct_Invoice_Date, Ledger_Idno", tr)

                '***** GST START *****
                cmd.CommandText = "Update FinishedProduct_Invoice_Head set Entry_VAT_GST_Type = 'GST',  FinishedProduct_Invoice_RefNo = '" & Trim(lbl_InvoiceNo.Text) & "'  ,   FinishedProduct_Invoice_Date = @FinishedProductInvoiceDate, Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", Cash_PartyName = '" & Trim(CsParNm) & "', Order_No = '" & Trim(txt_OrderNo.Text) & "', Order_Date = '" & Trim(txt_OrderDate.Text) & "', Dc_No = '" & Trim(txt_DcNo.Text) & "', Dc_Date = '" & Trim(txt_DcDate.Text) & "', SalesAc_IdNo = " & Str(Val(saleac_id)) & ", Tax_Type = 'VAT', TaxAc_IdNo = " & Str(Val(txac_id)) & ", Narration = '" & Trim(txt_Narration.Text) & "', Total_Qty = " & Str(Val(vTotQty)) & ", SubTotal_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Total_DiscountAmount = 0, Total_TaxAmount = 0, Gross_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_cashDisc_amt.Text)) & ", Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Tax_Perc = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", Labour_Charge = " & Str(Val(chk_Lab)) & " , AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", Round_Off = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Vehicle_No = '" & Trim(txt_VehicleNo.Text) & "', Payment_Terms = '" & Trim(txt_PaymentTerms.Text) & "' ,Sales_Order_Selection_Code = '" & Trim(cbo_OrderNo.Text) & "',  Electronic_Reference_No = '" & Trim(txt_Electronic_RefNo.Text) & "' ,  Transportation_Mode = '" & Trim(cbo_TransportMode.Text) & "'  ,  Date_Time_Of_Supply = '" & Trim(txt_DateTime_Of_Supply.Text) & "'  ,  CGst_Amount = " & Str(Val(lbl_CGstAmount.Text)) & " , SGst_Amount = " & Str(Val(lbl_SGstAmount.Text)) & " , IGst_Amount = " & Str(Val(lbl_IGstAmount.Text)) & ",DeliveryTo_IdNo = " & Str(Val(vDelvTo_IdNo)) & " , Place_Of_Supply = '" & Trim(txt_Place_Of_Supply.Text) & "' ,Due_Days='" & Trim(txt_Due_Days.Text) & "',Shirt_Bill_Status=" & Val(Vchk_shirt_bill) & " ,Transport_IdNo = " & Str(Val(Trans_id)) & ", Trans_Freight_Amt =" & Str(Val(txt_Trans_Freight.Text)) & " ,  Tcs_Name_caption = '" & Trim(txt_Tcs_Name.Text) & "',Dispatcher_IdNo = " & Val(Dispatch_Led_ID) & ", Tcs_percentage=" & Str(Val(txt_TcsPerc.Text)) & ",Tcs_Amount= " & Str(Val(lbl_TcsAmount.Text)) & " , TCS_Taxable_Value = " & Str(Val(txt_TCS_TaxableValue.Text)) & ", EDIT_TCS_TaxableValue = " & Str(Val(vTCS_AssVal_EditSTS)) & " , Tcs_Tax_Status = " & Str(Val(vTCS_Tax_Sts)) & " , TCSAmount_RoundOff_Status = " & Str(Val(vTCSAmtRndOff_STS)) & "  , Invoice_Value_Before_TCS = " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", RoundOff_Invoice_Value_Before_TCS = " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & " , Currency_Idno =" & Str(Val(Curr_id)) & " , Exchange_Rate =" & Str(Val(txt_ExchangeRate.Text)) & ", FinishedProduct_invoice_SuffixNo = '" & Trim(cbo_InvoiceSufixNo.Text) & "' , E_Invoice_IRNO = '" & Trim(txt_IR_No.Text) & "' , E_Invoice_QR_Image =  @QrCode  , E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & "  ,  E_Invoice_Cancelled_Status = " & eiCancel.ToString & " ,  E_Invoice_Cancellation_Reason = '" & txt_EInvoiceCancellationReson.Text & "'  ,    EWB_No = '" & txt_Electronic_RefNo.Text & "',EWB_Date = '" & txt_EWB_Date.Text & "',EWB_Valid_Upto = '" & txt_EWB_ValidUpto.Text & "',EWB_Cancelled = " & EWBCancel.ToString & " ,  EWBCancellation_Reason = '" & txt_EWB_Canellation_Reason.Text & "' , " &
                "   Courier_Name_Caption ='" & Trim(txt_CourierName_Cap.Text) & "' ,  Courier_Name ='" & Trim(txt_CourierName.Text) & "' ,  Courier_No_Caption ='" & Trim(txt_CourierNo_Cap.Text) & "'  ,Courier_No='" & Trim(txt_Courier_No.Text) & "'    ,Courier_Date_Caption ='" & Trim(txt_CourierDate_Cap.Text) & "'  ,Courier_Date ='" & Trim(txt_courier_date.Text) & "'  , Courier_Noof_Box_Caption ='" & Trim(txt_Courier_Noof_Caption_Cap.Text) & "' ,Courier_Noof_Box  =" & Val(txt_Courier_Noof_Box.Text) & " , Po_No    ='" & Trim(txt_PoNo.Text) & "',FinishedProduct_invoice_PrefixNo = '" & Trim(txt_InvoicePrefixNo.Text) & "' ,FinishedProduct_Invoice_No = '" & Trim(vInvoNo) & "' , pre_Carriage_by = '" & Trim(cbo_Pre_Carriage_by.Text) & "' , Place_of_receipt_by_Pre_Carrier   ='" & Trim(txt_Place_Of_Receipt_By_Pre_carrier.Text) & "' , Vessal_Flight_No  ='" & Trim(cbo_Vessal_Flight_No.Text) & "'   , Port_Of_Loading = '" & Trim(txt_Port_Of_Loading.Text) & "'  ,       Port_Of_Discharge   ='" & Trim(txt_Port_Of_Discharge.Text) & "'  , Final_Destination ='" & Trim(txt_Final_destination.Text) & "'  , Other_Reference  =  '" & Trim(txt_Other_Reference.Text) & "' ,Exporters_Reference   = '" & Trim(txt_Exports_Ref.Text) & "', Terms_Payment_Delivery_Detail_1 ='" & Trim(txt_Terms_Delivery_Payment_1.Text) & "',   Terms_Payment_Delivery_Detail_2  = '" & Trim(txt_Terms_Delivery_Payment_2.Text) & "' ,  Terms_Payment_Delivery_Detail_3  ='" & Trim(txt_Terms_Delivery_Payment_3.Text) & "'  ,    Total_Gross_Weight =" & Str(Val(vtot_grss_wgt)) & "  ,  Total_Weight =" & Str(Val(vtot_Net_wgt)) & " , GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & " , Total_No_of_Packs = " & Str(Val(vtot_No_Of_Packs)) & " , Lr_No = '" & Trim(txt_LrNo.Text) & "'  , Lr_Date  = '" & Trim(vLrDt) & "' , Inco_term = '" & Trim(txt_Inco_Term.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                '***** GST END *****

                'cmd.CommandText = "Update Sales_Delivery_Details Set Receipt_Quantity = a.Receipt_Quantity - b.Noof_Items from Sales_dELIVERY_Details a, Sales_Details b where b.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and b.Entry_Type = 'DELIVERY' and a.Sales_Delivery_Code = b.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = b.Sales_Delivery_Detail_SlNo"
                'cmd.ExecuteNonQuery()


            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Sales_Head", "Sales_Code", Val(lbl_Company.Tag), Trim(Pk_Condition) & NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Sales_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from FinishedProduct_Invoice_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            Partcls = "Inv : " & Trim(lbl_InvoiceNo.Text)
            PBlNo = Trim(lbl_InvoiceNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_InvoiceNo.Text)

            With dgv_Details

                Sno = 0

                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(DgvCol_Details.QUANTITY).Value) <> 0 Then

                        itm_id = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(DgvCol_Details.ITEM_NAME).Value, tr)

                        unt_id = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(DgvCol_Details.UNIT).Value, tr)

                        LotNo_ID = Common_Procedures.LotNo_NameToIdNo(con, .Rows(i).Cells(DgvCol_Details.LOT_NO).Value, tr)

                        vSize_Idno = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(DgvCol_Details.SIZE).Value, tr)

                        vColour_Idno = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(DgvCol_Details.COLOUR).Value, tr)



                        'Dim vProformaCode = ""
                        'Dim vProformaSlNo = 0
                        'If Trim(UCase(cbo_EntType.Text)) = "PROFORMA" Then
                        '    vProformaCode = Trim(.Rows(i).Cells(DgvCol_Details.PROFORMA_INV_CODE).Value)
                        'End If
                        'Dim DcCd = ""
                        'Dim DcSlNo = 0
                        'If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                        '    DcCd = Trim(.Rows(i).Cells(DgvCol_Details.SALES_DELV_CODE).Value)
                        '    DcSlNo = Val(.Rows(i).Cells(DgvCol_Details.SALES_DELV_SLNO).Value)
                        'End If

                        'VWarehouse_idno=4 'GODOWN

                        If itm_id <> 0 Then

                            Sno = Sno + 1

                            '***** GST START *****
                            'cmd.CommandText = "Insert into FinishedProduct_Invoice_Details ( FinishedProduct_Invoice_Code,             Company_IdNo         ,  FinishedProduct_Invoice_No ,       FinishedProduct_Invoice_RefNo  ,          for_OrderBy        , FinishedProduct_Invoice_Date,          Ledger_IdNo    ,        Sl_No         ,          Item_IdNo      ,          Unit_IdNo      ,                                                    Serial_No             ,                                      Noof_Items               ,                      Rate                ,                                                           Amount              ,                                  Total_Amount        ,                                        Cash_Discount_Perc_For_All_Item    ,                             Cash_Discount_Amount_For_All_Item  ,                                                      Assessable_Value            ,                                     HSN_Code                               ,                      Tax_Perc                        ,            LotNo_Idno              ,                     RateWithTax                            ,                                             Item_Set_IdNo      ,          Sales_Proforma_Code      , Sales_Delivery_Code   ,  Sales_dELIVERY_Detail_SlNo                      ,Entry_Type      ,                                Gross_Weight                      ,                                                         Weight     ,                                      Container_No                     ,               SIZE_IDNO         ,   COLOUR_iDNO  ) " &
                            '                        " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "'               , " & Str(Val(lbl_Company.Tag)) & ",    '" & Trim(vInvoNo) & "'  ,    '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", @FinishedProductInvoiceDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", '" & Trim(.Rows(i).Cells(DgvCol_Details.DESCRIBTION).Value) & "', " & Str(Val(.Rows(i).Cells(DgvCol_Details.QUANTITY).Value)) & ", " & Str(Val(.Rows(i).Cells(DgvCol_Details.RATE_QTY).Value)) & ", " & Str(Val(.Rows(i).Cells(DgvCol_Details.AMOUNT).Value)) & ", " & Str(Val(.Rows(i).Cells(DgvCol_Details.AMOUNT).Value)) & ", " & Str(Val(.Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value)) & ", " & Str(Val(.Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value)) & ", " & Str(Val(.Rows(i).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value)) & ", '" & Trim(.Rows(i).Cells(DgvCol_Details.HSN_CODE).Value) & "', " & Str(Val(.Rows(i).Cells(DgvCol_Details.GST_PERC).Value)) & " ,  " & Str(Val(LotNo_ID)) & "    ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.GST_RATE).Value)) & "," & Val(.Rows(i).Cells(DgvCol_Details.ITEM_SET_IDNO).Value) & " ,'" & Trim(vProformaCode) & "','" & Trim(DcCd) & "'                    , " & Val(DcSlNo) & "  ,'" & Trim(cbo_EntType.Text) & "'," & Str(Val(.Rows(i).Cells(DgvCol_Details.GROSS_WEIGHT).Value)) & "," & Str(Val(.Rows(i).Cells(DgvCol_Details.NET_WEIGHT).Value)) & "  , '" & Trim(.Rows(i).Cells(DgvCol_Details.CONTAINER_NO).Value) & "' ,    " & Str(Val(vSize_Idno)) & " ,  " & Str(Val(vColour_Idno)) & "   ) "
                            'cmd.ExecuteNonQuery()


                            cmd.CommandText = "Insert into FinishedProduct_Invoice_Details (FinishedProduct_Invoice_Code,           Company_IdNo            ,     FinishedProduct_Invoice_No     ,            for_OrderBy      ,  FinishedProduct_Invoice_Date ,        Ledger_IdNo      ,         Sl_No          ,     FinishedProduct_IdNo,       Item_IdNo    ,         Unit_IdNo       ,            Size_Idno          ,             Colour_Idno         ,                                 Order_No                       ,                             HSN_Code                         ,                             Tax_Perc                              ,                               No_Of_Packs                          ,                                 No_Of_Pcs_Per_Packs                        ,                            No_Of_Items                          ,                                 Weight                            ,                                Gross_Weight                        ,                                  Rate                            ,                             Amount                              ,                            Serial_No                             ,                                  Package_No                      ,                                Assessable_Value                           ) " &
                                                    "Values   ('" & Trim(Pk_Condition) & Trim(NewCode) & "'             , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_InvoiceNo.Text) & "' ,  " & Str(Val(vforOrdby)) & ",   @FinishedProductInvoiceDate , " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & "  , " & Str(Val(itm_id)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ",  " & Str(Val(vSize_Idno)) & " ,  " & Str(Val(vColour_Idno)) & " ,  '" & Trim(.Rows(i).Cells(DgvCol_Details.ORDER_NO).Value) & "' , '" & Trim(.Rows(i).Cells(DgvCol_Details.HSN_CODE).Value) & "',   " & Str(Val(.Rows(i).Cells(DgvCol_Details.GST_PERC).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.NO_OF_PACKS).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.NO_OF_PCS_PER_PACKS).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.QUANTITY).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.NET_WEIGHT).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.GROSS_WEIGHT).Value)) & ",  " & Str(Val(.Rows(i).Cells(DgvCol_Details.RATE_QTY).Value)) & " ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.AMOUNT).Value)) & "  , '" & Trim(.Rows(i).Cells(DgvCol_Details.DESCRIBTION).Value) & "' ,  '" & Trim(.Rows(i).Cells(DgvCol_Details.PACKAGE_NO).Value) & "' ,   " & Str(Val(.Rows(i).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value)) & " )"

                            cmd.ExecuteNonQuery()


                            cmd.CommandText = "Insert into Stock_Item_Processing_Details (                         Reference_Code,                   Company_IdNo            ,           Reference_No        ,           for_OrderBy          ,              Reference_Date       ,   DeliveryTo_StockIdNo     ,                           ReceivedFrom_StockIdNo          ,         Entry_ID     ,       Party_Bill_No  ,     Particulars        ,            Sl_No      ,         Item_IdNo        ,                                             Quantity             ,                                              Meters ) " &
                                                                         "  Values (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & " ,    @FinishedProductinvoiceDate    , " & Str(Val(led_id)) & "  ," & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "  , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & " , " & Str(Val(itm_id)) & " ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.QUANTITY).Value)) & " ," & Str(Val(.Rows(i).Cells(DgvCol_Details.NET_WEIGHT).Value)) & "  )"
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next i
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Sales_Details", "Sales_Code", Val(lbl_Company.Tag), Trim(Pk_Condition) & NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo,Serial_No,Noof_Items,Rate,Amount,Total_Amount,Cash_Discount_Perc_For_All_Item,Cash_Discount_Amount_For_All_Item,Assessable_Value,HSN_Code,Tax_Perc", "Sl_No", "Sales_Code, For_OrderBy, Company_IdNo, Sales_No, Sales_Date, Ledger_Idno", tr)

            End With

            '***** GST START *****
            '---Tax Details
            cmd.CommandText = "Delete from Sales_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_GSTTax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Sales_GST_Tax_Details   (        Sales_Code      ,               Company_IdNo       ,                Sales_No           ,                               for_OrderBy                                  , Sales_Date ,         Ledger_IdNo     ,            Sl_No     ,                    HSN_Code            ,                      Taxable_Amount      ,                      CGST_Percentage     ,                      CGST_Amount         ,                      SGST_Percentage      ,                      SGST_Amount         ,                      IGST_Percentage     ,                      IGST_Amount          ) " &
                                            "          Values                  ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @FinishedProductinvoicedate , " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With
            '***** GST END *****

            '***** GST START *****
            Dim vVouPos_IdNos As String = "", vVouPos_Amts As String = "", vVouPos_ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0
            Dim vVouPos_Narr As String = ""

            'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
            '    AcPos_ID = 1
            'Else
            AcPos_ID = led_id
            'End If

            '    Dim vTrans_Fgt As String = Format(Val(CSng(txt_Trans_Freight.Text)), "#############0.00")
            Dim vNetAmt As String = Format(Val(CSng(lbl_NetAmount.Text)), "#############0.00")

            '---GST
            vVouPos_IdNos = AcPos_ID & "|" & saleac_id & "|" & txac_id & "|" & "25|26|27|9|17|24|" & Common_Procedures.CommonLedger.TCS_PAYABLE_AC

            vVouPos_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - (Val(lbl_TaxAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(lbl_RoundOff.Text)) - Val(lbl_TcsAmount.Text) & "|" & Val(lbl_TaxAmount.Text) & "|" & Val(lbl_CGstAmount.Text) & "|" & Val(lbl_SGstAmount.Text) & "|" & Val(lbl_IGstAmount.Text) & "|" & Val(txt_Freight.Text) & "|" & Val(txt_AddLess.Text) & "|" & Val(lbl_RoundOff.Text) & "|" & Val(lbl_TcsAmount.Text)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES
                If Trim(txt_Electronic_RefNo.Text) <> "" Then
                    vVouPos_Narr = "Bill No . : " & Trim(txt_Electronic_RefNo.Text)
                Else
                    vVouPos_Narr = "Bill No . : " & Trim(lbl_InvoiceNo.Text)
                End If
            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1304" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1529" Then     '---- SANTHOSH BLUE METALS (KARANAMPETTAI)
                vVouPos_Narr = "Bill No . : " & Trim(lbl_InvoiceNo.Text)
                If Trim(txt_DcNo.Text) <> "" Then vVouPos_Narr = Trim(vVouPos_Narr) & ",  Dc No . : " & Trim(txt_DcNo.Text)
            Else
                vVouPos_Narr = "Bill No . : " & Trim(lbl_InvoiceNo.Text)
            End If

            If Common_Procedures.Voucher_Updation(con, "Gst.Sales", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, vVouPos_Narr, vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVouPos_ErrMsg)
            End If
            '***** GST END *****
            ''transport A/C Post
            vLed_IdNos = Trans_id & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVouPos_Amts = Val(txt_Trans_Freight.Text) & "|" & -1 * Val(txt_Trans_Freight.Text)
            vVouPos_Narr = "Sal : Inv No. " & Trim(lbl_InvoiceNo.Text) & " "

            If Common_Procedures.Voucher_Updation(con, "Sal.Frgt", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_InvoiceNo.Text), Convert.ToDateTime(dtp_Date.Text), vVouPos_Narr, vLed_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVouPos_ErrMsg)
            End If



            'Ac_id = 0
            'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
            '    Ac_id = 1
            'Else
            '    Ac_id = led_id
            'End If

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Head (     Voucher_Code            ,          For_OrderByCode   ,             Company_IdNo         ,           Voucher_No              ,             For_OrderBy    , Voucher_Type, Voucher_Date,           Debtor_Idno  ,          Creditor_Idno     ,                Total_VoucherAmount        ,         Narration                                , Indicate,       Year_For_Report                                     ,       Entry_Identification                  , Voucher_Receipt_Code ) " & _
            '                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ",    'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "',    1    , " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "',          ''          ) "
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details (       Voucher_Code                   ,          For_OrderByCode   ,              Company_IdNo        ,           Voucher_No              ,           For_OrderBy      , Voucher_Type, Voucher_Date, SL_No,        Ledger_IdNo     ,                       Voucher_Amount           ,              Narration                        ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                  "   Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",  'Sales',  @SalesDate ,   1  , " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            'cmd.ExecuteNonQuery()

            'Amt = Val(CSng(lbl_NetAmount.Text)) - Val(lbl_TaxAmount.Text) - Val(lbl_CGstAmount.Text) - Val(lbl_SGstAmount.Text) - Val(lbl_IGstAmount.Text) - Val(txt_Freight.Text) - Val(txt_AddLess.Text) - Val(lbl_RoundOff.Text)

            'cmd.CommandText = "Insert into Voucher_Details (      Voucher_Code                  ,          For_OrderByCode   ,             Company_IdNo         ,           Voucher_No              ,           For_OrderBy      , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo       ,     Voucher_Amount   ,     Narration                                 ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",  'Sales',  @SalesDate ,   2  , " & Str(Val(saleac_id)) & ", " & Str(Val(Amt)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            'cmd.ExecuteNonQuery()

            'If Val(lbl_TaxAmount.Text) <> 0 Then
            '    cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo     ,             Voucher_Amount          ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   3  , " & Str(Val(txac_id)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            '    cmd.ExecuteNonQuery()
            'End If

            'If Val(lbl_CGstAmount.Text) <> 0 Then
            '    L_ID = 25
            '    cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount       ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",    'Sales'  ,   @SalesDate,   4  , " & Str(Val(L_ID)) & ", " & Str(Val(lbl_CGstAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            '    cmd.ExecuteNonQuery()
            'End If

            'If Val(lbl_SGstAmount.Text) <> 0 Then
            '    L_ID = 26
            '    cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount       ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",    'Sales'  ,   @SalesDate,   5  , " & Str(Val(L_ID)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            '    cmd.ExecuteNonQuery()
            'End If

            'If Val(lbl_IGstAmount.Text) <> 0 Then
            '    L_ID = 27
            '    cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount       ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",    'Sales'  ,   @SalesDate,   6  , " & Str(Val(L_ID)) & ", " & Str(Val(lbl_IGstAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            '    cmd.ExecuteNonQuery()
            'End If


            'If Val(txt_Freight.Text) <> 0 Then
            '    L_ID = 9
            '    cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   7  , " & Str(Val(L_ID)) & ", " & Str(Val(txt_Freight.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            '    cmd.ExecuteNonQuery()
            'End If

            'If Val(txt_AddLess.Text) <> 0 Then
            '    L_ID = 17
            '    cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   8  , " & Str(Val(L_ID)) & ", " & Str(Val(txt_AddLess.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            '    cmd.ExecuteNonQuery()
            'End If

            'If Val(lbl_RoundOff.Text) <> 0 Then
            '    L_ID = 24
            '    cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount         ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   9  , " & Str(Val(L_ID)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            '    cmd.ExecuteNonQuery()
            'End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1150" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1376" Then  '---  SRIKA DESIGNS (TIRUPUR) 
                cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If



            NewCode2 = Trim((Pk_condition3)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode2), tr)
            '  Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)

            '---Bill Posting
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then

                If Val(led_id) <> 1 Then

                    '    vVouPos_IdNos = AcPos_ID & "|1"
                    '    vVouPos_Amts = Val(vNetAmt) & "|" & -1 * Val(vNetAmt)

                    '    If Common_Procedures.Voucher_Updation(con, "Cash.Rcpt", Val(lbl_Company.Tag), Trim(NewCode2), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Bill No. : " & Trim(lbl_InvoiceNo.Text) & " - Cash Receipt", vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                    '        Throw New ApplicationException(vVouPos_ErrMsg)
                    '    End If

                    'Else

                    ''---GST
                    'vVouPos_IdNos = AcPos_ID & "|" & saleac_id & "|" & txac_id & "|" & "25|26|27|9|17|24|" & Common_Procedures.CommonLedger.TCS_PAYABLE_AC

                    'vVouPos_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - (Val(lbl_TaxAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(lbl_RoundOff.Text)) - Val(lbl_TcsAmount.Text) & "|" & Val(lbl_TaxAmount.Text) & "|" & Val(lbl_CGstAmount.Text) & "|" & Val(lbl_SGstAmount.Text) & "|" & Val(lbl_IGstAmount.Text) & "|" & Val(txt_Freight.Text) & "|" & Val(txt_AddLess.Text) & "|" & Val(lbl_RoundOff.Text) & "|" & Val(lbl_TcsAmount.Text)


                    'VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, led_id, Trim(lbl_InvoiceNo.Text), 0, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
                    'If Trim(UCase(VouBil)) = "ERROR" Then
                    '    Throw New ApplicationException("Error on Voucher Bill Posting")
                    'End If


                    vVouPos_IdNos = AcPos_ID & "|1"
                    vVouPos_Amts = Val(vNetAmt) & "|" & -1 * Val(vNetAmt)

                    If Common_Procedures.Voucher_Updation(con, "Cash.Rcpt", Val(lbl_Company.Tag), Trim(NewCode2), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Bill No. : " & Trim(lbl_InvoiceNo.Text) & " - Cash Receipt", vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                        Throw New ApplicationException(vVouPos_ErrMsg)
                    End If

                End If

            Else

                VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, led_id, Trim(lbl_InvoiceNo.Text), 0, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
                If Trim(UCase(VouBil)) = "ERROR" Then
                    Throw New ApplicationException("Error on Voucher Bill Posting")
                End If

            End If


            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            move_record(lbl_InvoiceNo.Text)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean
        Dim itm_id As Integer
        Dim unt_id As Integer

        If Trim(cbo_ItemName.Text) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        itm_id = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_ItemName.Text)
        If itm_id = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If


        If Trim(cbo_Unit.Text) = "" Then
            MessageBox.Show("Invalid Unit", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        unt_id = Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text)
        If unt_id = 0 Then
            MessageBox.Show("Invalid Unit", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        If Val(txt_NoofItems.Text) = 0 Then
            MessageBox.Show("Invalid No.of Items", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_NoofItems.Enabled Then txt_NoofItems.Focus()
            Exit Sub
        End If

        If Val(txt_Rate.Text) = 0 Then
            MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Rate.Enabled Then txt_Rate.Focus()
            Exit Sub
        End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1

                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SL_NO).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(DgvCol_Details.ITEM_NAME).Value = cbo_ItemName.Text
                    .Rows(i).Cells(DgvCol_Details.UNIT).Value = cbo_Unit.Text
                    .Rows(i).Cells(DgvCol_Details.SIZE).Value = cbo_Size.Text
                    .Rows(i).Cells(DgvCol_Details.COLOUR).Value = Cbo_Colour.Text
                    .Rows(i).Cells(DgvCol_Details.DESCRIBTION).Value = txt_SerialNo.Text
                    .Rows(i).Cells(DgvCol_Details.PACKAGE_NO).Value = txt_Package_No.Text
                    .Rows(i).Cells(DgvCol_Details.GROSS_WEIGHT).Value = Format(Val(txt_Gross_Wgt.Text), "########0.000")
                    .Rows(i).Cells(DgvCol_Details.NET_WEIGHT).Value = Format(Val(txt_Net_wgt.Text), "########0.000")

                    .Rows(i).Cells(DgvCol_Details.QUANTITY).Value = Val(txt_NoofItems.Text)



                    .Rows(i).Cells(DgvCol_Details.RATE_QTY).Value = Format(Val(txt_Rate.Text), "########0.00")


                    .Rows(i).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(lbl_Amount.Text), "########0.00")

                    '***** GST START *****
                    .Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")

                    .Rows(i).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")

                    .Rows(i).Cells(DgvCol_Details.HSN_CODE).Value = lbl_Grid_HsnCode.Text
                    .Rows(i).Cells(DgvCol_Details.GST_PERC).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")

                    .Rows(i).Cells(DgvCol_Details.LOT_NO).Value = Trim(Cbo_Lot_No.Text)

                    .Rows(i).Cells(DgvCol_Details.GST_RATE).Value = Format(Val(txt_GSTRate.Text), "########0.00")

                    .Rows(i).Cells(DgvCol_Details.ORDER_NO).Value = Val(txt_Order_No.Text)

                    .Rows(i).Cells(DgvCol_Details.NO_OF_PACKS).Value = Val(txt_No_of_Packs.Text)

                    .Rows(i).Cells(DgvCol_Details.NO_OF_PCS_PER_PACKS).Value = Val(txt_No_Of_Pcs_Per_Packs.Text)



                    '***** GST END *****
                    '.Rows(i).Selected = True

                    MtchSTS = True

                    'If i >= 10 Then .FirstDisplayedScrollingRowIndex = i - 9

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SL_NO).Value = txt_SlNo.Text
                .Rows(n).Cells(DgvCol_Details.ITEM_NAME).Value = cbo_ItemName.Text
                .Rows(n).Cells(DgvCol_Details.UNIT).Value = cbo_Unit.Text
                .Rows(n).Cells(DgvCol_Details.DESCRIBTION).Value = txt_SerialNo.Text
                .Rows(n).Cells(DgvCol_Details.PACKAGE_NO).Value = txt_Package_No.Text
                .Rows(n).Cells(DgvCol_Details.SIZE).Value = cbo_Size.Text
                .Rows(n).Cells(DgvCol_Details.COLOUR).Value = Cbo_Colour.Text
                .Rows(n).Cells(DgvCol_Details.GROSS_WEIGHT).Value = Format(Val(txt_Gross_Wgt.Text), "########0.000")
                .Rows(n).Cells(DgvCol_Details.NET_WEIGHT).Value = Format(Val(txt_Net_wgt.Text), "########0.000")

                .Rows(n).Cells(DgvCol_Details.QUANTITY).Value = Val(txt_NoofItems.Text)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then 'AADHARSH
                    .Rows(n).Cells(DgvCol_Details.RATE_QTY).Value = Val(txt_Rate.Text)
                Else
                    .Rows(n).Cells(DgvCol_Details.RATE_QTY).Value = Format(Val(txt_Rate.Text), "########0.00")
                End If

                .Rows(n).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(lbl_Amount.Text), "########0.00")
                '***** GST START *****
                .Rows(n).Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")

                .Rows(n).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")

                .Rows(n).Cells(DgvCol_Details.HSN_CODE).Value = lbl_Grid_HsnCode.Text
                .Rows(n).Cells(DgvCol_Details.GST_PERC).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")

                .Rows(n).Cells(DgvCol_Details.LOT_NO).Value = Trim(Cbo_Lot_No.Text)

                .Rows(n).Cells(DgvCol_Details.GST_RATE).Value = Trim(txt_GSTRate.Text)

                .Rows(n).Cells(DgvCol_Details.ORDER_NO).Value = Val(txt_Order_No.Text)

                .Rows(n).Cells(DgvCol_Details.NO_OF_PACKS).Value = Val(txt_No_of_Packs.Text)

                .Rows(n).Cells(DgvCol_Details.NO_OF_PCS_PER_PACKS).Value = Val(txt_No_Of_Pcs_Per_Packs.Text)

                '***** GST END *****
                '.Rows(n).Selected = True

                'If n >= 10 Then .FirstDisplayedScrollingRowIndex = n - 9

            End If

        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_SerialNo.Text = ""

        txt_Package_No.Text = ""
        txt_Net_wgt.Text = ""
        txt_Gross_Wgt.Text = ""

        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_GSTRate.Text = ""
        lbl_Amount.Text = ""
        Cbo_Lot_No.Text = ""
        cbo_Size.Text = ""
        Cbo_Colour.Text = ""
        txt_No_Of_Pcs_Per_Packs.Text = ""
        txt_No_of_Packs.Text = ""
        txt_Order_No.Text = ""


        '***** GST START *****
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""


        '***** GST END *****

        Grid_Cell_DeSelect()

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub txt_NoofItems_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofItems.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            txt_Rate.Focus()
        End If

    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoofItems.TextChanged
        '***** GST START *****
        Call Amount_Calculation(False)
        '***** GST END *****
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        '***** GST START *****
        Call Amount_Calculation(False)
        '***** GST END *****
    End Sub
    Private Sub txt_GSTRate_TextChanged(sender As Object, e As EventArgs) Handles txt_GSTRate.TextChanged
        Call Amount_Calculation(False)
    End Sub
    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, txt_SlNo, cbo_Unit, "item_head", "item_Name", "", "(item_idno = 0)")
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, Nothing, Nothing, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_IdNo = 0)")
            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                txt_SlNo.Focus()

            ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If Trim(Common_Procedures.settings.CustomerCode) = "1107" Then
                    pnl_Bill_Rate.Visible = True
                    Bill_RateDetails()
                End If


                cbo_Unit.Focus()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Mtr_Qty As String
        Dim Unt_nm As String
        Dim Rate As String
        Dim Itm_idno As Integer = 0


        Try
            'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "item_head", "item_Name", "", "(item_idno = 0)")
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        If Asc(e.KeyChar) = 13 Then


            Show_Item_CurrentStock()

            With dgv_Details





                Itm_idno = Common_Procedures.Processed_Item_NameToIdNo(con, Trim(cbo_ItemName.Text))

                da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from Processed_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno Where a.Processed_Item_IdNo = " & Str(Val(Itm_idno)), con)
                dt = New DataTable
                da.Fill(dt)

                Rate = 0
                Mtr_Qty = 0
                Unt_nm = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        Mtr_Qty = Val(dt.Rows(0).Item("Meter_Qty").ToString)
                        Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)
                        Rate = Val(dt.Rows(0).Item("Sales_Rate").ToString)
                    End If
                End If

                dt.Dispose()
                da.Dispose()

                cbo_Unit.Text = Trim(Unt_nm)
                txt_Rate.Text = Format(Val(Rate), "#########0.00")
                txt_NoofItems.Text = Format(Val(Mtr_Qty), "#########0.00")



            End With

            '***** GST START *****  

            If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
                get_Item_Unit_Rate_TaxPerc()
            End If
            '***** GST END *****    
            If Trim(cbo_ItemName.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_CashDiscPerc.Focus()
            End If

            '-----------------
            If Trim(Common_Procedures.settings.CustomerCode) = "1107" Then
                pnl_Bill_Rate.Visible = True
                Bill_RateDetails()
            End If

            '--------------------

        End If

    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New FinishedProduct_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        With cbo_ItemName
            'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "item_head", "item_Name", "", "(item_idno = 0)")
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_IdNo = 0)")
            cbo_ItemName.Tag = cbo_ItemName.Text
        End With

        Show_Item_CurrentStock()
    End Sub

    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus
        '***** GST START *****
        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            get_Item_Unit_Rate_TaxPerc()

        End If
        '***** GST END *****
    End Sub

    '***** GST START *****
    Private Sub get_Item_Unit_Rate_TaxPerc()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Itm_id As Integer


        Itm_id = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_ItemName.Text)

        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            cbo_ItemName.Tag = cbo_ItemName.Text
            da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from Processed_Item_Head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.Processed_Item_IdNo = " & Val(Itm_id) & "", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)("unit_name").ToString) = False Then
                    cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                End If
                If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
                    txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
                End If

                get_Item_Tax(False)
            End If
            dt.Dispose()
            da.Dispose()
        End If

    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
    End Sub
    '***** GST END *****

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        'If e.KeyCode = 40 Then e.Handled = True : e.SuppressKeyPress = True : msk_Date.Focus()
        'If e.KeyCode = 38 Then e.Handled = True : e.SuppressKeyPress = True : txt_AddLess.Focus() ' SendKeys.Send("+{TAB}")

        If e.KeyCode = 40 Then

            'If cbo_EntType.Visible Then
            '    cbo_EntType.Focus()
            'Else
            '    cbo_Ledger.Focus()
            'End If

        End If
    End Sub

    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, cbo_Ledger, Nothing, "", "", "", "")

            If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                Check_Combo_Cash_Party_Name()

                If txt_cash_Party_name.Visible = True Then
                    txt_cash_Party_name.Focus()
                Else
                    txt_Electronic_RefNo.Focus()
                End If

            End If



        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, Nothing, "", "", "", "")

            If Asc(e.KeyChar) = 13 Then

                Check_Combo_Cash_Party_Name()

                If txt_cash_Party_name.Visible = True Then
                    txt_cash_Party_name.Focus()
                Else
                    txt_Electronic_RefNo.Focus()
                End If


            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportMode, txt_OrderDate, txt_Place_Of_Supply, "", "", "", "")

            'If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            '    If txt_PoNo.Visible Then
            '        txt_PoNo.Focus()

            '    Else
            '        txt_DcDate.Focus()
            '    End If

            'End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportMode.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportMode, txt_Place_Of_Supply, "", "", "", "", False)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

        'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
        '    cbo_Ledger.DropDownStyle = ComboBoxStyle.Simple
        '    cbo_Ledger.DataSource = Nothing
        'Else
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        End If
        cbo_Ledger.DropDownStyle = ComboBoxStyle.DropDown
        ' End If

        'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
        '    cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        'End If

        '***** GST START *****
        cbo_Ledger.Tag = cbo_Ledger.Text
        '***** GST END *****
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then

            '    If e.KeyValue = 38 Or (e.Control = True And e.KeyValue = 38) Then
            '        msk_Date.Focus()
            '    ElseIf e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            '        cbo_PaymentMethod.Focus()
            '    End If
            'Else

            'If Common_Procedures.settings.CustomerCode = "1186" Then
            '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
            'ElseIf Common_Procedures.settings.CustomerCode = "1545" Or Common_Procedures.settings.CustomerCode = "1556" Then
            '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
            'Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
            'End If

            ' End If

            'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_PaymentMethod, IIf(cbo_OrderNo.Visible = True, cbo_OrderNo, txt_OrderNo), "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try
            '  If Trim(UCase(cbo_PaymentMethod.Text)) <> "CASH" Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1366" Then  '--- SOWMIYA TRADERS (TIRUPUR)
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
            ElseIf Common_Procedures.settings.CustomerCode = "1186" Then
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
            End If

            '   End If


            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                    cbo_Ledger.Tag = cbo_Ledger.Text
                    Amount_Calculation(True)
                    Get_TCS_Sts_From_Ledger_Name()

                End If
                'If Trim(UCase(cbo_EntType.Text)) = "PROFORMA" Or Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then

                '    If MessageBox.Show("Do you want to select " & Trim(LCase(cbo_EntType.Text)) & " ?", "FOR " & Trim(UCase(cbo_EntType.Text)) & " SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                '        btn_Selection_Click(sender, e)
                '        'lbl_caption_proformano.Visible = True
                '        'txt_proformaNo.Visible = True


                '    Else
                '        cbo_PaymentMethod.Focus()

                '    End If
                'End If

                get_Ledger_TotalSales()
                get_Ledger_CurrentBalance()
                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1366" Then
                '    cbo_ItemName.Focus()
                'Else
                txt_OrderNo.Focus()
                'End If
            End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus

        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            Amount_Calculation(True)
            get_Ledger_TotalSales()
            get_Ledger_CurrentBalance()
        End If

        'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
        '    cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        'End If

    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown

        If e.KeyValue = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            '  txt_CashDiscPerc.Focus()
            txt_cashDisc_amt.Focus()
        End If

        If e.KeyValue = 40 Then
            e.Handled = True : e.SuppressKeyPress = True

            If txt_VehicleNo.Visible = True Then
                txt_VehicleNo.Focus()

            ElseIf txt_PaymentTerms.Visible = True Then
                txt_PaymentTerms.Focus()

            ElseIf txt_Due_Days.Visible = True Then
                txt_Due_Days.Focus()

            Else
                If MessageBox.Show("Do you want to Save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If

            End If
        End If


    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If txt_VehicleNo.Visible = True And txt_VehicleNo.Enabled Then
                txt_VehicleNo.Focus()

            ElseIf txt_PaymentTerms.Visible = True Then
                txt_PaymentTerms.Focus()

            ElseIf txt_Due_Days.Visible = True Then
                txt_Due_Days.Focus()

            Else
                If MessageBox.Show("Do you want to Save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If

            End If

        End If

    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        '***** GST START *****
        Amount_Calculation(True)
        ' NetAmount_Calculation()
        '***** GST END *****
    End Sub

    Private Sub txt_CashDiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscPerc.KeyDown
        If e.KeyCode = 40 Then
            txt_cashDisc_amt.Focus()
        End If
        ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_ItemName.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CashDiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_cashDisc_amt.Focus()
        End If

    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        Amount_Calculation(True)

        NetAmount_Calculation()

        If Str(Val(txt_CashDiscPerc.Text)) <> 0 Then
            txt_cashDisc_amt.ReadOnly = True
        Else
            txt_cashDisc_amt.ReadOnly = False
        End If


        If Val(txt_CashDiscPerc.Text) = 0 Then
            txt_cashDisc_amt.Text = ""

        End If
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_VatPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.TextChanged
        Gross_Discount_Tax_Amount_Calculation()
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Gross_Discount_Tax_Amount_Calculation()
    End Sub

    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyValue = 38 Then
            If cbo_DispatcherName.Enabled = True And cbo_DispatcherName.Visible = True Then
                'cbo_DispatcherName.Focus()

                msk_Lr_Date.Focus()

            ElseIf txt_ExchangeRate.Enabled = True And cbo_Currency.Visible = True Then
                txt_ExchangeRate.Focus()
            Else
                'cbo_TaxType.Focus()
            End If
        End If

        If e.KeyValue = 40 Then cbo_ItemName.Focus()
    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SL_NO).Value) = Val(txt_SlNo.Text) Then

                        cbo_ItemName.Text = Trim(.Rows(i).Cells(DgvCol_Details.ITEM_NAME).Value)
                        cbo_Unit.Text = Trim(.Rows(i).Cells(DgvCol_Details.UNIT).Value)
                        txt_SerialNo.Text = Trim(.Rows(i).Cells(DgvCol_Details.DESCRIBTION).Value)
                        txt_Package_No.Text = Trim(.Rows(i).Cells(DgvCol_Details.PACKAGE_NO).Value)

                        txt_Gross_Wgt.Text = Format(Val(.Rows(i).Cells(DgvCol_Details.GROSS_WEIGHT).Value), "#########0.000")
                        txt_Net_wgt.Text = Format(Val(.Rows(i).Cells(DgvCol_Details.NET_WEIGHT).Value), "#########0.000")

                        txt_NoofItems.Text = Val(.Rows(i).Cells(DgvCol_Details.QUANTITY).Value)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" Then 'AADHARSH
                            txt_Rate.Text = Val(.Rows(i).Cells(DgvCol_Details.RATE_QTY).Value)
                        Else
                            txt_Rate.Text = Format(Val(.Rows(i).Cells(DgvCol_Details.RATE_QTY).Value), "########0.00")
                        End If

                        lbl_Amount.Text = Format(Val(.Rows(i).Cells(DgvCol_Details.AMOUNT).Value), "########0.00")

                        '***** GST START *****
                        lbl_Grid_DiscPerc.Text = Format(Val(.Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value), "########0.00")
                        lbl_Grid_DiscAmount.Text = Format(Val(.Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value), "########0.00")

                        lbl_Grid_AssessableValue.Text = Format(Val(.Rows(i).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value), "########0.00")

                        lbl_Grid_HsnCode.Text = .Rows(i).Cells(DgvCol_Details.HSN_CODE).Value
                        lbl_Grid_GstPerc.Text = Format(Val(.Rows(i).Cells(DgvCol_Details.GST_PERC).Value), "########0.00")


                        '***** GST END *****
                        GST_Rate_Calculation()

                        Exit For

                    End If

                Next

            End With

            If Val(txt_SlNo.Text) = 0 Then
                txt_SlNo.Text = dgv_Details.Rows.Count + 1
                txt_CashDiscPerc.Focus()
            Else
                cbo_ItemName.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, cbo_ItemName, cbo_Size, "unit_head", "unit_Name", "", "(unit_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, cbo_Size, "unit_head", "unit_Name", "", "(unit_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub txt_SerialNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SerialNo.KeyDown
        If e.KeyCode = 40 Then

            If Cbo_Lot_No.Visible = True Then
                Cbo_Lot_No.Focus()
            Else
                btn_Add.Focus() ' SendKeys.Send("{TAB}")
            End If

        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_SerialNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SerialNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Cbo_Lot_No.Visible = True Then
                Cbo_Lot_No.Focus()
            Else
                btn_Add_Click(sender, e)
            End If

            'SendKeys.Send("{TAB}")
        End If
    End Sub

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
                Condt = " a.Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Code IN (select z.Sales_Code from Sales_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.Sales_No, a.Sales_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Sales_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code LIKE '" & Pk_Condition & "%' and a.Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
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

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, Nothing, "item_head", "item_Name", "", "(item_idno = 0)")
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
        'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
        '    cbo_Ledger.DropDownStyle = ComboBoxStyle.Simple
        'Else
        '    cbo_Ledger.DropDownStyle = ComboBoxStyle.DropDown
        'End If
        'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
        '    cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        'End If
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


        With dgv_Details

            If Trim(.CurrentRow.Cells(DgvCol_Details.ITEM_NAME).Value) <> "" Then
                If Val(.CurrentRow.Cells(DgvCol_Details.ITEM_SET_IDNO).Value) <> 0 Then
                    vDgv_Double_Click_STS = True
                    cbo_ItemName.Text = ""
                    Check_Item_In_Item_Set_Details()
                    If vItem_Set_Details_STS = True Then
                        Move_Data_From_DgvGrid_to_Input_Controls(.CurrentRow.Index)
                    Else
                        GoTo LOOP1
                    End If

                Else
LOOP1:

                    txt_SlNo.Text = Val(.CurrentRow.Cells(DgvCol_Details.SL_NO).Value)
                    cbo_ItemName.Text = Trim(.CurrentRow.Cells(DgvCol_Details.ITEM_NAME).Value)
                    cbo_Unit.Text = Trim(.CurrentRow.Cells(DgvCol_Details.UNIT).Value)
                    txt_SerialNo.Text = Trim(.CurrentRow.Cells(DgvCol_Details.DESCRIBTION).Value)
                    txt_Package_No.Text = Trim(.CurrentRow.Cells(DgvCol_Details.PACKAGE_NO).Value)

                    txt_Gross_Wgt.Text = Format(Val(.CurrentRow.Cells(DgvCol_Details.GROSS_WEIGHT).Value), "#########0.000")
                    txt_Net_wgt.Text = Format(Val(.CurrentRow.Cells(DgvCol_Details.NET_WEIGHT).Value), "#########0.000")

                    txt_NoofItems.Text = Val(.CurrentRow.Cells(DgvCol_Details.QUANTITY).Value)

                    txt_Rate.Text = Format(Val(.CurrentRow.Cells(DgvCol_Details.RATE_QTY).Value), "########0.00")


                    lbl_Amount.Text = Format(Val(.CurrentRow.Cells(DgvCol_Details.AMOUNT).Value), "########0.00")

                    '***** GST START *****
                    lbl_Grid_DiscPerc.Text = Format(Val(.CurrentRow.Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value), "########0.00")
                    lbl_Grid_DiscAmount.Text = Format(Val(.CurrentRow.Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value), "########0.00")

                    lbl_Grid_AssessableValue.Text = Format(Val(.CurrentRow.Cells(DgvCol_Details.ASSESSABLE_VALUE).Value), "########0.00")

                    lbl_Grid_HsnCode.Text = .CurrentRow.Cells(DgvCol_Details.HSN_CODE).Value

                    lbl_Grid_GstPerc.Text = Format(Val(.CurrentRow.Cells(DgvCol_Details.GST_PERC).Value), "########0.00")

                    Cbo_Lot_No.Text = .CurrentRow.Cells(DgvCol_Details.LOT_NO).Value
                    cbo_Size.Text = .CurrentRow.Cells(DgvCol_Details.SIZE).Value
                    Cbo_Colour.Text = .CurrentRow.Cells(DgvCol_Details.COLOUR).Value

                    txt_Order_No.Text = .CurrentRow.Cells(DgvCol_Details.ORDER_NO).Value
                    txt_No_of_Packs.Text = .CurrentRow.Cells(DgvCol_Details.NO_OF_PACKS).Value
                    txt_No_Of_Pcs_Per_Packs.Text = .CurrentRow.Cells(DgvCol_Details.NO_OF_PCS_PER_PACKS).Value

                    '***** GST END *****

                    GST_Rate_Calculation()

                    If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

                End If
            End If

        End With
        vDgv_Double_Click_STS = False
    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        MtchSTS = False

        With dgv_Details


            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SL_NO).Value) = Val(txt_SlNo.Text) Then

                    .Rows.RemoveAt(i)

                    MtchSTS = True

                    Exit For

                End If

            Next

            If MtchSTS = True Then
                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(DgvCol_Details.SL_NO).Value = i + 1
                Next
            End If


        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_SerialNo.Text = ""

        txt_Package_No.Text = ""
        txt_Net_wgt.Text = ""
        txt_Gross_Wgt.Text = ""

        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_GSTRate.Text = ""
        lbl_Amount.Text = ""
        '***** GST START *****
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_GstPerc.Text = ""
        '***** GST END *****
        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        On Error Resume Next
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index
                .Rows.RemoveAt(n)

                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(DgvCol_Details.SL_NO).Value = i + 1
                Next

            End With

            TotalAmount_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            cbo_ItemName.Text = ""
            cbo_Unit.Text = ""
            txt_SerialNo.Text = ""

            txt_Package_No.Text = ""
            txt_Net_wgt.Text = ""
            txt_Gross_Wgt.Text = ""

            txt_NoofItems.Text = ""
            txt_Rate.Text = ""
            lbl_Amount.Text = ""
            '***** GST START *****
            lbl_Grid_DiscPerc.Text = ""
            lbl_Grid_DiscAmount.Text = ""
            lbl_Grid_AssessableValue.Text = ""
            lbl_Grid_GstPerc.Text = ""
            lbl_Grid_HsnCode.Text = ""
            lbl_Grid_GstPerc.Text = ""

            '***** GST END *****

            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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

    Private Sub txt_PaymentTerms_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PaymentTerms.KeyDown
        If e.KeyCode = 40 Then btn_save.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_PaymentTerms_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PaymentTerms.KeyPress
        If Asc(e.KeyChar) = 13 Then
            '    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '        save_record()
            '    Else
            '        dtp_Date.Focus()
            '    End If
            txt_Due_Days.Focus()
        End If

    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_OrderDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OrderDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            cbo_TransportMode.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_OrderNo.Focus()
        End If

    End Sub

    Private Sub txt_OrderDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OrderDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_OrderDate.Text = Date.Today
            txt_OrderDate.SelectAll()
        End If
    End Sub

    Private Sub txt_DcDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DcDate.KeyDown
        If e.KeyValue = 38 Then txt_DcNo.Focus()
        If e.KeyValue = 40 Then cbo_TransportMode.Focus()
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_DcDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DcDate.KeyPress
        If Asc(e.KeyChar) = 13 Then cbo_TransportMode.Focus()
    End Sub

    Private Sub txt_DcDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DcDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_DcDate.Text = Date.Today
            txt_DcDate.SelectAll()
        End If
    End Sub

    Private Sub lbl_TaxAmount_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_TaxAmount.DoubleClick
        Dim VtAmt As String = ""

        VtAmt = InputBox("Enter vat Amount :", "FOR VAT AMOUNT ALTERATION....", Val(lbl_TaxAmount.Text))

        If Trim(VtAmt) <> "" Then
            If Val(VtAmt) <> 0 Then
                lbl_TaxAmount.Text = Format(Val(VtAmt), "#########0.00")
                NetAmount_Calculation()
            End If
        End If

        If txt_TaxPerc.Visible And txt_TaxPerc.Enabled Then txt_TaxPerc.Focus()

    End Sub

    Private Sub cbo_OrderNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OrderNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sales_Order_Head", "Sales_Order_Selection_Code", "", "")

    End Sub

    Private Sub cbo_OrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OrderNo.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OrderNo, txt_Electronic_RefNo, txt_OrderDate, "Sales_Order_Head", "Sales_Order_Selection_Code", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_OrderNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OrderNo.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OrderNo, txt_OrderDate, "Sales_Order_Head", "Sales_Order_Selection_Code", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    '***** GST START *****
    Private Sub get_Item_Tax(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0
        Dim gstrate As Single = 0

        Try

            If FrmLdSTS = True Then Exit Sub

            lbl_Grid_GstPerc.Text = ""
            lbl_Grid_HsnCode.Text = ""

            If chk_GSTTax_Invocie.Checked = True Then

                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

                ItmIdNo = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_ItemName.Text)

                lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")

                'da = New SqlClient.SqlDataAdapter("Select b.*,a.Sales_Rate,a.HSN_Code,a.Gst_Percentage  from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                da = New SqlClient.SqlDataAdapter("Select b.*,a.Sales_Rate from Processed_Item_Head a INNER JOIN ItemGroup_Head b ON a.Processed_ItemGroup_IdNo = b.itemgroup_idno Where a.Processed_Item_IdNo = " & Str(Val(ItmIdNo)), con)

                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then

                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                        lbl_Grid_HsnCode.Text = dt.Rows(0)("Item_HSN_Code").ToString
                    End If
                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                        lbl_Grid_GstPerc.Text = dt.Rows(0)("Item_GST_Percentage").ToString
                    End If

                    gstrate = 0
                    If Val(lbl_Grid_GstPerc.Text) <> 0 Then
                        gstrate = dt.Rows(0)("Sales_Rate").ToString * (lbl_Grid_GstPerc.Text / 100)
                    End If
                    txt_GSTRate.Text = Format(Val(dt.Rows(0)("Sales_Rate").ToString + gstrate), "#######0.00")

                End If

                If Val(txt_GSTRate.Text) <> 0 And Val(txt_Rate.Text) = 0 Then
                    Rate_Calculation_from_GSTRATE()
                Else
                    GST_Rate_Calculation()
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
    '***** GST END *****

    'Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Show_Item_CurrentStock()
    '    cbo_TaxType.Tag = cbo_TaxType.Text

    'End Sub

    'Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_Place_Of_Supply, Nothing, "", "", "", "")
    '    If e.KeyValue = 40 Then
    '        If cbo_DispatcherName.Enabled = True And cbo_DispatcherName.Visible = True Then

    '            cbo_DispatcherName.Focus()
    '        ElseIf cbo_Currency.Enabled = True And cbo_Currency.Visible = True Then
    '            cbo_Currency.Focus()

    '        ElseIf Trim(cbo_EntType.Text) = "DELIVERY" And dgv_Details.RowCount > 0 Then
    '            dgv_Details.Focus()
    '            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.QUANTITY)
    '            dgv_Details.CurrentCell.Selected = True

    '        Else
    '            cbo_ItemName.Focus()
    '        End If
    '    End If

    'End Sub

    'Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Try
    '        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, Nothing, "", "", "", "", True)
    '        If Asc(e.KeyChar) = 13 Then
    '            If cbo_DispatcherName.Enabled = True And cbo_DispatcherName.Visible = True Then

    '                cbo_DispatcherName.Focus()
    '            ElseIf cbo_Currency.Enabled = True And cbo_Currency.Visible = True Then
    '                cbo_Currency.Focus()
    '            ElseIf Trim(cbo_EntType.Text) = "DELIVERY" And dgv_Details.RowCount > 0 Then
    '                dgv_Details.Focus()
    '                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.QUANTITY)
    '                dgv_Details.CurrentCell.Selected = True

    '            Else
    '                cbo_ItemName.Focus()
    '            End If

    '            If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
    '                cbo_TaxType.Tag = cbo_TaxType.Text
    '                Amount_Calculation(True)
    '            End If
    '        End If

    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    '***** GST START *****
    'Private Sub cbo_TaxType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try

    '        If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
    '            cbo_TaxType.Tag = cbo_TaxType.Text
    '            Amount_Calculation(True)
    '        End If

    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try
    'End Sub

    'Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Amount_Calculation(True)
    '    cbo_TaxType.Tag = cbo_TaxType.Text
    'End Sub
    '***** GST END *****

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_Pdf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Pdf.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        Print_PDF_Status = False
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub Amount_Calculation(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0



        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub



        If GridAll_Row_STS = True Then

            With dgv_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(DgvCol_Details.ITEM_NAME).Value) <> "" Then

                        ItmIdNo = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(DgvCol_Details.ITEM_NAME).Value)
                        If ItmIdNo <> 0 Then

                            .Rows(i).Cells(DgvCol_Details.HSN_CODE).Value = ""
                            .Rows(i).Cells(DgvCol_Details.GST_PERC).Value = ""

                            If chk_GSTTax_Invocie.Checked = True Then
                                'If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                                'da = New SqlClient.SqlDataAdapter("Select b.*,a.HSN_Code,a.Gst_Percentage from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)

                                da = New SqlClient.SqlDataAdapter("Select b.* from Processed_Item_Head a INNER JOIN ItemGroup_Head b ON a.Processed_ItemGroup_IdNo = b.itemgroup_idno Where a.Processed_Item_IdNo = " & Str(Val(ItmIdNo)), con)
                                dt = New DataTable
                                da.Fill(dt)

                                If dt.Rows.Count > 0 Then

                                    'If Common_Procedures.settings.Item_Creation_Wise_Get_HSNCode_GST_Perc_STS = 1 Then

                                    '    If IsDBNull(dt.Rows(0)("HSN_Code").ToString) = False Then
                                    '        .Rows(i).Cells(DgvCol_Details.HSN_CODE).Value = dt.Rows(0)("HSN_Code").ToString
                                    '    End If
                                    '    If IsDBNull(dt.Rows(0)("Gst_Percentage").ToString) = False Then
                                    '        .Rows(i).Cells(DgvCol_Details.GST_PERC).Value = dt.Rows(0)("Gst_Percentage").ToString
                                    '    End If

                                    'Else
                                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                                        .Rows(i).Cells(DgvCol_Details.HSN_CODE).Value = dt.Rows(0)("Item_HSN_Code").ToString
                                    End If
                                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                                        .Rows(i).Cells(DgvCol_Details.GST_PERC).Value = Format(Val(dt.Rows(0)("Item_GST_Percentage").ToString), "#########0.00")
                                    End If
                                    'End If

                                End If
                                dt.Clear()

                            End If


                            If txt_ExchangeRate.Enabled = True And txt_ExchangeRate.Visible = True And Val(txt_ExchangeRate.Text) <> 0 Then
                                .Rows(i).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(.Rows(i).Cells(DgvCol_Details.QUANTITY).Value) * Val(.Rows(i).Cells(DgvCol_Details.RATE_QTY).Value) * Val(txt_ExchangeRate.Text), "#########0.00")
                            Else
                                .Rows(i).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(.Rows(i).Cells(DgvCol_Details.QUANTITY).Value) * Val(.Rows(i).Cells(DgvCol_Details.RATE_QTY).Value), "#########0.00")
                            End If

                            .Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
                            .Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value = Format(Val(.Rows(i).Cells(DgvCol_Details.AMOUNT).Value) * Val(.Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value) / 100, "#########0.00")
                            .Rows(i).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value = Format(Val(.Rows(i).Cells(DgvCol_Details.AMOUNT).Value) - Val(.Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value), "#########0.00")

                        End If

                    End If

                Next

            End With

            TotalAmount_Calculation()

        Else

            '  lbl_Amount.Text = Format(Val(txt_NoofItems.Text) * Val(txt_Rate.Text), "#########0.00")

            'If Val(txt_ExchangeRate.Text) <> 0 Then
            '    lbl_Amount.Text = Val(txt_NoofItems.Text) * Val(txt_Rate.Text) * Val(txt_ExchangeRate.Text)
            'Else
            lbl_Amount.Text = Val(txt_NoofItems.Text) * Val(txt_Rate.Text)
            'End If

            lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
            lbl_Grid_DiscAmount.Text = Format(Val(lbl_Amount.Text) * Val(lbl_Grid_DiscPerc.Text) / 100, "#########0.00")
            lbl_Grid_AssessableValue.Text = Format(Val(lbl_Amount.Text) - Val(lbl_Grid_DiscAmount.Text), "#########0.00")

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

        Dim vGrss_wgt = ""
        Dim vNet_wgt = ""
        Dim vTot_No_Of_Packs As Decimal = 0

        Dim vTotHsn As Integer = 0


        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        Sno = 0
        TotQty = 0
        TotGrsAmt = 0
        TotDiscAmt = 0
        TotAssval = 0
        vGrss_wgt = 0
        vNet_wgt = 0
        vTot_No_Of_Packs = 0

        For i = 0 To dgv_Details.RowCount - 1

            Sno = Sno + 1

            dgv_Details.Rows(i).Cells(DgvCol_Details.SL_NO).Value = Sno

            If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.QUANTITY).Value) <> 0 Then

                vGrss_wgt = vGrss_wgt + Val(dgv_Details.Rows(i).Cells(DgvCol_Details.GROSS_WEIGHT).Value)
                vNet_wgt = vNet_wgt + Val(dgv_Details.Rows(i).Cells(DgvCol_Details.NET_WEIGHT).Value)

                TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(DgvCol_Details.QUANTITY).Value)
                TotGrsAmt = TotGrsAmt + Val(dgv_Details.Rows(i).Cells(DgvCol_Details.AMOUNT).Value)

                '***** GST START *****
                TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value)
                TotAssval = TotAssval + Val(dgv_Details.Rows(i).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value)
                '***** GST END *****

                vTot_No_Of_Packs = vTot_No_Of_Packs + Val(dgv_Details.Rows(i).Cells(DgvCol_Details.NO_OF_PACKS).Value)

                vTotHsn = vTotHsn + 1

            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(DgvCol_Details.GROSS_WEIGHT).Value = Format(Val(vGrss_wgt), "########0.000")
            .Rows(0).Cells(DgvCol_Details.NET_WEIGHT).Value = Format(Val(vNet_wgt), "########0.000")

            .Rows(0).Cells(DgvCol_Details.QUANTITY).Value = Val(TotQty)
            .Rows(0).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(TotGrsAmt), "########0.00")
            '***** GST START *****
            .Rows(0).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value = Format(Val(TotDiscAmt), "########0.00")
            .Rows(0).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value = Format(Val(TotAssval), "########0.00")
            .Rows(0).Cells(DgvCol_Details.NO_OF_PACKS).Value = Format(Val(vTot_No_Of_Packs), "########0.00")
            .Rows(0).Cells(DgvCol_Details.HSN_CODE).Value = Val(vTotHsn)

            '***** GST END *****
        End With

        lbl_GrossAmount.Text = Format(TotGrsAmt, "########0.00")
        lbl_CashDiscAmount.Text = Format(TotDiscAmt, "########0.00")

        '***** GST START *****
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

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1209" And Trim(UCase(vEntryType)) <> "EXPORT" Then '------AADHARSH INTERNATIONAL
        '    lbl_IGstAmount.Text = Format(TotIGstAmt, "########0.00")
        '    lbl_CGstAmount.Text = Format(TotCGstAmt, "########0.00")
        '    lbl_SGstAmount.Text = Format(TotSGstAmt, "########0.00")
        'End If

        If Trim(UCase(vEntryType)) <> "EXPORT" Then
            lbl_IGstAmount.Text = Format(TotIGstAmt, "########0.00")
            lbl_CGstAmount.Text = Format(TotCGstAmt, "########0.00")
            lbl_SGstAmount.Text = Format(TotSGstAmt, "########0.00")
        End If

        Gross_Discount_Tax_Amount_Calculation()



    End Sub

    Private Sub Gross_Discount_Tax_Amount_Calculation()

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        lbl_TaxAmount.Text = ""
        'If Trim(UCase(cbo_TaxType.Text)) = "VAT" Then
        If chk_GSTTax_Invocie.Checked = False Then
            lbl_TaxAmount.Text = Format(Val(lbl_Assessable.Text) * Val(txt_TaxPerc.Text) / 100, "#########0.00")
        End If
        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal
        Dim vTCS_AssVal As String = 0
        Dim vTOT_SalAmt As String = 0
        Dim vTCS_Amt As String = 0
        Dim vInvAmt_Bfr_TCS As String = 0
        Dim Tax_Amt As Double = 0

        Dim GrsAmt As String = 0
        Dim AssVal As String = 0
        Dim vNet_Amt As String = 0
        Dim vGST_Amt As String = 0
        Dim vStrNetAmt As String = ""
        Dim TaxRndoff As String = ""
        Dim Taxass As String = ""
        Dim vTCS_Led_STS As String = 0
        Dim Led_ID As String = 0
        Dim vTCS_DED_STS As Boolean = False
        Dim vTDS_AssVal As String, vTDS_Amt As String
        Dim vTCS_Comp_STS As String = 0

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub


        Dim vTCS_StartDate As Date = #9/30/2020#
        Dim vMIN_TCS_assval As String = "5000000"
        Tax_Amt = Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text)

        vTCS_Comp_STS = 0
        Led_ID = 0
        vTCS_Led_STS = 0
        vTCS_DED_STS = False

        If chk_TCS_Tax.Checked = True Then

            If DateDiff("d", vTCS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                vTCS_Comp_STS = Common_Procedures.get_FieldValue(con, "company_head", "TCS_Company_Status", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")")

                If Val(vTCS_Comp_STS) = 1 Then

                    Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

                    vTCS_Led_STS = Common_Procedures.get_FieldValue(con, "ledger_head", "TCS_Sales_Status", "(ledger_idno = " & Str(Val(Led_ID)) & ")")

                    If Val(vTCS_Led_STS) = 1 Then
                        vTCS_DED_STS = True
                    End If

                    If vTCS_DED_STS = True Then

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

        vInvAmt_Bfr_TCS = Format(Val(lbl_Assessable.Text) + Val(Tax_Amt), "###########0.00")
        lbl_Invoice_Value_Before_TCS.Text = Format(Val(vInvAmt_Bfr_TCS), "###########0")
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = Format(Val(lbl_Invoice_Value_Before_TCS.Text) - Val(vInvAmt_Bfr_TCS), "###########0.00")

        If Str(Val(txt_CashDiscPerc.Text)) <> 0 Then
            txt_cashDisc_amt.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "########0.00")
        End If

        NtAmt = Val(lbl_GrossAmount.Text) - Val(txt_cashDisc_amt.Text) + Val(lbl_TaxAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(lbl_TcsAmount.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")


    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record


        pnl_Print.Visible = True
        pnl_Back.Enabled = False

        btn_Print_Invoice.Focus()

    End Sub

    Public Sub Print_Invoice()

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim NewCode As String
        Dim vPaprSz_STS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try



            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from FinishedProduct_Invoice_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        vPaprSz_STS = False

        prn_InpOpts = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1244" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1368" Then  '--- Madonna Tex
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "123")
        Else
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "12")
        End If

        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

        set_PaperSize_For_PrintDocument1()


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Or vPaprSz_STS = False Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument1.Print()
                        End If

                    Else
                        PrintDocument1.Print()

                    End If


                    'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

                    '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                    '    'PrintDocument1.DocumentName = "c:\test1.pdf"
                    '    'PrintDocument1.Print()

                    '    'Dim bytes As Byte() = PrintDocument1.Print("pdf")
                    '    ''Dim bytes As Byte() = RptViewer.LocalReport.Render("Excel")
                    '    'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.xls")

                    '    'Dim bytes As Byte() = System.IO.File.ReadAllBytes("C:\test1.pdf")
                    '    'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.pdf")
                    '    'fs.Write(bytes, 0, bytes.Length)
                    '    'fs.Close()
                    '    'MessageBox.Show("ok")

                    '    'PrintDocument1.Print()
                    'End If




                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            Try

                If vPaprSz_STS = False Then

                    set_PaperSize_For_PrintDocument1()

                    'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    'End If

                    'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                    '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    '        vPaprSz_STS = True
                    '        Exit For
                    '    End If
                    'Next

                End If


                Dim ppd As New PrintPreviewDialog




                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0


                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        Print_PDF_Status = False

    End Sub


    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0


        'MessageBox.Show("PrintDocument1_BeginPrint - 1 - START")

        If FrmLdSTS = True Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        cmd.Connection = con

        'MessageBox.Show("PrintDocument1_BeginPrint - 2 ")
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetIndx1 = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0
        prn_TotalMtrs = 0

        VCheck_ArticleNo = False

        Try


            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, a.DeliveryTo_IdNo , d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name, e.Ledger_PhoneNo as Agent_MobileNo, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, f.Ledger_mainName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_PhoneNo as DeliveryTo_LedgerPhoneNo, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_Mail as DeliveryTo_LedgerMailNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.MobileNo_Frsms as DeliveryTo_LedgerMobileNo_Frsms, f.Pan_No as DeliveryTo_PanNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code,e.Ledger_PhoneNo as Agent_PhoneNo , d.Ledger_GSTinNo as Transport_GSTinNo from FinishedProduct_Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo INNER JOIN Ledger_Head c ON (case when a.OnAc_IdNo <>0 then a.OnAc_IdNo else a.Ledger_IdNo end) = c.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' ", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Processed_Item_Nm, d.Unit_Name from FinishedProduct_Invoice_Details a INNER JOIN Processed_Item_Head b ON a.Item_IdNo = b.Processed_Item_IdNo  LEFT OUTER JOIN Unit_Head d ON a.Unit_IdNo = d.Unit_IdNo Where a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Format_2_Status = 1 Then
            Printing_PackingList_FinishedProduct_Format1(e)
        Else
            Printing_GST_FinishedProduct_ExportInvoice_Format1(e)
        End If
    End Sub

    Private Sub Printing_GST_FinishedProduct_ExportInvoice_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim SNo As Integer
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim vNoofHsnCodes As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String
        Dim clrNm1 As String, clrNm2 As String, clrNm3 As String
        Dim vORDNO1 As String, vORDNO2 As String, vORDNO3 As String
        Dim vSZNM1 As String, vSZNM2 As String, vSZNM3 As String
        Dim vFOOTR_NOOFLINES As Integer
        Dim vLine_Pen As Pen
        Dim vFontName As String = ""

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 45
            .Right = 50
            .Top = 40 ' 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With
        vFontName = "Calibri"
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
        TxtHgt = 17  ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 45 : ClArr(2) = 85 : ClArr(3) = 70 : ClArr(4) = 80 : ClArr(5) = 65 : ClArr(6) = 65 : ClArr(7) = 70 : ClArr(8) = 55 : ClArr(9) = 60 : ClArr(10) = 60
        ClArr(11) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If prn_PageNo <= 1 Then

            NoofItems_PerPage = 17 ' 15 ' 12 ' 15 


        Else

            NoofItems_PerPage = 8


        End If

        Dim vNOOFTAXLINES As Integer, vGST_PERC_AMT_FOR_PRNT As String
        vNOOFTAXLINES = 0
        vGST_PERC_AMT_FOR_PRNT = get_GSTPercentage_and_GSTAmount_For_Printing(EntryCode, vNOOFTAXLINES)
        If vNOOFTAXLINES <= 1 Then vNOOFTAXLINES = 2
        If vNOOFTAXLINES > 2 Then
            NoofItems_PerPage = NoofItems_PerPage - (vNOOFTAXLINES - 2)
        End If

        vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)


        vLine_Pen = New Pen(Color.Black, 2)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_FinishedProduct_ExportInvoice_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0

                    CurY = CurY - 5

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If prn_DetDt.Rows.Count > 0 Then

                                'If prn_PageNo <= 1 Then

                                '    NoofItems_PerPage = 12 ' 15 

                                'Else

                                '    NoofItems_PerPage = 8

                                'End If


                                'vGST_PERC_AMT_FOR_PRNT = get_GSTPercentage_and_GSTAmount_For_Printing(EntryCode, vNOOFTAXLINES)
                                'NoofItems_PerPage = NoofItems_PerPage - vNOOFTAXLINES
                                'vLine_Pen = New Pen(Color.Black, 2)

                                vFOOTR_NOOFLINES = 15 ' 10 ' 5
                                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then vFOOTR_NOOFLINES = vFOOTR_NOOFLINES + 1
                                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then vFOOTR_NOOFLINES = vFOOTR_NOOFLINES + 1
                                If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then vFOOTR_NOOFLINES = vFOOTR_NOOFLINES + 1
                                vFOOTR_NOOFLINES = vFOOTR_NOOFLINES + vNOOFTAXLINES + (vNoofHsnCodes + 4)

                                'vFOOTR_NOOFLINES = 10
                                'vFOOTR_NOOFLINES = vFOOTR_NOOFLINES + (vNOOFTAXLINES * 2) + (vNoofHsnCodes + 4)

                                If prn_DetIndx >= (prn_DetDt.Rows.Count - 2) Then

                                    If CurY >= 800 Or (CurY + (vFOOTR_NOOFLINES * TxtHgt)) >= (PageHeight - TxtHgt) Then

                                        If CurY < (PageHeight - TxtHgt - TxtHgt - 10) Then
                                            CurY = PageHeight - TxtHgt - TxtHgt - TxtHgt - 10
                                        Else
                                            CurY = PageHeight - TxtHgt - TxtHgt
                                        End If

                                        CurY = CurY + 10
                                        Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)

                                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                                        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                                        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(4))
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(4))
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(4))
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(4))
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(4))
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(4))
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(4))
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(4))
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), LnAr(4))
                                        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), LnAr(4))

                                        p1Font = New Font(vFontName, 8, FontStyle.Regular)
                                        Common_Procedures.Print_To_PrintDocument(e, "Page No. " & prn_PageNo, LMargin, CurY + 5, 2, PrintWidth, p1Font)

                                        e.HasMorePages = True
                                        Return

                                    End If


                                ElseIf CurY >= (PageHeight - TxtHgt - TxtHgt) Then

                                    If CurY < (PageHeight - TxtHgt - TxtHgt - 10) Then
                                        CurY = PageHeight - TxtHgt - TxtHgt - 10
                                    End If

                                    CurY = CurY + 10
                                    Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)

                                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                                    e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                                    e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


                                    p1Font = New Font(vFontName, 8, FontStyle.Regular)
                                    Common_Procedures.Print_To_PrintDocument(e, "Page No. " & prn_PageNo, LMargin, CurY + 5, 2, PrintWidth, p1Font)

                                    e.HasMorePages = True
                                    Return

                                End If

                            End If

                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Item_nm").ToString)


                            ItmNm2 = ""
                            ItmNm3 = ""
                            If Len(ItmNm1) > 10 Then
                                For I = 10 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 10
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If


                            If Len(ItmNm2) > 8 Then

                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 8

                                ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)

                            End If


                            clrNm1 = Common_Procedures.Colour_IdNoToName(con, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_Idno").ToString))

                            clrNm2 = ""
                            clrNm3 = ""

                            If Len(clrNm1) > 8 Then
                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(clrNm1), I, 1) = " " Or Mid$(Trim(clrNm1), I, 1) = "," Or Mid$(Trim(clrNm1), I, 1) = "." Or Mid$(Trim(clrNm1), I, 1) = "-" Or Mid$(Trim(clrNm1), I, 1) = "/" Or Mid$(Trim(clrNm1), I, 1) = "_" Or Mid$(Trim(clrNm1), I, 1) = "(" Or Mid$(Trim(clrNm1), I, 1) = ")" Or Mid$(Trim(clrNm1), I, 1) = "\" Or Mid$(Trim(clrNm1), I, 1) = "[" Or Mid$(Trim(clrNm1), I, 1) = "]" Or Mid$(Trim(clrNm1), I, 1) = "{" Or Mid$(Trim(clrNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 8

                                clrNm2 = Microsoft.VisualBasic.Right(Trim(clrNm1), Len(clrNm1) - I)
                                clrNm1 = Microsoft.VisualBasic.Left(Trim(clrNm1), I - 1)
                            End If

                            If Len(clrNm2) > 8 Then

                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(clrNm2), I, 1) = " " Or Mid$(Trim(clrNm2), I, 1) = "," Or Mid$(Trim(clrNm2), I, 1) = "." Or Mid$(Trim(clrNm2), I, 1) = "-" Or Mid$(Trim(clrNm2), I, 1) = "/" Or Mid$(Trim(clrNm2), I, 1) = "_" Or Mid$(Trim(clrNm2), I, 1) = "(" Or Mid$(Trim(clrNm2), I, 1) = ")" Or Mid$(Trim(clrNm2), I, 1) = "\" Or Mid$(Trim(clrNm2), I, 1) = "[" Or Mid$(Trim(clrNm2), I, 1) = "]" Or Mid$(Trim(clrNm2), I, 1) = "{" Or Mid$(Trim(clrNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 8

                                clrNm3 = Microsoft.VisualBasic.Right(Trim(clrNm2), Len(clrNm2) - I)
                                clrNm2 = Microsoft.VisualBasic.Left(Trim(clrNm2), I - 1)

                            End If



                            vORDNO1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Order_No").ToString)
                            vORDNO2 = ""
                            vORDNO3 = ""

                            If Len(vORDNO1) > 7 Then
                                For I = 7 To 1 Step -1
                                    If Mid$(Trim(vORDNO1), I, 1) = " " Or Mid$(Trim(vORDNO1), I, 1) = "," Or Mid$(Trim(vORDNO1), I, 1) = "." Or Mid$(Trim(vORDNO1), I, 1) = "-" Or Mid$(Trim(vORDNO1), I, 1) = "/" Or Mid$(Trim(vORDNO1), I, 1) = "_" Or Mid$(Trim(vORDNO1), I, 1) = "(" Or Mid$(Trim(vORDNO1), I, 1) = ")" Or Mid$(Trim(vORDNO1), I, 1) = "\" Or Mid$(Trim(vORDNO1), I, 1) = "[" Or Mid$(Trim(vORDNO1), I, 1) = "]" Or Mid$(Trim(vORDNO1), I, 1) = "{" Or Mid$(Trim(vORDNO1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 7

                                vORDNO2 = Microsoft.VisualBasic.Right(Trim(vORDNO1), Len(vORDNO1) - I)
                                vORDNO1 = Microsoft.VisualBasic.Left(Trim(vORDNO1), I - 1)
                            End If

                            If Len(vORDNO2) > 7 Then
                                For I = 7 To 1 Step -1
                                    If Mid$(Trim(vORDNO2), I, 1) = " " Or Mid$(Trim(vORDNO2), I, 1) = "," Or Mid$(Trim(vORDNO2), I, 1) = "." Or Mid$(Trim(vORDNO2), I, 1) = "-" Or Mid$(Trim(vORDNO2), I, 1) = "/" Or Mid$(Trim(vORDNO2), I, 1) = "_" Or Mid$(Trim(vORDNO2), I, 1) = "(" Or Mid$(Trim(vORDNO2), I, 1) = ")" Or Mid$(Trim(vORDNO2), I, 1) = "\" Or Mid$(Trim(vORDNO2), I, 1) = "[" Or Mid$(Trim(vORDNO2), I, 1) = "]" Or Mid$(Trim(vORDNO2), I, 1) = "{" Or Mid$(Trim(vORDNO2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 7

                                vORDNO3 = Microsoft.VisualBasic.Right(Trim(vORDNO2), Len(vORDNO2) - I)
                                vORDNO2 = Microsoft.VisualBasic.Left(Trim(vORDNO2), I - 1)
                            End If


                            vSZNM1 = Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Size_idno").ToString))
                            vSZNM2 = ""
                            vSZNM3 = ""

                            If Len(vSZNM1) > 7 Then
                                For I = 7 To 1 Step -1
                                    If Mid$(Trim(vSZNM1), I, 1) = " " Or Mid$(Trim(vSZNM1), I, 1) = "," Or Mid$(Trim(vSZNM1), I, 1) = "." Or Mid$(Trim(vSZNM1), I, 1) = "-" Or Mid$(Trim(vSZNM1), I, 1) = "/" Or Mid$(Trim(vSZNM1), I, 1) = "_" Or Mid$(Trim(vSZNM1), I, 1) = "(" Or Mid$(Trim(vSZNM1), I, 1) = ")" Or Mid$(Trim(vSZNM1), I, 1) = "\" Or Mid$(Trim(vSZNM1), I, 1) = "[" Or Mid$(Trim(vSZNM1), I, 1) = "]" Or Mid$(Trim(vSZNM1), I, 1) = "{" Or Mid$(Trim(vSZNM1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 7

                                vSZNM2 = Microsoft.VisualBasic.Right(Trim(vSZNM1), Len(vSZNM1) - I)
                                vSZNM1 = Microsoft.VisualBasic.Left(Trim(vSZNM1), I - 1)
                            End If

                            If Len(vSZNM2) > 7 Then
                                For I = 7 To 1 Step -1
                                    If Mid$(Trim(vSZNM2), I, 1) = " " Or Mid$(Trim(vSZNM2), I, 1) = "," Or Mid$(Trim(vSZNM2), I, 1) = "." Or Mid$(Trim(vSZNM2), I, 1) = "-" Or Mid$(Trim(vSZNM2), I, 1) = "/" Or Mid$(Trim(vSZNM2), I, 1) = "_" Or Mid$(Trim(vSZNM2), I, 1) = "(" Or Mid$(Trim(vSZNM2), I, 1) = ")" Or Mid$(Trim(vSZNM2), I, 1) = "\" Or Mid$(Trim(vSZNM2), I, 1) = "[" Or Mid$(Trim(vSZNM2), I, 1) = "]" Or Mid$(Trim(vSZNM2), I, 1) = "{" Or Mid$(Trim(vSZNM2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 7

                                vSZNM3 = Microsoft.VisualBasic.Right(Trim(vSZNM2), Len(vSZNM2) - I)
                                vSZNM2 = Microsoft.VisualBasic.Left(Trim(vSZNM2), I - 1)
                            End If



                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, ClArr(2), pFont,, True)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vSZNM1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, ClArr(3), pFont,, True)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(clrNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, ClArr(4), pFont,, True)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vORDNO1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, ClArr(5), pFont,, True)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Hsn_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 7, CurY, 0, ClArr(6), pFont,, True)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Pcs_per_Packs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Packs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) - 10, CurY, 1, 0, pFont)


                            If Trim(ItmNm2) <> "" Or Trim(vSZNM2) <> "" Or Trim(clrNm2) <> "" Or Trim(vORDNO2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vSZNM2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont,, True)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(clrNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vORDNO2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If


                            If Trim(clrNm3) <> "" Or Trim(vSZNM3) <> "" Or Trim(ItmNm3) <> "" Or Trim(vORDNO3) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vSZNM3), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont,, True)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(clrNm3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vORDNO3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If



                            NoofDets = NoofDets + 1

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_GST_FinishedProduct_ExportInvoice_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageHeight, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True, vFOOTR_NOOFLINES)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 0 ' 1
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If
                    End If


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES Not PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES Not PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub


    Private Sub Printing_GST_FinishedProduct_ExportInvoice_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font, p2font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Cmp_UAMNO As String = ""
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0, k As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim vbr_CmpName As SolidBrush          '--COMPANY TITTLE
        Dim vbr_CmpDets As SolidBrush          '--COMPANY DETAILS

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("Select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b On a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c On b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
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

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "COMMERCIAL INVOICE", LMargin, CurY - TxtHgt - 4, 2, PrintWidth, p1Font)

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1225" Then  '--- Yasen Tex (Mangalam)
            '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.YASEN_LOGO, Drawing.Image), LMargin + 20, CurY + 5, 120, 100)
            'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1244" Then  '--- Madonna Tex
            '    If InStr(1, Trim(UCase(prn_HdDt_New.Rows(0).Item("Company_Name").ToString)), "MARSLIN") > 0 Then
            '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MarslinTex, Drawing.Image), LMargin + 10, CurY + 5, 90, 110)
            '    Else
            '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MadonnaTex, Drawing.Image), LMargin + 10, CurY + 5, 90, 110)
            '    End If
            '    If InStr(1, Trim(UCase(prn_HdDt_New.Rows(0).Item("Company_Name").ToString)), "MARSLIN") > 0 Then
            '        If Vchk_shirt_bill <> 0 Then
            '            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.COMPANYLOGO_MARSLIN, Drawing.Image), PageWidth - 100, CurY + 5, 90, 100)

            '        Else
            '            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Marslin_Madonna_Tex, Drawing.Image), PageWidth - 100, CurY + 5, 90, 110)

            '        End If
            '    End If
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
                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 20, CurY + 5, 110, 100)

                            End If

                        End Using

                    End If

                End If

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

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 20, 90, 90)

                        End If

                    End Using
                End If
            End If

        End If

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_UAMNO = ""

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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

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


        CurY = CurY + TxtHgt - 15

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1225" Then  '--- Yasen Tex (Mangalam)
            Dim vLightGreenBrush As New SolidBrush(Color.FromArgb(171, 206, 26))
            p1Font = New Font("Cambria", 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, vLightGreenBrush)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1460" Then
            p1Font = New Font("Times New Roman", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then
            If Trim(UCase(Cmp_GSTIN_No)) = "33BUPPK6766D1ZC" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p2font, vbr_CmpName)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            End If
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpDets)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpDets)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, vbr_CmpDets)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, vbr_CmpDets)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, vbr_CmpDets)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, vbr_CmpDets)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font, vbr_CmpDets)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont, vbr_CmpDets)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        If Trim(Cmp_UAMNO) <> "" Then
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpDets)
        End If

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY - 10, PageWidth, CurY - 10)
            LnAr(2) = CurY


            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)
            ItmNm2 = ""

            If Len(ItmNm1) > 35 Then
                For i = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
            End If

            CurY = CurY - 5

        End If

        'CurY = CurY + TxtHgt + 5
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(2) = CurY

        'Y1 = CurY + 0.5
        'Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        'vHeading = Trim(UCase(prn_HdDt.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"

        'CurY = CurY + TxtHgt - 15
        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)       :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("FinishedProduct_invoice_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1460" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1508" Then
                Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Transportation_Mode").ToString) = "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "ROAD ", LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
                End If
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1508" Then ' S.m Knit Wear

                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Order_No").ToString)

                ItmNm2 = ""
                If Len(ItmNm1) > 20 Then
                    For k = 20 To 1 Step -1
                        If Mid$(Trim(ItmNm1), k, 1) = " " Or Mid$(Trim(ItmNm1), k, 1) = "," Or Mid$(Trim(ItmNm1), k, 1) = "." Or Mid$(Trim(ItmNm1), k, 1) = "-" Or Mid$(Trim(ItmNm1), k, 1) = "/" Or Mid$(Trim(ItmNm1), k, 1) = "_" Or Mid$(Trim(ItmNm1), k, 1) = "(" Or Mid$(Trim(ItmNm1), k, 1) = ")" Or Mid$(Trim(ItmNm1), k, 1) = "\" Or Mid$(Trim(ItmNm1), k, 1) = "[" Or Mid$(Trim(ItmNm1), k, 1) = "]" Or Mid$(Trim(ItmNm1), k, 1) = "{" Or Mid$(Trim(ItmNm1), k, 1) = "}" Then Exit For
                    Next k
                    If k = 0 Then k = 20
                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - k)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), k - 1)
                End If


                Common_Procedures.Print_To_PrintDocument(e, "Order No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            Else
                'Common_Procedures.Print_To_PrintDocument(e, "EwayBill No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Other Reference", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_Reference").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)




            End If

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("FinishedProduct_invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1508" Then
                'Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Total Pkgs", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_No_Of_Packs").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            Else
                If Trim(ItmNm2) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + C2 + 100, CurY1, 1, 0, pFont)
                End If
            End If

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Doc No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("LR_No").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)




            Common_Procedures.Print_To_PrintDocument(e, "Other Reference", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_Reference").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)






            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1460" Then
                CurY1 = CurY1 + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Shipment Date", LMargin + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)

                If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)
                End If

                Common_Procedures.Print_To_PrintDocument(e, "Destination", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
                End If

            Else
                '  CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Order No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + C2 + W1 + 30 + strWidth, CurY1, 0, 0, pFont)
                End If

            End If
            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY1 = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO) : ", LMargin + C2 + 10, CurY1, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO) : ", LMargin + 10, CurY1, 0, 0, p1Font)
            CurY = CurY1 + TxtHgt

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString & " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12

            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            Else
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            End If
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
                If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If
                If Trim(vDelvPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + C2 + 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + S1 + 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 40, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))

            ''***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            '***** GST START *****
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "/CTN", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PKG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRICE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
            '***** GST END *****

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_FinishedProduct_ExportInvoice_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageHeight As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vFOOTR_NOOFLINES As Integer)
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
        Dim vGST_PERC_AMT_FOR_PRNT As String = ""
        Dim ar_GSTDET() As String, ar_GSTAMT() As String
        Dim vNOOFTAXLINES As Integer
        Dim Cmp_GSTIN_No As String
        Dim vbr As SolidBrush                  '--COMMON BRUSH FOR ALL DETAILS 
        Dim vbr_CmpName As SolidBrush          '--COMPANY TITTLE
        Dim vbr_CmpDets As SolidBrush          '--COMPANY DETAILS
        Dim vFOOTER_topY As Double

        Try

            vFOOTER_topY = (PageHeight - TxtHgt - (vFOOTR_NOOFLINES * TxtHgt))
            If CurY < vFOOTER_topY Then
                CurY = vFOOTER_topY
            End If


            'If CurY >= 800 Or (CurY + (vFOOTR_NOOFLINES * TxtHgt)) >= (PageHeight - TxtHgt) Then
            '    If CurY < (PageHeight - TxtHgt - TxtHgt - 10) Then
            '        CurY = PageHeight - TxtHgt - TxtHgt - TxtHgt - 10
            '    Else
            '        CurY = PageHeight - TxtHgt - TxtHgt
            '    End If
            '    CurY = CurY + 10
            '    Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)
            'End if


            'For I = NoofDets + 1 To NoofItems_PerPage
            '    CurY = CurY + TxtHgt
            '    'prn_DetIndx = prn_DetIndx + 1
            'Next


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_No_Of_Packs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(6), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(6), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(6), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))


            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt - 10
                    'If Val(prn_PageNo) = 1 Then
                    '  p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                    '  Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 11, FontStyle.Bold)
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


            CurY = CurY - 10
            CurY = CurY + TxtHgt
            '***** GST START *****

            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Disc @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
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
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, p1Font)
                End If
            End If


            If is_LastPage = True Then
                vNOOFTAXLINES = 0
                vGST_PERC_AMT_FOR_PRNT = get_GSTPercentage_and_GSTAmount_For_Printing(EntryCode, vNOOFTAXLINES)
                If Trim(vGST_PERC_AMT_FOR_PRNT) <> "" Then

                    ar_GSTDET = Split(vGST_PERC_AMT_FOR_PRNT, "#^#")

                    For K = 0 To UBound(ar_GSTDET)
                        If Trim(ar_GSTDET(K)) <> "" Then
                            ar_GSTAMT = Split(ar_GSTDET(K), "$^$")
                            If Val(ar_GSTAMT(1)) <> 0 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ar_GSTAMT(0)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(ar_GSTAMT(1)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                            End If

                        End If
                    Next K
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Tcs @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 20, CurY, 0, 0, pFont)


            If is_LastPage = True Then

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(7), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6))

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

            vNoofHsnCodes = 0
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then '---- GAJAKHARNAA TRADERS (Somanur)
                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            End If
            If vNoofHsnCodes <> 0 Then
                Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            End If


            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            If Trim(prn_HdDt.Rows(0).Item("Jurisdiction").ToString) <> "" Then
                Jurs = Trim(prn_HdDt.Rows(0).Item("Jurisdiction").ToString)
            Else
                Jurs = Common_Procedures.settings.Jurisdiction
                If Trim(Jurs) = "" Then Jurs = "Tirupur"
            End If


            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, p1Font)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            If (Trim(Common_Procedures.settings.CustomerCode)) = "--1460--" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Terms & Conditions :", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " 1. Any Complaint regarding goods must be in willing within 2 days.", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " 2. Interest @ 24% will be charged if payment not made within due date.", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " 3. Any Claims out of this Sale is Subject to " & Trim(Jurs) & " Jurisdiction.", LMargin + 10, CurY, 0, 0, pFont)

            Else

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "We Declare that this Invoice Shows the actual price of the goods", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, pFont)

            End If

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15

            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            If (Trim(Common_Procedures.settings.CustomerCode)) <> "1460" And Trim(Common_Procedures.settings.CustomerCode) <> "1551" Then
                Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Jurs & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)
            End If

            If Print_PDF_Status = True Then
                CurY = CurY + TxtHgt - 15
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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
            If chk_GSTTax_Invocie.Checked = True Then

                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

            End If

            AssVal_Frgt_Othr_Charges = Val(txt_Freight.Text)

            cmd.Connection = con

            cmd.CommandText = "Truncate table EntryTemp"
            cmd.ExecuteNonQuery()

            With dgv_Details

                If .Rows.Count > 0 Then
                    For i = 0 To .Rows.Count - 1
                        If Trim(.Rows(i).Cells(DgvCol_Details.ITEM_NAME).Value) <> "" And Val(.Rows(i).Cells(DgvCol_Details.GST_PERC).Value) <> 0 Then
                            'If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(10).Value) <> "" And Val(.Rows(i).Cells(11).Value) <> 0 Then

                            cmd.CommandText = "Insert into EntryTemp (                    Name1                ,                   Currency1            ,                       Currency2                                      ) " &
                                              "            Values    ( '" & Trim(.Rows(i).Cells(DgvCol_Details.HSN_CODE).Value) & "', " & (Val(.Rows(i).Cells(DgvCol_Details.GST_PERC).Value)) & ", " & Str(Val(.Rows(i).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value) + AssVal_Frgt_Othr_Charges) & " ) "
                            cmd.ExecuteNonQuery()

                            AssVal_Frgt_Othr_Charges = 0

                        End If

                    Next
                End If
            End With

            With dgv_GSTTax_Details

                .Rows.Clear()
                Sno = 0

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as Assessable_Value from EntryTemp group by name1, Currency1 Having sum(Currency2) <> 0 ", con)
                'da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as Assessable_Value from EntryTemp group by name1, Currency1 Having sum(Currency2) <> 0 ", con)
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

            SubClAr(1) = 100 : SubClAr(2) = 100 : SubClAr(3) = 45 : SubClAr(4) = 90 : SubClAr(5) = 45 : SubClAr(6) = 90 : SubClAr(7) = 45 : SubClAr(8) = 90
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin, CurY, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1), CurY, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TAX AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'Da = New SqlClient.SqlDataAdapter("Select  a.cgst_Percentage as CGST_Percentage ,a.sgst_Percentage as SGST_Percentage ,a.igst_Percentage as IGST_Percentage,sum(a.cgst_amount) AS CGST_Amount,sum(a.sgst_amount) as SGST_Amount,sum(a.igst_amount) AS IGST_Amount ,sum(a.taxable_amount) as Taxable_Amount from Sales_GST_Tax_Details a Where Sales_Code = '" & Trim(EntryCode) & "' group by cgst_percentage , sgst_percentage , igst_percentage   ", con)

            'THIS ONE Is CORRECT
            'Da = New SqlClient.SqlDataAdapter("Select mAX(a.HSN_Code) As Hsn_Code ,mAX(a.cgst_Percentage) as CGST_Percentage ,maX(a.sgst_Percentage) as SGST_Percentage ,min(a.igst_Percentage) as IGST_Percentage,sum(a.cgst_amount) AS CGST_Amount,sum(a.sgst_amount) as SGST_Amount,sum(a.igst_amount) AS IGST_Amount ,sum(a.taxable_amount) as Taxable_Amount from Sales_GST_Tax_Details a Where Sales_Code = '" & Trim(EntryCode) & "' group by cgst_percentage , sgst_percentage , igst_percentage  ", con)

            Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx1 = 0
                NoofDets = 0
                NoofItems_Increment = 0

                CurY = CurY - 20

                Do While prn_DetIndx1 <= Dt.Rows.Count - 1

                    ItmNm1 = Trim(Dt.Rows(prn_DetIndx1).Item("HSN_CODE").ToString)

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
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx1).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx1).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx1).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx1).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx1).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx1).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx1).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx1).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx1).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx1).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx1).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx1).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx1).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx1).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx1).Item("SGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx1).Item("SGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx1).Item("IGST_Amount").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)
                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx1).Item("Taxable_Amount").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx1).Item("CGST_Amount").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx1).Item("SGST_Amount").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx1).Item("IGST_Amount").ToString)
                    prn_DetIndx1 = prn_DetIndx1 + 1
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
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), LnAr)

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


    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
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
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        Cmd.Connection = con

        Cmd.CommandText = "Truncate table EntryTempSub "
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into EntryTempSub (Meters1, Currency1) select (CGST_Percentage+SGST_Percentage), (CGST_Amount+SGST_Amount) from Sales_GST_Tax_Details  Where Sales_Code = '" & Trim(EntryCode) & "' and (CGST_Amount+SGST_Amount) <> 0"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into EntryTempSub (Meters1, Currency1) select IGST_Percentage, IGST_Amount from Sales_GST_Tax_Details  Where Sales_Code = '" & Trim(EntryCode) & "' and IGST_Amount <> 0"
        Cmd.ExecuteNonQuery()

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select Meters1, sum(Currency1) from EntryTempSub Group by Meters1 Having sum(Currency1) <> 0", con)
        'Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
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

    'Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim Dt2 As New DataTable
    '    Dim TaxPerc As Single = 0

    '    TaxPerc = 0

    '    Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
    '    Dt1 = New DataTable
    '    Da.Fill(Dt1)
    '    If Dt1.Rows.Count > 0 Then
    '        If Dt1.Rows.Count = 1 Then

    '            Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
    '            Dt2 = New DataTable
    '            Da.Fill(Dt2)
    '            If Dt2.Rows.Count > 0 Then
    '                If Val(Dt2.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
    '                    TaxPerc = Val(Dt2.Rows(0).Item("IGST_Percentage").ToString)
    '                Else
    '                    TaxPerc = Val(Dt2.Rows(0).Item("CGST_Percentage").ToString)
    '                End If
    '            End If
    '            Dt2.Clear()

    '        End If
    '    End If
    '    Dt1.Clear()

    '    Dt1.Dispose()
    '    Dt2.Dispose()
    '    Da.Dispose()

    '    get_GST_Tax_Percentage_For_Printing = Format(Val(TaxPerc), "#########0.00")

    'End Function

    Private Sub txt_DateTime_Of_Supply_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DateTime_Of_Supply.GotFocus
        If Trim(txt_DateTime_Of_Supply.Text) = "" And New_Entry = True Then
            txt_DateTime_Of_Supply.Text = Format(Now, "hh:mm tt")
        End If
    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DateTime_Of_Supply.KeyDown
        If e.KeyValue = 38 Then txt_Electronic_RefNo.Focus()
        If e.KeyValue = 40 Then txt_OrderNo.Focus()
    End Sub
    '***** GST END *****
    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, txt_Place_Of_Supply, cbo_DispatcherName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_TransportMode, txt_DateTime_Of_Supply, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_DispatcherName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_DateTime_Of_Supply, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
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



    Private Sub Printing_GST_HSN_Details_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
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

            SubClAr(1) = 100 : SubClAr(2) = 100 : SubClAr(3) = 45 : SubClAr(4) = 90 : SubClAr(5) = 45 : SubClAr(6) = 90 : SubClAr(7) = 45 : SubClAr(8) = 90
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin, CurY, 2, SubClAr(1), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1), CurY, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TAX AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Da = New SqlClient.SqlDataAdapter("Select * from EntryTemp Where Name1 = '" & Trim(EntryCode) & "'", con)
            'Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then



                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0

                CurY = CurY - 20



                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("Name2").ToString)
                    'ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

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

                    'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p2Font)
                    'Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
                    'Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
                    'Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
                    'Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
                    'Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
                    'Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
                    'Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

                    'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)
                    'NoofItems_Increment = NoofItems_Increment + 1

                    'NoofDets = NoofDets + 1

                    'Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)
                    'Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)
                    'Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)
                    'Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)
                    'prn_DetIndx = prn_DetIndx + 1


                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Currency1").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Currency1").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Currency2").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Currency2").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Currency3").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Currency3").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Currency4").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Currency4").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Currency5").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Currency3").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Currency6").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Currency6").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Currency7").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Currency7").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Currency3").ToString) + Val(Dt.Rows(prn_DetIndx).Item("Currency5").ToString) + Val(Dt.Rows(prn_DetIndx).Item("Currency7").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)
                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Currency1").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx).Item("Currency3").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx).Item("Currency5").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx).Item("Currency7").ToString)
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
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), LnAr)

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

        LastNo = lbl_InvoiceNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_InvoiceNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub btn_sms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_sms.Click

        Try


            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.KeyCode = 17 And e.Control = False And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

        If e.KeyValue = 38 Then
            e.Handled = True : e.SuppressKeyPress = True

            If txt_Due_Days.Visible = True Then
                txt_Due_Days.Focus()
            ElseIf txt_PaymentTerms.Visible = True Then
                txt_PaymentTerms.Focus()
            ElseIf txt_VehicleNo.Visible = True Then
                txt_VehicleNo.Focus()
            Else
                txt_Freight.Focus()
            End If
        End If
        If e.KeyValue = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            'If cbo_EntType.Visible Then
            '    cbo_EntType.Focus()
            'Else
            cbo_Ledger.Focus()

            'End If
        End If

    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If UCase(Chr(Asc(e.KeyChar))) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            'If cbo_EntType.Visible Then
            '    cbo_EntType.Focus()
            'Else
            cbo_Ledger.Focus()

            'End If
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
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

    Private Sub dtp_Date_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.ValueChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus
        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(msk_Date.Text) <= 31 And Microsoft.VisualBasic.DateAndTime.Month(msk_Date.Text) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(msk_Date.Text) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(msk_Date.Text) >= 2010 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If
        End If
    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DateTime_Of_Supply.KeyPress
        If UCase(e.KeyChar) = "D" Or UCase(e.KeyChar) = "T" Then
            txt_DateTime_Of_Supply.Text = Format(Now, "hh:mm tt")
            e.Handled = True
            txt_DateTime_Of_Supply.SelectionStart = txt_DateTime_Of_Supply.TextLength
        End If
    End Sub



    Private Sub txt_Due_Days_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Due_Days.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Transport_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_VehicleNo, txt_Trans_Freight, "Ledger_Head", "Ledger_Name", "", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Trans_Freight, "Ledger_Head", "Ledger_Name", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub txt_Trans_Freight_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Trans_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
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


            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            '-----------TOTAL SALES

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@entrydate", dtp_Date.Value.Date)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            If Led_ID <> 0 Then

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GSALE-%' OR a.Voucher_Code LIKE 'GSSAL-%' )" 'OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%') "
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

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(vPrevYrCode) & "' and (a.Voucher_Code LIKE 'GSALE-%' OR a.Voucher_Code LIKE 'GSSAL-%' )" 'and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%') "
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

    Private Sub cbo_Currency_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Currency.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "currency_head", "currency_Name", "", "")
    End Sub

    Private Sub cbo_Currency_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Currency.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Currency, Nothing, txt_ExchangeRate, "currency_head", "currency_Name", "", "")

        If (e.KeyValue = 38 And cbo_Currency.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_DispatcherName.Visible Then
                cbo_DispatcherName.Focus()
            Else
                'cbo_TaxType.Focus()
            End If

        End If

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

    Private Sub txt_ExchangeRate_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_ExchangeRate.KeyDown
        If e.KeyValue = 38 Then
            cbo_Currency.Focus()
        End If
        ' SendKeys.Send("+{TAB}")

        If e.KeyValue = 40 Then
            cbo_ItemName.Focus()
        End If
        ' SendKeys.Send("{TAB}")
    End Sub
    Private Sub txt_ExchangeRate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ExchangeRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            cbo_ItemName.Focus()
        End If
    End Sub

    Private Sub txt_ExchangeRate_TextChanged(sender As Object, e As System.EventArgs) Handles txt_ExchangeRate.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub lbl_Amount_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_Amount.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub lbl_IGstAmount_DoubleClick(sender As Object, e As System.EventArgs) Handles lbl_IGstAmount.DoubleClick
        Try
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" And Trim(UCase(vEntryType)) = "EXPORT" Then '------AADHARSH INTERNATIONAL
                lbl_IGstAmount.Text = InputBox("Enter IGST Value", "FOR IGST VALUE", Val(lbl_IGstAmount.Text))
            End If
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub lbl_SGstAmount_DoubleClick(sender As Object, e As System.EventArgs) Handles lbl_SGstAmount.DoubleClick
        Try
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" And Trim(UCase(vEntryType)) = "EXPORT" Then '------AADHARSH INTERNATIONAL
                lbl_SGstAmount.Text = InputBox("Enter sgst Value", "FOR SGST VALUE", Val(lbl_SGstAmount.Text))
            End If
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub lbl_CGstAmount_DoubleClick(sender As Object, e As System.EventArgs) Handles lbl_CGstAmount.DoubleClick
        Try
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1209" And Trim(UCase(vEntryType)) = "EXPORT" Then '------AADHARSH INTERNATIONAL
                lbl_CGstAmount.Text = InputBox("Enter CGST Value", "FOR CGST VALUE", Val(lbl_CGstAmount.Text))
            End If
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub lbl_IGstAmount_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_IGstAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_CGstAmount_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_CGstAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_SGstAmount_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_SGstAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_cashDisc_amt_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_cashDisc_amt.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_cashDisc_amt_TextChanged(sender As Object, e As System.EventArgs) Handles txt_cashDisc_amt.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        'If Val(Common_Procedures.User.IdNo) = 1 Then
        '    Dim f1 As New User_Modifications("")
        '    f1.Entry_Name = Me.Name
        '    f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        '    f1.ShowDialog()
        'End If
    End Sub


    Private Sub cbo_InvoiceSufixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_InvoiceSufixNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_InvoiceSufixNo, Nothing, msk_Date, "", "", "", "")
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_InvoiceSufixNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_InvoiceSufixNo, msk_Date, "", "", "", "", False)
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

                'Dim f As New Report_Details_1

                'f.RptSubReport_Index = RptSubReport_Index

                'For I = 1 To 10

                '    f.RptSubReportDet(I).ReportName = RptSubReportDet(I).ReportName
                '    f.RptSubReportDet(I).ReportGroupName = RptSubReportDet(I).ReportGroupName
                '    f.RptSubReportDet(I).ReportHeading = RptSubReportDet(I).ReportHeading
                '    f.RptSubReportDet(I).ReportInputs = RptSubReportDet(I).ReportInputs
                '    f.RptSubReportDet(I).IsGridReport = RptSubReportDet(I).IsGridReport
                '    f.RptSubReportDet(I).CurrentRowVal = RptSubReportDet(I).CurrentRowVal
                '    f.RptSubReportDet(I).TopRowVal = RptSubReportDet(I).TopRowVal

                '    f.RptSubReportDet(I).DateInp_Value1 = RptSubReportDet(I).DateInp_Value1
                '    f.RptSubReportDet(I).DateInp_Value2 = RptSubReportDet(I).DateInp_Value2
                '    f.RptSubReportDet(I).CboInp_Text1 = RptSubReportDet(I).CboInp_Text1
                '    f.RptSubReportDet(I).CboInp_Text2 = RptSubReportDet(I).CboInp_Text2
                '    f.RptSubReportDet(I).CboInp_Text3 = RptSubReportDet(I).CboInp_Text3
                '    f.RptSubReportDet(I).CboInp_Text4 = RptSubReportDet(I).CboInp_Text4
                '    f.RptSubReportDet(I).CboInp_Text5 = RptSubReportDet(I).CboInp_Text5

                '    For J = 1 To 10

                '        f.RptSubReportInpDet(I, J).PKey = RptSubReportInpDet(I, J).PKey
                '        f.RptSubReportInpDet(I, J).TableName = RptSubReportInpDet(I, J).TableName
                '        f.RptSubReportInpDet(I, J).Selection_FieldName = RptSubReportInpDet(I, J).Selection_FieldName
                '        f.RptSubReportInpDet(I, J).Return_FieldName = RptSubReportInpDet(I, J).Return_FieldName
                '        f.RptSubReportInpDet(I, J).Condition = RptSubReportInpDet(I, J).Condition
                '        f.RptSubReportInpDet(I, J).Display_Name = RptSubReportInpDet(I, J).Display_Name
                '        f.RptSubReportInpDet(I, J).BlankFieldCondition = RptSubReportInpDet(I, J).BlankFieldCondition
                '        f.RptSubReportInpDet(I, J).CtrlType_Cbo_OR_Txt = RptSubReportInpDet(I, J).CtrlType_Cbo_OR_Txt

                '    Next J

                'Next I

                'f.MdiParent = MDIParent1
                'f.Show()

                'f.msk_FromDate.Text = vDateInp1.ToShortDateString
                'f.msk_ToDate.Text = vDateInp2.ToShortDateString

                'f.cbo_Inputs1.Text = vCboInpText1
                'f.cbo_Inputs2.Text = vCboInpText2
                'f.cbo_Inputs3.Text = vCboInpText3
                'f.cbo_Inputs4.Text = vCboInpText4
                'f.cbo_Inputs5.Text = vCboInpText5

                'f.Show_Report()

                'If vCurRow > 0 Then
                '    If f.dgv_Report.Rows.Count > 0 And f.dgv_Report.Rows.Count >= vCurRow Then
                '        f.dgv_Report.CurrentCell = f.dgv_Report.Rows(vCurRow).Cells(0)
                '        f.dgv_Report.CurrentCell.Selected = True
                '    End If
                'End If
                'If vTopRow > 0 Then
                '    If f.dgv_Report.Rows.Count > 0 And f.dgv_Report.Rows.Count >= vTopRow Then
                '        f.dgv_Report.FirstDisplayedScrollingRowIndex = vTopRow
                '    End If
                'End If

            End If


        Catch ex As Exception

            '-----

        End Try

    End Sub

    Private Sub lbl_billrate_close_Click(sender As System.Object, e As System.EventArgs) Handles lbl_billrate_close.Click
        pnl_Bill_Rate.Visible = False
    End Sub


    Private Sub Bill_RateDetails()

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim OrdByNo As Single = 0
        Dim Led_id As Integer = 0
        Dim Item_id As Integer = 0, itemGp_id As Integer = 0
        Dim n As Integer, Sno As Integer

        Dim da3 As SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim vCmpSurNm As String = ""

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        'OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))
        ' Led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        Item_id = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
        '  itemGp_id = Common_Procedures.ItemGroup_NameToIdNo(con, cbo_itemGroup.Text)


        'vCmpSurNm = ""
        'da3 = New SqlClient.SqlDataAdapter("Select ch.company_Name from Company_Head ch where ch.company_idno = " & Str(Val(lbl_Company.Tag)), con)
        'dt3 = New DataTable
        'da3.Fill(dt3)
        'If dt3.Rows.Count > 0 Then
        '    vCmpSurNm = Common_Procedures.Remove_NonCharacters(dt3.Rows(0).Item("Company_Name").ToString)
        'End If
        'dt3.Clear()

        Try

            'Dim vbillrate As String = ""

            'vbillrate = ""

            da = New SqlClient.SqlDataAdapter("select top 1 a.Purchase_no, a.Purchase_Date,a.Rate , b.item_Name from purchase_details a INNER JOIN Item_Head b on b.item_idno = a.item_idno  where  a.Item_Idno =" & Str(Val(Item_id)) & " ", con) ' a.for_orderby < " & Str(Val(OrdByNo)) & " and a.Ledger_Idno =" & Str(Val(Led_id)) & " and  " & vbillrate & "   Order by a.for_Orderby desc, a.Sales_No desc", con)
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
                        .Rows(n).Cells(3).Value = dt.Rows(n).Item("item_Name").ToString
                        .Rows(n).Cells(4).Value = dt.Rows(n).Item("Rate").ToString

                    Next i

                End If
            End With

            dt.Clear()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Rate_Leave(sender As Object, e As EventArgs) Handles txt_Rate.Leave
        pnl_Bill_Rate.Visible = False

        pnl_ItemSet_Details.Visible = False
        pnl_Back.Enabled = True
    End Sub

    Private Function get_GSTPercentage_and_GSTAmount_For_Printing(ByVal EntryCode As String, ByRef vNOOFTAXLINES As Integer) As String
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vRETSTR As String = ""
        Dim S As String = ""
        Dim Nr As Long

        Cmd.Connection = con

        Cmd.CommandText = "Truncate table EntryTempSub"
        Nr = Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into EntryTempSub (Int1, Name1, Meters1, Currency1) select 1, 'CGST @', CGST_Percentage, CGST_Amount from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "' and CGST_Amount <> 0"
        Nr = Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into EntryTempSub (Int1, Name1, Meters1, Currency1) select 2, 'SGST @', SGST_Percentage, SGST_Amount from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "' and SGST_Amount <> 0"
        Nr = Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into EntryTempSub (Int1, Name1, Meters1, Currency1) select 3, 'IGST @', IGST_Percentage, IGST_Amount from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "' and IGST_Amount <> 0"
        Nr = Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Truncate table EntryTempSub "
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "Insert into EntryTempSub (Meters1, Currency1) select (CGST_Percentage+SGST_Percentage), (CGST_Amount+SGST_Amount) from Sales_GST_Tax_Details  Where Sales_Code = '" & Trim(EntryCode) & "' and (CGST_Amount+SGST_Amount) <> 0"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "Insert into EntryTempSub (Meters1, Currency1) select IGST_Percentage, IGST_Amount from Sales_GST_Tax_Details  Where Sales_Code = '" & Trim(EntryCode) & "' and IGST_Amount <> 0"
        'Cmd.ExecuteNonQuery()

        vRETSTR = ""
        vNOOFTAXLINES = 0
        Da = New SqlClient.SqlDataAdapter("Select Int1, Name1 as gsttaxcaption, Meters1 as gstperc, sum(Currency1) as gstamount from EntryTempSub Group by Int1, Name1, Meters1 Having sum(Currency1) <> 0 Order  by Meters1, Int1, Name1  ", con)
        'Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                If Val(Dt1.Rows(i).Item("gstamount").ToString) <> 0 Then

                    S = Trim(Dt1.Rows(i).Item("gsttaxcaption").ToString) & " " & Trim(Val(Dt1.Rows(i).Item("gstperc").ToString)) & " % " & "$^$" & Trim(Format(Val(Dt1.Rows(i).Item("gstamount").ToString), "##########0.00"))

                    vRETSTR = Trim(vRETSTR) & IIf(Trim(vRETSTR) <> "", "#^#", "") & Trim(S)
                    vNOOFTAXLINES = vNOOFTAXLINES + 1

                End If
            Next i
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da.Dispose()

        get_GSTPercentage_and_GSTAmount_For_Printing = Trim(vRETSTR)

    End Function


    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1481--" Then

            PpSzSTS = False

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                If ps.Width = 1000 And ps.Height = 600 Then
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    'MessageBox.Show("10x6 - ok")
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    If ps.Width = 800 And ps.Height = 600 Then
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        'MessageBox.Show("8x6 - ok")
                        Exit For
                    End If
                Next

            End If

            If PpSzSTS = False Then
                Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 10X6", 1000, 600)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
                PrintDocument1.DefaultPageSettings.Landscape = False
                'MessageBox.Show("10x6 - custom")
            End If

        Else

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If

    End Sub


    '----------***************** E invoice ************************

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
        grp_EInvoice.Visible = True
        grp_EInvoice.BringToFront()
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2
    End Sub
    Private Sub btn_CheckConnectivity1_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity1.Click
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GetAuthToken(rtbeInvoiceResponse)
    End Sub
    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Sales_Details Where Sales_Code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Sales_Head Where Sales_Code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) >0"
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



            Cmd.CommandText = "Insert into e_Invoice_Head (     e_Invoice_No     ,  e_Invoice_date , Buyer_IdNo,     Consignee_IdNo,          Assessable_Value  ,            CGST            , SGST            ,     IGST           ,   Cess,     State_Cess,         Round_Off         ,   Nett_Invoice_Value,           Ref_Sales_Code          ,                          Other_Charges            ,    Dispatcher_Idno       )" &
                              "Select                           Sales_No ,       Sales_Date,       Ledger_IdNo,        DeliveryTo_Idno,      Assessable_Value,          CGST_Amount,       SGST_Amount ,            IGST_Amount,          0   ,         0          ,         Round_Off    ,      Net_Amount          , '" & Trim(NewCode) & "',   ( ISNULL(TCS_Amount,0) + ISNULL(AddLess_Amount,0))  ,   Dispatcher_IdNo         from Sales_Head where Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()



            Cmd.CommandText = "Insert into e_Invoice_Details (Sl_No,        IsService,                  Product_Description         ,   HSN_Code,          Batch_Details,         Quantity,                   Unit ,             Unit_Price ,                                          Total_Amount,                                                                Discount             ,                                                Assessable_Amount,                                                                          GST_Rate   ,   SGST_Amount,      IGST_Amount,   CGST_Amount,        Cess_rate,       Cess_Amount,   CessNonAdvlAmount,       State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge,         Total_Item_Value,              AttributesDetails,                   Ref_Sales_Code )" &
                                        "            Select a.Sl_No,           0,               b.Item_Name as producDescription ,     a.HSN_Code,            ''          ,      a.Noof_Items  ,        c.Unit_Name  ,          a.Rate     ,      (a.Amount  + ( Case when a.sl_no = 1 then (sh.Freight_Amount ) else 0 end ))      ,  (case when a.sl_no  = 1  then sh.CashDiscount_Amount else 0 end )  ,    (a.Amount  + ( Case when a.sl_no = 1  then (sh.Freight_Amount - sh.CashDiscount_Amount  ) else 0 end )) ,         a.Tax_Perc,          0       ,         0       ,       0        ,         0       ,       0           ,        0         ,             0               ,0                 ,0                      ,0            ,         0 ,                               ''             ,    '" & Trim(NewCode) & "' " &
                                        " from Sales_Details a " &
                                        "inner join Sales_Head sh on sh.Sales_code = a.Sales_Code" &
                                        " inner join Item_Head b on a.Item_IdNo = b.Item_IdNo " &
                                        " inner join Unit_Head c on a.Unit_IdNo = c.Unit_IdNo  " &
                                         " Where a.Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()


            tr.Commit()


        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Sales_Head", "Sales_Code", Pk_Condition)

    End Sub

    Private Sub btn_Get_QR_Code_Click(sender As Object, e As EventArgs) Handles btn_Get_QR_Code.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GetIRNDetails(txt_eInvoiceNo.Text, NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Sales_Head", "Sales_Code", "INV")

    End Sub

    Private Sub btn_refresh_Click(sender As Object, e As EventArgs) Handles btn_refresh.Click


        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select E_Invoice_QR_Image,E_Invoice_IRNO,E_Invoice_ACK_No,E_Invoice_ACK_Date,E_Invoice_Cancelled_Status FROM  Sales_Head WHERE Sales_Code = '" & NewCode & "'", con)

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

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        If Len(Trim(txt_EInvoiceCancellationReson.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Sales_Head", "Sales_Code", con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)

    End Sub

    Private Sub btn_Generate_EWB_IRN_Click(sender As Object, e As EventArgs) Handles btn_Generate_EWB_IRN.Click
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Sales_Details Where Sales_Code = '" & NewCode & "'"
        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Sales_Head Where Sales_Code = '" & NewCode & "' and (Len(Electronic_Reference_No) >0 or Len(E_Invoice_IRNO) = 0 OR E_Invoice_IRNO IS NULL )"
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


            Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]                ,	[TransID]        ,	[TransMode] ,	[TransDocNo] ,[TransDocDate]  ,	    [VehicleNo]  ,              [Distance],                                                     	[VehType]  ,	[TransName]         , [InvCode]   ,    Company_Idno   , Company_Pincode  ,                            Shipped_To_Idno ,                                                              Shipped_To_Pincode )  " &
                                            " Select A.E_Invoice_IRNO  ,          t.Ledger_GSTINNo,        '1'    ,        a.LR_No   ,   a.LR_Date     ,       a.Vehicle_No     , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  D.Distance ELSE L.Distance END),      'R'    ,  t.Ledger_Name
                                            ,'" & NewCode & "' , a.Company_Idno  , tz.Company_Pincode  , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  a.DeliveryTo_IdNo ELSE a.Ledger_idno END) , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  d.Pincode ELSE L.Pincode END)   " &
                                                       " from Sales_Head a INNER JOIN Company_Head tz on tz.Company_idno = a.Company_Idno INNER JOIN Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D on a.DeliveryTo_IdNo = D.Ledger_IdNo LEFT OUTER JOIN Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo  Where a.Sales_Code = '" & NewCode & "'"

            Cmd.ExecuteNonQuery()


            'Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]                ,	[TransID]        ,	[TransMode] ,	[TransDocNo] ,[TransDocDate]  ,	    [VehicleNo]  ,              [Distance],                                                     	[VehType]  ,	[TransName]         , [InvCode]   ,    Company_Idno   , Company_Pincode  ,                            Shipped_To_Idno ,                                                              Shipped_To_Pincode )  " &
            '                                " Select A.E_Invoice_IRNO  ,          t.Ledger_GSTINNo,        '1'    ,        a.LR_No   ,   a.LR_Date     ,       a.Vehicle_No     , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  D.Distance ELSE L.Distance END),      'R'    ,  t.Transport_Name
            '                                ,'" & NewCode & "' , a.Company_Idno  , tz.Company_Pincode  , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  a.DeliveryTo_IdNo ELSE a.Ledger_idno END) , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  d.Pincode ELSE L.Pincode END)   " &
            '                                           " from Sales_Head a INNER JOIN Company_Head tz on tz.Company_idno = a.Company_Idno INNER JOIN Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D on a.DeliveryTo_IdNo = D.Ledger_IdNo LEFT OUTER JOIN Transport_Head T on a.Transport_IdNo = T.Transport_IdNo  Where a.Sales_Code = '" & NewCode & "'"

            'Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub
            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try


        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GenerateEWBByIRN(NewCode, rtbeInvoiceResponse, txt_eWayBill_No, txt_EWB_Date, txt_EWB_ValidUpto, con, "Sales_Head", "Sales_Code", txt_EWB_Canellation_Reason, txt_EWB_Cancel_Status, Pk_Condition, "Electronic_Reference_No")

        Cmd.CommandText = "DELETE FROM EWB_By_IRN WHERE INVCODE = '" & NewCode & "'"
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub btn_Print_EWB_Click(sender As Object, e As EventArgs) Handles btn_Print_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_Electronic_RefNo.Text, rtbeInvoiceResponse)
    End Sub

    Private Sub btn_Cancel_EWB_Click(sender As Object, e As EventArgs) Handles btn_Cancel_EWB.Click

        If Len(Trim(txt_EWB_Canellation_Reason.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))

        einv.Cancel_EWB_IRN(NewCode, txt_eWayBill_No.Text, rtbeInvoiceResponse, txt_EWB_Cancel_Status, con, "Sales_Head", "Sales_Code", txt_EWB_Canellation_Reason.Text)
    End Sub

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub
    Private Sub txt_eWayBill_No_TextChanged(sender As Object, e As EventArgs) Handles txt_eWayBill_No.TextChanged
        txt_Electronic_RefNo.Text = txt_eWayBill_No.Text
        txt_EWBNo.Text = txt_eWayBill_No.Text
    End Sub
    Private Sub txt_eInvoiceNo_TextChanged(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.TextChanged
        txt_IR_No.Text = txt_eInvoiceNo.Text
    End Sub
    Private Sub txt_IR_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_IR_No.KeyDown

        If e.KeyCode = 38 Then
            If txt_ExchangeRate.Visible Then
                txt_ExchangeRate.Focus()
            ElseIf cbo_Currency.Visible Then
                cbo_Currency.Focus()
            Else
                'cbo_TaxType.Focus()
            End If
        End If

        If e.KeyCode = 40 Then
            txt_SlNo.Focus()
        End If
    End Sub

    Private Sub txt_IR_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_IR_No.KeyPress

        If Asc(e.KeyChar) = 13 Then
            txt_SlNo.Focus()
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

            If Trim(cbo_Ledger.Text) <> "" Then

                Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

                If Led_ID <> 0 Then

                    GpCd = Common_Procedures.get_FieldValue(con, "ledger_head", "parent_code", "(ledger_idno = " & Str(Val(Led_ID)) & ")")
                    If GpCd Like "*~18~*" Then Datcondt = " and a.Voucher_date >= @companyfromdate " Else Datcondt = ""

                    cmd.CommandText = "select sum(a.Voucher_amount) as BalAmount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " " & Datcondt
                    da = New SqlClient.SqlDataAdapter(cmd)
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

                    lbl_CurrentBalance.Tag = n
                    lbl_CurrentBalance.Text = "Current Balance : " & Trim(Common_Procedures.Currency_Format(Math.Abs(Val(BalAmt)))) & IIf(Val(BalAmt) >= 0, " Cr", " Dr")

                Else

                    lbl_CurrentBalance.Tag = -100
                    lbl_CurrentBalance.Text = "Current Balance : "

                End If

            Else
                lbl_CurrentBalance.Tag = -100
                lbl_CurrentBalance.Text = "Current Balance : "

            End If

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE GETTI CURRENT BALANCE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub Cbo_Lot_No_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Lot_No.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_VehicleNo, txt_Trans_Freight, "LotNo_Head", "LotNo_Name", "Close_status = 0", "(LotNo_IdNo = 0)")
    End Sub
    Private Sub Cbo_Lot_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Lot_No.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "LotNo_Head", "LotNo_Name", "Close_status = 0", "(LotNo_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub
    Private Sub Cbo_Lot_No_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Lot_No.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LotNo_Head", "LotNo_Name", "Close_status = 0", "(LotNo_IdNo = 0)")
    End Sub
    Private Sub Cbo_Lot_No_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Lot_No.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LotNo_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Lot_No.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Get_Ledger_Details()

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim p1Font As Font
        Dim led_Name As String, LED_ID As String, led_Add1 As String, led_Add2 As String, led_Add3 As String, led_Add4 As String
        Dim led_PhNo As String, led_TinNo As String, Led_State As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Led_GSTTinNo As String, Cmp_GSTIN_No As String
        Dim strHeight As Single

        Try

            If Trim(cbo_Ledger.Text) <> "" Then

                LED_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

                da1 = New SqlClient.SqlDataAdapter("Select * from Ledger_HEad WHere Ledger_Idno = " & Val(LED_ID) & " ", con)
                dt1 = New DataTable
                da1.Fill(dt1)


                If dt1.Rows.Count > 0 Then

                    led_Name = "NAME :  " & dt1.Rows(0).Item("Ledger_Name").ToString
                    '    led_Name = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                    led_Add1 = "ADDRESS :  " & Trim(dt1.Rows(0).Item("Ledger_Address1").ToString)
                    led_Add2 = Trim(dt1.Rows(0).Item("Ledger_Address2").ToString)
                    led_Add3 = Trim(dt1.Rows(0).Item("Ledger_Address3").ToString)
                    led_Add4 = Trim(dt1.Rows(0).Item("Ledger_Address4").ToString)

                    '  led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                    If Trim(dt1.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then led_PhNo = "PH NO :  " & Trim(dt1.Rows(0).Item("Ledger_PhoneNo").ToString)

                    'Led_State = Trim(dt1.Rows(0).Item("Ledger_State_Name").ToString)
                    If Trim(dt1.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = "GSTIN :  " & Trim(dt1.Rows(0).Item("Ledger_GSTinNo").ToString)

                End If


                If Trim(led_Name) <> "" Then

                    txt_Ledger_Details.Text = Trim(led_Name) & IIf(txt_Ledger_Details.Text <> "", Environment.NewLine, Nothing) &
                    Trim(led_Add1) & IIf(txt_Ledger_Details.Text <> "" And Trim(led_Add1) <> "", Environment.NewLine, Nothing) &
                    Trim(led_Add2) & IIf(txt_Ledger_Details.Text <> "" And Trim(led_Add2) <> "", Environment.NewLine, Nothing) &
              Trim(led_Add3) & IIf(txt_Ledger_Details.Text <> "" And Trim(led_Add3) <> "", Environment.NewLine, Nothing) &
                Trim(led_Add4) & IIf(txt_Ledger_Details.Text <> "" And Trim(led_Add4) <> "", Environment.NewLine, "") &
                      Trim(Led_GSTTinNo) & IIf(txt_Ledger_Details.Text <> "" And Trim(Led_GSTTinNo) <> "", Environment.NewLine, Nothing) &
                 Trim(led_PhNo) & IIf(txt_Ledger_Details.Text <> "" And Trim(led_PhNo) <> "", Environment.NewLine, Nothing)

                End If


            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR ON GET LEDEGR DETAILS", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub
    Private Sub btn_Led_Details_Close_Click(sender As Object, e As EventArgs) Handles btn_Led_Details_Close.Click
        Pnl_ledger_Deatils.Visible = False
    End Sub
    Private Sub cbo_Ledger_Leave(sender As Object, e As EventArgs) Handles cbo_Ledger.Leave
        If Trim(Common_Procedures.settings.CustomerCode) = "1107" Then
            Get_Ledger_Details()
            btn_Led_Details_Close_Click(sender, e)
        End If
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            Get_TCS_Sts_From_Ledger_Name()
        End If
    End Sub

    Private Sub cbo_Ledger_TextChanged(sender As Object, e As EventArgs) Handles cbo_Ledger.TextChanged
        If Trim(Common_Procedures.settings.CustomerCode) = "1107" Then
            Get_Ledger_Details()
        End If

    End Sub

    Private Sub btn_Selection_Click(sender As Object, e As EventArgs)
        If Trim(Common_Procedures.settings.CustomerCode) = "1107" Then ' --- GAJAKARNA 
            Pnl_ledger_Deatils.Visible = True
            Pnl_ledger_Deatils.Location = New Point(10, 328)
        End If
        'If cbo_EntType.Visible And cbo_EntType.Enabled And Trim(UCase(cbo_EntType.Text)) <> Trim(UCase("DIRECT")) Then
        '    ' If Trim(Common_Procedures.settings.CustomerCode) = "1545" Then ' ---- BAGAVAN TEX
        '    Invoice_Selection()
        '    '  End If
        'End If
    End Sub
    Private Sub txt_GSTRate_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_GSTRate.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then

            txt_Rate.Focus()

        End If
        If e.KeyValue = 40 Then

            txt_Order_No.Focus()

            'txt_Container_No.Focus()
        End If
    End Sub

    Private Sub txt_GSTRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GSTRate.KeyPress
        If Asc(e.KeyChar) = 13 Then

            'txt_Container_No.Focus()

            txt_Order_No.Focus()

        End If
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub GST_Rate_Calculation()
        If chk_GSTTax_Invocie.Checked = True Then
            txt_GSTRate.Text = Format(Val(txt_Rate.Text) + (Val(txt_Rate.Text) * Val(lbl_Grid_GstPerc.Text) / 100), "#########0.00")
        Else
            txt_GSTRate.Text = Format(Val(txt_Rate.Text), "#########0.00")
        End If
    End Sub
    Private Sub Rate_Calculation_from_GSTRATE()
        If chk_GSTTax_Invocie.Checked = True Then
            'If Trim(UCase(cbo_TaxType.Text)) = Trim(UCase("GST")) Then
            txt_Rate.Text = Format((Val(txt_GSTRate.Text)) - (Val(txt_GSTRate.Text) * (Val(lbl_Grid_GstPerc.Text) / (100 + Val(lbl_Grid_GstPerc.Text)))), "###########0.00")
        Else
            txt_Rate.Text = Format(Val(txt_GSTRate.Text), "###########0.00")
        End If

    End Sub
    Private Sub txt_GSTRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTRate.KeyUp
        Rate_Calculation_from_GSTRATE()
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        GST_Rate_Calculation()
    End Sub


    Private Sub btn_Close_pnl_ItemSet_Details_Click(sender As Object, e As EventArgs) Handles btn_Close_pnl_ItemSet_Details.Click

        Move_Data_From_DgvGrid()
    End Sub

    Private Sub dgtxt_ItemSet_Details_Enter(sender As Object, e As System.EventArgs) Handles dgtxt_ItemSet_Details.Enter
        dgv_ItemSet_Details.EditingControl.BackColor = Color.Lime
        dgv_ItemSet_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_ItemSet_Details.SelectAll()

    End Sub


    Private Sub dgtxt_ItemSet_Details_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_ItemSet_Details.KeyPress
        With dgv_ItemSet_Details
            If .Visible Then
                If .Rows.Count > 0 Then

                    If .CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.RATE Or .CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.GST_RATE Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If


                    End If
                End If
            End If
        End With
    End Sub
    Private Sub dgtxt_ItemSet_Details_TextChanged(sender As Object, e As System.EventArgs) Handles dgtxt_ItemSet_Details.TextChanged
        Try
            With dgv_ItemSet_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_ItemSet_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgv_ItemSet_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_ItemSet_Details.EditingControlShowing
        dgtxt_ItemSet_Details = Nothing
        If dgv_ItemSet_Details.CurrentCell.ColumnIndex > DgvCol_ItemSet_Details.SLNO Then
            dgtxt_ItemSet_Details = CType(dgv_ItemSet_Details.EditingControl, DataGridViewTextBoxEditingControl)
        End If
    End Sub
    Private Sub Add_Data_From_Input_Controls_to_DgvGrid() ' --- ITEM SET DETAILS 

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim Led_IdNo As Integer = 0
        Dim MtchSTS As Boolean
        Dim itm_id As Integer
        Dim unt_id As Integer
        Dim vNo_Of_SET As Integer
        Dim gstrate As Single = 0

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub



        If Trim(cbo_ItemName.Text) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            pnl_ItemSet_Details.Visible = False
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If


        itm_id = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_ItemName.Text)
        If itm_id = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            pnl_ItemSet_Details.Visible = False
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        vNo_Of_SET = Val(txt_NoofItems.Text)
        If Val(vNo_Of_SET) = 0 Then
            MessageBox.Show("Invalid Quantity", "DOES NOT ADD...", MessageBoxButtons.OK)
            pnl_ItemSet_Details.Visible = False
            If txt_NoofItems.Enabled Then txt_NoofItems.Focus()
            Exit Sub
        End If

        Dim vHSN_Code_FieldName = ""
        Dim vGSt_Perc_FieldName = ""

        'If Common_Procedures.settings.Item_Creation_Wise_Get_HSNCode_GST_Perc_STS = 1 Then

        '    vHSN_Code_FieldName = "B.Hsn_Code"
        '    vGSt_Perc_FieldName = "B.Gst_Percentage"
        'Else
        '    vHSN_Code_FieldName = "c.Item_HSN_Code"
        '    vGSt_Perc_FieldName = "c.Item_GST_Percentage"

        'End If


        With dgv_ItemSet_Details

            da = New SqlClient.SqlDataAdapter("   Select A.SL_NO,b.ITEM_NAME,D.Unit_Name,A.Qty," & vHSN_Code_FieldName & " as HSN_CODE, " & vGSt_Perc_FieldName & " as GST_TAX ,b.Sales_Rate ,A.ITEM_IDNO " &
                                                " from Item_Sub_Details a  " &
                                                " INNER JOIN itemgroup_head B On A.Sub_Item_Idno =B.ITEM_IDNO   " &
                                                " INNER JOIN ITEMGROUP_HEAD C On B.ITEMGROUP_idno=c.ITEMGROUP_idno " &
                                                " LEFT OUTER JOIN Unit_Head D On B.Unit_IdNo =D.Unit_IdNo   " &
                                                " where a.item_idno=" & Val(itm_id) & "  And a.item_idno <> 0 ", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                .Rows.Clear()

                For i = 0 To dt.Rows.Count - 1
                    n = .Rows.Add()
                    .Rows(n).Cells(DgvCol_ItemSet_Details.SLNO).Value = dt.Rows(i).Item("SL_NO").ToString
                    .Rows(n).Cells(DgvCol_ItemSet_Details.ITEM_NAME).Value = dt.Rows(i).Item("ITEM_NAME").ToString
                    .Rows(n).Cells(DgvCol_ItemSet_Details.UNIT).Value = dt.Rows(i).Item("Unit_Name").ToString


                    .Rows(n).Cells(DgvCol_ItemSet_Details.QTY).Value = Format(Val(vNo_Of_SET) * dt.Rows(i).Item("Qty").ToString, "########0")
                    .Rows(n).Cells(DgvCol_ItemSet_Details.HSN_CODE).Value = dt.Rows(i).Item("HSN_CODE").ToString
                    .Rows(n).Cells(DgvCol_ItemSet_Details.GST_PERCEN).Value = dt.Rows(i).Item("GST_TAX").ToString
                    .Rows(n).Cells(DgvCol_ItemSet_Details.SET_ITEM_IDNO).Value = dt.Rows(i).Item("ITEM_IDNO").ToString

                    dgv_ItemSet_Details.Rows(n).Cells(DgvCol_ItemSet_Details.RATE).Value = Val(dt.Rows(i).Item("Sales_Rate").ToString)

                    gstrate = 0
                    If Val(dgv_ItemSet_Details.Rows(n).Cells(DgvCol_ItemSet_Details.GST_PERCEN).Value) <> 0 Then
                        gstrate = dt.Rows(i).Item("Sales_Rate").ToString * (dgv_ItemSet_Details.Rows(n).Cells(DgvCol_ItemSet_Details.GST_PERCEN).Value / 100)
                    End If

                    dgv_ItemSet_Details.Rows(n).Cells(DgvCol_ItemSet_Details.GST_RATE).Value = Format(Val(dt.Rows(i).Item("Sales_Rate").ToString + gstrate), "#######0.00")

                    If Val(dgv_ItemSet_Details.Rows(n).Cells(DgvCol_ItemSet_Details.GST_RATE).Value) <> 0 And Val(dgv_ItemSet_Details.Rows(n).Cells(DgvCol_ItemSet_Details.RATE).Value) = 0 Then

                        Grid_Rate_Calculation_from_GSTRATE(n)
                    Else
                        Grid_GST_Rate_Calculation(n)
                    End If


                Next

            End If
        End With

        pnl_ItemSet_Details.Visible = True
        dgv_ItemSet_Details.Focus()
        dgv_ItemSet_Details.CurrentCell = dgv_ItemSet_Details.Rows(0).Cells(DgvCol_ItemSet_Details.GST_RATE)

        da.Dispose()
        dt.Clear()


        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_SerialNo.Text = ""

        txt_Package_No.Text = ""
        txt_Net_wgt.Text = ""
        txt_Gross_Wgt.Text = ""

        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_GSTRate.Text = ""
        lbl_Amount.Text = ""
        Cbo_Lot_No.Text = ""
        '***** GST START *****
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""


    End Sub
    Private Sub dgv_ItemSet_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ItemSet_Details.CellLeave

        With dgv_ItemSet_Details

            If .CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.RATE Or .CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.GST_RATE Or .CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.AMOUNT Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")

                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""

                End If

            End If

        End With

    End Sub
    Private Sub dgv_ItemSet_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ItemSet_Details.CellValueChanged

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

            If Not IsNothing(dgv_ItemSet_Details.CurrentCell) Then

                With dgv_ItemSet_Details
                    If .Visible = True Then


                        If .CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.RATE Or .CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.GST_RATE Or .CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.QTY Then

                            If dgv_ItemSet_Details.CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.GST_RATE Then
                                Grid_Rate_Calculation_from_GSTRATE(e.RowIndex)
                            ElseIf dgv_ItemSet_Details.CurrentCell.ColumnIndex = DgvCol_ItemSet_Details.RATE Then
                                Grid_GST_Rate_Calculation(e.RowIndex)

                            End If

                            If Val(.Rows(e.RowIndex).Cells(DgvCol_ItemSet_Details.RATE).Value) <> 0 Then
                                .Rows(e.RowIndex).Cells(DgvCol_ItemSet_Details.AMOUNT).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_ItemSet_Details.QTY).Value) * Val(.Rows(e.RowIndex).Cells(DgvCol_ItemSet_Details.RATE).Value), "#########0.00")
                            End If

                        End If


                    End If

                End With
            End If


        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "Error While DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "Error While DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Grid_Rate_Calculation_from_GSTRATE(ByVal vROWNO As Integer)
        With dgv_ItemSet_Details

            If chk_GSTTax_Invocie.Checked = True Then
                dgv_ItemSet_Details.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.RATE).Value = Format((Val(.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.GST_RATE).Value)) - (Val(.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.GST_RATE).Value) * (Val(.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.GST_PERCEN).Value) / (100 + Val(.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.GST_PERCEN).Value)))), "###########0.00")
            Else
                .Rows(vROWNO).Cells(DgvCol_ItemSet_Details.RATE).Value = Format(Val(.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.GST_RATE).Value), "###########0.00")
            End If
        End With
    End Sub
    Private Sub Grid_GST_Rate_Calculation(ByVal vROWNO As Integer)
        With dgv_ItemSet_Details

            If chk_GSTTax_Invocie.Checked = True Then
                'If Trim(UCase(cbo_TaxType.Text)) = Trim(UCase("GST")) Then
                .Rows(vROWNO).Cells(DgvCol_ItemSet_Details.GST_RATE).Value = Format(Val(.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.RATE).Value) + (Val(.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.RATE).Value) * Val(.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.GST_PERCEN).Value) / 100), "#########0.00")
            Else
                .Rows(vROWNO).Cells(DgvCol_ItemSet_Details.GST_RATE).Value = Format(Val(.Rows(vROWNO).Cells(DgvCol_ItemSet_Details.RATE).Value), "#########0.00")
            End If
        End With

    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim I As Integer = 0
        Dim dgv1 As New DataGridView
        Dim Stck As String = ""

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing


            If ActiveControl.Name = dgv_ItemSet_Details.Name Then
                dgv1 = dgv_ItemSet_Details

            ElseIf pnl_ItemSet_Details.Visible = True Then
                dgv1 = dgv_ItemSet_Details

            Else

            End If


            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_ItemSet_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= DgvCol_ItemSet_Details.GST_RATE Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    Move_Data_From_DgvGrid()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DgvCol_ItemSet_Details.GST_RATE)

                                End If

                            Else
                                ' .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DgvCol_ItemSet_Details.GST_RATE)

                            End If


                            Return True


                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= DgvCol_ItemSet_Details.GST_RATE Then
                                If .CurrentCell.RowIndex = 0 Then
                                    Move_Data_From_DgvGrid()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_ItemSet_Details.GST_RATE)
                                End If


                            Else
                                '.CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_ItemSet_Details.GST_RATE)


                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If


                    ElseIf dgv1.Name = dgv_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex = DgvCol_Details.RATE_QTY Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    txt_CashDiscPerc.Focus()
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DgvCol_Details.QUANTITY)
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                '   txt_CashDiscPerc.Focus()

                            End If


                            Return True


                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex = DgvCol_Details.QUANTITY Then
                                If .CurrentCell.RowIndex = 0 Then
                                    'cbo_TaxType.Focus()
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_Details.QUANTITY)
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    End If
                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function
    Private Sub Move_Data_From_DgvGrid()

        Dim i As Integer
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand

        Dim n As Integer
        Dim MtchSTS As Boolean = False
        Dim vSET_Item_First_Name As String = ""
        Dim vSET_Item_ID As String = 0
        Dim vSET_Item_DGV_SLNo As String = 0
        Dim vSET_Item_Count As String = 0

        With dgv_ItemSet_Details



            da = New SqlClient.SqlDataAdapter("    Select b.Item_Name,  A.item_Idno,a.Qty,a.Sub_Item_Idno from  Item_Sub_Details a  " &
                                                " INNER JOIN itemgroup_head B On A.Sub_Item_Idno =B.ITEM_IDNO  " &
                                                " INNER JOIN ITEMGROUP_HEAD C On B.ITEMGROUP_idno=c.ITEMGROUP_idno    " &
                                                " where a.item_idno = " & Val(dgv_ItemSet_Details.Rows(0).Cells(DgvCol_ItemSet_Details.SET_ITEM_IDNO).Value) & " ", con)

            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                For k = 0 To dt.Rows.Count - 1
                    For j = 0 To dgv_Details.Rows.Count - 1

                        vSET_Item_First_Name = dt.Rows(k)(0).ToString
                        vSET_Item_ID = dt.Rows(k)(1).ToString

                        '  If  Val(dgv_Details.Rows(j).Cells(14).Value) = Val(vSET_Item_ID) Then
                        If Trim(dgv_Details.Rows(j).Cells(DgvCol_Details.ITEM_NAME).Value) = Trim(vSET_Item_First_Name) And Val(dgv_Details.Rows(j).Cells(DgvCol_Details.ITEM_SET_IDNO).Value) = Val(vSET_Item_ID) Then
                            vSET_Item_DGV_SLNo = j

                            dgv_Details.Rows.RemoveAt(vSET_Item_DGV_SLNo)
                            Exit For
                        End If
                    Next
                Next
            End If
            For i = 0 To .Rows.Count - 1

                If MtchSTS = False Then

                    n = dgv_Details.Rows.Add()

                    dgv_Details.Rows(n).Cells(DgvCol_Details.SL_NO).Value = .Rows(i).Cells(DgvCol_ItemSet_Details.SLNO).Value
                    dgv_Details.Rows(n).Cells(DgvCol_Details.ITEM_NAME).Value = .Rows(i).Cells(DgvCol_ItemSet_Details.ITEM_NAME).Value
                    dgv_Details.Rows(n).Cells(DgvCol_Details.UNIT).Value = .Rows(i).Cells(DgvCol_ItemSet_Details.UNIT).Value

                    dgv_Details.Rows(n).Cells(DgvCol_Details.QUANTITY).Value = Val(.Rows(i).Cells(DgvCol_ItemSet_Details.QTY).Value)
                    dgv_Details.Rows(n).Cells(DgvCol_Details.RATE_QTY).Value = .Rows(i).Cells(DgvCol_ItemSet_Details.RATE).Value
                    dgv_Details.Rows(n).Cells(DgvCol_Details.AMOUNT).Value = .Rows(i).Cells(DgvCol_ItemSet_Details.AMOUNT).Value

                    dgv_Details.Rows(n).Cells(DgvCol_Details.HSN_CODE).Value = .Rows(i).Cells(DgvCol_ItemSet_Details.HSN_CODE).Value
                    dgv_Details.Rows(n).Cells(DgvCol_Details.GST_PERC).Value = .Rows(i).Cells(DgvCol_ItemSet_Details.GST_PERCEN).Value

                    dgv_Details.Rows(n).Cells(DgvCol_Details.GST_RATE).Value = .Rows(i).Cells(DgvCol_ItemSet_Details.GST_RATE).Value

                    dgv_Details.Rows(n).Cells(DgvCol_Details.ITEM_SET_IDNO).Value = .Rows(i).Cells(DgvCol_ItemSet_Details.SET_ITEM_IDNO).Value

                End If

            Next




        End With
        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_SerialNo.Text = ""

        txt_Package_No.Text = ""
        txt_Net_wgt.Text = ""
        txt_Gross_Wgt.Text = ""

        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_GSTRate.Text = ""
        lbl_Amount.Text = ""
        Cbo_Lot_No.Text = ""
        '***** GST START *****
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""


        pnl_ItemSet_Details.Visible = False
        pnl_Back.Enabled = True
        cbo_ItemName.Focus()

    End Sub
    Private Sub Move_Data_From_DgvGrid_to_Input_Controls(ByVal vROW As Integer)
        Dim i As Integer
        Dim da As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim Led_IdNo As Integer = 0
        Dim MtchSTS As Boolean
        Dim itm_id As Integer
        Dim unt_id As Integer
        Dim z As Integer
        Dim gstrate As Single = 0
        Dim Ent_Rate As String = 0
        Dim NewCode As String = 0
        Dim vROWNO As Integer = 0
        Dim vSET_Item_First_Name As String = ""
        Dim vSET_Item_ID As String = 0
        Dim vSET_Item_DGV_SLNo As String = 0
        Dim vSET_Item_Count As String = 0


        NewCode = Trim(Trim(Pk_Condition) & Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        Dim vHSN_Code_FieldName = ""
        Dim vGSt_Perc_FieldName = ""

        'If Common_Procedures.settings.Item_Creation_Wise_Get_HSNCode_GST_Perc_STS = 1 Then

        '    vHSN_Code_FieldName = "B.Hsn_Code"
        '    vGSt_Perc_FieldName = "B.Gst_Percentage"
        'Else
        '    vHSN_Code_FieldName = "c.Item_HSN_Code"
        '    vGSt_Perc_FieldName = "c.Item_GST_Percentage"

        'End If


        If Val(vROW) <= dgv_Details.Rows.Count - 1 Then

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "TRUNCATE TABLE REPORTTEMPSUB"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "TRUNCATE TABLE ReportTemp"
            cmd.ExecuteNonQuery()

            cmd.CommandText = " INSERT INTO REPORTTEMPSUB (  INT1,    INT2  ,      INT3    	,    Name2	   ,	        	 Name3		,		 WEIGHT2		     ,		 Name4          ,Weight7 ) " &
                                " Select             a.SL_NO,  A.Sub_Item_Idno, A.ITEM_IDNO   ,B.ITEM_NAME ," & vHSN_Code_FieldName & " , " & vGSt_Perc_FieldName & ",       D.UNIT_nAME ,sum(a.Qty)   FROM  Item_Sub_Details A  " &
                                " INNER JOIN ITEM_HEAD B On (A.Sub_Item_Idno<> 0) And A.Sub_Item_Idno =B.ITEM_IDNO  " &
                                " INNER Join ITEMGROUP_HEAD C On B.ITEMGROUP_idno=c.ITEMGROUP_idno   " &
                                " INNER JOIN Unit_Head D On B.UNIT_IDNO=D.Unit_IdNo   " &
                                " WHERE A.ITEM_IDNO =" & Val(dgv_Details.Rows(vROW).Cells(DgvCol_Details.ITEM_SET_IDNO).Value) & " " &
                                " GROUP BY a.SL_NO,  A.ITEM_IDNO   , A.Sub_Item_Idno ,B.ITEM_NAME ," & vHSN_Code_FieldName & ", " & vGSt_Perc_FieldName & ",	D.UNIT_nAME   order by a.SL_NO  "
            cmd.ExecuteNonQuery()


            cmd.CommandText = " INSERT INTO REPORTTEMPSUB ( INT1,    INT2  ,			 INT3    	,    Name2	  ,		 Name3		,		 WEIGHT2		,		 Name4  ,			WEIGHT3		,				 Weight5		 ,		Weight6				  , Weight8		,NAME5)  " &
                            " Select					a.SL_NO,   A.ITEM_IDNO   , A.Item_Set_IdNo , B.ITEM_NAME ," & vHSN_Code_FieldName & ", " & vGSt_Perc_FieldName & ",	D.UNIT_nAME ,	SUM(A.RATE) As RATE  , SUM(A.RateWithTax) As RATE_GST , SUM(A.Amount) As AMOUNT , SUM(A.Noof_Items) As QTY ,A.Sales_Code FROM  Sales_Details A  " &
                            " INNER JOIN ITEM_HEAD B On (A.Item_Set_IdNo<> 0) And A.ITEM_IDNO =B.ITEM_IDNO    " &
                            " INNER JOIN ITEMGROUP_HEAD C On B.ITEMGROUP_idno=c.ITEMGROUP_idno   " &
                            " INNER JOIN Unit_Head D On B.UNIT_IDNO=D.Unit_IdNo   " &
                            " INNER JOIN  REPORTTEMPSUB RTMPS On A.Item_Set_IdNo =  RTMPS.INT3 And A.ITEM_IDNO =  RTMPS.INT2 " &
                            " WHERE A.Item_Set_IdNo =" & Val(dgv_Details.Rows(vROW).Cells(DgvCol_Details.ITEM_SET_IDNO).Value) & " And a.sales_code ='" & Trim(NewCode) & "' " &
                            " GROUP BY  a.SL_NO,    A.ITEM_IDNO   , A.Item_Set_IdNo , B.ITEM_NAME ," & vHSN_Code_FieldName & ", " & vGSt_Perc_FieldName & ",	D.UNIT_nAME ,A.Sales_Code  order by a.SL_NO  "
            cmd.ExecuteNonQuery()


            cmd.CommandText = " iNSERT INTO REPORTTEMP (       INT1,    INT2  ,			 INT3    	,    Name2	  ,		 Name3		,		 WEIGHT2		,		 Name4  ,		                                        	WEIGHT3		,				                                 Weight5		 ,	                                    	Weight6				                    ,                                        Weight7)   " &
                                                    "SELECT		INT1 , INT2  ,			 INT3    	,    Name2	  ,		 Name3		,		 WEIGHT2		,		 Name4  ,			 ( case when  WEIGHT3 <> 0 then weight3 else 0 end )		,		( case when  Weight5 <> 0 then Weight5 else 0 end )		 ,		( case when  Weight6 <> 0 then Weight6 else 0 end )				  , ( case when  Weight8 <> 0 then Weight8 else Weight7 end )	  " &
                                                    " FROM REPORTTEMPSUB   where Int3 =" & Val(dgv_Details.Rows(vROW).Cells(DgvCol_Details.ITEM_SET_IDNO).Value) & "  GROUP BY INT1,  INT2  ,			 INT3    	,    Name2	  ,		 Name3		,		 WEIGHT2		,		 Name4  ,			WEIGHT3		,				 Weight5		 ,		Weight6				  , Weight7  , Weight8   ORDER BY  INT1 "
            cmd.ExecuteNonQuery()


            da = New SqlClient.SqlDataAdapter("SELECT  NAME2 as Item_Name , NAME4 as Unit_Name , NAME3 as HSn_Code, WEIGHT2 as GSt_Perc, Weight7 as QTY ,WEIGHT3 as Rate,  Weight5 As GSt_Rate ,Weight6 as Amount ,INT3 as Set_Item_Idno FROM ReportTemp where   Weight7 <> 0  and  WEIGHT3 <> 0 and    Weight5 <> 0 and Weight6 <> 0", con)
            da.Fill(dt)
            If dt.Rows.Count = 0 Then
                da = New SqlClient.SqlDataAdapter("SELECT  NAME2 as Item_Name , NAME4 as Unit_Name , NAME3 as HSn_Code, WEIGHT2 as GSt_Perc, Weight7 as QTY ,WEIGHT3 as Rate,  Weight5 As GSt_Rate ,Weight6 as Amount ,INT3 as Set_Item_Idno FROM ReportTemp  where   Weight7 <>0  and  WEIGHT3 = 0 and    Weight5 = 0 and Weight6 = 0", con)
                da.Fill(dt)
            End If

            If dt.Rows.Count > 0 Then

                With dgv_Details

                    dgv_ItemSet_Details.Rows.Clear()

                    For vROWNO = 0 To dt.Rows.Count - 1

                        i = dgv_ItemSet_Details.Rows.Add()

                        Sno = Sno + 1

                        dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.SLNO).Value = Val(Sno)
                        dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.ITEM_NAME).Value = dt.Rows(vROWNO).Item("ITEM_NAME").ToString
                        dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.UNIT).Value = dt.Rows(vROWNO).Item("Unit_Name").ToString
                        If Val(txt_NoofItems.Text) <> 0 Then
                            dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.QTY).Value = Format(Val(txt_NoofItems.Text) * Val(dt.Rows(vROWNO).Item("QTY").ToString), "########0")
                        Else
                            dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.QTY).Value = Format(Val(dt.Rows(vROWNO).Item("QTY").ToString), "########0")
                        End If
                        'dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.QTY).Value = Format(Val(.Rows(vROWNO).Cells(4).Value), "########0.00")
                        dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.RATE).Value = Format(Val(dt.Rows(vROWNO).Item("Rate").ToString), "########0.00")
                        dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.AMOUNT).Value = Format(Val(dt.Rows(vROWNO).Item("amount").ToString), "########0.00")

                        dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.HSN_CODE).Value = dt.Rows(vROWNO).Item("HSn_Code").ToString
                        dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.GST_PERCEN).Value = Format(Val(dt.Rows(vROWNO).Item("GSt_Perc").ToString), "########0.00")
                        dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.GST_RATE).Value = Format(Val(dt.Rows(vROWNO).Item("GSt_Rate").ToString), "########0.00")

                        dgv_ItemSet_Details.Rows(i).Cells(DgvCol_ItemSet_Details.SET_ITEM_IDNO).Value = dt.Rows(vROWNO).Item("Set_Item_Idno").ToString

                    Next
                End With

            End If

            pnl_ItemSet_Details.Visible = True
            dgv_ItemSet_Details.Focus()
            dgv_ItemSet_Details.CurrentCell = dgv_ItemSet_Details.Rows(0).Cells(DgvCol_ItemSet_Details.GST_RATE)

        End If
    End Sub
    Private Sub Check_Item_In_Item_Set_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim itm_id As Integer

        vItem_Set_Details_STS = False

        If Trim(cbo_ItemName.Text) <> "" Then

            itm_id = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_ItemName.Text)
            If itm_id = 0 Then
                MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
                pnl_ItemSet_Details.Visible = False
                If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
                Exit Sub
            End If
            da = New SqlClient.SqlDataAdapter(" Select * From   Item_Sub_Details  where item_idno=" & Val(itm_id) & "   ", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                vItem_Set_Details_STS = True
            End If
        ElseIf Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(DgvCol_Details.ITEM_SET_IDNO).Value) <> 0 And vDgv_Double_Click_STS = True Then

            da = New SqlClient.SqlDataAdapter(" Select * From   Item_Sub_Details  where  item_idno= " & Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(DgvCol_Details.ITEM_SET_IDNO).Value) & "  ", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                vItem_Set_Details_STS = True
            End If

        End If
        da.Dispose()
        dt.Clear()


    End Sub
    Private Sub txt_Rate_Enter(sender As Object, e As EventArgs) Handles txt_Rate.Enter
        If Trim(cbo_ItemName.Text) <> "" Then
            'Check_Item_In_Item_Set_Details()

            If vItem_Set_Details_STS = True Then
                Add_Data_From_Input_Controls_to_DgvGrid()
            End If
        End If
    End Sub
    Private Sub btn_EWB_Click(sender As Object, e As EventArgs) Handles btn_EWB.Click
        rtbEWBResponse.Text = ""
        txt_EWBNo.Text = txt_Electronic_RefNo.Text
        Grp_EWB.Visible = True


        Grp_EWB.Left = (Me.Width - Grp_EWB.Width) / 2
        Grp_EWB.Top = (Me.Height - Grp_EWB.Height) / 2

        Grp_EWB.BringToFront()
    End Sub
    Private Sub Btn_EWB_Close_Click(sender As Object, e As EventArgs) Handles Btn_EWB_Close.Click
        Grp_EWB.Visible = False
    End Sub
    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select Electronic_Reference_No from Sales_Head where Sales_Code = '" & NewCode & "'", con)
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

        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]   , [Distance]          ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode] , [ShippedToGSTIN], [ShippedToTradeName] ) " &
                         " " &
                         " " &
                         "  SELECT               'O'              , '1'             ,   ''              ,    'INV'    , Sales_No ,a.Sales_date          , C.Company_GSTINNo, C.Company_Name   , (case when a.Dispatcher_Idno <> 0 then ( DISFRM.Ledger_Address1 + ',' + DISFRM.Ledger_Address2 ) else ( C.Company_Address1 + ',' + c.Company_Address2 ) end ), (case when a.Dispatcher_Idno <> 0 then ( DISFRM.Ledger_Address3 + ',' + DISFRM.Ledger_Address4 ) else ( c.Company_Address3 + ',' + C.Company_Address4 ) end ) , (case when a.Dispatcher_Idno <> 0 then DISFRM.City_Town else  c.Company_City end ) ," &
                         "  (case when a.Dispatcher_Idno <> 0 then DISFRM.Pincode else C.Company_Pincode  end )    ,  (case when a.Dispatcher_Idno <> 0 then DISFRMSTA.State_Code  else FS.State_Code end )  ,(case when a.Dispatcher_Idno <> 0 then DISFRMSTA.State_Code  else FS.State_Code end )    ,L.Ledger_GSTINNo  ,L.Ledger_Name, ( L.Ledger_Address1 + ',' + L.Ledger_Address2 )   , ( L.Ledger_Address3 + ',' + L.Ledger_Address4 ) , L.City_Town , L.Pincode , TS.State_Code,TS.State_Code," &
                         " 1                     , a.Round_Off, A.Assessable_Value    , A.CGST_Amount  ,  A.SGST_Amount , A.IGST_Amount   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Transport_Name ," &
                         " '' as LR_No        ,'' as LR_Date            ,a.Net_Amount         ,     CASE    WHEN a.Transportation_Mode = 'Rail' THEN '2'  WHEN a.Transportation_Mode = 'Air' THEN '3'  WHEN a.Transportation_Mode = 'Ship' THEN '4'    ELSE '1' END AS TrMode , L.Distance , " &
                         " a.Vehicle_No,'R','" & NewCode & "' , tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from Sales_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo left Outer Join Transport_Head T on a.Transport_IdNo = T.Transport_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo Left Outer Join Ledger_Head DISFRM  on a.Dispatcher_Idno <> 0 and a.Dispatcher_Idno = DISFRM.Ledger_IdNo   Left Outer Join State_Head DISFRMSTA On  DISFRM.State_IdNo = DISFRMSTA.State_IdNo   Left Outer Join State_Head FS On " &
                         " C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.State_IdNo = TS.State_IdNo  where a.Sales_Code = '" & NewCode & "'"

        CMD.ExecuteNonQuery()

        'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
        '                 "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
        '                 "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
        '                 "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]   , [Distance]          ," &
        '                 "[VehicleNo]      ,[VehicleType]   , [InvCode] , [ShippedToGSTIN], [ShippedToTradeName] ) " &
        '                 " " &
        '                 " " &
        '                 "  SELECT               'O'              , '1'             ,   ''              ,    'INV'    , '" & Trim(lbl_InvoiceNo.Text) & "',a.Sales_date          , C.Company_GSTINNo, C.Company_Name   , ( C.Company_Address1 + ',' + c.Company_Address2 ) , ( c.Company_Address3 + ',' + C.Company_Address4 ) , c.Company_City ," &
        '                 " C.Company_Pincode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo  ,L.Ledger_Name, ( L.Ledger_Address1 + ',' + L.Ledger_Address2 )   , ( L.Ledger_Address3 + ',' + L.Ledger_Address4 ) , L.City_Town , L.Pincode , TS.State_Code,TS.State_Code," &
        '                 " 1                     , a.Round_Off, A.Assessable_Value    , A.CGST_Amount  ,  A.SGST_Amount , A.IGST_Amount   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Transport_Name ," &
        '                 " '' as LR_No        ,'' as LR_Date            ,a.Net_Amount         ,     CASE    WHEN a.Transportation_Mode = 'Rail' THEN '2'  WHEN a.Transportation_Mode = 'Air' THEN '3'  WHEN a.Transportation_Mode = 'Ship' THEN '4'    ELSE '1' END AS TrMode , L.Distance , " &
        '                 " a.Vehicle_No,'R','" & NewCode & "' , tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from Sales_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
        '                 " Inner Join Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo left Outer Join Transport_Head T on a.Transport_IdNo = T.Transport_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo Left Outer Join State_Head FS On " &
        '                 " C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.State_IdNo = TS.State_IdNo  where a.Sales_Code = '" & NewCode & "'"

        'CMD.ExecuteNonQuery()


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim dt1 As New DataTable

        da = New SqlClient.SqlDataAdapter(" Select  I.Item_Name,IG.ItemGroup_Name,SD.HSN_Code,SD.Tax_Perc,sum(SD.Assessable_Value) As TaxableAmt,sum(SD.NoOf_Items) as Qty,Min(Sl_No), U.Unit_Name " &
                                          " from Sales_Details SD Inner Join Item_Head I On SD.Item_IdNo = I.Item_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " Inner Join Unit_Head U On SD.Unit_IdNo = U.Unit_IdNo Where SD.Sales_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.Item_Name,IG.ItemGroup_Name,SD.HSN_Code,IG.ItemGroup_Name ,SD.Tax_Perc ,U.Unit_Name ,SD.IGST_Percentage,SD.CGST_Percentage,SD.SGST_Percentage", con)
        'da = New SqlClient.SqlDataAdapter(" Select  I.Item_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,SD.Tax_Perc,sum(SD.Assessable_Value) As TaxableAmt,sum(SD.NoOf_Items) as Qty,Min(Sl_No), U.Unit_Name " &
        '                                  " from Sales_Details SD Inner Join Item_Head I On SD.Item_IdNo = I.Item_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
        '                                  " Inner Join Unit_Head U On SD.Unit_IdNo = U.Unit_IdNo Where SD.Sales_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " I.Item_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,SD.Tax_Perc ,U.Unit_Name ,SD.IGST_Percentage,SD.CGST_Percentage,SD.SGST_Percentage", con)
        'dt1 = New DataTable
        da.Fill(dt1)

        For I = 0 To dt1.Rows.Count - 1

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,      	[HSNCode]                 ,	[Quantity]                       ,               [QuantityUnit]                      ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode) " &
                              " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",      '" & dt1.Rows(I).Item(7).ToString & "'         ," & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "')"

            CMD.ExecuteNonQuery()

        Next

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Sales_Head", "Electronic_Reference_No", "Sales_Code", Pk_Condition)

    End Sub
    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub
    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_Electronic_RefNo.Text, rtbEWBResponse)
    End Sub
    Private Sub btn_EwbCancel_Click(sender As Object, e As EventArgs) Handles btn_EwbCancel.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub


        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.CancelEWB(txt_Electronic_RefNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "FinishedProduct_Invoice_Head", "Electronic_Reference_No", "Sales_Code")


    End Sub

    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_Electronic_RefNo.Text = txt_EWBNo.Text
        txt_eWayBill_No.Text = txt_EWBNo.Text
    End Sub

    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_Electronic_RefNo.Text, rtbEWBResponse, 1)
    End Sub

    Private Sub btn_Detail_PRINT_EWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINT_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_Electronic_RefNo.Text, rtbEWBResponse, 1)
    End Sub

    Private Sub txt_Electronic_RefNo_TextChanged(sender As Object, e As EventArgs) Handles txt_Electronic_RefNo.TextChanged
        txt_EWBNo.Text = txt_Electronic_RefNo.Text
        txt_eWayBill_No.Text = txt_EWBNo.Text
    End Sub

    Private Sub cbo_DispatcherName_Enter(sender As Object, e As EventArgs) Handles cbo_DispatcherName.Enter

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        cbo_DispatcherName.Tag = cbo_DispatcherName.Text

    End Sub

    Private Sub cbo_DispatcherName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_DispatcherName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DispatcherName, cbo_DeliveryTo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_DispatcherName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Currency.Visible Then
                cbo_Currency.Focus()

            Else

                txt_LrNo.Focus()

                cbo_ItemName.Focus()
            End If

        End If
    End Sub

    Private Sub cbo_DispatcherName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_DispatcherName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DispatcherName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If cbo_Currency.Visible Then
                cbo_Currency.Focus()

            Else

                txt_LrNo.Focus()

                'cbo_ItemName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DispatcherName_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_DispatcherName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DispatcherName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_PaymentMethod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentMethod.TextChanged
        'If Trim(UCase(cbo_PaymentMethod.Text)) <> "" Then
        '    If Trim(UCase(cbo_PaymentMethod.Text)) <> Trim(UCase(cbo_PaymentMethod.Tag)) Then

        '        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
        '            cbo_Ledger.DropDownStyle = ComboBoxStyle.Simple
        '        Else
        '            cbo_Ledger.DropDownStyle = ComboBoxStyle.DropDown
        '            cbo_Ledger.Text = ""
        '        End If
        '        cbo_PaymentMethod.Tag = cbo_PaymentMethod.Text
        '    End If
        'End If

    End Sub
    Private Sub Get_TCS_Sts_From_Ledger_Name()
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

        da1 = New SqlClient.SqlDataAdapter("Select a.* from Ledger_Head a Where a.Ledger_IdNo = " & Str(Val(Led_ID)), con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)("TCS_Sales_Status").ToString) = False Then
                vTCSDED_STS = dt1.Rows(0)("TCS_Sales_Status").ToString
            End If
            'If IsDBNull(dt1.Rows(0)("Sales_TDS_Deduction_Status").ToString) = False Then
            '    vTDSDED_STS = dt1.Rows(0)("Sales_TDS_Deduction_Status").ToString
            'End If
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

        If Trim(vDESPTO) <> "" Then
            txt_Place_Of_Supply.Text = Trim(vDESPTO)
            txt_Final_destination.Text = Trim(vDESPTO)
        End If

        cbo_Ledger.Tag = cbo_Ledger.Text
        cbo_DeliveryTo.Tag = cbo_DeliveryTo.Text

    End Sub

    Private Sub cbo_PaymentMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_PaymentMethod.SelectedIndexChanged

        'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
        '    txt_cash_Party_name.Visible = True
        '    lbl_Cash_Party.Visible = True
        '    cbo_PaymentMethod.Size = New Size(111, 23) '(lbl_InvoiceNo.Width + cbo_InvoiceSufixNo.Width) - 11

        '    ' lbl_ewaybill.Text = "EWB No"
        '    lbl_ewaybill.Left = lbl_dc_date.Left
        '    txt_Electronic_RefNo.Left = txt_DcDate.Left
        '    txt_Electronic_RefNo.Width = txt_DcDate.Width

        'Else
        '    txt_cash_Party_name.Visible = False
        '    lbl_Cash_Party.Visible = False

        '    cbo_PaymentMethod.Width = cbo_DeliveryTo.Width - 11

        '    'lbl_ewaybill.Text = "E-Way Bill No"
        '    lbl_ewaybill.Left = Label40.Left
        '    txt_Electronic_RefNo.Left = txt_Place_Of_Supply.Left
        '    txt_Electronic_RefNo.Width = txt_Place_Of_Supply.Width


        'End If
    End Sub

    Private Sub txt_cash_Party_name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_cash_Party_name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Electronic_RefNo.Focus()
        End If
    End Sub

    Private Sub txt_cash_Party_name_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_cash_Party_name.KeyDown
        If e.KeyCode = 38 Then
            cbo_PaymentMethod.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_Electronic_RefNo.Focus()
        End If
    End Sub
    Private Sub txt_cash_Party_name_LostFocus(sender As Object, e As EventArgs) Handles txt_cash_Party_name.LostFocus
        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(txt_cash_Party_name.Text) = "" And Trim(cbo_Ledger.Text) = "" Then
            txt_cash_Party_name.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If
    End Sub

    Private Sub txt_Electronic_RefNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Electronic_RefNo.KeyDown
        If e.KeyCode = 38 Then
            If txt_cash_Party_name.Visible = True Then
                txt_cash_Party_name.Focus()
            Else
                cbo_PaymentMethod.Focus()
            End If
        ElseIf e.KeyCode = 40 Then
            txt_DateTime_Of_Supply.Focus()
        End If
    End Sub

    Private Sub btn_Courier_details_Click(sender As Object, e As EventArgs) Handles btn_Courier_details.Click
        pnl_Back.Enabled = False
        pnl_Courier_Details.Visible = True
        pnl_Courier_Details.BringToFront()
        If txt_CourierName.Visible And txt_CourierName.Enabled Then txt_CourierName.Focus()
    End Sub

    Private Sub btn_Close_Pnl_Courier_Details_Click(sender As Object, e As EventArgs) Handles btn_Close_Pnl_Courier_Details.Click
        pnl_Back.Enabled = True
        pnl_Courier_Details.Visible = False
        txt_CashDiscPerc.Focus()
    End Sub

    Private Sub txt_OrderNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_OrderNo.KeyDown

        If e.KeyCode = 38 Then
            cbo_Ledger.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_OrderDate.Focus()
        End If
    End Sub

    Private Sub txt_OrderNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_OrderNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_OrderDate.Focus()
        End If
    End Sub

    Private Sub txt_PoNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_PoNo.KeyDown
        If e.KeyCode = 38 Then
            txt_OrderNo.Focus()
        ElseIf e.KeyCode = 40 Then
            cbo_TransportMode.Focus()
        End If
    End Sub

    Private Sub txt_PoNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_PoNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_TransportMode.Focus()
        End If
    End Sub

    Private Sub txt_CourierName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_CourierName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Courier_No.Focus()
        End If
    End Sub

    Private Sub txt_CourierName_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_CourierName.KeyDown
        If e.KeyCode = 38 Then
            txt_Courier_Noof_Box.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_Courier_No.Focus()
        End If
    End Sub

    Private Sub txt_Courier_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Courier_No.KeyDown
        If e.KeyCode = 38 Then
            txt_CourierName.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_courier_date.Focus()
        End If
    End Sub

    Private Sub txt_Courier_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Courier_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_courier_date.Focus()

        End If
    End Sub

    Private Sub txt_courier_date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_courier_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Courier_Noof_Box.Focus()

        End If
    End Sub

    Private Sub txt_courier_date_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_courier_date.KeyDown
        If e.KeyCode = 38 Then
            txt_Courier_No.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_Courier_Noof_Box.Focus()
        End If
    End Sub

    Private Sub txt_Courier_Noof_Box_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Courier_Noof_Box.KeyDown
        If e.KeyCode = 38 Then
            txt_courier_date.Focus()
        ElseIf e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to Close Courier Details ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                btn_Close_Pnl_Courier_Details_Click(sender, e)
            Else
                txt_CourierName.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Courier_Noof_Box_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Courier_Noof_Box.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to Close Courier Details ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                btn_Close_Pnl_Courier_Details_Click(sender, e)
            Else
                txt_CourierName.Focus()
            End If
        End If
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_InvoicePrefixNo.KeyPress

        If Asc(e.KeyChar) = 13 Then
            cbo_InvoiceSufixNo.Focus()
        End If

    End Sub


    Private Sub btn_Close_proforma_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_proforma_Selection.Click
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Ent_Qty As Single, Ent_Rate As Single

        dgv_Details.Rows.Clear()

        NoCalc_Status = True

        sno = 0


        For i = 0 To dgv_proforma_Selections.RowCount - 1

            If Val(dgv_proforma_Selections.Rows(i).Cells(8).Value) = 1 Then

                If Val(dgv_proforma_Selections.Rows(i).Cells(10).Value) <> 0 Then
                    Ent_Qty = Val(dgv_proforma_Selections.Rows(i).Cells(10).Value)
                Else
                    Ent_Qty = Val(dgv_proforma_Selections.Rows(i).Cells(4).Value)

                End If

                If Val(dgv_proforma_Selections.Rows(i).Cells(12).Value) <> 0 Then
                    Ent_Rate = Val(dgv_proforma_Selections.Rows(i).Cells(12).Value)

                Else
                    Ent_Rate = Val(dgv_proforma_Selections.Rows(i).Cells(5).Value)

                End If

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(DgvCol_Details.SL_NO).Value = Val(sno)
                dgv_Details.Rows(n).Cells(DgvCol_Details.ITEM_NAME).Value = dgv_proforma_Selections.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(DgvCol_Details.UNIT).Value = dgv_proforma_Selections.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(DgvCol_Details.DESCRIBTION).Value = dgv_proforma_Selections.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(DgvCol_Details.QUANTITY).Value = Val(Ent_Qty)
                dgv_Details.Rows(n).Cells(DgvCol_Details.RATE_QTY).Value = Val(Ent_Rate)
                dgv_Details.Rows(n).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
                'dgv_Details.Rows(n).Cells(7).Value =
                dgv_Details.Rows(n).Cells(DgvCol_Details.HSN_CODE).Value = dgv_proforma_Selections.Rows(i).Cells(15).Value
                dgv_Details.Rows(n).Cells(DgvCol_Details.GST_PERC).Value = dgv_proforma_Selections.Rows(i).Cells(16).Value ' Format((Val(Ent_Qty) * Val(Ent_Rate)) - Val(dgv_Details.Rows(n).Cells(12).Value), "##########0.00")
                dgv_Details.Rows(n).Cells(DgvCol_Details.PROFORMA_INV_CODE).Value = dgv_proforma_Selections.Rows(i).Cells(9).Value

                txt_proformaNo.Text = dgv_proforma_Selections.Rows(i).Cells(1).Value
                cbo_Transport.Text = dgv_proforma_Selections.Rows(i).Cells(13).Value
                txt_VehicleNo.Text = dgv_proforma_Selections.Rows(i).Cells(14).Value
                '   dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(9).Value


                ' Amount_Calculation(True)

            End If

        Next i



        NoCalc_Status = False
        Amount_Calculation(True)

        pnl_Back.Enabled = True

        pnl_proforma_selection.Visible = False

        'txt_BillNo.Focus()
        'cbo_EntType.Enabled = False

        If txt_Electronic_RefNo.Enabled And txt_Electronic_RefNo.Visible Then txt_Electronic_RefNo.Focus()

    End Sub

    Private Sub btn_proforma_Close_Click(sender As Object, e As EventArgs) Handles btn_Close_proforma_Selection.Click
        pnl_Back.Enabled = True

        pnl_proforma_selection.Visible = False
        cbo_PaymentMethod.Focus()
    End Sub
    Private Sub dgv_proforma_Selections_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_proforma_Selections.CellClick
        Select_Invoice(e.RowIndex)
    End Sub

    Private Sub Select_Invoice(ByVal RwIndx As Integer)
        Dim i As Integer
        Dim DGV_NAME As New DataGridView
        DGV_NAME = Nothing
        If ActiveControl.Name = dgv_proforma_Selections.Name Or ActiveControl.Name = dgv_Delivery_Selection.Name Then

            If ActiveControl.Name = dgv_proforma_Selections.Name Then

                DGV_NAME = dgv_proforma_Selections
            ElseIf ActiveControl.Name = dgv_Delivery_Selection.Name Then

                DGV_NAME = dgv_Delivery_Selection

            End If

            With DGV_NAME

                If DGV_NAME.RowCount > 0 And RwIndx >= 0 Then

                    DGV_NAME.Rows(RwIndx).Cells(8).Value = (Val(DGV_NAME.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(8).Value) = 0 Then .Rows(RwIndx).Cells(8).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                End If

            End With
            '
            'With dgv_proforma_Selections

            '    If dgv_proforma_Selections.RowCount > 0 And RwIndx >= 0 Then

            '        dgv_proforma_Selections.Rows(RwIndx).Cells(8).Value = (Val(dgv_proforma_Selections.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

            '        If Val(.Rows(RwIndx).Cells(8).Value) = 0 Then .Rows(RwIndx).Cells(8).Value = ""

            '        For i = 0 To .ColumnCount - 1
            '            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
            '        Next

            '    End If

            'End With
        End If

    End Sub
    'Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
    '    With dgv_Details
    '        vcbo_KeyDwnVal = e.KeyValue
    '    End With
    'End Sub
    Private Sub dgv_proforma_Selections_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_proforma_Selections.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_proforma_Selections.CurrentCell.RowIndex >= 0 Then
                Select_Invoice(dgv_proforma_Selections.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If

        If e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Then
            If dgv_proforma_Selections.CurrentCell.RowIndex >= 0 Then
                If Val(dgv_proforma_Selections.Rows(dgv_proforma_Selections.CurrentCell.RowIndex).Cells(8).Value) = 1 Then
                    e.Handled = True
                    Select_Invoice(dgv_proforma_Selections.CurrentCell.RowIndex)
                End If
            End If
        End If

    End Sub

    Private Sub cbo_EntType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntType, dtp_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_EntType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntType, cbo_Ledger, "", "", "", "")
    End Sub

    'Private Sub dtp_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_Date.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        If cbo_EntType.Visible Then
    '            cbo_EntType.Focus()
    '        Else
    '            cbo_Ledger.Focus()

    '        End If
    '    End If
    'End Sub

    'Private Sub cbo_EntType_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    If Trim(UCase(cbo_EntType.Text)) <> "DIRECT" And Trim(cbo_EntType.Text) <> "" And cbo_EntType.Visible Then
    '        Panel2.Enabled = False
    '        dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
    '        dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
    '        txt_OrderDate.Enabled = False
    '        txt_OrderNo.Enabled = False
    '        txt_DcDate.Enabled = False
    '        txt_DcNo.Enabled = False
    '        cbo_Transport.Enabled = False
    '        txt_VehicleNo.Enabled = False
    '        dgv_Details.ReadOnly = False
    '        For i = 0 To dgv_Details.ColumnCount - 1

    '            If i = 4 Or i = 5 Then
    '                dgv_Details.Columns(i).ReadOnly = False
    '            Else
    '                dgv_Details.Columns(i).ReadOnly = True

    '            End If
    '        Next


    '    Else
    '        Panel2.Enabled = True
    '        dgv_Details.ReadOnly = True
    '        dgv_Details.EditMode = DataGridViewEditMode.EditProgrammatically
    '        dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    '        txt_OrderDate.Enabled = True
    '        txt_OrderNo.Enabled = True
    '        txt_DcDate.Enabled = True
    '        txt_DcNo.Enabled = True
    '        cbo_Transport.Enabled = True
    '        txt_VehicleNo.Enabled = True
    '    End If
    'End Sub
    Public Sub Check_Combo_Cash_Party_Name()
        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
            txt_cash_Party_name.Visible = True
            lbl_Cash_Party.Visible = True
            cbo_PaymentMethod.Size = New Size(111, 23) '(lbl_InvoiceNo.Width + cbo_InvoiceSufixNo.Width) - 11

            ' lbl_ewaybill.Text = "EWB No"
            lbl_ewaybill.Left = lbl_dc_date.Left
            txt_Electronic_RefNo.Left = txt_DcDate.Left
            txt_Electronic_RefNo.Width = txt_DcDate.Width

        Else
            txt_cash_Party_name.Visible = False
            lbl_Cash_Party.Visible = False

            cbo_PaymentMethod.Width = cbo_DeliveryTo.Width - 11

            'lbl_ewaybill.Text = "E-Way Bill No"
            lbl_ewaybill.Left = Label40.Left
            txt_Electronic_RefNo.Left = txt_Place_Of_Supply.Left
            txt_Electronic_RefNo.Width = txt_Place_Of_Supply.Width


        End If
    End Sub

    Private Sub cbo_PaymentMethod_Leave(sender As Object, e As EventArgs) Handles cbo_PaymentMethod.Leave
        Check_Combo_Cash_Party_Name()
    End Sub

    Private Sub btn_Close_Delivery_Selection_Click(sender As Object, e As EventArgs) Handles btn_Close_Delivery_Selection.Click
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Ent_Qty As Single, Ent_Rate As Single

        dgv_Details.Rows.Clear()

        NoCalc_Status = True

        sno = 0

        For i = 0 To dgv_Delivery_Selection.RowCount - 1

            If Val(dgv_Delivery_Selection.Rows(i).Cells(8).Value) = 1 Then

                If Val(dgv_Delivery_Selection.Rows(i).Cells(12).Value) <> 0 Then
                    Ent_Qty = Val(dgv_Delivery_Selection.Rows(i).Cells(12).Value)

                Else
                    Ent_Qty = Val(dgv_Delivery_Selection.Rows(i).Cells(5).Value)

                End If

                If Val(dgv_Delivery_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    Ent_Rate = Val(dgv_Delivery_Selection.Rows(i).Cells(13).Value)

                Else
                    Ent_Rate = Val(dgv_Delivery_Selection.Rows(i).Cells(6).Value)

                End If

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(DgvCol_Details.SL_NO).Value = Val(sno)
                dgv_Details.Rows(n).Cells(DgvCol_Details.ITEM_NAME).Value = dgv_Delivery_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(DgvCol_Details.UNIT).Value = dgv_Delivery_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(DgvCol_Details.DESCRIBTION).Value = dgv_Delivery_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(DgvCol_Details.QUANTITY).Value = Val(Ent_Qty)
                dgv_Details.Rows(n).Cells(DgvCol_Details.RATE_QTY).Value = Val(Ent_Rate)
                dgv_Details.Rows(n).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
                dgv_Details.Rows(n).Cells(DgvCol_Details.HSN_CODE).Value = dgv_Delivery_Selection.Rows(i).Cells(19).Value
                dgv_Details.Rows(n).Cells(DgvCol_Details.GST_PERC).Value = dgv_Delivery_Selection.Rows(i).Cells(20).Value

                dgv_Details.Rows(n).Cells(DgvCol_Details.SALES_DELV_CODE).Value = dgv_Delivery_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(DgvCol_Details.SALES_DELV_SLNO).Value = dgv_Delivery_Selection.Rows(i).Cells(10).Value



                txt_OrderNo.Text = dgv_Delivery_Selection.Rows(i).Cells(15).Value
                txt_OrderDate.Text = dgv_Delivery_Selection.Rows(i).Cells(16).Value
                txt_DcNo.Text = dgv_Delivery_Selection.Rows(i).Cells(1).Value
                txt_DcDate.Text = dgv_Delivery_Selection.Rows(i).Cells(14).Value
                cbo_Transport.Text = dgv_Delivery_Selection.Rows(i).Cells(17).Value
                txt_VehicleNo.Text = dgv_Delivery_Selection.Rows(i).Cells(18).Value
                '   dgv_Details.Rows(n).Cells(10).Value = dgv_Delivery_Selection.Rows(i).Cells(9).Value

            End If

        Next i

        NoCalc_Status = False



        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        'txt_BillNo.Focus()
        'cbo_EntType.Enabled = False

        If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus()

    End Sub
    Private Sub dgv_Delivery_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Delivery_Selection.CellClick
        Select_Invoice(e.RowIndex)
    End Sub
    Private Sub dgv_Delivery_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Delivery_Selection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Delivery_Selection.CurrentCell.RowIndex >= 0 Then
                Select_Invoice(dgv_Delivery_Selection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If

        If e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Then
            If dgv_Delivery_Selection.CurrentCell.RowIndex >= 0 Then
                If Val(dgv_Delivery_Selection.Rows(dgv_Delivery_Selection.CurrentCell.RowIndex).Cells(8).Value) = 1 Then
                    e.Handled = True
                    Select_Invoice(dgv_Delivery_Selection.CurrentCell.RowIndex)
                End If
            End If
        End If

    End Sub
    Private Sub dgtxt_Details_TextChanged(sender As Object, e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = Nothing
        If dgv_Details.CurrentCell.ColumnIndex > 1 Then
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        End If
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then
                    If .CurrentCell.ColumnIndex = DgvCol_Details.RATE_QTY Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        Try

            With dgv_Details
                If .Visible Then

                    If IsNothing(.CurrentCell) Then Exit Sub

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = DgvCol_Details.RATE_QTY Then
                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If

                        End If
                    End If
                End If


            End With

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp

        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        dgv_Details_KeyUp(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub
    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Siz_idno As Integer = 0
        Dim sqft_qty As Single = 0

        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
                If .Visible Then
                    'If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Or Trim(UCase(cbo_EntType.Text)) = "SALES ORDER" Then
                    If .CurrentCell.ColumnIndex = DgvCol_Details.RATE_QTY Or .CurrentCell.ColumnIndex = DgvCol_Details.AMOUNT Or .CurrentCell.ColumnIndex = DgvCol_Details.FOOTER_CSH_DISC_PER Then

                        'If Trim(UCase(cbo_TaxType.Text)) = Trim(UCase("GST")) Then
                        '    .Rows(e.RowIndex).Cells(DgvCol_Details.GST_RATE).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_Details.RATE_QTY).Value) + (Val(.Rows(e.RowIndex).Cells(DgvCol_Details.RATE_QTY).Value) * Val(.Rows(e.RowIndex).Cells(DgvCol_Details.GST_PERC).Value) / 100), "#########0.00")
                        'Else
                        .Rows(e.RowIndex).Cells(DgvCol_Details.GST_RATE).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_Details.RATE_QTY).Value), "#########0.00")
                        'End If

                        If .Rows(.CurrentCell.RowIndex).Cells(DgvCol_Details.RATE_QTY).Value <> 0 Then
                            .Rows(.CurrentCell.RowIndex).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_Details.QUANTITY).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_Details.RATE_QTY).Value), "#########0.00")
                        Else
                            .Rows(.CurrentCell.RowIndex).Cells(DgvCol_Details.AMOUNT).Value = "" ' Format(Val(.Rows(.CurrentCell.RowIndex).Cells(5).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value), "#########0.00")
                        End If
                        .Rows(e.RowIndex).Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
                        .Rows(e.RowIndex).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_Details.AMOUNT).Value) * Val(.Rows(e.RowIndex).Cells(DgvCol_Details.FOOTER_CSH_DISC_PER).Value) / 100, "#########0.00")
                        .Rows(e.RowIndex).Cells(DgvCol_Details.ASSESSABLE_VALUE).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_Details.AMOUNT).Value) - Val(.Rows(e.RowIndex).Cells(DgvCol_Details.FOOTER_CSH_DISC_AMT).Value), "#########0.00")

                        TotalAmount_Calculation()
                        'End If
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try

    End Sub


    Private Sub Show_Item_CurrentStock()
        Dim vItemID As Integer
        Dim CurStk As Decimal

        If Trim(cbo_ItemName.Text) <> "" Then
            vItemID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
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

    Private Sub txt_Rate_GotFocus(sender As Object, e As EventArgs) Handles txt_Rate.GotFocus
        Show_Item_CurrentStock()
    End Sub

    Private Sub txt_SlNo_GotFocus(sender As Object, e As EventArgs) Handles txt_SlNo.GotFocus
        Show_Item_CurrentStock()
    End Sub

    Private Sub cbo_Unit_GotFocus(sender As Object, e As EventArgs) Handles cbo_Unit.GotFocus
        Show_Item_CurrentStock()
    End Sub

    Private Sub dtp_Date_GotFocus(sender As Object, e As EventArgs) Handles dtp_Date.GotFocus
        Show_Item_CurrentStock()
    End Sub
    Private Sub btn_ExportInv_Port_details_Click(sender As Object, e As EventArgs) Handles btn_ExportInv_Port_details.Click
        pnl_Back.Enabled = False
        pnl_EXport_Inv_Port_Details.Visible = True
        pnl_EXport_Inv_Port_Details.BringToFront()
        If txt_Exports_Ref.Visible And txt_Exports_Ref.Enabled Then txt_Exports_Ref.Focus()
    End Sub
    Private Sub btn_Close_ExportInv_Port_details_Click(sender As Object, e As EventArgs) Handles btn_Close_ExportInv_Port_details.Click
        pnl_Back.Enabled = True
        pnl_EXport_Inv_Port_Details.Visible = False
        txt_CashDiscPerc.Focus()
    End Sub
    Private Sub cbo_Pre_Carriage_by_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Pre_Carriage_by.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Pre_Carriage_by, txt_Other_Reference, txt_Place_Of_Receipt_By_Pre_carrier, "sales_head", "pre_Carriage_by", "(pre_Carriage_by <>'')", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Pre_Carriage_by_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Pre_Carriage_by.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Pre_Carriage_by, txt_Place_Of_Receipt_By_Pre_carrier, "sales_head", "pre_Carriage_by", "(pre_Carriage_by <>'')", "", False)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub cbo_Pre_Carriage_by_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Pre_Carriage_by.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "sales_head", "pre_Carriage_by", "(pre_Carriage_by <>'')", "")
    End Sub
    Private Sub cbo_Vessal_Flight_No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vessal_Flight_No.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vessal_Flight_No, txt_Place_Of_Receipt_By_Pre_carrier, txt_Port_Of_Loading, "sales_head", "Vessal_Flight_No", "(Vessal_Flight_No <>'')", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub cbo_Vessal_Flight_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vessal_Flight_No.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vessal_Flight_No, txt_Port_Of_Loading, "sales_head", "Vessal_Flight_No", "(Vessal_Flight_No <>'')", "", False)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Vessal_Flight_No_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vessal_Flight_No.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "sales_head", "Vessal_Flight_No", "(Vessal_Flight_No <>'')", "")
    End Sub
    Private Sub Printing_Export_INV_Format10(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        Dim vItem_Name1 = ""
        Dim vItem_Name2 = ""
        Dim vItem_Name3 = ""
        Dim vDesc = ""
        Dim vArry_Desc() As String

        Dim k = 0


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 45
            .Right = 50
            .Top = 40 ' 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Regular)

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
        TxtHgt = 17.5 '18.5 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1244" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then  '--- Madonna Tex
            NoofItems_PerPage = 10 ' 8
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1543" Then ' --- SRI SUBHAM TYRES
            NoofItems_PerPage = 16 ' 10   ' 14
        Else
            NoofItems_PerPage = 14 ' 10   ' 14
        End If


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 30 : ClArr(2) = 90 : ClArr(3) = 280 : ClArr(4) = 90 : ClArr(5) = 90 : ClArr(6) = 55
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim vNOOFTAXLINES As Integer, vGST_PERC_AMT_FOR_PRNT As String
        vNOOFTAXLINES = 0
        vGST_PERC_AMT_FOR_PRNT = get_GSTPercentage_and_GSTAmount_For_Printing(EntryCode, vNOOFTAXLINES)

        NoofItems_PerPage = 3 'NoofItems_PerPage - vNOOFTAXLINES

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

                vNoofHsnCodes = 0
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then '---- GAJAKHARNAA TRADERS (Somanur)
                    vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
                End If
                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 7
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                '***** GST END *****

                Printing_Export_INV_Format10_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0

                    CurY = CurY - 5
                    'If Val(prn_Count) > 1 Then
                    '    CurY = CurY - TxtHgt + 5
                    'Else
                    '    CurY = CurY - TxtHgt - 10
                    'End If
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "100 % HOSIERY GOODS", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    End If

                    If prn_DetMxIndx > 0 Then

                        Do While DetIndx <= prn_DetMxIndx

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_Export_INV_Format10_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                e.HasMorePages = True

                                Return

                            End If


                            CurY = CurY + TxtHgt
                            DetIndx = DetIndx + 1

                            vitem_Name1 = Trim(prn_DetAr(DetIndx, 2)) 'Trim(prn_DetDt.Rows(DetIndx).Item("Item_Name").ToString)
                            vitem_Name2 = ""

                            If Len(vitem_Name1) > 35 Then
                                For k = 35 To 1 Step -1
                                    If Mid$(Trim(vitem_Name1), k, 1) = " " Or Mid$(Trim(vitem_Name1), k, 1) = "," Or Mid$(Trim(vitem_Name1), k, 1) = "." Or Mid$(Trim(vitem_Name1), k, 1) = "-" Or Mid$(Trim(vitem_Name1), k, 1) = "/" Or Mid$(Trim(vitem_Name1), k, 1) = "_" Or Mid$(Trim(vitem_Name1), k, 1) = "(" Or Mid$(Trim(vitem_Name1), k, 1) = ")" Or Mid$(Trim(vitem_Name1), k, 1) = "\" Or Mid$(Trim(vitem_Name1), k, 1) = "[" Or Mid$(Trim(vitem_Name1), k, 1) = "]" Or Mid$(Trim(vitem_Name1), k, 1) = "{" Or Mid$(Trim(vitem_Name1), k, 1) = "}" Then Exit For
                                Next k
                                If k = 0 Then k = 35
                                vitem_Name2 = Microsoft.VisualBasic.Right(Trim(vitem_Name1), Len(vitem_Name1) - k)
                                vitem_Name1 = Microsoft.VisualBasic.Left(Trim(vitem_Name1), k - 1)
                            End If
                            vitem_Name3 = ""
                            If Len(vitem_Name2) > 35 Then
                                For K = 35 To 1 Step -1
                                    If Mid$(Trim(vitem_Name2), K, 1) = " " Or Mid$(Trim(vitem_Name2), K, 1) = "," Or Mid$(Trim(vitem_Name2), K, 1) = "." Or Mid$(Trim(vitem_Name2), K, 1) = "-" Or Mid$(Trim(vitem_Name2), K, 1) = "/" Or Mid$(Trim(vitem_Name2), K, 1) = "_" Or Mid$(Trim(vitem_Name2), K, 1) = "(" Or Mid$(Trim(vitem_Name2), K, 1) = ")" Or Mid$(Trim(vitem_Name2), K, 1) = "\" Or Mid$(Trim(vitem_Name2), K, 1) = "[" Or Mid$(Trim(vitem_Name2), K, 1) = "]" Or Mid$(Trim(vitem_Name2), K, 1) = "{" Or Mid$(Trim(vitem_Name2), K, 1) = "}" Then Exit For
                                Next K
                                If K = 0 Then K = 35
                                vitem_Name3 = Microsoft.VisualBasic.Right(Trim(vitem_Name2), Len(vitem_Name2) - K)
                                vitem_Name2 = Microsoft.VisualBasic.Left(Trim(vitem_Name2), K - 1)
                            End If



                            If Val(prn_DetAr(DetIndx, 5)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, "01" & " - " & Val(prn_DetAr(DetIndx, 5)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vitem_Name1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 15), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 5), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)



                                If Trim(vitem_Name2) <> "" Or Trim(prn_DetAr(DetIndx, 14)) <> "" Then
                                    NoofDets = NoofDets + 1
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(DetIndx, 14)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(vitem_Name2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                End If
                                If Trim(vitem_Name3) <> "" Then
                                    NoofDets = NoofDets + 1
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(vitem_Name3), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                End If


                                If Trim(prn_DetAr(DetIndx, 12)) <> "" Then
                                    Erase vArry_Desc
                                    vDesc = Trim(prn_DetAr(DetIndx, 12))
                                    vArry_Desc = Split(Trim(vDesc), ",")

                                    For I = 0 To UBound(vArry_Desc)
                                        If Trim(vArry_Desc(I)) <> "" Then
                                            NoofDets = NoofDets + 1
                                            CurY = CurY + TxtHgt
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(vArry_Desc(I)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                        End If
                                    Next

                                End If

                            End If

                            NoofDets = NoofDets + 1

                        Loop

                    End If

                    'If prn_Count > 1 Then
                    '    CurY = CurY - TxtHgt
                    'End If

                    Printing_Export_INV_Format10_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 0 ' 1
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

    Private Sub Printing_Export_INV_Format10_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font, p2font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Cmp_UAMNO As String = ""

        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0, k As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""

        Dim vbr As SolidBrush                  '--COMMON BRUSH FOR ALL DETAILS 
        Dim vbr_CmpName As SolidBrush          '--COMPANY TITTLE
        Dim vbr_CmpDets As SolidBrush          '--COMPANY DETAILS

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
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

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 4, 2, PrintWidth, p1Font)

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1225" Then  '--- Yasen Tex (Mangalam)
            '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.YASEN_LOGO, Drawing.Image), LMargin + 20, CurY + 5, 120, 100)
            'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1244" Then  '--- Madonna Tex
            '    If InStr(1, Trim(UCase(prn_HdDt_New.Rows(0).Item("Company_Name").ToString)), "MARSLIN") > 0 Then
            '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MarslinTex, Drawing.Image), LMargin + 10, CurY + 5, 90, 110)
            '    Else
            '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MadonnaTex, Drawing.Image), LMargin + 10, CurY + 5, 90, 110)
            '    End If
            '    If InStr(1, Trim(UCase(prn_HdDt_New.Rows(0).Item("Company_Name").ToString)), "MARSLIN") > 0 Then
            '        If Vchk_shirt_bill <> 0 Then
            '            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.COMPANYLOGO_MARSLIN, Drawing.Image), PageWidth - 100, CurY + 5, 90, 100)

            '        Else
            '            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Marslin_Madonna_Tex, Drawing.Image), PageWidth - 100, CurY + 5, 90, 110)

            '        End If
            '    End If
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
                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 5, 110, 100)

                            End If

                        End Using

                    End If

                End If

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

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 20, 90, 90)

                        End If

                    End Using
                End If
            End If

        End If

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_UAMNO = ""

        Cmp_Name = Trim(prn_HdDt_New.Rows(0).Item("Company_Name").ToString)

        Cmp_Add1 = Trim(prn_HdDt_New.Rows(0).Item("Company_Address1").ToString)
        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt_New.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt_New.Rows(0).Item("Company_Address2").ToString)
            End If
        Else
            Cmp_Add1 = Trim(prn_HdDt_New.Rows(0).Item("Company_Address2").ToString)
        End If

        Cmp_Add2 = Trim(prn_HdDt_New.Rows(0).Item("Company_Address3").ToString)
        If Trim(Cmp_Add2) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt_New.Rows(0).Item("Company_Address4").ToString)
            Else
                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt_New.Rows(0).Item("Company_Address4").ToString)
            End If
        Else
            Cmp_Add2 = Trim(prn_HdDt_New.Rows(0).Item("Company_Address4").ToString)
        End If

        If Trim(prn_HdDt_New.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt_New.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt_New.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt_New.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt_New.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt_New.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt_New.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt_New.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt_New.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt_New.Rows(0).Item("Company_EMail").ToString)
        End If
        If Trim(prn_HdDt_New.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt_New.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt_New.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt_New.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt_New.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt_New.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt_New.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt_New.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_UAM_No").ToString) <> "" Then
            Cmp_UAMNO = "UDYAM No. : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
        End If
        '***** GST END *****

        '***********************

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then  '----OMM ELECTRICAL CARES
            If Trim(UCase(Cmp_GSTIN_No)) = "33BUPPK6766D1ZC" Then

                vbr_CmpName = New SolidBrush(Color.Blue)
                vbr_CmpDets = New SolidBrush(Color.Green)

                p2font = New Font("bodoni mt black", 18, FontStyle.Bold)
            End If
        End If

        '***********************

        CurY = CurY + TxtHgt - 15

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1225" Then  '--- Yasen Tex (Mangalam)
            Dim vLightGreenBrush As New SolidBrush(Color.FromArgb(171, 206, 26))
            p1Font = New Font("Cambria", 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, vLightGreenBrush)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1460" Then
            p1Font = New Font("Times New Roman", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then
            If Trim(UCase(Cmp_GSTIN_No)) = "33BUPPK6766D1ZC" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p2font, vbr_CmpName)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            End If
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        ItmNm1 = Trim(prn_HdDt_New.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpDets)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpDets)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, vbr_CmpDets)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, vbr_CmpDets)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, vbr_CmpDets)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, vbr_CmpDets)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font, vbr_CmpDets)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont, vbr_CmpDets)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        If Trim(Cmp_UAMNO) <> "" Then
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpDets)
        End If

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY - 10, PageWidth, CurY - 10)
            LnAr(2) = CurY


            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)
            ItmNm2 = ""

            If Len(ItmNm1) > 35 Then
                For i = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
            End If

            CurY = CurY - 5

        End If

        'CurY = CurY + TxtHgt + 5
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(2) = CurY

        'Y1 = CurY + 0.5
        'Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        'vHeading = Trim(UCase(prn_HdDt_New.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"

        'CurY = CurY + TxtHgt - 15
        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try


            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) - 20
            W3 = ClAr(1) + ClAr(2) + 20

            W1 = e.Graphics.MeasureString("Reverse Charge :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_New.Rows(0).Item("Sales_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Exporter(s) Ref", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_New.Rows(0).Item("Exporters_Reference").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt_New.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            ItmNm1 = ""
            ItmNm2 = ""
            If Trim(prn_HdDt_New.Rows(0).Item("Other_Reference").ToString) <> "" Then

                ItmNm1 = Trim(prn_HdDt_New.Rows(0).Item("Other_Reference").ToString)
                ItmNm2 = ""

                If Len(ItmNm1) > 25 Then
                    For i = 25 To 1 Step -1
                        If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 25

                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
                End If


                Common_Procedures.Print_To_PrintDocument(e, "Other Reference(s) ", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Order No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_New.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            'If Trim(prn_HdDt_New.Rows(0).Item("Order_Date").ToString) <> "" Then
            strWidth = e.Graphics.MeasureString("     " & prn_HdDt_New.Rows(0).Item("Order_No").ToString, pFont).Width
            Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt_New.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1, 0, 0, pFont)
            'End If

            If Trim(ItmNm2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If

            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'Y1 = CurY + 0.5
            'Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY1 = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "CONSIGNEE : ", LMargin + 10, CurY1, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "BUYER (IF OTHER THEN CONSIGNEE) : ", LMargin + C2 + 10, CurY1, 0, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO) : ", LMargin + 10, CurY1, 0, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO) : ", LMargin + C2 + 10, CurY1, 0, 0, p1Font)
            CurY = CurY1 + TxtHgt


            '  e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            ' LnAr(3) = CurY
            ' CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt_New.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(prn_HdDt_New.Rows(0).Item("DelName").ToString <> "", "M/s. " & prn_HdDt_New.Rows(0).Item("DelName").ToString, ""), LMargin + C2 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt_New.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("DelAdd3").ToString & " " & prn_HdDt_New.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, IIf(prn_HdDt_New.Rows(0).Item("Ledger_PhoneNo").ToString <> "", " TEL : " & prn_HdDt_New.Rows(0).Item("Ledger_PhoneNo").ToString, ""), LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(prn_HdDt_New.Rows(0).Item("Del_Ledger_PhoneNo").ToString <> "", " TEL : " & prn_HdDt_New.Rows(0).Item("Del_Ledger_PhoneNo").ToString, ""), LMargin + C2 + 10, CurY, 0, 0, pFont)

            '
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt_New.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            'If Trim(prn_HdDt_New.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

            '    If Trim(prn_HdDt_New.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
            '    End If

            '    If Trim(vLedPanNo) <> "" Then
            '        strWidth = e.Graphics.MeasureString(" " & prn_HdDt_New.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    End If

            'End If

            'If Val(prn_HdDt_New.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
            '    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt_New.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            'Else
            '    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt_New.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            'End If
            'If Trim(prn_HdDt_New.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
            '    If Trim(prn_HdDt_New.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    End If
            '    If Trim(vDelvPanNo) <> "" Then
            '        strWidth = e.Graphics.MeasureString(" " & prn_HdDt_New.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    End If
            'End If



            'LnAr(3) = CurY
            ' If Trim(prn_HdDt_New.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Country of Origin   :   INDIA", LMargin + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
            '   Common_Procedures.Print_To_PrintDocument(e, "INDIA".ToString, LMargin + ClAr(1) + ClAr(2) - ClAr(3), CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt_New.Rows(0).Item("Ledger_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 65, CurY, 0, 0, pFont)
            'End If

            '  If Trim(prn_HdDt_New.Rows(0).Item("DelState_Name").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Country of Final Destination  :  " & prn_HdDt_New.Rows(0).Item("Final_Destination").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_New.Rows(0).Item("Final_Destination").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + 30, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt_New.Rows(0).Item("Delivery_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, CurY, 0, 0, pFont)
            '  End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY


            Dim Vprt_det_cap_left = 0F
            Dim Vprt_det_value_right = 0F
            Vprt_det_cap_left = ClAr(1) - 10
            Vprt_det_value_right = ClAr(1) + ClAr(2)
            pFont = New Font("Calibri", 8, FontStyle.Regular)

            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "Pre-Carriage by  : ", LMargin + Vprt_det_cap_left, CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Place of Receipt by Pre-Carrier  :  ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Terms of Delivery and Payment", LMargin + C2 + 10, CurY, 0, 0, pFont)


            If Trim(prn_HdDt_New.Rows(0).Item("pre_Carriage_by").ToString) <> "" Or Trim(prn_HdDt_New.Rows(0).Item("Place_of_receipt_by_Pre_Carrier").ToString) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_New.Rows(0).Item("pre_Carriage_by").ToString, LMargin + Vprt_det_cap_left, CurY, 2, ClAr(2), pFont, Shrink_To_Fit:=True)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_New.Rows(0).Item("Place_of_receipt_by_Pre_Carrier").ToString, LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont, Shrink_To_Fit:=True)
            End If

            CurY = CurY + TxtHgt - 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + C2, CurY)

            If Trim(prn_HdDt_New.Rows(0).Item("Terms_Payment_Delivery_Detail_1").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "1. " & prn_HdDt_New.Rows(0).Item("Terms_Payment_Delivery_Detail_1").ToString, LMargin + C2 + 10, CurY, 0, PageWidth, pFont, Shrink_To_Fit:=True)
            End If


            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Vessal/ Flight No  : ", LMargin + Vprt_det_cap_left, CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Port of Loading : ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)


            If Trim(prn_HdDt_New.Rows(0).Item("Vessal_Flight_No").ToString) <> "" Or Trim(prn_HdDt_New.Rows(0).Item("Port_Of_Loading").ToString) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_New.Rows(0).Item("Vessal_Flight_No").ToString, LMargin + Vprt_det_cap_left, CurY, 2, ClAr(2), pFont, Shrink_To_Fit:=True)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_New.Rows(0).Item("Port_Of_Loading").ToString, LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont, Shrink_To_Fit:=True)
            End If

            If Trim(prn_HdDt_New.Rows(0).Item("Terms_Payment_Delivery_Detail_2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "2. " & prn_HdDt_New.Rows(0).Item("Terms_Payment_Delivery_Detail_2").ToString, LMargin + C2 + 10, CurY, 0, PageWidth, pFont, Shrink_To_Fit:=True)
            End If

            CurY = CurY + TxtHgt - 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + C2, CurY)

            If Trim(prn_HdDt_New.Rows(0).Item("Terms_Payment_Delivery_Detail_3").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "3. " & prn_HdDt_New.Rows(0).Item("Terms_Payment_Delivery_Detail_3").ToString, LMargin + C2 + 10, CurY + 5, 0, PageWidth, pFont, Shrink_To_Fit:=True)
            End If

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Final Destination :", LMargin + Vprt_det_cap_left, CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Port of Discharge  :", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)


            If Trim(prn_HdDt_New.Rows(0).Item("Final_Destination").ToString) <> "" Or Trim(prn_HdDt_New.Rows(0).Item("Port_Of_Discharge").ToString) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_New.Rows(0).Item("Final_Destination").ToString, LMargin + Vprt_det_cap_left, CurY, 2, ClAr(2), pFont, Shrink_To_Fit:=True)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_New.Rows(0).Item("Port_Of_Discharge").ToString, LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont, Shrink_To_Fit:=True)
            End If

            CurY = CurY + TxtHgt '+ 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + W3, LnAr(4), LMargin + W3, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 25, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 25, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 25, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 25, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 70, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 70, LnAr(3))

            'Y1 = CurY + 0.5
            'Y2 = CurY + TxtHgt - 10 + TxtHgt + 10
            'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            ''***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            '***** GST START *****
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MARKS & NOS./ ", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONTAINER NO", LMargin + ClAr(1), CurY + 15, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO. & KIND OF PKGS ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIBTION OF GOODS", LMargin + ClAr(1) + ClAr(2), CurY + 15, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NET WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(1, 6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 15, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Currency_idNo").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 15, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Currency_idNo").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 15, 2, ClAr(7), pFont)
            '***** GST END *****

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Export_INV_Format10_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2font As Font
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
        Dim vGST_PERC_AMT_FOR_PRNT As String = ""
        Dim ar_GSTDET() As String, ar_GSTAMT() As String
        Dim vNOOFTAXLINES As Integer
        Dim Cmp_GSTIN_No As String

        Dim vbr As SolidBrush                  '--COMMON BRUSH FOR ALL DETAILS 
        Dim vbr_CmpName As SolidBrush          '--COMPANY TITTLE
        Dim vbr_CmpDets As SolidBrush          '--COMPANY DETAILS

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then  '----OMM ELECTRICAL CARES

                Cmp_GSTIN_No = prn_HdDt_New.Rows(0).Item("Company_GSTinNo").ToString

                If Trim(UCase(Cmp_GSTIN_No)) = "33BUPPK6766D1ZC" Then

                    vbr = New SolidBrush(Color.Blue)

                End If

            End If

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            '  e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))



            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt + 10 '- 10
                    'If Val(prn_PageNo) = 1 Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 11, FontStyle.Bold)
                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt + 2
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


            CurY = CurY - 10

            '***** GST START *****
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Total Gross Weight :  " & Format(Val(prn_HdDt.Rows(0).Item("Total_Gross_weight").ToString), "#######0.000") & " kgs ", LMargin + 20, CurY, 0, 0, pFont)

            '   Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N) : N", LMargin + 15, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then

                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                End If
            End If

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then


                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                End If
            End If

            vNOOFTAXLINES = 0
            vGST_PERC_AMT_FOR_PRNT = get_GSTPercentage_and_GSTAmount_For_Printing(EntryCode, vNOOFTAXLINES)
            'S = Trim(Dt1.Rows(I).Item("gsttaxcaption").ToString) & " " & Trim(Val(Dt1.Rows(I).Item("gstperc").ToString)) & "$^$" & Trim(Format(Val(Dt1.Rows(I).Item("gstamount").ToString), "##########0.00"))
            'vRETSTR = Trim(vRETSTR) & IIf(Trim(vRETSTR) <> "", "#^#", "") & Trim(S)
            If Trim(vGST_PERC_AMT_FOR_PRNT) <> "" Then

                ar_GSTDET = Split(vGST_PERC_AMT_FOR_PRNT, "#^#")

                For K = 0 To UBound(ar_GSTDET)
                    If Trim(ar_GSTDET(K)) <> "" Then
                        ar_GSTAMT = Split(ar_GSTDET(K), "$^$")
                        If Val(ar_GSTAMT(1)) <> 0 Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ar_GSTAMT(0)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(ar_GSTAMT(1)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        End If

                    End If
                Next K

            End If

            'If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    If is_LastPage = True Then
            '        If vTaxPerc <> 0 Then
            '            Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Else
            '            Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            'If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    If is_LastPage = True Then
            '        If vTaxPerc <> 0 Then
            '            Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Else
            '            Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            'If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    If is_LastPage = True Then
            '        If vTaxPerc <> 0 Then
            '            Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Else
            '            Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If
            ''***** GST END *****

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)

                End If
            End If



            If Yax > CurY Then
                CurY = Yax
            Else
                CurY = CurY
            End If

            CurY = CurY + TxtHgt - 10
            '   Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 20, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then

                'Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), Common_Procedures.Currency_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Currency_idNo").ToString)))



                'Rup1 = IIf(Trim(Rup1) <> "", Rup1 & " " & Common_Procedures.Currency_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Currency_idNo").ToString)), "")

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
            vNoofHsnCodes = 0
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then '---- GAJAKHARNAA TRADERS (Somanur)
            '    vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            'End If
            If vNoofHsnCodes <> 0 Then
                Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            End If

            '==========================
            '***** GST END *****

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            If Trim(prn_HdDt.Rows(0).Item("Jurisdiction").ToString) <> "" Then
                Jurs = Trim(prn_HdDt.Rows(0).Item("Jurisdiction").ToString)
            Else
                Jurs = Common_Procedures.settings.Jurisdiction
                If Trim(Jurs) = "" Then Jurs = "Tirupur"
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1225" Then  '--- Yasen Tex (Mangalam)
                Dim vLightGreenBrush As New SolidBrush(Color.FromArgb(171, 206, 26))
                p1Font = New Font("Cambria", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font, vLightGreenBrush)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1460" Then
                p1Font = New Font("Times New Roman", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then
                p2font = New Font("bodoni mt black", 12, FontStyle.Bold)
                If Trim(UCase(Cmp_GSTIN_No)) = "33BUPPK6766D1ZC" Then
                    Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p2font, vbr)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                End If
            Else
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            End If
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            If (Trim(Common_Procedures.settings.CustomerCode)) = "1460" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Terms & Conditions :", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " 1. Any Complaint regarding goods must be in willing within 2 days.", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " 2. Interest @ 24% will be charged if payment not made within due date.", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " 3. Any Claims out of this Sale is Subject to " & Trim(Jurs) & " Jurisdiction.", LMargin + 10, CurY, 0, 0, pFont)

            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1551" Then

                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, " Terms & Conditions :", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " 1. Overdue interest will be charged at 24% from the invoice date.", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " 2. We are not responsible for any loss or damage in transit.", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " 3. Subject to " & Trim(Jurs) & " jurisdiction.", LMargin + 10, CurY, 0, 0, pFont)

            Else

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, pFont)
                ' CurY = CurY + TxtHgt
                ' Common_Procedures.Print_To_PrintDocument(e, "We Declare that this Invoice Shows the actual price of the goods", LMargin + 10, CurY, 0, 0, pFont)
                ' CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, pFont)

            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15

            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            If (Trim(Common_Procedures.settings.CustomerCode)) <> "1460" And Trim(Common_Procedures.settings.CustomerCode) <> "1551" Then
                ' Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Jurs & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)
            End If

            If Print_PDF_Status = True Then
                CurY = CurY + TxtHgt - 15
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub cbo_TransportMode_TextChanged(sender As Object, e As EventArgs) Handles cbo_TransportMode.TextChanged
        If Trim(cbo_TransportMode.Text) <> "" Then
            cbo_Pre_Carriage_by.Text = cbo_TransportMode.Text
        End If
    End Sub

    Private Sub cbo_Pre_Carriage_by_TextChanged(sender As Object, e As EventArgs) Handles cbo_Pre_Carriage_by.TextChanged
        If Trim(cbo_Pre_Carriage_by.Text) <> "" Then
            cbo_TransportMode.Text = cbo_Pre_Carriage_by.Text
        End If
    End Sub

    Private Sub btn_Terms_Click(sender As Object, e As EventArgs) Handles btn_Terms.Click
        pnl_Terms.Visible = True
        pnl_Terms.BringToFront()
        pnl_Back.Enabled = False
        pnl_EXport_Inv_Port_Details.Enabled = False
        txt_Terms_Delivery_Payment_1.Focus()
    End Sub

    Private Sub btn_terms_Close_Click(sender As Object, e As EventArgs) Handles btn_terms_Close.Click
        pnl_Terms.Visible = False
        pnl_EXport_Inv_Port_Details.Enabled = True
        txt_Exports_Ref.Focus()
    End Sub

    Private Sub Cbo_Colour_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Cbo_Colour_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Colour, txt_No_of_Packs, "Colour_Head", "Colour_Name", "", "(Colour_Idno = 0)")
    End Sub

    Private Sub Cbo_Colour_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Colour, cbo_Size, txt_No_of_Packs, "Colour_Head", "Colour_Name", "", "(Colour_Idno = 0)")
    End Sub

    Private Sub Cbo_Colour_Enter(sender As Object, e As EventArgs) Handles Cbo_Colour.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_Idno = 0)")
    End Sub

    Private Sub cbo_Size_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Size.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Size_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, Cbo_Colour, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")
    End Sub

    Private Sub cbo_Size_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Size.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Size, cbo_Unit, Cbo_Colour, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")
    End Sub

    Private Sub cbo_Size_GotFocus(sender As Object, e As EventArgs) Handles cbo_Size.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")
    End Sub





    Private Sub txt_OrderDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_OrderDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_TransportMode.Focus()
        End If

    End Sub

    Private Sub txt_Order_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Order_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Gross_Wgt.Focus()
        End If
    End Sub

    Private Sub txt_Order_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Order_No.KeyDown
        If e.KeyCode = 40 Then
            txt_Gross_Wgt.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_GSTRate.Focus()
        End If
    End Sub


    Private Sub txt_No_of_Packs_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_No_of_Packs.KeyPress

        If Asc(e.KeyChar) = 13 Then
            txt_No_Of_Pcs_Per_Packs.Focus()
        End If

    End Sub

    Private Sub txt_No_of_Packs_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_No_of_Packs.KeyDown
        If e.KeyCode = 40 Then
            txt_No_Of_Pcs_Per_Packs.Focus()
        End If

        If e.KeyCode = 38 Then
            Cbo_Colour.Focus()
        End If

    End Sub



    Private Sub txt_No_Of_Pcs_Per_Packs_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_No_Of_Pcs_Per_Packs.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_NoofItems.Focus()
        End If
    End Sub

    Private Sub txt_No_Of_Pcs_Per_Packs_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_No_Of_Pcs_Per_Packs.KeyDown
        If e.KeyCode = 40 Then
            txt_NoofItems.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_No_of_Packs.Focus()
        End If

    End Sub

    Private Sub txt_Container_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Package_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_SerialNo.Focus()
        End If
    End Sub

    Private Sub txt_Container_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Package_No.KeyDown

        If e.KeyCode = 40 Then
            txt_SerialNo.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Net_wgt.Focus()
        End If

    End Sub


    Private Sub Quantity_Calculation()

        Dim vQty As Integer
        Dim vNo_Of_Packs As Integer
        Dim vNo_Of_Pcs_Per_Packs As Integer

        vNo_Of_Packs = Val(txt_No_of_Packs.Text)
        vNo_Of_Pcs_Per_Packs = Val(txt_No_Of_Pcs_Per_Packs.Text)

        vQty = Val(vNo_Of_Packs * vNo_Of_Pcs_Per_Packs)

        txt_NoofItems.Text = Val(vQty)


    End Sub

    Private Sub txt_No_of_Packs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_No_of_Packs.TextChanged
        Quantity_Calculation()
    End Sub

    Private Sub txt_No_Of_Pcs_Per_Packs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_No_Of_Pcs_Per_Packs.TextChanged
        Quantity_Calculation()
    End Sub

    Private Sub txt_Place_Of_Supply_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Place_Of_Supply.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_DeliveryTo.Focus()
        End If
    End Sub

    Private Sub txt_Place_Of_Supply_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Place_Of_Supply.KeyDown
        If e.KeyCode = 40 Then
            cbo_DeliveryTo.Focus()
        End If

        If e.KeyCode = 38 Then
            cbo_TransportMode.Focus()
        End If

    End Sub

    Private Sub txt_Order_No_TextChanged(sender As Object, e As EventArgs) Handles txt_Order_No.TextChanged

    End Sub

    Private Sub txt_Gross_Wgt_TextChanged(sender As Object, e As EventArgs) Handles txt_Gross_Wgt.TextChanged

    End Sub

    Private Sub txt_Gross_Wgt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Gross_Wgt.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Net_wgt.Focus()
        End If
    End Sub

    Private Sub txt_Gross_Wgt_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Gross_Wgt.KeyDown
        If e.KeyCode = 40 Then
            txt_Net_wgt.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Order_No.Focus()
        End If

    End Sub

    Private Sub txt_Net_wgt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Net_wgt.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Package_No.Focus()
        End If
    End Sub

    Private Sub txt_Net_wgt_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Net_wgt.KeyDown

        If e.KeyCode = 40 Then
            txt_Package_No.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Gross_Wgt.Focus()
        End If

    End Sub

    Private Sub txt_NoofItems_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_NoofItems.KeyDown

        If e.KeyCode = 40 Then
            txt_Rate.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_No_Of_Pcs_Per_Packs.Focus()
        End If

    End Sub

    Private Sub cbo_DeliveryTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_DeliveryTo.SelectedIndexChanged

    End Sub

    Private Sub txt_Due_Days_TextChanged(sender As Object, e As EventArgs) Handles txt_Due_Days.TextChanged

    End Sub

    Private Sub txt_VehicleNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_VehicleNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_VehicleNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_VehicleNo.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If


        If e.KeyCode = 38 Then
            txt_Freight.Focus()
        End If

    End Sub



    Private Sub dtp_Lrdate_TextChanged(sender As Object, e As EventArgs) Handles dtp_Lrdate.TextChanged

        Try
            If FrmLdSTS = True Then Exit Sub
            If Me.ActiveControl.Name <> msk_Lr_Date.Name Then
                If IsDate(dtp_Lrdate.Text) = True Then
                    msk_Lr_Date.Text = dtp_Lrdate.Text
                    msk_Lr_Date.SelectionStart = 0
                End If
            End If

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub dtp_Lrdate_KeyUp(sender As Object, e As KeyEventArgs) Handles dtp_Lrdate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Lrdate.Text = Date.Today
        End If
    End Sub



    Private Sub msk_Lr_Date_TextChanged(sender As Object, e As EventArgs) Handles msk_Lr_Date.TextChanged
        Try


            If FrmLdSTS = True Then Exit Sub

            If Me.ActiveControl.Name <> dtp_Lrdate.Name Then
                If IsDate(msk_Lr_Date.Text) = True Then
                    dtp_Lrdate.Value = Convert.ToDateTime(msk_Lr_Date.Text)
                End If
            End If

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub msk_Lr_Date_LostFocus(sender As Object, e As EventArgs) Handles msk_Lr_Date.LostFocus
        If IsDate(msk_Lr_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Lr_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Lr_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Lr_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Lr_Date.Text)) >= 2000 Then
                    dtp_Lrdate.Value = Convert.ToDateTime(msk_Lr_Date.Text)
                End If
            End If
        End If
    End Sub

    Private Sub msk_Lr_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_Lr_Date.KeyUp
        If IsDate(msk_Lr_Date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Lr_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Lr_Date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_Lr_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Lr_Date.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskLrText, vmskLrStrt)
        End If
    End Sub

    Private Sub msk_Lr_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_Lr_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Lr_Date.Text = Date.Today
            msk_Lr_Date.SelectionStart = 0
        End If



        If Asc(e.KeyChar) = 13 Then
            cbo_ItemName.Focus()
        End If

    End Sub

    Private Sub msk_Lr_Date_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_Lr_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskLrText = ""
        vmskLrStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskLrText = msk_Lr_Date.Text
            vmskLrStrt = msk_Lr_Date.SelectionStart
        End If


        If e.KeyCode = 40 Then
            cbo_ItemName.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_LrNo.Focus()
        End If

    End Sub

    Private Sub txt_LrNo_TextChanged(sender As Object, e As EventArgs) Handles txt_LrNo.TextChanged

    End Sub

    Private Sub txt_LrNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_LrNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            msk_Lr_Date.Focus()
        End If
    End Sub

    Private Sub txt_LrNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_LrNo.KeyDown

        If e.KeyCode = 40 Then
            msk_Lr_Date.Focus()
        End If

        If e.KeyCode = 38 Then
            cbo_DispatcherName.Focus()
        End If

    End Sub


    Private Sub dtp_Lrdate_Enter(sender As Object, e As EventArgs) Handles dtp_Lrdate.Enter
        msk_Lr_Date.Focus()
        msk_Lr_Date.SelectionStart = 0
    End Sub


    Private Sub Printing_PackingList_FinishedProduct_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim vFOOTER_topY As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim vNoofHsnCodes As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String
        Dim clrNm1 As String, clrNm2 As String, clrNm3 As String
        Dim vFOOTR_NOOFLINES As Integer
        Dim vLine_Pen As Pen
        Dim vFontName As String = ""

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15
            .Right = 40
            .Top = 35
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        'With PrintDocument1.DefaultPageSettings.PaperSize
        '    PrintWidth = (.Width / 2) - RMargin - LMargin
        '    PrintHeight = (.Height / 2) - TMargin - BMargin
        '    PageWidth = (.Width / 2) - RMargin
        '    PageHeight = (.Height / 2) - BMargin
        'End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'ClArr(0) = 0
        'ClArr(1) = 45 : ClArr(2) = 125 : ClArr(3) = 70 : ClArr(4) = 90 : ClArr(5) = 75 : ClArr(6) = 70 : ClArr(7) = 70 : ClArr(8) = 60 : ClArr(9) = 80
        'ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))


        ClArr(0) = 0
        ClArr(1) = 45 : ClArr(2) = 70 : ClArr(3) = 70 : ClArr(4) = 80 : ClArr(5) = 70 : ClArr(6) = 75 : ClArr(7) = 70 : ClArr(8) = 70 : ClArr(9) = 60 : ClArr(10) = 80
        ClArr(11) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10))


        TxtHgt = 18 '19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        vLine_Pen = New Pen(Color.Black, 2)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                'If prn_HdMxIndx > 0 Then

                'Erase LnAr
                '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

                Printing_PackingList_FinishedProduct_Format1_PageHeader(PrintDocument1, e, prn_HdDt, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, prn_HdIndx)
                'CurY = CurY - 10

                NoofDets = 0

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If prn_DetDt.Rows.Count > 0 Then

                            If prn_PageNo <= 1 Then

                                NoofItems_PerPage = 15 ' 15 

                            Else
                                NoofItems_PerPage = 24

                            End If

                            vFOOTR_NOOFLINES = 10
                            vFOOTER_topY = (vFOOTR_NOOFLINES * TxtHgt)

                            If prn_DetIndx >= (prn_DetDt.Rows.Count - 2) Then

                                If CurY >= 800 Or (CurY + (vFOOTR_NOOFLINES * TxtHgt)) >= (PageHeight - TxtHgt) Then

                                    If CurY < (PageHeight - TxtHgt - TxtHgt - 10) Then
                                        CurY = PageHeight - TxtHgt - TxtHgt - TxtHgt - 10
                                    End If

                                    CurY = CurY + 10
                                    Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)

                                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                                    e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                                    e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), LnAr(4))
                                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), LnAr(4))


                                    p1Font = New Font(vFontName, 8, FontStyle.Regular)
                                    Common_Procedures.Print_To_PrintDocument(e, "Page No. " & prn_PageNo, LMargin, CurY + 5, 2, PrintWidth, p1Font)

                                    e.HasMorePages = True
                                    Return

                                End If


                            ElseIf CurY >= (PageHeight - TxtHgt - TxtHgt) Then

                                If CurY < (PageHeight - TxtHgt - TxtHgt - 10) Then
                                    CurY = PageHeight - TxtHgt - TxtHgt - 10
                                End If

                                CurY = CurY + 10
                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)

                                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                                e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                                e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


                                p1Font = New Font(vFontName, 8, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, "Page No. " & prn_PageNo, LMargin, CurY + 5, 2, PrintWidth, p1Font)

                                e.HasMorePages = True
                                Return

                            End If

                        End If


                        '-------------------------------------------------------------------------------------------


                        'If prn_PageNo <= 1 Then

                        '    NoofItems_PerPage = 18 '12 ' 15 

                        'Else
                        '    NoofItems_PerPage = 20

                        'End If

                        'vFOOTR_NOOFLINES = 10

                        'If prn_DetIndx >= (prn_DetDt.Rows.Count - 2) Then

                        '    If CurY >= 700 Or (CurY + (vFOOTR_NOOFLINES * TxtHgt)) >= (PageHeight - TxtHgt) Then

                        '        If CurY < (PageHeight - TxtHgt - TxtHgt - 10) Then
                        '            CurY = PageHeight - TxtHgt - TxtHgt - 10
                        '        End If

                        '        CurY = CurY + 10
                        '        Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)

                        '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                        '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(4))
                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(4))
                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(4))
                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(4))
                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(4))
                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(4))
                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(4))
                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(4))
                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), LnAr(4))
                        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), LnAr(4))

                        '        p1Font = New Font(vFontName, 8, FontStyle.Regular)
                        '        Common_Procedures.Print_To_PrintDocument(e, "Page No. " & prn_PageNo, LMargin, CurY + 5, 2, PrintWidth, p1Font)

                        '        e.HasMorePages = True
                        '        Return

                        '    End If


                        'ElseIf CurY >= (PageHeight - TxtHgt - TxtHgt) Then

                        '    If CurY < (PageHeight - TxtHgt - TxtHgt - 10) Then
                        '        CurY = PageHeight - TxtHgt - TxtHgt - 10
                        '    End If

                        '    CurY = CurY + 10
                        '    Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)

                        '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        '    e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                        '    e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


                        '    p1Font = New Font(vFontName, 8, FontStyle.Regular)
                        '    Common_Procedures.Print_To_PrintDocument(e, "Page No. " & prn_PageNo, LMargin, CurY + 5, 2, PrintWidth, p1Font)

                        '    e.HasMorePages = True
                        '    Return

                        'End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Item_Nm").ToString)

                        ItmNm2 = ""
                        If Len(ItmNm1) > 10 Then
                            For I = 10 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 10
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If



                        If Len(ItmNm2) > 8 Then

                            For I = 8 To 1 Step -1
                                If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 8

                            ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                            ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)

                        End If


                        clrNm1 = Common_Procedures.Colour_IdNoToName(con, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_Idno").ToString))

                        clrNm2 = ""
                        clrNm3 = ""

                        If Len(clrNm1) > 8 Then
                            For I = 8 To 1 Step -1
                                If Mid$(Trim(clrNm1), I, 1) = " " Or Mid$(Trim(clrNm1), I, 1) = "," Or Mid$(Trim(clrNm1), I, 1) = "." Or Mid$(Trim(clrNm1), I, 1) = "-" Or Mid$(Trim(clrNm1), I, 1) = "/" Or Mid$(Trim(clrNm1), I, 1) = "_" Or Mid$(Trim(clrNm1), I, 1) = "(" Or Mid$(Trim(clrNm1), I, 1) = ")" Or Mid$(Trim(clrNm1), I, 1) = "\" Or Mid$(Trim(clrNm1), I, 1) = "[" Or Mid$(Trim(clrNm1), I, 1) = "]" Or Mid$(Trim(clrNm1), I, 1) = "{" Or Mid$(Trim(clrNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 8

                            clrNm2 = Microsoft.VisualBasic.Right(Trim(clrNm1), Len(clrNm1) - I)
                            clrNm1 = Microsoft.VisualBasic.Left(Trim(clrNm1), I - 1)
                        End If

                        If Len(clrNm2) > 8 Then

                            For I = 8 To 1 Step -1
                                If Mid$(Trim(clrNm2), I, 1) = " " Or Mid$(Trim(clrNm2), I, 1) = "," Or Mid$(Trim(clrNm2), I, 1) = "." Or Mid$(Trim(clrNm2), I, 1) = "-" Or Mid$(Trim(clrNm2), I, 1) = "/" Or Mid$(Trim(clrNm2), I, 1) = "_" Or Mid$(Trim(clrNm2), I, 1) = "(" Or Mid$(Trim(clrNm2), I, 1) = ")" Or Mid$(Trim(clrNm2), I, 1) = "\" Or Mid$(Trim(clrNm2), I, 1) = "[" Or Mid$(Trim(clrNm2), I, 1) = "]" Or Mid$(Trim(clrNm2), I, 1) = "{" Or Mid$(Trim(clrNm2), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 8

                            clrNm3 = Microsoft.VisualBasic.Right(Trim(clrNm2), Len(clrNm2) - I)
                            clrNm2 = Microsoft.VisualBasic.Left(Trim(clrNm2), I - 1)

                        End If

                        CurY = CurY + TxtHgt

                        pFont = New Font("Calibri", 10, FontStyle.Regular)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Size_IdNoToName(con, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Size_idno").ToString)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(clrNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_No").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Pcs_per_Packs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Packs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Package_No").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Gross_Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) - 10, CurY, 1, 0, pFont)

                        If Trim(ItmNm2) <> "" Or Trim(clrNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(clrNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If


                        If Trim(clrNm3) <> "" Or Trim(ItmNm3) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(clrNm3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Order_No").ToString) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, "Order No : " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Order_No").ToString), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        'End If

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_PackingList_FinishedProduct_Format1_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub


    Private Sub Printing_PackingList_FinishedProduct_Format1_PageHeader(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal prn_HdIndx As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font, p2font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Cmp_UAMNO As String = ""

        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0, k As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""

        Dim vbr As SolidBrush                  '--COMMON BRUSH FOR ALL DETAILS 
        Dim vbr_CmpName As SolidBrush          '--COMPANY TITTLE
        Dim vbr_CmpDets As SolidBrush          '--COMPANY DETAILS

        Dim EntryCode As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
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

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST", LMargin, CurY - TxtHgt - 4, 2, PrintWidth, p1Font)

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY


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



        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 20, 90, 90)

                        End If

                    End Using
                End If
            End If

        End If

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_UAMNO = ""

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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
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

        '***********************

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then  '----OMM ELECTRICAL CARES
            If Trim(UCase(Cmp_GSTIN_No)) = "33BUPPK6766D1ZC" Then

                vbr_CmpName = New SolidBrush(Color.Blue)
                vbr_CmpDets = New SolidBrush(Color.Green)

                p2font = New Font("bodoni mt black", 18, FontStyle.Bold)
            End If
        End If

        '***********************

        CurY = CurY + TxtHgt - 15

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1225" Then  '--- Yasen Tex (Mangalam)
            Dim vLightGreenBrush As New SolidBrush(Color.FromArgb(171, 206, 26))
            p1Font = New Font("Cambria", 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, vLightGreenBrush)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1460" Then
            p1Font = New Font("Times New Roman", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then
            If Trim(UCase(Cmp_GSTIN_No)) = "33BUPPK6766D1ZC" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p2font, vbr_CmpName)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            End If
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpDets)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpDets)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, vbr_CmpDets)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, vbr_CmpDets)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, vbr_CmpDets)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, vbr_CmpDets)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font, vbr_CmpDets)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont, vbr_CmpDets)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, vbr_CmpDets)

        If Trim(Cmp_UAMNO) <> "" Then
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, p1Font, vbr_CmpDets)
        End If

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY - 10, PageWidth, CurY - 10)
            LnAr(2) = CurY


            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)
            ItmNm2 = ""

            If Len(ItmNm1) > 35 Then
                For i = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
            End If

            CurY = CurY - 5

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)               :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("FinishedProduct_invoice_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Transportation_Mode").ToString) = "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ROAD ", LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("FinishedProduct_invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1508" Then

            Common_Procedures.Print_To_PrintDocument(e, "Inco Term", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Inco_Term").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Country Of Origin", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "India", LMargin + W1 + 30, CurY1, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Destination", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If


            CurY1 = CurY1 + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Country Of Final Destination ", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Final_Destination").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Final_Destination").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Other Reference", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_Reference").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Port Of Loading", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Port_Of_Loading").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Port Of Discharge", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Port_Of_Discharge").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Place Of Receipt", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_of_receipt_by_Pre_Carrier").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY1 = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO) : ", LMargin + C2 + 10, CurY1, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO) : ", LMargin + 10, CurY1, 0, 0, p1Font)
            CurY = CurY1 + TxtHgt

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString & " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12

            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            Else
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            End If
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
                If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If
                If Trim(vDelvPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If
            End If



            CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + C2 + 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + S1 + 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 40, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))

            ''***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            '***** GST START *****
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESC", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ORDER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "NO.OF PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "/PKG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "NO.OF ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PKG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "PKG NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "NET", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WGT(KGS)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "GROSS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WGT(KGS)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY + TxtHgt, 2, ClAr(11), pFont)

            '***** GST END *****

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_PackingList_FinishedProduct_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal prn_HdIndx As Integer, ByVal is_LastPage As Boolean)
        Dim I As Integer
        Dim p1Font As Font

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY




            If is_LastPage = True Then

                'Dim vPACKTYPE As String = ""
                'vPACKTYPE = prn_HdDt.Rows(0).Item("Packing_Type").ToString
                'If Trim(vPACKTYPE) = "" Then
                '    vPACKTYPE = "BALES"
                'Else
                '    vPACKTYPE = Trim(vPACKTYPE) & "S"
                'End If

                CurY = CurY + TxtHgt - 10

                Common_Procedures.Print_To_PrintDocument(e, "TOTAL ", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_No_Of_Packs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Gross_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(7) = CurY
            End If





            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(4))


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 5, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 10, FontStyle.Bold)


            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Decleration :  ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "We declare that this document shows the actual goods described and the ", LMargin + ClAr(1) + 50, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "particulars are true & correct", LMargin + 10, CurY, 2, ClAr(2), p1Font)




            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(7))


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub btn_Print_Bundle_Click(sender As Object, e As EventArgs) Handles btn_Print_Bundle.Click
        Format_2_Status = 1
        Print_PDF_Status = False
        btn_Close_Print_Click(sender, e)
        Print_Invoice()
    End Sub

    Private Sub btn_Print_Invoice_Click(sender As Object, e As EventArgs) Handles btn_Print_Invoice.Click
        Format_2_Status = 0
        Print_PDF_Status = False
        btn_Close_Print_Click(sender, e)
        Print_Invoice()
    End Sub

    Private Sub btn_Print_Cancel_Click(sender As Object, e As EventArgs) Handles btn_Print_Cancel.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Close_Print_Click(sender As Object, e As EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Enter(sender As Object, e As EventArgs) Handles btn_Print_Invoice.Enter
        btn_Print_Invoice.BackColor = Color.Lime
        btn_Print_Invoice.ForeColor = Color.Blue
    End Sub

    Private Sub btn_Print_Invoice_Leave(sender As Object, e As EventArgs) Handles btn_Print_Invoice.Leave
        btn_Print_Invoice.BackColor = Color.FromArgb(5, 50, 110)
        btn_Print_Invoice.ForeColor = Color.White
    End Sub

    Private Sub btn_Print_Bundle_Enter(sender As Object, e As EventArgs) Handles btn_Print_Bundle.Enter
        btn_Print_Bundle.BackColor = Color.Lime
        btn_Print_Bundle.ForeColor = Color.Blue
    End Sub

    Private Sub btn_Print_Bundle_Leave(sender As Object, e As EventArgs) Handles btn_Print_Bundle.Leave
        btn_Print_Bundle.BackColor = Color.FromArgb(5, 50, 110)
        btn_Print_Bundle.ForeColor = Color.White
    End Sub

    Private Sub btn_Print_Cancel_Enter(sender As Object, e As EventArgs) Handles btn_Print_Cancel.Enter
        btn_Print_Cancel.BackColor = Color.Lime
        btn_Print_Cancel.ForeColor = Color.Blue
    End Sub

    Private Sub btn_Print_Cancel_Leave(sender As Object, e As EventArgs) Handles btn_Print_Cancel.Leave
        btn_Print_Cancel.BackColor = Color.FromArgb(255, 90, 90)
        btn_Print_Cancel.ForeColor = Color.White
    End Sub

End Class




