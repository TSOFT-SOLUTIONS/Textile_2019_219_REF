Public Class Yarn_Sales_Order_GST

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GYPRO-"
    Private Pk_Condition2 As String = "GYPOA-"
    Private Pk_Condition3 As String = "GYPOF-"
    Private Pk_Condition4 As String = "GYPOT-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private prn_Status As Integer = 0
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
    Public vmskBillOldText As String = ""
    Public vmskBillSelStrt As Integer = -1

    Public vmsDelyOldTxt As String = ""
    Public vmsDelySelStrt As String = ""


    Private Print_PDF_Status As Boolean = False
    Dim ItmNm1 As String, ItmNm2 As String


    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub
    Private Enum dgvCol_Details As Integer
        SlNo
        Count
        Mill_Name
        Bag
        cone_per_bag
        cones
        Wt_per_bag
        weight
        Rate_for
        rate
        amount
        Colour
        Detail_slno
        purchase_bags
        purchase_cone
        purchase_weight
        Disc_perc
        Disc_AMt
        Taxable_value
        Gst_Perc
        Hsn_Code
        Ex_Mill
        Cancel_Order
        invoice_bags
        invoice_Cones
        invoice_wgt


    End Enum
    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False
        chk_Verified_Status.Checked = False
        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        pnl_Filter.Visible = False
        pnl_Tax.Visible = False
        Print_PDF_Status = False

        vmskOldText = ""
        vmskSelStrt = -1
        vmskBillOldText = ""
        vmskBillSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        cbo_Through.Text = "DIRECT"
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        lbl_OrderNo.Text = ""
        lbl_OrderCode.Text = ""
        cbo_Agent.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_Delvat.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        txt_CommRate.Text = ""
        cbo_CommType.Text = "BAG"
        lbl_CommAmount.Text = ""
        txt_Sales_Order_No.Text = ""

        lbl_GrossAmount.Text = ""
        txt_Attend.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""

        txt_AssessableValue.Text = ""

        cbo_TaxType.Text = "GST"
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""

        txt_Freight.Text = ""
        txt_AddLessAfterTax_Text.Text = "Add/Less"
        txt_AddLess_AfterTax.Text = ""
        txt_AddLess_BeforeTax.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        lbl_AmountInWords.Text = "Rupees  :  "

        cbo_Transport.Text = ""
        txt_Note.Text = ""
        txt_DueDays.Text = ""
        txt_PaymenTerms.Text = ""

        Cbo_DeliveryAddress.Text = ""
        msk_DeliveryDate.Text = ""
        dtp_DeliveryDate.Text = ""

        msk_grdate.Text = ""
        dtp_gr_date.Text = ""

        cbo_transportation.Text = ""
        cbo_tax_type.Text = ""
        chk_CloseStatus.Checked = False

        lbl_UserName_CreatedBy.Text = ""
        lbl_UserName_ModifiedBy.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Tax_Details.Rows.Clear()
        dgv_Tax_Total_Details.Rows.Clear()
        dgv_Tax_Total_Details.Rows.Add()
        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_DeliveryAt.Text = ""
            cbo_Filter_Count.Text = ""
            cbo_Filter_Count.SelectedIndex = -1
            cbo_Filter_DeliveryAt.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Agent.Enabled = True
        cbo_Agent.BackColor = Color.White

        cbo_Delvat.Enabled = True
        cbo_Delvat.BackColor = Color.White

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_VehicleNo.Enabled = True
        cbo_VehicleNo.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        cbo_Grid_CountName.Enabled = True
        cbo_Grid_CountName.BackColor = Color.White

        cbo_Grid_MillName.Enabled = True
        cbo_Grid_MillName.BackColor = Color.White

        cbo_Colour.Visible = False
        cbo_Colour.Tag = -1
        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim mskdtxbx As MaskedTextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_RateFor.Name Then
            cbo_Grid_RateFor.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.DeepPink
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If
    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
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
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Yarn_Sales_Order_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Order_Code = '" & Trim(NewCode) & "' and Entry_VAT_GST_Type = 'GST'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Yarn_Sales_Order_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Yarn_Sales_Order_Date")
                msk_Date.Text = dtp_Date.Text

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_Delvat.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_Idno").ToString))
                cbo_Through.Text = dt1.Rows(0).Item("Through_Name").ToString
                txt_CommRate.Text = Val(dt1.Rows(0).Item("Agent_Commission_Rate").ToString)
                cbo_CommType.Text = dt1.Rows(0).Item("Agent_Commission_Type").ToString
                lbl_CommAmount.Text = dt1.Rows(0).Item("Agent_Commission_Commission").ToString

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                txt_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")

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


                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "#########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "#########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "#########0.00")

                txt_AddLess_BeforeTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), "#########0.00")
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
                txt_AddLessAfterTax_Text.Text = dt1.Rows(0).Item("AddLessAfterTax_Text").ToString
                If Trim(txt_AddLessAfterTax_Text.Text) = "" Then txt_AddLessAfterTax_Text.Text = "Add/Less"
                txt_AddLess_AfterTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_Attend.Text = dt1.Rows(0).Item("Attend").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                Cbo_DeliveryAddress.Text = Trim(dt1.Rows(0).Item("Delivery_Address").ToString)
                txt_DueDays.Text = Val(dt1.Rows(0).Item("Due_Days").ToString)
                txt_PaymenTerms.Text = Trim(dt1.Rows(0).Item("Payment_Terms").ToString)
                dtp_DeliveryDate.Text = dt1.Rows(0).Item("Delivery_Date").ToString
                msk_DeliveryDate.Text = dtp_DeliveryDate.Text

                cbo_tax_type.Text = Trim(dt1.Rows(0).Item("taxtype").ToString)

                cbo_transportation.Text = Trim(dt1.Rows(0).Item("Transportations").ToString)
                txt_Sales_Order_No.Text = Trim(dt1.Rows(0).Item("Party_OrderNo").ToString)

                dtp_gr_date.Text = dt1.Rows(0).Item("Gr_Date").ToString
                msk_grdate.Text = dtp_gr_date.Text

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                lbl_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                lbl_OrderCode.Text = dt1.Rows(0).Item("Own_Order_Code").ToString

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True
                If Val(dt1.Rows(0).Item("Order_Close_Status").ToString) = 1 Then chk_CloseStatus.Checked = True


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Mill_Name, c.Count_name , D.Colour_Name from Yarn_Sales_Order_Details a INNER JOIN Mill_Head b ON a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN COLOUR_Head d ON a.Colour_IdNo = d.Colour_IdNo Where a.Yarn_Sales_Order_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(dgvCol_Details.SlNo).Value = Val(SNo)
                            .Rows(n).Cells(dgvCol_Details.Count).Value = dt2.Rows(i).Item("Count_Name").ToString
                            .Rows(n).Cells(dgvCol_Details.Mill_Name).Value = dt2.Rows(i).Item("Mill_Name").ToString
                            .Rows(n).Cells(dgvCol_Details.Bag).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                            .Rows(n).Cells(dgvCol_Details.cones).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                            .Rows(n).Cells(dgvCol_Details.weight).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            .Rows(n).Cells(dgvCol_Details.Rate_for).Value = dt2.Rows(i).Item("Rate_For").ToString
                            .Rows(n).Cells(dgvCol_Details.rate).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            .Rows(n).Cells(dgvCol_Details.amount).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            If dgv_Details.Columns(dgvCol_Details.Colour).Visible = True Then
                                .Rows(n).Cells(dgvCol_Details.Colour).Value = dt2.Rows(i).Item("Colour_Name").ToString
                            End If
                            .Rows(n).Cells(dgvCol_Details.Detail_slno).Value = Val(dt2.Rows(i).Item("Yarn_Sales_Order_Details_Slno").ToString)
                            .Rows(n).Cells(dgvCol_Details.purchase_bags).Value = Val(dt2.Rows(i).Item("Purchase_Bags").ToString)
                            .Rows(n).Cells(dgvCol_Details.purchase_cone).Value = Val(dt2.Rows(i).Item("Purchase_Cones").ToString)
                            .Rows(n).Cells(dgvCol_Details.purchase_weight).Value = Format(Val(dt2.Rows(i).Item("Purchase_Weight").ToString), "########0.000")
                            .Rows(n).Cells(dgvCol_Details.invoice_bags).Value = Val(dt2.Rows(i).Item("invoice_Bags").ToString)
                            .Rows(n).Cells(dgvCol_Details.invoice_Cones).Value = Val(dt2.Rows(i).Item("invoice_Cones").ToString)
                            .Rows(n).Cells(dgvCol_Details.invoice_wgt).Value = Format(Val(dt2.Rows(i).Item("invoice_Weight").ToString), "########0.000")


                            'If Trim(Common_Procedures.settings.CustomerCode) <> "1152" Then
                            If Val(.Rows(n).Cells(dgvCol_Details.purchase_weight).Value) <> 0 Then
                                For j = 0 To .ColumnCount - 1

                                    If j <> dgvCol_Details.Bag And j <> dgvCol_Details.cones And j <> dgvCol_Details.weight And j <> dgvCol_Details.Cancel_Order Then
                                        .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                    End If

                                Next j
                                LockSTS = True
                            End If
                            'End If

                            If Val(.Rows(n).Cells(dgvCol_Details.invoice_wgt).Value) <> 0 Then
                                For j = 0 To .ColumnCount - 1
                                    If j <> dgvCol_Details.Bag And j <> dgvCol_Details.cones And j <> dgvCol_Details.weight And j <> dgvCol_Details.Cancel_Order Then
                                        .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                    End If

                                Next j
                                LockSTS = True
                            End If

                            .Rows(n).Cells(dgvCol_Details.Disc_perc).Value = Val(dt2.Rows(i).Item("Cash_Discount_Percentage").ToString)
                            .Rows(n).Cells(dgvCol_Details.Disc_AMt).Value = Val(dt2.Rows(i).Item("Cash_Discount_Amount").ToString)
                            .Rows(n).Cells(dgvCol_Details.Taxable_value).Value = Val(dt2.Rows(i).Item("Taxable_Value").ToString)
                            .Rows(n).Cells(dgvCol_Details.Gst_Perc).Value = Val(dt2.Rows(i).Item("GST_Percentage").ToString)
                            .Rows(n).Cells(dgvCol_Details.Hsn_Code).Value = Val(dt2.Rows(i).Item("HSN_Code").ToString)
                            .Rows(n).Cells(dgvCol_Details.Ex_Mill).Value = Trim(dt2.Rows(i).Item("Ex_Mill").ToString)
                            .Rows(n).Cells(dgvCol_Details.Cancel_Order).Value = Format(Val(dt2.Rows(i).Item("Cancelled_Weight").ToString), "##########0.000")


                            .Rows(n).Cells(dgvCol_Details.cone_per_bag).Value = Val(dt2.Rows(i).Item("Cones_per_Bag").ToString)
                            .Rows(n).Cells(dgvCol_Details.Wt_per_bag).Value = Format(Val(dt2.Rows(i).Item("Wgt_per_Bag").ToString), "########0.00")
                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()


                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                End With

                da4 = New SqlClient.SqlDataAdapter("Select a.* from Yarn_Sales_Order_GST_Tax_Details a Where a.Yarn_Sales_Order_Code = '" & Trim(NewCode) & "' ", con)
                dt4 = New DataTable
                da4.Fill(dt4)

                With dgv_Tax_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For I = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = SNo
                            .Rows(n).Cells(1).Value = Trim(dt4.Rows(I).Item("HSN_Code").ToString)
                            .Rows(n).Cells(2).Value = IIf(Val(dt4.Rows(I).Item("Taxable_Amount").ToString) <> 0, Format(Val(dt4.Rows(I).Item("Taxable_Amount").ToString), "############0.00"), "")
                            .Rows(n).Cells(3).Value = IIf(Val(dt4.Rows(I).Item("CGST_Percentage").ToString) <> 0, Val(dt4.Rows(I).Item("CGST_Percentage").ToString), "")
                            .Rows(n).Cells(4).Value = IIf(Val(dt4.Rows(I).Item("CGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(I).Item("CGST_Amount").ToString), "##########0.00"), "")
                            .Rows(n).Cells(5).Value = IIf(Val(dt4.Rows(I).Item("SGST_Percentage").ToString) <> 0, Val(dt4.Rows(I).Item("SGST_Percentage").ToString), "")
                            .Rows(n).Cells(6).Value = IIf(Val(dt4.Rows(I).Item("SGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(I).Item("SGST_Amount").ToString), "###########0.00"), "")
                            .Rows(n).Cells(7).Value = IIf(Val(dt4.Rows(I).Item("IGST_Percentage").ToString) <> 0, Val(dt4.Rows(I).Item("IGST_Percentage").ToString), "")
                            .Rows(n).Cells(8).Value = IIf(Val(dt4.Rows(I).Item("IGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(I).Item("IGST_Amount").ToString), "###########0.00"), "")
                        Next I

                    End If

                End With

            End If


            If LockSTS = True Then
                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray
                cbo_Agent.Enabled = False
                cbo_Agent.BackColor = Color.LightGray

                cbo_Delvat.Enabled = False
                cbo_Delvat.BackColor = Color.LightGray


                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                cbo_VehicleNo.Enabled = False
                cbo_VehicleNo.BackColor = Color.LightGray


                cbo_Grid_CountName.Enabled = False
                cbo_Grid_CountName.BackColor = Color.LightGray

                cbo_Grid_MillName.Enabled = False
                cbo_Grid_MillName.BackColor = Color.LightGray
            End If


            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
            'If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()
        End Try

        NoCalc_Status = False

    End Sub

    Private Sub Yarn_Purchase_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Delvat.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Delvat.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Yarn_Purchase_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim dt9 As New DataTable

        Me.Text = ""

        con.Open()


        cbo_CommType.Items.Clear()
        cbo_CommType.Items.Add("BAG")
        cbo_CommType.Items.Add("%")

        'Common_Procedures.get_VehicleNo_From_All_Entries(con)

        cbo_Grid_RateFor.Items.Clear()
        cbo_Grid_RateFor.Items.Add("BAG")
        cbo_Grid_RateFor.Items.Add("KG")

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("GST")
        cbo_TaxType.Items.Add("NO TAX")
        Label8.Visible = False
        Cbo_DeliveryAddress.Visible = False

        cbo_Verified_Sts.Items.Clear()
        cbo_Verified_Sts.Items.Add("")
        cbo_Verified_Sts.Items.Add("YES")
        cbo_Verified_Sts.Items.Add("NO")


        cbo_Through.Items.Clear()
        cbo_Through.Items.Add(" ")
        cbo_Through.Items.Add("DIRECT")
        cbo_Through.Items.Add("BANK")
        cbo_Through.Items.Add("AGENT")
        'dgv_Details.Columns(9).Visible = False
        'If Val(Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status) = 1 Then

        '    dgv_Details.Columns(9).Visible = True

        'Else

        '    dgv_Details.Columns(1).Width = dgv_Details.Columns(1).Width + 30
        '    dgv_Details.Columns(2).Width = dgv_Details.Columns(2).Width + 60
        '    dgv_Details.Columns(8).Width = dgv_Details.Columns(8).Width + 10

        '    dgv_Details_Total.Columns(1).Width = dgv_Details_Total.Columns(1).Width + 30
        '    dgv_Details_Total.Columns(2).Width = dgv_Details_Total.Columns(2).Width + 60
        '    dgv_Details_Total.Columns(8).Width = dgv_Details_Total.Columns(8).Width + 10
        'End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1037" Then '------PRAKASH TEXTILES (SOMANUR)
            dgv_Details.Columns(dgvCol_Details.Ex_Mill).Visible = True
        End If
        dgv_Details.ColumnHeadersDefaultCellStyle.Font = New Font("Calibri", 8.25, FontStyle.Bold)
        dgv_Details.DefaultCellStyle.Font = New Font("Calibri", 9, FontStyle.Bold)
        chk_Verified_Status.Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1249" Or Trim(Common_Procedures.settings.CustomerCode) = "1116" Then

            If Val(Common_Procedures.User.IdNo) <> 1 And Common_Procedures.UR.Ledger_Verifition = "" Then chk_Verified_Status.Visible = True
        End If



        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If
        If Common_Procedures.settings.CustomerCode = "1186" Then
            lbl_PaymenTerms.Text = "Delivery Due Days"


            Label14.Text = "Transportation"
            cbo_transportation.Visible = True
            cbo_tax_type.Visible = True
            cbo_Transport.Visible = False
            cbo_VehicleNo.Visible = False
            cbo_TaxType.Visible = False
            Label85.Visible = False

        Else
            txt_Note.Size = New Size(771, 23)
        End If
        txt_caption_attend.Visible = True
        txt_Attend.Visible = True
        'If Trim(Common_Procedures.settings.CustomerCode) = "1152" Then '------JPR TEXTILES (PALLADAM)
        '    dgv_Details.Columns(20).Visible = True
        '    dgv_Details.Columns(1).Width = 90
        '    dgv_Details.Columns(2).Width = 130
        '    dgv_Details_Total.Columns(1).Width = 90
        '    dgv_Details_Total.Columns(2).Width = 130
        '    dgv_Details_Total.Columns(10).Visible = True
        'End If


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = (Me.Height - pnl_Tax.Height) \ 2
        pnl_Tax.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = ((Me.Height - pnl_Print.Height) \ 2) - 100
        pnl_Print.BringToFront()


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = ((Me.Height - pnl_Selection.Height) \ 2) - 100
        pnl_Selection.BringToFront()


        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then
                chk_Verified_Status.Visible = True
                lbl_verfied_sts.Visible = True
                cbo_Verified_Sts.Visible = True
            End If
        Else
            chk_Verified_Status.Visible = False
            lbl_verfied_sts.Visible = False
            cbo_Verified_Sts.Visible = False
        End If


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delvat.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Through.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_CommRate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_CommAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLessAfterTax_Text.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_AfterTax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_BeforeTax.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Attend.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_DeliveryAt.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AssessableValue.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_DeliveryAddress.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DueDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PaymenTerms.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_DeliveryDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_DeliveryDate.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_grdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_gr_date.GotFocus, AddressOf ControlGotFocus



        AddHandler cbo_tax_type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_transportation.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Sales_Order_No.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Sales_Order_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Through.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AssessableValue.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delvat.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_CommAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLessAfterTax_Text.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_AddLess_AfterTax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_BeforeTax.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Attend.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_DeliveryAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_DeliveryAddress.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DueDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PaymenTerms.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_DeliveryDate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_DeliveryDate.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_grdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_gr_date.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_tax_type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_transportation.LostFocus, AddressOf ControlLostFocus


        'AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Sales_Order_No.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLessAfterTax_Text.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_AfterTax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DueDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PaymenTerms.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Sales_Order_No.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLessAfterTax_Text.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_AfterTax.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DueDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PaymenTerms.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler chk_CloseStatus.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_CloseStatus.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Yarn_Sales_Order_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Yarn_Sales_Order_GST_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Tax.Visible = True Then
                    btn_Tax_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then


                    If .CurrentCell.ColumnIndex >= .ColumnCount - 12 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            txt_DiscPerc.Focus()

                        Else
                            If Trim(Common_Procedures.settings.CustomerCode) = "1037" And .CurrentCell.ColumnIndex = dgvCol_Details.Ex_Mill Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Cancel_Order)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details.Count)
                            End If
                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(dgvCol_Details.Count).Value) = "" Then
                            txt_DiscPerc.Focus()

                        Else

                            If Trim(Common_Procedures.settings.CustomerCode) = "1037" Then '---PRAKASH TEXTILES
                                If .CurrentCell.ColumnIndex = dgvCol_Details.rate And .Columns(dgvCol_Details.Ex_Mill).Visible = True Then
                                    .CurrentCell = .CurrentRow.Cells(dgvCol_Details.Ex_Mill)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                End If
                            ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.rate Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Cancel_Order)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            End If

                        End If

                    End If

                    Return True


                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            lbl_CommAmount.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_Details.rate)

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
    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)
        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Yarn_Purchase_Order_Entry, New_Entry, Me, con, "Yarn_Sales_Order_Head", "Yarn_Sales_Order_Code", NewCode, "Yarn_Sales_Order_Date", "(Yarn_Sales_Order_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Yarn_Sales_Order_Head", "Verified_Status", "(Yarn_Sales_Order_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Purchase_Weight) from Yarn_Sales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already  Purchase for this Order", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()



        Da = New SqlClient.SqlDataAdapter("select sum(Invoice_Weight) from Yarn_Sales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already  Invoiced for this Order", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans




            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Yarn_Sales_Order_Head", "Yarn_Sales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Yarn_Sales_Order_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Yarn_Sales_Order_Details", "Yarn_Sales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount, Colour_Idno,Cash_Discount_Percentage,Cash_Discount_Amount,Taxable_Value,GST_Percentage,HSN_Code,Ex_Mill,Cancelled_Weight", "Sl_No", "Yarn_Sales_Order_Code, For_OrderBy, Company_IdNo, Yarn_Sales_Order_No, Yarn_Sales_Order_Date, Ledger_Idno", trans)


            cmd.CommandText = "delete from Yarn_Sales_Order_GST_Tax_Details  where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Yarn_Sales_Order_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Yarn_Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()
            'If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()
        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_DeliveryAt.Text = ""
            cbo_Filter_Count.Text = ""

            cbo_Filter_DeliveryAt.SelectedIndex = -1
            cbo_Filter_Count.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Sales_Order_No from Yarn_Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Yarn_Sales_Order_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Sales_Order_No from Yarn_Sales_Order_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Yarn_Sales_Order_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Sales_Order_No from Yarn_Sales_Order_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Yarn_Sales_Order_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Sales_Order_No from Yarn_Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Yarn_Sales_Order_No desc", con)
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
        Dim Dt2 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Sales_Order_Head", "Yarn_Sales_Order_Code", "For_OrderBy", "Entry_VAT_GST_Type = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red


            msk_Date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*,  c.ledger_name as TaxAcName from Yarn_Sales_Order_Head a  LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Yarn_Sales_Order_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Yarn_Sales_Order_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("Yarn_Sales_Order_Date").ToString
                End If

                If Dt1.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = Dt1.Rows(0).Item("Tax_Type").ToString

                If Dt1.Rows(0).Item("TaxType").ToString <> "" Then cbo_tax_type.Text = Dt1.Rows(0).Item("TaxType").ToString

                If Dt1.Rows(0).Item("transportations").ToString <> "" Then cbo_transportation.Text = Dt1.Rows(0).Item("transportations").ToString


                If Dt1.Rows(0).Item("Agent_Commission_Type").ToString <> "" Then cbo_CommType.Text = Dt1.Rows(0).Item("Agent_Commission_Type").ToString
                If Dt1.Rows(0).Item("AddLessAfterTax_Text").ToString <> "" Then txt_AddLessAfterTax_Text.Text = Dt1.Rows(0).Item("AddLessAfterTax_Text").ToString
                If Trim(txt_AddLessAfterTax_Text.Text) = "" Then txt_AddLessAfterTax_Text.Text = "Add/Less"


                Da1 = New SqlClient.SqlDataAdapter("Select a.* from Yarn_Sales_Order_Details a Where a.Yarn_Sales_Order_Code = '" & Trim(Dt1.Rows(0).Item("Yarn_Sales_Order_Code").ToString) & "' Order by a.sl_no", con)
                Dt2 = New DataTable
                Da1.Fill(Dt2)

                If Dt2.Rows.Count > 0 Then

                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Rows(0).Cells(dgvCol_Details.Rate_for).Value = Dt2.Rows(0).Item("Rate_For").ToString
                    End If

                End If

                Dt2.Clear()

            End If

            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RefCode = Pk_Condition & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_Order_No from Yarn_Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Yarn_Purchase_Order_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            ' InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_Order_No from Yarn_Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim PurAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim clr_ID As Integer = 0
        Dim Del_ID As Integer
        Dim Agt_Idno As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Ord_No As String = ""
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim vTotCns As Single, vTotBgs As Single, vTotWght As Single, vTotCancel_Wght As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim uSR_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""
        Dim Comm_Amt As Double = 0
        Dim ag_Comm As Double = 0
        Dim agtds_perc As Double = 0
        Dim RecNo As String = ""
        Dim RecCd As String = ""
        Dim RecSlNo As Long = 0
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim vCREATED_DTTM_TXT As String = ""
        Dim vMODIFIED_DTTM_TXT As String = ""
        Dim vCLOSE_STS = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Val(Common_Procedures.User.IdNo) = 0 Then
            MessageBox.Show("Invalid User Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Yarn_Purchase_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Yarn_Purchase_Order_Entry, New_Entry, Me, con, "Yarn_Sales_Order_Head", "Yarn_Sales_Order_Code", NewCode, "Yarn_Sales_Order_Date", "(Yarn_Sales_Order_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Yarn_Sales_Order_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Yarn_Sales_Order_Head", "Verified_Status", "(Yarn_Sales_Order_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Purchase Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Purchase Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If
        If Trim(lbl_OrderCode.Text) <> "" Then


            If Led_ID <> 0 Then

                Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Head a INNER JOIN Own_order_Yarn_Details b ON a.Own_Order_Code =b.Own_Order_Code where a.Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' and  b.Ledger_idno = " & Str(Val(Led_ID)), con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    Ord_No = Dt1.Rows(0).Item("Order_No").ToString

                End If
            End If
            If Trim(Ord_No) <> Trim(lbl_OrderNo.Text) Then
                MessageBox.Show("Invalid Mismatch Of Order No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
                Exit Sub
            End If
        End If
        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        Del_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Delvat.Text)
        If Del_ID = 0 Then Del_ID = 4


        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        If Trim(lbl_NetAmount.Text) = "" Then lbl_NetAmount.Text = 0


        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCol_Details.Bag).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.cones).Value) <> 0 Then

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Count).Value)
                    If Cnt_ID = 0 Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.Count)
                        End If
                        Exit Sub
                    End If

                    Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Mill_Name).Value)
                    If Mill_ID = 0 Then
                        MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.Mill_Name)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(dgvCol_Details.weight).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.weight)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With


        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        If Trim(txt_AddLessAfterTax_Text.Text) = "" Then txt_AddLessAfterTax_Text.Text = "Add/Less"

        NoCalc_Status = False
        Total_Calculation()

        vTotCns = 0 : vTotBgs = 0 : vTotWght = 0 : vTotCancel_Wght = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBgs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotCns = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotWght = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotCancel_Wght = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
        End If



        vCLOSE_STS = 0
        If chk_CloseStatus.Checked = True Then vCLOSE_STS = 1

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Sales_Order_Head", "Yarn_Sales_Order_Code", "For_OrderBy", "Entry_VAT_GST_Type = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()



            cmd.Parameters.AddWithValue("@PurchaseDate", Convert.ToDateTime(msk_Date.Text))
            cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(msk_DeliveryDate.Text))
            cmd.Parameters.AddWithValue("@Grdate", Convert.ToDateTime(msk_grdate.Text))

            vCREATED_DTTM_TXT = ""
            vMODIFIED_DTTM_TXT = ""

            vCREATED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@createddatetime", Now)

            vMODIFIED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@modifieddatetime", Now)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Yarn_Sales_Order_Head (  Entry_VAT_GST_Type , Yarn_Sales_Order_Code ,               Company_IdNo       ,     Yarn_Sales_Order_No     ,                               for_OrderBy                               , Yarn_Sales_Order_Date ,        Ledger_IdNo      ,             Agent_IdNo    ,        DeliveryTo_Idno  ,        Agent_Commission_Rate   ,         Agent_Commission_Type    ,   Agent_Commission_Commission   ,       Total_Bags     ,           Total_Cones     ,          Total_Weight      , Total_Cancelled_Weight            ,               Total_Amount             ,             Discount_Percentage     ,              Discount_Amount          ,                AddLess_BeforeTax_Amount      ,                 Assessable_Value           ,           TaxAc_IdNo      ,               Tax_Type           ,                  Freight_Amount     ,               AddLessAfterTax_Text            ,                   AddLess_Amount            ,               RoundOff_Amount      ,                  Net_Amount               ,         Transport_IdNo    ,              Note            ,              User_idNo          ,               Vehicle_No           ,          Total_CGST_Amount        ,              Total_SGST_Amount    ,          Total_IGST_Amount        ,               Delivery_Address           ,               Order_No           ,               Own_Order_Code       ,            Due_Days          , Delivery_Date ,            Payment_Terms          ,  Verified_Status  ,Attend,Gr_Date,Taxtype,Transportations,Through_Name,Party_OrderNo                                                                                             ,  Order_Close_Status    ,                     created_useridno           ,   created_DateTime,          created_DateTime_Text    , Last_modified_useridno, Last_modified_DateTime, Last_modified_DateTime_Text  ) " &
                "Values                                              (           'GST'     ,  '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(vOrdByNo)) & " ,        @PurchaseDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Agt_Idno)) & ", " & Str(Val(Del_ID)) & ", " & Val(txt_CommRate.Text) & " , '" & Trim(cbo_CommType.Text) & "', " & Val(lbl_CommAmount.Text) & ", " & Val(vTotBgs) & " , " & Str(Val(vTotCns)) & " , " & Str(Val(vTotWght)) & " , " & Str(Val(vTotCancel_Wght)) & " , " & Str(Val(lbl_GrossAmount.Text)) & " , " & Str(Val(txt_DiscPerc.Text)) & " , " & Str(Val(lbl_DiscAmount.Text)) & " , " & Str(Val(txt_AddLess_BeforeTax.Text)) & " , " & Str(Val(txt_AssessableValue.Text)) & " , " & Str(Val(TxAc_ID)) & " , '" & Trim(cbo_TaxType.Text) & "' ,  " & Str(Val(txt_Freight.Text)) & " , '" & Trim(txt_AddLessAfterTax_Text.Text) & "' , " & Str(Val(txt_AddLess_AfterTax.Text)) & " , " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Str(Val(Trans_ID)) & ",'" & Trim(txt_Note.Text) & "' ,  " & Val(lbl_UserName.Text) & " , '" & Trim(cbo_VehicleNo.Text) & "' , " & Val(lbl_CGST_Amount.Text) & " , " & Val(lbl_SGST_Amount.Text) & " , " & Val(lbl_IGST_Amount.Text) & " , '" & Trim(Cbo_DeliveryAddress.Text) & "' , '" & Trim(lbl_OrderNo.Text) & "' , '" & Trim(lbl_OrderCode.Text) & "' ," & Val(txt_DueDays.Text) & " , @DeliveryDate ,'" & Trim(txt_PaymenTerms.Text) & "' ," & Val(Verified_STS) & ",'" & Trim(txt_Attend.Text) & "',@Grdate,'" & Trim(cbo_tax_type.Text) & "','" & Trim(cbo_transportation.Text) & "','" & Trim(cbo_Through.Text) & "','" & Trim(txt_Sales_Order_No.Text) & "', " & Val(vCLOSE_STS) & ",      " & Str(Val(Common_Procedures.User.IdNo)) & ",  @createddatetime ,  '" & Trim(vCREATED_DTTM_TXT) & "',              0        ,     NUll              ,          ''   ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Yarn_Sales_Order_Head", "Yarn_Sales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Sales_Order_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Yarn_Sales_Order_Details", "Yarn_Sales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount, Colour_Idno,Cash_Discount_Percentage,Cash_Discount_Amount,Taxable_Value,GST_Percentage,HSN_Code,Ex_Mill,Cancelled_Weight", "Sl_No", "Yarn_Sales_Order_Code, For_OrderBy, Company_IdNo, Yarn_Sales_Order_No, Yarn_Sales_Order_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Yarn_Sales_Order_Head set Entry_VAT_GST_Type = 'GST' , Yarn_Sales_Order_Date = @PurchaseDate, Delivery_Date = @DeliveryDate , Ledger_IdNo = " & Str(Val(Led_ID)) & ", Agent_IdNo = " & Str(Val(Agt_Idno)) & ", DeliveryTo_Idno = " & Str(Val(Del_ID)) & ", Agent_Commission_Rate = " & Val(txt_CommRate.Text) & ",Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' ,  Agent_Commission_Type = '" & Trim(cbo_CommType.Text) & "', Agent_Commission_Commission =" & Val(lbl_CommAmount.Text) & ", Total_Bags = " & Val(vTotBgs) & ",Total_Cones  = " & Str(Val(vTotCns)) & ", Total_Weight = " & Str(Val(vTotWght)) & " , Total_Cancelled_Weight = " & Str(Val(vTotCancel_Wght)) & " , Total_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ", AddLess_BeforeTax_Amount = " & Str(Val(txt_AddLess_BeforeTax.Text)) & ", Assessable_Value = " & Str(Val(txt_AssessableValue.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "',Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLessAfterTax_Text = '" & Trim(txt_AddLessAfterTax_Text.Text) & "', AddLess_Amount = " & Str(Val(txt_AddLess_AfterTax.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Transport_IdNo  = " & Str(Val(Trans_ID)) & ", Note = '" & Trim(txt_Note.Text) & "' ,   User_IdNo = " & Val(lbl_UserName.Text) & " ,Total_CGST_Amount = " & Val(lbl_CGST_Amount.Text) & " ,Total_SGST_Amount = " & Val(lbl_SGST_Amount.Text) & ",Total_IGST_Amount =" & Val(lbl_IGST_Amount.Text) & ",Delivery_Address = '" & Trim(Cbo_DeliveryAddress.Text) & "',Order_No = '" & Trim(lbl_OrderNo.Text) & "',Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' , Due_Days = " & Val(txt_DueDays.Text) & " , Payment_Terms = '" & Trim(txt_PaymenTerms.Text) & "',Verified_Status= " & Val(Verified_STS) & " ,Attend='" & Trim(txt_Attend.Text) & "' ,Gr_Date=@Grdate,TaxType='" & Trim(cbo_tax_type.Text) & "',transportations='" & Trim(cbo_transportation.Text) & "',Through_Name='" & Trim(cbo_Through.Text) & "' ,Party_OrderNo='" & Trim(txt_Sales_Order_No.Text) & "' , Order_Close_Status = " & Val(vCLOSE_STS) & " , Last_modified_useridno = " & Str(Val(Common_Procedures.User.IdNo)) & ", Last_modified_DateTime = @modifieddatetime, Last_modified_DateTime_Text = '" & Trim(vMODIFIED_DTTM_TXT) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Yarn_Sales_Order_Head", "Yarn_Sales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Sales_Order_Code, Company_IdNo, for_OrderBy", tr)


            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "PurcOrd : Ref No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Yarn_Sales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "' and Purchase_Weight = 0 "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Yarn_Sales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "' and Invoice_Weight = 0 and  Invoice_bags = 0 "
            cmd.ExecuteNonQuery()


            With dgv_Details

                Sno = 0
                YrnClthNm = ""
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCol_Details.weight).Value) <> 0 Then

                        Sno = Sno + 1


                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Count).Value, tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Mill_Name).Value, tr)

                        clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Colour).Value, tr)

                        Nr = 0
                        cmd.CommandText = "Update  Yarn_Sales_Order_Details set Yarn_Sales_Order_Date = @PurchaseDate ,Ledger_IdNo = " & Val(Led_ID) & ", Sl_No  = " & Str(Val(Sno)) & " , count_idno = " & Str(Val(Cnt_ID)) & " ,  Mill_IdNo =  " & Str(Val(Mill_ID)) & ", Bags = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Bag).Value)) & " , Cones = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Count).Value)) & " ,       Weight = " & Str(Val(.Rows(i).Cells(dgvCol_Details.weight).Value)) & ",Rate_For= '" & Trim(.Rows(i).Cells(dgvCol_Details.Rate_for).Value) & "',Rate =  " & Str(Val(.Rows(i).Cells(dgvCol_Details.rate).Value)) & ",Amount =  " & Str(Val(.Rows(i).Cells(dgvCol_Details.amount).Value)) & " ,Colour_Idno = " & Str(Val(clr_ID)) & " , Cancelled_Weight = " & Val(.Rows(i).Cells(dgvCol_Details.Cancel_Order).Value) & "             where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'  and Yarn_Sales_Order_Details_SlNo = " & Val(.Rows(i).Cells(dgvCol_Details.Detail_slno).Value)
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then


                            cmd.CommandText = "Insert into Yarn_Sales_Order_Details ( Yarn_Sales_Order_Code  ,               Company_IdNo       ,   Yarn_Sales_Order_No      ,                     for_OrderBy                                         , Yarn_Sales_Order_Date,   Ledger_IdNo       ,            Sl_No     ,        Count_IdNo       ,          Mill_IdNo       ,                       Bags                                 ,                     Cones                                     ,                           Weight                          ,                   Rate_For                                        ,                     Rate                                 ,                  Amount                   , Colour_Idno              ,    Cash_Discount_Percentage                                 ,    Cash_Discount_Amount                                    ,                 Taxable_Value                                        ,  GST_Percentage                                                   ,                       HSN_Code                           ,     Ex_Mill                                                    ,     Cancelled_Weight                                                              ,Cones_per_bag                         ,                                                 Wgt_per_bag  ) " &
                            "Values                                                    (   '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "',             " & Str(Val(vOrdByNo)) & "                                  , @PurchaseDate           , " & Val(Led_ID) & " , " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mill_ID)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Bag).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.cones).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.weight).Value)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.Rate_for).Value) & "', " & Str(Val(.Rows(i).Cells(dgvCol_Details.rate).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.amount).Value)) & " , " & Str(Val(clr_ID)) & " ," & Str(Val(.Rows(i).Cells(dgvCol_Details.Disc_perc).Value)) & "," & Str(Val(.Rows(i).Cells(dgvCol_Details.Disc_AMt).Value)) & "," & Str(Val(.Rows(i).Cells(dgvCol_Details.Taxable_value).Value)) & "," & Str(Val(.Rows(i).Cells(dgvCol_Details.Gst_Perc).Value)) & ",'" & Trim(.Rows(i).Cells(dgvCol_Details.Hsn_Code).Value) & "' ,'" & Trim(.Rows(i).Cells(dgvCol_Details.Ex_Mill).Value) & "' ," & Val(.Rows(i).Cells(dgvCol_Details.Cancel_Order).Value) & "," & Val(.Rows(i).Cells(dgvCol_Details.cone_per_bag).Value) & "," & Val(.Rows(i).Cells(dgvCol_Details.Wt_per_bag).Value) & " ) "
                            cmd.ExecuteNonQuery()

                        End If



                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Yarn_Sales_Order_Details", "Yarn_Sales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount, Colour_Idno,Cash_Discount_Percentage,Cash_Discount_Amount,Taxable_Value,GST_Percentage,HSN_Code,Ex_Mill,Cancelled_Weight", "Sl_No", "Yarn_Sales_Order_Code, For_OrderBy, Company_IdNo, Yarn_Sales_Order_No, Yarn_Sales_Order_Date, Ledger_Idno", tr)

            End With
            cmd.CommandText = "Delete from Yarn_Sales_Order_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Tax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Yarn_Sales_Order_GST_Tax_Details   ( Yarn_Sales_Order_Code  ,               Company_IdNo       ,      Yarn_Sales_Order_No                ,                               for_OrderBy                                  , Yarn_Sales_Order_Date    ,         Ledger_IdNo     ,            Sl_No     , HSN_Code                               ,Taxable_Amount                            ,CGST_Percentage                           ,CGST_Amount                               ,SGST_Percentage                            ,SGST_Amount                              ,IGST_Percentage                          ,IGST_Amount ) " &
                        "Values                                                              (   '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",       @PurchaseDate    , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "  ," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With



            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(dgvCol_Details.Count).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(dgvCol_Details.Mill_Name).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_Details

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
                        Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
                        Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                    If .CurrentCell.ColumnIndex = 3 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(dgvCol_Details.cone_per_bag).Value = Cn_bag
                            .Rows(.CurrentRow.Index).Cells(dgvCol_Details.cones).Value = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Bag).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Wt_per_bag).Value = Wgt_Bag
                            .Rows(.CurrentRow.Index).Cells(dgvCol_Details.weight).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(dgvCol_Details.Bag).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(dgvCol_Details.weight).Value = Format(.Rows(.CurrentRow.Index).Cells(dgvCol_Details.cones).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub get_YarnCount_Calculation()
        Dim Cne_Wgt As Single = 0
        Dim Cnt As Single = 0

        Cnt = 64.814 / (Cne_Wgt)

    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub


    'Private Sub Weight_Calculation()
    '    Dim n As Integer
    '    Dim dt As New DataTable
    '    With dgv_Details
    '        If dt.Rows.Count > 0 Then
    '            .Rows.Clear()
    '            For i = 0 To dt.Rows.Count - 1

    '                n = .Rows.Add()
    '                If Val(dgv_Details.Rows(0).Cells(3).Value) <> 0 Then
    '                    .Rows(n).Cells(6).Value = Val(dgv_Details.Rows(0).Cells(3).Value) * Val(dgv_Details.Rows(0).Cells(6).Value)
    '                End If
    '                If Val(dgv_Details.Rows(0).Cells(4).Value) <> 0 Then
    '                    .Rows(n).Cells(6).Value = Val(dgv_Details.Rows(0).Cells(4).Value) * Val(dgv_Details.Rows(0).Cells(6).Value)
    '                End If

    '            Next i
    '        End If
    '    End With
    '    'If Val(txt_Cones_Bag.Text) <> 0 Then
    '    '    txt_Cones.Text = Val(txt_Bags.Text) * Val(txt_Cones_Bag.Text)
    '    'End If
    '    'If Val(txt_Weight_Cone.Text) <> 0 Then
    '    '    txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(txt_Weight_Cone.Text), "#########0.000")
    '    'End If
    'End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_AddLess_AfterTax, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub


    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        cbo_PartyName.Tag = cbo_PartyName.Text
    End Sub

    Private Sub cbo_Party_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_Date, txt_Sales_Order_No, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Party_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then
                If MessageBox.Show("Do you want to select Internal Order:", "FOR INTERNAL ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    txt_Sales_Order_No.Focus()

                End If

            Else
                txt_Sales_Order_No.Focus()
            End If

            If Trim(UCase(cbo_PartyName.Tag)) <> Trim(UCase(cbo_PartyName.Text)) Then
                cbo_PartyName.Tag = cbo_PartyName.Text
                GST_Calculation()


            End If
        End If
    End Sub

    Private Sub cbo_Delvat_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Delvat.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- SUBHAM Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If
    End Sub

    Private Sub cbo_Delvat_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delvat.KeyDown
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- SUBHAM Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delvat, txt_Sales_Order_No, txt_Attend, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delvat, txt_Sales_Order_No, txt_Attend, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Delvat_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Delvat.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- SUBHAM Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delvat, txt_Attend, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delvat, txt_Attend, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
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
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Sales_Order_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Yarn_Sales_Order_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Sales_Order_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_DeliveryAt.Text) <> "" Then
                Del_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_DeliveryAt.Text)
            End If

            If Trim(cbo_Filter_Count.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_Count.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If
            If Val(Del_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(Del_IdNo)) & " "
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Count_Idno = " & Str(Val(Cnt_IdNo)) & " "
            End If

            If cbo_Verified_Sts.Visible = True And Trim(cbo_Verified_Sts.Text) <> "" Then

                If Trim(cbo_Verified_Sts.Text) = "YES" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 1 "
                ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 0 "
                End If


            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as PartyName, d.Ledger_Name as Delv_Name from Yarn_Sales_Order_Head a INNER JOIN Yarn_Sales_Order_Details b ON a.Yarn_Sales_Order_Code = b.Yarn_Sales_Order_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.DeliveryTo_Idno = d.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Order_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Sales_Order_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from Yarn_Sales_Order_Head a INNER JOIN Yarn_Sales_Order_Details b ON a.Yarn_Sales_Order_Code = b.Yarn_Sales_Order_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Order_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Sales_Order_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Yarn_Sales_Order_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Yarn_Sales_Order_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Delv_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Common_Procedures.Count_IdNoToName(con, Val(dt2.Rows(i).Item("Count_IdNo").ToString))
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_DeliveryAt, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_DeliveryAt, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        dgv_Details_CellLeave(sender, e)
                        If dgv_Details.CurrentCell.ColumnIndex = dgvCol_Details.Bag Or dgv_Details.CurrentCell.ColumnIndex = dgvCol_Details.cones Then
                            get_MillCount_Details()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL END EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle


        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then

                        If Val(.CurrentRow.Cells(dgvCol_Details.SlNo).Value) = 0 Then
                            .CurrentRow.Cells(dgvCol_Details.SlNo).Value = .CurrentRow.Index + 1
                        End If

                        If e.ColumnIndex = dgvCol_Details.Count And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.purchase_weight).Value) = 0 Then

                            If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                                cbo_Grid_CountName.Tag = -1
                                Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                                Dt1 = New DataTable
                                Da.Fill(Dt1)
                                cbo_Grid_CountName.DataSource = Dt1
                                cbo_Grid_CountName.DisplayMember = "Count_Name"

                                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                                cbo_Grid_CountName.Left = .Left + rect.Left
                                cbo_Grid_CountName.Top = .Top + rect.Top

                                cbo_Grid_CountName.Width = rect.Width
                                cbo_Grid_CountName.Height = rect.Height
                                cbo_Grid_CountName.Text = .CurrentCell.Value

                                cbo_Grid_CountName.Tag = Val(e.RowIndex)
                                cbo_Grid_CountName.Visible = True

                                cbo_Grid_CountName.BringToFront()
                                cbo_Grid_CountName.Focus()

                            End If

                        Else
                            cbo_Grid_CountName.Visible = False

                        End If

                        If e.ColumnIndex = dgvCol_Details.Mill_Name And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.purchase_weight).Value) = 0 Then

                            If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                                cbo_Grid_MillName.Tag = -1
                                Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                                Dt1 = New DataTable
                                Da.Fill(Dt1)
                                cbo_Grid_MillName.DataSource = Dt1
                                cbo_Grid_MillName.DisplayMember = "Mill_Name"

                                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                                cbo_Grid_MillName.Left = .Left + rect.Left
                                cbo_Grid_MillName.Top = .Top + rect.Top

                                cbo_Grid_MillName.Width = rect.Width
                                cbo_Grid_MillName.Height = rect.Height
                                cbo_Grid_MillName.Text = .CurrentCell.Value

                                cbo_Grid_MillName.Tag = Val(e.RowIndex)
                                cbo_Grid_MillName.Visible = True

                                cbo_Grid_MillName.BringToFront()
                                cbo_Grid_MillName.Focus()

                            End If

                        Else
                            cbo_Grid_MillName.Visible = False

                        End If

                        If e.ColumnIndex = dgvCol_Details.Rate_for And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.purchase_weight).Value) = 0 Then

                            If cbo_Grid_RateFor.Visible = False Or Val(cbo_Grid_RateFor.Tag) <> e.RowIndex Then

                                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                                cbo_Grid_RateFor.Left = .Left + rect.Left
                                cbo_Grid_RateFor.Top = .Top + rect.Top

                                cbo_Grid_RateFor.Width = rect.Width
                                cbo_Grid_RateFor.Height = rect.Height
                                cbo_Grid_RateFor.Text = .CurrentCell.Value

                                cbo_Grid_RateFor.Tag = Val(e.RowIndex)
                                cbo_Grid_RateFor.Visible = True

                                cbo_Grid_RateFor.BringToFront()
                                cbo_Grid_RateFor.Focus()

                            End If

                        Else
                            cbo_Grid_RateFor.Visible = False

                        End If

                        If dgv_Details.Columns(dgvCol_Details.Colour).Visible = True Then
                            If e.ColumnIndex = dgvCol_Details.Colour And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.purchase_weight).Value) = 0 Then
                                If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                                    cbo_Colour.Tag = -1
                                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                                    Dt2 = New DataTable
                                    Da.Fill(Dt2)
                                    cbo_Colour.DataSource = Dt2
                                    cbo_Colour.DisplayMember = "Colour_Name"

                                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                                    cbo_Colour.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                                    cbo_Colour.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                                    cbo_Colour.Width = rect.Width  ' .CurrentCell.Size.Width
                                    cbo_Colour.Height = rect.Height  ' rect.Height

                                    cbo_Colour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                                    cbo_Colour.Tag = Val(e.RowIndex)
                                    cbo_Colour.Visible = True

                                    cbo_Colour.BringToFront()
                                    cbo_Colour.Focus()



                                End If


                            Else

                                'cbo_Grid_MillName.Tag = -1
                                'cbo_Grid_MillName.Text = ""
                                cbo_Colour.Visible = False


                            End If
                        End If


                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = dgvCol_Details.weight Then
                            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                            Else
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                            End If
                        End If

                        If .CurrentCell.ColumnIndex = dgvCol_Details.rate Or .CurrentCell.ColumnIndex = dgvCol_Details.amount Then
                            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                            Else
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                            End If
                        End If
                    End If
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try
            If Not IsNothing(dgv_Details.CurrentCell) Then
                With dgv_Details
                    If .Visible Then
                        If .Rows.Count > 0 Then
                            If .CurrentCell.ColumnIndex = dgvCol_Details.Bag Or .CurrentCell.ColumnIndex = dgvCol_Details.weight Or .CurrentCell.ColumnIndex = dgvCol_Details.Rate_for Or .CurrentCell.ColumnIndex = dgvCol_Details.rate Or .CurrentCell.ColumnIndex = dgvCol_Details.Cancel_Order Or .CurrentCell.ColumnIndex = dgvCol_Details.cone_per_bag Or .CurrentCell.ColumnIndex = dgvCol_Details.Wt_per_bag Then



                                Amount_Calculation(e.RowIndex, e.ColumnIndex)



                            End If

                        End If

                    End If

                End With
            End If



        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        Try
            With dgv_Details
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.purchase_weight).Value) <> 0 Then
                            If .CurrentCell.ColumnIndex <> dgvCol_Details.Bag And .CurrentCell.ColumnIndex <> dgvCol_Details.cones And .CurrentCell.ColumnIndex <> dgvCol_Details.weight And .CurrentCell.ColumnIndex <> dgvCol_Details.Cancel_Order Then
                                e.Handled = True
                                e.SuppressKeyPress = True
                            End If

                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = dgvCol_Details.Bag Or .CurrentCell.ColumnIndex = dgvCol_Details.cones Or .CurrentCell.ColumnIndex = dgvCol_Details.weight Or .CurrentCell.ColumnIndex = dgvCol_Details.rate Or .CurrentCell.ColumnIndex = dgvCol_Details.amount Or .CurrentCell.ColumnIndex = dgvCol_Details.cone_per_bag Or .CurrentCell.ColumnIndex = dgvCol_Details.Wt_per_bag Or .CurrentCell.ColumnIndex = dgvCol_Details.Cancel_Order Then

                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If

                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        Try
            With dgv_Details

                If e.KeyCode = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            lbl_CommAmount.Focus()
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                        End If
                    End If
                End If

                If e.KeyCode = Keys.Right Then
                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
                            txt_DiscPerc.Focus()
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details.Count)
                        End If
                    End If
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details
                    If .Rows.Count > 0 Then
                        If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.purchase_weight).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.invoice_wgt).Value) = 0 Then
                            n = .CurrentRow.Index

                            If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                                For i = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(i).Value = ""
                                Next

                            Else
                                .Rows.RemoveAt(n)

                            End If

                            Total_Calculation()
                        End If

                    End If

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_Details
                n = .RowCount

                If .Rows.Count > 0 Then
                    .Rows(n - 1).Cells(dgvCol_Details.SlNo).Value = Val(n)
                    '.Rows(e.RowIndex).Cells(dgvCol_Details.SlNo).Value = Val(e.RowIndex) + 1
                    If e.RowIndex > 0 Then
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Rate_for).Value = Trim(UCase(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Rate_for).Value))
                    Else
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Rate_for).Value = "KG"
                    End If
                    'n = .RowCount
                    '.Rows(n - 1).Cells(0).Value = Val(n)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS ROWS ADD....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub txt_AddLess_BeforeTax_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess_BeforeTax.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                txt_Freight.Focus()
            Else
                txt_AddLess_AfterTax.Focus()
            End If
        End If

    End Sub

    Private Sub txt_AddLess_BeforeTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_BeforeTax.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                txt_Freight.Focus()
            Else
                txt_AddLess_AfterTax.Focus()
            End If

        End If

    End Sub

    Private Sub txt_AddLess_BeforeTax_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess_BeforeTax.LostFocus
        If Val(txt_AddLess_BeforeTax.Text) <> 0 Then
            txt_AddLess_BeforeTax.Text = Format(Val(txt_AddLess_BeforeTax.Text), "#########0.00")
        Else
            txt_AddLess_BeforeTax.Text = ""
        End If
    End Sub

    Private Sub txt_AddLess_BeforeTax_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess_BeforeTax.TextChanged
        NetAmount_Calculation()
    End Sub



    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_AfterTax.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    cbo_Transport.Focus()
        'End If
    End Sub

    Private Sub txt_AddLess_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess_AfterTax.LostFocus
        If Val(txt_AddLess_AfterTax.Text) <> 0 Then
            txt_AddLess_AfterTax.Text = Format(Val(txt_AddLess_AfterTax.Text), "#########0.00")
        Else
            txt_AddLess_AfterTax.Text = ""
        End If
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess_AfterTax.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.LostFocus
        If Val(txt_Freight.Text) <> 0 Then
            txt_Freight.Text = Format(Val(txt_Freight.Text), "#########0.00")
        Else
            txt_Freight.Text = ""
        End If

    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_VatPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_DiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPerc.KeyDown
        If e.KeyValue = 38 Then

            If dgv_Details.Rows.Count > 0 Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Count)
            Else
                txt_CommRate.Focus()
            End If

        End If


        If e.KeyValue = 40 Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1186" Then
                cbo_transportation.Focus()

            Else
                txt_AddLess_BeforeTax.Focus()
            End If
        End If

    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1186" Then
                cbo_transportation.Focus()

            Else
                txt_AddLess_BeforeTax.Focus()
            End If

        End If
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then

            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
                'dtp_Date.Focus()
            End If


        End If
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
                'dtp_Date.Focus()
            End If
        End If


    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If CurCol = dgvCol_Details.Bag Or CurCol = dgvCol_Details.weight Or CurCol = dgvCol_Details.Rate_for Or CurCol = dgvCol_Details.rate Or CurCol = dgvCol_Details.Cancel_Order Or CurCol = dgvCol_Details.cone_per_bag Or CurCol = dgvCol_Details.Wt_per_bag Then

                            If Trim(UCase(.Rows(CurRow).Cells(dgvCol_Details.Rate_for).Value)) = "BAG" Then
                                .Rows(CurRow).Cells(dgvCol_Details.amount).Value = Format(Val(.Rows(CurRow).Cells(dgvCol_Details.Bag).Value) * Val(.Rows(CurRow).Cells(dgvCol_Details.rate).Value), "#########0.00")
                            Else
                                .Rows(CurRow).Cells(dgvCol_Details.amount).Value = Format(Val(.Rows(CurRow).Cells(dgvCol_Details.weight).Value) * Val(.Rows(CurRow).Cells(dgvCol_Details.rate).Value), "#########0.00")
                            End If


                            If Val(.Rows(CurRow).Cells(dgvCol_Details.Wt_per_bag).Value) <> 0 Or Val(.Rows(CurRow).Cells(dgvCol_Details.cone_per_bag).Value) <> 0 Then

                                .Rows(CurRow).Cells(dgvCol_Details.weight).Value = Format(Val(.Rows(CurRow).Cells(dgvCol_Details.Wt_per_bag).Value) * Val(.Rows(CurRow).Cells(dgvCol_Details.Bag).Value), "#########0.00")

                                .Rows(CurRow).Cells(dgvCol_Details.cones).Value = Val(.Rows(CurRow).Cells(dgvCol_Details.cone_per_bag).Value) * Val(.Rows(CurRow).Cells(dgvCol_Details.Bag).Value)
                            End If

                            Total_Calculation()

                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "AMOUNT CALCULATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBgs As Single
        Dim TotCns As Single
        Dim TotWgt As Single
        Dim TotAmt As Single
        Dim Tot_Cnsl_Odr As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBgs = 0 : TotCns = 0 : TotWgt = 0 : TotAmt = 0 : Tot_Cnsl_Odr = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(dgvCol_Details.SlNo).Value = Sno
                If Trim(.Rows(i).Cells(dgvCol_Details.Count).Value) <> "" And (Val(.Rows(i).Cells(dgvCol_Details.Bag).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.weight).Value) <> 0) Then

                    TotBgs = TotBgs + Val(.Rows(i).Cells(dgvCol_Details.Bag).Value)
                    TotCns = TotCns + Val(.Rows(i).Cells(dgvCol_Details.cones).Value)
                    TotWgt = TotWgt + Val(.Rows(i).Cells(dgvCol_Details.weight).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(dgvCol_Details.amount).Value)
                    Tot_Cnsl_Odr = Tot_Cnsl_Odr + Val(.Rows(i).Cells(dgvCol_Details.Cancel_Order).Value)

                End If

            Next

        End With

        lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotBgs)
            .Rows(0).Cells(4).Value = Val(TotCns)
            .Rows(0).Cells(5).Value = Format(Val(TotWgt), "########0.000")
            .Rows(0).Cells(8).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(10).Value = Format(Val(Tot_Cnsl_Odr), "########0.000")
        End With

        Agent_Commission_Calculation()
        GST_Calculation()
        NetAmount_Calculation()

    End Sub

    Private Sub Agent_Commission_Calculation()
        Dim AgCommAmt As Single = 0
        Dim TotBags As Integer = 0

        With dgv_Details_Total
            If .RowCount > 0 Then
                TotBags = Val(.Rows(0).Cells(3).Value)
            End If
        End With

        If Trim(UCase(cbo_CommType.Text)) = "%" Then
            AgCommAmt = Val(lbl_GrossAmount.Text) * Val(txt_CommRate.Text) / 100
        Else
            AgCommAmt = Val(TotBags) * Val(txt_CommRate.Text)
        End If

        lbl_CommAmount.Text = Format(Val(AgCommAmt), "#########0.00")

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single
        Dim GST_Amt As Single = 0
        If NoCalc_Status = True Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then

            lbl_DiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

            txt_AssessableValue.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess_BeforeTax.Text) + Val(txt_Freight.Text), "########0.00")

        End If
        GST_Amt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Or "1369" Then
        '    NtAmt = Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess_BeforeTax.Text) + GST_Amt
        'Else
        NtAmt = Val(txt_AssessableValue.Text) + Val(txt_AddLess_AfterTax.Text) + GST_Amt
        ' End If



        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")
        If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""

        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        Printing_Selection()
        'pnl_Back.Enabled = False
        'pnl_Print.Visible = True
    End Sub

    Private Sub Printing_Selection()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        Dim NewCode As String = ""

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Yarn_Purchase_Order_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Yarn_Sales_Order_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'", con)
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

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        If PpSzSTS = False Then
            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()
                Else
                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                If ps.Width = 800 And ps.Height = 600 Then
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PpSzSTS = True
                                    Exit For
                                End If
                            Next

                            If PpSzSTS = False Then
                                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                                        PpSzSTS = True
                                        Exit For
                                    End If
                                Next

                                If PpSzSTS = False Then
                                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                            PrintDocument1.DefaultPageSettings.PaperSize = ps
                                            Exit For
                                        End If
                                    Next
                                End If

                            End If

                            PrintDocument1.Print()

                        End If

                    Else

                        PrintDocument1.Print()

                    End If
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName , e.Ledger_Name as Agent_Name,CSH.State_Name as Company_State_Name  ,CSH.State_Code as Company_State_Code ,LSH.State_Name as Ledger_State_Name ,LSH.State_Code as Ledger_State_Code , f.Ledger_MAINName as DelName , f.Ledger_Address1 as DelAdd1 ,f.Ledger_Address2 as DelAdd2, f.Ledger_Address3 as DelAdd3 ,f.Ledger_Address4 as DelAdd4, f.Pan_No DelPanNo, f.Ledger_GSTinNo as DelGSTinNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code  from Yarn_Sales_Order_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_HEad CSH on b.Company_State_IdNo = CSH.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN State_HEad LSH on c.Ledger_State_IdNo = LSH.State_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo = a.Agent_IdNo LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on f.Ledger_State_IdNo = DSH.State_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.count_name , c.Mill_Name,b.Count_Description ,I.Item_GST_Percentage,i.Item_HSN_Code from Yarn_Sales_Order_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo LEFT OUTER JOIN ItemGroup_Head I ON b.ItemGroup_IdNo = I.ItemGroup_IdNo where a.Yarn_Sales_Order_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Yarn_Sales_Order_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)


            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Trim(Common_Procedures.settings.CustomerCode) = "1037" Then '-----PRAKASH TEXTILES
            Printing_Format2GST(e)
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1234" Then '-----ARULJOTHI EXPORTS PVT LTD
            Printing_Format2(e)
            'Printing_FormatGST_1234(e)
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1334" Then '-----SIVASELVI TEXTILES
            Printing_FormatGST_1334(e)
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1186" Then '----- United Weaves
            Printing_Format_1186(e)
        Else
            Printing_FormatGST(e)
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 45
            .Top = 45
            .Bottom = 45
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

        NoofItems_PerPage = 15 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(50) : ClArr(2) = 60 : ClArr(3) = 220 : ClArr(4) = 80 : ClArr(5) = 120 : ClArr(6) = 100
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 18 Then
                            For I = 18 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 18
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim DelvToName As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Sales_Order_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Order_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
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

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " YARN PURCHASE RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            W1 = e.Graphics.MeasureString("PURCHASE NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Bill NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10

            DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))

            'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Delivery At", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + W1 + 30, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Rec No", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Receipt_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        'Dim W1 As Single = 0
        Dim BmsInWrds As String

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width
            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( - )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "VAT. 5 % ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY = CurY + TxtHgt
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "3. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

            '' Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. All payment should be made by A/C payesr cheque or draft.", LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 50

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
                .Top = 300
            Else
                .Top = 10
            End If

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

        NoofItems_PerPage = 4 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(60) : ClArr(2) = 210 : ClArr(3) = 120 : ClArr(4) = 120 : ClArr(5) = 120 : ClArr(6) = 120
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18 '18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        CurY = CurY + TxtHgt

                        prn_DetSNo = prn_DetSNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 15, CurY, 0, 0, pFont)


                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString & "  -  " & prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString, LMargin + ClArr(1) + 11, CurY, 0, 0, pFont,, True, LMargin + ClArr(1) + ClArr(2) - 5)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 15, CurY, 1, 0, pFont)
                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 15, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, PageWidth - 15, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim DelvToName As String
        Dim CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""


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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
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

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
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
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then
            Common_Procedures.Print_To_PrintDocument(e, "SALES ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
        Common_Procedures.Print_To_PrintDocument(e, " YARN SALES ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        End If
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            W1 = e.Graphics.MeasureString("SALES NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "SO No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Bill NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SO Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))


            Common_Procedures.Print_To_PrintDocument(e, "Delivery To", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Payment Terms", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "     :", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(4), LMargin + C1, LnAr(2))
            'CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt


            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))


            C1 = ClAr(1) + ClAr(2) + ClAr(3) - 100
            W1 = e.Graphics.MeasureString("AGENT  : ", pFont).Width
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Agent :", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString), LMargin + W1 + 30, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Agent_Commission_Rate").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Comm ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Agent_Commission_Rate").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Bill Amount   :  " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), PageWidth - 15, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Bill Amount   :  ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 110, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Signature of the Receiver", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_Through, txt_CommRate, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_CommRate, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub



    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub



    Private Sub cbo_Delvat_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delvat.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Delvat.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, Nothing, Nothing, "", "", "", "")
        If (e.KeyValue = 38 And cbo_TaxType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_PaymenTerms.Visible = True Then
                txt_PaymenTerms.Focus()
            Else
                msk_DeliveryDate.Focus()
            End If
        End If
        If (e.KeyValue = 40 And cbo_TaxType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Count)
            Else
                txt_DiscPerc.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Count)
            Else
                txt_DiscPerc.Focus()
            End If

        End If
    End Sub

    Private Sub cbo_TaxType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.LostFocus
        If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
            cbo_TaxType.Tag = cbo_TaxType.Text
            GST_Calculation()
        End If
    End Sub

    Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
        GST_Calculation()
    End Sub

    Private Sub cbo_Grid_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_CountName.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If cbo_TaxType.Visible = True Then
                    cbo_TaxType.Focus()
                Else
                    msk_DeliveryDate.Focus()
                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(dgvCol_Details.Count).Value) = "" Then
                    txt_DiscPerc.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End If
        End With

    End Sub

    Private Sub cbo_Grid_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        'Dim Cn_bag As Integer
        'Dim Wgt_Bag As Integer
        'Dim Wgt_Cn As Integer
        'Dim mill_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(dgvCol_Details.Count).Value = Trim(cbo_Grid_CountName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(dgvCol_Details.Count).Value) = "" Then
                    txt_DiscPerc.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If
        'If Asc(e.KeyChar) = 13 Then

        '    With dgv_Details

        '        If Val(.Rows(.CurrentRow.Index).Cells(3).Value) = 0 Or Trim(.Rows(.CurrentRow.Index).Cells(4).Value) = 0 Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_Grid_CountName.Text)) Then

        '            mill_idno_idno = Common_Procedures.Processed_Item_NameToIdNo(con, Trim(cbo_Grid_CountName.Text))

        '            da = New SqlClient.SqlDataAdapter("select a.Meter_Qty, b.unit_name from Processed_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno Where a.Processed_Item_IdNo = " & Str(Val(Itm_idno)), con)
        '            dt = New DataTable
        '            da.Fill(dt)

        '            Mtr_Qty = 0
        '            Unt_nm = ""
        '            If dt.Rows.Count > 0 Then
        '                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '                    Mtr_Qty = Val(dt.Rows(0).Item("Meter_Qty").ToString)
        '                    Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)
        '                End If
        '            End If

        '            dt.Dispose()
        '            da.Dispose()

        '            If Val(Mtr_Qty) <> 0 Then .Rows(.CurrentRow.Index).Cells(4).Value = Format(Val(Mtr_Qty), "#########0.00")
        '            .Rows(dgv_Details.CurrentRow.Index).Cells(6).Value = Trim(Unt_nm)

        '        End If

        '        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)

        '        If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
        '            txt_DiscPerc.Focus()

        '        Else
        '            .Focus()
        '            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

        '        End If

        '    End With

        'End If

    End Sub

    Private Sub cbo_Grid_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp

        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    txt_DiscPerc.Focus()

                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(dgvCol_Details.Count)

                End If


            End If

        End With
    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    txt_DiscPerc.Focus()

                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(dgvCol_Details.Count)

                End If
            End With

        End If
    End Sub
    Private Sub cbo_Grid_millName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_MillName.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_RackNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_RackNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(dgvCol_Details.Mill_Name).Value = Trim(cbo_Grid_MillName.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Grid_RackNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_Grid_MillName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.LostFocus

        'If Trim(UCase(cbo_Grid_MillName.Tag)) <> Trim(UCase(cbo_Grid_MillName.Text)) Then
        '    get_MillCount_Details()
        'End If
    End Sub

    Private Sub cbo_Grid_RackNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.Mill_Name Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_RateFor.Text)
    End Sub

    Private Sub cbo_Grid_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_RateFor.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_RateFor, Nothing, Nothing, "", "", "", "")


        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_RateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_RateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Grid_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_RateFor.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_RateFor, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(dgvCol_Details.Rate_for).Value = Trim(cbo_Grid_RateFor.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Filter_DeliveryAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_DeliveryAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_DeliveryAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DeliveryAt.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DeliveryAt, cbo_Filter_PartyName, cbo_Filter_Count, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_DeliveryAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DeliveryAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DeliveryAt, cbo_Filter_Count, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER')", "(Ledger_IdNo = 0)")

    End Sub




    Private Sub txt_Commbag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True


    End Sub

    Private Sub cbo_CommType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CommType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CommType, txt_CommRate, Nothing, "", "", "", "")
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Count)
            End If
        End If

    End Sub

    Private Sub cbo_CommType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CommType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CommType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Count)

            End If
        End If
    End Sub

    Private Sub txt_CommRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CommRate.TextChanged
        Agent_Commission_Calculation()
    End Sub

    Private Sub cbo_Grid_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.TextChanged
        Try
            If cbo_Grid_RateFor.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_RateFor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.Rate_for Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RateFor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub lbl_NetAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_NetAmount.TextChanged
        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If
    End Sub

    Private Sub cbo_CommType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CommType.TextChanged
        Agent_Commission_Calculation()
    End Sub


    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
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

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_save_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                            dgv_Details_KeyUp(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Filter_DeliveryAt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DeliveryAt.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Delvat.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.TextChanged
        Try
            If cbo_Colour.Visible Then
                With dgv_Details
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.Colour Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_PartyName.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub txt_AssessableValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AssessableValue.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_AssessableValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AssessableValue.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Date.ValueChanged
        msk_Date.Text = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
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

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PartyName.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Note.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub cbo_Agent_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.LostFocus
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ag_Perc As Single = 0
        Dim Ag_BagRate As Single = 0
        Dim Ag_idno As Integer = 0

        Ag_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        da = New SqlClient.SqlDataAdapter("select a.* from ledger_head a where a.ledger_idno = " & Str(Val(Ag_idno)) & "  and a.Ledger_Type='AGENT'", con)
        dt = New DataTable
        da.Fill(dt)

        ag_Perc = 0
        Ag_BagRate = 0

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                ag_Perc = Val(dt.Rows(0).Item("Yarn_Comm_Percentage").ToString)
                Ag_BagRate = Val(dt.Rows(0).Item("Yarn_Comm_Bag").ToString)
            End If
        End If
        dt.Dispose()
        da.Dispose()

        If Trim(UCase(cbo_CommType.Text)) = "BAG" Then

            txt_CommRate.Text = Val(Ag_BagRate)

        Else

            txt_CommRate.Text = Val(ag_Perc)

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

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        Else
            movenext_record()
        End If
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Sales_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, txt_Note, "Yarn_Sales_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_Note, "Yarn_Sales_Head", "Vehicle_No", "", "", False)

    End Sub


    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub cbo_Filter_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Count.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Count.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Count, cbo_Filter_DeliveryAt, btn_Filter_Show, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")


        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_Filter_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Count, btn_Filter_Show, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show_Click(sender, e)
            End If
        End If

    End Sub

    Private Sub Total_Tax_Calculation()
        Dim Sno As Integer
        Dim TotAss_Val As Single
        Dim TotCGST_amt As Single
        Dim TotSGST_amt As Double
        Dim TotIGST_amt As Double
        If Common_Procedures.settings.CustomerCode = "1186" Then Exit Sub

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotAss_Val = 0 : TotCGST_amt = 0 : TotSGST_amt = 0 : TotIGST_amt = 0

        With dgv_Tax_Details
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



        With dgv_Tax_Total_Details
            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(2).Value = Format(Val(TotAss_Val), "##########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotCGST_amt), "##########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotSGST_amt), "##########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotIGST_amt), "##########0.00")

        End With

        txt_AssessableValue.Text = Format(Val(TotAss_Val), "##########0.00")
        lbl_CGST_Amount.Text = IIf(Val(TotCGST_amt) <> 0, Format(Val(TotCGST_amt), "##########0.00"), "")
        lbl_SGST_Amount.Text = IIf(Val(TotSGST_amt) <> 0, Format(Val(TotSGST_amt), "##########0.00"), "")
        lbl_IGST_Amount.Text = IIf(Val(TotIGST_amt) <> 0, Format(Val(TotIGST_amt), "##########0.00"), "")

    End Sub

    Private Sub GST_Calculation()
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim CGST_Per As Single = 0, SGST_Per As Single = 0, IGST_Per As Single = 0, GST_Per As Single = 0
        Dim HSN_Code As String = ""
        Dim Taxable_Amount As Double = 0
        Dim Led_IdNo As Integer = 0

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            With dgv_Details

                If dgv_Details.Rows.Count > 0 Then

                    For RowIndx = 0 To dgv_Details.Rows.Count - 1


                        .Rows(RowIndx).Cells(dgvCol_Details.Disc_perc).Value = ""
                        .Rows(RowIndx).Cells(dgvCol_Details.Disc_AMt).Value = ""
                        .Rows(RowIndx).Cells(dgvCol_Details.Taxable_value).Value = ""  ' Taxable value
                        .Rows(RowIndx).Cells(dgvCol_Details.Gst_Perc).Value = ""  ' GST %
                        .Rows(RowIndx).Cells(dgvCol_Details.Hsn_Code).Value = ""  ' HSN code

                        If Trim(.Rows(RowIndx).Cells(dgvCol_Details.Count).Value) <> "" Or Val(.Rows(RowIndx).Cells(dgvCol_Details.Bag).Value) = 0 Or Val(.Rows(RowIndx).Cells(dgvCol_Details.weight).Value) = 0 Or Val(.Rows(RowIndx).Cells(dgvCol_Details.amount).Value) = 0 Then

                            HSN_Code = ""
                            GST_Per = 0
                            Get_GST_Percentage_From_ItemGroup(Trim(.Rows(RowIndx).Cells(dgvCol_Details.Count).Value), HSN_Code, GST_Per)


                            '--Cash discount
                            .Rows(RowIndx).Cells(dgvCol_Details.Disc_perc).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                            .Rows(RowIndx).Cells(dgvCol_Details.Disc_AMt).Value = Format(Val(.Rows(RowIndx).Cells(dgvCol_Details.Count).Value) * (Val(.Rows(RowIndx).Cells(dgvCol_Details.Disc_perc).Value) / 100), "########0.00")

                            '-- Taxable value = amount -  cash disc
                            Taxable_Amount = Val(.Rows(RowIndx).Cells(dgvCol_Details.amount).Value) - Val(.Rows(RowIndx).Cells(dgvCol_Details.Disc_AMt).Value)


                            .Rows(RowIndx).Cells(dgvCol_Details.Taxable_value).Value = Format(Val(Taxable_Amount), "##########0.00")
                            .Rows(RowIndx).Cells(dgvCol_Details.Gst_Perc).Value = Format(Val(GST_Per), "########0.00")
                            .Rows(RowIndx).Cells(dgvCol_Details.Hsn_Code).Value = Trim(HSN_Code)

                        End If

                    Next RowIndx

                    Get_HSN_CodeWise_Tax_Details()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DO GST CALCULATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub Get_GST_Percentage_From_ItemGroup(ByVal CountName As String, ByRef HSN_Code As String, ByRef GST_PerCent As Single)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            HSN_Code = ""
            GST_PerCent = 0

            da = New SqlClient.SqlDataAdapter("select a.* from ItemGroup_Head a INNER JOIN Count_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where b.Count_Name ='" & Trim(CountName) & "'", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Item_HSN_Code").ToString) = False Then
                    HSN_Code = Trim(dt.Rows(0).Item("Item_HSN_Code").ToString)
                End If
                If IsDBNull(dt.Rows(0).Item("Item_GST_Percentage").ToString) = False Then
                    'CGST_PerCent = Val(dt.Rows(0).Item("Item_GST_Percentage").ToString) / 2
                    'SGST_PerCent = Val(dt.Rows(0).Item("Item_GST_Percentage").ToString) / 2
                    'IGST_PerCent = Val(dt.Rows(0).Item("Item_GST_Percentage").ToString)

                    GST_PerCent = Val(dt.Rows(0).Item("Item_GST_Percentage").ToString)

                End If

            End If

            dt.Clear()


        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub cbo_PartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.LostFocus
        If Trim(UCase(cbo_PartyName.Tag)) <> Trim(UCase(cbo_PartyName.Text)) Then
            cbo_PartyName.Tag = cbo_PartyName.Text
            GST_Calculation()
        End If
    End Sub
    Private Sub Get_HSN_CodeWise_Tax_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim Led_IdNo As Integer = 0
        Dim AssVal_Pack_Frgt_Ins_Amt As String = ""
        Dim InterStateStatus As Boolean = False

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            If cbo_TaxType.Text = "GST" Then

                AssVal_Pack_Frgt_Ins_Amt = Format((Val(txt_Freight.Text)) + (Val(txt_AddLess_BeforeTax.Text)), "#########0.00")

                With dgv_Details

                    If .Rows.Count > 0 Then
                        For i = 0 To .Rows.Count - 1
                            If Trim(.Rows(i).Cells(dgvCol_Details.Count).Value) <> "" And Val(.Rows(i).Cells(dgvCol_Details.Gst_Perc).Value) <> 0 And Trim(.Rows(i).Cells(dgvCol_Details.Hsn_Code).Value) <> "" Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1                ,                  Currency1            ,                       Currency2                                             ) " &
                                                    "          Values     ( '" & Trim(.Rows(i).Cells(dgvCol_Details.Hsn_Code).Value) & "', " & Val(.Rows(i).Cells(dgvCol_Details.Gst_Perc).Value) & " ,  " & Str(Val(.Rows(i).Cells(dgvCol_Details.Taxable_value).Value) + Val(AssVal_Pack_Frgt_Ins_Amt)) & " ) "
                                cmd.ExecuteNonQuery()

                                AssVal_Pack_Frgt_Ins_Amt = 0

                            End If
                        Next

                    End If

                End With

            End If


            With dgv_Tax_Details

                .Rows.Clear()
                Sno = 0

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as TaxableAmount from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 order by Name1, Currency1", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
                    InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), Led_IdNo)

                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1

                        .Rows(n).Cells(0).Value = Sno
                        .Rows(n).Cells(1).Value = dt.Rows(i).Item("HSN_Code").ToString

                        .Rows(n).Cells(2).Value = Format(Val(dt.Rows(i).Item("TaxableAmount").ToString), "############0.00")
                        If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""

                        If InterStateStatus = True Then

                            .Rows(n).Cells(7).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString), "#############0.00")
                            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                        Else

                            .Rows(n).Cells(3).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString) / 2, "############0.00")
                            If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""

                            .Rows(n).Cells(5).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString) / 2, "############0.00")
                            If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""

                        End If

                        .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                        .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                        .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                    Next

                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

            End With

            Total_Tax_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub cbo_PartyName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.SelectedIndexChanged
        Total_Calculation()
    End Sub

    Private Sub btn_Tax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Tax.Click
        pnl_Back.Enabled = False
        pnl_Tax.Visible = True
        pnl_Tax.Focus()
    End Sub

    Private Sub btn_Tax_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Tax_Close.Click
        pnl_Tax.Visible = False
        pnl_Back.Enabled = True
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Close_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Receipt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Order.Click
        prn_Status = 1
        Printing_Selection()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub Printing_FormatGST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CntName1 As String, CntName2 As String


        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30
            .Bottom = 45
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

        NoofItems_PerPage = 10 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(40) : ClArr(2) = 180 : ClArr(3) = 70 : ClArr(4) = 55 : ClArr(5) = 50 : ClArr(6) = 75 : ClArr(7) = 85 : ClArr(8) = 75
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormatGST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_FormatGST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If
                        If Common_Procedures.settings.CustomerCode = "1333" Then
                            CntName1 = (prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString & "-" & prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString)
                        Else
                            CntName1 = prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString
                        End If

                        CntName2 = ""
                        If Len(CntName1) > 10 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(CntName1), I, 1) = " " Or Mid$(Trim(CntName1), I, 1) = "," _
                                    Or Mid$(Trim(CntName1), I, 1) = "." Or Mid$(Trim(CntName1), I, 1) = "-" _
                                    Or Mid$(Trim(CntName1), I, 1) = "/" Or Mid$(Trim(CntName1), I, 1) = "_" _
                                    Or Mid$(Trim(CntName1), I, 1) = "(" Or Mid$(Trim(CntName1), I, 1) = ")" _
                                    Or Mid$(Trim(CntName1), I, 1) = "\" Or Mid$(Trim(CntName1), I, 1) = "[" _
                                    Or Mid$(Trim(CntName1), I, 1) = "]" Or Mid$(Trim(CntName1), I, 1) = "{" Or Mid$(Trim(CntName1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20

                            CntName2 = Microsoft.VisualBasic.Right(Trim(CntName1), Len(CntName1) - I)
                            CntName1 = Microsoft.VisualBasic.Left(Trim(CntName1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        If Common_Procedures.settings.CustomerCode = "1186" Then
                            Common_Procedures.Print_To_PrintDocument(e, CntName1 & " , " & ItmNm1, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, CntName1, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("GST_Percentage").ToString), "############0.0") & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        End If

                        If Common_Procedures.settings.CustomerCode = "1186" Then
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("bags").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format((Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString)) / (Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString)), "######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            End If

                        Else
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            End If
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If
                        If Common_Procedures.settings.CustomerCode = "1186" Then
                            If Trim(CntName2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntName2) & " , " & ItmNm2, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        Else
                            If Trim(CntName2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntName2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        End If


                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_FormatGST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormatGST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim DelvToName As String = ""
        Dim CurY1 As Single = 0, CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Purchase_Head a  INNER JOIN Ledger_Head b ON  b.Ledger_IdNo = a.Ledger_Idno  LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
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
            If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
            End If
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
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


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

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
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN SALES ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("PURCHASE NO     : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "PO Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Common_Procedures.settings.CustomerCode <> "1186" Then


                Common_Procedures.Print_To_PrintDocument(e, "Due Days", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If prn_HdDt.Rows(0).Item("Attend").ToString <> "" Then


                Common_Procedures.Print_To_PrintDocument(e, "Attend Mr.", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Attend").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Common_Procedures.settings.CustomerCode <> "1186" Then


                Common_Procedures.Print_To_PrintDocument(e, "Delivery Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            'DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery To", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                pFont = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "YES", LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + 10

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            If Trim(prn_DetDt.Rows(0).Item("Mill_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Mill Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Mill_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 5
            pFont = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            If Common_Procedures.settings.CustomerCode = "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "WGT/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Else

                Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY
            pFont = New Font("Calibri", 11, FontStyle.Regular)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormatGST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""

        Dim DelInc As Integer = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable


        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
            If Common_Procedures.settings.CustomerCode <> "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width


            CurY = CurY + TxtHgt - 10
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                End If
            End If





            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS : ", LMargin + 10, CurY + 10, 0, 0, p1Font)
            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY + TxtHgt + 10, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + 10, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + 10, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + 10, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN  :   " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + 10, 0, 0, p1Font)

            End If



            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            'DelGSTinNo
            CurY = CurY + TxtHgt + 10

            Common_Procedures.Print_To_PrintDocument(e, " Payment Terms  :  " & prn_HdDt.Rows(0).Item("Due_Days").ToString & " Days", LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            If Common_Procedures.settings.CustomerCode = "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "Order Completion Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Else

                Common_Procedures.Print_To_PrintDocument(e, " Delivery Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess Before Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess After Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If


            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 10



            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 5, CurY, 1, 0, p1Font)
            If Common_Procedures.settings.CustomerCode <> "1369" And Common_Procedures.settings.CustomerCode <> "1186" Then
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
                LnAr(8) = CurY

                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY



            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + +ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            CurY = CurY + TxtHgt - 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            BmsInWrds = StrConv(BmsInWrds, VbStrConv.ProperCase)
            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If
            If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Note : " & prn_HdDt.Rows(0).Item("Note").ToString, LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If


            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt


            CurY = CurY + TxtHgt



            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub Printing_Format2GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 20
            .Bottom = 45
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

        NoofItems_PerPage = 20 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(40) : ClArr(2) = 180 : ClArr(3) = 90 : ClArr(4) = 90 : ClArr(5) = 100 : ClArr(6) = 110
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("GST_Percentage").ToString), "############0.0") & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 5, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ex_Mill").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, PageWidth - 5, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format2GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim DelvToName As String = ""
        Dim CurY1 As Single = 0, CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Purchase_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
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
            If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
            End If


        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
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


        CurY = CurY + TxtHgt - 10
        If Trim(Common_Procedures.settings.CustomerCode) = "1037" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Prakash_logo, Drawing.Image), LMargin + 10, CurY, 100, 80)
        End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

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
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("PURCHASE NO     : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "PO Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            If Trim(Common_Procedures.settings.CustomerCode) = "1037" Then '----PRAKASH TEXTILES (SOMANUR)
                Common_Procedures.Print_To_PrintDocument(e, "Due Days", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Due Days", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Delivery Schedule", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "MMMM-yy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            'DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery To", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                pFont = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "YES", LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + 10

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Agent Name : " & prn_HdDt.Rows(0).Item("Agent_Name").ToString, PageWidth - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 5
            pFont = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "EXTRA", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY
            pFont = New Font("Calibri", 11, FontStyle.Regular)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""
        Dim DelvAddAr() As String
        Dim DelInc As Integer = 0


        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), PageWidth - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 10
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + +ClAr(5) + ClAr(6), LnAr(4))


            C1 = ClAr(1) + ClAr(2) + ClAr(3) - 10
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width

            'CurY = CurY + TxtHgt - 10
            'If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            '    Else
            '        Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    End If
            'End If
            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS : ", LMargin + 10, CurY, 0, 0, p1Font)

            Erase DelvAddAr
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString) <> "" Then
                DelvAddAr = Split(Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString), ",")
                DelInc = -1

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm1 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm2 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm3 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm4 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm5 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm6 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm7 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm8 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm9 = Trim(DelvAddAr(DelInc))
                End If
                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm10 = Trim(DelvAddAr(DelInc))
                End If
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm1, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm2, LMargin + 10, CurY + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm3, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm4, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm5, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm6, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm7, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm9, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " Payment Terms  :  " & prn_HdDt.Rows(0).Item("Due_Days").ToString & " Days", PageWidth - 10, CurY, 1, 0, p1Font)
            ''Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " Delivery Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), PageWidth - 10, CurY, 1, 0, p1Font)
            ''Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "AddLess Before Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
            '    End If
            'End If

            ''Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
            '    End If
            'End If
            ''Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "AddLess After Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
            '    End If
            'End If


            ''Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            ''Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            'End If

            CurY = CurY + TxtHgt + TxtHgt + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            'If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            '    End If

            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            'End If
            'CurY = CurY + TxtHgt

            'If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            '    End If

            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            '    End If

            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            'End If
            'CurY = CurY + TxtHgt

            'If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            '    End If

            'End If


            'CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(8) = CurY
            'CurY = CurY + TxtHgt - 10
            ''Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 5, CurY, 1, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)
            'LnAr(8) = CurY

            'CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

            'CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(9) = CurY
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))

            'CurY = CurY + TxtHgt - 10
            'BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            'BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            'Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Please arrange to supply as per details shown above to the conditions and we agree to abide by them", LMargin + 5, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Print_ReverseCharge_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_ReverseCharge.Click
        prn_Status = 2
        Printing_Selection()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Yarn_Sales_Order_GST_Tax_Details Where Yarn_Sales_Order_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Yarn_Sales_Order_GST_Tax_Details Where Yarn_Sales_Order_Code = '" & Trim(EntryCode) & "'", con)
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

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
                .Top = 300
            Else
                .Top = 30
            End If

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

        NoofItems_PerPage = 4 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(60) : ClArr(2) = 180 : ClArr(3) = 80 : ClArr(4) = 150 : ClArr(5) = 160
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        CurY = CurY + TxtHgt

                        prn_DetSNo = prn_DetSNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 11, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 15, CurY, 1, 0, pFont)
                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 15, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "###########0.00"), PageWidth - 15, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim DelvToName As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
                'Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
                ' Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
            End If

        End If

        CurY = CurY + 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " YARN PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + strHeight + 10
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



        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            W1 = e.Graphics.MeasureString("PURCHASE NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Purchase No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Bill NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Bill Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))


            Common_Procedures.Print_To_PrintDocument(e, "Delivery To", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt - 5




            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(4), LMargin + C1, LnAr(2))
            'CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY - 10, 2, ClAr(6), pFont)
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "(Include .GST)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt - 10, 2, ClAr(6), p1Font)
            ' Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt


            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY



            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) - 100
            W1 = e.Graphics.MeasureString("AGENT  : ", pFont).Width
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Agent :", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString), LMargin + W1 + 30, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Agent_Commission_Rate").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Comm ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Agent_Commission_Rate").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Bill Amount   :  " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), PageWidth - 15, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Bill Amount   :  ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 110, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Signature of the Receiver", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        'Print_PDF_Status = False
    End Sub

    'Private Sub Cbo_DeliveryAddress_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_DeliveryAddress.GotFocus
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Sales_Order_Head", "Delivery_Address", "", "")
    'End Sub


    'Private Sub Cbo_DeliveryAddress_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_DeliveryAddress.KeyDown
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_DeliveryAddress, cbo_Delvat, cbo_Agent, "Yarn_Sales_Order_Head", "Delivery_Address", "", "")
    'End Sub

    'Private Sub Cbo_DeliveryAddress_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_DeliveryAddress.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_DeliveryAddress, cbo_Agent, "Yarn_Sales_Order_Head", "Delivery_Address", "", "", False)
    'End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
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

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        'If LedIdNo = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection
            If Val(LedIdNo) <> 0 Then

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name,c.*  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno  INNER JOIN Own_Order_Yarn_Details c ON  c.Own_Order_Code = a.Own_Order_Code LEFT OUTER JOIN Yarn_Sales_Order_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'   and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno INNER JOIN Own_Order_Yarn_Details c ON  c.Own_Order_Code = a.Own_Order_Code   LEFT OUTER JOIN Yarn_Sales_Order_Head d ON d.Yarn_Sales_Order_Code = a.Own_Order_Code    where a.Yarn_Sales_Order_Code = ''  and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("OWn_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            Else
                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno   LEFT OUTER JOIN Yarn_Sales_Order_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Yarn_Sales_Order_Code = '" & Trim(NewCode) & "'   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno    LEFT OUTER JOIN Yarn_Sales_Order_Head d ON d.Yarn_Sales_Order_Code = a.Own_Order_Code    where a.Yarn_Sales_Order_Code = ''   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            End If
        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(6).Value = ""

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

                Select_Piece(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Receipt_Selection()
    End Sub

    Private Sub Close_Receipt_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        lbl_OrderNo.Text = ""
        lbl_OrderCode.Text = ""

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then

                ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                lbl_OrderNo.Text = dgv_Selection.Rows(i).Cells(3).Value
                lbl_OrderCode.Text = dgv_Selection.Rows(i).Cells(7).Value

            End If

        Next

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        If cbo_Delvat.Enabled And cbo_Delvat.Visible Then cbo_Delvat.Focus()

    End Sub

    Private Sub dtp_DeliveryDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_DeliveryDate.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_DeliveryDate.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_DeliveryDate.Focus()
        End If
    End Sub

    Private Sub dtp_DeliveryDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_DeliveryDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub dtp_DeliveryDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_DeliveryDate.KeyUp
        If e.KeyCode = 17 Then
            e.Handled = True
            msk_Date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_DeliveryDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_DeliveryDate.TextChanged
        If IsDate(dtp_DeliveryDate.Text) = True Then
            msk_DeliveryDate.Text = dtp_DeliveryDate.Text
            msk_DeliveryDate.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_DeliveryDate_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_DeliveryDate.ValueChanged
        msk_DeliveryDate.Text = dtp_DeliveryDate.Text
    End Sub

    Private Sub msk_DeliveryDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_DeliveryDate.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_DueDays.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True And e.SuppressKeyPress = True
            If Common_Procedures.settings.CustomerCode = "1186" Then
                cbo_tax_type.Focus()
            Else

                cbo_TaxType.Focus()

            End If


        End If

        vmsDelyOldTxt = ""
        vmsDelySelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            e.Handled = True
            vmsDelyOldTxt = msk_DeliveryDate.Text
            vmsDelySelStrt = msk_DeliveryDate.SelectionStart
        End If
    End Sub

    Private Sub msk_DeliveryDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_DeliveryDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            e.Handled = True
            msk_DeliveryDate.Text = Date.Today
        End If
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1186" Then
                cbo_tax_type.Focus()
            Else

                cbo_TaxType.Focus()

            End If
        End If

    End Sub

    Private Sub msk_DeliveryDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_DeliveryDate.KeyUp
        Dim vmsDelRetTxt As String = ""
        Dim vmsDelRetSltTxt As Integer = -1

        If e.KeyCode = 107 Then
            e.Handled = True
            msk_DeliveryDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_DeliveryDate.Text))
            msk_DeliveryDate.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            e.Handled = True
            msk_DeliveryDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_DeliveryDate.Text))
            msk_DeliveryDate.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(con, e, vmsDelyOldTxt, vmsDelySelStrt)
        End If
    End Sub

    Private Sub msk_DeliveryDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_DeliveryDate.LostFocus
        If IsDate(dtp_DeliveryDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_DeliveryDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_DeliveryDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_DeliveryDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_DeliveryDate.Text)) >= 2000 Then
                    dtp_DeliveryDate.Value = Convert.ToDateTime(msk_DeliveryDate.Text)
                End If
            End If
        End If
    End Sub



    Private Sub Printing_FormatGST_1334(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CntName1 As String, CntName2 As String


        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30
            .Bottom = 45
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

        NoofItems_PerPage = 12 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 110 : ClArr(3) = 70 : ClArr(4) = 55 : ClArr(5) = 50 : ClArr(6) = 60 : ClArr(7) = 90 : ClArr(8) = 70
        ClArr(9) = 85
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormatGST_PageHeader_1334(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_FormatGST_PageFooter_1334(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CntName1 = prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString
                        CntName2 = ""
                        If Len(CntName1) > 10 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(CntName1), I, 1) = " " Or Mid$(Trim(CntName1), I, 1) = "," _
                                    Or Mid$(Trim(CntName1), I, 1) = "." Or Mid$(Trim(CntName1), I, 1) = "-" _
                                    Or Mid$(Trim(CntName1), I, 1) = "/" Or Mid$(Trim(CntName1), I, 1) = "_" _
                                    Or Mid$(Trim(CntName1), I, 1) = "(" Or Mid$(Trim(CntName1), I, 1) = ")" _
                                    Or Mid$(Trim(CntName1), I, 1) = "\" Or Mid$(Trim(CntName1), I, 1) = "[" _
                                    Or Mid$(Trim(CntName1), I, 1) = "]" Or Mid$(Trim(CntName1), I, 1) = "{" Or Mid$(Trim(CntName1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20

                            CntName2 = Microsoft.VisualBasic.Right(Trim(CntName1), Len(CntName1) - I)
                            CntName1 = Microsoft.VisualBasic.Left(Trim(CntName1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, CntName1, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("GST_Percentage").ToString), "############0.0") & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 2, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString, PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(CntName2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CntName2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If


                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_FormatGST_PageFooter_1334(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormatGST_PageHeader_1334(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim DelvToName As String = ""
        Dim CurY1 As Single = 0, CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Purchase_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
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
            If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
            End If
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
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


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

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
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("PURCHASE NO     : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "PO Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Due Days", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Delivery Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            'DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery To", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                pFont = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "YES", LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + 10

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 5
            pFont = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Count Disc", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY
            pFont = New Font("Calibri", 11, FontStyle.Regular)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormatGST_PageFooter_1334(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""
        Dim DelvAddAr() As String
        Dim DelInc As Integer = 0


        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width


            CurY = CurY + TxtHgt - 10
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                End If
            End If
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS : ", LMargin + 10, CurY, 0, 0, p1Font)

            Erase DelvAddAr
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString) <> "" Then
                DelvAddAr = Split(Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString), ",")
                DelInc = -1

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm1 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm2 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm3 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm4 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm5 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm6 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm7 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm8 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm9 = Trim(DelvAddAr(DelInc))
                End If
                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm10 = Trim(DelvAddAr(DelInc))
                End If
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm1, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm2, LMargin + 10, CurY + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm3, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm4, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm5, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm6, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm7, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm9, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


            Common_Procedures.Print_To_PrintDocument(e, " Payment Terms  :  " & prn_HdDt.Rows(0).Item("Due_Days").ToString & " Days", LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " Delivery Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess Before Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess After Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If


            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 10



            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + +ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            CurY = CurY + TxtHgt - 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt


            CurY = CurY + TxtHgt



            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Verified_Sts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Verified_Sts.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Verified_Sts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Verified_Sts.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, cbo_Filter_Count, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub


    Public Sub Get_vehicle_from_Transport()

        If Common_Procedures.settings.CustomerCode <> "1186" Then Exit Sub


        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim transport_id As Integer
        transport_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Transport.Text)
        Da = New SqlClient.SqlDataAdapter("select vehicle_no from ledger_head where ledger_idno=" & Str(Val(transport_id)) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count <> 0 Then
            cbo_VehicleNo.Text = Dt.Rows(0).Item("vehicle_no").ToString


        End If
        Dt.Clear()
    End Sub
    Private Sub btn_UserModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
    Private Sub GraceTime_Calculation()
        If FrmLdSTS = True Then Exit Sub
        msk_DeliveryDate.Text = ""
        msk_grdate.Text = ""

        If IsDate(msk_Date.Text) = True And Val(txt_PaymenTerms.Text) >= 0 Then
            msk_DeliveryDate.Text = DateAdd("d", Val(txt_PaymenTerms.Text), Convert.ToDateTime(msk_Date.Text))
        End If

        If IsDate(msk_Date.Text) = True And Val(txt_PaymenTerms.Text) >= 0 Then
            msk_grdate.Text = DateAdd("d", Val(txt_DueDays.Text), Convert.ToDateTime(msk_Date.Text))
        End If
    End Sub

    Private Sub txt_PaymenTerms_TextChanged(sender As Object, e As EventArgs) Handles txt_PaymenTerms.TextChanged

        GraceTime_Calculation()
    End Sub

    Private Sub txt_Attend_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Attend.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then

            cbo_Through.Focus()


        End If
    End Sub

    Private Sub txt_Attend_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Attend.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Through.Focus()



        End If
    End Sub
    Private Sub Printing_Format_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CntName1 As String, CntName2 As String


        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30
            .Bottom = 45
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9.75, FontStyle.Regular)

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

        NoofItems_PerPage = 6 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 100 : ClArr(3) = 60 : ClArr(4) = 150 : ClArr(5) = 50 : ClArr(6) = 50 : ClArr(7) = 65 : ClArr(8) = 60 : ClArr(9) = 60
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_1186_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If
                        If Common_Procedures.settings.CustomerCode = "1333" Then
                            CntName1 = (prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString & "-" & prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString)
                        Else
                            CntName1 = prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString
                        End If

                        CntName2 = ""
                        If Len(CntName1) > 10 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(CntName1), I, 1) = " " Or Mid$(Trim(CntName1), I, 1) = "," _
                                    Or Mid$(Trim(CntName1), I, 1) = "." Or Mid$(Trim(CntName1), I, 1) = "-" _
                                    Or Mid$(Trim(CntName1), I, 1) = "/" Or Mid$(Trim(CntName1), I, 1) = "_" _
                                    Or Mid$(Trim(CntName1), I, 1) = "(" Or Mid$(Trim(CntName1), I, 1) = ")" _
                                    Or Mid$(Trim(CntName1), I, 1) = "\" Or Mid$(Trim(CntName1), I, 1) = "[" _
                                    Or Mid$(Trim(CntName1), I, 1) = "]" Or Mid$(Trim(CntName1), I, 1) = "{" Or Mid$(Trim(CntName1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20

                            CntName2 = Microsoft.VisualBasic.Right(Trim(CntName1), Len(CntName1) - I)
                            CntName1 = Microsoft.VisualBasic.Left(Trim(CntName1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, CntName1, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)


                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format((Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_per_bag").ToString)), "######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        End If



                        Common_Procedures.Print_To_PrintDocument(e, Format((Val(prn_DetDt.Rows(prn_DetIndx).Item("wgt_per_bag").ToString)), "######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format((Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString)), "######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If
                        If Common_Procedures.settings.CustomerCode = "1186" Then
                            If Trim(CntName2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntName2) & " , " & ItmNm2, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        Else
                            If Trim(CntName2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntName2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        End If


                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1186_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_Add3 As String, city As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String

        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0
        Dim strHeight1 As Single
        Dim I As Integer = 0
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothPurchase_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothPurchase_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY


        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""


        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        'Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        'Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString

        If InStr(1, Trim(UCase(Cmp_Name)), "UNITED") > 0 And InStr(1, Trim(UCase(Cmp_Name)), "WEAVES") > 0 Then
            Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Else
            Cmp_Add1 = "" & prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = "" & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        End If

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
            Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            city = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            If InStr(1, Trim(UCase(Cmp_Name)), "UNITED") > 0 And InStr(1, Trim(UCase(Cmp_Name)), "WEAVES") > 0 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)
            End If
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & "," & city, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        'Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "  / " & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "/ " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "YARN SALES ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(15) = CurY

        CurY = CurY + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SO No. : YN/SO-" & prn_HdDt.Rows(0).Item("Yarn_Sales_Order_No").ToString & "/" & Common_Procedures.FnYearCode, LMargin + 10, CurY, 0, 0, p1Font)
        'strHeight1 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("ClothPurchase_Order_No").ToString, p1Font).Height

        Common_Procedures.Print_To_PrintDocument(e, "SO Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Order_date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + +ClAr(2) + ClAr(3) + (ClAr(4) / 2) + 20, CurY, 2, 0, p1Font)
        'strHeight1 = e.Graphics.MeasureString(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothPurchase_Order_Date").ToString), "dd-MM-yyyy").ToString, p1Font).Height

        Common_Procedures.Print_To_PrintDocument(e, "Party PO No. : " & prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 60, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 50, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + +ClAr(2) + ClAr(3) + (ClAr(4) / 2) + 10, CurY, LMargin + ClAr(1) + +ClAr(2) + ClAr(3) + (ClAr(4) / 2) + 10, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        W1 = e.Graphics.MeasureString("PARTY P.O NO: ", pFont).Width
        w2 = e.Graphics.MeasureString("Desp TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Italic Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT : ", LMargin + C1 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Ledger_MAINName").ToString)

        ItmNm2 = ""
        If Len(ItmNm1) > 30 Then
            For I = 30 To 1 Step -1
                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
            Next I
            If I = 0 Then I = 30

            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)
        If Trim(ItmNm2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(ItmNm2), LMargin + 10, CurY, 0, 0, p1Font)
        End If
        'Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_MAINName").ToString, LMargin + 10, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        End If

        If prn_HdDt.Rows(0).Item("DelGSTinNo").ToString <> "" Then
            'CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "GSTIN  : " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        End If
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "THROUGH     :  " & prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, " AGENT NAME  :  Mr . " & Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Agent_IdNo").ToString)), LMargin + C1 + 20, CurY, 0, 0, pFont)


        'Delivery_GSTinNo
        CurY = CurY + TxtHgt



        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        CurY = CurY + 10
        If Trim(prn_HdDt.Rows(0).Item("Attend").ToString) <> "" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Kind Attn. :  ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 65, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Mr." & Trim(prn_HdDt.Rows(0).Item("Attend").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, p1Font)

        End If
        CurY = CurY + TxtHgt + 20

        Common_Procedures.Print_To_PrintDocument(e, "Dear Sir, ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "We are hereby confirming the sales of yarn as follows :", LMargin + ClAr(1), CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 20

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        CurY = CurY + TxtHgt - 10

        CurY = CurY + TxtHgt - 5
        pFont = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "S.No", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)


        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)
        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/ KG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (Rs.)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)
        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, "(Excl.gst)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        pFont = New Font("Calibri", 11, FontStyle.Regular)



    End Sub

    Private Sub Printing_Format_1186_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p3Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""

        Dim DelInc As Integer = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim BInc As Integer
        Dim BnkDetAr() As String

        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0



        For I = NoofDets + 1 To NoofItems_PerPage

            CurY = CurY + TxtHgt

            prn_DetIndx = prn_DetIndx + 1

        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
        If Common_Procedures.settings.CustomerCode <> "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "" & Format((Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString)), "############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)


        CurY = CurY + TxtHgt - 10

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10

        W1 = e.Graphics.MeasureString("Delivery Schedule         : ", pFont).Width


        CurY = CurY + TxtHgt + 10

        '& prn_HdDt.Rows(0).Item("Attend").ToString


        p1Font = New Font("Calibri", 10, FontStyle.Bold)


        '--------------------------------------------------------------------



        CurY1 = CurY


        Erase BnkDetAr
        If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

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




        '-------------------------------------------------------



        p3Font = New Font("Calibri", 10, FontStyle.Bold)







        ' Common_Procedures.Print_To_PrintDocument(e, " Payment" & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + 10, CurY, 0, 0, p1Font)

        'If Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> "" Then
        Common_Procedures.Print_To_PrintDocument(e, " Cash Discount", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %", LMargin + W1 + 10, CurY, 0, 0, pFont)

        'End If


        If BankNm1 <> "" Then
            'CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 0, 0, p3Font)
        End If




        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, " Tax Type", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("TaxType").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)

        If BankNm2 <> "" Then
            'CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 0, 0, p3Font)
        End If


        'AddLessAfterTax_Text
        If Val(prn_HdDt.Rows(0).Item("Due_days").ToString) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Payment Days", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Val(prn_HdDt.Rows(0).Item("Due_days").ToString), "#####0") & "  Days  " & " / " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Gr_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
        Else
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Payment Days", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Gr_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : AGAINST RTGS \ PROFORMA", LMargin + W1 + 10, CurY, 0, 0, pFont)
        End If

        If BankNm3 <> "" Then
            'CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 0, 0, p3Font)
        End If

        If Val(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Delivery Schedule", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : Before " & Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) & "   Days " & " / " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Delivery_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
        Else
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Delivery Schedule", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Delivery_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :IMMEDIATE ", LMargin + W1 + 10, CurY, 0, 0, pFont)
        End If

        If BankNm4 <> "" Then
            'CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 0, 0, p3Font)
        End If


        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, " Transportation", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Transportations").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then



            Common_Procedures.Print_To_PrintDocument(e, " Remarks", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 20
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 10

        Common_Procedures.Print_To_PrintDocument(e, "a). Delivery Date should be Stictly adhered to.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "b). Kindly mention the PO number in the Invoice.", LMargin + 12, CurY, 0, 0, pFont)

        If Common_Procedures.settings.CustomerCode <> "1186" Then
            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + 5
            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "NOTE : Please send yarn test report of the dispatched yarn along with the goods dispatch . ", LMargin + 10, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt


        CurY = CurY + TxtHgt


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" And Print_PDF_Status = True Then

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.UNITED_WEAVES_SIGN, Drawing.Image), LMargin + 10, CurY, 90, 55)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

        End If

        'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 350, CurY, 0, 0, pFont)



        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)



    End Sub

    Private Sub dtp_gr_date_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_gr_date.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_grdate.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_grdate.Focus()
        End If
    End Sub

    Private Sub dtp_gr_date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_gr_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_grdate.Focus()
        End If
    End Sub

    Private Sub dtp_gr_date_KeyUp(sender As Object, e As KeyEventArgs) Handles dtp_gr_date.KeyUp
        If e.KeyCode = 17 Then
            e.Handled = True
            msk_grdate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_gr_date_TextChanged(sender As Object, e As EventArgs) Handles dtp_gr_date.TextChanged
        If IsDate(dtp_gr_date.Text) = True Then
            msk_grdate.Text = dtp_gr_date.Text
            msk_grdate.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_gr_date_ValueChanged(sender As Object, e As EventArgs) Handles dtp_gr_date.ValueChanged
        msk_grdate.Text = dtp_gr_date.Text
    End Sub

    Private Sub msk_grdate_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_grdate.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_PaymenTerms.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True And e.SuppressKeyPress = True

            txt_PaymenTerms.Focus()



        End If

        vmsDelyOldTxt = ""
        vmsDelySelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            e.Handled = True
            vmsDelyOldTxt = msk_grdate.Text
            vmsDelySelStrt = msk_grdate.SelectionStart
        End If
    End Sub

    Private Sub msk_grdate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_grdate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            e.Handled = True
            msk_grdate.Text = Date.Today
        End If
        If Asc(e.KeyChar) = 13 Then

            txt_PaymenTerms.Focus()


        End If
    End Sub

    Private Sub msk_grdate_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_grdate.KeyUp
        Dim vmsDelRetTxt As String = ""
        Dim vmsDelRetSltTxt As Integer = -1

        If e.KeyCode = 107 Then
            e.Handled = True
            msk_grdate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_grdate.Text))
            msk_grdate.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            e.Handled = True
            msk_grdate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_grdate.Text))
            msk_grdate.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(con, e, vmsDelyOldTxt, vmsDelySelStrt)
        End If
    End Sub



    Private Sub msk_grdate_LostFocus(sender As Object, e As EventArgs) Handles msk_grdate.LostFocus
        If IsDate(dtp_gr_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_grdate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_grdate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_grdate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_grdate.Text)) >= 2000 Then
                    dtp_gr_date.Value = Convert.ToDateTime(msk_grdate.Text)
                End If
            End If
        End If
    End Sub

    Private Sub cbo_transportation_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_transportation.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Sales_Order_Head", "Transportations", "", "(Yarn_Sales_Order_No=0)")

    End Sub
    Private Sub cbo_transportation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_transportation.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_transportation, txt_DiscPerc, cbo_TaxType, "Yarn_Sales_Order_Head", "Transportations", "", "(Yarn_Sales_Order_No=0)")

    End Sub

    Private Sub cbo_transportation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_transportation.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_transportation, txt_Note, "Yarn_Sales_Order_Head", "Transportations", "", "(Yarn_Sales_Order_No = 0)", False)

    End Sub

    Private Sub cbo_Tax_Type_GotFocus(sender As Object, e As EventArgs) Handles cbo_tax_type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Sales_Order_Head", "taxtype", "", "(Yarn_Sales_Order_No=0)")
    End Sub

    Private Sub cbo_tax_Type_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_tax_type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_tax_type, msk_DeliveryDate, Nothing, "Yarn_Sales_Order_Head", "taxtype", "", "(Yarn_Sales_Order_No=0)")
        If (e.KeyValue = 40) Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Count)

            Else
                txt_Note.Focus()

            End If


        End If
    End Sub

    Private Sub cbo_Tax_Type_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_tax_type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_tax_type, Nothing, "Yarn_Sales_Order_Head", "taxtype", "", "(Yarn_Sales_Order_No = 0)", False)
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Count)

            Else
                txt_Note.Focus()

            End If
        End If
    End Sub

    Private Sub Yarn_Sales_Order_GST_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout

    End Sub

    Private Sub txt_DueDays_TextChanged(sender As Object, e As EventArgs) Handles txt_DueDays.TextChanged
        GraceTime_Calculation()
    End Sub

    Private Sub cbo_Through_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Through.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Through_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Through.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Through, txt_Attend, cbo_Agent, "", "", "", "")
    End Sub

    Private Sub cbo_Through_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Through.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Through, cbo_Agent, "", "", "", "")
    End Sub


    Private Sub Printing_FormatGST_1234(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CntName1 As String, CntName2 As String


        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30
            .Bottom = 45
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

        NoofItems_PerPage = 2 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(40) : ClArr(2) = 180 : ClArr(3) = 70 : ClArr(4) = 55 : ClArr(5) = 50 : ClArr(6) = 75 : ClArr(7) = 85 : ClArr(8) = 75
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 15 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormatGST_1234_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_FormatGST_1234_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If
                        If Common_Procedures.settings.CustomerCode = "1333" Then
                            CntName1 = (prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString & "-" & prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString)
                        Else
                            CntName1 = prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString
                        End If

                        CntName2 = ""
                        If Len(CntName1) > 10 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(CntName1), I, 1) = " " Or Mid$(Trim(CntName1), I, 1) = "," _
                                    Or Mid$(Trim(CntName1), I, 1) = "." Or Mid$(Trim(CntName1), I, 1) = "-" _
                                    Or Mid$(Trim(CntName1), I, 1) = "/" Or Mid$(Trim(CntName1), I, 1) = "_" _
                                    Or Mid$(Trim(CntName1), I, 1) = "(" Or Mid$(Trim(CntName1), I, 1) = ")" _
                                    Or Mid$(Trim(CntName1), I, 1) = "\" Or Mid$(Trim(CntName1), I, 1) = "[" _
                                    Or Mid$(Trim(CntName1), I, 1) = "]" Or Mid$(Trim(CntName1), I, 1) = "{" Or Mid$(Trim(CntName1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20

                            CntName2 = Microsoft.VisualBasic.Right(Trim(CntName1), Len(CntName1) - I)
                            CntName1 = Microsoft.VisualBasic.Left(Trim(CntName1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        If Common_Procedures.settings.CustomerCode = "1186" Then
                            Common_Procedures.Print_To_PrintDocument(e, CntName1 & " , " & ItmNm1, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, CntName1, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("GST_Percentage").ToString), "############0.0") & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        End If

                        If Common_Procedures.settings.CustomerCode = "1186" Then
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("bags").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format((Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString)) / (Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString)), "######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            End If

                        Else
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            End If
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If
                        If Common_Procedures.settings.CustomerCode = "1186" Then
                            If Trim(CntName2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntName2) & " , " & ItmNm2, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        Else
                            If Trim(CntName2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntName2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        End If


                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_FormatGST_1234_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormatGST_1234_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim DelvToName As String = ""
        Dim CurY1 As Single = 0, CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Purchase_Head a  INNER JOIN Ledger_Head b ON  b.Ledger_IdNo = a.Ledger_Idno  LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
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
            If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
            End If
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
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


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

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
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN SALES ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("PURCHASE NO     : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "PO Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Common_Procedures.settings.CustomerCode <> "1186" Then


                Common_Procedures.Print_To_PrintDocument(e, "Due Days", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If prn_HdDt.Rows(0).Item("Attend").ToString <> "" Then


                Common_Procedures.Print_To_PrintDocument(e, "Attend Mr.", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Attend").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Common_Procedures.settings.CustomerCode <> "1186" Then


                Common_Procedures.Print_To_PrintDocument(e, "Delivery Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            'DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery To", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                pFont = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "YES", LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + 10

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 5
            pFont = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            If Common_Procedures.settings.CustomerCode = "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "WGT/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Else

                Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY
            pFont = New Font("Calibri", 11, FontStyle.Regular)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormatGST_1234_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""

        Dim DelInc As Integer = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable


        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
            If Common_Procedures.settings.CustomerCode <> "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width


            CurY = CurY + TxtHgt - 10
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                End If
            End If





            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS : ", LMargin + 10, CurY + 10, 0, 0, p1Font)
            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY + TxtHgt + 10, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + 10, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + 10, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + 10, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN  :   " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + 10, 0, 0, p1Font)

            End If



            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            'DelGSTinNo
            CurY = CurY + TxtHgt + 10

            Common_Procedures.Print_To_PrintDocument(e, " Payment Terms  :  " & prn_HdDt.Rows(0).Item("Due_Days").ToString & " Days", LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            If Common_Procedures.settings.CustomerCode = "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "Order Completion Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Else

                Common_Procedures.Print_To_PrintDocument(e, " Delivery Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess Before Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess After Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If


            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 10



            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 5, CurY, 1, 0, p1Font)
            If Common_Procedures.settings.CustomerCode <> "1369" And Common_Procedures.settings.CustomerCode <> "1186" Then
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
                LnAr(8) = CurY

                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY



            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + +ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            CurY = CurY + TxtHgt - 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            BmsInWrds = StrConv(BmsInWrds, VbStrConv.ProperCase)
            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If
            If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Note : " & prn_HdDt.Rows(0).Item("Note").ToString, LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If


            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt


            CurY = CurY + TxtHgt



            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

End Class