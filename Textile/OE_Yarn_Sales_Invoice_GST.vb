Imports System.Drawing.Printing
Imports System.IO
Public Class OE_Yarn_Sales_Invoice_GST
    Implements Interface_MDIActions
    Public vOE_Entry_Yarn_No As Integer = 0
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GCNIN-" ' "CNINV-"
    Private Pk_Condition2 As String = "GINCN-" '"INVCN-"
    Private NoFo_STS As Integer = 0
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_Count As Integer

    Dim prn_GST_Perc As Single
    Dim prn_CGST_Amount As Double
    Dim prn_SGST_Amount As Double
    Dim prn_IGST_Amount As Double
    Dim vTot_BagNos As Single
    Private Print_PDF_Status As Boolean = False


    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Pack_Selection.Visible = False
        pnl_Selection.Visible = False
        lbl_InvNo.Text = ""
        lbl_InvNo.ForeColor = Color.Black
        Print_PDF_Status = False

        cbo_EntType.Text = "DIRECT"

        dtp_Date.Text = ""
        dtp_DesDate.Text = ""
        cbo_PartyName.Text = ""
        txt_lotNo.Text = ""

        cbo_SalesAc.Text = ""
        cbo_CountName.Text = ""
        cbo_Description_Count.Text = ""
        cbo_Filter_Count.Text = ""
        cbo_BagKg.Text = "BAG"
        cbo_Agent.Text = ""
        cbo_Vechile.Text = ""
        cbo_Conetype.Text = ""
        txt_InvoiceBag.Text = ""
        txt_InvWgt.Text = ""
        txt_Description.Text = ""
        cbo_YarnDescription.Text = "100% COTTON GREY YARN"
        cbo_Transport.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262---" Then '---- Sri Sathuragiri Texttiles (OE) (Rasipalayam)
            cbo_YarnDescription.Text = ""
        End If
        txt_DiscPerc.Text = ""
        txt_BaleNos.Text = ""
        txt_CommBag.Text = ""
        Txt_pack_no_of_bags.Text = ""
        txt_BagNoSelection.Text = ""

        lbl_Amount.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""
        txt_BaleNos.Text = ""
        txt_DelAddress1.Text = ""
        txt_DeliveryAddress.Text = ""
        txt_DcNo.Text = ""
        txt_DiscPerc.Text = ""
        cbo_TaxType.Text = "GST"
        lbl_Grid_HsnCode.Text = ""
        txt_Freight.Text = ""
        txt_AddLess.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        txt_TotalChippam.Text = ""
        txt_DesTime.Text = ""
        txt_rate.Text = ""
        cbo_DeliveryTo.Text = ""


        txt_OrderNo.Text = ""
        txt_OrderDate.Text = ""
        txt_EWay_BillNo.Text = ""
        cbo_Destination.Text = ""
        cbo_PaymentTerms.Text = ""
        cbo_DeliveryTerms.Text = ""
        txt_DeliveryNote.Text = ""
        txt_Supplier_Reference.Text = ""
        txt_Other_References.Text = ""

        txt_Tcs_Name.Text = "TCS"
        txt_TcsPerc.Text = ""
        lbl_TcsAmount.Text = ""
        pnl_TotalSales_Amount.Visible = True
        txt_TCS_TaxableValue.Text = ""
        txt_TCS_TaxableValue.Enabled = False
        lbl_TotalSales_Amount_Current_Year.Text = "0.00"
        lbl_TotalSales_Amount_Previous_Year.Text = "0.00"

        chk_TCS_Tax.Checked = True


        pic_IRN_QRCode_Image.BackgroundImage = Nothing
        txt_IR_No.Text = ""
        txt_eInvoiceNo.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckDate.Text = ""
        txt_eInvoice_CancelStatus.Text = ""

        txt_eWayBill_No.Text = ""
        txt_EWB_Date.Text = ""
        txt_EWB_ValidUpto.Text = ""
        rtbeInvoiceResponse.Text = ""
        txt_EWB_Cancel_Status.Text = ""
        txt_EWB_Canellation_Reason.Text = ""




        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ConeType.Text = ""
            cbo_Filter_Count.Text = ""

            cbo_Filter_Count.SelectedIndex = -1
            cbo_Filter_ConeType.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Grid_Cell_DeSelect()

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
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Cotton_Invoice_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvNo.Text = dt1.Rows(0).Item("Cotton_Invoice_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Cotton_Invoice_Date").ToString
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_CountName.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Count_IdNo").ToString))
                cbo_Description_Count.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Des_Count_IdNo").ToString))
                cbo_Conetype.Text = Common_Procedures.Conetype_IdNoToName(con, Val(dt1.Rows(0).Item("ConeType_Idno").ToString))
                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                txt_Description.Text = dt1.Rows(0).Item("Description").ToString
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_EntType.Text = dt1.Rows(0).Item("Entry_Type").ToString

                txt_CommBag.Text = Format(Val(dt1.Rows(0).Item("Com_Bag").ToString), "##########0.00")
                txt_InvoiceBag.Text = Format(Val(dt1.Rows(0).Item("Invoice_Bags").ToString), "#########0.00")
                txt_InvWgt.Text = Format(Val(dt1.Rows(0).Item("Invoice_Weight").ToString), "#########0.00")
                txt_rate.Text = Format(Val(dt1.Rows(0).Item("Rate").ToString), "#########0.00")
                lbl_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "#########0.00")
                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Vat_Type").ToString
                If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")

                lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGST_Amount").ToString), "#########0.00")
                lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGST_Amount").ToString), "#########0.00")
                lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGST_Amount").ToString), "#########0.00")
                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Taxable_Amount").ToString), "#########0.00")


                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                cbo_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString
                cbo_BagKg.Text = dt1.Rows(0).Item("Commission_Type").ToString
                txt_TotalChippam.Text = Format(Val(dt1.Rows(0).Item("Total_Chippam").ToString), "#########0.00")
                dtp_DesDate.Text = dt1.Rows(0).Item("Des_Date").ToString
                txt_DesTime.Text = dt1.Rows(0).Item("Des_Time_Text").ToString
                txt_DcNo.Text = dt1.Rows(0).Item("Dc_No").ToString
                txt_BaleNos.Text = dt1.Rows(0).Item("Bale_Nos").ToString
                txt_DeliveryAddress.Text = dt1.Rows(0).Item("Delivery_Address").ToString
                txt_DelAddress1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString

                lbl_ReceiptCode.Text = dt1.Rows(0).Item("Cotton_Delivery_Code").ToString

                cbo_YarnDescription.Text = dt1.Rows(0).Item("Yarn_Details").ToString
                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString

                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                'cbo_DeliveryTo.Text = Common_Procedures.Despatch_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                lbl_grid_GstPerc.Text = dt1.Rows(0).Item("GST_Percentage").ToString
                lbl_Grid_HsnCode.Text = dt1.Rows(0).Item("HSN_Code").ToString

                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_OrderDate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_EWay_BillNo.Text = dt1.Rows(0).Item("EWay_BIll_No").ToString
                cbo_Destination.Text = dt1.Rows(0).Item("Destination").ToString
                cbo_PaymentTerms.Text = dt1.Rows(0).Item("Payment_Terms").ToString
                cbo_DeliveryTerms.Text = dt1.Rows(0).Item("Delivery_Terms").ToString
                txt_DeliveryNote.Text = dt1.Rows(0).Item("Delivery_Note").ToString
                txt_Supplier_Reference.Text = dt1.Rows(0).Item("Supplier_Reference").ToString
                txt_Other_References.Text = dt1.Rows(0).Item("Other_References").ToString
                txt_lotNo.Text = Trim(dt1.Rows(0).Item("lot_no").ToString)
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                txt_TCS_TaxableValue.Text = dt1.Rows(0).Item("TCS_Taxable_Value").ToString
                If Val(dt1.Rows(0).Item("EDIT_TCS_TaxableValue").ToString) = 1 Then
                    txt_TCS_TaxableValue.Enabled = True
                End If
                txt_TcsPerc.Text = Val(dt1.Rows(0).Item("Tcs_Percentage").ToString)
                lbl_TcsAmount.Text = dt1.Rows(0).Item("TCS_Amount").ToString

                If Val(dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False


                txt_IR_No.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                txt_eInvoiceNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_Date")) Then txt_eInvoiceAckDate.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_Date").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancelled_Status")) Then txt_eInvoice_CancelStatus.Text = IIf(dt1.Rows(0).Item("E_Invoice_Cancelled_Status") = True, "Cancelled", "Active")

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



                da2 = New SqlClient.SqlDataAdapter("Select a.* from Cotton_Invoice_Details a  Where a.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Bag_No").ToString
                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("Bag_Code").ToString
                            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cotton_Packing_Code").ToString
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cotton_Delivery_Code").ToString
                            .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Cotton_Delivery_Details_Slno").ToString
                            .Rows(n).Cells(7).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("StockfROM_IdNo").ToString))

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "###########0.00")

                End With

                get_Ledger_TotalSales()

            End If

            Grid_Cell_DeSelect()
            If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

        NoCalc_Status = False



    End Sub

    Private Sub Cotton_Invoice_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Conetype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CONETYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Conetype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Description_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Description_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
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

                Me.Text = Me.Name & "  -  " & lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Cotton_Invoice_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub


    Private Sub Cotton_Invoice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Pack_Selection.Visible = True Then
                    btn_Pack_Close_Selection_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub
                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cotton_Invoice_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""

        con.Open()

        cbo_BagKg.Items.Clear()
        cbo_BagKg.Items.Add("")
        cbo_BagKg.Items.Add("BAG")
        cbo_BagKg.Items.Add("KG")

        cbo_EntType.Items.Clear()
        cbo_EntType.Items.Add("")
        cbo_EntType.Items.Add("DIRECT")
        cbo_EntType.Items.Add("PACKING")
        cbo_EntType.Items.Add("ORDER")
        cbo_EntType.Items.Add("DELIVERY")


        'Common_Procedures.get_VehicleNo_From_All_Entries(con)


        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("")
        cbo_TaxType.Items.Add("GST")
        cbo_TaxType.Items.Add("NO TAX")

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            btn_SaveAll.Visible = True
        End If

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Pack_Selection.Visible = False
        pnl_Pack_Selection.Left = (Me.Width - pnl_Pack_Selection.Width) \ 2
        pnl_Pack_Selection.Top = (Me.Height - pnl_Pack_Selection.Height) \ 2
        pnl_Pack_Selection.BringToFront()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BagKg.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Conetype.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Description_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnDescription.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvoicePrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ConeType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvoiceBag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvWgt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommBag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BaleNos.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Description.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalChippam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DesTime.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_DesDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Description_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelAddress1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryAddress.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BagNoSelection.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EWay_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Destination.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryNote.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Supplier_Reference.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Other_References.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_lotNo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_TCS_TaxableValue.Enter, AddressOf ControlGotFocus
        AddHandler txt_TcsPerc.Enter, AddressOf ControlGotFocus
        AddHandler txt_IR_No.Enter, AddressOf ControlGotFocus

        AddHandler txt_TCS_TaxableValue.Leave, AddressOf ControlLostFocus
        AddHandler txt_TcsPerc.Leave, AddressOf ControlLostFocus

        AddHandler txt_lotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BagKg.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_YarnDescription.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Conetype.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvoicePrefixNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_ConeType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvoiceBag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvWgt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommBag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BaleNos.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Description.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DesTime.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalChippam.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_DesDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Description_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelAddress1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryAddress.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BagNoSelection.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EWay_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryNote.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Destination.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Supplier_Reference.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Other_References.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IR_No.Leave, AddressOf ControlLostFocus

        AddHandler txt_TCS_TaxableValue.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TcsPerc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_InvoiceBag.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_InvWgt.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Description.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommBag.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BaleNos.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalChippam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_DesDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DesTime.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DeliveryAddress.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EWay_BillNo.KeyDown, AddressOf TextBoxControlKeyDown
        '   AddHandler txt_Supplier_Reference.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Other_References.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DeliveryNote.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_lotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_IR_No.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_TCS_TaxableValue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TcsPerc.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvoiceBag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvWgt.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Description.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_CommBag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BaleNos.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalChippam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_DesDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DeliveryAddress.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DesTime.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_BaleNos.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvoicePrefixNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EWay_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Supplier_Reference.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Other_References.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_lotNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_IR_No.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    'Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
    '    Dim dgv1 As New DataGridView

    '    On Error Resume Next


    '    If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

    '        dgv1 = Nothing

    '        If ActiveControl.Name = dgv_Details.Name Then
    '            dgv1 = dgv_Details

    '        ElseIf dgv_Details.IsCurrentRowDirty = True Then
    '            dgv1 = dgv_Details

    '        Else
    '            dgv1 = dgv_Details

    '        End If

    '        With dgv1
    '            If keyData = Keys.Enter Or keyData = Keys.Down Then

    '                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
    '                    If .CurrentCell.RowIndex = .RowCount - 1 Then
    '                        If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
    '                            save_record()
    '                        Else
    '                            dtp_Date.Focus()
    '                        End If
    '                    Else
    '                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

    '                    End If

    '                Else

    '                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
    '                        If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
    '                            save_record()
    '                        Else
    '                            dtp_Date.Focus()
    '                        End If
    '                    Else
    '                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

    '                    End If

    '                End If

    '                Return True

    '            ElseIf keyData = Keys.Up Then
    '                If .CurrentCell.ColumnIndex <= 1 Then
    '                    If .CurrentCell.RowIndex = 0 Then
    '                        cbo_Filter_Count.Focus()

    '                    Else
    '                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex - 1)

    '                    End If

    '                Else
    '                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

    '                End If

    '                Return True



    '            Else
    '                Return MyBase.ProcessCmdKey(msg, keyData)

    '            End If

    '        End With

    '    Else

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    End If

    'End Function

    Public Sub Print_record() Implements Interface_MDIActions.print_record
        printing_invoice()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        Print_record()
    End Sub

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim CmpName As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.OEENTRY_INVOICE_ENTRY, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Cotton_invoice_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "123")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")


        CmpName = Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" And prn_Status = 1 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" And (Microsoft.VisualBasic.Left(Trim(UCase(CmpName)), 3) = "BNC" And Microsoft.VisualBasic.InStr(1, Trim(UCase(CmpName)), "GARMENT") > 0) Then
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1013" Then
                    If Print_PDF_Status = True Then
                        PrintDocument1.DocumentName = "Invoice"
                        PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                        PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                        PrintDocument1.Print()

                    Else

                        PrintDocument1.Print()

                    End If


                Else

                    If Print_PDF_Status = True Then
                        PrintDocument1.DocumentName = "Invoice"
                        PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                        PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                        PrintDocument1.Print()

                    Else

                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument1.Print()
                        End If

                    End If


                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1


                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                ppd.Document.DefaultPageSettings.PaperSize = pkCustomSize1

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        prn_Count = 0

        Try

            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as Agent_name ,SH.* ,Lsh.State_Name as Ledger_State_Name ,Lsh.State_Code as Ledger_State_Code from Cotton_invoice_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.Ledger_State_Idno = Lsh.State_IDno INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo LEFT OUTER JOIN State_Head SH ON c.Company_State_IdNo = SH.State_Idno LEFT OUTER JOIN Ledger_Head D ON a.Agent_IdNo = d.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_invoice_code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            'prn_HdDt = New DataTable
            'da1.Fill(prn_HdDt)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, C.*, c.area_idno as Ledger_AreaIdNo, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, SDAH.Ledger_Name as DeliveryTo_LedgerName, SDAH.Ledger_Address1 as DeliveryTo_LedgerAddress1, SDAH.Ledger_Address2 as DeliveryTo_LedgerAddress2, SDAH.Ledger_Address3 as DeliveryTo_LedgerAddress3, SDAH.Ledger_Address4 as DeliveryTo_LedgerAddress4, SDAH.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, SDAH.Ledger_PhoneNo as DeliveryTo_LedgerPhoneNo, '' as DeliveryTo_PanNo, SDAH.Area_IdNo as PlaceOF_AreaIdNo, SDAST.State_Name as DeliveryTo_State_Name, SDAST.State_Code as DeliveryTo_State_Code from Cotton_invoice_Head a " &
                                          "INNER JOIN Company_Head                    b ON a.Company_IdNo = b.Company_IdNo " &
                                          "LEFT OUTER JOIN State_Head                 Csh ON b.Company_State_IdNo = Csh.State_IdNo " &
                                          "INNER JOIN Ledger_Head                     c ON  a.Ledger_IdNo = c.Ledger_IdNo " &
                                          "LEFT OUTER JOIN State_Head                 Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo  " &
                                          "Left outer JOIN Ledger_Head                d ON a.Transport_IdNo = d.Ledger_IdNo " &
                                          "Left outer JOIN Ledger_Head                e ON a.Agent_IdNo = e.Ledger_IdNo " &
                                          "LEFT OUTER JOIN Ledger_Head SDAH ON SDAH.Ledger_IdNo = a.DeliveryTo_IdNo " &
                                          "LEFT OUTER JOIN State_Head  SDAST ON SDAST.State_IdNo = SDAH.Ledger_State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_invoice_code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)


            ''da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, C.*, c.area_idno as Ledger_AreaIdNo, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, SDAH.Party_Name as DeliveryTo_LedgerName, SDAH.Address1 as DeliveryTo_LedgerAddress1, SDAH.Address2 as DeliveryTo_LedgerAddress2, SDAH.Address3 as DeliveryTo_LedgerAddress3, SDAH.Address4 as DeliveryTo_LedgerAddress4, SDAH.Gstin_No as DeliveryTo_LedgerGSTinNo, SDAH.Phone_No as DeliveryTo_LedgerPhoneNo, ' ' as DeliveryTo_PanNo, SDAH.Area_IdNo as PlaceOF_AreaIdNo, SDAST.State_Name as DeliveryTo_State_Name, SDAST.State_Code as DeliveryTo_State_Code from Cotton_invoice_Head a " &
            '                              "INNER JOIN Company_Head                    b ON a.Company_IdNo = b.Company_IdNo " &
            '                              "LEFT OUTER JOIN State_Head                 Csh ON b.Company_State_IdNo = Csh.State_IdNo " &
            '                              "INNER JOIN Ledger_Head                     c ON  a.Ledger_IdNo = c.Ledger_IdNo " &
            '                              "LEFT OUTER JOIN State_Head                 Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo  " &
            '                              "Left outer JOIN Ledger_Head                d ON a.Transport_IdNo = d.Ledger_IdNo " &
            '                              "Left outer JOIN Ledger_Head                e ON a.Agent_IdNo = e.Ledger_IdNo " &
            '                              "LEFT OUTER JOIN Sales_DeliveryAddress_Head SDAH ON SDAH.Party_IdNo = a.DeliveryTo_IdNo " &
            '                              "LEFT OUTER JOIN State_Head                 SDAST ON SDAST.State_IdNo = SDAH.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_invoice_code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)

            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, C.*, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, f.Ledger_Name as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo, f.Area_IdNo as PlaceOF_AreaIdNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code, SDAH.*, LSH.State_name as Party_statename, LSH.State_Code as Party_StateCode from Cotton_invoice_Head a " &
            '                              "INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo " &
            '                              "LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo " &
            '                              "INNER JOIN Ledger_Head c ON  a.Ledger_IdNo = c.Ledger_IdNo " &
            '                              " LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo  " &
            '                              "Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo " &
            '                              "Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo " &
            '                              "LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo  " &
            '                              " LEFT OUTER JOIN Sales_DeliveryAddress_Head SDAH ON SDAH.Party_IdNo = a.DeliveryTo_IdNo " &
            '                              " LEFT OUTER JOIN Sales_DeliveryAddress_Head SDA ON SDA.State_IdNo = Lsh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_invoice_code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)


            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, C.Count_Description as Count_Name_Description, c.Count_Name from Cotton_invoice_Head a INNER JOIN Count_Head b on a.Des_Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Count_Head c on a.Count_idno = c.Count_idno  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_invoice_code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Cotton_Invoice_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da2.Dispose()

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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then  'Kalaimagal Textile (Palladam)
            Printing_Format2(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1302" Then '---- RAJA MANGAY COTTON MILLS (PALLADAM)    (OR)   RAJAMANGAY 
            Printing_Format3(e)
        Else
            Printing_Format1(e)
        End If
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.OEENTRY_INVOICE_ENTRY, New_Entry, Me, con, "Cotton_Invoice_Head", "Cotton_Invoice_Code", NewCode, "Cotton_Invoice_Date", "(Cotton_Invoice_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_HankYarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Cotton_Order_Details set Invoice_Weight = a.Invoice_Weight - b.Invoice_Weight, Invoice_bags = a.Invoice_bags - b.Invoice_Bags from Cotton_Order_Details a, Cotton_Invoice_Head b where b.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Cotton_Order_Code = b.Cotton_Order_Code and a.Cotton_Order_Details_Slno = b.Cotton_Order_Details_Slno"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Cotton_Delivery_Head set Cotton_Invoice_Code = '' Where Cotton_Invoice_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '',Cotton_Invoice_Increment = Cotton_Invoice_Increment - 1  Where Cotton_Invoice_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Invoice_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()


        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then



            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ConeType.Text = ""
            cbo_Filter_Count.Text = ""
            cbo_Filter_Count.SelectedIndex = -1
            cbo_Filter_ConeType.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Invoice_No from Cotton_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby, Cotton_Invoice_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Invoice_No from Cotton_Invoice_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby, Cotton_Invoice_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Invoice_No from Cotton_Invoice_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby desc, Cotton_Invoice_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Invoice_No from Cotton_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby desc, Cotton_Invoice_No desc", con)
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

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1353" Then
                lbl_InvNo.Text = Common_Procedures.get_CotConv_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1087" Then 'Kalaimagal Palladam
                lbl_InvNo.Text = Common_Procedures.get_YarnTex_OEYarn_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            Else
                lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_Invoice_Head", "Cotton_Invoice_Code", "For_OrderBy", "Entry_VAT_GST_Type ='GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            End If

            lbl_InvNo.ForeColor = Color.Red

            ' If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

            Da1 = New SqlClient.SqlDataAdapter("select top 1 a.* from Cotton_Invoice_Head a where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cotton_Invoice_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                If Dt1.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then txt_InvoicePrefixNo.Text = Dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                If Dt1.Rows(0).Item("Entry_Type").ToString <> "" Then cbo_EntType.Text = Dt1.Rows(0).Item("Entry_Type").ToString
                If Dt1.Rows(0).Item("SalesAc_IdNo").ToString <> "" Then cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                If Dt1.Rows(0).Item("Discount_Percentage").ToString <> "" Then txt_DiscPerc.Text = Val(Dt1.Rows(0).Item("Discount_Percentage").ToString)
                If Dt1.Rows(0).Item("Vat_Type").ToString <> "" Then cbo_TaxType.Text = Dt1.Rows(0).Item("Vat_Type").ToString
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1262" Then '---- Sri Sathuragiri Texttiles (OE) (Rasipalayam)
                    If Dt1.Rows(0).Item("Yarn_Details").ToString <> "" Then cbo_YarnDescription.Text = Dt1.Rows(0).Item("Yarn_Details").ToString
                End If
                If Val(Dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False
            End If

            Dt1.Clear()

            If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            '  If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim nCotInmovno As String, inpno As String
        Dim RefCode As String
        Dim nJbWrkInvCode As String = ""
        Dim nJbWrkMovNo As String = ""
        Dim dt1 As New DataTable
        Dim nCotwstcode As String = ""
        Dim nCotWstMovNo As String = ""

        Dim vYSInvCode As String = ""
        Dim nYSMovNo As String = ""


        Try

            inpno = InputBox("Enter Inv No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Invoice_No from Cotton_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(RefCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            nCotInmovno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    nCotInmovno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If
            Dt.Clear()


            nJbWrkMovNo = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Then '---- sathuragiri
                nJbWrkInvCode = "GJCBL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Jobwork_Conversion_Bill_No from Jobwork_Conversion_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Conversion_Bill_Code = '" & Trim(nJbWrkInvCode) & "'", con)
                Dt = New DataTable
                Da.Fill(Dt)

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        nJbWrkMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If
                Dt.Clear()

            End If

            nCotWstMovNo = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1353" Then '---- balaji spinning mill
                nCotwstcode = "GSCWS-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Cotton_Waste_Sales_No from Cotton_Waste_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Waste_Sales_Code = '" & Trim(nJbWrkInvCode) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        nCotWstMovNo = Trim(dt1.Rows(0)(0).ToString)
                    End If
                End If
                Dt1.Clear()

            End If

            nYSMovNo = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then 'Kalaimagal Palladam
                vYSInvCode = "GYNSL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_No from Yarn_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(vYSInvCode) & "'", con)
                dt1 = New DataTable
                Da.Fill(dt1)

                If dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        nYSMovNo = Trim(dt1.Rows(0)(0).ToString)
                    End If
                End If
                dt1.Clear()

            End If


            If Val(nCotInmovno) <> 0 Then
                move_record(nCotInmovno)

            ElseIf Val(nJbWrkMovNo) <> 0 And Val(nCotWstMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in JobWork Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(nYSMovNo) <> 0 Then
                MessageBox.Show("Already This Invoice No. is in Textile Yarn Sales Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Inv No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim nCotmovno As String, inpno As String
        'Dim InvCode As String
        Dim nCoInvCode As String = ""
        Dim nJbWrkInvCde As String = ""
        Dim nJbWrkMovNo As String = ""

        Dim nCotwstcode As String = ""
        Dim nCotWstMovNo As String = ""

        Dim vYSInvCode As String = ""
        Dim nYSMovNo As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.OEENTRY_INVOICE_ENTRY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Inv No.", "FOR NEW INV NO. INSERTION...")

            nCoInvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            'InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Invoice_No from Cotton_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(nCoInvCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            'movno = ""

            nCotmovno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    nCotmovno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            nJbWrkMovNo = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Then '---- Sathuragiri

                nJbWrkInvCde = "GJCBL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Jobwork_Conversion_Bill_No from Jobwork_Conversion_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Conversion_Bill_Code = '" & Trim(nJbWrkInvCde) & "'", con)
                Dt = New DataTable
                Da.Fill(Dt)

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        nJbWrkMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If
                Dt.Clear()

            End If

            nCotWstMovNo = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1353" Then '---- balaji spinning mill
                nCotwstcode = "GSCWS-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                da1 = New SqlClient.SqlDataAdapter("select Cotton_Waste_Sales_No from Cotton_Waste_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Waste_Sales_Code = '" & Trim(nCotwstcode) & "'", con)
                Dt1 = New DataTable
                da1.Fill(dt1)

                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        nCotWstMovNo = Trim(dt1.Rows(0)(0).ToString)
                    End If
                End If
                Dt1.Clear()

            End If

            nYSMovNo = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then 'Kalaimagal Palladam
                vYSInvCode = "GYNSL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_No from Yarn_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(vYSInvCode) & "'", con)
                dt1 = New DataTable
                Da.Fill(dt1)

                If dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        nYSMovNo = Trim(dt1.Rows(0)(0).ToString)
                    End If
                End If
                dt1.Clear()

            End If



            If Val(nCotmovno) <> 0 Then
                move_record(nCotmovno)

            ElseIf Val(nJbWrkMovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Jobwork Conversion Invoice", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            ElseIf Val(nCotWstMovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Waste Sales Invoice", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(nYSMovNo) <> 0 Then
                MessageBox.Show("Already This Invoice No. is in Textile Yarn Sales Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid INV No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvNo.Text = Trim(UCase(inpno))

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
        Dim SalesAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Agt_Idno As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim DesCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotBgsNo As Single, vTotWgt As Single, ComAmt As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        ' Dim Trans_ID As Integer
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""
        Dim Nr As Integer = 0
        Dim stk_ID As Integer = 0
        Dim vDelvTo_IdNo As Integer = 0
        Dim vTrans_IdNo As Integer = 0

        Dim vTCS_AssVal_EditSTS As Integer = 0
        Dim vTCS_Tax_Sts As Integer = 0
        Dim vEInvAckDate As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.OEENTRY_INVOICE_ENTRY, New_Entry, Me, con, "Cotton_Invoice_Head", "Cotton_Invoice_Code", NewCode, "Cotton_Invoice_Date", "(Cotton_Invoice_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Cotton_Invoice_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If (Trim(UCase(cbo_EntType.Text)) <> "DIRECT" And Trim(UCase(cbo_EntType.Text)) <> "PACKING" And Trim(UCase(cbo_EntType.Text)) <> "ORDER" And Trim(UCase(cbo_EntType.Text)) <> "DELIVERY") Then
            MessageBox.Show("Invalid Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        If Val(Cnt_ID) = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        DesCnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Description_Count.Text)
        'If Val(DesCnt_ID) = 0 Then
        '    MessageBox.Show("Invalid  Description Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()
        '    Exit Sub
        'End If

        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        Col_ID = Common_Procedures.ConeType_NameToIdNo(con, cbo_Conetype.Text)

        SalesAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        vDelvTo_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        'vDelvTo_IdNo = Common_Procedures.Despatch_NameToIdNo(con, cbo_DeliveryTo.Text)
        vTrans_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)




        'If vDelvTo_IdNo = 0 Then
        '    cbo_DeliveryTo.Text = cbo_PartyName.Text
        '    vDelvTo_IdNo = Common_Procedures.Despatch_NameToIdNo(con, cbo_DeliveryTo.Text)
        'End If

        If SalesAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()
            Exit Sub
        End If
        'If Val(txt_TotalChippam.Text) = 0 Then
        '    MessageBox.Show("Invalid Chippam", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If txt_TotalChippam.Enabled And txt_TotalChippam.Visible Then txt_TotalChippam.Focus()
        '    Exit Sub
        'End If


        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    'If Trim(.Rows(i).Cells(1).Value) = "" Then
                    '    MessageBox.Show("Invalid BagNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(1)
                    '    End If
                    '    Exit Sub
                    'End If


                    If Val(.Rows(i).Cells(2).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        'If Val(lbl_CGstAmount.Text) <> 0 Or Val(lbl_SGstAmount.Text) <> 0 Or Val(lbl_IGstAmount.Text) <> 0 Or Val(lbl_CGstAmount.Text) <> 0 Then
        '    MessageBox.Show("Invalid Tax A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_TaxType.Enabled And cbo_TaxType.Visible Then cbo_TaxType.Focus()
        '    Exit Sub
        'End If

        If (Trim(cbo_TaxType.Text) = "" Or Trim(cbo_TaxType.Text) = "-NIL-") Then
            MessageBox.Show("Invalid Tax Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_TaxType.Enabled And cbo_TaxType.Visible Then cbo_TaxType.Focus()
            Exit Sub
        End If
        'NoFo_STS = 0
        'If chk_Less_Comm.Checked = True Then NoFo_STS = 1

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Then '---- Sri Sathuragiri Texttiles (OE) (Rasipalayam)
            If Trim(cbo_YarnDescription.Text) = "" Then
                MessageBox.Show("Invalid Yarn Description", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_YarnDescription.Enabled And cbo_YarnDescription.Visible Then cbo_YarnDescription.Focus()
                Exit Sub
            End If
        End If

        vTCS_AssVal_EditSTS = 0
        If txt_TCS_TaxableValue.Enabled = True Then vTCS_AssVal_EditSTS = 1

        vTCS_Tax_Sts = 0
        If chk_TCS_Tax.Checked = True Then vTCS_Tax_Sts = 1

        NoCalc_Status = False
        Total_Calculation()

        vTotBgsNo = 0 : vTotWgt = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBgsNo = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(2).Value())

        End If

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@InvDate", dtp_Date.Value.Date)
        cmd.Parameters.AddWithValue("@DesDate", dtp_DesDate.Value.Date)

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
            eiCancel = "1"
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1353" Then      '----- sathuragiri
                    lbl_InvNo.Text = Common_Procedures.get_CotConv_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1087" Then 'Kalaimagal Palladam
                    lbl_InvNo.Text = Common_Procedures.get_YarnTex_OEYarn_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                Else
                    lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_Invoice_Head", "Cotton_Invoice_Code", "For_OrderBy", "Entry_VAT_GST_Type ='GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            End If

            cmd.Connection = con
            cmd.Transaction = tr



            If New_Entry = True Then
                cmd.CommandText = "Insert into Cotton_Invoice_Head ( Entry_VAT_GST_Type ,      Cotton_Invoice_Code                   ,               Company_IdNo       ,           Cotton_Invoice_No    ,                               for_OrderBy                             , Cotton_Invoice_Date ,         Ledger_IdNo      ,   Count_IdNo            ,     ConeType_Idno         ,         SalesAc_IdNo    ,    Des_Count_IdNo           ,   Agent_IdNo             ,              Description              ,           Com_Bag                   ,           Invoice_Bags          ,      Invoice_Weight            ,                  Rate       ,    Amount                        ,  Discount_Percentage                ,              Discount_Amount         ,              Vat_Type           , Vat_Percentage       , Vat_Amount        ,           Freight_Amount          ,              AddLess_Amount       ,               RoundOff_Amount       ,                  Net_Amount               ,   Total_Bags          ,        Total_Weight     ,  Vechile_No                        ,   Total_Chippam                     ,       Des_Date         , Des_Time_Text                     ,                     Dc_No     ,                 Bale_Nos         ,     Delivery_Address                    ,                Delivery_Address1    ,            Cotton_Order_Code      ,    Cotton_Order_details_SlNo         ,                Commission_Type ,                    Entry_Type    ,Yarn_Details                              ,               Invoice_PrefixNo                   , Cotton_Delivery_Code               ,   CGST_Amount                          , SGST_Amount                               , IGST_Amount                    , Taxable_Amount                        ,   GST_Percentage                    ,                    HSN_Code                ,       DeliveryTo_IdNo    ,               Order_No          ,               Order_Date          ,               EWay_BIll_No          ,               Destination           ,               Payment_Terms          ,               Delivery_Terms          ,               Delivery_Note          ,               Supplier_Reference           ,               Other_References           ,                       Lot_No     ,       Tcs_Name_caption           ,              Tcs_percentage       ,                Tcs_Amount    ,      TCS_Taxable_Value,                                EDIT_TCS_TaxableValue      ,                    Tcs_Tax_Status   ,              E_Invoice_IRNO  ,        E_Invoice_QR_Image  ,     Transport_IdNo) " &
                                    "     Values                  (   'GST'             ,'" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",      @InvDate       , " & Str(Val(Led_ID)) & " , " & Str(Val(Cnt_ID)) & " , " & Str(Val(Col_ID)) & " ,  " & Val(SalesAc_ID) & ", " & Str(Val(DesCnt_ID)) & ", " & Str(Val(Agt_Idno)) & ",   '" & Trim(txt_Description.Text) & "',   " & Str(Val(txt_CommBag.Text)) & ", " & Val(txt_InvoiceBag.Text) & ",   " & Val(txt_InvWgt.Text) & " ,  " & Val(txt_rate.Text) & " , " & Str(Val(lbl_Amount.Text)) & ",  " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", '" & Trim(cbo_TaxType.Text) & "',          0           ,   0               , " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & " , " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Val(vTotBgsNo) & "," & Str(Val(vTotWgt)) & ",   '" & Trim(cbo_Vechile.Text) & "'  ," & Val(txt_TotalChippam.Text) & "  , @DesDate               , '" & Trim(txt_DesTime.Text) & "'  , '" & Trim(txt_DcNo.Text) & "' , '" & Trim(txt_BaleNos.Text) & "' , '" & Trim(txt_DeliveryAddress.Text) & "', '" & Trim(txt_DelAddress1.Text) & "', '" & Trim(lbl_OrderCode.Text) & "', " & Val(lbl_OrderDetailSlNo.Text) & ", '" & Trim(cbo_BagKg.Text) & "' , '" & Trim(cbo_EntType.Text) & "' , '" & Trim(cbo_YarnDescription.Text) & "' ,  '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' ,'" & Trim(lbl_ReceiptCode.Text) & "', " & Str(Val(lbl_CGstAmount.Text)) & " , " & Str(Val(lbl_SGstAmount.Text)) & "  ," & Str(Val(lbl_IGstAmount.Text)) & " ," & Str(Val(lbl_Assessable.Text)) & "," & Str(Val(lbl_grid_GstPerc.Text)) & ",'" & Trim(lbl_Grid_HsnCode.Text) & "'," & Str(Val(vDelvTo_IdNo)) & " , '" & Trim(txt_OrderNo.Text) & "', '" & Trim(txt_OrderDate.Text) & "', '" & Trim(txt_EWay_BillNo.Text) & "', '" & Trim(cbo_Destination.Text) & "', '" & Trim(cbo_PaymentTerms.Text) & "', '" & Trim(cbo_DeliveryTerms.Text) & "', '" & Trim(txt_DeliveryNote.Text) & "', '" & Trim(txt_Supplier_Reference.Text) & "', '" & Trim(txt_Other_References.Text) & "' , '" & Trim(txt_lotNo.Text) & "' , '" & Trim(txt_Tcs_Name.Text) & "', " & Str(Val(txt_TcsPerc.Text)) & ", " & Str(Val(lbl_TcsAmount.Text)) & "  , " & Str(Val(txt_TCS_TaxableValue.Text)) & ", " & Str(Val(vTCS_AssVal_EditSTS)) & "  ,  " & Str(Val(vTCS_Tax_Sts)) & " ,  '" & Trim(txt_IR_No.Text) & "'   ,     @QrCode     ," & Str(Val(vTrans_IdNo)) & " ) "
                cmd.ExecuteNonQuery()

                Nr = 0
                cmd.CommandText = "Update Cotton_Order_Details Set Invoice_Weight = Invoice_Weight + " & Str(Val(txt_InvWgt.Text)) & ", Invoice_bags = Invoice_Bags+  " & Val(lbl_Totalbags.Text) & "  Where Cotton_Order_code = '" & Trim(lbl_OrderCode.Text) & "' and Cotton_Order_Details_Slno = " & Str(Val(lbl_OrderDetailSlNo.Text)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                Nr = cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Cotton_Invoice_Head set  Entry_VAT_GST_Type = 'GST', Cotton_Invoice_Date = @InvDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ",  Entry_Type = '" & Trim(cbo_EntType.Text) & "', ConeType_Idno = " & Str(Val(Col_ID)) & ", Count_IdNo = " & Str(Val(Cnt_ID)) & ",SalesAc_IdNo = " & Str(Val(SalesAc_ID)) & ", Agent_IdNo = " & Str(Val(Agt_Idno)) & ", Des_Count_idNo = " & Val(DesCnt_ID) & ",   Com_Bag =  " & Str(Val(txt_CommBag.Text)) & " ,      Invoice_Bags  = " & Val(txt_InvoiceBag.Text) & " ,   Invoice_Weight  =  " & Val(txt_InvWgt.Text) & " ,  Rate =  " & Val(txt_rate.Text) & ",Amount = " & Str(Val(lbl_Amount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ",  Vat_Type = '" & Trim(cbo_TaxType.Text) & "', Vat_Percentage = 0, Vat_Amount = 0, Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", Vechile_No = '" & Trim(cbo_Vechile.Text) & "' , AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Total_Bags = " & Val(vTotBgsNo) & ",Total_Weight  = " & Str(Val(vTotWgt)) & ", Total_Chippam =  " & Str(Val(txt_TotalChippam.Text)) & " ,      Des_Date  = @DesDate ,   Des_Time_Text  =  '" & Trim(txt_DesTime.Text) & "' , Dc_No = '" & Trim(txt_DcNo.Text) & "' , Bale_Nos = '" & Trim(txt_BaleNos.Text) & "' ,Cotton_Delivery_Code ='" & Trim(lbl_ReceiptCode.Text) & "' ,   Invoice_PrefixNo = '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' , Yarn_Details =  '" & Trim(cbo_YarnDescription.Text) & "', Delivery_Address =  '" & Trim(txt_DeliveryAddress.Text) & "' ,Delivery_Address1 = '" & Trim(txt_DelAddress1.Text) & "', Cotton_order_Code=  '" & Trim(lbl_OrderCode.Text) & "' , Cotton_Order_details_SlNo =  " & Val(lbl_OrderDetailSlNo.Text) & " ,Commission_Type = '" & Trim(cbo_BagKg.Text) & "', CGST_Amount = " & Val(lbl_CGstAmount.Text) & " ,SGST_Amount = " & Val(lbl_SGstAmount.Text) & " , IGST_Amount = " & Val(lbl_IGstAmount.Text) & " , Taxable_Amount = " & Val(lbl_Assessable.Text) & ",GST_Percentage=" & Str(Val(lbl_grid_GstPerc.Text)) & " ,HSN_Code='" & Trim(lbl_Grid_HsnCode.Text) & "', DeliveryTo_IdNo = " & Str(Val(vDelvTo_IdNo)) & " , Order_No = '" & Trim(txt_OrderNo.Text) & "' , Order_Date = '" & Trim(txt_OrderDate.Text) & "' , EWay_BIll_No = '" & Trim(txt_EWay_BillNo.Text) & "'  , Destination = '" & Trim(cbo_Destination.Text) & "'  , Payment_Terms = '" & Trim(cbo_PaymentTerms.Text) & "'  , Delivery_Terms = '" & Trim(cbo_DeliveryTerms.Text) & "' , Delivery_Note = '" & Trim(txt_DeliveryNote.Text) & "' , Supplier_Reference = '" & Trim(txt_Supplier_Reference.Text) & "' ,  Other_References = '" & Trim(txt_Other_References.Text) & "',Lot_No= '" & Trim(txt_lotNo.Text) & "' ,Tcs_Name_caption='" & Trim(txt_Tcs_Name.Text) & "', Tcs_percentage=" & Str(Val(txt_TcsPerc.Text)) & ",Tcs_Amount= " & Str(Val(lbl_TcsAmount.Text)) & " , TCS_Taxable_Value = " & Str(Val(txt_TCS_TaxableValue.Text)) & ", EDIT_TCS_TaxableValue = " & Str(Val(vTCS_AssVal_EditSTS)) & "  ,  Tcs_Tax_Status = " & Str(Val(vTCS_Tax_Sts)) & " ,  E_Invoice_IRNO = '" & Trim(txt_IR_No.Text) & "' , E_Invoice_QR_Image =  @QrCode  ,  E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & " , EWB_No = '" & txt_eWayBill_No.Text & "',EWB_Date = '" & txt_EWB_Date.Text & "',EWB_Valid_Upto = '" & txt_EWB_ValidUpto.Text & "' , EWB_Cancelled = " & EWBCancel.ToString & " , EWBCancellation_Reason = '" & txt_EWB_Canellation_Reason.Text & "',Transport_IdNo=" & Str(Val(vTrans_IdNo)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Cotton_Order_Details set Invoice_Weight = a.Invoice_Weight - b.Invoice_Weight, Invoice_bags = a.Invoice_bags - b.Invoice_Bags from Cotton_Order_Details a, Cotton_Invoice_Head b where b.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Cotton_Order_Code = b.Cotton_Order_Code and a.Cotton_Order_Details_Slno = b.Cotton_Order_Details_Slno"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '', Cotton_Invoice_Increment = Cotton_Invoice_Increment - 1  Where Cotton_Invoice_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                'cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = ''  Where Cotton_Invoice_Code =  '" & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()
                cmd.CommandText = "Update Cotton_Delivery_Head set Cotton_Invoice_Code = '' Where Cotton_Invoice_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Partcls = Trim((cbo_PartyName.Text))
            PBlNo = Trim(lbl_InvNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_InvNo.Text)

            cmd.CommandText = "Delete from Cotton_Invoice_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_HankYarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" And Trim(lbl_ReceiptCode.Text) <> "" Then
                cmd.CommandText = "Update Cotton_Delivery_Head set Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' where Cotton_Delivery_Code = '" & Trim(lbl_ReceiptCode.Text) & "' and Ledger_IdNo = " & Str(Val(Led_ID))
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    Throw New ApplicationException("Mismatch of Party & Delivery Details")
                    'tr.Rollback()
                    'MessageBox.Show("Mismatch of Party & Receipt Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            With dgv_Details

                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Sno = Sno + 1


                        stk_ID = 0
                        stk_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(7).Value), tr)


                        cmd.CommandText = "Insert into Cotton_Invoice_Details ( Cotton_Invoice_Code ,               Company_IdNo       ,   Cotton_Invoice_No    ,                     for_OrderBy                                            ,              Cotton_Invoice_Date,             Sl_No     ,                                    Bag_No            ,                Weight                     ,Bag_Code                   , Cotton_Packing_Code                  ,    Cotton_Delivery_Code           ,     Cotton_Delivery_Details_Slno   , StockfROM_IdNo  ) " &
                                            "     Values                 (   '" & Trim(Pk_Condition) & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",       @InvDate            ,  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ",  '" & Trim(.Rows(i).Cells(3).Value) & "', '" & Trim(.Rows(i).Cells(4).Value) & "',  '" & Trim(.Rows(i).Cells(5).Value) & "' , " & Val(.Rows(i).Cells(6).Value) & " , " & Val(stk_ID) & " ) "
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(cbo_EntType.Text)) = "PACKING" Then
                            Nr = 0
                            cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' , Cotton_Invoice_Increment = Cotton_Invoice_Increment + 1 Where Bag_Code = '" & Trim(.Rows(i).Cells(3).Value) & "' AND  Cotton_packing_Code= '" & Trim(.Rows(i).Cells(4).Value) & "' and Count_IdNo  =  " & Str(Val(Cnt_ID)) & " and ConeType_Idno =  " & Str(Val(Col_ID)) & ""
                            Nr = cmd.ExecuteNonQuery()
                        End If


                        If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
                            Nr = 0
                            cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' , Cotton_Invoice_Increment = Cotton_Invoice_Increment + 1 Where Bag_Code = '" & Trim(.Rows(i).Cells(3).Value) & "' AND  Cotton_packing_Code= '" & Trim(.Rows(i).Cells(4).Value) & "' and Count_IdNo  =  " & Str(Val(Cnt_ID)) & " and ConeType_Idno =  " & Str(Val(Col_ID)) & ""
                            Nr = cmd.ExecuteNonQuery()
                        End If
                        'If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then

                        '    Nr = 0
                        '    cmd.CommandText = "Update Cotton_dELIVERY_Head set Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Where Cotton_Delivery_Code = '" & Trim(.Rows(i).Cells(5).Value) & "' AND  Cotton_Delivery_Details_Slno = " & Val(.Rows(i).Cells(6).Value) & "  "
                        '    Nr = cmd.ExecuteNonQuery()
                        'End If
                        If Trim(UCase(cbo_EntType.Text)) <> "DELIVERY" Then

                            cmd.CommandText = "Insert into Stock_hankYarn_Processing_Details ( Reference_Code                        ,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,     Party_Bill_No   ,  Entry_ID         ,   Sl_No      ,     Ledger_idNo      ,               Count_IdNo    ,                ConeType_Idno      , Form_No  ,           Chippam               ,        Weight                                 ) " &
                                                                        "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",    @InvDate   , '" & Trim(PBlNo) & "', '" & Trim(EntID) & "',     " & Str(Val(Sno)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Cnt_ID)) & "," & Str(Val(Col_ID)) & ",  ''   ," & (-1 * Val(txt_TotalChippam.Text)) & "," & Str(-1 * Val(txt_InvWgt.Text)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next

            End With

            If Trim(UCase(cbo_EntType.Text)) <> "DELIVERY" Then

                EntID = Trim(Pk_Condition) & Trim(lbl_InvNo.Text)
                PBlNo = Trim(lbl_InvNo.Text)
                Partcls = Trim(cbo_PartyName.Text)

                Da = New SqlClient.SqlDataAdapter("select count(Bag_No) as bags ,sum(Weight) as wgt , StockfROM_IdNo from Cotton_Invoice_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' group by StockfROM_IdNo ", con)
                Dt1 = New DataTable
                Da.SelectCommand.Transaction = tr
                Da.Fill(Dt1)

                Sno = 0

                If Dt1.Rows.Count > 0 Then
                    For I = 0 To Dt1.Rows.Count - 1
                        Sno = Sno + 1
                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (                       SoftwareType_IdNo  ,                                      Reference_Code                      ,      Company_IdNo                 ,       Reference_No        ,                               For_OrderBy                         ,        Reference_Date,    Particulars ,         Party_Bill_No   ,      Entry_ID      ,             Sl_No              , Count_idNo      ,        ConeType_Idno            ,       Bags                                               ,         Weight                                         ,                   StockAt_IdNo   ) " &
                                                                           "   Values  (" & Str(Val(Common_Procedures.SoftwareTypes.OE_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",    @InvDate   , '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', '" & Trim(EntID) & "' ," & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & "," & Str(Val(Col_ID)) & ", " & Str(-1 * Val(Dt1.Rows(I).Item("bags").ToString)) & "  ," & Str(-1 * Val(Dt1.Rows(I).Item("wgt").ToString)) & " ," & Str(Val(Dt1.Rows(I).Item("StockfROM_IdNo").ToString)) & " )"
                        cmd.ExecuteNonQuery()
                    Next I
                End If
                Dt1.Clear()

                'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code                        ,             Company_IdNo                 ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,   Particulars ,   Party_Bill_No   ,   Entry_ID          ,             Sl_No      ,            Count_idNo      ,        ConeType_Idno  ,  Bags              ,         Weight                                 ) " & _
                '                                                     "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",    @InvDate   , '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "','" & Trim(EntID) & "',  " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & "," & Str(Val(Col_ID)) & ", " & Str(-1 * Val(vTotBgsNo)) & "  ," & Str(-1 * Val(vTotWgt)) & " )"
                'cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            'If Trim(UCase(cbo_BagKg.Text)) = "BAG" Then

            ComAmt = Val(txt_InvoiceBag.Text) * Val(txt_CommBag.Text)

            'Else
            '    ComAmt = Val(txt_InvWgt.Text) * Val(txt_CommBag.Text)

            'End If


            If Val(Agt_Idno) <> 0 Then

                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date,      Ledger_IdNo    ,           Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,             Amount                         ,   Commission_Amount       ,Commission_Type               ,  Weight                       ,Commission_For ,   NoOfBags          ,  Commission_Rate) " &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",   @InvDate  , " & Str(Led_ID) & ", " & Str(Val(Agt_Idno)) & "   , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  " & Str(Val(CSng(lbl_NetAmount.Text))) & ",   " & Str(Val(ComAmt)) & ",'BAG', " & Val(txt_InvWgt.Text) & ",'YARN'         , " & Val(txt_InvoiceBag.Text) & "," & Val(txt_CommBag.Text) & ") "
                cmd.ExecuteNonQuery()

            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            Dim vTCSAmt As String = Format(Val((lbl_TcsAmount.Text)), "#############0.00")

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1353" Then '---- SHRI BALAJII SPINNING MILL (SOMANUR)    (OR)    BALAJI SPINNING MILL (SOMANUR)
                vLed_IdNos = Led_ID & "|" & SalesAc_ID
                vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & Val(CSng(lbl_NetAmount.Text))

            Else
                vLed_IdNos = Led_ID & "|" & SalesAc_ID & "|24|25|26|32"
                vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(CSng(lbl_CGstAmount.Text)) - Val(CSng(lbl_SGstAmount.Text)) - Val(CSng(lbl_IGstAmount.Text)) - Val(vTCSAmt)) & "|" & Val(CSng(lbl_CGstAmount.Text)) & "|" & Val(CSng(lbl_SGstAmount.Text)) & "|" & Val(CSng(lbl_IGstAmount.Text)) & "|" & Val(vTCSAmt)
            End If
            If Common_Procedures.Voucher_Updation(con, "Yarn.Inv", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_InvNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_InvNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.OE_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
            vVou_Amts = Val(ComAmt) & "|" & -1 * Val(ComAmt)
            If Common_Procedures.Voucher_Updation(con, "AgComm.GInv", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_InvNo.Text), dtp_Date.Value.Date, "Inv No : " & Trim(lbl_InvNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.OE_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(lbl_InvNo.Text), Agt_Idno, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.OE_Software, SaveAll_STS)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(Trim(lbl_InvNo.Text))
                End If

            Else

                move_record(Trim(lbl_InvNo.Text))

            End If

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If



            If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()

        Catch ex As Exception
            tr.Rollback()

            Timer1.Enabled = False
            SaveAll_STS = False

            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


        End Try

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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer, CnTy_Id As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            CnTy_Id = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Invoice_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Cotton_Invoice_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Invoice_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_ConeType.Text) <> "" Then
                CnTy_Id = Common_Procedures.ConeType_NameToIdNo(con, cbo_Filter_ConeType.Text)
            End If

            If Trim(cbo_Filter_Count.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_Count.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If
            If Val(CnTy_Id) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.ConeType_IdNo = " & Str(Val(CnTy_Id)) & " "
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Count_IdNo = " & Str(Val(Cnt_IdNo)) & " "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name , d.Conetype_Name ,e.Count_Name  from Cotton_Invoice_Head a INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN ConeType_Head d ON a.Conetype_IdNo = d.Conetype_IdNo  LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cotton_Invoice_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from Cotton_Invoice_Head a INNER JOIN Cotton_Invoice_Details b ON a.Cotton_Invoice_Code = b.Cotton_Invoice_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cotton_Invoice_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Cotton_Invoice_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cotton_Invoice_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("ConeType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Invoice_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")
                    ' dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_NetWeight").ToString), "########0.000")

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

    'Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
    '    dgv_Details_CellLeave(sender, e)

    'End Sub

    'Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim Dt2 As New DataTable
    '    Dim rect As Rectangle

    '    With dgv_Details

    '        If Val(.CurrentRow.Cells(0).Value) = 0 Then
    '            .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
    '        End If


    '    End With

    'End Sub

    'Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
    '    With dgv_Details
    '        If .CurrentCell.ColumnIndex = 2 Then
    '            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
    '                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
    '            Else
    '                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
    '            End If
    '        End If

    '        'If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
    '        '    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
    '        '        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
    '        '    Else
    '        '        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
    '        '    End If
    '        'End If
    '    End With
    'End Sub

    'Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
    '    On Error Resume Next

    '    With dgv_Details
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Then

    '                Total_Calculation()

    '            End If
    '        End If
    '    End With

    'End Sub

    'Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
    '    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub

    'Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
    '    dgv_Details.EditingControl.BackColor = Color.Lime
    'End Sub

    'Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
    '    On Error Resume Next
    '    With dgv_Details
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 2 Then

    '                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
    '                    e.Handled = True
    '                End If

    '            End If
    '        End If
    '    End With

    'End Sub



    'Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
    '    Dim i As Integer
    '    Dim n As Integer

    '    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

    '        With dgv_Details

    '            n = .CurrentRow.Index

    '            If .CurrentCell.RowIndex = .Rows.Count - 1 Then
    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(n).Cells(i).Value = ""
    '                Next

    '            Else
    '                .Rows.RemoveAt(n)

    '            End If

    '            For i = 0 To .Rows.Count - 1
    '                .Rows(i).Cells(0).Value = i + 1
    '            Next

    '        End With

    '    End If
    'End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    'Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
    '    Dim n As Integer = 0

    '    With dgv_Details

    '        n = .RowCount
    '        .Rows(n - 1).Cells(0).Value = Val(n)
    '    End With
    'End Sub
    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click

        save_record()

    End Sub

    Private Sub txt_AddLess_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.GotFocus
        '----
    End Sub

    Private Sub txt_AddLess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess.KeyDown

        If e.KeyValue = 40 Then

            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then


                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
            Else
                '  cbo_Vechile.Focus()
                If txt_TcsPerc.Enabled = True Or txt_TcsPerc.Visible = True Then

                    txt_TcsPerc.Focus()
                Else
                    txt_BaleNos.Focus()

                End If
            End If
        End If
        If e.KeyValue = 38 Then

            txt_Freight.Focus()


        End If

    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then


                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
            Else
                ' cbo_Vechile.Focus()
                If txt_TcsPerc.Enabled = True Or txt_TcsPerc.Visible = True Then

                    txt_TcsPerc.Focus()
                Else
                    txt_BaleNos.Focus()

                End If
            End If
        End If
    End Sub

    Private Sub txt_AddLess_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.LostFocus
        txt_AddLess.Text = Format(Val(txt_AddLess.Text), "#########0.00")
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Packing_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.LostFocus
        txt_Freight.Text = Format(Val(txt_Freight.Text), "#########0.00")
    End Sub

    Private Sub txt_Packing_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
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
            txt_rate.Focus()
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub


    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBgNos As Single
        Dim TotChess As Single
        Dim TotWgt As Single


        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBgNos = 0 : TotChess = 0 : TotWgt = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    TotBgNos = TotBgNos + 1
                    TotWgt = TotWgt + Val(.Rows(i).Cells(2).Value)

                End If

            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBgNos)
            .Rows(0).Cells(2).Value = Format(Val(TotWgt), "########0.000")

        End With

        lbl_Totalbags.Text = Val(TotBgNos)

        If Trim(cbo_EntType.Text) = "PACKING" Then
            txt_InvoiceBag.Text = Val(TotBgNos)
            txt_InvWgt.Text = Format(Val(TotWgt), "###########0.000")
            'NetAmount_Calculation()
        End If


    End Sub


    Private Sub NetAmount_Calculation()
        Dim BlAmt As Double

        Dim AssVal As Double
        Dim AssAmt As Single = 0
        Dim CGSTAmt As Single = 0
        Dim SGSTAmt As Single = 0
        Dim IGSTAmt As Single = 0
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim Led_IdNo As Integer

        Dim vGST_Amt As String = 0
        Dim vTOT_SalAmt As String = 0
        Dim VTCS_AssVal As String = 0

        If NoCalc_Status = True Then Exit Sub

        If Trim(UCase(cbo_BagKg.Text)) = "BAG" Then
            lbl_Amount.Text = Format(Val(txt_InvoiceBag.Text) * Val(txt_rate.Text), "########0.00")
        Else
            lbl_Amount.Text = Format(Val(txt_InvWgt.Text) * Val(txt_rate.Text), "########0.00")
        End If


        lbl_DiscAmount.Text = Format(Val(lbl_Amount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

        lbl_DiscAmount.Text = Format(Val(lbl_Amount.Text) * Val(txt_DiscPerc.Text) / 100, "#########0.00")

        lbl_Assessable.Text = Format(Val(lbl_Amount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess.Text) + +Val(txt_Freight.Text), "#########0.00")

        AssVal = Format(Val(lbl_Assessable.Text), "##########0.00")


        lbl_CGstAmount.Text = 0
        lbl_SGstAmount.Text = 0
        lbl_IGstAmount.Text = 0

        If Trim(cbo_TaxType.Text) = "GST" Then

            Led_IdNo = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "Ledger_Name = '" & Trim(cbo_PartyName.Text) & "'"))
            Get_State_Code(Led_IdNo, Ledger_State_Code, Company_State_Code)

            lbl_grid_GstPerc.Text = 0

            lbl_Grid_HsnCode.Text = ""
            lbl_Grid_HsnCode.Text = Common_Procedures.get_FieldValue(con, "Count_Head", "HSN_Code", "Count_Name = '" & Trim(cbo_CountName.Text) & "'")

            lbl_grid_GstPerc.Text = Val(Common_Procedures.get_FieldValue(con, "Count_Head", "GST_Percentege", "Count_Name = '" & Trim(cbo_CountName.Text) & "'"))


            If Trim(Company_State_Code) = Trim(Ledger_State_Code) Then
                '-CGST 
                lbl_CGstAmount.Text = Format(Val(lbl_Assessable.Text) * (Val(lbl_grid_GstPerc.Text) / 2) / 100, "#########0.00")
                '-SGST 
                lbl_SGstAmount.Text = Format(Val(lbl_Assessable.Text) * (Val(lbl_grid_GstPerc.Text) / 2) / 100, "#########0.00")

            ElseIf Trim(Company_State_Code) <> Trim(Ledger_State_Code) Then
                '-IGST 
                lbl_IGstAmount.Text = Format(Val(lbl_Assessable.Text) * Val(lbl_grid_GstPerc.Text) / 100, "#########0.00")

            End If

        End If

        vGST_Amt = Format(Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text), "###########0.00")

        BlAmt = Val(lbl_Assessable.Text) + vGST_Amt

        If Val(lbl_TotalSales_Amount_Current_Year.Text) = 0 Then lbl_TotalSales_Amount_Current_Year.Text = "0.00"
        If Val(lbl_TotalSales_Amount_Previous_Year.Text) = 0 Then lbl_TotalSales_Amount_Previous_Year.Text = "0.00"



        '---------

        Dim vTCS_StartDate As Date = #9/30/2020#
        Dim vMIN_TCS_assval As String = "5000000"

        If chk_TCS_Tax.Checked = True Then
            If DateDiff("d", vTCS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                If txt_TCS_TaxableValue.Enabled = False Then

                    vTOT_SalAmt = Format(Val(lbl_Assessable.Text) + Val(vGST_Amt), "###########0")

                    VTCS_AssVal = 0
                    If Val(CSng(lbl_TotalSales_Amount_Previous_Year.Text)) > Val(vMIN_TCS_assval) Then
                        VTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf Val(CSng(lbl_TotalSales_Amount_Current_Year.Text)) > Val(vMIN_TCS_assval) Then
                        VTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf (Val(CSng(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt)) > Val(vMIN_TCS_assval) Then
                        VTCS_AssVal = Format(Val(CSng(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt) - Val(vMIN_TCS_assval), "############0")

                    End If
                    txt_TCS_TaxableValue.Text = Format(Val(VTCS_AssVal), "############0.00")

                    If Val(txt_TCS_TaxableValue.Text) > 0 Then
                        If Val(txt_TcsPerc.Text) = 0 Then
                            txt_TcsPerc.Text = "0.075"
                        End If
                    End If

                End If

                lbl_TcsAmount.Text = Format(Val(txt_TCS_TaxableValue.Text) * Val(txt_TcsPerc.Text) / 100, "########0")

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

        '---------
        lbl_NetAmount.Text = Format(Val(BlAmt), "##########0") + Val(lbl_TcsAmount.Text)
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))
        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(BlAmt) - Val(lbl_TcsAmount.Text), "#########0.00")




    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, dtp_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                If MessageBox.Show("Do you want to Select Delivery :", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                Else
                    txt_DcNo.Focus()
                End If

            Else

                txt_DcNo.Focus()
            End If

            get_Ledger_TotalSales()

        End If


    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_lotNo, cbo_DeliveryTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, cbo_DeliveryTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            ' get_agent_comm_bag()
        End If





    End Sub
    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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
    Private Sub cbo_BagKg_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BagKg.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BagKg, cbo_DeliveryTo, txt_CommBag, " ", "", "", "")

    End Sub

    Private Sub cbo_BagKg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BagKg.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BagKg, txt_CommBag, " ", "", "", "")

    End Sub

    Private Sub cbo_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        ' Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, txt_orderNo, cbo_Colour, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, txt_DcNo, cbo_Conetype, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")


    End Sub

    Private Sub cbo_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CountName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CountName, cbo_Conetype, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New OE_Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_Conetype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Conetype.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")
    End Sub
    Private Sub cbo_Conetype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Conetype.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Conetype, cbo_CountName, cbo_YarnDescription, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")


    End Sub

    Private Sub cbo_Conetype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Conetype.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Conetype, Nothing, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_EntType.Text) = "PACKING" Then
                If MessageBox.Show("Do you want to select Pack  :", "FOR PACKING SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Pack_Selection_Click(sender, e)
                Else
                    cbo_YarnDescription.Focus()
                End If
            Else
                cbo_YarnDescription.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Conetyper_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Conetype.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Conetype.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub
    'Private Sub cbo_VatAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyUp
    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

    '        Common_Procedures.MDI_LedType = ""
    '        Dim f As New Ledger_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_VatAc.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()
    '    End If
    'End Sub


    Private Sub btn_close_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_Vechile_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.DropDownClosed
        Try
            'With cbo_DriverName
            '    If .SelectedIndex = -1 Then
            '        '.SelectedText = ""
            '        .SelectedIndex = 0
            '    End If
            'End With

        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cotton_Invoice_Head", "Vechile_No", "", "(Vechile_No = '')")
    End Sub
    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        If e.KeyValue = 38 And cbo_Vechile.DroppedDown = False Then
            e.Handled = True
            '  txt_AddLess.Focus()
            txt_BaleNos.Focus()

            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_Vechile.DroppedDown = False Then
            e.Handled = True
            txt_EWay_BillNo.Focus()
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_Vechile.DroppedDown = False Then
            cbo_Vechile.DroppedDown = True
        End If
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, txt_AddLess, txt_TotalChippam, "Cotton_Invoice_Head", "Vechile_No", "", "")

    End Sub

    'Private Sub cbo_Vechile_KeyPress_111(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, txt_TotalChippam, "Cotton_Invoice_Head", "Vechile_No", "", "", False)
    'End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Dim Indx As Integer = -1
        Dim strFindStr As String = ""

        Try
            If Asc(e.KeyChar) = 8 Then
                If cbo_Vechile.SelectionStart <= 1 Then
                    cbo_Vechile.Text = ""
                    Exit Sub
                End If
                If cbo_Vechile.SelectionLength = 0 Then
                    strFindStr = cbo_Vechile.Text.Substring(0, cbo_Vechile.Text.Length - 1)
                Else
                    strFindStr = cbo_Vechile.Text.Substring(0, cbo_Vechile.SelectionStart - 1)
                End If

            Else

                If cbo_Vechile.SelectionLength = 0 Then
                    strFindStr = cbo_Vechile.Text & e.KeyChar
                Else
                    strFindStr = cbo_Vechile.Text.Substring(0, cbo_Vechile.SelectionStart) & e.KeyChar
                End If

            End If

            Indx = cbo_Vechile.FindString(strFindStr)

            If Indx <> -1 Then
                cbo_Vechile.SelectedText = ""
                cbo_Vechile.SelectedIndex = Indx
                cbo_Vechile.SelectionStart = strFindStr.Length
                cbo_Vechile.SelectionLength = cbo_Vechile.Text.Length
                e.Handled = True
            Else
                'e.Handled = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If Asc(e.KeyChar) = 13 Then
            ' txt_BaleNos.Focus()
            txt_EWay_BillNo.Focus()

        End If

    End Sub


    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_YarnDescription, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation()

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_Comm_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommBag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_InvWgt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_InvWgt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        NetAmount_Calculation()
    End Sub

    Private Sub txt_rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        NetAmount_Calculation()
    End Sub
    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Count.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Count, cbo_Filter_PartyName, cbo_Filter_ConeType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")


    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Count.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Count, cbo_Filter_ConeType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ConeType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ConeType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ConeType, cbo_Filter_Count, btn_Filter_Show, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")

    End Sub

    Private Sub cbo_Filter_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ConeType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ConeType, btn_Filter_Show, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_Count, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Count, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_IdNo = 0)")

    End Sub




    Private Sub txt_DelAddress1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelAddress1.KeyDown
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
        If e.KeyValue = 38 Then
            txt_DeliveryAddress.Focus()
        End If
    End Sub

    Private Sub txt_DelAddress1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelAddress1.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bls As Single = 0
        Dim Ent_Bags As String = ""
        Dim Ent_InWgt As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_Rate As Single = 0

        If Trim(cbo_EntType.Text) = "" Then
            MessageBox.Show("Invalid Type", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        If Trim(UCase(cbo_EntType.Text)) = "ORDER" Then

            With dgv_Selection

                lbl_Heading_Selection.Text = "ORDER SELECTION"

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Count_Name, d.Ledger_Name as agentname, d.Yarn_Comm_Bag , e.ConeType_Name,   h.Total_bags as Ent_Bags , h.Invoice_Weight as Ent_Invoice_Weight from Cotton_Order_Head a INNER JOIN Cotton_Order_details b ON a.Cotton_Order_Code = b.Cotton_Order_Code  LEFT OUTER JOIN Count_Head c ON b.Count_IdNo = c.Count_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN ConeType_Head e ON b.ConeType_Idno = e.ConeType_Idno LEFT OUTER JOIN Cotton_Invoice_Head h ON h.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and b.Cotton_Order_Code = h.Cotton_Order_Code and b.Cotton_Order_Details_Slno = h.Cotton_Order_Details_Slno Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " b.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Weight -  b.Invoice_Weight) > 0 or h.Invoice_Weight > 0 ) order by a.Cotton_Order_Date, a.for_orderby, a.Cotton_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Bags = ""
                        Ent_InWgt = 0

                        If IsDBNull(Dt1.Rows(i).Item("Ent_Bags").ToString) = False Then
                            Ent_Bags = Val(Dt1.Rows(i).Item("Ent_Bags").ToString)
                        End If

                        If IsDBNull(Dt1.Rows(i).Item("Ent_Invoice_Weight").ToString) = False Then
                            Ent_InWgt = Val(Dt1.Rows(i).Item("Ent_Invoice_Weight").ToString)
                        End If

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cotton_Order_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Count_Name").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("ConeType_Name").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Bags").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Invoice_Bag").ToString
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString) - Val(Dt1.Rows(i).Item("Invoice_Weight").ToString) + Val(Ent_InWgt), "#########0.00")
                        .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Rate").ToString)
                        .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Amount").ToString), "#########0.00")

                        If Ent_InWgt > 0 Then
                            .Rows(n).Cells(9).Value = "1"
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next

                        Else
                            .Rows(n).Cells(9).Value = ""

                        End If

                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cotton_Order_Code").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Cotton_Order_Details_Slno").ToString
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("agentname").ToString
                        '.Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Commision_Bag").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Freight").ToString
                        .Rows(n).Cells(15).Value = Val(Ent_Bags)
                        .Rows(n).Cells(16).Value = Ent_InWgt


                    Next
                End If

                Dt1.Clear()


            End With
        End If

        If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then


            With dgv_Selection

                lbl_Heading_Selection.Text = "DELIVERY SELECTION"

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*,  c.Count_Name, d.Ledger_Name as agentname, d.Yarn_Comm_Bag, e.ConeType_Name,t.ledger_name as Transportname from Cotton_Delivery_Head a  LEFT OUTER JOIN Count_Head c ON A.Count_IdNo = c.Count_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN ConeType_Head e ON A.ConeType_Idno = e.ConeType_Idno  LEFT OUTER JOIN Ledger_Head t ON a.Transport_IdNo = t.Ledger_IdNo    Where  A.Cotton_Invoice_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & "  order by a.Cotton_Delivery_Date, a.for_orderby, a.Cotton_Delivery_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)


                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cotton_Delivery_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Count_Name").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("ConeType_Name").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Total_Bags").ToString - Dt1.Rows(i).Item("Return_Bags").ToString
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString) - Dt1.Rows(i).Item("Return_Weight").ToString, "#########0.000")

                        .Rows(n).Cells(6).Value = "1"

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Cotton_Delivery_Code").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Cotton_Delivery_Date").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("agentname").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Vechile_No").ToString
                        .Rows(n).Cells(11).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Des_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Des_Time_Text").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Bale_Nos").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Delivery_Address").ToString
                        .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString
                        .Rows(n).Cells(16).Value = Format(Val(Dt1.Rows(i).Item("Total_Chippam").ToString), "#########0.00")
                        .Rows(n).Cells(17).Value = Format(Val(Dt1.Rows(i).Item("Yarn_Comm_Bag").ToString), "#########0.00")
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("TransportName").ToString
                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next
                End If

                Dt1.Clear()

                '  Da = New SqlClient.SqlDataAdapter("select a.*, c.Count_Name, d.Ledger_Name as agentname, e.ConeType_Name, d.Yarn_Comm_Bag   from Cotton_Delivery_Head a  LEFT OUTER JOIN Count_Head c ON A.Count_IdNo = c.Count_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN ConeType_Head e ON A.ConeType_Idno = e.ConeType_Idno    Where  A.Cotton_Invoice_Code =  '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & "  order by a.Cotton_Delivery_Date, a.for_orderby, a.Cotton_Delivery_No", con)
                Da = New SqlClient.SqlDataAdapter("select a.*, c.Count_Name, d.Ledger_Name as agentname, e.ConeType_Name, d.Yarn_Comm_Bag,t.ledger_name as Transportname   from Cotton_Delivery_Head a  LEFT OUTER JOIN Count_Head c ON A.Count_IdNo = c.Count_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN ConeType_Head e ON A.ConeType_Idno = e.ConeType_Idno  LEFT OUTER JOIN Ledger_Head t ON a.Transport_IdNo = t.Ledger_IdNo    Where  A.Cotton_Invoice_Code =  '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & "  order by a.Cotton_Delivery_Date, a.for_orderby, a.Cotton_Delivery_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cotton_Delivery_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Count_Name").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("ConeType_Name").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Total_Bags").ToString - Dt1.Rows(i).Item("Return_Bags").ToString
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString) - Dt1.Rows(i).Item("Return_Weight").ToString, "#########0.000")

                        .Rows(n).Cells(6).Value = ""

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Cotton_Delivery_Code").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Cotton_Delivery_Date").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("agentname").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Vechile_No").ToString
                        .Rows(n).Cells(11).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Des_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Des_Time_Text").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Bale_Nos").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Delivery_Address").ToString
                        .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString

                        .Rows(n).Cells(16).Value = Format(Val(Dt1.Rows(i).Item("Total_Chippam").ToString), "#########0.00")
                        .Rows(n).Cells(17).Value = Format(Val(Dt1.Rows(i).Item("Yarn_Comm_Bag").ToString), "#########0.00")
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Transportname").ToString
                    Next
                End If

                Dt1.Clear()


            End With
        End If

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        If dgv_Selection.Enabled And dgv_Selection.Visible Then
            dgv_Selection.Focus()
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If


    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer, j As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(6).Value = ""
                    For j = 0 To .Columns.Count - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Black
                    Next
                Next

                .Rows(RwIndx).Cells(6).Value = 1

                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

                Cotton_Invoice_Selection()

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
        Cotton_Invoice_Selection()

    End Sub

    Private Sub Cotton_Invoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim K As Integer = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable



        dgv_Details.Rows.Clear()
        SNo = 0
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then

                n = dgv_Details.Rows.Add()
                SNo = SNo + 1

                lbl_ReceiptCode.Text = dgv_Selection.Rows(i).Cells(7).Value

                txt_DcNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                cbo_CountName.Text = dgv_Selection.Rows(i).Cells(2).Value
                cbo_Conetype.Text = dgv_Selection.Rows(i).Cells(3).Value
                txt_InvoiceBag.Text = dgv_Selection.Rows(i).Cells(4).Value
                txt_InvWgt.Text = dgv_Selection.Rows(i).Cells(5).Value
                cbo_Agent.Text = dgv_Selection.Rows(i).Cells(9).Value
                cbo_Vechile.Text = dgv_Selection.Rows(i).Cells(10).Value
                dtp_DesDate.Text = dgv_Selection.Rows(i).Cells(11).Value
                txt_DesTime.Text = dgv_Selection.Rows(i).Cells(12).Value
                txt_TotalChippam.Text = dgv_Selection.Rows(i).Cells(16).Value
                txt_BaleNos.Text = dgv_Selection.Rows(i).Cells(13).Value
                txt_DeliveryAddress.Text = dgv_Selection.Rows(i).Cells(14).Value
                txt_DelAddress1.Text = dgv_Selection.Rows(i).Cells(15).Value
                txt_CommBag.Text = dgv_Selection.Rows(i).Cells(17).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(18).Value


                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Cotton_Delivery_Details a LEFT OUTER JOIN Cotton_Packing_Details B ON b.Cotton_invoice_Code = 'CNDEL-' + '" & Trim(dgv_Selection.Rows(i).Cells(7).Value) & "' and a.Bag_Code = b.Bag_Code Where a.Cotton_Delivery_Code = '" & Trim(dgv_Selection.Rows(i).Cells(7).Value) & "' and b.Cotton_Delivery_Return_Code = '' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For j = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(j).Item("Bag_No").ToString
                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(j).Item("Weight").ToString), "########0.000")
                            .Rows(n).Cells(3).Value = dt2.Rows(j).Item("Bag_Code").ToString
                            .Rows(n).Cells(4).Value = dt2.Rows(j).Item("Cotton_Packing_Code").ToString
                            .Rows(n).Cells(5).Value = dt2.Rows(j).Item("Cotton_Delivery_Details_SlNo").ToString
                            .Rows(n).Cells(6).Value = dt2.Rows(j).Item("Cotton_Invoice_Code").ToString
                            .Rows(n).Cells(7).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(j).Item("StockfROM_IdNo").ToString))


                        Next j

                    End If

                    ' If .RowCount = 0 Then .Rows.Add()

                End With

            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_YarnDescription.Enabled And cbo_YarnDescription.Visible Then cbo_YarnDescription.Focus()

    End Sub


    Private Sub btn_Pack_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Pack_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Cnt_IdNo As Integer
        Dim CnTy_IdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0, Ent_ShtMtrs As Single = 0

        Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        If Trim(UCase(cbo_EntType.Text)) = "PACKING" Then

            If Cnt_IdNo = 0 Then
                MessageBox.Show("Invalid Count Name", "DOES NOT SELECT PACKING SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
                Exit Sub
            End If

            CnTy_IdNo = Common_Procedures.ConeType_NameToIdNo(con, cbo_Conetype.Text)

            If CnTy_IdNo = 0 Then
                MessageBox.Show("Invalid ConeType Name", "DOES NOT SELECT PACKING SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Conetype.Enabled And cbo_Conetype.Visible Then cbo_Conetype.Focus()
                Exit Sub
            End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
            If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
                CompIDCondt = ""
            End If


            With dgv_packSelection

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details  A LEFT OUTER JOIN Cotton_Invoice_Details b ON a.Cotton_Packing_Code = b.Cotton_Packing_Code and a.Bag_Code = b.Bag_Code where a.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Conetype_Idno = " & Str(Val(CnTy_IdNo)) & " order by  a.sl_no,a.Cotton_Packing_Date, a.for_orderby, a.Cotton_Packing_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Bag_No").ToString
                        .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(i).Item("Net_Weight").ToString), "#########0.000")
                        .Rows(n).Cells(3).Value = "1"
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Bag_Code").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cotton_Packing_Code").ToString
                        .Rows(n).Cells(6).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("StockAt_IdNo").ToString))

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details a   where a.Cotton_Invoice_Code  = '' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Conetype_Idno = " & Str(Val(CnTy_IdNo)) & " order by  a.cotton_packing_code, a.sl_no, a.Cotton_packing_Date, a.for_orderby, a.Cotton_packing_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                'Da = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details a   where a.Cotton_Invoice_Code  = '' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Conetype_Idno = " & Str(Val(CnTy_IdNo)) & " order by a.Cotton_Packing_Date, a.for_orderby ,  a.Cotton_Packing_No ", con)
                'Dt1 = New DataTable
                'Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        '.Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Bag_No").ToString
                        .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(i).Item("Net_Weight").ToString), "#########0.000")
                        .Rows(n).Cells(3).Value = ""
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Bag_Code").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cotton_Packing_Code").ToString
                        .Rows(n).Cells(6).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("StockAt_IdNo").ToString))

                    Next

                End If
                Dt1.Clear()

            End With

        End If
        pnl_Pack_Selection.Visible = True
        pnl_Back.Enabled = False
        Txt_pack_no_of_bags.Focus()

    End Sub


    Private Sub dgv_Pack_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Select_PackPiece(e.RowIndex)
    End Sub



    Private Sub dgv_PackSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_packSelection.CurrentCell.RowIndex >= 0 Then

                n = dgv_packSelection.CurrentCell.RowIndex

                Select_PackPiece(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Pack_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Close_Pack_Selection()
    End Sub



    Private Sub dtp_DesDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_DesDate.LostFocus
        txt_DesTime.Text = Format(Now, "Short Time")
    End Sub

    Private Sub cbo_Grid_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Description_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Description_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Description_Count, cbo_SalesAc, cbo_Agent, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_EntType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntType, dtp_Date, cbo_PartyName, "", "", "", "")
    End Sub

    Private Sub cbo_EntType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntType, cbo_PartyName, "", "", "", "")
    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Description_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Description_Count, cbo_Agent, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Description_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Description_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_YarnDescription_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnDescription.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cotton_Invoice_Head", "Yarn_Details", "", "(Yarn_Details <> '')")
    End Sub

    Private Sub cbo_YarnDescription_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnDescription.KeyDown
        If e.KeyValue = 38 And cbo_YarnDescription.DroppedDown = False Then
            e.Handled = True
            cbo_Conetype.Focus()
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_YarnDescription.DroppedDown = False Then
            e.Handled = True
            cbo_SalesAc.Focus()
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_YarnDescription.DroppedDown = False Then
            cbo_YarnDescription.DroppedDown = True
        End If

    End Sub

    Private Sub cbo_YarnDescription_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_YarnDescription.KeyPress
        Dim Indx As Integer = -1
        Dim strFindStr As String = ""

        Try
            If Asc(e.KeyChar) = 8 Then
                If cbo_YarnDescription.SelectionStart <= 1 Then
                    cbo_YarnDescription.Text = ""
                    Exit Sub
                End If
                If cbo_YarnDescription.SelectionLength = 0 Then
                    strFindStr = cbo_YarnDescription.Text.Substring(0, cbo_YarnDescription.Text.Length - 1)
                Else
                    strFindStr = cbo_YarnDescription.Text.Substring(0, cbo_YarnDescription.SelectionStart - 1)
                End If

            Else

                If cbo_YarnDescription.SelectionLength = 0 Then
                    strFindStr = cbo_YarnDescription.Text & e.KeyChar
                Else
                    strFindStr = cbo_YarnDescription.Text.Substring(0, cbo_YarnDescription.SelectionStart) & e.KeyChar
                End If

            End If

            Indx = cbo_YarnDescription.FindString(strFindStr)

            If Indx <> -1 Then
                cbo_YarnDescription.SelectedText = ""
                cbo_YarnDescription.SelectedIndex = Indx
                cbo_YarnDescription.SelectionStart = strFindStr.Length
                cbo_YarnDescription.SelectionLength = cbo_YarnDescription.Text.Length
                e.Handled = True
            Else
                'e.Handled = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If Asc(e.KeyChar) = 13 Then
            cbo_SalesAc.Focus()
        End If

    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
        prn_Status = 1
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_InvoicePrefixNo.KeyDown
        On Error Resume Next
        '   If e.KeyValue = 38 Then txt_Packing.Focus()
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_EntType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntType.TextChanged

        If Trim(cbo_EntType.Text) = "PACKING" Then

            txt_DcNo.Enabled = True
            txt_DesTime.Enabled = True
            cbo_Conetype.Enabled = True
            cbo_Description_Count.Enabled = True
            cbo_Agent.Enabled = True
            cbo_Vechile.Enabled = True
            txt_TotalChippam.Enabled = True
            dtp_DesDate.Enabled = True
            txt_InvWgt.Enabled = False
            txt_InvoiceBag.Enabled = False
            txt_DeliveryAddress.Enabled = True
            txt_DelAddress1.Enabled = True
            txt_BaleNos.Enabled = True
            btn_Pack_Selection.Enabled = True
            btn_Selection.Enabled = False

        ElseIf Trim(cbo_EntType.Text) = "DIRECT" Then

            txt_DcNo.Enabled = True
            txt_DesTime.Enabled = True
            cbo_Conetype.Enabled = True
            cbo_Description_Count.Enabled = True
            cbo_Agent.Enabled = True
            txt_InvWgt.Enabled = True
            txt_InvoiceBag.Enabled = True
            cbo_Vechile.Enabled = True
            txt_TotalChippam.Enabled = True
            dtp_DesDate.Enabled = True
            txt_DeliveryAddress.Enabled = True
            txt_DelAddress1.Enabled = True
            txt_BaleNos.Enabled = True
            btn_Pack_Selection.Enabled = True
            btn_Selection.Enabled = True

        Else

            txt_DcNo.Enabled = False
            txt_DesTime.Enabled = False
            cbo_Conetype.Enabled = False
            cbo_Description_Count.Enabled = False
            cbo_Agent.Enabled = False
            cbo_Vechile.Enabled = False
            txt_TotalChippam.Enabled = False
            dtp_DesDate.Enabled = False
            txt_DeliveryAddress.Enabled = False
            txt_DelAddress1.Enabled = False
            txt_BaleNos.Enabled = True
            btn_Pack_Selection.Enabled = False
            btn_Selection.Enabled = True

        End If
    End Sub

    Private Sub Get_State_Code(ByVal Ledger_IDno As Integer, ByRef Ledger_State_Code As String, ByRef Company_State_Code As String)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            da = New SqlClient.SqlDataAdapter("Select * from Ledger_Head a LEFT OUTER JOIN State_Head b ON a.Ledger_State_IdNo = b.State_IdNo where a.Ledger_IdNo = " & Str(Val(Ledger_IDno)), con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("State_Code").ToString) = False Then
                    Ledger_State_Code = Trim(dt.Rows(0).Item("State_Code").ToString)
                End If

            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

            da = New SqlClient.SqlDataAdapter("Select * from Company_Head a LEFT OUTER JOIN State_Head b ON a.Company_State_IdNo = b.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)), con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("State_Code").ToString) = False Then
                    Company_State_Code = Trim(dt.Rows(0).Item("State_Code").ToString)
                End If
            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub
    Private Sub Cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_Agent, cbo_DeliveryTo, "", "", "", "")
    End Sub

    Private Sub Cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, cbo_DeliveryTo, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_PartyName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmDesc1 As String, ItmDesc2 As String
        Dim CntNm1 As String, CntNm2 As String
        Dim ps As Printing.PaperSize
        Dim vLine_Pen = New Pen(Color.Black, 2)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 30 '60
            .Right = 70
            .Top = 20 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 35 ' S.NO
        ClArr(2) = 70       'COUNT
        ClArr(3) = 180      'DESCRIPTION OF GOODS
        ClArr(4) = 80       'HSN CODE
        ClArr(5) = 50       'GST %
        ClArr(6) = 70       'NO.OF.BAG
        ClArr(7) = 70       'TOTAL WGT
        ClArr(8) = 80       'RATE/KG
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))  'AMOUNT


        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then


                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, vLine_Pen)

                Try


                    NoofDets = 0
                    DetIndx = 0

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


                            CntNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Count_Name_Description").ToString)
                            If Trim(CntNm1) = "" Then
                                CntNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Count_Name").ToString)
                            End If
                            CntNm2 = ""
                            If Len(CntNm1) > 8 Then
                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(CntNm1), I, 1) = " " Or Mid$(Trim(CntNm1), I, 1) = "," Or Mid$(Trim(CntNm1), I, 1) = "." Or Mid$(Trim(CntNm1), I, 1) = "-" Or Mid$(Trim(CntNm1), I, 1) = "/" Or Mid$(Trim(CntNm1), I, 1) = "_" Or Mid$(Trim(CntNm1), I, 1) = "(" Or Mid$(Trim(CntNm1), I, 1) = ")" Or Mid$(Trim(CntNm1), I, 1) = "\" Or Mid$(Trim(CntNm1), I, 1) = "[" Or Mid$(Trim(CntNm1), I, 1) = "]" Or Mid$(Trim(CntNm1), I, 1) = "{" Or Mid$(Trim(CntNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 8
                                CntNm2 = Microsoft.VisualBasic.Right(Trim(CntNm1), Len(CntNm1) - I)
                                CntNm1 = Microsoft.VisualBasic.Left(Trim(CntNm1), I - 1)
                            End If

                            ItmDesc1 = Trim(prn_DetDt.Rows(DetIndx).Item("Yarn_Details").ToString)
                            ItmDesc2 = ""
                            If Len(ItmDesc1) > 45 Then
                                For I = 45 To 1 Step -1
                                    If Mid$(Trim(ItmDesc1), I, 1) = " " Or Mid$(Trim(ItmDesc1), I, 1) = "," Or Mid$(Trim(ItmDesc1), I, 1) = "." Or Mid$(Trim(ItmDesc1), I, 1) = "-" Or Mid$(Trim(ItmDesc1), I, 1) = "/" Or Mid$(Trim(ItmDesc1), I, 1) = "_" Or Mid$(Trim(ItmDesc1), I, 1) = "(" Or Mid$(Trim(ItmDesc1), I, 1) = ")" Or Mid$(Trim(ItmDesc1), I, 1) = "\" Or Mid$(Trim(ItmDesc1), I, 1) = "[" Or Mid$(Trim(ItmDesc1), I, 1) = "]" Or Mid$(Trim(ItmDesc1), I, 1) = "{" Or Mid$(Trim(ItmDesc1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 45
                                ItmDesc2 = Microsoft.VisualBasic.Right(Trim(ItmDesc1), Len(ItmDesc1) - I)
                                ItmDesc1 = Microsoft.VisualBasic.Left(Trim(ItmDesc1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            NoofDets = NoofDets + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(NoofDets)), LMargin + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CntNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Invoice_Bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 20, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Invoice_Weight").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            ' Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString) & " x ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(CntNm2) <> "" Or Trim(ItmDesc2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    'Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets)
                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                vTot_BagNos = 0

                                e.HasMorePages = True
                                Return
                            End If

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

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByRef NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "TRANSPORT COPY"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                Else
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL FOR RECEIPIENT"
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
        End If

        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        '  End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

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

        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Then
        '    Dim imglft As Single = 0
        '    Dim imghgt As Single = 0
        '    imglft = (PageWidth - DirectCast(Global.OESpinning.My.Resources.Resources.ShivaMurugan_LOGO, Drawing.Image).Width) \ 2
        '    imghgt = DirectCast(Global.OESpinning.My.Resources.Resources.ShivaMurugan_LOGO, Drawing.Image).Width
        '    e.Graphics.DrawImage(DirectCast(Global.OESpinning.My.Resources.Resources.ShivaMurugan_LOGO, Drawing.Image), LMargin + 200, CurY - 5, 300, 27)
        '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'Else
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'End If



        CurY = CurY + strHeight - 7
        If Desc <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
        If PrintWidth > strWidth Then
            If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont)
            End If

            NoofItems_PerPage = NoofItems_PerPage - 1

        Else

            If Cmp_Add1 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            End If

        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30
            W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
            S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

            W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
            S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

            W3 = e.Graphics.MeasureString("INVOICE   DATE", pFont).Width
            S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width

            CurY = CurY + 10

            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Cotton_invoice_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_invoice_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("EWay_BIll_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ELECTRONIC REF.NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EWay_BIll_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            'LnAr(2) = CurY

            CurY1 = CurY
            CurY2 = CurY

            '---left side

            CurY1 = CurY1 + 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font)

            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY1 = CurY1 + strHeight
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, pFont)
            End If


            '--Right Side

            CurY2 = CurY2 + 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY2 = CurY2 + strHeight
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            CurY2 = CurY2 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, " PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont)
            ElseIf Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, " PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY2, 0, PrintWidth, pFont)
            End If


            CurY = IIf(CurY1 > CurY2, CurY1, CurY2)

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
            LnAr(3) = CurY



            W2 = e.Graphics.MeasureString("DOCUMENT THROUGH   : ", pFont).Width
            S2 = e.Graphics.MeasureString("DATE & TIME OF SUPPLY  :", pFont).Width

            '--Right Side
            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "AGENT NAME ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Dc_no").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("Des_Time_Text").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, FormatDateTime(prn_HdDt.Rows(0).Item("Des_Time_Text").ToString), LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
            End If

            'If Val(prn_HdDt.Rows(0).Item("Des_Date").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "DATE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, FormatDateTime(prn_HdDt.Rows(0).Item("Des_Date").ToString), LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
            '    ' Common_Procedures.Print_To_PrintDocument(e, FormatDateTime(prn_HdDt.Rows(0).Item("Des_Date").ToString) & " " & prn_HdDt.Rows(0).Item("Des_Time_Text").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Dim vPlaceOfSply As String = ""


            If Val(prn_HdDt.Rows(0).Item("PlaceOF_AreaIdNo").ToString) <> 0 Then
                vPlaceOfSply = Common_Procedures.Area_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("PlaceOF_AreaIdNo").ToString))
            ElseIf Val(prn_HdDt.Rows(0).Item("Ledger_AreaIdNo").ToString) <> 0 Then
                vPlaceOfSply = Common_Procedures.Area_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Ledger_AreaIdNo").ToString))
            Else
                If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                    vPlaceOfSply = prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString
                Else
                    vPlaceOfSply = prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString
                End If

            End If

            Common_Procedures.Print_To_PrintDocument(e, vPlaceOfSply, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(3))
            LnAr(4) = CurY

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim CurY1 As Single = 0
        Dim BmsInWrds As String = ""
        Dim W2 As Single
        Dim vLine_Pen = New Pen(Color.Black, 2)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            CurY = CurY + TxtHgt - 10


            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(6), LMargin, LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1), LnAr(6), LMargin + ClArr(1), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2), LnAr(6), LMargin + ClArr(1) + ClArr(2), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(4))

            'e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(4))

            'p1Font = New Font("Calibri", 14, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
            ' ''  End If
            'If Trim(prn_OriDupTri) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            'End If

            W2 = e.Graphics.MeasureString("BAG NO'S : ", pFont).Width

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

            ' Common_Procedures.Print_To_PrintDocument(e, "Bag/Chippam No : " & (prn_HdDt.Rows(0).Item("Bale_Nos").ToString), LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt + 10

            'CurY1 = CurY
            'Left Side
            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery Address : ", LMargin + 10, CurY1, 0, 0, pFont)

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString), LMargin + 10, CurY1, 0, 0, pFont)

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString), LMargin + 10, CurY1, 0, 0, pFont)

            'CurY1 = CurY1 + TxtHgt
            'p1Font = New Font("Calibri", 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p1Font)
            'CurY1 = CurY1 + TxtHgt
            'p1Font = New Font("Calibri", 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p1Font)
            'CurY1 = CurY1 + TxtHgt
            'p1Font = New Font("Calibri", 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p1Font)
            'CurY1 = CurY1 + TxtHgt
            'p1Font = New Font("Calibri", 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p1Font)
            'CurY1 = CurY1 + TxtHgt
            'CurY1 = CurY1 + TxtHgt
            'CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            '   Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY1, 0, 0, p1Font)
            'Right Side
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    'CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            Common_Procedures.Print_To_PrintDocument(e, "Bag No's : " & prn_HdDt.Rows(0).Item("Bale_Nos").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    'CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Addless_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                    'CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & prn_HdDt.Rows(0).Item("Discount_Percentage").ToString & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            '-------------------------------------------------------------------

            prn_CGST_Amount = prn_HdDt.Rows(0).Item("CGst_Amount").ToString
            prn_SGST_Amount = prn_HdDt.Rows(0).Item("SGst_Amount").ToString
            prn_IGST_Amount = prn_HdDt.Rows(0).Item("IGst_Amount").ToString

            prn_GST_Perc = prn_HdDt.Rows(0).Item("GST_Percentage").ToString ' Val(Common_Procedures.get_FieldValue(con, "Count_Head", "GST_Percentege", "Count_Name = '" & Trim(cbo_CountName.Text) & "'"))

            If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 16
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Taxable_Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, p1Font)
                End If
            End If

            If Val(prn_CGST_Amount) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If prn_CGST_Amount <> 0 Then
                        'Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + -10, CurY, 1, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_SGST_Amount) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If prn_SGST_Amount <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_IGST_Amount) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If prn_IGST_Amount <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            '***** GST END *****


            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("TCS_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("TCs_name_caption").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            '----------------------------------------------------------------------


            CurY = CurY + TxtHgt

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, " Round Off", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            ''e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, PageWidth, CurY)

            ' e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY + 10, PageWidth, CurY + 10)
            'CurY = CurY + TxtHgt + 15
            CurY = CurY + TxtHgt


            If CurY1 > CurY Then CurY = CurY1


            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            'CurY = CurY + TxtHgt - 10
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, PageWidth, CurY)
            CurY = CurY + 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 20, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p1Font)

            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(6))

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
            CurY = CurY + 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount In Words  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 7
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            CurY = CurY + 10
            BmsInWrds = ""
            If (Val(prn_CGST_Amount) + Val(prn_SGST_Amount) + Val(prn_IGST_Amount)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_CGST_Amount) + Val(prn_SGST_Amount) + Val(prn_IGST_Amount))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)


            Common_Procedures.Print_To_PrintDocument(e, "Bank Details : " & BankNm1 & ", " & BankNm2 & ", " & BankNm3 & ", " & BankNm4, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 5


            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt + 3
            Common_Procedures.Print_To_PrintDocument(e, "We are  responsible for yarn in yarn shape only not in fabric stage", LMargin + 25, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases when goods leave our permission", LMargin + 25, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "All Payment should be made by A\c Payee Cheque or Draft", LMargin + 25, CurY, 0, 0, pFont)
            Dim vJurisdctn As String = ""
            vJurisdctn = Trim(Common_Procedures.settings.Jurisdiction)
            If Trim(vJurisdctn) = "" Then vJurisdctn = "Tirupur"
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(vJurisdctn) & " jurisdiction ", LMargin + 25, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Interest at value of 24% will be charge from the due date", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            'CurY = CurY + TxtHgt - 5
            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            CurY = CurY + 2
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 7, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt + 4
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(vLine_Pen, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim vLine_Pen = New Pen(Color.Black, 2)
        Dim vFontName As String = ""
        Dim vOUTERBORDERLine_Pen As Pen
        Dim vMorePages_STS As Boolean


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40 ' 30 '60
            .Right = 70
            .Top = 40 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        vFontName = "Arial"

        pFont = New Font(vFontName, 9, FontStyle.Regular)
        vOUTERBORDERLine_Pen = New Pen(Color.Black, 1)
        vLine_Pen = New Pen(Color.Black, 1)

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

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 4 ' 12

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35) ' S.NO
        ClArr(2) = 75       'COUNT
        ClArr(3) = 190 '180      'DESCRIPTION OF GOODS
        ClArr(4) = 70       'HSN CODE
        ClArr(5) = 55       'GST %
        ClArr(6) = 60 '70       'NO.OF.BAG
        ClArr(7) = 70       'TOTAL WGT
        ClArr(8) = 80       'RATE/KG
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))  'AMOUNT


        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, vLine_Pen, vFontName, vOUTERBORDERLine_Pen)

                Try


                    NoofDets = 0
                    DetIndx = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False, vFontName, vMorePages_STS, vOUTERBORDERLine_Pen)

                                e.HasMorePages = True
                                Return

                            End If

                            ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Yarn_Details").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 45 Then
                                For I = 30 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 45
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)


                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Invoice_Bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 20, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Invoice_Weight").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            ' Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString) & " x ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, "Lot No : " & Trim(prn_HdDt.Rows(0).Item("Lot_No").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY + 15, 0, 0, pFont)

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    'Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets)
                    Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True, vFontName, vMorePages_STS, vOUTERBORDERLine_Pen)


                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                e.HasMorePages = True
                                Return
                            End If

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

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByRef NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen, ByVal vFontName As String, ByVal vOUTERBORDERLine_Pen As Pen)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String, Cmp_UAM_No As String, Cmp_UAM_CAP As String

        Dim S As String
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""

        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim I As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "TRANSPORT COPY"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
                    ElseIf Val(S) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                Else
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL FOR RECEIPIENT"
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
        End If

        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        p1Font = New Font(vFontName, 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        '  End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

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

        If Trim(prn_HdDt.Rows(0).Item("Company_UAM_No").ToString) <> "" Then
            Cmp_UAM_CAP = UCase("Udyam : ")
            Cmp_UAM_No = prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1088-" Then '---- Kalaimagal OE (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)

        Else

            If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                '.BackgroundImage = Image.FromStream(ms)


                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 10, 100, 100)
                                '--    e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 5, 100, 100)

                                'e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 20, CurY + 5, 110, 100)
                                'e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)

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

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 110, CurY + 5, 100, 100)

                        End If

                    End Using
                End If
            End If

        End If

        CurY = CurY + 5
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height




        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            p1Font = New Font("Americana std", 20, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, StrConv(Cmp_Name, VbStrConv.ProperCase), LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            p1Font = New Font(vFontName, 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If


        p1Font = New Font(vFontName, 18, FontStyle.Bold)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 5
        If Desc <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
        If PrintWidth > strWidth Then
            If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont)
            End If

            NoofItems_PerPage = NoofItems_PerPage - 1

        Else

            If Cmp_Add1 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            End If

        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt

        p1Font = New Font(vFontName, 10, FontStyle.Regular)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), pFont).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font(vFontName, 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, pFont)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, pFont).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font(vFontName, 10, FontStyle.Regular)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, pFont)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, pFont).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font(vFontName, 10, FontStyle.Regular)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, pFont)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, pFont).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)


        If Trim(UCase(Cmp_UAM_No)) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_UAM_CAP & Cmp_UAM_No, LMargin, CurY - 5, 2, PrintWidth, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

        End If


        'If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    p1Font = New Font(vFontName, 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "IRN : " & prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString, LMargin, CurY, 2, PrintWidth, p1Font)
        'End If




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
            p1Font = New Font(vFontName, 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
            End If


        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) '+ ClAr(5) - 30
            W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
            S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

            W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
            S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

            W3 = e.Graphics.MeasureString("INVOICE   DATE", pFont).Width
            S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width

            CurY = CurY + 10
            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (OE) (palladam)
                Inv_No = prn_HdDt.Rows(0).Item("Cotton_invoice_No").ToString
                InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

                If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & Trim(Format(Val(Inv_No), "######0000")) & Trim(InvSubNo), LMargin + W3 + 30, CurY, 0, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######0000")) & Trim(InvSubNo), LMargin + W3 + 30, CurY, 0, 0, p1Font)
                End If

            Else
                If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "_" & prn_HdDt.Rows(0).Item("Cotton_invoice_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_invoice_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
                End If

            End If




            Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("EWay_BIll_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL.NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EWay_BIll_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            'LnAr(2) = CurY

            CurY1 = CurY
            CurY2 = CurY

            '---left side

            CurY1 = CurY1 + 10
            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "BILLED TO :", LMargin + 10, CurY1, 0, 0, p1Font)

            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY1 = CurY1 + strHeight
            p1Font = New Font(vFontName, 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, pFont)
            End If


            '--Right Side

            CurY2 = CurY2 + 10
            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "SHIPPED TO :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY2 = CurY2 + strHeight
            p1Font = New Font(vFontName, 9, FontStyle.Bold)
            If prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)
            End If

            If prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            ElseIf prn_HdDt.Rows(0).Item("Ledger_Address4").ToString <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            'CurY2 = CurY2 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, " PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont)
            End If



            CurY = IIf(CurY1 > CurY2, CurY1, CurY2)


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
            LnAr(3) = CurY



            W2 = e.Graphics.MeasureString("DOCUMENT THROUGH   : ", pFont).Width
            S2 = e.Graphics.MeasureString("DATE & TIME OF SUPPLY  :", pFont).Width

            '--Right Side
            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "AGENT NAME ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 - 20, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 - 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 - 30, CurY, 0, 0, pFont)
            If prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 - 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + C1 + S2 - 20, CurY, 0, 0, pFont)

            End If



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(3))
            LnAr(4) = CurY


            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 15, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 15, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'CurY = CurY + 10
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            'End If
            'p1Font = New Font(vFontName, 8, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vFontName As String, ByRef vIS_HasMorePages As Boolean, ByVal vOUTERBORDERLine_Pen As Pen)
        Dim p1Font As Font
        Dim p2Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim CurY1 As Single = 0
        Dim BmsInWrds As String = ""
        Dim vLine_Pen = New Pen(Color.Black, 1)
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            CurY = CurY + TxtHgt - 10


            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(6), LMargin, LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1), LnAr(6), LMargin + ClArr(1), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2), LnAr(6), LMargin + ClArr(1) + ClArr(2), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(4))

            'e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(4))

            'p1Font = New Font(vFontName, 14, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
            ' ''  End If
            'If Trim(prn_OriDupTri) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

            'End If

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
            If (prn_HdDt.Rows(0).Item("Bale_Nos").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Bag Nos : " & (prn_HdDt.Rows(0).Item("Bale_Nos").ToString), LMargin + 10, CurY, 0, 0, pFont)

            End If

            'CurY = CurY + TxtHgt + 10

            CurY1 = CurY
            'Left Side


            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery Address : ", LMargin + 10, CurY1, 0, 0, pFont)

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString), LMargin + 10, CurY1, 0, 0, pFont)

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString), LMargin + 10, CurY1, 0, 0, pFont)


            'CurY1 = CurY1 + TxtHgt
            'p1Font = New Font(vFontName, 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p1Font)
            'CurY1 = CurY1 + TxtHgt
            'p1Font = New Font(vFontName, 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p1Font)
            'CurY1 = CurY1 + TxtHgt
            'p1Font = New Font(vFontName, 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p1Font)
            'CurY1 = CurY1 + TxtHgt
            'p1Font = New Font(vFontName, 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt
            CurY1 = CurY1 + TxtHgt
            CurY1 = CurY1 + TxtHgt
            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            '   Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY1, 0, 0, p1Font)
            'Right Side

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Addless_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & prn_HdDt.Rows(0).Item("Discount_Percentage").ToString & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            '-------------------------------------------------------------------

            prn_CGST_Amount = prn_HdDt.Rows(0).Item("CGst_Amount").ToString
            prn_SGST_Amount = prn_HdDt.Rows(0).Item("SGst_Amount").ToString
            prn_IGST_Amount = prn_HdDt.Rows(0).Item("IGst_Amount").ToString

            prn_GST_Perc = Val(Common_Procedures.get_FieldValue(con, "Count_Head", "GST_Percentege", "Count_Name = '" & Trim(cbo_CountName.Text) & "'"))

            If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font(vFontName, 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Taxable_Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, p1Font)
                End If
            End If

            If Val(prn_CGST_Amount) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If prn_CGST_Amount <> 0 Then
                        'Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + -10, CurY, 1, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_SGST_Amount) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If prn_SGST_Amount <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_IGST_Amount) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If prn_IGST_Amount <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            '***** GST END *****



            '----------------------------------------------------------------------

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("TCS_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("TCs_name_caption").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, " Round Off", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            ''e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, PageWidth, CurY)

            ' e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY + 10, PageWidth, CurY + 10)
            'CurY = CurY + TxtHgt + 15
            CurY = CurY + TxtHgt


            If CurY1 > CurY Then CurY = CurY1


            'p1Font = New Font(vFontName, 13, FontStyle.Bold)
            'CurY = CurY + TxtHgt - 10
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, PageWidth, CurY)
            CurY = CurY + 10
            p1Font = New Font(vFontName, 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 20, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font(vFontName, 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p1Font)

            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(6))
            ' e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(5))

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
            CurY = CurY + 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            p1Font = New Font(vFontName, 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            CurY = CurY + 5
            BmsInWrds = ""
            If (Val(prn_CGST_Amount) + Val(prn_SGST_Amount) + Val(prn_IGST_Amount)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_CGST_Amount) + Val(prn_SGST_Amount) + Val(prn_IGST_Amount))
            End If

            p1Font = New Font(vFontName, 9.5, FontStyle.Bold)

            'Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Bank Details : " & BankNm1 & ", " & BankNm2 & ", " & BankNm3 & ", " & BankNm4, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            p1Font = New Font(vFontName, 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "GOODS CLEARED UNDER EXEMPTION NOTIFICATION NO 30/2004 DT 09.07.2004 ", LMargin, CurY, 2, PageWidth, pFont)




            CurY = CurY + TxtHgt - 5

            '******************************************

            p1Font = New Font(vFontName, 9, FontStyle.Underline)
            p2Font = New Font("Webdings", 7, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 3
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 5, CurY + 3, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We are  responsible for yarn in yarn shape only not in fabric stage", LMargin + 15, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 5, CurY + 3, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases when goods leave our permission", LMargin + 15, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 15, CurY + 3, 2, PrintWidth, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Interest at value of 24% will be charge from the due date", LMargin + 190, CurY, 2, PrintWidth, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 5, CurY + 3, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "All Payment should be made by A\c Payee Cheque or Draft", LMargin + 15, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 15, CurY + 3, 2, PrintWidth, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 185, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 5, CurY + 3, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 15, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 15, CurY + 3, 2, PrintWidth, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 110, CurY, 2, PrintWidth, pFont)


            '*********************************************

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            'CurY = CurY + TxtHgt - 5
            'p1Font = New Font(vFontName, 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            CurY = CurY + 5
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font(vFontName, 7, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            p1Font = New Font(vFontName, 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(vLine_Pen, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_idno = 0)")
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_Agent, cbo_BagKg, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_idno = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_Agent, cbo_BagKg, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_BagKg, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_idno = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_BagKg, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation
            'Dim f As New Delivery_Party_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim J As Integer = 0

        With dgv_packSelection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(3).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_PackPiece(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub


    Private Sub chk_SelectAll_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer = 0
        Dim J As Integer = 0

        With dgv_packSelection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(3).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_PackPiece(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With
    End Sub

    Private Sub dgv_packSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_packSelection.CellClick
        Select_PackPiece(e.RowIndex)
    End Sub

    Private Sub Select_PackPiece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_packSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(3).Value = (Val(.Rows(RwIndx).Cells(3).Value) + 1) Mod 2
                If Val(.Rows(RwIndx).Cells(3).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(3).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If


            End If

        End With

    End Sub



    Private Sub dgv_packSelection_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_packSelection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_packSelection.CurrentCell.RowIndex >= 0 Then

                n = dgv_packSelection.CurrentCell.RowIndex

                Select_PackPiece(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Pack_Close_Selection_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Pack_Close_Selection.Click
        Close_Pack_Selection()
    End Sub

    Private Sub Close_Pack_Selection()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        Dim BagNo As String
        Dim vFor_OrdBy_BagNo As String = ""
        Dim FsNo As Single, LsNo As Single
        Dim FsBagNo As String, LsBagNo As String


        Cmd.Connection = con


        With dgv_Details



            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()


            pnl_Back.Enabled = True

            dgv_Details.Rows.Clear()

            BagNo = ""

            For i = 0 To dgv_packSelection.RowCount - 1

                If Val(dgv_packSelection.Rows(i).Cells(3).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(sno)
                    .Rows(n).Cells(1).Value = dgv_packSelection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_packSelection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(3).Value = dgv_packSelection.Rows(i).Cells(4).Value
                    .Rows(n).Cells(4).Value = dgv_packSelection.Rows(i).Cells(5).Value
                    .Rows(n).Cells(7).Value = dgv_packSelection.Rows(i).Cells(6).Value

                    vFor_OrdBy_BagNo = Str(Val(dgv_packSelection.Rows(i).Cells(1).Value))

                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(dgv_packSelection.Rows(i).Cells(1).Value) & "', " & Str(Val(vFor_OrdBy_BagNo)) & " ) "
                    Cmd.ExecuteNonQuery()

                End If
                '  Total_Calculation()
            Next

            BagNo = ""
            FsNo = 0 : LsNo = 0
            FsBagNo = "" : LsBagNo = ""


            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bag_no, Meters1 as fororderby_bagno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name1", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                FsNo = Val(Dt1.Rows(0).Item("fororderby_bagno").ToString)
                LsNo = Val(Dt1.Rows(0).Item("fororderby_bagno").ToString)

                FsBagNo = Trim(UCase(Dt1.Rows(0).Item("Bag_no").ToString))
                LsBagNo = Trim(UCase(Dt1.Rows(0).Item("Bag_no").ToString))


                For i = 1 To Dt1.Rows.Count - 1

                    If LsNo + 1 = Val(Dt1.Rows(i).Item("fororderby_bagno").ToString) Then
                        LsNo = Val(Dt1.Rows(i).Item("fororderby_bagno").ToString)
                        LsBagNo = Trim(UCase(Dt1.Rows(i).Item("Bag_no").ToString))

                    Else
                        If FsNo = LsNo Then
                            BagNo = BagNo & Trim(FsBagNo) & ","
                        Else
                            BagNo = BagNo & Trim(FsBagNo) & "-" & Trim(LsBagNo) & ","

                        End If

                        FsNo = Dt1.Rows(i).Item("fororderby_bagno").ToString
                        LsNo = Dt1.Rows(i).Item("fororderby_bagno").ToString

                        FsBagNo = Trim(UCase(Dt1.Rows(i).Item("Bag_no").ToString))
                        LsBagNo = Trim(UCase(Dt1.Rows(i).Item("Bag_no").ToString))

                    End If

                Next

                If FsNo = LsNo Then
                    BagNo = BagNo & Trim(FsBagNo)
                Else
                    BagNo = BagNo & Trim(FsBagNo) & "-" & Trim(LsBagNo)
                End If

            End If

            Dt1.Clear()
            txt_BaleNos.Text = BagNo

            Total_Calculation()

        End With

        pnl_Back.Enabled = True
        pnl_Pack_Selection.Visible = False

        If cbo_SalesAc.Visible And cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
    End Sub


    Private Sub btn_pack_select_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_pack_select.Click
        If Trim(Txt_pack_no_of_bags.Text) <> "" Then
            pack_selection_noOfBags()
        Else
            txt_BagNoSelection.Focus()
        End If

        Dim StNo As String = ""
        Dim BgNo As String = ""
        Dim k As Integer = 0

        If Trim(txt_BagNoSelection.Text) <> "" Then

            BgNo = Trim(txt_BagNoSelection.Text)

            For k = 0 To dgv_packSelection.Rows.Count - 1
                If Trim(UCase(BgNo)) = Trim(UCase(dgv_packSelection.Rows(k).Cells(1).Value)) Then
                    Call Select_Bag(k)

                    dgv_packSelection.CurrentCell = dgv_packSelection.Rows(k).Cells(0)
                    If k >= 11 Then dgv_packSelection.FirstDisplayedScrollingRowIndex = k - 10

                    Exit For

                End If
            Next

            txt_BagNoSelection.Text = ""
            If txt_BagNoSelection.Enabled = True Then txt_BagNoSelection.Focus()

        End If
    End Sub


    Private Sub Txt_pack_no_of_bags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Txt_pack_no_of_bags.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(Txt_pack_no_of_bags.Text) <> "" Then
                pack_selection_noOfBags()
            Else
                txt_BagNoSelection.Focus()
            End If


            Dim StNo As String = ""
            Dim BgNo As String = ""
            Dim k As Integer = 0

            If Trim(txt_BagNoSelection.Text) <> "" Then

                BgNo = Trim(txt_BagNoSelection.Text)

                For k = 0 To dgv_packSelection.Rows.Count - 1
                    If Trim(UCase(BgNo)) = Trim(UCase(dgv_packSelection.Rows(k).Cells(1).Value)) Then
                        Call Select_Bag(k)

                        dgv_packSelection.CurrentCell = dgv_packSelection.Rows(k).Cells(0)
                        If k >= 11 Then dgv_packSelection.FirstDisplayedScrollingRowIndex = k - 10

                        Exit For

                    End If
                Next

                txt_BagNoSelection.Text = ""
                If txt_BagNoSelection.Enabled = True Then txt_BagNoSelection.Focus()

            End If

        End If

    End Sub

    Private Sub pack_selection_noOfBags()
        Dim i As Integer = 0
        Dim J As Integer = 0

        With dgv_packSelection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(3).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If Val(Txt_pack_no_of_bags.Text) > 0 Then
                For i = 0 To Val(Txt_pack_no_of_bags.Text) - 1
                    Select_PackPiece(i)
                Next i
            End If

        End With
    End Sub


    'Private Sub get_agent_comm_bag()
    '    Dim da As SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim agnIdNo As Integer = 0
    '    Dim NewCode As String

    '    'If Trim(UCase(cbo_Agent.Tag)) <> Trim(UCase(cbo_Agent.Text)) And Trim(UCase(cbo_Agent.Text)) <> Trim(UCase("System.Data.DataRowView")) Then

    '    cbo_Agent.Tag = cbo_Agent.Text

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    txt_CommBag.Text = ""


    '    agnIdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_Agent.Text)
    '    If agnIdNo <> 0 Then

    '        da = New SqlClient.SqlDataAdapter("select DISTINCT b.Ledger_Name as agentname, b.Yarn_Comm_Bag from Cotton_Invoice_Head a LEFT OUTER JOIN ledger_head b ON a.Agent_IdNo = b.Ledger_IdNo where  b.ledger_Name = '" & Trim(cbo_Agent.Text) & "'", con)
    '        dt = New DataTable
    '        da.Fill(dt)
    '        If dt.Rows.Count > 0 Then
    '            If IsDBNull(dt.Rows(0)("Yarn_Comm_Bag").ToString) = False Then
    '                txt_CommBag.Text = dt.Rows(0)("Yarn_Comm_Bag").ToString
    '            End If
    '        End If
    '        dt.Dispose()
    '        da.Dispose()



    '    End If

    '    'End If

    'End Sub



    Private Sub txt_BagNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BagNoSelection.KeyDown
        If e.KeyValue = 38 Then
            Txt_pack_no_of_bags.Focus()
        End If

        If e.KeyValue = 40 Then
            If dgv_packSelection.Rows.Count > 0 Then
                dgv_packSelection.Focus()
                dgv_packSelection.CurrentCell = dgv_packSelection.Rows(0).Cells(0)
                dgv_packSelection.CurrentCell.Selected = True
            End If

        End If
    End Sub

    Private Sub txt_BagNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BagNoSelection.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If Trim(txt_BagNoSelection.Text) <> "" Then
                btn_pack_select_Click(sender, e)
            Else
                If dgv_packSelection.Rows.Count > 0 Then
                    dgv_packSelection.Focus()
                    dgv_packSelection.CurrentCell = dgv_packSelection.Rows(0).Cells(0)
                    dgv_packSelection.CurrentCell.Selected = True
                End If

            End If

        End If

    End Sub

    Private Sub Select_Bag(ByVal RwIndx As Integer)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0
        Dim MxId As Integer = 0

        Try

            With dgv_packSelection

                Cmd.Connection = con

                If .RowCount > 0 And RwIndx >= 0 Then

                    If Val(.Rows(RwIndx).Cells(7).Value) > 0 And Val(.Rows(RwIndx).Cells(8).Value) <> Val(.Rows(RwIndx).Cells(9).Value) Then
                        MessageBox.Show("Cannot deselect" & Chr(13) & "Already this bags delivered to others")
                        Exit Sub
                    End If

                    .Rows(RwIndx).Cells(3).Value = (Val(.Rows(RwIndx).Cells(3).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(3).Value) = 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

                        MxId = Common_Procedures.get_MaxIdNo(con, "" & Trim(Common_Procedures.EntryTempSubTable) & "", "Int1", "")

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Name1, Name2, Name3) Values (" & Str(Val(MxId)) & ", '" & Trim(.Rows(RwIndx).Cells(8).Value) & "', '" & Trim(.Rows(RwIndx).Cells(1).Value) & "', " & Str(Val(.Rows(RwIndx).Cells(3).Value)) & " ) "
                        Cmd.ExecuteNonQuery()

                    Else

                        .Rows(RwIndx).Cells(3).Value = ""
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                        Next

                        Cmd.CommandText = "Delete from " & Trim(Common_Procedures.EntryTempSubTable) & " where Name1 = '" & Trim(.Rows(RwIndx).Cells(8).Value) & "' and Name2 = '" & Trim(.Rows(RwIndx).Cells(3).Value) & "'"
                        Cmd.ExecuteNonQuery()

                    End If

                End If

            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Txt_pack_no_of_bags_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_pack_no_of_bags.TextChanged
        pack_selection_noOfBags()
    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmDesc1 As String, ItmDesc2 As String
        Dim CntNm1 As String, CntNm2 As String
        Dim ps As Printing.PaperSize
        Dim vLine_Pen = New Pen(Color.Black, 2)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 30 '60
            .Right = 70
            .Top = 30 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        'TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 35 ' S.NO
        ClArr(2) = 70       'COUNT
        ClArr(3) = 180      'DESCRIPTION OF GOODS
        ClArr(4) = 80       'HSN CODE
        ClArr(5) = 50       'GST %
        ClArr(6) = 70       'NO.OF.BAG
        ClArr(7) = 70       'TOTAL WGT
        ClArr(8) = 80       'RATE/KG
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))  'AMOUNT


        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then


                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, vLine_Pen)

                Try


                    NoofDets = 0
                    DetIndx = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            CntNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Count_Name_Description").ToString)
                            If Trim(CntNm1) = "" Then
                                CntNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Count_Name").ToString)
                            End If
                            CntNm2 = ""
                            If Len(CntNm1) > 8 Then
                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(CntNm1), I, 1) = " " Or Mid$(Trim(CntNm1), I, 1) = "," Or Mid$(Trim(CntNm1), I, 1) = "." Or Mid$(Trim(CntNm1), I, 1) = "-" Or Mid$(Trim(CntNm1), I, 1) = "/" Or Mid$(Trim(CntNm1), I, 1) = "_" Or Mid$(Trim(CntNm1), I, 1) = "(" Or Mid$(Trim(CntNm1), I, 1) = ")" Or Mid$(Trim(CntNm1), I, 1) = "\" Or Mid$(Trim(CntNm1), I, 1) = "[" Or Mid$(Trim(CntNm1), I, 1) = "]" Or Mid$(Trim(CntNm1), I, 1) = "{" Or Mid$(Trim(CntNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 8
                                CntNm2 = Microsoft.VisualBasic.Right(Trim(CntNm1), Len(CntNm1) - I)
                                CntNm1 = Microsoft.VisualBasic.Left(Trim(CntNm1), I - 1)
                            End If

                            ItmDesc1 = Trim(prn_DetDt.Rows(DetIndx).Item("Yarn_Details").ToString)
                            ItmDesc2 = ""
                            If Len(ItmDesc1) > 22 Then
                                For I = 22 To 1 Step -1
                                    If Mid$(Trim(ItmDesc1), I, 1) = " " Or Mid$(Trim(ItmDesc1), I, 1) = "," Or Mid$(Trim(ItmDesc1), I, 1) = "." Or Mid$(Trim(ItmDesc1), I, 1) = "-" Or Mid$(Trim(ItmDesc1), I, 1) = "/" Or Mid$(Trim(ItmDesc1), I, 1) = "_" Or Mid$(Trim(ItmDesc1), I, 1) = "(" Or Mid$(Trim(ItmDesc1), I, 1) = ")" Or Mid$(Trim(ItmDesc1), I, 1) = "\" Or Mid$(Trim(ItmDesc1), I, 1) = "[" Or Mid$(Trim(ItmDesc1), I, 1) = "]" Or Mid$(Trim(ItmDesc1), I, 1) = "{" Or Mid$(Trim(ItmDesc1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 22
                                ItmDesc2 = Microsoft.VisualBasic.Right(Trim(ItmDesc1), Len(ItmDesc1) - I)
                                ItmDesc1 = Microsoft.VisualBasic.Left(Trim(ItmDesc1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            NoofDets = NoofDets + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(NoofDets)), LMargin + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CntNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Invoice_Bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 20, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Invoice_Weight").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            ' Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString) & " x ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(CntNm2) <> "" Or Trim(ItmDesc2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    'Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets)
                    Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                vTot_BagNos = 0

                                e.HasMorePages = True
                                Return
                            End If

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

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByRef NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""


        PageNo = PageNo + 1
        CurY = TMargin

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "TRANSPORT COPY"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                Else
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL FOR RECEIPIENT"
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
        End If

        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        '  End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

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

        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        p1Font = New Font("Calibri", 20, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Then
        '    Dim imglft As Single = 0
        '    Dim imghgt As Single = 0
        '    imglft = (PageWidth - DirectCast(Global.OESpinning.My.Resources.Resources.ShivaMurugan_LOGO, Drawing.Image).Width) \ 2
        '    imghgt = DirectCast(Global.OESpinning.My.Resources.Resources.ShivaMurugan_LOGO, Drawing.Image).Width
        '    e.Graphics.DrawImage(DirectCast(Global.OESpinning.My.Resources.Resources.ShivaMurugan_LOGO, Drawing.Image), LMargin + 200, CurY - 5, 300, 27)
        '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'Else
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'End If



        CurY = CurY + strHeight - 7
        If Desc <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
        If PrintWidth > strWidth Then
            If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont)
            End If

            NoofItems_PerPage = NoofItems_PerPage - 1

        Else

            If Cmp_Add1 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            End If

        End If

        CurY = CurY + TxtHgt
        If Trim(Cmp_PhNo) <> "" And Trim(Cmp_EMail) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)
        ElseIf Trim(Cmp_PhNo) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo), LMargin, CurY, 2, PrintWidth, pFont)
        ElseIf Trim(Cmp_EMail) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)
        End If

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, p1Font)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30
            W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
            S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

            W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
            S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

            W3 = e.Graphics.MeasureString("INVOICE   DATE", pFont).Width
            S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width

            CurY = CurY + 10

            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)

            Inv_No = prn_HdDt.Rows(0).Item("Cotton_invoice_No").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1302" Then '---- RAJA MANGAY COTTON MILLS (PALLADAM)    (OR)   RAJAMANGAY 
                If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & Trim(Format(Val(Inv_No), "######000")) & "/" & Trim(Common_Procedures.FnYearCode), LMargin + W3 + 30, CurY, 0, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & "/" & Trim(Common_Procedures.FnYearCode), LMargin + W3 + 30, CurY, 0, 0, p1Font)
                End If
            ElseIf prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & Trim(Format(Val(Inv_No), "######000")), LMargin + W3 + 30, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Inv_No, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, pFont)

            'If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL No.", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EWay_BIll_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            'LnAr(2) = CurY

            CurY1 = CurY
            CurY2 = CurY

            '---left side

            CurY1 = CurY1 + 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font)

            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY1 = CurY1 + strHeight
            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, pFont)
            End If


            '--Right Side

            CurY2 = CurY2 + 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY2 = CurY2 + strHeight
            p1Font = New Font("Calibri", 13, FontStyle.Bold)

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            CurY2 = CurY2 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)
            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, p1Font).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, " PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont)
            ElseIf Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p1Font).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, " PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY2, 0, PrintWidth, pFont)
            End If


            CurY = IIf(CurY1 > CurY2, CurY1, CurY2)

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
            LnAr(3) = CurY



            W2 = e.Graphics.MeasureString("SUPPLIERS'S REF  : ", pFont).Width
            S2 = e.Graphics.MeasureString("OTHER REFERENCE(S) :", pFont).Width

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Agent Name ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Delivery Note", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Note").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Dc No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Des_Date").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString & "    Dt.: " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Des_Date")), "dd-MM-yyyy").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Supplier's Ref", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Supplier_Reference").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Order No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "    Dt.: " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Other Reference(s)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Other_References").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Destination", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Destination").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)




            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Payment Terms", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Delivery Terms", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Terms").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(3))
            LnAr(4) = CurY

            CurY = CurY + 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), p1Font)


            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim CurY1 As Single = 0
        Dim AmtInWrds As String = ""
        Dim W2 As Single
        Dim vLine_Pen = New Pen(Color.Black, 2)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            CurY = CurY + TxtHgt - 10


            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(6), LMargin, LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1), LnAr(6), LMargin + ClArr(1), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2), LnAr(6), LMargin + ClArr(1) + ClArr(2), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(4))

            'e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(4))

            'p1Font = New Font("Calibri", 14, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
            ' ''  End If
            'If Trim(prn_OriDupTri) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            'End If

            W2 = e.Graphics.MeasureString("BAG NO'S : ", pFont).Width

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

            ' Common_Procedures.Print_To_PrintDocument(e, "Bag/Chippam No : " & (prn_HdDt.Rows(0).Item("Bale_Nos").ToString), LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt + 10

            CurY1 = CurY

            '-----Left Side

            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Bale_Nos").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Bag No's : " & prn_HdDt.Rows(0).Item("Bale_Nos").ToString, LMargin + 10, CurY, 0, 0, pFont)
                CurY1 = CurY1 + TxtHgt
            End If


            CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 13, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt + 2
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt - 1
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt - 1
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt - 1
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p1Font)

            'CurY1 = CurY1 + TxtHgt
            'CurY1 = CurY1 + TxtHgt
            'CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            '   Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY1, 0, 0, p1Font)




            'Right Side
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Addless_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & prn_HdDt.Rows(0).Item("Discount_Percentage").ToString & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            '-------------------------------------------------------------------

            prn_CGST_Amount = prn_HdDt.Rows(0).Item("CGst_Amount").ToString
            prn_SGST_Amount = prn_HdDt.Rows(0).Item("SGst_Amount").ToString
            prn_IGST_Amount = prn_HdDt.Rows(0).Item("IGst_Amount").ToString

            prn_GST_Perc = prn_HdDt.Rows(0).Item("GST_Percentage").ToString ' Val(Common_Procedures.get_FieldValue(con, "Count_Head", "GST_Percentege", "Count_Name = '" & Trim(cbo_CountName.Text) & "'"))

            If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 16
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 11, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Taxable_Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, p1Font)
                End If

            End If

            CurY = CurY + TxtHgt
            If Val(prn_CGST_Amount) <> 0 Then
                If is_LastPage = True Then
                    If prn_CGST_Amount <> 0 Then
                        'Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + -10, CurY, 1, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_SGST_Amount) <> 0 Then

                If is_LastPage = True Then
                    If prn_SGST_Amount <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_IGST_Amount) <> 0 Then

                If is_LastPage = True Then
                    If prn_IGST_Amount <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If



            '----------------------------------------------------------------------

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("TCS_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("TCs_name_caption").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, " Round Off", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            ''e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, PageWidth, CurY)

            ' e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY + 10, PageWidth, CurY + 10)
            'CurY = CurY + TxtHgt + 15
            CurY = CurY + TxtHgt


            If CurY1 > CurY Then CurY = CurY1


            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            'CurY = CurY + TxtHgt - 10
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, PageWidth, CurY)
            CurY = CurY + 10
            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 20, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 13, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p1Font)

            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(6))

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
            CurY = CurY + 10
            AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            AmtInWrds = Replace(Trim(AmtInWrds), "", "")

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (in words)  : " & AmtInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 7
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            '*****************************************************************
            Dim SubClAr(15) As Single
            Dim Ttl_TaxAmt As Double = 0, Ttl_CGst As Double = 0, Ttl_Sgst As Double = 0, Ttl_igst As Double = 0
            Dim LnAr2 As Single

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 120 : SubClAr(2) = 120 : SubClAr(3) = 45 : SubClAr(4) = 90 : SubClAr(5) = 45 : SubClAr(6) = 90 : SubClAr(7) = 45 : SubClAr(8) = 90
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin, CurY + 15, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 15, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY)
            LnAr2 = CurY
            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TAX AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)

            Ttl_TaxAmt = Ttl_TaxAmt + Val(prn_HdDt.Rows(0).Item("Taxable_Amount").ToString)
            Ttl_CGst = Ttl_CGst + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString)
            Ttl_Sgst = Ttl_Sgst + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString)
            Ttl_igst = Ttl_igst + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString)



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr(10))
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr(10))
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr(10))
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr(10))
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), LnAr(10))

            CurY = CurY + 5
            AmtInWrds = ""
            If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
                AmtInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & AmtInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)


            '*****************************************************************

            AmtInWrds = ""
            If (Val(prn_CGST_Amount) + Val(prn_SGST_Amount) + Val(prn_IGST_Amount)) <> 0 Then
                AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_CGST_Amount) + Val(prn_SGST_Amount) + Val(prn_IGST_Amount))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)


            CurY = CurY + 10


            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Declaration : ", LMargin + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt + 3
            Common_Procedures.Print_To_PrintDocument(e, "We Declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.", LMargin + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(11) = CurY

            CurY = CurY + 2
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 7, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt + 4
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(vLine_Pen, PageWidth, LnAr(1), PageWidth, CurY)

            Dim vJurisdctn As String = ""
            vJurisdctn = Trim(Common_Procedures.settings.Jurisdiction)
            If Trim(vJurisdctn) = "" Then vJurisdctn = "Tirupur"
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(vJurisdctn) & " jurisdiction ", LMargin, CurY, 2, PageWidth, pFont)


            If Print_PDF_Status = True Then
                CurY = CurY + TxtHgt - 15
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Destination_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Destination.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cotton_Invoice_Head", "Destination", "", "(Destination = '')")
    End Sub

    Private Sub cbo_Destination_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Destination.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Destination, txt_EWay_BillNo, txt_Supplier_Reference, "Cotton_Invoice_Head", "Destination", "", "(Destination = '')")
    End Sub

    Private Sub cbo_Destination_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Destination.KeyPress
        Dim Indx As Integer = -1
        Dim strFindStr As String = ""

        Try
            If Asc(e.KeyChar) = 8 Then
                If cbo_Destination.SelectionStart <= 1 Then
                    cbo_Destination.Text = ""
                    Exit Sub
                End If
                If cbo_Destination.SelectionLength = 0 Then
                    strFindStr = cbo_Destination.Text.Substring(0, cbo_Destination.Text.Length - 1)
                Else
                    strFindStr = cbo_Destination.Text.Substring(0, cbo_Destination.SelectionStart - 1)
                End If

            Else

                If cbo_Destination.SelectionLength = 0 Then
                    strFindStr = cbo_Destination.Text & e.KeyChar
                Else
                    strFindStr = cbo_Destination.Text.Substring(0, cbo_Destination.SelectionStart) & e.KeyChar
                End If

            End If

            Indx = cbo_Destination.FindString(strFindStr)

            If Indx <> -1 Then
                cbo_Destination.SelectedText = ""
                cbo_Destination.SelectedIndex = Indx
                cbo_Destination.SelectionStart = strFindStr.Length
                cbo_Destination.SelectionLength = cbo_Destination.Text.Length
                e.Handled = True

            Else
                'e.Handled = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If Asc(e.KeyChar) = 13 Then
            txt_Supplier_Reference.Focus()
        End If

    End Sub


    Private Sub cbo_PaymentTerms_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentTerms.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cotton_Invoice_Head", "Payment_Terms", "", "(Payment_Terms = '')")
    End Sub

    Private Sub cbo_PaymentTerms_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentTerms.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentTerms, txt_IR_No, cbo_DeliveryTerms, "Cotton_Invoice_Head", "Payment_Terms", "", "(Payment_Terms = '')")
    End Sub

    Private Sub cbo_PaymentTerms_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentTerms.KeyPress
        Dim Indx As Integer = -1
        Dim strFindStr As String = ""

        Try
            If Asc(e.KeyChar) = 8 Then
                If cbo_PaymentTerms.SelectionStart <= 1 Then
                    cbo_PaymentTerms.Text = ""
                    Exit Sub
                End If
                If cbo_PaymentTerms.SelectionLength = 0 Then
                    strFindStr = cbo_PaymentTerms.Text.Substring(0, cbo_PaymentTerms.Text.Length - 1)
                Else
                    strFindStr = cbo_PaymentTerms.Text.Substring(0, cbo_PaymentTerms.SelectionStart - 1)
                End If

            Else

                If cbo_PaymentTerms.SelectionLength = 0 Then
                    strFindStr = cbo_PaymentTerms.Text & e.KeyChar
                Else
                    strFindStr = cbo_PaymentTerms.Text.Substring(0, cbo_PaymentTerms.SelectionStart) & e.KeyChar
                End If

            End If

            Indx = cbo_PaymentTerms.FindString(strFindStr)

            If Indx <> -1 Then
                cbo_PaymentTerms.SelectedText = ""
                cbo_PaymentTerms.SelectedIndex = Indx
                cbo_PaymentTerms.SelectionStart = strFindStr.Length
                cbo_PaymentTerms.SelectionLength = cbo_PaymentTerms.Text.Length
                e.Handled = True

            Else
                'e.Handled = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If Asc(e.KeyChar) = 13 Then
            cbo_DeliveryTerms.Focus()
        End If

    End Sub

    Private Sub cbo_DeliveryTerms_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTerms.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cotton_Invoice_Head", "Destination", "", "(Destination = '')")
    End Sub

    Private Sub cbo_DeliveryTerms_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTerms.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Destination, cbo_PaymentTerms, txt_Other_References, "Cotton_Invoice_Head", "Destination", "", "(Destination = '')")
    End Sub

    Private Sub cbo_DeliveryTerms_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTerms.KeyPress
        Dim Indx As Integer = -1
        Dim strFindStr As String = ""

        Try
            If Asc(e.KeyChar) = 8 Then
                If cbo_DeliveryTerms.SelectionStart <= 1 Then
                    cbo_DeliveryTerms.Text = ""
                    Exit Sub
                End If
                If cbo_DeliveryTerms.SelectionLength = 0 Then
                    strFindStr = cbo_DeliveryTerms.Text.Substring(0, cbo_DeliveryTerms.Text.Length - 1)
                Else
                    strFindStr = cbo_DeliveryTerms.Text.Substring(0, cbo_DeliveryTerms.SelectionStart - 1)
                End If

            Else

                If cbo_DeliveryTerms.SelectionLength = 0 Then
                    strFindStr = cbo_DeliveryTerms.Text & e.KeyChar
                Else
                    strFindStr = cbo_DeliveryTerms.Text.Substring(0, cbo_DeliveryTerms.SelectionStart) & e.KeyChar
                End If

            End If

            Indx = cbo_DeliveryTerms.FindString(strFindStr)

            If Indx <> -1 Then
                cbo_DeliveryTerms.SelectedText = ""
                cbo_DeliveryTerms.SelectedIndex = Indx
                cbo_DeliveryTerms.SelectionStart = strFindStr.Length
                cbo_DeliveryTerms.SelectionLength = cbo_DeliveryTerms.Text.Length
                e.Handled = True

            Else
                'e.Handled = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If Asc(e.KeyChar) = 13 Then
            txt_Other_References.Focus()
        End If

    End Sub

    Private Sub txt_DeliveryNote_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DeliveryNote.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to Save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()

            Else
                dtp_Date.Focus()

            End If

        End If
    End Sub

    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        Print_record()
        Print_PDF_Status = False
    End Sub

    Private Sub btn_close_pack_selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close_pack_selection.Click
        Close_Pack_Selection()
        pnl_Back.Enabled = True
        pnl_Pack_Selection.Visible = False
    End Sub

    Private Sub txt_InvoiceBag_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_InvoiceBag.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_InvWgt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_InvWgt.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_rate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_BagKg_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BagKg.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub btn_EDIT_TCS_TaxableValue_Click(sender As System.Object, e As System.EventArgs) Handles btn_EDIT_TCS_TaxableValue.Click
        txt_TCS_TaxableValue.Enabled = Not txt_TCS_TaxableValue.Enabled
        If txt_TCS_TaxableValue.Enabled Then
            txt_TCS_TaxableValue.Focus()
        Else
            txt_TcsPerc.Focus()
        End If
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


            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            '-----------TOTAL SALES

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@entrydate", dtp_Date.Value.Date)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

            If Led_ID <> 0 Then

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and (a.Voucher_Code LIKE 'GSCWS-%' OR a.Voucher_Code LIKE 'GCNIN-%'  OR a.Voucher_Code LIKE 'GSPTS-%') "
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

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(vPrevYrCode) & "' and (a.Voucher_Code LIKE 'GSCWS-%' OR a.Voucher_Code LIKE 'GCNIN-%'  OR a.Voucher_Code LIKE 'GSPTS-%') "
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

    Private Sub txt_TCS_TaxableValue_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TCS_TaxableValue.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TcsPerc_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TcsPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub chk_TCS_Tax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCS_Tax.CheckedChanged
        NetAmount_Calculation()
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

    Private Sub txt_Supplier_Reference_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Supplier_Reference.KeyDown
        If e.KeyValue = 38 Then
            cbo_Destination.Focus()
        End If
        If e.KeyValue = 40 Then
            txt_IR_No.Focus()
        End If
    End Sub

    Private Sub txt_Supplier_Reference_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Supplier_Reference.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_IR_No.Focus()
        End If
    End Sub

    Private Sub btn_EInvoice_Generation_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click
        'rtbeInvoiceResponse.Text = ""
        'txt_EWBNo.Text = txt_Electronic_RefNo.Text
        grp_EInvoice.Visible = True
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

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Cotton_Invoice_Head Where Cotton_Invoice_Code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Cotton_Invoice_Head Where Cotton_Invoice_Code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) > 0"
        c = Cmd.ExecuteScalar

        If c > 0 Then
            Dim k As Integer = MsgBox("An IRN Has been Generated already for this Invoice. Do you want to Delete the Previous IRN ?", vbYesNo, "IRN Generated")
            If k = vbNo Then
                MsgBox("Cannot Create a New IRN When there is an IRN generated already !", vbOKOnly, "Duplicate IRN ")
                Exit Sub
            Else
                '----
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

            Dim vDocNo As String = ""
            Dim vDocYs_No As String = ""
            Dim InvSubNo As String = ""


            Cmd.CommandText = "Select Cotton_Invoice_No from Cotton_Invoice_Head Where Cotton_Invoice_Code = '" & Trim(NewCode) & "'"
            vDocYs_No = Cmd.ExecuteScalar

            InvSubNo = Replace(Trim(vDocYs_No), Trim(Val(vDocYs_No)), "")

            If Common_Procedures.settings.CustomerCode = "1087" Then


                vDocNo = Trim(txt_InvoicePrefixNo.Text) & Trim(Format(Val(vDocYs_No), "######0000")) & Trim(InvSubNo)
            Else
                If Trim(txt_InvoicePrefixNo.Text) <> "" Then
                    vDocNo = Trim(txt_InvoicePrefixNo.Text) & "-" & vDocYs_No
                Else
                    vDocNo = vDocYs_No
                End If
            End If

            'Cmd.CommandText = "Insert into e_Invoice_Head Select (Invoice_PrefixNo + Cotton_Invoice_No) , Cotton_Invoice_Date, Ledger_IdNo, DeliveryTo_Idno, Taxable_Amount, " &
            '                  " CGST_Amount, SGST_Amount, " &
            '                  " IGST_Amount , 0, 0, RoundOff_Amount, (Net_Amount-TCS_Amount) as Nett_Invoice_Amount, '" & Trim(NewCode) & "',ISNULL(TCS_AMOUNT,0) as OtherCharges from Cotton_Invoice_Head where Cotton_Invoice_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Head Select '" & Trim(vDocNo) & "'  , Cotton_Invoice_Date, Ledger_IdNo, (CASE WHEN DeliveryTo_Idno <> 0 THEN DeliveryTo_Idno ELSE Ledger_IdNo END) as delvtoidno, Taxable_Amount, " &
                              " CGST_Amount, SGST_Amount, " &
                              " IGST_Amount , 0, 0, RoundOff_Amount, (Net_Amount) as Nett_Invoice_Amount, '" & Trim(NewCode) & "',ISNULL(TCS_AMOUNT,0) as OtherCharges , DeliveryTo_Idno from Cotton_Invoice_Head where Cotton_Invoice_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()


            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Details  " &
                               " Select 1 as Sl_No, 0 as IsServc, (c.Count_Name + ' ' + a.Yarn_Details ) as producDescription , a.HSN_Code, '' as batchdetails, a.Invoice_Weight, 'KGS' as UOM, a.Rate, (a.Amount + (Freight_Amount+AddLess_Amount) ), a.Discount_Amount, " &
                              " a.Taxable_Amount, a.GST_Percentage, 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, " &
                              " 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                              " from Cotton_Invoice_Head a " &
                              " inner join Count_head C on a.Count_IdNo = c.Count_IdNo " &
                              " Where Cotton_Invoice_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            'Cmd.CommandText = "Insert into e_Invoice_Details  " &
            '                   " Select 1 as Sl_No, 0 as IsServc, (c.Count_Name + ' ' + a.Yarn_Details ) as producDescription , a.HSN_Code, '' as batchdetails, a.Invoice_Weight, 'KGS' as UOM, a.Rate, (a.Amount + (Freight_Amount+AddLess_Amount) ), a.Discount_Amount, " &
            '                  " a.Taxable_Amount, a.GST_Percentage, 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, " &
            '                  " (Freight_Amount+AddLess_Amount) as OthChrg, a.Taxable_Amount as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
            '                  " from Cotton_Invoice_Head a " &
            '                  " inner join Count_head C on a.Count_IdNo = c.Count_IdNo " &
            '                  " Where Cotton_Invoice_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()

            'Cmd.CommandText = "Insert into e_Invoice_Details Select 1 as Sl_No, 0, (c.Count_Name + ' ' + a.Yarn_Details ) as producDescription , a.HSN_Code, '', a.Invoice_Weight, 'KGS', a.Rate, a.Amount, a.Discount_Amount, " &
            '                  " a.Taxable_Amount, a.GST_Percentage, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0," &
            '                  " a.Taxable_Amount, '', '" & Trim(NewCode) & "' " &
            '                  " from Cotton_Invoice_Head a " &
            '                  " inner join Count_head C on a.Count_IdNo = c.Count_IdNo " &
            '                  " Where Cotton_Invoice_Code = '" & Trim(NewCode) & "'"
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
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Cotton_Invoice_Head", "Cotton_Invoice_Code", Pk_Condition)

    End Sub

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Cotton_Invoice_Head", "Cotton_Invoice_Code", con, txt_eInvoice_CancelStatus, NewCode, "Data entry mistake")

    End Sub

    Private Sub btn_Refresh_eInvoice_Info_Click(sender As Object, e As EventArgs) Handles btn_Refresh_eInvoice_Info.Click

        'Threading.Thread.Sleep(10000)

        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.RefresheInvoiceInfoByIRN(txt_eInvoiceNo.Text, Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Cotton_Invoice_Head", "Cotton_Invoice_Code")


        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select E_Invoice_QR_Image,E_Invoice_IRNO,E_Invoice_ACK_No,E_Invoice_ACK_Date,E_Invoice_Cancelled_Status FROM  Yarn_Sales_Head WHERE Yarn_Sales_Code = '" & NewCode & "'", con)

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

    Private Sub cbo_TaxType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_TaxType.SelectedIndexChanged

    End Sub

    Private Sub cbo_CountName_TextChanged(sender As Object, e As EventArgs) Handles cbo_CountName.TextChanged

    End Sub

    Private Sub btn_Generate_EWB_IRN_Click(sender As Object, e As EventArgs) Handles btn_Generate_EWB_IRN.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Cotton_Invoice_Head Where Cotton_Invoice_Code = '" & NewCode & "'"
        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Cotton_Invoice_Head Where Cotton_Invoice_Code = '" & NewCode & "' and (Len(EWay_BIll_No) >0 or Len(E_Invoice_IRNO) = 0 OR E_Invoice_IRNO IS NULL )"
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


            Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]         ,     	[TransID]        ,	            [TransMode]  ,	[TransDocNo]    , [TransDocDate] ,       	[VehicleNo]        ,                [Distance]                                              ,	[VehType] ,	[TransName]         ,    [InvCode]           ,  Company_Idno ,     Company_Pincode,                                           Shipped_To_Idno                        ,                                       Shipped_To_Pincode               ) " &
                                " Select                A.E_Invoice_IRNO  ,  ISNULL(t.Ledger_GSTINNo, '' ) ,        '1'    ,          ''   ,             Null         ,       a.Vechile_No , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  D.Distance ELSE L.Distance END),      'R'    ,  t.Ledger_Mainname  , '" & Trim(NewCode) & "' , tZ.Company_IdNo, tZ.Company_PinCode, (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  a.DeliveryTo_IdNo ELSE a.Ledger_IdNo END), (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  D.Pincode ELSE L.Pincode END)    " &
                                    " from Cotton_Invoice_Head a INNER JOIN Company_Head tZ on a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D on a.DeliveryTo_IdNo = D.Ledger_IdNo LEFT OUTER JOIN Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Where a.Cotton_Invoice_Code = '" & Trim(NewCode) & "'"

            Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()

            MessageBox.Show(ex.Message + " Cannot Generate Eway-Bill.", "ERROR WHILE GENERATING E-WAY BILL BY IRN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Exit Sub

        End Try


        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateEWBByIRN(NewCode, rtbeInvoiceResponse, txt_eWayBill_No, txt_EWB_Date, txt_EWB_ValidUpto, con, "Cotton_Invoice_Head", "Cotton_Invoice_Code", txt_EWB_Canellation_Reason, txt_EWB_Cancel_Status, Pk_Condition)

        Cmd.CommandText = "DELETE FROM EWB_By_IRN WHERE INVCODE = '" & NewCode & "'"
        Cmd.ExecuteNonQuery()


    End Sub

    Private Sub btn_Print_EWB_Click(sender As Object, e As EventArgs) Handles btn_Print_EWB.Click

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWay_BillNo.Text, rtbeInvoiceResponse, 0)

    End Sub

    Private Sub btn_Cancel_EWB_Click(sender As Object, e As EventArgs) Handles btn_Cancel_EWB.Click
        If Len(Trim(txt_EWB_Canellation_Reason.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))

        einv.Cancel_EWB_IRN(NewCode, txt_eWayBill_No.Text, rtbeInvoiceResponse, txt_eInvoice_CancelStatus, con, "Cotton_Invoice_Head", "Cotton_Invoice_Code", txt_EWB_Canellation_Reason.Text)
    End Sub
    Private Sub txt_eWayBill_No_TextChanged(sender As Object, e As EventArgs) Handles txt_eWayBill_No.TextChanged
        txt_EWay_BillNo.Text = txt_eWayBill_No.Text
    End Sub

    Private Sub btn_Get_QR_Code_Click(sender As Object, e As EventArgs) Handles btn_Get_QR_Code.Click
        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con

        CMD.CommandText = "DELETE FROM " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_refresh where IRN = '" & txt_eInvoiceNo.Text & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = " INSERT INTO " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_Refresh ([IRN] ,[ACK_No] , [DOC_No] , [SEARCH_BY]  , [COMPANY_IDNO],[Update_Table] ,[Update_table_Unique_Code],[COMPANYGROUP_IDNO] ) VALUES " &
                          "('" & txt_eInvoiceNo.Text & "' ,'','','I'," & Val(Common_Procedures.CompIdNo).ToString & ",'ClothSales_Invoice_Head', 'E_Invoice_IRNO'," & Val(Common_Procedures.CompGroupIdNo).ToString & ")"
        CMD.ExecuteNonQuery()

        Shell(Application.StartupPath & "\Refresh_IRN.EXE")
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

        LastNo = lbl_InvNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_InvNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub
    Private Sub cbo_Transport_GotFocus(sender As Object, e As EventArgs) Handles cbo_Transport.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Transport_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Transport.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_BaleNos, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_TcsPerc, txt_BaleNos, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Transport_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Transport.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
End Class