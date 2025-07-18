Imports System.IO
Public Class Yarn_Purchase_Return_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GYPRT-"
    Private Pk_Condition2 As String = "GYPAC-"
    Private PkCondition_Old As String = "GYPUR-"
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
    Public vmskLrText As String = ""
    Public vmskLrStrt As Integer = -1
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Public vmskBillOldText As String = ""
    Public vmskBillSelStrt As Integer = -1

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True
        chk_Verified_Status.Checked = False

        New_Entry = False
        Insert_Entry = False
        pnl_Selection.Visible = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        grp_EInvoice.Visible = False
        Grp_EWB.Visible = False

        txt_vehicle_no.Text = ""
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_PartyName.Text = ""

        cbo_Agent.Text = ""
        cbo_PurchaseAc.Text = ""
        txt_RecNo.Text = ""

        cbo_Delvat.Text = "" 'Common_Procedures.Ledger_IdNoToName(con, 4)
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        txt_BillNo.Text = ""
        txt_CommRate.Text = ""
        cbo_CommType.Text = "BAG"
        lbl_CommAmount.Text = ""

        lbl_GrossAmount.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""
        lbl_OrderCode.Text = ""
        lbl_OrderNo.Text = ""
        lbl_AssessableValue.Text = ""

        txt_YarnPurc_PrefixNo.Text = ""
        cbo_YarnPurc_SufixNo.Text = ""


        msk_BillDate.Text = ""
        msk_Lr_Date.Text = ""
        chk_TaxAmount_RoundOff_STS.Checked = False
        cbo_TaxType.Text = "GST"
        lbl_CGSTAmount.Text = ""
        lbl_SGSTAmount.Text = ""
        lbl_IGSTAmount.Text = ""

        txt_Freight.Text = ""
        txt_AddLess_AfterTax.Text = ""
        txt_AddLess_BeforeTax.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        lbl_AmountInWords.Text = "Rupees  :  "

        cbo_Transport.Text = ""
        txt_Note.Text = ""
        '--------------------

        pic_IRN_QRCode_Image.BackgroundImage = Nothing
        txt_IR_No.Text = ""

        txt_eInvoiceNo.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckNo.Enabled = True
        txt_eInvoice_CancelStatus.Enabled = False
        txt_eInvoiceAckDate.Text = ""
        txt_eInvoice_CancelStatus.Text = ""
        txt_EInvoiceCancellationReson.Text = ""

        rtbeInvoiceResponse.Text = ""
        txt_ElectronicRefNo.Text = ""

        '--------------------------

        txt_Tcs_Name.Text = "TCS"
        txt_TcsPerc.Text = ""
        lbl_TcsAmount.Text = ""
        txt_TCS_TaxableValue.Text = ""
        txt_TcsPerc.Enabled = False
        txt_TCS_TaxableValue.Enabled = False
        chk_TCSAmount_RoundOff_STS.Checked = True
        chk_TCS_Tax.Checked = False


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
            txt_BillNo.Text = ""
            cbo_Filter_DeliveryAt.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False

        NoCalc_Status = False

    End Sub
    Public Sub Get_vehicle_from_Transport()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim transport_id As Integer
        transport_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Transport.Text)
        Da = New SqlClient.SqlDataAdapter("select vehicle_no from ledger_head where ledger_idno=" & Str(Val(transport_id)) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count <> 0 Then
            txt_vehicle_no.Text = Dt.Rows(0).Item("vehicle_no").ToString


        End If
        Dt.Clear()
    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskBox As MaskedTextBox
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
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
            mskBox = Me.ActiveControl
            mskBox.SelectionStart = -1

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

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
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
        Dim NewCode2 As String
        Dim n As Integer
        Dim SNo As Integer
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt4 As New DataTable

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)
        NewCode2 = Trim(PkCondition_Old) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)



        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Yarn_Purchase_Return_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ( a.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' or  a.Yarn_Purchase_Return_Code = '" & Trim(NewCode2) & "' ) and a.Entry_VAT_GST_Type = 'GST' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                'lbl_RefNo.Text = dt1.Rows(0).Item("Yarn_Purchase_Return_No").ToString

                txt_YarnPurc_PrefixNo.Text = dt1.Rows(0).Item("Yarn_Purchase_Return_PrefixNo").ToString
                lbl_RefNo.Text = dt1.Rows(0).Item("Yarn_Purchase_Return_RefNo").ToString
                cbo_YarnPurc_SufixNo.Text = dt1.Rows(0).Item("Yarn_Purchase_Return_SuffixNo").ToString

                dtp_Date.Text = dt1.Rows(0).Item("Yarn_Purchase_Return_Date")

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))

                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))

                cbo_Delvat.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_Idno").ToString))
                txt_RecNo.Text = dt1.Rows(0).Item("Delivery_Receipt_No").ToString
                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                dtp_BillDate.Text = dt1.Rows(0).Item("Bill_Date").ToString
                msk_BillDate.Text = dtp_BillDate.Text
                txt_CommRate.Text = Val(dt1.Rows(0).Item("Agent_Commission_Rate").ToString)
                cbo_CommType.Text = dt1.Rows(0).Item("Agent_Commission_Type").ToString
                lbl_CommAmount.Text = dt1.Rows(0).Item("Agent_Commission_Commission").ToString

                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                msk_Lr_Date.Text = dt1.Rows(0).Item("Lr_DateTxt").ToString
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                lbl_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")
                lbl_CGSTAmount.Text = Format(Val(dt1.Rows(0).Item("CGst_Amount").ToString), "########0.00")
                lbl_SGSTAmount.Text = Format(Val(dt1.Rows(0).Item("SGst_Amount").ToString), "########0.00")
                lbl_IGSTAmount.Text = Format(Val(dt1.Rows(0).Item("IGst_Amount").ToString), "########0.00")
                txt_vehicle_no.Text = dt1.Rows(0).Item("vehicle_no").ToString
                txt_AddLess_BeforeTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), "#########0.00")
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
                txt_AddLess_AfterTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                cbo_TaxType.Text = dt1.Rows(0).Item("Entry_GST_Tax_Type").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                chk_TaxAmount_RoundOff_STS.Checked = False
                If IsDBNull(dt1.Rows(0).Item("TaxAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("TaxAmount_RoundOff_Status").ToString) = 1 Then chk_TaxAmount_RoundOff_STS.Checked = True Else chk_TaxAmount_RoundOff_STS.Checked = False
                End If
                lbl_OrderNo.Text = dt1.Rows(0).Item("Our_Order_No").ToString
                lbl_OrderCode.Text = dt1.Rows(0).Item("Own_Order_Code").ToString

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True


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

                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EInvoiceCancellationReson.Text = Trim(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)

                txt_ElectronicRefNo.Text = Trim(dt1.Rows(0).Item("Electronic_Reference_No").ToString)


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Mill_Name, c.Count_name from Yarn_Purchase_Return_Details a INNER JOIN Mill_Head b ON a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where ( a.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' or a.Yarn_Purchase_Return_Code = '" & Trim(NewCode2) & "' ) Order by a.sl_no", con)
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
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Mill_Name").ToString
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Rate_For").ToString
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            .Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Cash_Discount_Percentage").ToString)
                            .Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Cash_Discount_Amount").ToString)
                            .Rows(n).Cells(11).Value = Val(dt2.Rows(i).Item("Taxable_Value").ToString)
                            .Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("HSN_Code").ToString)
                            .Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("GST_Percentage").ToString)
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
                da4 = New SqlClient.SqlDataAdapter("Select a.* from Yarn_Purchase_Return_GST_Tax_Details a Where (a.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' or a.Yarn_Purchase_Return_Code = '" & Trim(NewCode2) & "' ) ", con)
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

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

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
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Delvat.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SIZING" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Delvat.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Yarn_Purchase_Return_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        Me.Text = ""

        con.Open()

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User.Name))

        chk_Verified_Status.Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1249" Or Trim(Common_Procedures.settings.CustomerCode) = "1116" Then

            If Val(Common_Procedures.User.IdNo) <> 1 And Common_Procedures.UR.Ledger_Verifition = "" Then chk_Verified_Status.Visible = True
        End If

        ' Common_Procedures.get_CashPartyName_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        '    If Common_Procedures.settings.CustomerCode = "1266" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt2)
            cbo_Delvat.DataSource = dt2
            cbo_Delvat.DisplayMember = "Ledger_DisplayName"
            'Else

            '    da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN') order by Ledger_DisplayName", con)
            '    da.Fill(dt2)
            '    cbo_Delvat.DataSource = dt2
            '    cbo_Delvat.DisplayMember = "Ledger_DisplayName"
            'End If


            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Agent.DataSource = dt3
        cbo_Agent.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 27 ) order by Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_PurchaseAc.DataSource = dt4
        cbo_PurchaseAc.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt6)
        cbo_Transport.DataSource = dt6
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        cbo_CommType.Items.Clear()
        cbo_CommType.Items.Add("BAG")
        cbo_CommType.Items.Add("%")

        'Common_Procedures.get_VehicleNo_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select distinct(Count_Name) from Count_Head order by Count_Name", con)
        da.Fill(dt7)
        cbo_Grid_CountName.DataSource = dt7
        cbo_Grid_CountName.DisplayMember = "Count_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Mill_Name) from Mill_Head order by Mill_Name", con)
        da.Fill(dt8)
        cbo_Grid_MillName.DataSource = dt8
        cbo_Grid_MillName.DisplayMember = "Mill_Name"

        cbo_Grid_RateFor.Items.Clear()
        cbo_Grid_RateFor.Items.Add("BAG")
        cbo_Grid_RateFor.Items.Add("KG")

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("GST")
        cbo_TaxType.Items.Add("NO TAX")


        cbo_Verified_Sts.Items.Clear()
        cbo_Verified_Sts.Items.Add("")
        cbo_Verified_Sts.Items.Add("YES")
        cbo_Verified_Sts.Items.Add("NO")

        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = (Me.Height - pnl_Tax.Height) \ 2
        pnl_Tax.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = ((Me.Height - pnl_Print.Height) \ 2) - 100
        pnl_Print.BringToFront()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
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


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        cbo_YarnPurc_SufixNo.Items.Clear()
        cbo_YarnPurc_SufixNo.Items.Add("")
        cbo_YarnPurc_SufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_YarnPurc_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_YarnPurc_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))
        cbo_YarnPurc_SufixNo.Items.Add("/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)))

        '   lbl_caption_vehicleno.Visible = False
        'txt_vehicle_no.Visible = False
        'If Trim(Common_Procedures.settings.CustomerCode) = "1186" Then

        '    lbl_caption_vehicleno.Visible = True
        '    txt_vehicle_no.Visible = True

        'End If
        AddHandler txt_vehicle_no.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_YarnPurc_PrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnPurc_SufixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delvat.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RecNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommRate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_CommAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_AfterTax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_BeforeTax.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_DeliveryAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FilterBillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_BillDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Lr_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_vehicle_no.LostFocus, AddressOf ControlLostFocus


        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_YarnPurc_PrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_YarnPurc_SufixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delvat.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RecNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_CommAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_AfterTax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_BeforeTax.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_DeliveryAt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FilterBillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_BillDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Lr_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RecNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FilterBillNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BillNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_AfterTax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_BeforeTax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_BillDate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler msk_Lr_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RecNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FilterBillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_AfterTax.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_BeforeTax.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_BillDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler msk_Lr_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrNo.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_IR_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ElectronicRefNo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_IR_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ElectronicRefNo.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Yarn_Purchase_Return_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Yarn_Purchase_Return_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress


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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            Else
                dgv1 = dgv_Details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 7 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            txt_DiscPerc.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            txt_DiscPerc.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            lbl_CommAmount.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

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
        Dim NewCode2 As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        NewCode2 = Trim(Pk_Condition2) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Yarn_Purchase_Return_Entry, New_Entry, Me, con, "Yarn_Purchase_Return_Head", "Yarn_Purchase_Return_Code", NewCode, "Yarn_Purchase_Return_Date", "(Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Yarn_Purchase_Return_Head", "Verified_Status", "(Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "')")) = 1 Then
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

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            NewCode2 = Trim(PkCondition_Old) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Yarn_Purchase_Return_Head", "Yarn_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Yarn_Purchase_Return_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Yarn_Purchase_Return_Details", "Yarn_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount,Cash_Discount_Percentage,Cash_Discount_Amount ,Taxable_Value,HSN_Code,GST_Percentage", "Sl_No", "Yarn_Purchase_Return_Code, For_OrderBy, Company_IdNo, Yarn_Purchase_Return_No, Yarn_Purchase_Return_Date, Ledger_Idno", trans)


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode), trans)
            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode2), trans)

            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Reference_Code = '" & Trim(PkCondition_Old) & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and (Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Reference_Code = '" & Trim(PkCondition_Old) & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and (Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Reference_Code = '" & Trim(PkCondition_Old) & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Yarn_Purchase_Return_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' or Yarn_Purchase_Return_Code = '" & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Yarn_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' or Yarn_Purchase_Return_Code = '" & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Yarn_Purchase_Return_GST_Tax_Details  where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' or Yarn_Purchase_Return_Code = '" & Trim(NewCode2) & "')"
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

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) )order by Ledger_DisplayName", con)
            da.Fill(dt2)
            cbo_Filter_DeliveryAt.DataSource = dt2
            cbo_Filter_DeliveryAt.DisplayMember = "Ledger_DisplayName"


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_DeliveryAt.Text = ""
            txt_FilterBillNo.Text = ""

            cbo_Filter_DeliveryAt.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Purchase_Return_RefNo from Yarn_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Yarn_Purchase_Return_RefNo", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Purchase_Return_RefNo from Yarn_Purchase_Return_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Yarn_Purchase_Return_RefNo", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Purchase_Return_RefNo from Yarn_Purchase_Return_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Yarn_Purchase_Return_RefNo desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Purchase_Return_RefNo from Yarn_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Yarn_Purchase_Return_RefNo desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            If Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then

                lbl_RefNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "DRNT")

            Else
                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Purchase_Return_Head", "Yarn_Purchase_Return_Code", "For_OrderBy", "Entry_VAT_GST_Type = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            End If
            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            dtp_Date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as PurchaseAcName, c.ledger_name as TaxAcName from Yarn_Purchase_Return_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Entry_VAT_GST_Type = 'GST' Order by a.for_Orderby desc, a.Yarn_Purchase_Return_RefNo desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Yarn_Purchase_Return_Date").ToString <> "" Then dtp_Date.Text = Dt1.Rows(0).Item("Yarn_Purchase_Return_Date").ToString
                End If

                If Dt1.Rows(0).Item("PurchaseAcName").ToString <> "" Then cbo_PurchaseAc.Text = Dt1.Rows(0).Item("PurchaseAcName").ToString
                ' If Dt1.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = Dt1.Rows(0).Item("Tax_Type").ToString

                If Dt1.Rows(0).Item("Agent_Commission_Type").ToString <> "" Then cbo_CommType.Text = Dt1.Rows(0).Item("Agent_Commission_Type").ToString

                'If IsDBNull(Dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = False Then
                '    If Val(Dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False
                'End If

                If IsDBNull(Dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(Dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If

                If IsDBNull(Dt1.Rows(0).Item("Yarn_Purchase_Return_PrefixNo").ToString) = False Then
                    If Dt1.Rows(0).Item("Yarn_Purchase_Return_PrefixNo").ToString <> "" Then txt_YarnPurc_PrefixNo.Text = Dt1.Rows(0).Item("Yarn_Purchase_Return_PrefixNo").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("Yarn_Purchase_Return_SuffixNo").ToString) = False Then
                    If Dt1.Rows(0).Item("Yarn_Purchase_Return_SuffixNo").ToString <> "" Then cbo_YarnPurc_SufixNo.Text = Dt1.Rows(0).Item("Yarn_Purchase_Return_SuffixNo").ToString
                End If

                Da1 = New SqlClient.SqlDataAdapter("Select a.* from Yarn_Purchase_Return_Details a Where a.Yarn_Purchase_Return_Code = '" & Trim(Dt1.Rows(0).Item("Yarn_Purchase_Return_Code").ToString) & "' Order by a.sl_no", con)
                Dt2 = New DataTable
                Da1.Fill(Dt2)

                If Dt2.Rows.Count > 0 Then

                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Rows(0).Cells(6).Value = Dt2.Rows(0).Item("Rate_For").ToString
                    End If

                End If

                Dt2.Clear()

            End If

            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String, vmovno As String
        Dim RefCode As String

        Dim vRefCode As String = ""
        Try

            inpno = InputBox("Enter Rtn No.", "FOR FINDING...")

            RefCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Purchase_Return_RefNo from Yarn_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code = '" & Trim(RefCode) & "' and Entry_VAT_GST_Type = 'GST'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()


            If Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then

                vRefCode = "GSDBN-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(vRefCode) & "'", con)
                Dt = New DataTable
                Da.Fill(Dt)
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vmovno = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

            End If

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vmovno) <> 0 Then
                MessageBox.Show("This Invoice No. is in Debit Note", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show("Rtn No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim YMovNo As String = ""
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim YInvCode As String = ""
        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Yarn_Purchase_Return_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Rtn No.", "FOR NEW Rtn NO. INSERTION...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Purchase_Return_RefNo from Yarn_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code = '" & Trim(InvCode) & "' and Entry_VAT_GST_Type = 'GST' ", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            If Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then

                YInvCode = "GSDBN-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da1 = New SqlClient.SqlDataAdapter("select Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code='" & Trim(YInvCode) & "' ", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                        YMovNo = Trim(Dt1.Rows(0)(0).ToString)

                    End If

                    Dt1.Clear()
                End If



            End If



            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(YMovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Debit Note", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Rtn No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim NewCode2 As String = ""
        Dim PurAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Led_ID As Integer = 0
        Dim RndOff_STS As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim Del_ID As Integer
        Dim Agt_Idno As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotCns As Single, vTotBgs As Single, vTotWght As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim usr_idno As Single = 0
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""
        Dim vLrDt As String = ""
        Dim vBillDt As String = ""
        Dim OurOrd_No As String = ""

        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim vEInvAckDate As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        Dim vTCS_Tax_Sts As Integer = 0
        Dim vTCSAmtRndOff_STS As Integer = 0
        Dim vTCS_AssVal_EditSTS As Integer = 0
        Dim vYarnPurc_No As String = ""


        If Val(lbl_Company.Tag) = 0 Then


            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Yarn_Purchase_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Yarn_Purchase_Return_Entry, New_Entry, Me, con, "Yarn_Purchase_Return_Head", "Yarn_Purchase_Return_Code", NewCode, "Yarn_Purchase_Return_Date", "(Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Yarn_Purchase_Return_RefNo desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Yarn_Purchase_Return_Head", "Verified_Status", "(Yarn_Purchase_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If
        'If IsDate(dtp_BillDate.Text) = False Then
        '    MessageBox.Show("Invalid Bill Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        '    Exit Sub
        'End If
        If IsDate(dtp_Lrdate.Text) = False Then
            MessageBox.Show("Invalid Lr Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If
        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If
        If Trim(lbl_OrderCode.Text) <> "" Then


            If Led_ID <> 0 Then

                Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Head a INNER JOIN Own_order_Yarn_Details b ON a.Own_Order_Code =b.Own_Order_Code where a.Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' and  b.Ledger_idno = " & Str(Val(Led_ID)), con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    OurOrd_No = Dt1.Rows(0).Item("Order_No").ToString

                End If
            End If
            If Trim(OurOrd_No) <> Trim(lbl_OrderNo.Text) Then
                MessageBox.Show("Invalid Mismatch Of Order No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
                Exit Sub
            End If
        End If
        If Trim(txt_BillNo.Text) = "" Then
            MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
            Exit Sub
        End If
        If IsDate(msk_BillDate.Text) = False Then
            msk_BillDate.Text = msk_Date.Text
        End If

        If IsDate(msk_BillDate.Text) = False Then
            MessageBox.Show("Invalid Bill Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_BillDate.Enabled And msk_BillDate.Visible Then msk_BillDate.Focus()
            Exit Sub
        End If

        'If Not (Convert.ToDateTime(msk_BillDate.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_BillDate.Text) <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Bill Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If msk_BillDate.Enabled And msk_BillDate.Visible Then msk_BillDate.Focus()
        '    Exit Sub
        'End If

        vLrDt = ""
        If Trim(dtp_Lrdate.Text) <> "" Then
            If IsDate(dtp_Lrdate.Text) = True Then
                vLrDt = Trim(msk_Lr_Date.Text)
            End If
        End If


        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        Del_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Delvat.Text)
        ' If Del_ID = 0 Then Del_ID = 4
        PurAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo


        If PurAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Cnt_ID = 0 Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(2).Value)
                    If Mill_ID = 0 Then
                        MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With



        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@YarnDate", dtp_Date.Value.Date)
        cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(msk_BillDate.Text))

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

        vYarnPurc_No = Trim(txt_YarnPurc_PrefixNo.Text) & Trim(lbl_RefNo.Text) & Trim(cbo_YarnPurc_SufixNo.Text)

        RndOff_STS = 0
        If chk_TaxAmount_RoundOff_STS.Checked = True Then RndOff_STS = 1




        vTCS_Tax_Sts = 0
        If chk_TCS_Tax.Checked = True Then vTCS_Tax_Sts = 1

        vTCS_AssVal_EditSTS = 0
        If txt_TCS_TaxableValue.Enabled = True Then vTCS_AssVal_EditSTS = 1

        vTCSAmtRndOff_STS = 0
        If chk_TCSAmount_RoundOff_STS.Checked = True Then vTCSAmtRndOff_STS = 1


        NoCalc_Status = False
        Total_Calculation()

        vTotCns = 0 : vTotBgs = 0 : vTotWght = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBgs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotCns = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotWght = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
        End If

        cmd.Connection = con

        cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
        cmd.ExecuteNonQuery()

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                If Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then

                    lbl_RefNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "DRNT", tr)
                Else
                    lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Purchase_Return_Head", "Yarn_Purchase_Return_Code", "For_OrderBy", "Entry_VAT_GST_Type = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr



            If New_Entry = True Then

                cmd.CommandText = "Insert into Yarn_Purchase_Return_Head (  Entry_VAT_GST_Type  ,     Yarn_Purchase_Return_Code ,               Company_IdNo       ,    Yarn_Purchase_Return_RefNo  ,       Yarn_Purchase_Return_PrefixNo       ,        Yarn_Purchase_Return_SuffixNo         , Yarn_Purchase_Return_No           ,                                for_Orderby                              , Yarn_Purchase_Return_Date,        Ledger_IdNo      ,          Agent_IdNo       ,        PurchaseAc_IdNo    ,      DeliveryTo_Idno    ,                Bill_No          ,  Bill_Date ,               Lr_No          ,       Lr_DateTxt     ,        Delivery_Receipt_No    ,   Agent_Commission_Rate       ,         Agent_Commission_Type    ,   Agent_Commission_Commission   ,       Total_Bags     ,          Total_Cones   ,          Total_Weight      ,               Total_Amount            ,             Discount_Percentage    ,              Discount_Amount         ,                AddLess_BeforeTax_Amount     ,                 Assessable_Value          ,            Freight_Amount          ,                 AddLess_Amount             ,               RoundOff_Amount      ,                  Net_Amount               ,         Transport_IdNo    ,            Note              ,   User_IdNo                   ,          Entry_GST_Tax_Type     ,                 CGst_Amount          ,                 SGst_Amount          ,               IGst_Amount            , TaxAmount_RoundOff_Status   ,              Our_Order_No        ,                  Own_Order_Code     ,verified_status ,                      Vehicle_no              ,          Tcs_Name_caption        ,           EDIT_TCS_TaxableValue      ,              TCS_Taxable_Value             ,            Tcs_percentage         ,                Tcs_Amount           ,           Tcs_Tax_Status               , TCSAmount_RoundOff_Status  ) " &
                                        "      Values                    (        'GST'         ,   '" & Trim(NewCode) & "'     , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "' , '" & Trim(txt_YarnPurc_PrefixNo.Text) & "' , '" & Trim(cbo_YarnPurc_SufixNo.Text) & "'   ,  '" & Trim(vYarnPurc_No) & "'   , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",             @YarnDate    , " & Str(Val(Led_ID)) & ", " & Str(Val(Agt_Idno)) & ", " & Str(Val(PurAc_ID)) & ", " & Str(Val(Del_ID)) & ",  '" & Trim(txt_BillNo.Text) & "', @BillDate  , '" & Trim(txt_LrNo.Text) & "', '" & Trim(vLrDt) & "', '" & Trim(txt_RecNo.Text) & "', " & Val(txt_CommRate.Text) & ", '" & Trim(cbo_CommType.Text) & "', " & Val(lbl_CommAmount.Text) & ",  " & Val(vTotBgs) & "," & Str(Val(vTotCns)) & ", " & Str(Val(vTotWght)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(txt_AddLess_BeforeTax.Text)) & ", " & Str(Val(lbl_AssessableValue.Text)) & ",  " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess_AfterTax.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Str(Val(Trans_ID)) & ",'" & Trim(txt_Note.Text) & "' , " & Val(lbl_UserName.Text) & ", '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(lbl_CGSTAmount.Text)) & ", " & Str(Val(lbl_SGSTAmount.Text)) & ", " & Str(Val(lbl_IGSTAmount.Text)) & ", " & Str(Val(RndOff_STS)) & ", '" & Trim(lbl_OrderNo.Text) & "' ,    '" & Trim(lbl_OrderCode.Text) & "' ," & Val(Verified_STS) & ",'" & Trim(txt_vehicle_no.Text) & "'  ,    '" & Trim(txt_Tcs_Name.Text) & "', " & Str(Val(vTCS_AssVal_EditSTS)) & ", " & Str(Val(txt_TCS_TaxableValue.Text)) & ", " & Str(Val(txt_TcsPerc.Text)) & ", " & Str(Val(lbl_TcsAmount.Text)) & " , " & Str(Val(vTCS_Tax_Sts)) & " , " & Str(Val(vTCSAmtRndOff_STS)) & "  ) "
                cmd.ExecuteNonQuery()


            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Yarn_Purchase_Return_Head", "Yarn_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Purchase_Return_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Yarn_Purchase_Return_Details", "Yarn_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount,Cash_Discount_Percentage,Cash_Discount_Amount ,Taxable_Value,HSN_Code,GST_Percentage", "Sl_No", "Yarn_Purchase_Return_Code, For_OrderBy, Company_IdNo, Yarn_Purchase_Return_No, Yarn_Purchase_Return_Date, Ledger_Idno", tr)

                NewCode2 = Trim(PkCondition_Old) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                cmd.CommandText = "Update Yarn_Purchase_Return_Head Set Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' , Entry_VAT_GST_Type = 'GST' , Yarn_Purchase_Return_Date = @YarnDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Bill_No = '" & Trim(txt_BillNo.Text) & "', Bill_Date = @BillDate, Lr_No = '" & Trim(txt_LrNo.Text) & "', Lr_DateTxt = '" & Trim(vLrDt) & "', Agent_IdNo = " & Str(Val(Agt_Idno)) & ", PurchaseAc_IdNo = " & Str(Val(PurAc_ID)) & ", DeliveryTo_Idno = " & Str(Val(Del_ID)) & ", Delivery_Receipt_No = '" & Trim(txt_RecNo.Text) & "', Agent_Commission_Rate = " & Val(txt_CommRate.Text) & ", Agent_Commission_Type = '" & Trim(cbo_CommType.Text) & "', Agent_Commission_Commission =" & Val(lbl_CommAmount.Text) & ", Total_Bags = " & Val(vTotBgs) & ", Total_Cones = " & Str(Val(vTotCns)) & ", Total_Weight = " & Str(Val(vTotWght)) & ", Total_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ", AddLess_BeforeTax_Amount = " & Str(Val(txt_AddLess_BeforeTax.Text)) & ", Assessable_Value = " & Str(Val(lbl_AssessableValue.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess_AfterTax.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Transport_IdNo  = " & Str(Val(Trans_ID)) & ", Note = '" & Trim(txt_Note.Text) & "', User_IdNo = " & Val(lbl_UserName.Text) & ", Entry_GST_Tax_Type = '" & Trim(cbo_TaxType.Text) & "',  CGst_Amount = " & Str(Val(lbl_CGSTAmount.Text)) & " , SGst_Amount = " & Str(Val(lbl_SGSTAmount.Text)) & " , IGst_Amount = " & Str(Val(lbl_IGSTAmount.Text)) & ", TaxAmount_RoundOff_Status = " & Str(Val(RndOff_STS)) & ", Our_Order_No = '" & Trim(lbl_OrderNo.Text) & "', Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "',Verified_Status=" & Val(Verified_STS) & " ,Vehicle_No='" & Trim(txt_vehicle_no.Text) & "' , Tcs_Name_caption = '" & Trim(txt_Tcs_Name.Text) & "', EDIT_TCS_TaxableValue = " & Str(Val(vTCS_AssVal_EditSTS)) & " , TCS_Taxable_Value = " & Str(Val(txt_TCS_TaxableValue.Text)) & ", Tcs_percentage = " & Str(Val(txt_TcsPerc.Text)) & "  , Tcs_Amount = " & Str(Val(lbl_TcsAmount.Text)) & " , Tcs_Tax_Status =  " & Str(Val(vTCS_Tax_Sts)) & " , TCSAmount_RoundOff_Status = " & Str(Val(vTCSAmtRndOff_STS)) & " , E_Invoice_IRNO = '" & Trim(txt_IR_No.Text) & "' , E_Invoice_QR_Image = @QrCode  , E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & "  ,  E_Invoice_Cancelled_Status = " & eiCancel.ToString & " ,  E_Invoice_Cancellation_Reason = '" & txt_EInvoiceCancellationReson.Text & "'  ,    Electronic_Reference_No = '" & txt_ElectronicRefNo.Text & "' , Yarn_Purchase_Return_PrefixNo = '" & Trim(txt_YarnPurc_PrefixNo.Text) & "' ,  Yarn_Purchase_Return_SuffixNo = '" & Trim(cbo_YarnPurc_SufixNo.Text) & "', Yarn_Purchase_Return_No = '" & Trim(vYarnPurc_No) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and (Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' or Yarn_Purchase_Return_Code = '" & Trim(NewCode2) & "')"
                cmd.ExecuteNonQuery()

            End If


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Yarn_Purchase_Return_Head", "Yarn_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Purchase_Return_Code, Company_IdNo, for_OrderBy", tr)


            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(txt_BillNo.Text)
            Partcls = "Purc : Ref No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Yarn_Purchase_Return_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and (Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' or Yarn_Purchase_Return_Code = '" & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and (Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Reference_Code = '" & Trim(PkCondition_Old) & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and (Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Reference_Code = '" & Trim(PkCondition_Old) & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                YrnClthNm = ""
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        If Trim(YrnClthNm) = "" Then YrnClthNm = Trim(.Rows(i).Cells(1).Value) & "/" & Trim(.Rows(i).Cells(2).Value)

                        cmd.CommandText = "Insert into Yarn_Purchase_Return_Details ( Yarn_Purchase_Return_Code ,               Company_IdNo       ,   Yarn_Purchase_Return_No    ,                     for_OrderBy                                            ,              Yarn_Purchase_Return_Date,             Sl_No     ,              Count_IdNo         ,          Mill_IdNo       ,                     Bags            ,                 Cones                ,                        Weight         ,                   Rate_For                       ,                     Rate                 ,                  Amount        ,Cash_Discount_Percentage                  ,Cash_Discount_Amount                      ,Taxable_Value                    ,                    HSN_Code            ,          GST_Percentage                               ) " &
                                                "     Values                 (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @YarnDate            ,  " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mill_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", '" & Trim(.Rows(i).Cells(6).Value) & "', " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & "           ,  " & Str(Val(.Rows(i).Cells(9).Value)) & "," & Str(Val(.Rows(i).Cells(10).Value)) & "," & Str(Val(.Rows(i).Cells(11).Value)) & ",'" & Trim(.Rows(i).Cells(12).Value) & "' ," & Str(Val(.Rows(i).Cells(13).Value)) & "        ) "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @YarnDate, 0, " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", 'MILL', " & Str(Val(Mill_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " ,  " & Str(Val(Led_ID)) & ",0)"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            If Val(vTotBgs) <> 0 Or Val(vTotCns) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo                     , Reference_No                  , for_OrderBy                                                            , Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno       , Entry_ID             , Party_Bill_No        , Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags               , Empty_Cones              , Particulars            ) " &
                "Values                                    ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @YarnDate     , 0              , " & Str(Val(Del_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', 1    , 0              , 0         , " & Str(Val(vTotBgs)) & ", " & Str(Val(vTotCns)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If


            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Reference_Code = '" & Trim(PkCondition_Old) & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()

            If Val(Agt_Idno) <> 0 Then

                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date, Commission_For,     Ledger_IdNo     ,      Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,      Yarn_Cloth_Name     ,         Bags_Meters       ,               Amount               ,              Commission_Type      ,       Commission_Rate              ,            Commission_Amount         ) " &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @YarnDate   ,     'YARN'    , " & Str(Led_ID) & ", " & Str(Agt_Idno) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', '" & Trim(YrnClthNm) & "', " & Str(Val(vTotBgs)) & "  ,  " & Str(Val(lbl_NetAmount.Text)) & ", '" & Trim(cbo_CommType.Text) & "', " & Str(Val(txt_CommRate.Text)) & ", -1 *" & Str(Val(lbl_CommAmount.Text)) & ")"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Yarn_Purchase_Return_Details", "Yarn_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount,Cash_Discount_Percentage,Cash_Discount_Amount ,Taxable_Value,HSN_Code,GST_Percentage", "Sl_No", "Yarn_Purchase_Return_Code, For_OrderBy, Company_IdNo, Yarn_Purchase_Return_No, Yarn_Purchase_Return_Date, Ledger_Idno", tr)

            cmd.CommandText = "Delete from Yarn_Purchase_Return_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and (Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' or Yarn_Purchase_Return_Code = '" & Trim(NewCode2) & "')"
            cmd.ExecuteNonQuery()

            With dgv_Tax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Yarn_Purchase_Return_GST_Tax_Details   ( Yarn_Purchase_Return_Code          ,               Company_IdNo       ,      Yarn_Purchase_Return_No                ,                               for_OrderBy     , Yarn_Purchase_Return_Date    ,         Ledger_IdNo     ,            Sl_No     , HSN_Code                               ,Taxable_Amount                            ,CGST_Percentage                           ,CGST_Amount                               ,SGST_Percentage                            ,SGST_Amount                              ,IGST_Percentage                          ,IGST_Amount ) " &
                                                "     Values                        (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @YarnDate    , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "  ," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            '    '  If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            vLed_IdNos = Led_ID & "|" & PurAc_ID & "|24|25|26"
            vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(lbl_CGSTAmount.Text) - Val(lbl_SGSTAmount.Text) - Val(lbl_IGSTAmount.Text)) & "|" & Val(lbl_CGSTAmount.Text) & "|" & Val(lbl_SGSTAmount.Text) & "|" & Val(lbl_IGSTAmount.Text)
            If Common_Procedures.Voucher_Updation(con, "Yarn.Pur.RetGst", Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If
            ' End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then '---- Kalaimagal Textiles (Avinashi)
                If Val(lbl_CommAmount.Text) <> 0 Then
                    vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
                    vVou_Amts = -1 * Val(lbl_CommAmount.Text) & "|" & Val(lbl_CommAmount.Text)
                    If Common_Procedures.Voucher_Updation(con, "Ag.Com.Ret", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                    End If
                End If
            End If

            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(txt_BillNo.Text), Agt_Idno, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                          " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If

            tr.Commit()
            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


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

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value)

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
                            .Rows(.CurrentRow.Index).Cells(4).Value = .Rows(.CurrentRow.Index).Cells(3).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(3).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = Format(.Rows(.CurrentRow.Index).Cells(4).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Trim(Common_Procedures.settings.CustomerCode) = "1186" Then
            Get_vehicle_from_Transport()
        End If
    End Sub


    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_AddLess_AfterTax, txt_vehicle_no, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Trim(Common_Procedures.settings.CustomerCode) = "1186" Then
            Get_vehicle_from_Transport()
        End If
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_vehicle_no, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Trim(Common_Procedures.settings.CustomerCode) = "1186" Then
            Get_vehicle_from_Transport()
        End If
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        cbo_PartyName.Tag = cbo_PartyName.Text
    End Sub
    Private Sub cbo_Party_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_Date, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Party_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            cbo_PurchaseAc.Focus()
            'If MessageBox.Show("Do you want to select order:", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '    btn_Selection_Click(sender, e)
            'Else

            '    cbo_PurchaseAc.Focus()


            'End If
            'If Trim(UCase(cbo_PartyName.Tag)) <> Trim(UCase(cbo_PartyName.Text)) Then
            '    cbo_PartyName.Tag = cbo_PartyName.Text
            '    GST_Calculation()
            'End If
        End If
    End Sub

    Private Sub cbo_Delvat_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delvat.KeyDown
        ' If Common_Procedures.settings.CustomerCode = "1266" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delvat, msk_Lr_Date, cbo_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")

        'Else
        '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delvat, cbo_TaxType, cbo_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING')", "(Ledger_IdNo = 0)")
        'End If


    End Sub

    Private Sub cbo_Delvat_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Delvat.KeyPress
        '     If Common_Procedures.settings.CustomerCode = "1266" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delvat, cbo_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        'Else
        '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delvat, cbo_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING')", "(Ledger_IdNo = 0)")
        'End If


    End Sub

    Private Sub cbo_PurchaseAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAc.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")

    End Sub


    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, cbo_PartyName, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
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
                Condt = "a.Yarn_Purchase_Return_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Yarn_Purchase_Return_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Purchase_Return_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_DeliveryAt.Text) <> "" Then
                Del_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_DeliveryAt.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If
            If Val(Del_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(Del_IdNo)) & " "
            End If

            If Trim(txt_FilterBillNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Bill_No = '" & Trim(txt_FilterBillNo.Text) & "' "
            End If

            If cbo_Verified_Sts.Visible = True And Trim(cbo_Verified_Sts.Text) <> "" Then

                If Trim(cbo_Verified_Sts.Text) = "YES" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 1 "
                ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 0 "
                End If

            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as PartyName, d.Ledger_Name as Delv_Name from Yarn_Purchase_Return_Head a INNER JOIN Yarn_Purchase_Return_Details b ON a.Yarn_Purchase_Return_Code = b.Yarn_Purchase_Return_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.DeliveryTo_Idno = d.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Entry_VAT_GST_Type = 'GST'  and a.Yarn_Purchase_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Purchase_Return_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from Yarn_Purchase_Return_Head a INNER JOIN Yarn_Purchase_Return_Details b ON a.Yarn_Purchase_Return_Code = b.Yarn_Purchase_Return_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Purchase_Return_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Yarn_Purchase_Return_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Yarn_Purchase_Return_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Delv_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        ' Common_Procedures.get_CashPartyName_From_All_Entries(con)
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, txt_FilterBillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, txt_FilterBillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")



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
        dgv_Details_CellLeave(sender, e)
        If dgv_Details.CurrentCell.ColumnIndex = 3 Or dgv_Details.CurrentCell.ColumnIndex = 4 Then
            get_MillCount_Details()
        End If
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

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

            If e.ColumnIndex = 2 Then

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

            If e.ColumnIndex = 6 Then

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

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        If Not IsNothing(dgv_Details.CurrentCell) Then

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then

                        Amount_Calculation(e.RowIndex, e.ColumnIndex)

                    End If
                End If
            End With
        End If


    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
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
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

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
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            .Rows(e.RowIndex).Cells(0).Value = Val(e.RowIndex) + 1
            If e.RowIndex > 0 Then
                .Rows(e.RowIndex).Cells(6).Value = Trim(UCase(.Rows(e.RowIndex - 1).Cells(6).Value))
            Else
                .Rows(e.RowIndex).Cells(6).Value = "KG"
            End If
            'n = .RowCount
            '.Rows(n - 1).Cells(0).Value = Val(n)
        End With
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

    Private Sub txt_AddLess_BeforeTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_BeforeTax.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
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
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True

            Else
                lbl_CommAmount.Focus()

            End If
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then txt_ElectronicRefNo.Focus() 'SendKeys.Send("+{TAB}")

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        On Error Resume Next

        With dgv_Details
            If .Visible Then
                If CurCol = 3 Or CurCol = 4 Or CurCol = 5 Or CurCol = 6 Or CurCol = 7 Then

                    If Trim(UCase(.Rows(CurRow).Cells(6).Value)) = "BAG" Then
                        .Rows(CurRow).Cells(8).Value = Format(Val(.Rows(CurRow).Cells(3).Value) * Val(.Rows(CurRow).Cells(7).Value), "#########0.00")
                    Else
                        .Rows(CurRow).Cells(8).Value = Format(Val(.Rows(CurRow).Cells(5).Value) * Val(.Rows(CurRow).Cells(7).Value), "#########0.00")
                    End If

                    Total_Calculation()

                End If
            End If
        End With

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBgs As Single
        Dim TotCns As Single
        Dim TotWgt As Single
        Dim TotAmt As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBgs = 0 : TotCns = 0 : TotWgt = 0 : TotAmt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" Or (Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0) Then

                    TotBgs = TotBgs + Val(.Rows(i).Cells(3).Value)
                    TotCns = TotCns + Val(.Rows(i).Cells(4).Value)
                    TotWgt = TotWgt + Val(.Rows(i).Cells(5).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(8).Value)

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
        Dim vTOT_SalAmt As String = 0


        If NoCalc_Status = True Then Exit Sub

        lbl_DiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

        lbl_AssessableValue.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess_BeforeTax.Text) + Val(txt_Freight.Text), "########0.00")

        GST_Amt = Val(lbl_CGSTAmount.Text) + Val(lbl_SGSTAmount.Text) + Val(lbl_IGSTAmount.Text)


        vTOT_SalAmt = Format(Val(lbl_AssessableValue.Text) + Val(GST_Amt), "###########0")

        Dim vTCS_StartDate As Date = #9/30/2020#
        Dim vTDS_StartDate As Date = #6/30/2021#
        Dim Led_ID As String = 0


        If chk_TCS_Tax.Checked = True Then

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)


            Dim vTCS_Led_STS As String = 0

            Dim vTCS_DED_STS As Boolean = False

            If DateDiff("d", vTCS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                vTCS_DED_STS = True

                If DateDiff("d", vTDS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                    vTCS_Led_STS = Common_Procedures.get_FieldValue(con, "ledger_head", "TCS_PURCHASE_Status", "(ledger_idno = " & Str(Val(Led_ID)) & ")")

                    vTCS_DED_STS = False
                    If Val(vTCS_Led_STS) = 1 Then
                        vTCS_DED_STS = True
                    End If

                End If

                If vTCS_DED_STS = True Then

                    txt_TCS_TaxableValue.Text = Format(Val(vTOT_SalAmt), "############0.00")


                    If Val(txt_TCS_TaxableValue.Text) > 0 Then
                        If Val(txt_TcsPerc.Text) = 0 Then
                            txt_TcsPerc.Text = "0.1"
                        End If

                        lbl_TcsAmount.Text = Format(Val(vTOT_SalAmt) * Val(txt_TcsPerc.Text) / 100, "##########0.00")

                        If chk_TCSAmount_RoundOff_STS.Checked = True Then
                            lbl_TcsAmount.Text = Format(Val(lbl_TcsAmount.Text), "##########0")
                        Else
                            lbl_TcsAmount.Text = Format(Val(lbl_TcsAmount.Text), "#########0.00")
                        End If



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


        NtAmt = Val(lbl_AssessableValue.Text) + Val(GST_Amt) + Val(lbl_TcsAmount.Text) + Val(txt_AddLess_AfterTax.Text)
        'NtAmt = Val(lbl_AssessableValue.Text) + Val(GST_Amt) + Val(txt_Freight.Text) + Val(txt_AddLess_AfterTax.Text) + Val(lbl_TcsAmount.Text)


        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")
        If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""

        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

    Private Sub Print_Selection()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Yarn_Purchase_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
    Public Sub print_record() Implements Interface_MDIActions.print_record
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Yarn_Purchase_Return_Entry, New_Entry) = False Then Exit Sub

        Print_Selection()

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as Transport_Name,CSH.State_Name as Company_State_Name  ,CSH.State_Code as Company_State_Code ,LSH.State_Name as Ledger_State_Name ,LSH.State_Code as Ledger_State_Code, f.Ledger_mainName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code from Yarn_Purchase_Return_Head a " &
                                               " INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo " &
                                               "LEFT OUTER JOIN State_HEad CSH on b.Company_State_IdNo = CSH.State_IdNo " &
                                               " INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN State_HEad " &
                                               "LSH on c.Ledger_State_IdNo = LSH.State_IdNo " &
                                               " LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo =a.Transport_IdNo " &
                                               "LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo " &
                                               "LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Mill_Name, c.Count_name,c.Count_Description from Yarn_Purchase_Return_Details a INNER JOIN Mill_Head b ON a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where a.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
        Printing_FormatGST(e)
        'End If
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
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
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
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 40
            .Top = 40
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 8 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(50) : ClArr(2) = 60 : ClArr(3) = 220 : ClArr(4) = 100 : ClArr(5) = 120 : ClArr(6) = 100
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

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


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 18 Then
                            For I = 18 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 18
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType1_Meters").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            SNo = SNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("JobWork_Piece_Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType1_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType1").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType1_Meters").ToString) * Val(prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType1").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType2_Meters").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            SNo = SNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("JobWork_Piece_Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type2, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType2_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType2").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType2_Meters").ToString) * Val(prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType2").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType3_Meters").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            SNo = SNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("JobWork_Piece_Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type3, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType3_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType3").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType3_Meters").ToString) * Val(prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType3").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType4_Meters").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            SNo = SNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("JobWork_Piece_Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type4, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType4_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType4").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType4_Meters").ToString) * Val(prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType4").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType5_Meters").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            SNo = SNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("JobWork_Piece_Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type5, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType5_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType5").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_ClothType5_Meters").ToString) * Val(prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType5").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Purchase_Return_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Return_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "JOBWORK INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 20
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("BILL NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "BILL NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Purchase_Return_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Purchase_Return_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Despatch To : " & Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " TYPE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 50
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
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

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY = CurY + TxtHgt
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_Delvat, txt_CommRate, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    txt_CommRate.Focus()
                    'dgv_Details.Focus()
                    'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    'dgv_Details.CurrentCell.Selected = True

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
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
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
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

    Private Sub cbo_Grid_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.LostFocus

    End Sub

    Private Sub cbo_Grid_ItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
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
                .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_Grid_MillName.Text)
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
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
                .Rows(.CurrentCell.RowIndex).Cells.Item(6).Value = Trim(cbo_Grid_RateFor.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Filter_DeliveryAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_DeliveryAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) )", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_DeliveryAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DeliveryAt.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DeliveryAt, txt_FilterBillNo, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) )", "(Ledger_IdNo = 0)")


        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Filter_DeliveryAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DeliveryAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DeliveryAt, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) )", "(Ledger_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    btn_Filter_Show_Click(sender, e)
        'End If
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                ' btn_Filter_Show.Focus()
                btn_Filter_Show_Click(sender, e)
            End If
        End If

    End Sub

    Private Sub txt_CommRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CommRate.KeyDown
        If e.KeyValue = 38 Then cbo_Agent.Focus()
        If e.KeyValue = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub



    Private Sub txt_Commbag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_CommType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CommType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CommType, txt_CommRate, Nothing, "", "", "", "")
        If e.KeyValue = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If

    End Sub

    Private Sub cbo_CommType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CommType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CommType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_CommRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CommRate.TextChanged
        Agent_Commission_Calculation()
    End Sub

    Private Sub cbo_Grid_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.TextChanged
        Try
            If cbo_Grid_RateFor.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_RateFor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RateFor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_Receipt.Enabled And btn_Print_Receipt.Visible Then
            btn_Print_Receipt.Focus()
        End If
    End Sub

    Private Sub btn_save_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
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


    Private Sub chk_TaxAmount_RoundOff_STS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_TaxAmount_RoundOff_STS.CheckedChanged
        Total_Calculation()
        NetAmount_Calculation()
    End Sub
    Private Sub Total_Tax_Calculation()
        Dim Sno As Integer
        Dim TotAss_Val As Single
        Dim TotCGST_amt As Single
        Dim TotSGST_amt As Double
        Dim TotIGST_amt As Double

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

        lbl_AssessableValue.Text = Format(Val(TotAss_Val), "##########0.00")
        lbl_CGSTAmount.Text = IIf(Val(TotCGST_amt) <> 0, Format(Val(TotCGST_amt), "##########0.00"), "")
        lbl_SGSTAmount.Text = IIf(Val(TotSGST_amt) <> 0, Format(Val(TotSGST_amt), "##########0.00"), "")
        lbl_IGSTAmount.Text = IIf(Val(TotIGST_amt) <> 0, Format(Val(TotIGST_amt), "##########0.00"), "")

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


                        .Rows(RowIndx).Cells(9).Value = ""
                        .Rows(RowIndx).Cells(10).Value = ""
                        .Rows(RowIndx).Cells(11).Value = ""  ' Taxable value
                        .Rows(RowIndx).Cells(12).Value = ""  ' HSN CODE
                        .Rows(RowIndx).Cells(13).Value = ""  ' GST%

                        If Trim(.Rows(RowIndx).Cells(1).Value) <> "" Or Val(.Rows(RowIndx).Cells(3).Value) = 0 Or Val(.Rows(RowIndx).Cells(4).Value) = 0 Or Val(.Rows(RowIndx).Cells(5).Value) = 0 Or Val(.Rows(RowIndx).Cells(8).Value) = 0 Then

                            HSN_Code = ""
                            GST_Per = 0
                            Get_GST_Percentage_From_ItemGroup(Trim(.Rows(RowIndx).Cells(1).Value), HSN_Code, GST_Per)


                            '--Cash discount
                            .Rows(RowIndx).Cells(9).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                            .Rows(RowIndx).Cells(10).Value = Format(Val(.Rows(RowIndx).Cells(8).Value) * (Val(.Rows(RowIndx).Cells(9).Value) / 100), "########0.00")

                            '-- Taxable value = amount -  cash disc
                            Taxable_Amount = Val(.Rows(RowIndx).Cells(8).Value) - Val(.Rows(RowIndx).Cells(10).Value)


                            .Rows(RowIndx).Cells(11).Value = Format(Val(Taxable_Amount), "##########0.00")
                            .Rows(RowIndx).Cells(12).Value = Trim(HSN_Code)
                            .Rows(RowIndx).Cells(13).Value = Format(Val(GST_Per), "########0.00")


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

                AssVal_Pack_Frgt_Ins_Amt = Format(Val(txt_Freight.Text) + Val(txt_AddLess_BeforeTax.Text), "#########0.00")

                With dgv_Details

                    If .Rows.Count > 0 Then
                        For i = 0 To .Rows.Count - 1
                            If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(13).Value) <> 0 And Trim(.Rows(i).Cells(12).Value) <> "" Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1                ,                  Currency1            ,                       Currency2                                             ) " &
                                                    "          Values     ( '" & Trim(.Rows(i).Cells(12).Value) & "', " & Val(.Rows(i).Cells(13).Value) & " ,  " & Str(Val(.Rows(i).Cells(11).Value) + Val(AssVal_Pack_Frgt_Ins_Amt)) & " ) "
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

                        If chk_TaxAmount_RoundOff_STS.Checked = True Then


                            .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "#############0")
                            If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                            .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "#############0")
                            If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                            .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "#############0")
                            If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                        Else

                            .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "#############0.00")
                            If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                            .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "#############0.00")
                            If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                            .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "#############0.00")
                            If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                        End If

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

    Private Sub btn_Print_Receipt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Receipt.Click
        prn_Status = 1
        Print_Selection()
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

        NoofItems_PerPage = 10 '14 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 30 : ClArr(2) = 70 : ClArr(3) = 75 : ClArr(4) = 50 : ClArr(5) = 190 : ClArr(6) = 55 : ClArr(7) = 90 : ClArr(8) = 75
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

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


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString)
                        If Trim(ItmNm1) = "" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_name").ToString) & "-" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        End If
                        ItmNm2 = ""

                        If Len(ItmNm1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("GST_Percentage").ToString), "############0.0") & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
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
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim I As Integer = 0
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim vLine_Pen = New Pen(Color.Black, 2)
        Dim ItmNm1 As String = "", ItmNm2 As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Purchase_Return_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Return_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
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

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 120, CurY + 10, 110, 110)

                        End If

                    End Using
                End If
            End If

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then '---ArulJothi
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "PURCHASE RETURN", LMargin, CurY, 2, PrintWidth, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "PURCHASE RETURN / DEBIT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        End If


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

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

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
            End If


        End If

        CurY = CurY + TxtHgt + 5

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 60
            W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
            S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

            W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
            'S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE    ", pFont).Width
            S2 = e.Graphics.MeasureString("SUPPLIER INV DATE    ", pFont).Width


            W3 = e.Graphics.MeasureString("CREDIT NOTE DATE", pFont).Width
            S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width


            CurY1 = CurY
            CurY2 = CurY

            '---left side

            CurY1 = CurY1 + 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font)

            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY1 = CurY1 + strHeight
            p1Font = New Font("Calibri", 11, FontStyle.Bold)


            If Trim(prn_HdDt.Rows(0).Item("Ledger_mainName").ToString) <> "" Then
                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Ledger_mainName").ToString)
            End If

            ItmNm2 = ""
            If Len(ItmNm1) > 25 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & ItmNm1, LMargin + S1 + 10, CurY1, 0, 0, p1Font)
            If ItmNm2 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + S1 + 30, CurY1, 0, 0, p1Font)
            End If
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

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString)
            End If

            ItmNm2 = ""
            If Len(ItmNm1) > 25 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & ItmNm1, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)
            If ItmNm2 <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + C1 + S1 + 30, CurY2, 0, 0, p1Font)
            End If

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
            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DEBIT NOTE NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Purchase_Return_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "SUPPLIER INV NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DEBIT NOTE DATE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Purchase_Return_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "SUPPLIER INV DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bill_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + S2 + 30, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "AGENT", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("Agent_IdNo").ToString), LMargin + W3 + 30, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("Transport_IdNo").ToString), LMargin + C1 + S2 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If prn_HdDt.Rows(0).Item("Vehicle_no").ToString <> "" Then


                Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_no").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            End If

            If prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString <> "" Then

                Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, p1Font)


            Else
            If prn_HdDt.Rows(0).Item("Lr_DateTxt").ToString <> "" And IsDate(prn_HdDt.Rows(0).Item("Lr_DateTxt").ToString) Then
                Common_Procedures.Print_To_PrintDocument(e, "LR NO : " & prn_HdDt.Rows(0).Item("Lr_No").ToString & "   LR DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Lr_DateTxt").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(vLine_Pen, PageWidth / 2, CurY, PageWidth / 2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

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

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)


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
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width
            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("note").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "NOTE :" & prn_HdDt.Rows(0).Item("note").ToString, LMargin + 10, CurY, 0, 0, pFont)
            End If




            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 10
            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If



            If Val(prn_HdDt.Rows(0).Item("TCS_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Add : TCS  @ " & Format(Val(prn_HdDt.Rows(0).Item("TCS_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("TCS_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            'CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            CurY = CurY + TxtHgt - 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(StrConv(BmsInWrds, VbStrConv.ProperCase)), "", "")

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

    Private Sub btn_Print_ReverseCharge_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_ReverseCharge.Click
        prn_Status = 2
        Print_Selection()
        btn_print_Close_Click(sender, e)
    End Sub
    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Yarn_Purchase_Return_GST_Tax_Details Where Yarn_Purchase_Return_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Yarn_Purchase_Return_GST_Tax_Details Where Yarn_Purchase_Return_Code = '" & Trim(EntryCode) & "'", con)
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
    Private Sub dtp_Lrdate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Lrdate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Lrdate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_Lrdate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Lrdate.TextChanged
        If IsDate(dtp_Lrdate.Text) = True Then

            msk_Lr_Date.Text = dtp_Lrdate.Text
            msk_Lr_Date.SelectionStart = 0
        End If
    End Sub
    Private Sub dtp_Lrdate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Lrdate.ValueChanged
        msk_Lr_Date.Text = dtp_Lrdate.Text
    End Sub

    Private Sub dtp_Lrdate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Lrdate.Enter
        msk_Lr_Date.Focus()
        msk_Lr_Date.SelectionStart = 0
    End Sub
    Private Sub msk_Lr_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Lr_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If


        If e.KeyCode = 38 Then
            txt_LrNo.Focus()
        ElseIf e.KeyCode = 40 Then
            cbo_Delvat.Focus()

        End If

    End Sub

    Private Sub msk_Lr_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Lr_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Lr_Date.Text = Date.Today
        End If

        If IsDate(msk_Lr_Date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Lr_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Lr_Date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_Lr_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Lr_Date.Text))
            End If
        End If

        'If e.KeyCode = 46 Or e.KeyCode = 8 Then
        '    Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        'End If

    End Sub
    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, msk_Lr_Date, cbo_Delvat, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, cbo_Delvat, "", "", "", "")
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Date.Text = Date.Today
        End If

        If IsDate(msk_Date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            End If
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Date.ValueChanged
        msk_Date.Text = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub


    Private Sub dtp_BillDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_BillDate.ValueChanged
        msk_BillDate.Text = dtp_BillDate.Text
    End Sub

    Private Sub dtp_BillDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_BillDate.Enter
        'msk_BillDate.Focus()
        'msk_BillDate.SelectionStart = -1
    End Sub

    Private Sub msk_BillDate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_BillDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_BillDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_BillDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BillDate.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_BillDate.Text = Date.Today
        'End If

        If IsDate(msk_BillDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_BillDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_BillDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_BillDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_BillDate.Text))
            End If
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

    Private Sub msk_BillDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BillDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_BillDate.Text
            vmskSelStrt = msk_BillDate.SelectionStart
        End If
    End Sub

    Private Sub msk_Lr_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Lr_Date.LostFocus
        If IsDate(msk_Lr_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Lr_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_BillDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Lr_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_BillDate.Text)) >= 2000 Then
                    dtp_Lrdate.Value = Convert.ToDateTime(msk_Lr_Date.Text)
                End If
            End If
        End If
    End Sub


    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer = 0, SNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim NewCode As String = ""
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0

        Exit Sub

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

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name,c.*  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno  INNER JOIN Own_Order_Yarn_Details c ON  c.Own_Order_Code = a.Own_Order_Code LEFT OUTER JOIN Yarn_Purchase_Return_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "'   and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
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

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno INNER JOIN Own_Order_Yarn_Details c ON  c.Own_Order_Code = a.Own_Order_Code   LEFT OUTER JOIN Yarn_Purchase_Return_Head d ON d.Yarn_Purchase_Return_Code = a.Own_Order_Code    where a.Yarn_Purchase_Return_Code = ''  and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
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

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno   LEFT OUTER JOIN Yarn_Purchase_Return_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "'   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
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

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno    LEFT OUTER JOIN Yarn_Purchase_Return_Head d ON d.Yarn_Purchase_Return_Code = a.Own_Order_Code    where a.Yarn_Purchase_Return_Code = ''   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
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

        If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()



    End Sub

    Private Sub cbo_Verified_Sts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Verified_Sts.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub
    Private Sub cbo_Verified_Sts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Verified_Sts.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, cbo_Filter_DeliveryAt, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub


    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub


    Private Sub txt_TCS_TaxableValue_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TCS_TaxableValue.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_TCS_TaxableValue_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_TCS_TaxableValue.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub btn_EDIT_TCS_TaxableValue_Click(sender As System.Object, e As System.EventArgs) Handles btn_EDIT_TCS_TaxableValue.Click
        txt_TCS_TaxableValue.Enabled = Not txt_TCS_TaxableValue.Enabled
        txt_TcsPerc.Enabled = Not txt_TcsPerc.Enabled
        If txt_TCS_TaxableValue.Enabled Then
            txt_TCS_TaxableValue.Focus()

        Else
            txt_Note.Focus()

        End If
    End Sub

    Private Sub txt_TcsPerc_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TcsPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_TcsPerc_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TcsPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub chk_TCS_Tax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCS_Tax.CheckedChanged
        NetAmount_Calculation()
    End Sub
    Private Sub chk_TCSAmount_RoundOff_STS_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCSAmount_RoundOff_STS.CheckedChanged
        NetAmount_Calculation()
    End Sub
    Private Sub btn_EInvoice_Generation_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click
        'rtbeInvoiceResponse.Text = ""
        'txt_EWBNo.Text = txt_Electronic_RefNo.Text

        btn_GENERATEEWB.Enabled = True
        btn_Generate_eInvoice.Enabled = True


        grp_EInvoice.Visible = True
        grp_EInvoice.BringToFront()
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2

        btn_CheckConnectivity1.Enabled = False
        btn_CheckConnectivity1.Visible = False

    End Sub
    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click

        btn_GENERATEEWB.Enabled = True
        btn_Generate_eInvoice.Enabled = True


        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        'Grp_EWB.Left = (Me.Width - grp_EInvoice.Width) / 2
        'Grp_EWB.Top = (Me.Height - grp_EInvoice.Height) / 2 + 200
        Grp_EWB.Location = New Point(250, 250)

    End Sub
    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Yarn_Purchase_Return_Details Where Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Yarn_Purchase_Return_Head Where Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) >0"
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


            Cmd.CommandText = "Insert into e_Invoice_Head (     e_Invoice_No             ,    e_Invoice_date         ,        Buyer_IdNo,         Consignee_IdNo,       Assessable_Value      ,     CGST      ,   SGST            ,   IGST       ,    Cess,    State_Cess,     Round_Off         , Nett_Invoice_Value ,      Ref_Sales_Code      ,                   Other_Charges                   ,       Dispatcher_idno )" &
                              "Select                          Yarn_Purchase_Return_No   , Yarn_Purchase_Return_Date ,       Ledger_IdNo,        DeliveryTo_Idno,      Assessable_Value     ,   CGst_Amount    ,  SGST_Amount     ,  IGST_Amount  ,      0   ,        0    ,   RoundOff_Amount    ,    Net_Amount      ,  '" & Trim(NewCode) & "' , (ISNULL(TCS_Amount,0)+ ISNULL(AddLess_Amount,0)) , DeliveryTo_Idno  from Yarn_Purchase_Return_Head where Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "'"

            Cmd.ExecuteNonQuery()

            Dim vPARTICULARS_FIELDNAME As String = ""

            If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
                vPARTICULARS_FIELDNAME = "(c.Count_Name)"  '"(c.Count_Name + ' ' + Ig.Item_Hsn_Code)" ' "(c.Count_Name + ' ' + c.Count_Description)"
            Else
                vPARTICULARS_FIELDNAME = "(c.Count_Name + ' ' + c.Count_Description + '-' + d.Mill_Name)"
            End If

            'Cmd.CommandText = "Insert into e_Invoice_Details (Sl_No, IsService, Product_Description                               , HSN_Code, Batch_Details, Quantity, Unit,   Unit_Price,     Total_Amount                                                                                             ,                                Discount                    ,      Assessable_Amount                                                                                                            ,   GST_Rate      , SGST_Amount, IGST_Amount, CGST_Amount, Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails, Ref_Sales_Code )" &
            '                                        " Select a.Sl_No, 0,       " & vPARTICULARS_FIELDNAME & " as producDescription, a.HSN_Code, ''          , Weight ,'KGS', a.Rate      , (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Freight_Amount+b.Packing_Amount+b.AddLess_Amount) else 0 end )  ) , (CASE WHEN a.sl_no = 1 then b.Discount_Amount ELSE 0 END ) ,  (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Freight_Amount+b.Packing_Amount+b.AddLess_Amount-b.Discount_Amount ) else 0 end )  )  , a.GST_Percentage, 0          , 0          , 0          , 0        , 0          , 0                , 0              , 0                , 0                     , 0           ,   0             , ''               ,'" & Trim(NewCode) & "' " &
            '                                        " from Yarn_Purchase_Return_Details a " &
            '                                        " INNER JOIN Yarn_Purchase_Return_Head b  ON a.Yarn_Purchase_Return_Code =  b.Yarn_Purchase_Return_Code " &
            '                                        " inner join Count_head C on a.Count_IdNo = c.Count_IdNo " &
            '                                        " inner join Mill_head d on a.Mill_IdNo = d.Mill_IdNo LEFT OUTER JOIN Itemgroup_head Ig on ig.ItemGroup_IdNo = c.ItemGroup_IdNo  " &
            '                                        " Where a.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "'"



            Cmd.CommandText = "Insert into e_Invoice_Details (	Sl_No , IsService,                                   Product_Description                               , HSN_Code, Batch_Details, Quantity, Unit, Unit_Price,											         Total_Amount                                           ,                                Discount                                  ,										                    Assessable_Amount                                                           ,   GST_Rate      , SGST_Amount, IGST_Amount, CGST_Amount, Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails ,       Ref_Sales_Code ) " &
                               " Select                         a.Sl_No,    0     , (c.Count_Name + ' ' + c.Count_Description + '-' + d.Mill_Name) as producDescription, a.HSN_Code, ''          , Weight ,'KGS' , a.Rate    , (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Freight_Amount+b.AddLess_BeforeTax_Amount) else 0 end )  )AS Total_Amount , (CASE WHEN a.sl_no = 1 then b.Discount_Amount ELSE 0 END )  AS Discount ,  (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Freight_Amount+b.AddLess_BeforeTax_Amount-b.Discount_Amount ) else 0 end )  ) AS  Assessable_Amount , a.GST_Percentage, 0          , 0          , 0          , 0        , 0          , 0                , 0              , 0                , 0                     , 0           ,   0             , ''               ,'" & Trim(NewCode) & "' " &
                              "from Yarn_Purchase_Return_Details a " &
                              "INNER JOIN Yarn_Purchase_Return_Head b  ON a.Yarn_Purchase_Return_Code =  b.Yarn_Purchase_Return_Code " &
                              "inner join Count_head C on a.Count_IdNo = c.Count_IdNo " &
                              "inner join Mill_head d on a.Mill_IdNo = d.Mill_IdNo  " &
                              "LEFT OUTER JOIN Itemgroup_head Ig on ig.ItemGroup_IdNo = c.ItemGroup_IdNo  " &
                              "Where a.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()


            tr.Commit()


        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try

        btn_Generate_eInvoice.Enabled = False

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Yarn_Purchase_Return_Head", "Yarn_Purchase_Return_Code", Trim(Pk_Condition), "DBN")

    End Sub

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        If Len(Trim(txt_EInvoiceCancellationReson.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Yarn_Purchase_Return_Head", "Yarn_Purchase_Return_Code", con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)

    End Sub

    Private Sub btn_Get_QR_Code_Click(sender As Object, e As EventArgs) Handles btn_Get_QR_Code.Click

        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con

        CMD.CommandText = "DELETE FROM " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_refresh where IRN = '" & txt_eInvoiceNo.Text & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = " INSERT INTO " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_Refresh ([IRN] ,[ACK_No] , [DOC_No] , [SEARCH_BY]  , [COMPANY_IDNO],[Update_Table] ,[Update_table_Unique_Code],[COMPANYGROUP_IDNO] ) VALUES " &
                          "('" & txt_eInvoiceNo.Text & "' ,'','','I'," & Val(Common_Procedures.CompIdNo).ToString & ",'Yarn_Purchase_Return_Head', 'E_Invoice_IRNO'," & Val(Common_Procedures.CompGroupIdNo).ToString & ")"
        CMD.ExecuteNonQuery()

        Shell(Application.StartupPath & "\Refresh_IRN.EXE")

    End Sub

    Private Sub txt_eInvoiceNo_TextChanged(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.TextChanged
        txt_IR_No.Text = txt_eInvoiceNo.Text
    End Sub

    Private Sub btn_refresh_Click(sender As Object, e As EventArgs) Handles btn_refresh.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select E_Invoice_QR_Image,E_Invoice_IRNO,E_Invoice_ACK_No,E_Invoice_ACK_Date,E_Invoice_Cancelled_Status FROM  Yarn_Purchase_Return_Head WHERE Yarn_Purchase_Return_Code = '" & NewCode & "'", con)

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



    Private Sub btn_Print_EWB_Click(sender As Object, e As EventArgs)
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_ElectronicRefNo.Text, rtbeInvoiceResponse, 0)
    End Sub
    Private Sub btn_Detail_PRINT_EWB_Click(sender As Object, e As EventArgs)
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_ElectronicRefNo.Text, rtbEWBResponse, 1)
    End Sub
    Private Sub txt_IR_No_TextChanged(sender As Object, e As EventArgs) Handles txt_IR_No.TextChanged
        txt_eInvoiceNo.Text = txt_IR_No.Text

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

    Private Sub txt_vehicle_no_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_vehicle_no.KeyDown
        If e.KeyCode = 38 Then
            cbo_Transport.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_IR_No.Focus()

        End If
    End Sub

    Private Sub txt_vehicle_no_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_vehicle_no.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_IR_No.Focus()

        End If
    End Sub

    Private Sub txt_IR_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_IR_No.KeyDown
        If e.KeyCode = 38 Then
            txt_vehicle_no.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_ElectronicRefNo.Focus()

        End If
    End Sub

    Private Sub txt_IR_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_IR_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_ElectronicRefNo.Focus()
        End If
    End Sub

    Private Sub txt_ElectronicRefNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_ElectronicRefNo.KeyDown
        If e.KeyCode = 38 Then
            txt_IR_No.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_Note.Focus()

        End If
    End Sub

    Private Sub txt_ElectronicRefNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_ElectronicRefNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Note.Focus()
        End If
    End Sub
    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()


        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select Electronic_Reference_No from Yarn_Purchase_Return_Head where Yarn_Purchase_Return_Code = '" & NewCode & "'", con)
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


        If Len(Trim(cbo_Delvat.Text)) = 0 Then

            'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
            '             "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
            '             "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
            '             "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
            '             "[VehicleNo]      ,[VehicleType]   , [InvCode]) " &
            '             " " &
            '             " " &
            '             "  SELECT               'O'              , '1'             ,   ''              ,    'INV'    , a.Yarn_Purchase_Return_No ,a.Yarn_Purchase_Return_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
            '             " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo  ,L.Ledger_Name,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,L.City_Town,L.Pincode, TS.State_Code,TS.State_Code," &
            '             " 1                     ,a.AddLess_Amount + a.RoundOff_Amount, A.Total_Taxable_Value    , A.Total_CGST_Amount  ,  A.Total_SGST_Amount , A.Total_IGST_Amount   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
            '             " a.LR_No        ,         a.Lr_Date            , a.Net_Amount         ,     CASE    WHEN a.Transport_Mode = 'Rail' THEN '2'  WHEN a.Transport_Mode = 'Air' THEN '3'  WHEN a.Transport_Mode = 'Ship' THEN '4'    ELSE '1' END AS TrMode ," &
            '             " a.Vehicle_No,'R','" & NewCode & "' from Yarn_Purchase_Return_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
            '             " Inner Join Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Left Outer Join State_Head FS On " &
            '             " C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  where a.Yarn_Purchase_Return_Code = '" & NewCode & "'"



            CMD.CommandText = "Insert into EWB_Head ([SupplyType]  , [SubSupplyType]  , [SubSupplyDesc]  ,	[DocType]  ,		[EWBGenDocNo]		,				[EWBDocDate]	    ,  [FromGSTIN]       ,   [FromTradeName]  ,					[FromAddress1]			,					[FromAddress2]      ,	[FromPlace]		,	[FromPINCode]      ,	[FromStateCode]		,	[ActualFromStateCode] ,		[ToGSTIN]     ,		[ToTradeName]	,					[ToAddress1]      ,						[ToAddress2]    ,	[ToPlace]       ,	[ToPINCode] ,	[ToStateCode] ,		[ActualToStateCode] ,[TransactionType],					[OtherValue]		   ,	[Total_value]      ,  [CGST_Value]	,	[SGST_Value]	,	[IGST_Value]    ,	[CessValue],[CessNonAdvolValue]       ,	   [TransporterID]    , 	[TransporterName]   ,  	[TransportDOCNo]   , [TransportDOCDate]    ,[TotalInvValue]    ,    [TransMode]  ,	[VehicleNo]     ,	[VehicleType]   , [InvCode])  " &
                                        "SELECT   	  'O'          ,		 '1'      ,			 ''      ,    'INV'    ,  a.Yarn_Purchase_Return_No ,	 a.Yarn_Purchase_Return_Date	, C.Company_GSTINNo	 ,   C.Company_Name   ,C.Company_Address1+C.Company_Address2	,c.Company_Address3+C.Company_Address4	,	C.Company_City	, C.Company_PinCode    ,	 FS.State_Code		,		FS.State_Code     , L.Ledger_GSTINNo  ,		L.Ledger_Name	,L.Ledger_Address1+L.Ledger_Address2  ,L.Ledger_Address3+L.Ledger_Address4	,	L.City_Town		,   L.Pincode   ,   TS.State_Code ,		TS.State_Code		,		 1        ,	a.AddLess_Amount + a.RoundOff_Amount   , A.Assessable_Value    , A.CGST_Amount  ,  A.SGST_Amount	, A.IGST_Amount     ,		 0     ,		0	              ,	    t.Ledger_GSTINNo  ,		t.Ledger_Name   	,		Null as lrNO   , Null as LrDate        , a.Net_Amount      ,   '1' AS TrMode ,	a.Vehicle_No	,		'R'			,'" & NewCode & "'  " &
                                        "from Yarn_Purchase_Return_Head a   " &
                                        "inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo  " &
                                        "Inner Join Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo  " &
                                        "left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo  " &
                                        "Left Outer Join State_Head FS On  C.Company_State_IdNo = fs.State_IdNo  " &
                                        "left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo   " &
                                        "where a.Yarn_Purchase_Return_Code = '" & NewCode & "'"




        Else

            'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
            '             "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
            '             "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
            '             "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
            '             "[VehicleNo]      ,[VehicleType]   , [InvCode]) " &
            '             " " &
            '             " " &
            '             "  SELECT               'O'              , '1'             ,   ''              ,    'INV'    ,  a.Yarn_Purchase_Return_No , a.Yarn_Purchase_Return_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
            '             " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo  ,L.Ledger_Name,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,L.City_Town,L.Pincode, TS.State_Code,TS.State_Code," &
            '             " 1                     ,a.AddLess_Amount + a.RoundOff_Amount, A.Total_Taxable_Value    , A.Total_CGST_Amount  ,  A.Total_SGST_Amount , A.Total_IGST_Amount   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
            '             " a.LR_No        , a.Lr_Date         , a.Net_Amount         ,     CASE    WHEN a.Transport_Mode = 'Rail' THEN '2'  WHEN a.Transport_Mode = 'Air' THEN '3'  WHEN a.Transport_Mode = 'Ship' THEN '4'    ELSE '1' END AS TrMode ," &
            '             " a.Vehicle_No,'R','" & NewCode & "' from Yarn_Purchase_Return_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
            '             " Inner Join Ledger_Head L on a.DeliveryTo_IdNo = L.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Left Outer Join State_Head FS On " &
            '             " C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  where a.Yarn_Purchase_Return_Code = '" & NewCode & "'"



            CMD.CommandText = "Insert into EWB_Head ( [SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,	[DocType]  ,		[EWBGenDocNo]		,				[EWBDocDate]	    ,  [FromGSTIN]       ,   [FromTradeName]  ,					[FromAddress1]			,					[FromAddress2]      ,	[FromPlace]		,	[FromPINCode]      ,	[FromStateCode]		,	[ActualFromStateCode] ,		[ToGSTIN]     ,		[ToTradeName]	,					[ToAddress1]      ,						[ToAddress2]    ,	[ToPlace]       ,	[ToPINCode] ,	[ToStateCode] ,		[ActualToStateCode] ,[TransactionType],					[OtherValue]		   ,	[Total_value]      ,  [CGST_Value]	,	[SGST_Value]	,	[IGST_Value]    ,	[CessValue],[CessNonAdvolValue],	[TransporterID]    ,	[TransporterName]   ,  	[TransportDOCNo]   , [TransportDOCDate]    ,[TotalInvValue]    ,    [TransMode]  ,	[VehicleNo]     ,	[VehicleType]   , [InvCode])   " &
                           "SELECT		    	    	  'O'       ,		 '1'      ,			 ''      ,    'INV'    ,  a.Yarn_Purchase_Return_No ,	 a.Yarn_Purchase_Return_Date	, C.Company_GSTINNo	 ,   C.Company_Name   ,C.Company_Address1+C.Company_Address2	,c.Company_Address3+C.Company_Address4	,	C.Company_City	, C.Company_PinCode    ,	 FS.State_Code		,		FS.State_Code     , L.Ledger_GSTINNo  ,		L.Ledger_Name	,L.Ledger_Address1+L.Ledger_Address2  ,L.Ledger_Address3+L.Ledger_Address4	,	L.City_Town		,   L.Pincode   ,   TS.State_Code ,		TS.State_Code		,		 1        ,	a.AddLess_Amount + a.RoundOff_Amount   , A.Assessable_Value    , A.CGST_Amount  ,  A.SGST_Amount	, A.IGST_Amount     ,		 0     ,		0	       ,	t.Ledger_GSTINNo  ,		t.Ledger_Name   	,		Null as lrNO   , Null as LrDate        , a.Net_Amount      ,   '1' AS TrMode ,	a.Vehicle_No	,		'R'			,'" & NewCode & "'  " &
                           "from Yarn_Purchase_Return_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo  " &
                           "Inner Join Ledger_Head L on a.DeliveryTo_IdNo = L.Ledger_IdNo  " &
                           "left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo  " &
                           "Left Outer Join State_Head FS On  C.Company_State_IdNo = fs.State_IdNo  " &
                           "left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  " &
                           "where a.Yarn_Purchase_Return_Code = '" & NewCode & "'"




        End If





        CMD.ExecuteNonQuery()

        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim dt1 As New DataTable



        da = New SqlClient.SqlDataAdapter(" Select  I.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.Item_GST_Percentage,sum(SD.Taxable_Value) As TaxableAmt,sum(SD.Weight) as Qty,Min(Sl_No), 'WGT' AS Units " &
                                          " from Yarn_Purchase_Return_Details SD Inner Join Count_Head I On SD.Count_IdNo = I.Count_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " Where SD.Yarn_Purchase_Return_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage", con)
        dt1 = New DataTable
        da.Fill(dt1)

        For I = 0 To dt1.Rows.Count - 1

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode) " &
                              " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",'KGS'          ," & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "')"

            CMD.ExecuteNonQuery()

        Next

        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Yarn_Purchase_Return_Head", "Electronic_Reference_No", "Yarn_Purchase_Return_Code", Pk_Condition)


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

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_ElectronicRefNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Yarn_Purchase_Return_Head", "Electronic_Reference_No", "Yarn_Purchase_Return_Code")

    End Sub

    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_ElectronicRefNo.Text, rtbEWBResponse, 0)
    End Sub
    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_ElectronicRefNo.Text, rtbEWBResponse, 1)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_ElectronicRefNo.Text = txt_EWBNo.Text
    End Sub

    Private Sub txt_ElectronicRefNo_TextChanged(sender As Object, e As EventArgs) Handles txt_ElectronicRefNo.TextChanged
        txt_EWBNo.Text = txt_ElectronicRefNo.Text
    End Sub
    Private Sub msk_Lr_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_Lr_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Delvat.Focus()

        End If
    End Sub

    Private Sub cbo_YarnPurc_SufixNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_YarnPurc_SufixNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_YarnPurc_SufixNo, msk_Date, "", "", "", "", False)
    End Sub

    Private Sub cbo_YarnPurc_SufixNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_YarnPurc_SufixNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnPurc_SufixNo, txt_YarnPurc_PrefixNo, msk_Date, "", "", "", "")
    End Sub
    Private Sub Get_TCS_STS_From_Ledger()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Led_ID As Integer
        Dim vDESP_LEDID As Integer
        Dim vDESPTO As String
        Dim vTDSDED_STS As String
        Dim vTCSDED_STS As String

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If Led_ID = 0 Then Exit Sub


        vDESPTO = ""
        vTCSDED_STS = ""
        vTDSDED_STS = ""

        da1 = New SqlClient.SqlDataAdapter("Select a.* from Ledger_Head a Where a.Ledger_IdNo = " & Str(Val(Led_ID)), con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)("TCS_PURCHASE_Status").ToString) = False Then
                vTCSDED_STS = dt1.Rows(0)("TCS_PURCHASE_Status").ToString
            End If
            If IsDBNull(dt1.Rows(0)("PURCHASE_TDS_Deduction_Status").ToString) = False Then
                vTDSDED_STS = dt1.Rows(0)("PURCHASE_TDS_Deduction_Status").ToString
            End If
        End If
        dt1.Clear()

        vDESPTO = ""
        da1 = New SqlClient.SqlDataAdapter("Select a.*, b.area_name from Ledger_Head a LEFT OUTER JOIN Area_Head b ON a.area_idno = b.area_idno Where a.Ledger_IdNo = " & Str(Val(Led_ID)), con)
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

        cbo_PartyName.Tag = cbo_PartyName.Text

    End Sub
    Private Sub cbo_PartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.LostFocus
        If Trim(UCase(cbo_PartyName.Tag)) <> Trim(UCase(cbo_PartyName.Text)) Then
            cbo_PartyName.Tag = cbo_PartyName.Text
            GST_Calculation()
            Get_TCS_STS_From_Ledger()
        End If
    End Sub
End Class