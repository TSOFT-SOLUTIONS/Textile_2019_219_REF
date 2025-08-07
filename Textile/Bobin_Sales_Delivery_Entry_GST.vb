Imports System.IO
Public Class Bobin_Sales_Delivery_Entry_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False
    Private vEMAIL_Attachment_FileName As String

    Private Pk_Condition As String = "GBSDL-"
    Private Pk_Condition1 As String = "GBDCF-"

    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer = 0

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_KuriDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private dgv_LevColNo As Integer
    Private Balance_Bobin As Double = 0
    Private Balance_Amount As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private prn_Count As Integer
    Private prn_HdAr(1000, 10) As String
    Private prn_DetAr(1000, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private NoCalc_Status As Boolean = False


    Dim prn_GST_Perc As Single
    Dim prn_CGST_Amount As Double
    Dim prn_SGST_Amount As Double
    Dim prn_IGST_Amount As Double

    Public vmskGrText As String = ""
    Public vmskGrStrt As Integer = -1
    Private prn_OriDupTri_Count As String = ""
    Private MoveCal_sts As Boolean = False

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Enum DGVCol_BobinSalesDetails
        SNo '0
        Ends_Count '1
        Colour '2
        Border_Size '3
        Bobin_Size '4
        'NoOfBobins '5
        'MeterBobin '6
        MeterBobin '5
        NoOfBobins '6
        Total_Meter '7
        MeterReel '8
        NoOfReel '9
        Rate '10
        Amount '11
        Bobin_InvoiceCode '12
        Details_SNo '13
    End Enum

    Private Sub clear()

        NoCalc_Status = True
        New_Entry = False
        Insert_Entry = False
        MoveCal_sts = True


        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Print_PDF_Status = False
        EMAIL_Status = False
        WHATSAPP_Status = False
        vEMAIL_Attachment_FileName = ""

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_InvNo.Text = ""
        lbl_InvNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        lbl_Net_Amt.Text = ""
        cbo_Ledger.Text = ""
        cbo_VechileNo.Text = ""
        cbo_Transport.Text = ""
        txt_Freight.Text = ""
        txt_PartyBobin.Text = ""
        txt_OurBobin.Text = ""
        txt_Remarks.Text = ""
        txt_InvoicePrefixNo.Text = ""
        txt_ElectronicRefNo.Text = ""
        txt_DateAndTimeOFSupply.Text = ""
        cbo_TransportMode.Text = ""
        cbo_DeliveryTo.Text = ""
        cbo_RateFor.Text = "METER"
        dgv_BobinDetails.Columns(7).HeaderText = "TOTAL METER"

        lbl_stock_bobin.Tag = 0
        lbl_stock_bobin.Text = ""

        lbl_stock_meter.Tag = 0
        lbl_stock_meter.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_BobinDetails.Rows.Clear()
        dgv_BobinDetails_Total.Rows.Clear()
        Grid_DeSelect()

        cbo_Grid_Bobin_Size.Visible = False
        cbo_Grid_Bobin_Size.Tag = -1
        cbo_Grid_Bobin_Size.Text = ""

        cbo_BobinEnds.Visible = False
        cbo_BobinEnds.Tag = -1
        cbo_BobinColour.Visible = False
        cbo_BobinColour.Tag = -1
        cbo_BobinBorderSize.Visible = False
        cbo_BobinBorderSize.Tag = -1

        cbo_BobinEnds.Text = ""
        cbo_BobinColour.Text = ""
        cbo_BobinBorderSize.Text = ""

        lbl_ItemGrp_ID.Text = "0"
        lbl_Grid_GST_Perc.Text = ""
        lbl_Grid_HSNCode.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        lbl_TaxableValue.Text = ""

        txt_Freight_Name.Text = "Frieght"
        txt_Frieght_After.Text = ""

        txt_GrTime.Text = ""
        msk_Date.Text = ""
        'dgv_Details.Tag = ""
        'dgv_LevColNo = -1
        lbl_Roundoff.Text = ""

        '-----------------------------


        pic_IRN_QRCode_Image.BackgroundImage = Nothing
        txt_eInvoiceNo.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckDate.Text = ""
        txt_eInvoice_CancelStatus.Text = ""

        txt_EInvoiceCancellationReson.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckDate.Text = ""
        txt_EInvoiceCancellationReson.Text = ""
        txt_eInvoice_CancelStatus.Text = ""

        Grp_EWB.Visible = False
        txt_eWayBill_No.Text = ""
        txt_EWB_Date.Text = ""
        txt_EWB_ValidUpto.Text = ""
        txt_EWB_Cancel_Status.Text = ""
        txt_EWB_Canellation_Reason.Text = ""

        txt_eWayBill_No.Enabled = True
        rtbeInvoiceResponse.Text = ""
        rtbEWBResponse.Text = ""

        grp_EInvoice.Visible = False

        '---------------------------

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_BobinEnds.Enabled = True
        cbo_BobinEnds.BackColor = Color.White

        cbo_BobinColour.Enabled = True
        cbo_BobinColour.BackColor = Color.White

        cbo_BobinBorderSize.Enabled = True
        cbo_BobinBorderSize.BackColor = Color.White

        dgv_BobinDetails.ReadOnly = False

        dgv_ActCtrlName = ""


        NoCalc_Status = False
        MoveCal_sts = False
        chk_NoStockPosting.Checked = False
    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails_Total.CurrentCell) Then dgv_BobinDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = True
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
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_BobinEnds.Name Then
            cbo_BobinEnds.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BobinColour.Name Then
            cbo_BobinColour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BobinBorderSize.Name Then
            cbo_BobinBorderSize.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_BobinDetails.Name Then
            Grid_DeSelect()
        End If

        'If Me.ActiveControl.Name <> dgv_BobinDetails.Name Then
        '    Common_Procedures.Hide_CurrentStock_Display()
        'End If

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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next

        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails_Total.CurrentCell) Then dgv_BobinDetails_Total.CurrentCell.Selected = False
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
        MoveCal_sts = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Bobin_Jari_SalesDelivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where a.Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                lbl_InvNo.Text = dt1.Rows(0).Item("Bobin_Jari_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Bobin_Jari_Delivery_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_VechileNo.Text = dt1.Rows(0).Item("Vechile_No").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "########0.00")
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_PartyBobin.Text = Format(Val(dt1.Rows(0).Item("Party_Bobin").ToString), "########0.00")
                txt_OurBobin.Text = Format(Val(dt1.Rows(0).Item("OurOwn_Bobin").ToString), "########0.00")
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                cbo_SalesAcc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                msk_GrDate.Text = dt1.Rows(0).Item("Gr_Date").ToString
                txt_GrTime.Text = dt1.Rows(0).Item("Gr_Time").ToString
                cbo_RateFor.Text = dt1.Rows(0).Item("Rate_For").ToString

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))



                If Val(dt1.Rows(0).Item("No_Stock_Posting").ToString) = 1 Then
                    chk_NoStockPosting.Checked = True
                Else
                    chk_NoStockPosting.Checked = False
                End If

                If dt1.Rows(0).Item("Entry_VAT_GST_Type").ToString = "GST" Then
                    chk_GSTTax_Invocie.Checked = True
                Else
                    chk_GSTTax_Invocie.Checked = False
                End If


                txt_eInvoiceNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                txt_ElectronicRefNo.Text = Trim(dt1.Rows(0).Item("Electronic_Reference_No").ToString)
                'txt_EWBNo.Text = Trim(dt1.Rows(0).Item("Electronic_Reference_No").ToString)

                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_Date")) Then txt_eInvoiceAckDate.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_Date").ToString)
                If Trim(txt_eInvoiceNo.Text) <> "" Then
                    If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancelled_Status")) Then
                        txt_eInvoice_CancelStatus.Text = IIf(dt1.Rows(0).Item("E_Invoice_Cancelled_Status") = True, "Cancelled", "Active")
                    End If
                End If

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
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Date")) Then txt_EWB_Date.Text = Trim(dt1.Rows(0).Item("EWB_Date").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Valid_Upto")) Then txt_EWB_ValidUpto.Text = Trim(dt1.Rows(0).Item("EWB_Valid_Upto").ToString)
                If Trim(txt_eWayBill_No.Text) <> "" Then
                    If Not IsDBNull(dt1.Rows(0).Item("EWB_Cancelled")) Then
                        If dt1.Rows(0).Item("EWB_Cancelled") = True Then
                            txt_EWB_Cancel_Status.Text = "Cancelled"
                        Else
                            txt_EWB_Cancel_Status.Text = "Active"
                        End If
                    End If
                End If


                If Not IsDBNull(dt1.Rows(0).Item("EWBCANCELLATION_REASON")) Then txt_EWB_Canellation_Reason.Text = Trim(dt1.Rows(0).Item("EWBCANCELLATION_REASON").ToString)



                da2 = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name, c.Colour_Name, d.BorderSize_Name from Bobin_Jari_SalesDelivery_Bobin_Details a INNER JOIN Endscount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN BorderSize_Head d ON a.BorderSize_IdNo = d.BorderSize_IdNo Where a.Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_BobinDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_BobinDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.SNo).Value = Val(SNo)
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.Ends_Count).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.Colour).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.Border_Size).Value = dt2.Rows(i).Item("BorderSize_Name").ToString
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.NoOfBobins).Value = Val(dt2.Rows(i).Item("Bobins").ToString)

                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.Bobin_Size).Value = Common_Procedures.BobinSize_IdNoToName(con, Val(dt2.Rows(i).Item("Bobin_Size_IdNo").ToString))
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.MeterBobin).Value = Format(Val(dt2.Rows(i).Item("Meter_Bobin").ToString), "########0.00")
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value = Format(Val(dt2.Rows(i).Item("METERS").ToString), "########0.00")

                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.MeterReel).Value = Format(Val(dt2.Rows(i).Item("Meter_Reel").ToString), "########0.00")
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value = Val(dt2.Rows(i).Item("reel").ToString)
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.Rate).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.Amount).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                        'dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.Bobin_InvoiceCode).Value = dt2.Rows(i).Item("Bobin_Jari_Sales_Invoice_Code").ToString
                        'dgv_BobinDetails.Rows(n).Cells(DGVCol_BobinSalesDetails.Details_SNo).Value = dt2.Rows(i).Item("Bobin_Jari_Delivery_Bobin_Slno").ToString

                        'If Val(dgv_KuriDetails.Rows(n).Cells(7).Value) <> 0 Then
                        '    For j = 0 To dgv_KuriDetails.ColumnCount - 1
                        '        dgv_KuriDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        '    Next j
                        '    LockSTS = True
                        'End If
                    Next i

                End If
                dt2.Clear()


                'lbl_stock_bobin.Text = Common_Procedures.get_Bobin_CurrentStock(con, Val(lbl_Company.Tag), vLedID, vCntID)
                'lbl_stock_meter.Text = Common_Procedures.get_Bobin_CurrentStock(con, Val(lbl_Company.Tag), vLedID, vCntID)

                With dgv_BobinDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Bobins").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")

                    .Rows(0).Cells(8).Value = Val(dt1.Rows(0).Item("Total_Reels").ToString)
                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")

                End With

                lbl_Grid_GST_Perc.Text = Format(Val(dt1.Rows(0).Item("GST_Percentage").ToString), "########0.00")
                lbl_Grid_HSNCode.Text = dt1.Rows(0).Item("HSN_Code").ToString
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "########0.00")
                txt_Frieght_After.Text = Format(Val(dt1.Rows(0).Item("Frieght_2").ToString), "########0.00")
                lbl_TaxableValue.Text = Format(Val(dt1.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00")

                txt_Freight_Name.Text = dt1.Rows(0).Item("Frieght_2_Text").ToString
                lbl_ItemGrp_ID.Text = Val(dt1.Rows(0).Item("Item_Group_id").ToString)

                'txt_ElectronicRefNo.Text = Trim(dt1.Rows(0).Item("Electronic_Reference_No").ToString)
                txt_DateAndTimeOFSupply.Text = Trim(dt1.Rows(0).Item("Date_And_Time_Of_Supply").ToString)
                cbo_TransportMode.Text = Trim(dt1.Rows(0).Item("Transport_Mode").ToString)

                lbl_Net_Amt.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")

                lbl_Roundoff.Text = Format(Val(dt1.Rows(0).Item("Round_Off_Amount").ToString), "########0.00")

            End If
            dt1.Clear()

            If LockSTS = True Then
                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                cbo_BobinEnds.Enabled = False
                cbo_BobinEnds.BackColor = Color.LightGray

                cbo_BobinColour.Enabled = False
                cbo_BobinColour.BackColor = Color.LightGray

                cbo_BobinBorderSize.Enabled = False
                cbo_BobinBorderSize.BackColor = Color.LightGray

                dgv_BobinDetails.ReadOnly = True


            End If

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dgv_ActCtrlName = ""
            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()

        End Try
        NoCalc_Status = False
        MoveCal_sts = False
        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Bobin_Sales_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinEnds.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinEnds.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinColour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BORDER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinColour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinBorderSize.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BORDERSIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinBorderSize.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            '----MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        lbl_ItemGrp_ID.Text = "0"
        FrmLdSTS = False

    End Sub

    Private Sub Bobin_Sales_Delivery_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
        da.Fill(dt1)
        cbo_BobinEnds.DataSource = dt1
        cbo_BobinEnds.DisplayMember = "EndsCount_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Ledger.DataSource = dt2
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Transport.DataSource = dt3
        cbo_Transport.DisplayMember = "Ledger_DisplayName"


        'da = New SqlClient.SqlDataAdapter("select distinct(Vechile_No) from Bobin_Jari_SalesDelivery_Head order by Vechile_No", con)
        'da.Fill(dt4)
        'cbo_VechileNo.DataSource = dt4
        'cbo_VechileNo.DisplayMember = "Vechile_No"

        cbo_RateFor.Items.Clear()
        cbo_RateFor.Items.Add(" ")
        cbo_RateFor.Items.Add("METER")
        cbo_RateFor.Items.Add("KG")


        cbo_BobinEnds.Visible = False

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        'pnl_stock.Visible = False
        'pnl_stock.Left = (Me.Width - pnl_stock.Width) \ 2
        'pnl_stock.Top = (Me.Height - pnl_stock.Height) \ 2

        pnl_stock.Visible = False
        pnl_stock.Left = pnl_Back.Left + 18
        pnl_stock.Top = pnl_Back.Top + pnl_Back.Height - pnl_stock.Height - 10



        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Common_Procedures.settings.CustomerCode = "1027" Then
            pnl_Print.Visible = False
            pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
            pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
            pnl_Print.BringToFront()
        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinEnds.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinColour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinBorderSize.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAcc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvoicePrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VechileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OurBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Frieght_After.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ElectronicRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTimeOFSupply.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_GrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrTime.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Bobin_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RateFor.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinEnds.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinColour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinBorderSize.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VechileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OurBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAcc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Frieght_After.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvoicePrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ElectronicRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateAndTimeOFSupply.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_GrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GrTime.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Bobin_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RateFor.LostFocus, AddressOf ControlLostFocus


        'AddHandler msk_GrDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ElectronicRefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DateAndTimeOFSupply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GrTime.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Remarks.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvoicePrefixNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ElectronicRefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateAndTimeOFSupply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GrTime.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Bobin_Sales_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
        'Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub Bobin_Sales_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True And pnl_stock.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    btn_stock_close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Print.Visible = True Then
                    btn_Close_Print_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_BobinDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_BobinDetails.Name Then
                dgv1 = dgv_BobinDetails

            ElseIf dgv_BobinDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BobinDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_BobinDetails.Name.ToString)) Then
                dgv1 = dgv_BobinDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 4 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If dgv1.Name = dgv_BobinDetails.Name Then
                                    txt_PartyBobin.Focus()
                                Else
                                    txt_Remarks.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DGVCol_BobinSalesDetails.Ends_Count)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                'If dgv1.Name = dgv_BobinDetails.Name Then
                                '    txt_Freight.Focus()
                                'Else
                                '    If dgv_BobinDetails.Rows.Count > 0 Then
                                '        dgv_BobinDetails.Focus()
                                '        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                                '        dgv_BobinDetails.CurrentCell.Selected = True
                                '    Else
                                '        txt_Freight.Focus()
                                '    End If
                                'End If
                                txt_Freight.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 4)

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
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "Company_Type = 'ACCOUNT'"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()
                    dt1.Dispose()
                    da.Dispose()

                    new_record()

                Else
                    Me.Close()

                End If

            Else

                Me.Close()

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
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text)

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Bobin_Sales_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Bobin_Sales_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Bobin_Sales_delivery_Entry, New_Entry, Me, con, "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", NewCode, "Bobin_Jari_Delivery_Date", "(Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select BobinSales_Invoice_Code from Bobin_Jari_SalesDelivery_Head Where Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) <> "" Then
                    MessageBox.Show("Already Invoiced", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Bobin_Jari_Delivery_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Bobin_Jari_SalesDelivery_Bobin_Details", "Bobin_Jari_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "EndsCount_IdNo, Colour_IdNo, BorderSize_IdNo, Bobins, Meter_Bobin, Meters, Meter_Reel, Reel, Rate, Amount", "Sl_No", "Bobin_Jari_Delivery_Code, For_OrderBy, Company_IdNo, Bobin_Jari_SalesDelivery_No, Bobin_Jari_SalesDelivery_Date, Ledger_Idno", trans)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Bobin_Jari_SalesDelivery_Bobin_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Bobin_Jari_Delivery_Jari_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bobin_Jari_SalesDelivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select EndsCount_name from EndsCount_head order by EndsCount_name", con)
            da.Fill(dt2)
            cbo_Filter_EndsName.DataSource = dt2
            cbo_Filter_EndsName.DisplayMember = "EndsCount_name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_EndsName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_EndsName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Jari_Delivery_No from Bobin_Jari_SalesDelivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (Bobin_Jari_Delivery_Code LIKE '" & Trim(Pk_Condition) & "%') and Bobin_Jari_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bobin_Jari_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Jari_Delivery_No from Bobin_Jari_SalesDelivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and (Bobin_Jari_Delivery_Code LIKE '" & Trim(Pk_Condition) & "%')  and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bobin_Jari_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Jari_Delivery_No from Bobin_Jari_SalesDelivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and (Bobin_Jari_Delivery_Code LIKE '" & Trim(Pk_Condition) & "%')  and Bobin_Jari_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Jari_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Jari_Delivery_No from Bobin_Jari_SalesDelivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (Bobin_Jari_Delivery_Code LIKE '" & Trim(Pk_Condition) & "%')  and Bobin_Jari_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Jari_Delivery_No desc", con)
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

            lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", "For_OrderBy", "(Bobin_Jari_Delivery_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_InvNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Bobin_Jari_SalesDelivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Jari_Delivery_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Bobin_Jari_Delivery_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Bobin_Jari_Delivery_Date").ToString
                    If dt1.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                End If
            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bobin_Jari_Delivery_No from Bobin_Jari_SalesDelivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Bobin_Sales_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Bobin_Sales_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Bobin_Sales_delivery_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW DELIVERY INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bobin_Jari_Delivery_No from Bobin_Jari_SalesDelivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Ens_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim Cnt_ID As Integer = 0
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim BthSz_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim vEdsCnt_ID As Integer = 0
        Dim PBlNo As String = ""
        Dim vTotBbns As Single, vTotMtrs As Single
        Dim vTotReel As Single, vTotamt As Single, vTotWgt As Single
        Dim Nr As Integer = 0
        Dim noStockpost As Integer = 0
        Dim SlAc_ID As Integer = 0
        Dim TaxType As String = ""
        Dim ItmGrpID As Integer = 0
        Dim vDelvTo_IdNo As Integer = 0
        Dim vGrDt As String = ""
        Dim BbnSz_Id As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Bobin_Sales_Delivery_Entry, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Bobin_Sales_delivery_Entry, New_Entry, Me, con, "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", NewCode, "Bobin_Jari_Delivery_Date", "(Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Bobin_Jari_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        SlAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAcc.Text)
        If SlAc_ID = 0 Then
            MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAcc.Enabled Then cbo_SalesAcc.Focus()
            Exit Sub
        End If
        vDelvTo_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)

        noStockpost = 0
        If chk_NoStockPosting.Checked = True Then noStockpost = 1
        If chk_GSTTax_Invocie.Checked = True Then
            TaxType = "GST"
        Else
            TaxType = "NO TAX"
        End If
        vGrDt = ""
        If Trim(msk_GrDate.Text) <> "" Then
            If IsDate(msk_GrDate.Text) = True Then
                vGrDt = Trim(msk_GrDate.Text)
            End If
        End If
        ''ItmGrpID = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "ItemGroup_IdNo", "EndsCount_Name = '" & Trim(cbo_BobinEnds.Text) & "'")


        ''lbl_Grid_HSNCode.Text = Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_HSN_Code", "ItemGroup_IdNo = '" & ItmGrpID & "'")

        'lbl_Grid_GST_Perc.Text = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_GST_Percentage", "ItemGroup_IdNo = '" & ItmGrpID & "'"))

        Delv_ID = 0  ' Led_ID

        Rec_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        With dgv_BobinDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(DGVCol_BobinSalesDetails.Ends_Count).Value) = "" Then
                        MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(DGVCol_BobinSalesDetails.SNo)
                        End If
                        Exit Sub
                    End If

                    'BbnSz_Id = Common_Procedures.BobinSize_NameToIdNo(con, Trim(cbo_Grid_Bobin_Size.Text))
                    'If Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Bobin_Size).Value) = 0 Then
                    '    MessageBox.Show("Invalid Bobin Size Name?...", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '    cbo_Grid_Bobin_Size.Focus()
                    '    Exit Sub
                    'End If

                    If Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(DGVCol_BobinSalesDetails.Total_Meter)
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value) = 0 Then
                        MessageBox.Show("Invalid Reel..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(DGVCol_BobinSalesDetails.NoOfReel)
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Amount).Value) = 0 Then
                        MessageBox.Show("Invalid Amount..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(DGVCol_BobinSalesDetails.Rate)
                        Exit Sub
                    End If

                End If

            Next
        End With
        ' lbl_UserName.Text = Common_Procedures.User.IdNo
        NoCalc_Status = True
        MoveCal_sts = True
        '   Total_Calculation()

        vTotBbns = 0 : vTotMtrs = 0 : vTotReel = 0 : vTotamt = 0
        If dgv_BobinDetails_Total.RowCount > 0 Then
            vTotBbns = Val(dgv_BobinDetails_Total.Rows(0).Cells(5).Value())   '4 
            vTotMtrs = Val(dgv_BobinDetails_Total.Rows(0).Cells(6).Value())   '6
            vTotReel = Val(dgv_BobinDetails_Total.Rows(0).Cells(8).Value())
            vTotamt = Val(dgv_BobinDetails_Total.Rows(0).Cells(10).Value())

        End If

        If Val(lbl_Net_Amt.Text) = 0 Then lbl_Net_Amt.Text = 0
        If Val(lbl_CGST_Amount.Text) = 0 Then lbl_CGST_Amount.Text = 0
        If Val(lbl_SGST_Amount.Text) = 0 Then lbl_SGST_Amount.Text = 0
        If Val(lbl_IGST_Amount.Text) = 0 Then lbl_IGST_Amount.Text = 0

        'If (Val(txt_OurBobin.Text) + Val(txt_PartyBobin.Text)) <> Val(vTotBbns) Then
        '    MessageBox.Show("Invalid Bobins..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_PartyBobin.Enabled Then txt_PartyBobin.Focus()
        '    Exit Sub
        'End If

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

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

        Dim vEInvAckDate As String = ""
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

                lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", "For_OrderBy", "(Bobin_Jari_Delivery_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            'Da = New SqlClient.SqlDataAdapter("select count(*) from Bobin_Jari_SalesDelivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "' and BobinSales_Invoice_Code <> ''", con)
            'Da.SelectCommand.Transaction = tr
            'Dt1 = New DataTable
            'Da.Fill(Dt1)
            'If Dt1.Rows.Count > 0 Then
            '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
            '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
            '            Throw New ApplicationException("Already Invoiced")
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'Dt1.Clear()

            cmd.Connection = con
            cmd.Transaction = tr



            vOrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Bobin_Jari_SalesDelivery_Head ( Bobin_Jari_Delivery_Code,        Company_IdNo                ,   Bobin_Jari_Delivery_No      , for_OrderBy       ,   Bobin_Jari_Delivery_Date    ,  Invoice_PrefixNo  ,      Ledger_IdNo      ,              Vechile_No          ,           Freight                 ,   Transport_IdNo          ,       Total_Bobins        ,       Total_Meters        ,           Total_Reels         ,       Total_Amount    ,           Total_Weight    ,                   Party_Bobin         ,       OurOwn_Bobin                ,       Remarks                     ,       No_Stock_Posting        ,   SalesAc_IdNo                      ,       User_IdNo  ,                             Frieght_2       ,           Total_Taxable_Value     ,                Frieght_2_Text,                Total_CGST_Amount       ,                 Total_SGST_Amount               ,         Total_IGST_Amount              ,       Entry_VAT_GST_Type                  ,HSN_Code                             ,  GST_Percentage                       ,         Item_Group_id           ,       Net_Amount                 ,  Electronic_Reference_No            ,                 Date_And_Time_Of_Supply                             ,Transport_Mode      ,                                               DeliveryTo_IdNo   ,       Gr_Time                   ,         Gr_Date  ,                     Rate_For              ,       Round_Off_Amount        )" &
                                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(vOrdByNo) & ",          @EntryDate          , '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "'  , " & Str(Val(Led_ID)) & ",'" & Trim(cbo_VechileNo.Text) & "'," & Str(Val(txt_Freight.Text)) & "," & Str(Val(Trans_ID)) & ", " & Str(Val(vTotBbns)) & " , " & Str(Val(vTotMtrs)) & ",  " & Str(Val(vTotReel)) & " , " & Str(Val(vTotamt)) & ",  " & Str(Val(vTotWgt)) & " , " & Str(Val(txt_PartyBobin.Text)) & ", " & Str(Val(txt_OurBobin.Text)) & ", '" & Trim(txt_Remarks.Text) & "'," & Str(Val(noStockpost)) & ",  " & Val(SlAc_ID) & "," & Val(Common_Procedures.User.IdNo) & " ," & Str(Val(txt_Frieght_After.Text)) & " ," & Str(Val(lbl_TaxableValue.Text)) & ",'" & Trim(txt_Freight_Name.Text) & "'," & Str(Val(lbl_CGST_Amount.Text)) & "," & Str(Val(lbl_SGST_Amount.Text)) & "," & Str(Val(lbl_IGST_Amount.Text)) & ",'" & Trim(TaxType) & "','" & Trim(lbl_Grid_HSNCode.Text) & "'," & Str(Val(lbl_Grid_GST_Perc.Text)) & ", " & Val(lbl_ItemGrp_ID.Text) & ",  " & Str(Val(CSng(lbl_Net_Amt.Text))) & " ,'" & Trim(txt_ElectronicRefNo.Text) & "','" & Trim(txt_DateAndTimeOFSupply.Text) & "' ,'" & Trim(cbo_TransportMode.Text) & "' ," & Str(Val(vDelvTo_IdNo)) & ", " & Str(Val(txt_GrTime.Text)) & ", '" & Trim(vGrDt) & "' , '" & Trim(cbo_RateFor.Text) & "' ," & Str(Val(lbl_Roundoff.Text)) & " )"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bobin_Jari_Delivery_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Bobin_Jari_SalesDelivery_Bobin_Details", "Bobin_Jari_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "EndsCount_IdNo, Colour_IdNo, BorderSize_IdNo, Bobins, Meter_Bobin, Meters, Meter_Reel, Reel, Rate, Amount", "Sl_No", "Bobin_Jari_Delivery_Code, For_OrderBy, Company_IdNo, Bobin_Jari_SalesDelivery_No, Bobin_Jari_SalesDelivery_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Bobin_Jari_SalesDelivery_Head set Bobin_Jari_Delivery_Date = @EntryDate, Ledger_IdNo = " & Val(Led_ID) & ", Vechile_No = '" & Trim(cbo_VechileNo.Text) & "', Freight = " & Str(Val(txt_Freight.Text)) & ", Invoice_PrefixNo = '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' , Transport_IdNo = " & Str(Val(Trans_ID)) & ", Total_Bobins = " & Val(vTotBbns) & " , Total_Meters = " & Val(vTotMtrs) & ", SalesAc_IdNo = " & Val(SlAc_ID) & " ,  Total_Reels = " & Val(vTotReel) & ", Total_Amount = " & Val(vTotamt) & ", Total_Weight = " & Val(vTotWgt) & ", Party_Bobin = " & Str(Val(txt_PartyBobin.Text)) & " , OurOwn_Bobin = " & Str(Val(txt_OurBobin.Text)) & ", Remarks = '" & Trim(txt_Remarks.Text) & "' , No_Stock_Posting = " & Str(Val(noStockpost)) & " , User_Idno = " & Val(Common_Procedures.User.IdNo) & " ,Frieght_2=" & Str(Val(txt_Frieght_After.Text)) & ",Frieght_2_Text='" & Trim(txt_Freight_Name.Text) & "',Total_Taxable_Value=" & Str(Val(lbl_TaxableValue.Text)) & ",Total_CGST_Amount=" & Str(Val(lbl_CGST_Amount.Text)) & ",Total_SGST_Amount=" & Str(Val(lbl_SGST_Amount.Text)) & ",Total_IGST_Amount=" & Str(Val(lbl_IGST_Amount.Text)) & ",Entry_VAT_GST_Type='" & TaxType & "',HSN_Code='" & Trim(lbl_Grid_HSNCode.Text) & "',Electronic_Reference_No ='" & Trim(txt_ElectronicRefNo.Text) & "'   ,Date_And_Time_Of_Supply ='" & Trim(txt_DateAndTimeOFSupply.Text) & "' ,Transport_Mode ='" & Trim(cbo_TransportMode.Text) & "' , Round_Off_Amount =" & Val(lbl_Roundoff.Text) & " , DeliveryTo_IdNo = " & Str(Val(vDelvTo_IdNo)) & ", GST_Percentage=" & Str(Val(lbl_Grid_GST_Perc.Text)) & " , Item_Group_id =" & Val(lbl_ItemGrp_ID.Text) & ",Net_Amount=" & Str(Val(CSng(lbl_Net_Amt.Text))) & " ,  Gr_Time = " & Str(Val(txt_GrTime.Text)) & ", Gr_Date = '" & Trim(vGrDt) & "' , Rate_For = '" & Trim(cbo_RateFor.Text) & "' ,E_Invoice_IRNO = '" & Trim(txt_eInvoiceNo.Text) & "' , E_Invoice_QR_Image =  @QrCode  , E_Invoice_ACK_No = '" & Trim(txt_eInvoiceAckNo.Text) & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & "  ,  E_Invoice_Cancelled_Status = " & eiCancel.ToString & " ,  E_Invoice_Cancellation_Reason = '" & Trim(txt_EInvoiceCancellationReson.Text) & "'  ,    EWB_No = '" & Trim(txt_eWayBill_No.Text) & "',EWB_Date = '" & Trim(txt_EWB_Date.Text) & "',EWB_Valid_Upto = '" & Trim(txt_EWB_ValidUpto.Text) & "',EWB_Cancelled = " & EWBCancel.ToString & " ,  EWBCancellation_Reason = '" & Trim(txt_EWB_Canellation_Reason.Text) & "'     Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"

                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bobin_Jari_Delivery_Code, Company_IdNo, for_OrderBy", tr)


            Partcls = "BobDelv : Dc.No. " & Trim(lbl_InvNo.Text)
            PBlNo = Trim(lbl_InvNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_InvNo.Text)

            cmd.CommandText = "Delete from Bobin_Jari_SalesDelivery_Bobin_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Bobin_Jari_Delivery_Jari_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_BobinDetails
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(DGVCol_BobinSalesDetails.Ends_Count).Value) <> "" And Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) <> 0 Then

                        Sno = Sno + 1

                        Ens_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(DGVCol_BobinSalesDetails.Ends_Count).Value, tr)

                        Clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(DGVCol_BobinSalesDetails.Colour).Value, tr)

                        BthSz_ID = Common_Procedures.BorderSize_NameToIdNo(con, .Rows(i).Cells(DGVCol_BobinSalesDetails.Border_Size).Value, tr)

                        BbnSz_Id = Common_Procedures.BobinSize_NameToIdNo(con, .Rows(i).Cells(DGVCol_BobinSalesDetails.Bobin_Size).Value, tr)

                        Nr = 0
                        cmd.CommandText = "Update  Bobin_Jari_SalesDelivery_Bobin_Details set Bobin_Jari_Delivery_Date = @EntryDate , Sl_No  = " & Str(Val(Sno)) & " , EndsCount_IdNo = " & Str(Val(Ens_ID)) & "  , Colour_IdNo = " & Str(Val(Clr_ID)) & "  , BorderSize_IdNo = " & Str(Val(BthSz_ID)) & " , Bobins = " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfBobins).Value) & " , Meter_Bobin = " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.MeterBobin).Value) & " , Meters = " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) & " , Meter_Reel = " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.MeterReel).Value) & " , Reel = " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value) & " , Rate = " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Rate).Value) & " , Amount = " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Amount).Value) & " , Bobin_Jari_Sales_Invoice_Code = '" & Trim(.Rows(i).Cells(DGVCol_BobinSalesDetails.Bobin_InvoiceCode).Value) & "' , Bobin_Size_IdNo = " & Str(Val(BbnSz_Id)) & "  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Bobin_Jari_Delivery_Bobin_Slno = " & Str(Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Details_SNo).Value))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Bobin_Jari_SalesDelivery_Bobin_Details ( Bobin_Jari_Delivery_Code, Company_IdNo                     , Bobin_Jari_Delivery_No       , for_OrderBy          , Bobin_Jari_Delivery_Date, Sl_No               , EndsCount_IdNo          , Colour_IdNo              , BorderSize_IdNo           , Bobins                                                                , Meter_Bobin                                                           , Meters                                                                      , Meter_Reel                                                                , Reel                                                                     , Rate                                                                 , Amount                                                                 ,  Bobin_Jari_Sales_Invoice_Code                                                   , Bobin_Size_IdNo             ) " &
                            "Values                                            ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate              ," & Str(Val(Sno)) & ", " & Str(Val(Ens_ID)) & ", " & Str(Val(Clr_ID)) & " , " & Str(Val(BthSz_ID)) & ", " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfBobins).Value) & ", " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.MeterBobin).Value) & ", " & Str(Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value)) & ", " & Str(Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.MeterReel).Value)) & ", " & Str(Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value)) & ", " & Str(Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Rate).Value)) & ", " & Str(Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Amount).Value)) & ", '" & Trim(.Rows(i).Cells(DGVCol_BobinSalesDetails.Bobin_InvoiceCode).Value) & "' , " & Str(Val(BbnSz_Id)) & "  )"
                            cmd.ExecuteNonQuery()

                        End If

                        If chk_NoStockPosting.Checked = False Then
                            cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo                     , Reference_No                 , for_OrderBy           , Reference_Date, DeliveryTo_Idno         , ReceivedFrom_Idno, Entry_ID             , Party_Bill_No        , Particulars            , Sl_No                , Empty_Cones, Empty_Bobin                                                           , EmptyBobin_Party                     , Empty_Jumbo) " &
                            "Values                                      ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate     , " & Str(Val(Led_ID)) & ", 4                , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", 0          , " & Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfBobins).Value) & ", " & Str(Val(txt_PartyBobin.Text)) & ", 0          )"
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo                     , Reference_No                 , for_OrderBy          , Reference_Date, DeliveryTo_Idno          , ReceivedFrom_Idno       , StockOf_IdNo            , Entry_ID             , Party_Bill_No        , Particulars            , Sl_No                , EndsCount_IdNo          , Colour_IdNo             , Bobins                                                                     , Meters                                                                       ) " &
                            "Values                        ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate    , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Ens_ID)) & ", " & Str(Val(Clr_ID)) & ", " & Str(Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfBobins).Value)) & ", " & Str(Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value)) & " )"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Bobin_Jari_SalesDelivery_Bobin_Details", "Bobin_Jari_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "EndsCount_IdNo, Colour_IdNo, BorderSize_IdNo, Bobins, Meter_Bobin, Meters, Meter_Reel, Reel, Rate, Amount", "Sl_No", "Bobin_Jari_Delivery_Code, For_OrderBy, Company_IdNo, Bobin_Jari_SalesDelivery_No, Bobin_Jari_SalesDelivery_Date, Ledger_Idno", tr)

            End With

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), tr)

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0


            Dim vNetAmt As String = Format(Val(CSng(lbl_Net_Amt.Text)), "#############0.00")
            Dim vCGSTAmt As String = Format(Val(CSng(lbl_CGST_Amount.Text)), "#############0.00")
            Dim vSGSTAmt As String = Format(Val(CSng(lbl_SGST_Amount.Text)), "#############0.00")
            Dim vIGSTAmt As String = Format(Val(CSng(lbl_IGST_Amount.Text)), "#############0.00")


            AcPos_ID = Led_ID

            vLed_IdNos = AcPos_ID & "|" & SlAc_ID & "|" & "24|25|26"

            vVou_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - (Val(vCGSTAmt) + Val(vSGSTAmt) + Val(vIGSTAmt)) & "|" & Val(vCGSTAmt) & "|" & Val(vSGSTAmt) & "|" & Val(vIGSTAmt)
            'vVou_Amts = -1 * Val(CSng(lbl_Net_Amt.Text)) & "|" & (Val(CSng(lbl_Net_Amt.Text)))

            If Common_Procedures.Voucher_Updation(con, "Bobin.Sale", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_InvNo.Text), Convert.ToDateTime(dtp_Date.Text), "Dc No : " & Trim(lbl_InvNo.Text) & ", Mtrs : " & Trim(Format(Val(vTotMtrs), "#########0.00")), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "Bobin.Dc.Frgt", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_InvNo.Text), Convert.ToDateTime(dtp_Date.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(dtp_Date.Text), AcPos_ID, Trim(lbl_InvNo.Text), 0, Val(CSng(lbl_Net_Amt.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If


            If Val(txt_OurBobin.Text) <> 0 Or Val(txt_PartyBobin.Text) Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Cones, Empty_Bobin, EmptyBobin_Party, Empty_Jumbo) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Led_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, 0, " & Str(Val(txt_OurBobin.Text)) & ", " & Str(Val(txt_PartyBobin.Text)) & ", 0)"
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_InvNo.Text)
                End If
            Else
                move_record(lbl_InvNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Total_Calculation()
        Dim vTotBbnS As Single, vTotMtrs As Single, vTotReel As Single, vTotAmt As Single
        Dim i As Integer
        Dim sno As Integer
        Dim AssAmt As Single = 0
        Dim CGSTAmt As Single = 0
        Dim SGSTAmt As Single = 0
        Dim IGSTAmt As Single = 0
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim ItmGrpID As Integer = 0
        Dim Count_Id As Integer = 0

        If FrmLdSTS = True Or NoCalc_Status = True Or MoveCal_sts = True Then Exit Sub

        Try

            vTotBbnS = 0 : vTotMtrs = 0 : vTotReel = 0 : vTotAmt = 0

            With dgv_BobinDetails
                For i = 0 To .Rows.Count - 1

                    sno = sno + 1

                    .Rows(i).Cells(0).Value = sno

                    If Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) <> 0 Then

                        vTotBbnS = vTotBbnS + Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfBobins).Value)
                        vTotMtrs = vTotMtrs + Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value)
                        vTotReel = vTotReel + Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value)
                        vTotAmt = vTotAmt + Val(.Rows(i).Cells(DGVCol_BobinSalesDetails.Amount).Value)

                    End If
                Next
            End With

            If dgv_BobinDetails_Total.Rows.Count <= 0 Then dgv_BobinDetails_Total.Rows.Add()
            dgv_BobinDetails_Total.Rows(0).Cells(5).Value = Val(vTotBbnS)
            dgv_BobinDetails_Total.Rows(0).Cells(6).Value = Format(Val(vTotMtrs), "#########0.00")
            dgv_BobinDetails_Total.Rows(0).Cells(8).Value = Val(vTotReel)
            dgv_BobinDetails_Total.Rows(0).Cells(10).Value = Format(Val(vTotAmt), "#########0.00")

            lbl_TaxableValue.Text = Format(Val(vTotAmt) + Val(txt_Freight.Text), "#########0.00")
            Net_Amount_Calculation()

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub Net_Amount_Calculation()
        Dim BlAmt As Double
        Dim AssAmt As Single = 0
        Dim CGSTAmt As Single = 0
        Dim SGSTAmt As Single = 0
        Dim IGSTAmt As Single = 0
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim Led_IdNo As Integer
        Dim ItmGrpID As Integer = 0
        Dim Count_Id As Integer = 0

        Dim TtAmt As Double
        Dim vStrNetAmt As String = ""
        If FrmLdSTS = True Or NoCalc_Status = True Or MoveCal_sts = True Then Exit Sub


        Try

            AssAmt = Val(lbl_TaxableValue.Text)

            'lbl_CGST_Amount.Text = 0
            'lbl_SGST_Amount.Text = 0
            'lbl_IGST_Amount.Text = 0

            If dgv_BobinDetails.Rows.Count > 0 Then
                Count_Id = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "EndsCount_Name = '" & Trim(dgv_BobinDetails.Rows(0).Cells(DGVCol_BobinSalesDetails.Ends_Count).Value) & "'"))

                lbl_ItemGrp_ID.Text = Val(Common_Procedures.get_FieldValue(con, "Count_Head", "ItemGroup_IdNo", "Count_IdNo = " & Val(Count_Id) & ""))

                lbl_Grid_HSNCode.Text = Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_HSN_Code", "ItemGroup_IdNo = '" & Trim(Val(lbl_ItemGrp_ID.Text)) & "'")

                lbl_Grid_GST_Perc.Text = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_GST_Percentage", "ItemGroup_IdNo = '" & Trim(Val(lbl_ItemGrp_ID.Text)) & "'"))
            End If

            If chk_GSTTax_Invocie.Checked = True Then

                Led_IdNo = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "Ledger_Name = '" & Trim(cbo_Ledger.Text) & "'"))
                Get_State_Code(Led_IdNo, Ledger_State_Code, Company_State_Code)

                '  lbl_Grid_GST_Perc.Text = 0

                ' lbl_Grid_HSNCode.Text = ""

                'HSN_GST_Details()

                If Trim(Company_State_Code) = Trim(Ledger_State_Code) Then
                    '-CGST 
                    lbl_CGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * (Val(lbl_Grid_GST_Perc.Text) / 2) / 100, "#########0.00")
                    '-SGST 
                    lbl_SGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * (Val(lbl_Grid_GST_Perc.Text) / 2) / 100, "#########0.00")

                ElseIf Trim(Company_State_Code) <> Trim(Ledger_State_Code) Then
                    '-IGST 
                    lbl_IGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * Val(lbl_Grid_GST_Perc.Text) / 100, "#########0.00")

                End If

            End If


            BlAmt = Val(lbl_TaxableValue.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text) + Val(txt_Frieght_After.Text)

            lbl_Net_Amt.Text = Format(Val(BlAmt), "#########0")
            '   lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Val(lbl_Net_Amt.Text))
            lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amt.Text)))


            '-------ROUNDOFF


            TtAmt = Format(Val(lbl_TaxableValue.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text) + Val(txt_Frieght_After.Text), "#########0.00")

            vStrNetAmt = Format(Val(TtAmt), "##########0.00")

            lbl_Roundoff.Text = Format(Val(CSng(lbl_Net_Amt.Text)) - Val(vStrNetAmt), "###########0.00")

            'lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amt.Text)))
            '-------


        Catch ex As Exception
            '----

            MessageBox.Show(ex.Message, "", Nothing)
        End Try

    End Sub

    Private Sub Meters_Calculation_111()
        Dim i As Integer
        Dim sno As Integer
        Dim vtotMtrs As Single
        Dim vtotReel As Single
        Dim vtotAmt As Single

        Try
            vtotMtrs = 0 : sno = 0 : vtotReel = 0 : vtotAmt = 0
            With dgv_BobinDetails
                For i = 0 To dgv_BobinDetails.Rows.Count - 1

                    sno = sno + 1

                    .Rows(i).Cells(DGVCol_BobinSalesDetails.SNo).Value = sno

                    vtotMtrs = Val(dgv_BobinDetails.Rows(i).Cells(DGVCol_BobinSalesDetails.MeterBobin).Value) * Val(dgv_BobinDetails.Rows(i).Cells(DGVCol_BobinSalesDetails.NoOfBobins).Value)
                    dgv_BobinDetails.Rows(i).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value = Format(Val(vtotMtrs), "#########0.00")

                Next
            End With

            Total_Calculation()

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub Grid_Single_Row_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Ends_Idno As Integer = 0
        Dim Tot_Meters As String = 0
        Dim vtotMtrs As String = 0
        Dim vNOOFREELS As String = 0
        Dim Ends As Single

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Or MoveCal_sts = True Then Exit Sub

            If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub

            With dgv_BobinDetails

                If .Visible Then

                    If .Rows.Count > 0 Then

                        If CurCol = DGVCol_BobinSalesDetails.NoOfBobins Or CurCol = DGVCol_BobinSalesDetails.MeterBobin Then

                            vtotMtrs = Val(dgv_BobinDetails.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.MeterBobin).Value) * Val(dgv_BobinDetails.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.NoOfBobins).Value)

                            dgv_BobinDetails.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value = Format(Val(vtotMtrs), "#########0.00")

                        End If

                        If CurCol = DGVCol_BobinSalesDetails.NoOfBobins Or CurCol = DGVCol_BobinSalesDetails.Total_Meter Or CurCol = DGVCol_BobinSalesDetails.MeterReel Or CurCol = DGVCol_BobinSalesDetails.NoOfReel Or CurCol = DGVCol_BobinSalesDetails.Rate Then

                            Ends = 0
                            If Trim(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.Ends_Count).Value) <> "" Then

                                Ends_Idno = Common_Procedures.EndsCount_NameToIdNo(con, Trim(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.Ends_Count).Value))

                                da = New SqlClient.SqlDataAdapter("Select a.Ends_Name from EndsCount_Head a  Where a.EndsCount_IdNo = " & Str(Val(Ends_Idno)), con)
                                dt = New DataTable
                                da.Fill(dt)

                                Tot_Meters = 0 : Ends = 0

                                If dt.Rows.Count > 0 Then
                                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                                        Ends = Val(dt.Rows(0).Item("Ends_Name").ToString)
                                    End If
                                End If

                                dt.Dispose()
                                da.Dispose()
                            End If

                            vNOOFREELS = 0
                            If Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) <> 0 Then
                                If Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.MeterReel).Value) > 0 Then
                                    If Trim(UCase(cbo_RateFor.Text)) = Trim(UCase("KG")) Then
                                        vNOOFREELS = Format(Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) / Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.MeterReel).Value), "#########0")
                                    Else
                                        vNOOFREELS = Format((Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) * Val(Ends)) / Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.MeterReel).Value), "#########0")
                                    End If
                                End If
                                '.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value = Format((Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) * Val(Ends)) / Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.MeterReel).Value), "#########0")
                            End If
                            .Rows(CurRow).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value = Format(Val(vNOOFREELS), "###########0")

                            .Rows(CurRow).Cells(DGVCol_BobinSalesDetails.Amount).Value = Format(Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value) * Val(.Rows(CurRow).Cells(DGVCol_BobinSalesDetails.Rate).Value), "#########0.00")

                            Total_Calculation()

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            '-----
        End Try
    End Sub


    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, txt_ElectronicRefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_ElectronicRefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        Try
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

        Catch ex As Exception
            '--------
        End Try

    End Sub

    Private Sub dgv_BobinDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEndEdit
        'Try
        '    With dgv_BobinDetails

        '        If .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.NoOfBobins Then
        '            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
        '                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
        '            End If
        '        End If

        '    End With

        'Catch ex As Exception
        '    '-----
        'End Try
    End Sub

    Private Sub dgv_BobinDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        Try

            With dgv_BobinDetails

                dgv_ActCtrlName = .Name.ToString

                If Val(.CurrentRow.Cells(DGVCol_BobinSalesDetails.SNo).Value) = 0 Then
                    .CurrentRow.Cells(DGVCol_BobinSalesDetails.SNo).Value = .CurrentRow.Index + 1
                End If

                If e.ColumnIndex = DGVCol_BobinSalesDetails.Ends_Count Then

                    If cbo_BobinEnds.Visible = False Or Val(cbo_BobinEnds.Tag) <> e.RowIndex Then

                        cbo_BobinEnds.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_BobinEnds.DataSource = Dt1
                        cbo_BobinEnds.DisplayMember = "EndsCount_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_BobinEnds.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_BobinEnds.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_BobinEnds.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_BobinEnds.Height = rect.Height  ' rect.Height

                        cbo_BobinEnds.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_BobinEnds.Tag = Val(e.RowIndex)
                        cbo_BobinEnds.Visible = True

                        cbo_BobinEnds.BringToFront()
                        cbo_BobinEnds.Focus()

                        'cbo_Grid_CountName.Visible = False
                        'cbo_Grid_MillName.Visible = False

                    End If

                Else
                    cbo_BobinEnds.Visible = False


                End If


                If e.ColumnIndex = DGVCol_BobinSalesDetails.Colour Then

                    If cbo_BobinColour.Visible = False Or Val(cbo_BobinColour.Tag) <> e.RowIndex Then

                        cbo_BobinColour.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_BobinColour.DataSource = Dt2
                        cbo_BobinColour.DisplayMember = "Colour_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_BobinColour.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_BobinColour.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_BobinColour.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_BobinColour.Height = rect.Height  ' rect.Height

                        cbo_BobinColour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_BobinColour.Tag = Val(e.RowIndex)
                        cbo_BobinColour.Visible = True

                        cbo_BobinColour.BringToFront()
                        cbo_BobinColour.Focus()

                    End If

                Else

                    'cbo_Grid_MillName.Tag = -1
                    'cbo_Grid_MillName.Text = ""
                    cbo_BobinColour.Visible = False

                End If

                If e.ColumnIndex = DGVCol_BobinSalesDetails.Border_Size Then

                    If cbo_BobinBorderSize.Visible = False Or Val(cbo_BobinBorderSize.Tag) <> e.RowIndex Then

                        cbo_BobinBorderSize.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select BorderSize_Name from BorderSize_Head order by BorderSize_Name", con)
                        Dt3 = New DataTable
                        Da.Fill(Dt3)
                        cbo_BobinBorderSize.DataSource = Dt3
                        cbo_BobinBorderSize.DisplayMember = "BorderSize_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_BobinBorderSize.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_BobinBorderSize.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_BobinBorderSize.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_BobinBorderSize.Height = rect.Height  ' rect.Height

                        cbo_BobinBorderSize.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_BobinBorderSize.Tag = Val(e.RowIndex)
                        cbo_BobinBorderSize.Visible = True

                        cbo_BobinBorderSize.BringToFront()
                        cbo_BobinBorderSize.Focus()



                    End If

                Else

                    cbo_BobinBorderSize.Visible = False


                End If


                If e.ColumnIndex = DGVCol_BobinSalesDetails.Bobin_Size Then

                    If cbo_Grid_Bobin_Size.Visible = False Or Val(cbo_Grid_Bobin_Size.Tag) <> e.RowIndex Then

                        cbo_Grid_Bobin_Size.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                        Dt4 = New DataTable
                        Da.Fill(Dt4)
                        cbo_Grid_Bobin_Size.DataSource = Dt4
                        cbo_Grid_Bobin_Size.DisplayMember = "EndsCount_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_Bobin_Size.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_Bobin_Size.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_Bobin_Size.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_Grid_Bobin_Size.Height = rect.Height  ' rect.Height

                        cbo_Grid_Bobin_Size.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_Bobin_Size.Tag = Val(e.RowIndex)
                        cbo_Grid_Bobin_Size.Visible = True

                        cbo_Grid_Bobin_Size.BringToFront()
                        cbo_Grid_Bobin_Size.Focus()

                        'cbo_Grid_CountName.Visible = False
                        'cbo_Grid_MillName.Visible = False

                    End If
                Else
                    cbo_Grid_Bobin_Size.Visible = False


                End If

                'If e.ColumnIndex = DGVCol_BobinSalesDetails.NoOfBobins Then
                '    'pnl_stock.Visible = True
                '    'pnl_Back.Visible = False
                '    Show_Bobin_CurrentStock()
                'End If



            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_BobinDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellLeave

        Try
            With dgv_BobinDetails

                If .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.Total_Meter Or .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.NoOfReel Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    End If
                End If

                If .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.NoOfBobins Then

                    pnl_stock.Visible = True
                    pnl_stock.Enabled = True
                    pnl_stock.BringToFront()
                    pnl_Back.Enabled = True

                    Show_Bobin_CurrentStock()
                Else
                    pnl_stock.Visible = False
                    pnl_stock.Enabled = False
                End If

            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_BobinDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellValueChanged
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Ends_Idno As Integer = 0
        Dim Tot_Meters As Single = 0
        Dim Ends As Single = 0

        Try
            If FrmLdSTS = True Then Exit Sub

            If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub

            With dgv_BobinDetails
                If .Visible Then
                    If .Rows.Count > 0 Then

                        Grid_Single_Row_Calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)

                        'If .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.NoOfBobins Or .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.MeterBobin Then
                        '    Meters_Calculation()
                        'End If

                        'If .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.NoOfBobins Or .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.Total_Meter Or .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.MeterReel Or .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.NoOfReel Or .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.Rate Then
                        '    Ends = 0
                        '    If Trim(.CurrentRow.Cells(DGVCol_BobinSalesDetails.Ends_Count).Value) <> "" Then
                        '        Ends_Idno = Common_Procedures.EndsCount_NameToIdNo(con, Trim(.CurrentRow.Cells(DGVCol_BobinSalesDetails.Ends_Count).Value))

                        '        da = New SqlClient.SqlDataAdapter("select a.Ends_Name from EndsCount_Head a  Where a.EndsCount_IdNo = " & Str(Val(Ends_Idno)), con)
                        '        dt = New DataTable
                        '        da.Fill(dt)

                        '        Tot_Meters = 0 : Ends = 0

                        '        If dt.Rows.Count > 0 Then
                        '            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        '                Ends = Val(dt.Rows(0).Item("Ends_Name").ToString)
                        '            End If
                        '        End If

                        '        dt.Dispose()
                        '        da.Dispose()
                        '    End If

                        '    If Val(.CurrentRow.Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) <> 0 Then

                        '        If Trim(UCase(cbo_RateFor.Text)) = Trim(UCase("KG")) Then
                        '            .Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) / Val(.Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.MeterReel).Value), "#########0")

                        '        Else
                        '            .Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value = Format((Val(.Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) * Val(Ends)) / Val(.Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.MeterReel).Value), "#########0")

                        '        End If
                        '        '.Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value = Format((Val(.Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) * Val(Ends)) / Val(.Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.MeterReel).Value), "#########0")

                        '    End If

                        '    .Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.Amount).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(DGVCol_BobinSalesDetails.Rate).Value), "#########0.00")

                        'End If

                        'Total_Calculation()

                    End If

                End If

            End With

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgv_BobinDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BobinDetails.EditingControlShowing
        Try
            dgtxt_BobinDetails = CType(dgv_BobinDetails.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_BobinDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BobinDetails.Enter
        Try
            dgv_ActCtrlName = dgv_BobinDetails.Name
            dgv_BobinDetails.EditingControl.BackColor = Color.Lime
            dgv_BobinDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_BobinDetails.SelectAll()
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_BobinDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BobinDetails.KeyPress

        Try
            With dgv_BobinDetails

                If Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = DGVCol_BobinSalesDetails.NoOfBobins Or Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = DGVCol_BobinSalesDetails.MeterBobin Or Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = DGVCol_BobinSalesDetails.Total_Meter Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_BobinDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BobinDetails.KeyUp
        Dim n As Integer = 0

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_BobinDetails

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else

                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                End With

                'Meters_Calculation()

            End If

        Catch ex As Exception
            '------
        End Try

    End Sub


    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_BobinDetails.RowsAdded
        Dim n As Integer = 0

        If FrmLdSTS = True Then Exit Sub
        Try
            If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
            With dgv_BobinDetails
                n = .RowCount
                .Rows(n - 1).Cells(DGVCol_BobinSalesDetails.SNo).Value = Val(n)
            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub dgv_BobinDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BobinDetails.LostFocus

        On Error Resume Next
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False

        'With dgv_BobinDetails

        '    If Asc(e.keychar) = 13 Then
        '        If Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = DGVCol_BobinSalesDetails.NoOfBobins Then

        '            pnl_stock.Visible = True
        '            pnl_Back.Visible = False

        '        End If
        '    End If

        'End With

    End Sub

    Private Sub cbo_BobinEnds_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinEnds.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "(Close_Status=0)", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Ends_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinEnds.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinEnds, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "(Close_Status=0)", "(EndsCount_IdNo = 0)")

        With dgv_BobinDetails

            If (e.KeyValue = 38 And cbo_BobinEnds.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    txt_Freight.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_BobinEnds.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    txt_Frieght_After.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinEnds.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinEnds, Nothing, "EndsCount_Head", "EndsCount_Name", "(Close_Status=0)", "(EndsCount_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_BobinDetails

                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_BobinEnds.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(DGVCol_BobinSalesDetails.Ends_Count).Value) = "" Then

                    txt_Frieght_After.Focus()


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If
    End Sub

    Private Sub cbo_Ends_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinEnds.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinEnds.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinEnds.TextChanged

        If FrmLdSTS = True Then Exit Sub
        Try
            If cbo_BobinEnds.Visible Then
                With dgv_BobinDetails
                    If IsNothing(.CurrentCell) Then Exit Sub
                    If .Rows.Count > 0 Then
                        If Val(cbo_BobinEnds.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.Ends_Count Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinEnds.Text)
                        End If
                    End If
                End With
            End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinColour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_BorderName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinColour.KeyDown
        Dim dep_idno As Integer = 0

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinColour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
            With dgv_BobinDetails

                If (e.KeyValue = 38 And cbo_BobinColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If

                If (e.KeyValue = 40 And cbo_BobinColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub cbo_BorderName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinColour.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinColour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                With dgv_BobinDetails
                    If .Rows.Count > 0 Then
                        .Focus()
                        .Rows(.CurrentCell.RowIndex).Cells.Item(DGVCol_BobinSalesDetails.Colour).Value = Trim(cbo_BobinColour.Text)
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                End With
            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_BorderName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinColour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinColour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BorderName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinColour.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub
            If cbo_BobinColour.Visible Then
                With dgv_BobinDetails
                    If IsNothing(.CurrentCell) Then Exit Sub
                    If .Rows.Count > 0 Then
                        If Val(cbo_BobinColour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.Colour Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinColour.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_BorderSize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinBorderSize.KeyDown

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinBorderSize, Nothing, Nothing, "BorderSize_Head", "BorderSize_Name", "", "(BorderSize_IdNo = 0)")

            With dgv_BobinDetails

                If (e.KeyValue = 38 And cbo_BobinBorderSize.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If

                If (e.KeyValue = 40 And cbo_BobinBorderSize.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub cbo_BorderSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinBorderSize.KeyPress

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinBorderSize, Nothing, "BorderSize_Head", "BorderSize_Name", "", "(BorderSize_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                With dgv_BobinDetails
                    If .Rows.Count > 0 Then
                        .Focus()
                        .Rows(.CurrentCell.RowIndex).Cells.Item(DGVCol_BobinSalesDetails.Border_Size).Value = Trim(cbo_BobinBorderSize.Text)
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End With

            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_BorderSize_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinBorderSize.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New BorderSize_Creation()

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinBorderSize.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BorderSize_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinBorderSize.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub
            If cbo_BobinBorderSize.Visible Then
                If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
                With dgv_BobinDetails

                    If Val(cbo_BobinBorderSize.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.Border_Size Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinBorderSize.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VechileNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VechileNo, cbo_Transport, txt_Freight, "Bobin_Jari_SalesDelivery_Head", "Vechile_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VechileNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VechileNo, txt_Freight, "Bobin_Jari_SalesDelivery_Head", "Vechile_No", "", "", False)
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_SalesAcc, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'If e.KeyValue = 40 And cbo_Transport.DroppedDown = False Then
        '    If dgv_BobinDetails.Rows.Count > 0 Then
        '        dgv_BobinDetails.Focus()
        '        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
        '        dgv_BobinDetails.CurrentCell.Selected = True

        '    Else
        '        txt_PartyBobin.Focus()

        '    End If
        'End If
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 And cbo_Transport.DroppedDown = False Then
        '    If dgv_BobinDetails.Rows.Count > 0 Then
        '        dgv_BobinDetails.Focus()
        '        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
        '        dgv_BobinDetails.CurrentCell.Selected = True

        '    Else
        '        txt_PartyBobin.Focus()

        '    End If
        'End If
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, proc_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            proc_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bobin_Jari_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Bobin_Jari_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bobin_Jari_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_EndsName.Text) <> "" Then
                proc_IdNo = Common_Procedures.Process_NameToIdNo(con, cbo_Filter_EndsName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If




            If Trim(cbo_Filter_EndsName.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Bobin_Jari_SalesDelivery_Head IN (select z1.Bobin_Jari_SalesDelivery_Head from Bobin_Jari_SalesDelivery_Bobin_Details z1 where z1.Ends = '" & Trim(cbo_Filter_EndsName.Text) & "')"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Bobin_Jari_SalesDelivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Jari_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Bobin_Jari_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Bobin_Jari_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Total_Bobins").ToString)
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")


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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_EndsName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub


    Private Sub cbo_Filter_EndsName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsName, dtp_Filter_ToDate, cbo_Filter_PartyName, "EndsCount_Head", "EndsCount_name", "", "(endsCount_iDNO = 0)")

    End Sub

    Private Sub cbo_Filter_ProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsName, cbo_Filter_PartyName, "endsCount_Head", "EndsCount_name", "", "(EndsCount_iDNO = 0)")
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


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        EMAIL_Status = False
        WHATSAPP_Status = False
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Bobin_Sales_delivery_Entry, New_Entry) = False Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1027" Then

            pnl_Back.Enabled = False

            pnl_Print.Visible = True

            If btn_Print_For_bobin.Enabled And btn_Print_For_bobin.Visible Then
                btn_Print_For_bobin.Focus()
            End If

        Else

            Print_Selection()

        End If






        'If pnl_Print.Visible <> False Then
        '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        '    Try

        '        da1 = New SqlClient.SqlDataAdapter("select * from Bobin_Jari_SalesDelivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        '        dt1 = New DataTable
        '        da1.Fill(dt1)

        '        If dt1.Rows.Count <= 0 Then

        '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub

        '        End If

        '        dt1.Dispose()
        '        da1.Dispose()

        '    Catch ex As Exception
        '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '    End Try

        '    'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        '    'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        '    'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next

        '    'prn_InpOpts = ""
        '    'prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
        '    'prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")


        '    If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

        '        Try


        '            If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
        '                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        '                    PrintDocument1.Print()
        '                End If

        '            Else
        '                PrintDocument1.Print()

        '            End If

        '        Catch ex As Exception
        '            MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '        End Try


        '    Else
        '        Try

        '            Dim ppd As New PrintPreviewDialog

        '            ppd.Document = PrintDocument1

        '            ppd.WindowState = FormWindowState.Maximized
        '            ppd.StartPosition = FormStartPosition.CenterScreen
        '            'ppd.ClientSize = New Size(600, 600)
        '            ppd.PrintPreviewControl.AutoZoom = True
        '            ppd.PrintPreviewControl.Zoom = 1.0

        '            ppd.ShowDialog()

        '        Catch ex As Exception
        '            MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '        End Try

        '    End If


        'End If



    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dtbl1 As New DataTable
        Dim nr As Integer = 0
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName , Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, f.Ledger_mainName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code from Bobin_Jari_SalesDelivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* ,  b.EndscOUNT_Name , c.Colour_name from Bobin_Jari_SalesDelivery_Bobin_Details a INNER JOIN EndscOUNT_Head b ON a.EndscOUNT_idno = b.endscOUNT_idno LEFT OUTER JOIN Colour_Head c ON a.Colour_idno = c.Colour_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName,lsh.State_Code as Ledger_State_Code,csh.State_Code as Company_State_Code from Bobin_Jari_SalesDelivery_Head a " &
                                             "INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo " &
                                              "INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo " &
                                              "INNER JOIN State_Head lsh ON c.Ledger_State_IdNo = lsh.State_IdNo " &
                                             "INNER JOIN State_Head csh ON b.Company_State_IdNo = csh.State_IdNo " &
                                            " Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)

                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Common_Procedures.settings.CustomerCode = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Printing_Delivery_Format_GST_1116(e)

        ElseIf Common_Procedures.settings.CustomerCode = "1027" Then
            If prn_Status = 1 Then
                Printing_Delivery_Format_GST2(e)
            End If
            If prn_Status = 2 Then

                Printing_Delivery_Format_GST_1027(e)
            End If


            '    pnl_Print.Visible = True
            '    pnl_Back.Enabled = False
            '    If btn_Print_For_bobin.Enabled And btn_Print_For_bobin.Visible Then
            '        btn_Print_For_bobin.Focus()

            '    Else btn_Print_for_reel.Enabled And btn_Print_for_reel.Visible Then
            '        btn_Print_for_reel.Focus()
            '    End If
            '    btn_Print_Cancel_Click(sender, e)
        Else

            Printing_Delivery_Format_GST2(e)

        End If


    End Sub

    Private Sub Printing_Delivery_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 40
            .Top = 10 ' 30
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

        NoofItems_PerPage = 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'ClAr(1) = 35
        'ClAr(2) = 120
        'ClAr(3) = 145
        'ClAr(4) = 60
        'ClAr(5) = 70
        'ClAr(6) = 85
        'ClAr(7) = 65
        'ClAr(8) = 70
        'ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        ClAr(1) = 35        'SNO
        ClAr(2) = 100       'PARTICULARS
        ClAr(3) = 70        'HSN CODE
        ClAr(4) = 55        'GST %
        ClAr(5) = 60        'ENDS/count
        ClAr(6) = 60       'Bobins
        ClAr(7) = 60       'Meter_Bobin
        ClAr(8) = 85       'Meters
        ClAr(9) = 65       'REEL
        ClAr(10) = 60      'RATE
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)) 'AMOUNT

        TxtHgt = 17.75 ' 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Delivery_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1, s2 As Single

        PageNo = PageNo + 1

        CurY = TMargin + 2

        da2 = New SqlClient.SqlDataAdapter("select a.* from " & Trim(Common_Procedures.EntryTempTable) & " a ", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            '  Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
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
        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BOBIN INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        'C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "INV.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        ''CurY = CurY + TxtHgt + 10
        ''If prn_HdDt.Rows(0).Item("Party_OrderNo").ToString <> "" Then
        ''    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        ''    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        ''    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        ''End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "VECHILE NO  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + s2 + 30, CurY, 0, 0, pFont)

        ' CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "ENDS/COUNT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt, 2, ClAr(8), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "ENDS/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt, 2, ClAr(8), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub
    Private Sub Printing_Delivery_Format_GST_1027(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ps As Printing.PaperSize
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer
        Dim vLine_Pen As Pen

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 55
            .Top = 20 ' 30
            .Bottom = 40
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

        NoofItems_PerPage = 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClAr(1) = 35        'SNO
        ClAr(2) = 385       'PARTICULARS

        'ClAr(3) = 170        'ENDS
        'ClAr(4) = 55       'Bobins
        'ClAr(5) = 60       'Meter_Bobin
        'ClAr(6) = 0       'Meters

        ClAr(3) = 80        'HSN CODE
        ClAr(4) = 55        'GST %

        ClAr(5) = 60       'REEL
        ClAr(6) = 55     'RATE
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)) 'AMOUNT

        TxtHgt = 16.75 '17.75 ' 18

        EntryCode = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vLine_Pen = New Pen(Color.Black, 2)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format_GST_1027_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format_GST_1027_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 2, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 5, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                        '' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

                        '----------
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, (Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)

                        '----------

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format_GST_1027_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Delivery_Format_GST_1027_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String, Cmp_UAMNO As String
        Dim S As String
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""

        PageNo = PageNo + 1
        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

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



        CurY = TMargin + 2

        p1Font = New Font("Calibri", 14, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

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
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_UAMNO = ""

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
            Cmp_UAMNO = "UDYAM No. : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
        End If


        'p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If

        CurY = CurY + TxtHgt - 15
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


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

        If Trim(Cmp_UAMNO) <> "" Then
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, p1Font)
        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY


        C1 = ClAr(1) + ClAr(2) - 30
        W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
        S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

        W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
        S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

        W3 = e.Graphics.MeasureString("INVOICE   DATE", pFont).Width
        S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)


        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "ELECTRONIC REF.NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
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
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, pFont)
            End If
        End If



        '--Right Side

        CurY2 = CurY2 + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY2 = CurY2 + strHeight
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)

        CurY2 = CurY2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont)
            End If
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
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + 20
        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DUE DAYS                              :    " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + C1 + 10, CurY, 0, 0, pFont)
        End If
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, pFont).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + W2 + strWidth + 60, CurY, 0, 0, pFont)
        'End If



        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + strWidth + W1 + 60, CurY, 0, 0, pFont)
        'End If

        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DOCUMENT THROUGH", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


        'If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "LC NO", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        '    If Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + strWidth + W2 + 60, CurY, 0, 0, pFont)
        '    End If
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), pFont)

        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY + TxtHgt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt)
        LnAr(10) = CurY
        CurY = CurY + TxtHgt + 10
        'Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)

        '---------LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS ", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

        CurY = CurY - 20

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        '---------

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        CurY = CurY + TxtHgt + TxtHgt + 20
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format_GST_1027_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim ItmNm1 As String = ""
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim SubClAr(15) As Single
        Dim p1Font As Font, p2Font As Font, p3Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))


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

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bobins").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + s2 + C1 + 20, CurY, 0, 0, pFont)

        CurY1 = CurY + 5
        p3Font = New Font("Calibri", 10, FontStyle.Bold)
        If BankNm1 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm2 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm3 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm4 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL AMOUNT :   ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 2, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL AMOUNT :   ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
        Balance_Calculation()

        'CurY = CurY + TxtHgt + 10
        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
        End If
        'CurY = CurY + TxtHgt

        If is_LastPage = True Then
            If Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Frieght_2_Text").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
        End If
        CurY = CurY + TxtHgt - 10
        '-------------------------------------------------------------------

        prn_CGST_Amount = prn_HdDt.Rows(0).Item("Total_CGst_Amount").ToString
        prn_SGST_Amount = prn_HdDt.Rows(0).Item("Total_SGst_Amount").ToString
        prn_IGST_Amount = prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString

        prn_GST_Perc = Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)


        If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, PageWidth, CurY)
            Else
                CurY = CurY + 10
            End If

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 2, CurY, 0, 0, pFont)
                '  Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p1Font)
            End If
        End If
        CurY = CurY + TxtHgt
        If Val(prn_CGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 2, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        End If
        CurY = CurY + TxtHgt
        If Val(prn_SGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 2, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt
        If Val(prn_IGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 2, CurY, 0, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        End If

        '***** GST END *****
        TtAmt = Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) + Val(prn_IGST_Amount) + Val(prn_SGST_Amount) + Val(prn_CGST_Amount) + Val(prn_HdDt.Rows(0).Item("Freight").ToString), "#########0.00")

        rndoff = 0
        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

        CurY = CurY + TxtHgt
        If Val(rndoff) <> 0 Then
            If Val(rndoff) >= 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (+) ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 2, CurY, 0, 0, pFont)

                '  Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 2, CurY, 0, 0, pFont)

                'Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + 60, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30 + 2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE AMOUNT ", LMargin + C1, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 30, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Amount, LMargin + s2 + C1 + 70, CurY, 0, 0, pFont)

        '  CurY = CurY + TxtHgt

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))

        CurY = CurY + 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim(BmsInWrds), "", "")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            BmsInWrds = Trim(UCase(BmsInWrds))
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        '=============GST SUMMARY============

        '  vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

        Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), Pens.Black)



        '==========================

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt

        p2Font = New Font("Webdings", 8, FontStyle.Bold)
        p1Font = New Font("Calibri", 8, FontStyle.Bold)


        ''1
        'If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        '    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 25, CurY, 0, 0, p1Font)
        'Else
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 25, CurY, 0, 0, p1Font)
        'End If
        '3
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods.", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

        '2
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit.", LMargin + 25, CurY, 0, 0, p1Font)
        '4
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction. ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY + 40, 0, 0, pFont)
        End If

        CurY = CurY + 5
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        '   CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub
    Private Sub Printing_Delivery_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bobins").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + C1, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + s2 + C1 + 10, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT :   " & prn_HdDt.Rows(0).Item("Total_Amount").ToString, PageWidth, CurY, 1, 0, p1Font)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL JUMPO ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Jumbos").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL CONES ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Cones").ToString, LMargin + s2 + C1 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL WEIGHT ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#######0.000"), LMargin + s2 + 30, CurY, 0, 0, pFont)
        Balance_Calculation()

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + 60, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BALANCE AMOUNT ", LMargin + C1, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Balance_Amount, LMargin + s2 + C1 + 70, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 2
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next
        'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS : " & vprn_BlNos, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "( " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(prn_HdDt.Rows(0).Item("User_IdNo").ToString)))) & " )", LMargin, CurY + 10, 2, PageWidth, p1Font)

        CurY = CurY + TxtHgt + 30

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)



        Common_Procedures.Print_To_PrintDocument(e, "Receiver Signature", LMargin + 5, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin, CurY, 2, PageWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Balance_Calculation()
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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        'da = New SqlClient.SqlDataAdapter("select max(Party_Rec_No) from Weaver_Payment_Head where Ledger_idno = " & Val(Led_ID) & " and Weaver_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'", con)
        'da.Fill(dt1)
        'NewNo = 0
        'If dt1.Rows.Count > 0 Then

        '    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
        '        NewNo = Val(dt1.Rows(0)(0).ToString)
        '    End If
        'End If

        'NewNo = NewNo + 1

        'lbl_PartyRecNo.Text = NewNo

        '-----------BALANCE

        da = New SqlClient.SqlDataAdapter("select  sum(a.voucher_amount) as amount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " ", con)
        Dtbl1 = New DataTable
        da.Fill(Dtbl1)

        Balance_Amount = ""
        Bal = 0
        If Dtbl1.Rows.Count > 0 Then
            For i = 0 To Dtbl1.Rows.Count - 1
                Amt = Val(Dtbl1.Rows(i).Item("amount").ToString)
                Balance_Amount = Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Cr", " Dr")

                Amt = Val(Dtbl1.Rows(i).Item("amount").ToString)
                Balance_Amount = Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Cr", " Dr")
            Next i
        End If


        '-------- Empty Bobin
        cmd.Connection = con

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, Name1, Int2) Select a.DeliveryTo_Idno, tP.Ledger_Name,  sum(a.Empty_BObin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and  (a.Empty_BObin) <> 0 group by a.DeliveryTo_Idno, tP.Ledger_Name having sum(a.Empty_BObin) <> 0 "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, Name1, Int2) Select a.ReceivedFrom_Idno, tP.Ledger_Name,  -1*sum(a.Empty_BObin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and (a.Empty_BObin) <> 0 group by a.ReceivedFrom_Idno, tP.Ledger_Name having sum(a.Empty_BObin) <> 0 "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, Name1, Int2) Select Int1, Name1,  sum(Int2) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1, Name1  having sum(Int2) <> 0 "
        cmd.ExecuteNonQuery()

        Balance_Bobin = 0

        da = New SqlClient.SqlDataAdapter("select Int1, name1, Int2 from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
        Dtbl1 = New DataTable
        da.Fill(Dtbl1)

        If Dtbl1.Rows.Count > 0 Then
            For i = 0 To Dtbl1.Rows.Count - 1
                Balance_Bobin = Val(Dtbl1.Rows(i).Item("Int2").ToString)
            Next i
        End If
        Dt.Dispose()
        da.Dispose()
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_PartyBobin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PartyBobin.KeyDown
        If e.KeyValue = 38 Then
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(DGVCol_BobinSalesDetails.Ends_Count)
                dgv_BobinDetails.CurrentCell.Selected = True

            Else
                txt_Freight.Focus()

            End If
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_PartyBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PartyBobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        Try
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then
                If dgv_BobinDetails.Rows.Count > 0 Then
                    dgv_BobinDetails.Focus()
                    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(DGVCol_BobinSalesDetails.Ends_Count)
                    dgv_BobinDetails.CurrentCell.Selected = True

                Else
                    txt_PartyBobin.Focus()

                End If
            End If

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
            If Asc(e.KeyChar) = 13 Then
                If dgv_BobinDetails.Rows.Count > 0 Then
                    dgv_BobinDetails.Focus()
                    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(DGVCol_BobinSalesDetails.Ends_Count)
                    dgv_BobinDetails.CurrentCell.Selected = True

                Else
                    txt_PartyBobin.Focus()

                End If
            End If

        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub txt_OutBobin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OurBobin.KeyDown
        Try
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then

                txt_Remarks.Focus()


            End If

        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub txt_OutBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OurBobin.KeyPress
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
            If Asc(e.KeyChar) = 13 Then

                txt_Remarks.Focus()


            End If

        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub cbo_KuriCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub dgtxt_BobinDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BobinDetails.KeyUp
        dgv_BobinDetails_KeyUp(sender, e)
    End Sub

    Private Sub chk_NoStockPosting_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_SalesAcc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAcc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAcc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAcc, cbo_DeliveryTo, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAcc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAcc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAcc, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAcc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAcc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_Ledger.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_InvoicePrefixNo.Focus()
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
            msk_Date.Text = Date.Today
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
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

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub chk_NoStockPosting_KeyPress1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_NoStockPosting.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub txt_Frieght_After_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Frieght_After.KeyDown
        If e.KeyValue = 38 Then
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(DGVCol_BobinSalesDetails.Ends_Count)
                dgv_BobinDetails.CurrentCell.Selected = True

            Else
                txt_Freight.Focus()
            End If
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Frieght_After_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Frieght_After.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        Net_Amount_Calculation()
    End Sub


    Private Sub Printing_Delivery_Format_GST2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ps As Printing.PaperSize
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer
        Dim vLine_Pen As Pen

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 55
            .Top = 20 ' 30
            .Bottom = 40
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

        NoofItems_PerPage = 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClAr(1) = 35        'SNO
        ClAr(2) = 100       'PARTICULARS

        ClAr(3) = 170        'ENDS
        ClAr(4) = 55       'Bobins
        ClAr(5) = 60       'Meter_Bobin
        ClAr(6) = 0       'Meters

        ClAr(7) = 80        'HSN CODE
        ClAr(8) = 55        'GST %

        ClAr(9) = 60       'REEL
        ClAr(10) = 55     'RATE
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)) 'AMOUNT

        TxtHgt = 16.75 '17.75 ' 18

        EntryCode = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vLine_Pen = New Pen(Color.Black, 2)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format_GST2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format_GST2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 2, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 5, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                        '' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

                        '----------
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, (Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

                        '----------

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format_GST2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Delivery_Format_GST2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String, Cmp_UAMNO As String
        Dim S As String
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""

        PageNo = PageNo + 1
        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

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



        CurY = TMargin + 2

        p1Font = New Font("Calibri", 14, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

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
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_UAMNO = ""

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
            Cmp_UAMNO = "UDYAM No. : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
        End If

        'p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If

        CurY = CurY + TxtHgt - 15
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


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

        If Trim(Cmp_UAMNO) <> "" Then
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, p1Font)
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30
        W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
        S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

        W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
        S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

        W3 = e.Graphics.MeasureString("INVOICE   DATE", pFont).Width
        S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)


        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "ELECTRONIC REF.NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
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
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, pFont)
            End If
        End If



        '--Right Side

        CurY2 = CurY2 + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY2 = CurY2 + strHeight
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)

        CurY2 = CurY2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont)
            End If
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
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + 20
        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DUE DAYS                              :    " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + C1 + 10, CurY, 0, 0, pFont)
        End If
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, pFont).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + W2 + strWidth + 60, CurY, 0, 0, pFont)
        'End If



        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + strWidth + W1 + 60, CurY, 0, 0, pFont)
        'End If

        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DOCUMENT THROUGH", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


        'If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "LC NO", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        '    If Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + strWidth + W2 + 60, CurY, 0, 0, pFont)
        '    End If
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), pFont)

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY + TxtHgt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt)
        LnAr(10) = CurY
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)

        '---------
        Common_Procedures.Print_To_PrintDocument(e, "ENDS ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BOBINS ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

        CurY = CurY - 20

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        '---------

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt + TxtHgt + 20
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format_GST2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim ItmNm1 As String = ""
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim SubClAr(15) As Single
        Dim p1Font As Font, p2Font As Font, p3Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))


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

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bobins").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + s2 + C1 + 20, CurY, 0, 0, pFont)

        CurY1 = CurY + 5
        p3Font = New Font("Calibri", 10, FontStyle.Bold)
        If BankNm1 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm2 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm3 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm4 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL AMOUNT :   ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
        Balance_Calculation()

        'CurY = CurY + TxtHgt + 10
        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
        End If
        'CurY = CurY + TxtHgt

        If is_LastPage = True Then
            If Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Frieght_2_Text").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
        End If
        CurY = CurY + TxtHgt - 10
        '-------------------------------------------------------------------

        prn_CGST_Amount = prn_HdDt.Rows(0).Item("Total_CGst_Amount").ToString
        prn_SGST_Amount = prn_HdDt.Rows(0).Item("Total_SGst_Amount").ToString
        prn_IGST_Amount = prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString

        prn_GST_Perc = Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)


        If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, PageWidth, CurY)
            Else
                CurY = CurY + 10
            End If

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
                '  Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p1Font)
            End If
        End If
        CurY = CurY + TxtHgt
        If Val(prn_CGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        End If
        CurY = CurY + TxtHgt
        If Val(prn_SGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt
        If Val(prn_IGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        End If

        '***** GST END *****
        TtAmt = Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) + Val(prn_IGST_Amount) + Val(prn_SGST_Amount) + Val(prn_CGST_Amount) + Val(prn_HdDt.Rows(0).Item("Freight").ToString), "#########0.00")

        rndoff = 0
        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

        CurY = CurY + TxtHgt
        If Val(rndoff) <> 0 Then
            If Val(rndoff) >= 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (+) ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)

                '  Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)

                'Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + 60, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE AMOUNT ", LMargin + C1, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 30, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Amount, LMargin + s2 + C1 + 70, CurY, 0, 0, pFont)

        '  CurY = CurY + TxtHgt

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

        CurY = CurY + 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim(BmsInWrds), "", "")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            BmsInWrds = Trim(UCase(BmsInWrds))
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        '=============GST SUMMARY============

        '  vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

        Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), Pens.Black)



        '==========================

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt

        p2Font = New Font("Webdings", 8, FontStyle.Bold)
        p1Font = New Font("Calibri", 8, FontStyle.Bold)


        ''1
        'If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        '    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 25, CurY, 0, 0, p1Font)
        'Else
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 25, CurY, 0, 0, p1Font)
        'End If
        '3
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods.", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

        '2
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit.", LMargin + 25, CurY, 0, 0, p1Font)
        '4
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction. ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY + 40, 0, 0, pFont)
        End If

        CurY = CurY + 5
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        '   CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub
    Private Sub Printing_Delivery_Format_GST1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ps As Printing.PaperSize
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 40
            .Top = 20 ' 30
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

        NoofItems_PerPage = 10   ' 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClAr(1) = 35        'SNO
        ClAr(2) = 100       'PARTICULARS

        ClAr(3) = 170        'ENDS
        ClAr(4) = 55       'Bobins
        ClAr(5) = 60       'Meter_Bobin
        ClAr(6) = 0       'Meters

        ClAr(7) = 80        'HSN CODE
        ClAr(8) = 55        'GST %

        ClAr(9) = 60       'REEL
        ClAr(10) = 55     'RATE
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)) 'AMOUNT

        TxtHgt = 17.75 ' 18

        EntryCode = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format_GST1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format_GST1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 2, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 5, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                        '' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

                        '----------
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, (Val(prn_DetDt.Rows(0).Item("Bobins").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_DetIndx).Item("HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_DetIndx).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

                        '----------

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format_GST1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Delivery_Format_GST1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single
        Dim W1 As Single
        Dim S1, s2 As Single
        Dim S As String
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0

        PageNo = PageNo + 1
        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

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



        CurY = TMargin + 2

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        da2 = New SqlClient.SqlDataAdapter("select a.* from " & Trim(Common_Procedures.EntryTempTable) & " a ", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            '  Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()
        pFont = New Font("Calibri", 10, FontStyle.Bold)
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY



        '------------
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

        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If

        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


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
        '--------------------

        CurY = CurY + TxtHgt - 5

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        'C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "INV.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "VECHILE NO  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "GSTIN  NO  :" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString & " ", LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "STATE      : " & Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Ledger_State_Idno").ToString)) & " ", LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "STATE CODE :" & prn_DetDt1.Rows(0).Item("Ledger_State_Code").ToString & " ", LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)

        '---------
        Common_Procedures.Print_To_PrintDocument(e, "ENDS ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BOBINS ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        '---------

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format_GST1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim ItmNm1 As String = ""
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim SubClAr(15) As Single
        Dim p1Font As Font, p2Font As Font, p3Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))


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

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bobins").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + s2 + C1 + 20, CurY, 0, 0, pFont)

        CurY1 = CurY + 5
        p3Font = New Font("Calibri", 10, FontStyle.Bold)
        If BankNm1 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm2 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm3 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm4 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL AMOUNT :   ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
        Balance_Calculation()

        'CurY = CurY + TxtHgt + 10
        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
        End If
        'CurY = CurY + TxtHgt

        If is_LastPage = True Then
            If Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Frieght_2_Text").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
        End If
        CurY = CurY + TxtHgt - 10
        '-------------------------------------------------------------------

        prn_CGST_Amount = prn_HdDt.Rows(0).Item("Total_CGst_Amount").ToString
        prn_SGST_Amount = prn_HdDt.Rows(0).Item("Total_SGst_Amount").ToString
        prn_IGST_Amount = prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString

        prn_GST_Perc = Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)


        If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, PageWidth, CurY)
            Else
                CurY = CurY + 10
            End If

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
                '  Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p1Font)
            End If
        End If
        CurY = CurY + TxtHgt
        If Val(prn_CGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        End If
        CurY = CurY + TxtHgt
        If Val(prn_SGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt
        If Val(prn_IGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        End If

        '***** GST END *****
        TtAmt = Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) + Val(prn_IGST_Amount) + Val(prn_SGST_Amount) + Val(prn_CGST_Amount) + Val(prn_HdDt.Rows(0).Item("Freight").ToString), "#########0.00")

        rndoff = 0
        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

        CurY = CurY + TxtHgt
        If Val(rndoff) <> 0 Then
            If Val(rndoff) >= 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (+) ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)

                '  Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)

                'Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + 60, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE AMOUNT ", LMargin + C1, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 30, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Amount, LMargin + s2 + C1 + 70, CurY, 0, 0, pFont)

        '  CurY = CurY + TxtHgt

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

        CurY = CurY + 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim(BmsInWrds), "", "")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            BmsInWrds = Trim(UCase(BmsInWrds))
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        '=============GST SUMMARY============

        '  vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

        Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), Pens.Black)



        '==========================

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt

        p2Font = New Font("Webdings", 8, FontStyle.Bold)
        p1Font = New Font("Calibri", 8, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile

            Common_Procedures.Print_To_PrintDocument(e, "Interest will be Charged at 24% P.A for the overdue payments from the Date of Invoice", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any delay , Loss Or Damage During the Transport", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Quality Complaint Will be accepted only in Grey Stage for Fabrics and Cotton Yarn Stage for Yarns", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Palladam jurisdiction Only", LMargin + 10, CurY, 0, 0, p1Font)

        Else

            ''1
            'If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
            '    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 25, CurY, 0, 0, p1Font)
            'Else
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 25, CurY, 0, 0, p1Font)
            'End If
            '3
            Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods.", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

            '2
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit.", LMargin + 25, CurY, 0, 0, p1Font)
            '4
            Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction. ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY + 40, 0, 0, pFont)
        End If

        CurY = CurY + 5
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        '   CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub
    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByRef CurY As Single, ByRef TopLnYAxis As Single, ByVal vLine_Pen As Pen)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim I As Integer = 0
        Dim p1Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim SNo As Integer = 0
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String = ""

        Try

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 110 : SubClAr(2) = 130 : SubClAr(3) = 48 : SubClAr(4) = 90 : SubClAr(5) = 48 : SubClAr(6) = 90 : SubClAr(7) = 48 : SubClAr(8) = 90
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            CurY = CurY + 5
            pFont = New Font("Calibri", 9, FontStyle.Bold)
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


            CurY = CurY - 15

            CurY = CurY + TxtHgt + 3
            pFont = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)

            Ttl_TaxAmt = Ttl_TaxAmt + Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString)
            Ttl_CGst = Ttl_CGst + Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString)
            Ttl_Sgst = Ttl_Sgst + Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString)
            Ttl_igst = Ttl_igst + Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)


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

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), TopLnYAxis)

            CurY = CurY + 5
            BmsInWrds = ""
            If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_RateFor, cbo_SalesAcc, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_SalesAcc, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
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
    Private Sub cbo_TransportMode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportMode.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Bobin_Jari_SalesDelivery_Head", "Transport_Mode", "", "")
    End Sub

    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportMode, cbo_DeliveryTo, txt_GrTime, "Bobin_Jari_SalesDelivery_Head", "Transport_Mode", "", "")
    End Sub

    Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportMode.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportMode, txt_GrTime, "Bobin_Jari_SalesDelivery_Head", "Transport_Mode", "", "", False)
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_InvoicePrefixNo.KeyDown
        On Error Resume Next
        ' If e.KeyValue = 38 Then txt_Packing.Focus()
        If e.KeyValue = 40 Then msk_Date.Focus()
    End Sub

    Private Sub txt_Frieght_After_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Frieght_After.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub txt_GrTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GrTime.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub txt_GrTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GrTime.TextChanged
        GraceTime_Calculation()
    End Sub
    Private Sub GraceTime_Calculation()

        msk_GrDate.Text = ""
        If IsDate(msk_Date.Text) = True And Val(txt_GrTime.Text) >= 0 Then
            msk_GrDate.Text = DateAdd("d", Val(txt_GrTime.Text), Convert.ToDateTime(msk_Date.Text))
        End If

    End Sub

    Private Sub msk_grDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_GrDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 38 Then
            e.Handled = True And e.SuppressKeyPress = True
            txt_GrTime.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True And e.SuppressKeyPress = True
            cbo_RateFor.Focus()
        End If

        vmskGrText = ""
        vmskGrStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskGrText = msk_GrDate.Text
            vmskGrStrt = msk_GrDate.SelectionStart
        End If

    End Sub

    Private Sub msk_GrDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_GrDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_RateFor.Focus()
        End If
    End Sub

    Private Sub msk_grDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_GrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_GrDate.Text = Date.Today
            msk_GrDate.SelectionStart = msk_GrDate.Text.Length
        End If
        If IsDate(msk_GrDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_GrDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_GrDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_GrDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_GrDate.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskGrText, vmskGrStrt)
        End If

    End Sub
    Private Sub dtp_GrDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_GrDate.ValueChanged
        msk_GrDate.Text = dtp_GrDate.Text
    End Sub

    Private Sub dtp_GrDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_GrDate.Enter
        msk_GrDate.Focus()
        msk_GrDate.SelectionStart = 0
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub cbo_Grid_Bobin_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Bobin_Size.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Bobin_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Bobin_Size.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Bobin_Size, Nothing, Nothing, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
        With dgv_BobinDetails
            If e.KeyCode = 38 And cbo_Grid_Bobin_Size.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then

                If .Visible Then
                    If .Rows.Count <= 1 Then
                        txt_Freight.Focus()
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    End If
                End If

            End If
            If e.KeyCode = 40 And cbo_Grid_Bobin_Size.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    Else
                        txt_Frieght_After.Focus()
                    End If
                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Bobin_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Bobin_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Bobin_Size, Nothing, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
        With dgv_BobinDetails

            If Asc(e.KeyChar) = 13 Then
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    Else
                        txt_Frieght_After.Focus()
                    End If
                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Bobin_Size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Bobin_Size.KeyUp
        If e.KeyCode = 17 And e.Control = True Then
            e.Handled = True
            Dim f As New Bobin_Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Bobin_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Bobin_Size_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Bobin_Size.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
            With dgv_BobinDetails
                If cbo_Grid_Bobin_Size.Visible Then
                    If Val(cbo_Grid_Bobin_Size.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DGVCol_BobinSalesDetails.Bobin_Size Then
                        .Rows(.CurrentRow.Index).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Bobin_Size.Text)
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Show_Bobin_CurrentStock()
        Dim vEndsCnt_IdNo As Integer
        Dim vLedID As Integer
        Dim CurStk As Decimal
        Dim Vdate As Date



        If Trim(cbo_BobinEnds.Text) <> "" Then
            vLedID = 4 ' Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            vEndsCnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_BobinEnds.Text)
            Vdate = dtp_Date.Value


            If Val(cbo_Ledger.Tag) <> Val(vLedID) Or Val(lbl_stock_bobin.Tag) <> Val(vEndsCnt_IdNo) Or Val(lbl_stock_meter.Tag) <> Val(vEndsCnt_IdNo) Then
                lbl_stock_bobin.Tag = 0
                lbl_stock_bobin.Text = ""

                lbl_stock_meter.Tag = 0
                lbl_stock_meter.Text = ""
                CurStk = 0
                If Val(vLedID) <> 0 And Val(vEndsCnt_IdNo) <> 0 Then
                    CurStk = Common_Procedures.get_Bobin_CurrentStock(con, Val(lbl_Company.Tag), vLedID, vEndsCnt_IdNo)
                    cbo_Ledger.Tag = Val(vLedID)

                    lbl_stock_bobin.Tag = Val(vEndsCnt_IdNo)
                    lbl_stock_bobin.Text = Val(CurStk)

                    lbl_stock_meter.Tag = Val(vEndsCnt_IdNo)
                    lbl_stock_meter.Text = Format(Val(CurStk), "#########0.00")
                End If
            End If

        Else
            cbo_Ledger.Tag = 0

        End If
    End Sub

    Private Sub btn_stock_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_stock_close.Click
        pnl_Back.Enabled = True
        pnl_stock.Visible = False
        Filter_Status = False
        ' Me.Close()
    End Sub



    Private Sub Printing_Delivery_Format_GST_1116(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ps As Printing.PaperSize
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer
        Dim vLine_Pen As Pen
        Dim S As String
        Dim vBrushClr As Brush

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 55
            .Top = 20 ' 30
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        ' prn_Count = prn_Count + 1
        'S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        NoofItems_PerPage = 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClAr(1) = 35        'SNO
        ClAr(2) = 100       'PARTICULARS

        ClAr(3) = 170        'ENDS
        ClAr(4) = 55       'Bobins
        ClAr(5) = 60       'Meter_Bobin
        ClAr(6) = 0       'Meters

        ClAr(7) = 80        'HSN CODE
        ClAr(8) = 55        'GST %

        ClAr(9) = 60       'REEL
        ClAr(10) = 55     'RATE
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)) 'AMOUNT

        TxtHgt = 17.75 ' 18

        EntryCode = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vLine_Pen = New Pen(Color.Black, 2)

        vBrushClr = Brushes.Black

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format_GST_1116_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format_GST_1116_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1

                        'If prn_OriDupTri_Count = 1 Then
                        '    vBrushClr = Brushes.Blue
                        'ElseIf prn_OriDupTri_Count = 2 Or prn_OriDupTri_Count = 4 Then
                        '    vBrushClr = Brushes.Blue
                        'ElseIf prn_OriDupTri_Count = 3 Then
                        '    vBrushClr = Brushes.Blue
                        'Else
                        '    vBrushClr = Brushes.Black
                        'End If
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont, vBrushClr)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont, vBrushClr)

                        '----------
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 5, CurY, 0, 0, pFont, vBrushClr)
                        Common_Procedures.Print_To_PrintDocument(e, (Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont, vBrushClr)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont, vBrushClr)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 3, CurY, 1, 0, pFont, vBrushClr)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont, vBrushClr)

                        '----------

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont, vBrushClr)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont, vBrushClr)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont, vBrushClr)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont, vBrushClr)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format_GST_1116_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)


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
                End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Delivery_Format_GST_1116_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim vBrushClr As Brush


        PageNo = PageNo + 1
        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        prn_OriDupTri_Count = 0

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                prn_OriDupTri_Count = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(prn_OriDupTri_Count) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR RECEIPIENT"
                ElseIf Val(prn_OriDupTri_Count) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(prn_OriDupTri_Count) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(prn_OriDupTri_Count) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If



        CurY = TMargin + 2

        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        'If prn_OriDupTri_Count = 1 Then
        '    vBrushClr = Brushes.Red
        'ElseIf prn_OriDupTri_Count = 2 Or prn_OriDupTri_Count = 4 Then
        '    vBrushClr = Brushes.Red
        'ElseIf prn_OriDupTri_Count = 3 Then
        '    vBrushClr = Brushes.Red
        'Else
        vBrushClr = Brushes.Black
        'End If

        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font, vBrushClr)

        '  End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont, vBrushClr)
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

        'p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If

        CurY = CurY + TxtHgt - 15

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        If Trim(Common_Procedures.settings.CustomerCode) = "1116" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Lourdu_matha_tex_logo, Drawing.Image), LMargin + 10, CurY, 110, 110)
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

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)

                        End If

                    End Using
                End If
            End If

        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, vBrushClr)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight - 7
        If Desc <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont, vBrushClr)
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
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, vBrushClr)
            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, vBrushClr)
            End If

        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont, vBrushClr)

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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, vBrushClr)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width

        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, vBrushClr)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, vBrushClr)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width

        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, vBrushClr)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font, vBrushClr)
            strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont, vBrushClr)
        End If

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then



            Dim ItmNm1 As String = ""
            Dim ItmNm2 As String = ""
            Dim I As Integer


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
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
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


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30
        W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
        S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

        W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
        S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

        W3 = e.Graphics.MeasureString("INVOICE   DATE", pFont).Width
        S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont, vBrushClr)


        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font, vBrushClr)
        Else

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font, vBrushClr)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont, vBrushClr)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont, vBrushClr)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font, vBrushClr)

        If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "ELECTRONIC REF.NO ", LMargin + C1 + 10, CurY, 0, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont, vBrushClr)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'LnAr(2) = CurY

        CurY1 = CurY
        CurY2 = CurY

        '---left side

        CurY1 = CurY1 + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font, vBrushClr)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY1 = CurY1 + strHeight
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font, vBrushClr)

        CurY1 = CurY1 + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, vBrushClr)

        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, vBrushClr)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, vBrushClr)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, vBrushClr)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont, vBrushClr)
        End If

        CurY1 = CurY1 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : ", LMargin + S1 + 10, CurY1, 0, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10 + 60, CurY1, 0, 0, pFont, vBrushClr)

        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, pFont, vBrushClr)
            End If
        End If



        '--Right Side

        CurY2 = CurY2 + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font, vBrushClr)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY2 = CurY2 + strHeight
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font, vBrushClr)

        CurY2 = CurY2 + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, vBrushClr)

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, vBrushClr)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, vBrushClr)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, vBrushClr)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, vBrushClr)
        End If

        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : ", LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10 + 60, CurY2, 0, 0, pFont, vBrushClr)

        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then

                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont, vBrushClr)
            End If
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
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont, vBrushClr)

        CurY = CurY + 20
        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "DUE DAYS                              :    ", LMargin + C1 + 10, CurY, 0, 0, pFont, vBrushClr)


            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + C1 + S2 + 30, CurY, 0, 0, pFont, vBrushClr)

        End If
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, pFont).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + W2 + strWidth + 60, CurY, 0, 0, pFont)
        'End If



        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + strWidth + W1 + 60, CurY, 0, 0, pFont)
        'End If

        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont, vBrushClr)




        Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont, vBrushClr)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DOCUMENT THROUGH", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


        'If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "LC NO", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        '    If Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + strWidth + W2 + 60, CurY, 0, 0, pFont)
        '    End If
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), pFont, vBrushClr)

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY + TxtHgt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt)
        LnAr(10) = CurY
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont, vBrushClr)

        'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)

        '---------
        Common_Procedures.Print_To_PrintDocument(e, "ENDS ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, "BOBINS ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont, vBrushClr)

        CurY = CurY - 20

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont, vBrushClr)
        '---------

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont, vBrushClr)

        CurY = CurY + TxtHgt + TxtHgt + 20
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format_GST_1116_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim ItmNm1 As String = ""
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim SubClAr(15) As Single
        Dim p1Font As Font, p2Font As Font, p3Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0
        Dim vBrushClr As Brush
        Dim S As String

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))


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

        CurY = CurY + TxtHgt - 10

        'If prn_OriDupTri_Count = 1 Then
        '    vBrushClr = Brushes.Red
        'ElseIf prn_OriDupTri_Count = 2 Or prn_OriDupTri_Count = 4 Then
        '    vBrushClr = Brushes.Red
        'ElseIf prn_OriDupTri_Count = 3 Then
        '    vBrushClr = Brushes.Red
        'Else
        vBrushClr = Brushes.Black
        'End If
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL BOBIN ", LMargin + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bobins").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont, vBrushClr)



        CurY1 = CurY + 5
        p3Font = New Font("Calibri", 10, FontStyle.Bold)
        If BankNm1 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font, vBrushClr)
        End If
        If BankNm2 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font, vBrushClr)
        End If
        If BankNm3 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font, vBrushClr)
        End If
        If BankNm4 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font, vBrushClr)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 90, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 30, CurY, 0, 0, pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Reels").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, vBrushClr)

        Balance_Calculation()

        'CurY = CurY + TxtHgt + 10
        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont, vBrushClr)

                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, vBrushClr)
            End If
        End If


        If is_LastPage = True Then
            If Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Frieght_2_Text").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont, vBrushClr)

                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, vBrushClr)
            End If
        End If
        CurY = CurY + TxtHgt - 10

        '-------------------------------------------------------------------

        prn_CGST_Amount = prn_HdDt.Rows(0).Item("Total_CGst_Amount").ToString
        prn_SGST_Amount = prn_HdDt.Rows(0).Item("Total_SGst_Amount").ToString
        prn_IGST_Amount = prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString

        prn_GST_Perc = Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)


        If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Frieght_2").ToString) <> 0 Then
                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, PageWidth, CurY)
            Else
                CurY = CurY + 15
            End If
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(7) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))

            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont, vBrushClr)

                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p1Font, vBrushClr)
            End If
        End If

        CurY = CurY + TxtHgt

        If Val(prn_CGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont, vBrushClr)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, vBrushClr)

        End If
        CurY = CurY + TxtHgt
        If Val(prn_SGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont, vBrushClr)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, vBrushClr)

        End If

        CurY = CurY + TxtHgt
        If Val(prn_IGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont, vBrushClr)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, vBrushClr)

        End If
        '***** GST END *****
        TtAmt = Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) + Val(prn_IGST_Amount) + Val(prn_SGST_Amount) + Val(prn_CGST_Amount) + Val(prn_HdDt.Rows(0).Item("Freight").ToString), "#########0.00")

        rndoff = 0
        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

        CurY = CurY + TxtHgt
        If Val(rndoff) <> 0 Then
            If Val(rndoff) >= 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (+) ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont, vBrushClr)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont, vBrushClr)

            End If
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, vBrushClr)
        End If

        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font, vBrushClr)

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY


        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN ", LMargin + 10, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 30, CurY, 0, 0, pFont, vBrushClr)
        Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + 60, CurY, 0, 0, pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 2, CurY, 0, 0, p1Font, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font, vBrushClr)

        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE AMOUNT ", LMargin + C1, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 30, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Amount, LMargin + s2 + C1 + 70, CurY, 0, 0, pFont)

        '  CurY = CurY + TxtHgt

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

        CurY = CurY + 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim(BmsInWrds), "", "")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            BmsInWrds = Trim(UCase(BmsInWrds))
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        'Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, BmsInWrds, LMargin + 10 + 210, CurY, 0, 0, p1Font, vBrushClr)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        '=============GST SUMMARY============

        '  vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) = "" Then

            Printing_GST_HSN_Details_Format1116(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), Pens.Black)

        End If

        '==========================

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font, vBrushClr)

        CurY = CurY + TxtHgt

        p2Font = New Font("Webdings", 8, FontStyle.Bold)
        p1Font = New Font("Calibri", 8, FontStyle.Bold)


        ''1
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 25, CurY, 0, 0, p1Font, vBrushClr)

        '3
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods.", PrintWidth / 2 + 25, CurY, 0, 0, p1Font, vBrushClr)

        '2
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit.", LMargin + 25, CurY, 0, 0, p1Font, vBrushClr)
        '4
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction. ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font, vBrushClr)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY + 40, 0, 0, pFont, vBrushClr)
        End If

        CurY = CurY + 5
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font, vBrushClr)
        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, vBrushClr)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont, vBrushClr)

        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont, vBrushClr)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont, vBrushClr)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub



    Private Sub Printing_GST_HSN_Details_Format1116(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByRef CurY As Single, ByRef TopLnYAxis As Single, ByVal vLine_Pen As Pen)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim I As Integer = 0
        Dim p1Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim SNo As Integer = 0
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String = ""
        Dim vBrushClr As Brush


        Try

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 110 : SubClAr(2) = 130 : SubClAr(3) = 48 : SubClAr(4) = 90 : SubClAr(5) = 48 : SubClAr(6) = 90 : SubClAr(7) = 48 : SubClAr(8) = 90
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            CurY = CurY + 5
            pFont = New Font("Calibri", 9, FontStyle.Bold)


            'If prn_OriDupTri_Count = 1 Then
            '    vBrushClr = Brushes.Red
            'ElseIf prn_OriDupTri_Count = 2 Or prn_OriDupTri_Count = 4 Then
            '    vBrushClr = Brushes.Red
            'ElseIf prn_OriDupTri_Count = 3 Then
            '    vBrushClr = Brushes.Red
            'Else
            vBrushClr = Brushes.Black
            'End If

            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin, CurY + 15, 2, SubClAr(1), pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 15, 2, SubClAr(2), pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont, vBrushClr)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY)
            LnAr2 = CurY
            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont, vBrushClr)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont, vBrushClr)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont, vBrushClr)

            Common_Procedures.Print_To_PrintDocument(e, "TAX AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont, vBrushClr)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)


            CurY = CurY - 15

            CurY = CurY + TxtHgt + 3
            pFont = New Font("Calibri", 9, FontStyle.Regular)

            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + 10, CurY, 0, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont, vBrushClr)

            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont, vBrushClr)

            Ttl_TaxAmt = Ttl_TaxAmt + Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString)
            Ttl_CGst = Ttl_CGst + Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString)
            Ttl_Sgst = Ttl_Sgst + Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString)
            Ttl_igst = Ttl_igst + Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont, vBrushClr)

            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont, vBrushClr)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont, vBrushClr)

            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont, vBrushClr)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), TopLnYAxis)

            CurY = CurY + 5
            BmsInWrds = ""
            If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            ' Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : ", LMargin + 10, CurY, 0, 0, p1Font, vBrushClr)

            Common_Procedures.Print_To_PrintDocument(e, BmsInWrds, LMargin + 10 + 160, CurY, 0, 0, p1Font, vBrushClr)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub btn_Print_For_bobin_Click(sender As Object, e As EventArgs) Handles btn_Print_For_bobin.Click
        prn_Status = 1
        Print_Selection()
        'Printing_Delivery_Format_GST2(e)
        btn_Close_Print_Click(sender, e)
        'Printing_Delivery_Format_GST2(e)

    End Sub

    Private Sub btn_Print_for_reel_Click(sender As Object, e As EventArgs) Handles btn_Print_for_reel.Click
        prn_Status = 2
        Print_Selection()
        'Printing_Delivery_Format_GST_1027(e)
        btn_Close_Print_Click(sender, e)
        '
    End Sub

    Private Sub btn_Print_Cancel_Click(sender As Object, e As EventArgs) Handles btn_Print_Cancel.Click
        btn_Close_Print_Click(sender, e)
    End Sub

    Private Sub btn_Close_Print_Click(sender As Object, e As EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub Print_Selection()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        Dim NewCode As String = ""
        Dim Def_PrntrNm As String = ""
        Dim vFILNm As String = ""
        Dim vFLPATH As String = ""
        Dim vPDFFLPATH_and_NAME As String = ""
        Dim vPRNTRNAME As String
        Dim vPARTYNM As String = ""

        'If pnl_Print.Visible <> False Then
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Bobin_Jari_SalesDelivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                If EMAIL_Status = False And WHATSAPP_Status = False Then
                    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next


        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then


            Try
                If Print_PDF_Status = True Then

                    vFLPATH = ""
                    vPRNTRNAME = Common_Procedures.get_PDF_PrinterName(EMAIL_Status, WHATSAPP_Status, vFLPATH)

                    If Trim(vPRNTRNAME) = "" Then
                        Exit Sub
                    End If

                    Def_PrntrNm = PrintDocument1.PrinterSettings.PrinterName

                    vFILNm = Trim("Invoice_" & Trim(lbl_InvNo.Text) & ".pdf")

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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                'ppd.ClientSize = New Size(600, 600)
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If



        'End If


        '    End If
    End Sub

    Private Sub Cbo_RateFor_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RateFor.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub Cbo_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RateFor.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RateFor, msk_GrDate, cbo_DeliveryTo, "", "", "", "")
    End Sub

    Private Sub Cbo_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RateFor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RateFor, cbo_DeliveryTo, "", "", "", "")
    End Sub

    Private Sub Cbo_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RateFor.TextChanged
        Try
            NoCalc_Status = True
            If cbo_RateFor.Visible = True And Trim(UCase(cbo_RateFor.Text)) = "KG" Then
                dgv_BobinDetails.Columns(DGVCol_BobinSalesDetails.Total_Meter).HeaderText = "TOTAL WEIGHT"

            Else
                dgv_BobinDetails.Columns(DGVCol_BobinSalesDetails.Total_Meter).HeaderText = "TOTAL METER"

            End If

            NoCalc_Status = False

        Catch ex As Exception
            '----

        End Try

        Grid_All_Row_Calculation()

    End Sub

    Private Sub Grid_All_Row_Calculation()
        Dim I As Integer = 0

        NoCalc_Status = False

        For I = 0 To dgv_BobinDetails.Rows.Count - 1
            If Val(dgv_BobinDetails.Rows(I).Cells(DGVCol_BobinSalesDetails.Total_Meter).Value) <> 0 Or Val(dgv_BobinDetails.Rows(I).Cells(DGVCol_BobinSalesDetails.MeterReel).Value) <> 0 Or Val(dgv_BobinDetails.Rows(I).Cells(DGVCol_BobinSalesDetails.NoOfReel).Value) <> 0 Then
                Grid_Single_Row_Calculation(I, DGVCol_BobinSalesDetails.MeterReel)
            End If
        Next

        NoCalc_Status = False

    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        EMAIL_Status = False
        WHATSAPP_Status = False
        print_record()
        'Print_PDF_Status = False
    End Sub

    Private Sub btn_SendSMS_Click(sender As Object, e As EventArgs) Handles btn_SendSMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", AgPNo As String = ""
        Dim Led_IdNo As Integer = 0, Agnt_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            If Trim(PhNo) = "" Then
                PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            End If

            smstxt = Trim(cbo_Ledger.Text) & vbCrLf
            smstxt = smstxt & " Inv No : " & Trim(txt_InvoicePrefixNo.Text) & Trim(lbl_InvNo.Text) & vbCrLf
            smstxt = smstxt & " Date : " & Trim(msk_Date.Text) & vbCrLf

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

            Common_Procedures.Print_OR_Preview_Status = 1
            Print_PDF_Status = True
            EMAIL_Status = False
            WHATSAPP_Status = True
            vEMAIL_Attachment_FileName = ""

            Print_Selection()

            EMAIL_Entry.vAttchFilepath = ""
            If Trim(vEMAIL_Attachment_FileName) <> "" Then
                If System.IO.File.Exists(vEMAIL_Attachment_FileName) = True Then
                    Sms_Entry.vAttchFilepath = Trim(vEMAIL_Attachment_FileName)
                End If
            End If


            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click

        btn_GENERATEEWB.Enabled = True
        btn_Generate_eInvoice.Enabled = True

        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - grp_EInvoice.Width) / 2
        Grp_EWB.Top = (Me.Height - grp_EInvoice.Height) / 2 + 200
    End Sub

    Private Sub btn_Get_QR_Code_Click(sender As Object, e As EventArgs) Handles btn_Get_QR_Code.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GetIRNDetails(txt_eInvoiceNo.Text, NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", "INV")


    End Sub
    Private Sub btn_refresh_Click(sender As Object, e As EventArgs) Handles btn_refresh.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select E_Invoice_QR_Image,E_Invoice_IRNO,E_Invoice_ACK_No,E_Invoice_ACK_Date,E_Invoice_Cancelled_Status FROM Bobin_Jari_SalesDelivery_Head WHERE Bobin_Jari_Delivery_Code = '" & NewCode & "'", con)

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
    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim VInvno As String = ""

        If Trim(txt_InvoicePrefixNo.Text) <> "" Then
            VInvno = Trim(txt_InvoicePrefixNo.Text) & "-" & Trim(lbl_InvNo.Text)
        Else
            VInvno = Trim(lbl_InvNo.Text)
        End If

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Bobin_Jari_SalesDelivery_Bobin_Details Where Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Bobin_Jari_SalesDelivery_Head Where Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) >0"
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


            'Dim vDocNo As String = ""
            'Dim vDocYs_No As String = ""
            'Dim InvSubNo As String = ""

            'Cmd.CommandText = "Select Bobin_Jari_Delivery_No from Bobin_Jari_SalesDelivery_Head Where Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"
            'vDocYs_No = Cmd.ExecuteScalar

            'If Common_Procedures.settings.CustomerCode = "1087" Then

            '    Cmd.CommandText = "Select Yarn_Sales_RefNo from Bobin_Jari_SalesDelivery_Head Where Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"
            '    vDocYs_No = Cmd.ExecuteScalar
            '    InvSubNo = Replace(Trim(vDocYs_No), Trim(Val(vDocYs_No)), "")

            '    vDocNo = Trim(txt_InvoicePrefixNo.Text) & Trim(Format(Val(vDocYs_No), "######0000")) & Trim(InvSubNo) & Trim(cbo_InvoiceSufixNo.Text)

            'Else

            '    'If Trim(txt_InvoicePrefixNo.Text) <> "" Then
            '    '    vDocNo = Trim(txt_InvoicePrefixNo.Text) & "-" & vDocYs_No
            '    'Else
            '    vDocNo = vDocYs_No
            '    'End If

            'End If

            'Cmd.CommandText = "Insert into e_Invoice_Head (     e_Invoice_No                  ,  e_Invoice_date ,        Buyer_IdNo,    Consignee_IdNo,     Assessable_Value  ,     CGST            ,   SGST            ,   IGST           ,    Cess,   State_Cess,         Round_Off         , Nett_Invoice_Value,         Ref_Sales_Code          , Other_Charges ,       Dispatcher_idno )" &
            '                  "Select                        Bobin_Jari_Delivery_No ,          Bobin_Jari_Delivery_Date,       Ledger_IdNo,        DeliveryTo_Idno,    Total_Taxable_Value,    Total_CGST_Amount,  Total_SGST_Amount,  Total_IGST_Amount,      0   ,   0          ,RoundOff_Amount    ,    Net_Amount          , '" & Trim(NewCode) & "',      ISNULL(TCS_Amount,0) , DeliveryTo_Idno from Bobin_Jari_SalesDelivery_Head where Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"


            Cmd.CommandText = "Insert into e_Invoice_Head (     e_Invoice_No        ,      e_Invoice_date         ,        Buyer_IdNo,         Consignee_IdNo,       Assessable_Value   ,          CGST            ,      SGST            ,      IGST           ,    Cess     ,   State_Cess  ,         Round_Off         , Nett_Invoice_Value      ,            Ref_Sales_Code   ,     Other_Charges   ,       Dispatcher_idno ) " &
                              "Select                        '" & Trim(VInvno) & "'  ,     Bobin_Jari_Delivery_Date,       Ledger_IdNo,        DeliveryTo_Idno,      Total_Taxable_Value  ,    Total_CGST_Amount   ,  Total_SGST_Amount    ,  Total_IGST_Amount,           0   ,        0      ,       Round_Off_Amount    ,    Net_Amount          ,      '" & Trim(NewCode) & "'  ,      Frieght_2    ,         DeliveryTo_Idno  From Bobin_Jari_SalesDelivery_Head where Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "' "

            Cmd.ExecuteNonQuery()

            Dim vPARTICULARS_FIELDNAME As String = ""

            Dim vUnit_FIELDNAME As String = ""
            Dim vQTY_FIELDNAME As String = ""


            If Trim(Common_Procedures.settings.CustomerCode) = "1116" Then
                vQTY_FIELDNAME = "REEL"
                vUnit_FIELDNAME = "OTH"

            Else
                vQTY_FIELDNAME = "Meters"
                vUnit_FIELDNAME = "MTR"
            End If

            'If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
            '    vPARTICULARS_FIELDNAME = "(c.Count_Name)"  '"(c.Count_Name + ' ' + Ig.Item_Hsn_Code)" ' "(c.Count_Name + ' ' + c.Count_Description)"
            'Else
            '    vPARTICULARS_FIELDNAME = "(c.Count_Name + ' ' + c.Count_Description + '-' + d.Mill_Name)"
            'End If

            'Cmd.CommandText = "Insert into e_Invoice_Details (Sl_No, IsService, Product_Description                               , HSN_Code, Batch_Details, Quantity, Unit,   Unit_Price,     Total_Amount                                                                                             ,                                Discount                    ,      Assessable_Amount                                                                                                            ,   GST_Rate      , SGST_Amount, IGST_Amount, CGST_Amount, Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails, Ref_Sales_Code )" &
            '                                        " Select a.Sl_No, 0,       " & vPARTICULARS_FIELDNAME & " as producDescription, a.HSN_Code, ''          , Weight ,'KGS', a.Rate      , (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Freight_Amount+b.Packing_Amount+b.AddLess_Amount) else 0 end )  ) , (CASE WHEN a.sl_no = 1 then b.Discount_Amount ELSE 0 END ) ,  (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Freight_Amount+b.Packing_Amount+b.AddLess_Amount-b.Discount_Amount ) else 0 end )  )  , a.GST_Percentage, 0          , 0          , 0          , 0        , 0          , 0                , 0              , 0                , 0                     , 0           ,   0             , ''               ,'" & Trim(NewCode) & "' " &
            '                                        " from Bobin_Jari_SalesDelivery_Bobin_Details a " &
            '                                        " INNER JOIN Bobin_Jari_SalesDelivery_Head b  ON a.Bobin_Jari_Delivery_Code =  b.Bobin_Jari_Delivery_Code " &
            '                                        " inner join Count_head C on a.Count_IdNo = c.Count_IdNo " &
            '                                        " inner join Mill_head d on a.Mill_IdNo = d.Mill_IdNo LEFT OUTER JOIN Itemgroup_head Ig on ig.ItemGroup_IdNo = c.ItemGroup_IdNo  " &
            '                                        " Where a.Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"




            Cmd.CommandText = "Insert into e_Invoice_Details (  Sl_No  ,    IsService  ,            Product_Description             ,  HSN_Code ,    Batch_Details         ,          Quantity          ,           Unit            ,  Unit_Price    ,                       Total_Amount                                  ,    Discount   ,                        Assessable_Amount                             ,    GST_Rate      , SGST_Amount, IGST_Amount, CGST_Amount, Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails, Ref_Sales_Code ) " &
                                                    "Select    a.Sl_No ,       0       ,    ED.EndsCount_Name as producDescription , b.HSN_Code, ''   AS Batch_Details     , " & Trim(vQTY_FIELDNAME) & "   ,'" & Trim(vUnit_FIELDNAME) & "' ,    a.Rate      , (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Freight) else 0 end )  ) , 0 AS DISCOUNT ,  (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Freight) else 0 end )  )  , b.GST_Percentage ,    0      ,     0      , 0          , 0        , 0          , 0                , 0              , 0                , 0                     , 0           ,   0             , ''               , '" & Trim(NewCode) & "' " &
                                                    "from Bobin_Jari_SalesDelivery_Bobin_Details a " &
                                                    "INNER JOIN Bobin_Jari_SalesDelivery_Head b  ON a.Bobin_Jari_Delivery_Code =  b.Bobin_Jari_Delivery_Code  " &
                                                    "LEFT OUTER JOIN EndsCount_Head ED ON a.EndsCount_IdNo=ed.EndsCount_IdNo " &
                                                    "LEFT OUTER JOIN Count_head C on ED.Count_IdNo = c.Count_IdNo  " &
                                                    "LEFT OUTER JOIN Itemgroup_head Ig on ig.ItemGroup_IdNo = c.ItemGroup_IdNo  " &
                                                    "Where a.Bobin_Jari_Delivery_Code =  '" & Trim(NewCode) & "' "


            Cmd.ExecuteNonQuery()



            tr.Commit()


        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try

        btn_Generate_eInvoice.Enabled = False

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", Pk_Condition)

    End Sub

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        If Len(Trim(txt_EInvoiceCancellationReson.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)

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
    Private Sub btn_Generate_EWB_Click(sender As Object, e As EventArgs) Handles btn_Generate_EWB_IRN.Click
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Bobin_Jari_SalesDelivery_Bobin_Details Where Bobin_Jari_Delivery_Code = '" & NewCode & "'"
        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Bobin_Jari_SalesDelivery_Head Where Bobin_Jari_Delivery_Code = '" & NewCode & "' and (Len(Electronic_Reference_No) >0 or Len(E_Invoice_IRNO) = 0 OR E_Invoice_IRNO IS NULL )"
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


            Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]         ,     	[TransID]            ,    [TransMode]  ,	[TransDocNo]       , [TransDocDate] ,	[VehicleNo]        ,                [Distance]                                              ,	[VehType] ,	[TransName]         ,    [InvCode]           ,  Company_Idno ,     Company_Pincode,                                           Shipped_To_Idno                        ,                                       Shipped_To_Pincode               ) " &
                                " Select                A.E_Invoice_IRNO  ,  ISNULL(t.Ledger_GSTINNo, '' ) ,        '1'    ,        NULL as lr_no   ,   Null   as lr_date      ,       a.Vechile_No , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  D.Distance ELSE L.Distance END),      'R'    ,  t.Ledger_Mainname  , '" & Trim(NewCode) & "' , tZ.Company_IdNo, tZ.Company_PinCode, (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  a.DeliveryTo_IdNo ELSE a.Ledger_IdNo END), (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  D.Pincode ELSE L.Pincode END)    " &
                                    " from Bobin_Jari_SalesDelivery_Head a INNER JOIN Company_Head tZ on a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D on a.DeliveryTo_IdNo = D.Ledger_IdNo LEFT OUTER JOIN Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Where a.Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"

            Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()

            MessageBox.Show(ex.Message + " Cannot Generate IRN.", "ERROR WHILE GENERATING E-WAY BILL BY IRN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Exit Sub

        End Try

        btn_Generate_EWB_IRN.Enabled = False

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateEWBByIRN(NewCode, rtbeInvoiceResponse, txt_eWayBill_No, txt_EWB_Date, txt_EWB_ValidUpto, con, "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", txt_EWB_Canellation_Reason, txt_EWB_Cancel_Status, Pk_Condition)

        Cmd.CommandText = "DELETE FROM EWB_By_IRN WHERE INVCODE = '" & NewCode & "'"
        Cmd.ExecuteNonQuery()

    End Sub
    Private Sub btn_Print_EWB_Click(sender As Object, e As EventArgs) Handles btn_Print_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_ElectronicRefNo.Text, rtbeInvoiceResponse, 0)
    End Sub
    Private Sub btn_Detail_PRINT_EWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINT_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_ElectronicRefNo.Text, rtbEWBResponse, 1)
    End Sub
    Private Sub btn_Cancel_EWB_Click(sender As Object, e As EventArgs) Handles btn_Cancel_EWB.Click

        If Len(Trim(txt_EWB_Canellation_Reason.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))

        einv.Cancel_EWB_IRN(NewCode, txt_eWayBill_No.Text, rtbeInvoiceResponse, txt_EWB_Cancel_Status, con, "Bobin_Jari_SalesDelivery_Head", "Bobin_Jari_Delivery_Code", txt_EWB_Canellation_Reason.Text)
    End Sub
    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub
    Private Sub btn_EInvoice_Generation_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click

        btn_GENERATEEWB.Enabled = True
        btn_Generate_eInvoice.Enabled = True


        grp_EInvoice.Visible = True
        grp_EInvoice.BringToFront()
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2

        btn_CheckConnectivity1.Enabled = False
        btn_CheckConnectivity1.Visible = False

    End Sub

    Private Sub txt_eWayBill_No_TextChanged(sender As Object, e As EventArgs) Handles txt_eWayBill_No.TextChanged
        txt_ElectronicRefNo.Text = txt_eWayBill_No.Text

    End Sub

    Private Sub txt_ElectronicRefNo_TextChanged(sender As Object, e As EventArgs) Handles txt_ElectronicRefNo.TextChanged

        txt_eWayBill_No.Text = txt_ElectronicRefNo.Text
        txt_EWBNo.Text = txt_ElectronicRefNo.Text

    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()


        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim VInvno As String = ""

        If Trim(txt_InvoicePrefixNo.Text) <> "" Then
            VInvno = Trim(txt_InvoicePrefixNo.Text) & "-" & Trim(lbl_InvNo.Text)
        Else
            VInvno = Trim(lbl_InvNo.Text)
        End If

        Dim da As New SqlClient.SqlDataAdapter("Select Electronic_Reference_No from Bobin_Jari_SalesDelivery_Head where Bobin_Jari_Delivery_Code = '" & NewCode & "'", con)
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
            '             "  SELECT               'O'              , '1'             ,   ''              ,    'INV'    , '" & Trim(VInvno) & "' ,a.Bobin_Jari_Delivery_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
            '             " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo  ,L.Ledger_Name,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,L.City_Town,L.Pincode, TS.State_Code,TS.State_Code," &
            '             " 1                     ,a.AddLess_Amount + a.RoundOff_Amount, A.Total_Taxable_Value    , A.Total_CGST_Amount  ,  A.Total_SGST_Amount , A.Total_IGST_Amount   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
            '             " a.LR_No        ,         a.Lr_Date            , a.Net_Amount         ,     CASE    WHEN a.Transport_Mode = 'Rail' THEN '2'  WHEN a.Transport_Mode = 'Air' THEN '3'  WHEN a.Transport_Mode = 'Ship' THEN '4'    ELSE '1' END AS TrMode ," &
            '             " a.Vehicle_No,'R','" & NewCode & "' from Bobin_Jari_SalesDelivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
            '             " Inner Join Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Left Outer Join State_Head FS On " &
            '             " C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  where a.Bobin_Jari_Delivery_Code = '" & NewCode & "'"




            CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]     ,   [DocType]   ,	    [EWBGenDocNo]       ,        [EWBDocDate]            ,    [FromGSTIN]     , [FromTradeName]   ,	             [FromAddress1]			  ,				[FromAddress2]			,		  [FromPlace]  ,  [FromPINCode]       ,		[FromStateCode] ,	[ActualFromStateCode]   ,		[ToGSTIN]       ,	   [ToTradeName]	    ,             [ToAddress1]      ,                   [ToAddress2]        ,    [ToPlace]    ,   [ToPINCode]  ,  [ToStateCode] , [ActualToStateCode] ,[TransactionType]	    ,	   [OtherValue]     ,	 [Total_value]         ,   [CGST_Value]        ,   [SGST_Value]       ,     [IGST_Value]      ,		[CessValue]	,[CessNonAdvolValue],	[TransporterID]    , [TransporterName]	,	[TransportDOCNo] ,[TransportDOCDate]    , [TotalInvValue]       ,														[TransMode]																					             ,  [VehicleNo]    ,[VehicleType]  ,     [InvCode])     " &
                                   "SELECT                 'O'        ,       '1'       ,       ''         ,    'INV'      ,    '" & Trim(VInvno) & "'  ,  a.Bobin_Jari_Delivery_Date    , C.Company_GSTINNo  , C.Company_Name    ,	C.Company_Address1+C.Company_Address2 , c.Company_Address3+C.Company_Address4,		C.Company_City , C.Company_PinCode    ,		 FS.State_Code  ,		FS.State_Code		 ,L.Ledger_GSTINNo    ,			L.Ledger_Name		,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,  L.City_Town     ,   L.Pincode   , TS.State_Code ,      TS.State_Code,			1               ,	Frieght_2           , A.Total_Taxable_Value    ,  A.Total_CGST_Amount  ,  A.Total_SGST_Amount , A.Total_IGST_Amount   ,		   0		,         0         ,	t.Ledger_GSTINNo   ,	 t.Ledger_Name	 ,		 0 AS LR_NO  ,  NULL AS LR_DATE    ,	 a.Net_Amount       ,     CASE    WHEN a.Transport_Mode = 'Rail' THEN '2'  WHEN a.Transport_Mode = 'Air' THEN '3'  WHEN a.Transport_Mode = 'Ship' THEN '4'    ELSE '1' END AS TrMode , a.Vechile_No    ,        'R'    , '" & NewCode & "' " &
                                "from Bobin_Jari_SalesDelivery_Head a    " &
                                "inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo    " &
                                "Inner Join Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo   " &
                                "left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo   " &
                                "Left Outer Join State_Head FS On  C.Company_State_IdNo = fs.State_IdNo  " &
                                "left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  " &
                                "where a.Bobin_Jari_Delivery_Code = 'GBSDL-2-5001/23-24' "





        Else

            'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
            '             "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
            '             "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
            '             "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
            '             "[VehicleNo]      ,[VehicleType]   , [InvCode]) " &
            '             " " &
            '             " " &
            '             "  SELECT               'O'              , '1'             ,   ''              ,    'INV'    ,  a.Yarn_Sales_No , a.Yarn_Sales_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
            '             " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo  ,L.Ledger_Name,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,L.City_Town,L.Pincode, TS.State_Code,TS.State_Code," &
            '             " 1                     ,a.AddLess_Amount + a.RoundOff_Amount, A.Total_Taxable_Value    , A.Total_CGST_Amount  ,  A.Total_SGST_Amount , A.Total_IGST_Amount   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
            '             " a.LR_No        , a.Lr_Date         , a.Net_Amount         ,     CASE    WHEN a.Transport_Mode = 'Rail' THEN '2'  WHEN a.Transport_Mode = 'Air' THEN '3'  WHEN a.Transport_Mode = 'Ship' THEN '4'    ELSE '1' END AS TrMode ," &
            '             " a.Vehicle_No,'R','" & NewCode & "' from Bobin_Jari_SalesDelivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
            '             " Inner Join Ledger_Head L on a.DeliveryTo_IdNo = L.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Left Outer Join State_Head FS On " &
            '             " C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  where a.Bobin_Jari_Delivery_Code = '" & NewCode & "'"


            CMD.CommandText = "Insert into EWB_Head ([SupplyType]    ,[SubSupplyType]  , [SubSupplyDesc]  ,   [DocType]  ,    	[EWBGenDocNo]       ,       [EWBDocDate]            ,    [FromGSTIN]     , [FromTradeName]   ,	             [FromAddress1]			  ,				[FromAddress2]			,		  [FromPlace]  ,  [FromPINCode]       ,		[FromStateCode] ,	[ActualFromStateCode]   ,		[ToGSTIN]       ,	   [ToTradeName]	    ,             [ToAddress1]      ,                   [ToAddress2]        ,    [ToPlace]    ,   [ToPINCode]  ,  [ToStateCode] , [ActualToStateCode] ,[TransactionType]	    ,	   [OtherValue]     ,	 [Total_value]         ,   [CGST_Value]        ,   [SGST_Value]       ,     [IGST_Value]      ,		[CessValue]	,[CessNonAdvolValue],	[TransporterID]    , [TransporterName]	,	[TransportDOCNo] ,[TransportDOCDate]    , [TotalInvValue]       ,														[TransMode]																					             ,  [VehicleNo]    ,[VehicleType]  ,     [InvCode])     " &
                                    "SELECT               'O'        ,       '1'       ,       ''         ,    'INV'     ,   '" & Trim(VInvno) & "' , a.Bobin_Jari_Delivery_Date    , C.Company_GSTINNo  , C.Company_Name    ,	C.Company_Address1+C.Company_Address2 , c.Company_Address3+C.Company_Address4,		C.Company_City , C.Company_PinCode    ,		 FS.State_Code  ,		FS.State_Code		 ,L.Ledger_GSTINNo    ,			L.Ledger_Name		,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,  L.City_Town     ,   L.Pincode   , TS.State_Code ,      TS.State_Code,			1               ,	Frieght_2           , A.Total_Taxable_Value    ,  A.Total_CGST_Amount  ,  A.Total_SGST_Amount , A.Total_IGST_Amount   ,		   0		,         0         ,	t.Ledger_GSTINNo   ,	 t.Ledger_Name	 ,		 NULL AS LR_NO  ,  NULL AS LR_DATE    ,	 a.Net_Amount       ,     CASE    WHEN a.Transport_Mode = 'Rail' THEN '2'  WHEN a.Transport_Mode = 'Air' THEN '3'  WHEN a.Transport_Mode = 'Ship' THEN '4'    ELSE '1' END AS TrMode , a.Vechile_No    ,        'R'    , '" & NewCode & "'    " &
                                    "from Bobin_Jari_SalesDelivery_Head a    " &
                                    "inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo   " &
                                    "Inner Join Ledger_Head L on a.DeliveryTo_IdNo = L.Ledger_IdNo  " &
                                    "left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo  " &
                                    "Left Outer Join State_Head FS On  C.Company_State_IdNo = fs.State_IdNo  " &
                                    "left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo   " &
                                    "where a.Bobin_Jari_Delivery_Code = '" & NewCode & "'  "



        End If

        CMD.ExecuteNonQuery()

        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim dt1 As New DataTable

        Dim vPARTICULARS_FIELDNAME As String = ""

        Dim vUnit_FIELDNAME As String = ""
        Dim vQTY_FIELDNAME As String = ""


        If Trim(Common_Procedures.settings.CustomerCode) = "1116" Then
            vQTY_FIELDNAME = "REEL"
            vUnit_FIELDNAME = "OTH"

        Else
            vQTY_FIELDNAME = "Meters"
            vUnit_FIELDNAME = "MTR"
        End If


        da = New SqlClient.SqlDataAdapter("Select Min(Sl_No) AS SL_NO, ED.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.Item_GST_Percentage,sum(SD.Amount) As TaxableAmt,sum(SD." & Trim(vQTY_FIELDNAME) & ") as Qty ,'" & Trim(vUnit_FIELDNAME) & "' AS Units  " &
                                          "from Bobin_Jari_SalesDelivery_Bobin_Details SD  LEFT OUTER JOIN EndsCount_Head ED ON SD.EndsCount_IdNo=ed.EndsCount_IdNo  Inner Join Count_Head I On ED.Count_IdNo = I.Count_IdNo   Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo  " &
                                          "Where SD.Bobin_Jari_Delivery_Code = '" & NewCode & "'  " &
                                          "Group By  ED.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage ", con)


        'da = New SqlClient.SqlDataAdapter(" Select  I.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.Item_GST_Percentage,sum(SD.Taxable_Value) As TaxableAmt,sum(SD.Weight) as Qty,Min(Sl_No), 'WGT' AS Units " &
        '                                  " from Bobin_Jari_SalesDelivery_Bobin_Details SD Inner Join Count_Head I On SD.Count_IdNo = I.Count_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
        '                                  " Where SD.Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " I.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage", con)
        'dt1 = New DataTable
        da.Fill(dt1)

        For I = 0 To dt1.Rows.Count - 1

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]        ,	        [HSNCode]           ,	[Quantity]                        ,               [QuantityUnit]               ,           Tax_Perc                   ,	[CessRate]        ,	[CessNonAdvol]  ,	    [TaxableAmount]      ,     InvCode) " &
                              " values                 (" & dt1.Rows(I).Item(0).ToString & ",'" & dt1.Rows(I).Item(1) & "',       ''                     , '" & dt1.Rows(I).Item(3) & "', " & dt1.Rows(I).Item(6).ToString & ",    '" & Trim(dt1.Rows(I).Item(7).ToString) & "'   ," & dt1.Rows(I).Item(4).ToString & "  ,     0             ,        0        , " & dt1.Rows(I).Item(5) & " ,'" & NewCode & "')"

            CMD.ExecuteNonQuery()

        Next

        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Bobin_Jari_SalesDelivery_Head", "Electronic_Reference_No", "Bobin_Jari_Delivery_Code", Pk_Condition)


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

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_EWBNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Bobin_Jari_SalesDelivery_Head", "Electronic_Reference_No", "Bobin_Jari_Delivery_Code")

    End Sub

    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 0)
    End Sub
    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 1)
    End Sub
    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_ElectronicRefNo.Text = txt_EWBNo.Text
    End Sub

    Private Sub txt_Freight_TextChanged(sender As Object, e As EventArgs) Handles txt_Freight.TextChanged
        Total_Calculation()
    End Sub
End Class