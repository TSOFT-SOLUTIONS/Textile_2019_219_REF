Imports System.Drawing.Printing
Imports System.IO

Public Class ClothSales_Return_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GCLSR-"
    Private Prec_ActCtrl As New Control
    Private NoCalc_Status As Boolean = False
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private GSTPerc As Single = 0

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private prn_Count As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private Print_PDF_Status As Boolean = False

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False
        pnl_Selection.Visible = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        grp_EInvoice.Visible = False
        Grp_EWB.Visible = False


        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_InvNo.Text = ""
        cbo_PartyName.Text = ""
        cbo_PartyName.Tag = ""
        cbo_Cloth.Text = ""
        cbo_Type.Text = ""
        txt_InvNo.Text = ""
        Txt_folding.Text = ""
        cbo_SalesAc.Text = ""
        txt_Rate.Text = ""

        txt_NoOfPcs.Text = ""
        txt_ShortMtrs.Text = ""
        txt_PcsNoFrom.Text = "1"

        lbl_PcsNoTo.Text = ""
        txt_Meters.Text = ""
        cbo_Transport.Text = ""
        txt_Freight.Text = ""
        cbo_DeliveryTo.Text = ""
        msk_Invoice_Date.Text = ""

        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        lbl_AssessableValue.Text = ""
        lbl_Net_Amt.Text = ""
        lbl_Grid_HSNCode.Text = ""
        lbl_Amount.Text = ""
        lbl_Trade_Disc_Perc.Text = ""
        lbl_Cash_Disc_Perc.Text = ""
        lbl_Grid_GstPerc.Text = ""

        txt_Insurance.Text = ""
        txt_Packing.Text = ""
        txt_Trade_Disc.Text = ""
        txt_Cash_Disc.Text = ""
        txt_vehicle_no.Text = ""
        '------------------

        pic_IRN_QRCode_Image.BackgroundImage = Nothing
        txt_eInvoiceNo.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckNo.Enabled = True
        txt_eInvoice_CancelStatus.Enabled = False
        txt_eInvoiceAckDate.Text = ""
        txt_eInvoice_CancelStatus.Text = ""
        txt_EInvoiceCancellationReson.Text = ""

        rtbeInvoiceResponse.Text = ""

        chk_Einvoice_No_Sts.Checked = False
        chk_Ewb_No_Sts.Checked = False

        txt_EWBNo.Text = ""
        rtbEWBResponse.Text = ""

        '------------------

        txt_Note.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        lbl_ClothSales_Return_Code.Text = ""
        lbl_ClothSales_Return_SlNo.Text = ""

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

        chk_TCS_Tax.Checked = True

        lbl_Invoice_Value_Before_TCS.Text = ""
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details.AllowUserToAddRows = True

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        txt_InvNo.Enabled = True
        txt_InvNo.BackColor = Color.White

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Cloth.Enabled = True
        cbo_Cloth.BackColor = Color.White

        txt_NoOfPcs.Enabled = True
        txt_NoOfPcs.BackColor = Color.White

        txt_PcsNoFrom.Enabled = True
        txt_PcsNoFrom.BackColor = Color.White

        txt_Meters.Enabled = True
        txt_Meters.BackColor = Color.White

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Cloth.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()
        End If

        NoCalc_Status = False

    End Sub


    Private Sub NetAmount_Calculation()
        Dim InvMtrs As Single = 0, TtMtrs As Single = 0, Fmt As Single = 0
        Dim GrsAmt As Single = 0
        Dim NtAmt As Single = 0
        Dim InterStateStatus As Boolean = False
        Dim AssVal As Double = 0, BlAmt As Double = 0
        Dim AssAmt As Single = 0
        Dim CGSTAmt As Single = 0
        Dim SGSTAmt As Single = 0
        Dim IGSTAmt As Single = 0
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim Led_IdNo As Integer
        Dim ItmGrpID As Integer = 0
        Dim Tax_Amt As Double = 0
        Dim vTCS_AssVal As String = 0
        Dim vTOT_SalAmt As String = 0
        Dim vTCS_Amt As String = 0
        Dim vInvAmt_Bfr_TCS As String = 0
        Dim vTotMtr As String = 0

        Dim Fldmtr As Double = 0
        Dim Fldperc As String = 0
        Dim VBefFldAmount As String = "0"
        Dim VAfFldAmount As String = "0"

        If NoCalc_Status = True Then Exit Sub


        vTotMtr = Format(Val(txt_Meters.Text) + Val(txt_ShortMtrs.Text), "#########0.00")

        If Val(Txt_folding.Text) = 0 Or Val(Txt_folding.Text) = 100 Then
            fldmtr = Val(vTotMtr)

        Else

            '----------------


            Fldperc = Format(100 - Val(Txt_folding.Text), "##########0.00")
            Fmt = Val(vTotMtr) * Val(Fldperc) / 100


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1152" Then
                Fmt = Format(Math.Abs(Val(Fmt)), "######0.00")
                Fmt = Common_Procedures.Meter_RoundOff(Fmt)
            End If

            If (100 - Val(Txt_folding.Text)) > 0 Then
                Fldmtr = Format(Val(vTotMtr) - Val(Fmt), "#########0.00")
            Else
                Fldmtr = Format(Val(vTotMtr) + Val(Fmt), "#########0.00")
            End If


            '--------------------

            'Fmt = ((100 - Val(Txt_folding.Text)) / 100) * Val(vTotMtr)

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1152" Then
            '    Fmt = Format(Math.Abs(Val(Fmt)), "######0.00")
            '    Fmt = Common_Procedures.Meter_RoundOff(Fmt)
            'End If

            'If (100 - Val(Txt_folding.Text)) > 0 Then
            '    Fldmtr = Format(Val(vTotMtr) - Val(Fmt), "#########0.00")
            'Else
            '    Fldmtr = Format(Val(vTotMtr) + Val(Fmt), "#########0.00")
            'End If




        End If



        GrsAmt = Val(Fldmtr) * Val(txt_Rate.Text)

        lbl_Amount.Text = Format(Val(GrsAmt), "#########0.00")

        lbl_Trade_Disc_Perc.Text = Format(Val(GrsAmt) * Val(txt_Trade_Disc.Text) / 100, "########0.00")

        lbl_Cash_Disc_Perc.Text = Format(Val(GrsAmt) * Val(txt_Cash_Disc.Text) / 100, "########0.00")

        AssVal = Val(GrsAmt) - Val(lbl_Trade_Disc_Perc.Text) - Val(lbl_Cash_Disc_Perc.Text) + (Val(txt_Insurance.Text) + Val(txt_Freight.Text) + Val(txt_Packing.Text))

        lbl_AssessableValue.Text = Format(Val(AssVal), "#########0.00")



        'lbl_AssessableValue.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_AssessableValue.Text)))

        '---------------------------------
        Led_IdNo = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "Ledger_Name = '" & Trim(cbo_PartyName.Text) & "'"))
        InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), Led_IdNo)


        lbl_CGST_Amount.Text = "0.00"

        lbl_SGST_Amount.Text = "0.00"

        lbl_IGST_Amount.Text = "0.00"
        lbl_Net_Amt.Text = ""
        lbl_Grid_HSNCode.Text = ""



        ItmGrpID = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "ItemGroup_IdNo", "Cloth_Name = '" & Trim(cbo_Cloth.Text) & "'"))

        lbl_Grid_HSNCode.Text = Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_HSN_Code", "ItemGroup_IdNo = '" & Trim(Val(ItmGrpID)) & "'")

        GSTPerc = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_GST_Percentage", "ItemGroup_IdNo = '" & Trim(Val(ItmGrpID)) & "'"))
        lbl_Grid_GstPerc.Text = Val(GSTPerc)

        If InterStateStatus = True Then
            '-IGST 
            lbl_IGST_Amount.Text = Format(Val(lbl_AssessableValue.Text) * Val(GSTPerc) / 100, "#########0.00")

        Else
            '-CGST 
            GSTPerc = Val(GSTPerc) / 2
            lbl_CGST_Amount.Text = Format(Val(lbl_AssessableValue.Text) * Val(GSTPerc) / 100, "#########0.00")
            '-SGST 
            lbl_SGST_Amount.Text = Format(Val(lbl_AssessableValue.Text) * Val(GSTPerc) / 100, "#########0.00")

        End If

        Tax_Amt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)



        If Val(lbl_TotalSales_Amount_Current_Year.Text) = 0 Then lbl_TotalSales_Amount_Current_Year.Text = "0.00"
        If Val(lbl_TotalSales_Amount_Previous_Year.Text) = 0 Then lbl_TotalSales_Amount_Previous_Year.Text = "0.00"

        Dim vTCS_StartDate As Date = #9/30/2020#
        Dim vMIN_TCS_assval As String = "5000000"

        If chk_TCS_Tax.Checked = True Then

            If DateDiff("d", vTCS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                If txt_TCS_TaxableValue.Enabled = False Then

                    vTOT_SalAmt = Format(Val(lbl_AssessableValue.Text) + Val(Tax_Amt), "###########0")

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

        vInvAmt_Bfr_TCS = Format(Val(lbl_AssessableValue.Text) + Val(Tax_Amt), "###########0.00")
        lbl_Invoice_Value_Before_TCS.Text = Format(Val(vInvAmt_Bfr_TCS), "###########0")
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = Format(Val(lbl_Invoice_Value_Before_TCS.Text) - Val(vInvAmt_Bfr_TCS), "###########0.00")

        NtAmt = Val(GrsAmt) - Val(lbl_Trade_Disc_Perc.Text) - Val(lbl_Cash_Disc_Perc.Text) + Val(txt_Insurance.Text) + Val(txt_Freight.Text) + Val(txt_Packing.Text) + Tax_Amt + Val(lbl_TcsAmount.Text)

        lbl_Net_Amt.Text = Format(Val(NtAmt), "#########0")

        lbl_Net_Amt.Text = Format(Val(lbl_Net_Amt.Text), "##########0.00")
        'lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amt.Text)))

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

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then  dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim LockSTS As Boolean = False
        Dim LtCd As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from ClothSales_Return_Head a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("ClothSales_Return_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("ClothSales_Return_Date").ToString
                msk_Date.Text = dtp_Date.Text
                dtp_Invoice_Date.Text = dt1.Rows(0).Item("Invoice_Date").ToString
                msk_Invoice_Date.Text = dtp_Invoice_Date.Text
                cbo_Type.Text = dt1.Rows(0).Item("ClothSales_Return_Type").ToString
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))
                txt_Rate.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "########0.00")
                txt_InvNo.Text = dt1.Rows(0).Item("Invoice_No").ToString
                txt_NoOfPcs.Text = Val(dt1.Rows(0).Item("noof_pcs").ToString)
                txt_ShortMtrs.Text = Format(Val(dt1.Rows(0).Item("Short_Meters").ToString), "########0.00")
                txt_PcsNoFrom.Text = dt1.Rows(0).Item("pcs_fromno").ToString
                lbl_PcsNoTo.Text = dt1.Rows(0).Item("pcs_tono").ToString
                txt_Meters.Text = Format(Val(dt1.Rows(0).Item("Return_Meters").ToString), "########0.00")
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "########0.00")
                If Val(txt_Freight.Text) = 0 Then
                    txt_Freight.Text = ""
                End If
                Txt_folding.Text = Format(Val(dt1.Rows(0).Item("Folding_percentage").ToString), "########0.00")
                txt_vehicle_no.Text = (dt1.Rows(0).Item("Vehicle_no").ToString)
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                lbl_ClothSales_Return_Code.Text = dt1.Rows(0).Item("ClothSales_Invoice_Code").ToString
                lbl_ClothSales_Return_SlNo.Text = dt1.Rows(0).Item("ClothSales_Invoice_SlNo").ToString

                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                lbl_Grid_HSNCode.Text = dt1.Rows(0).Item("Item_HSN_Code").ToString
                lbl_Grid_GstPerc.Text = dt1.Rows(0).Item("Item_GST_Perc").ToString
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("CGST_Amount").ToString), "########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("SGST_Amount").ToString), "########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("IGST_Amount").ToString), "########0.00")
                lbl_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Total_Taxable_Amount").ToString), "#########0.00")
                lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00"))
                lbl_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")

                 txt_Packing.Text = dt1.Rows(0).Item("Packing_Amount").ToString
             
                txt_Trade_Disc.Text = dt1.Rows(0).Item("Trade_Discount").ToString
                lbl_Trade_Disc_Perc.Text = dt1.Rows(0).Item("Trade_Discount_Perc").ToString
                txt_Cash_Disc.Text = dt1.Rows(0).Item("Cash_Discount").ToString
                lbl_Cash_Disc_Perc.Text = dt1.Rows(0).Item("Cash_Discount_Perc").ToString
          
                If dt1.Rows(0).Item("Insurance_Name").ToString <> "" Then
                    txt_Insurance_Name.Text = dt1.Rows(0).Item("Insurance_Name").ToString
                End If
                If dt1.Rows(0).Item("CashDisc_Name").ToString <> "" Then
                    txt_CashDic_Name.Text = dt1.Rows(0).Item("CashDisc_Name").ToString
                End If
                If dt1.Rows(0).Item("TradeDisc_Name").ToString <> "" Then
                    txt_TradeDic_Name.Text = dt1.Rows(0).Item("TradeDisc_Name").ToString
                End If
                If dt1.Rows(0).Item("Insurance_Amount").ToString <> "" Then
                    txt_Insurance.Text = dt1.Rows(0).Item("Insurance_Amount").ToString
                End If
                If dt1.Rows(0).Item("Packing_Name").ToString <> "" Then
                    txt_Packing_Name.Text = dt1.Rows(0).Item("Packing_Name").ToString
                End If
           

                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))



                lbl_Invoice_Value_Before_TCS.Text = dt1.Rows(0).Item("Invoice_Value_Before_TCS").ToString
                lbl_RoundOff_Invoice_Value_Before_TCS.Text = dt1.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString
                If IsDBNull(dt1.Rows(0).Item("Tcs_Name_caption").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Tcs_Name_caption").ToString) <> "" Then
                        txt_Tcs_Name.Text = dt1.Rows(0).Item("Tcs_Name_caption").ToString
                    End If
                End If

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
                'LtCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.LotCode.Sales_Return_GST) & "/" & Trim(Common_Procedures.FnYearCode)
                If Val(dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False


                '-----------------------------


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

                If Not IsDBNull(dt1.Rows(0).Item("EWB_No")) Then txt_EWBNo.Text = Trim(dt1.Rows(0).Item("EWB_No").ToString)


                If Trim(txt_eInvoiceNo.Text) <> "" Then

                    chk_Einvoice_No_Sts.Checked = True
                Else
                    chk_Einvoice_No_Sts.Checked = False
                End If

                If Trim(txt_EWBNo.Text) <> "" Then

                    chk_Ewb_No_Sts.Checked = True
                Else
                    chk_Ewb_No_Sts.Checked = False
                End If


                '----------------------------

                da2 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Create_Status = 1 Order by Sl_No, Piece_No", con)
                    'da2 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Create_Status = 1 Order by Sl_No, Piece_No", con)
                    dt2 = New DataTable
                    da2.Fill(dt2)

                    dgv_Details.Rows.Clear()

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_Details.Rows.Add()

                            dgv_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Piece_No").ToString
                            dgv_Details.Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")

                        Next i

                    End If
                    dt2.Clear()

                    With dgv_Details_Total
                        If .RowCount = 0 Then .Rows.Add()
                        .Rows(0).Cells(0).Value = Val(dt1.Rows(0).Item("Total_Return_Pcs").ToString)
                        .Rows(0).Cells(1).Value = Format(Val(dt1.Rows(0).Item("Total_Return_Meters").ToString), "########0.00")
                    End With

                    get_Ledger_TotalSales()
                End If

                dt1.Clear()

            Grid_Cell_DeSelect()

            If LockSTS = True Then

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray


                txt_NoOfPcs.Enabled = False
                txt_NoOfPcs.BackColor = Color.LightGray

                txt_PcsNoFrom.Enabled = False
                txt_PcsNoFrom.BackColor = Color.LightGray

                txt_Meters.Enabled = False
                txt_Meters.BackColor = Color.LightGray

                dgv_Details.AllowUserToAddRows = False

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then  dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub ClothSales_Return_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub ClothSales_Return_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt2)
        cbo_Cloth.DataSource = dt2
        cbo_Cloth.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Transport.DataSource = dt3
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 28 ) order by Ledger_DisplayName", con)
        da.Fill(dt5)
        cbo_SalesAc.DataSource = dt5
        cbo_SalesAc.DisplayMember = "Ledger_DisplayName"


        cbo_Type.Items.Clear()
        cbo_Type.Items.Add("")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("INVOICE")

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Common_Procedures.settings.CustomerCode = "1186" Then
            lbl_caption_vehicleno.Visible = True
            txt_vehicle_no.Visible = True
        End If
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvNo.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsNoFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ShortMtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Tcs_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TcsPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TCS_TaxableValue.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Packing.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Insurance.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cash_Disc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Trade_Disc.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Invoice_Date.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Tcs_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TcsPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TCS_TaxableValue.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvNo.LostFocus, AddressOf ControlLostFocus
        AddHandler Txt_folding.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsNoFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Insurance.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cash_Disc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Trade_Disc.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Invoice_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ShortMtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_InvNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfPcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Insurance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cash_Disc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Trade_Disc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Invoice_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ShortMtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Tcs_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TcsPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TCS_TaxableValue.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Txt_folding.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler Txt_folding.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfPcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Insurance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cash_Disc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Trade_Disc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Invoice_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ShortMtrs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TcsPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TCS_TaxableValue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tcs_Name.KeyPress, AddressOf TextBoxControlKeyPress

        dtp_Date.Text = ""
        txt_InvNo.Text = ""
        cbo_PartyName.Text = ""
        cbo_PartyName.Tag = ""

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub ClothSales_Return_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub ClothSales_Return_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(1).Value) = 0 Then
                                If txt_Meters.Enabled And txt_Meters.Visible Then
                                    txt_Meters.Focus()
                                Else
                                    txt_Note.Focus()
                                End If


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                txt_PcsNoFrom.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

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

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Sales_Return_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Sales_Return_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ClothSales_Sales_Return_Entry, New_Entry, Me, con, "ClothSales_Return_Head", "ClothSales_Return_Code", NewCode, "ClothSales_Return_Date", "(ClothSales_Return_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from ClothSales_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  Weaver_Piece_Checking_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Piece checking prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "ClothSales_Return_head", "ClothSales_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "ClothSales_Return_Code, Company_IdNo, for_OrderBy", trans)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Update ClothSales_Invoice_Details set Return_Meters = a.Return_Meters - b.Return_Meters from ClothSales_Invoice_Details a, ClothSales_Return_Head b Where b.ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and a.ClothSales_Invoice_Code = b.ClothSales_Invoice_Code and a.ClothSales_Invoice_SlNo = b.ClothSales_Invoice_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where Ledger_IdNo = 0 or ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select cloth_name from cloth_head order by cloth_name", con)
            da.Fill(dt2)
            cbo_Filter_Cloth.DataSource = dt2
            cbo_Filter_Cloth.DisplayMember = "cloth_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Cloth.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1

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
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code LIKE '" & Trim(Pk_Condition) & "%' AND ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothSales_Return_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Return_No from ClothSales_Return_Head where for_orderby > " & Str(Val(OrdByNo)) & " and ClothSales_Return_Code LIKE '" & Trim(Pk_Condition) & "%' AND company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothSales_Return_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Return_No from ClothSales_Return_Head where for_orderby < " & Str(Val(OrdByNo)) & " and ClothSales_Return_Code LIKE '" & Trim(Pk_Condition) & "%' AND company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothSales_Return_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " AND ClothSales_Return_Code LIKE '" & Trim(Pk_Condition) & "%' and ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothSales_Return_No desc", con)
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
            If Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then
                lbl_RefNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "CRNT")
            Else
                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Return_Head", "ClothSales_Return_Code", "For_OrderBy", "ClothSales_Return_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            End If
            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothSales_Return_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("ClothSales_Return_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("ClothSales_Return_Date").ToString
                End If
                If IsDBNull(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString) = False Then cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))
                If IsDBNull(dt1.Rows(0).Item("ClothSales_Return_Type").ToString) = False Then cbo_Type.Text = dt1.Rows(0).Item("ClothSales_Return_Type").ToString

                If IsDBNull(dt1.Rows(0).Item("Tcs_Name_caption").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Tcs_Name_caption").ToString) <> "" Then txt_Tcs_Name.Text = dt1.Rows(0).Item("Tcs_Name_caption").ToString
                End If


                If IsDBNull(dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False
                End If

                If IsDBNull(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If


                If Trim(txt_eInvoiceNo.Text) <> "" Then
                    chk_Einvoice_No_Sts.Checked = True
                Else
                    chk_Einvoice_No_Sts.Checked = False
                End If
                If Trim(txt_EWBNo.Text) <> "" Then
                    chk_Ewb_No_Sts.Checked = True
                Else
                    chk_Ewb_No_Sts.Checked = False
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
        Dim vRecCode As String = ""
        Dim vmovno As String = ""
        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(RecCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then

                vRecCode = "GSCRN-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(vRecCode) & "'", con)
                Dt = New DataTable
                Da.Fill(Dt)
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vmovno = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

            End If

            Dt.Dispose()
            Da.Dispose()



            If Val(movno) <> 0 Then
                move_record(movno)
            ElseIf Val(vmovno) <> 0 Then
                MessageBox.Show("This Invoice No. is in credit Note", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim vRecCode As String = ""
        Dim vmovno As String = ""
        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Sales_Return_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Sales_Return_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ClothSales_Sales_Return_Entry, New_Entry) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and  ClothSales_Return_Code LIKE '" & Trim(Pk_Condition) & "%' AND ClothSales_Return_Code = '" & Trim(RecCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()




            If Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then

                vRecCode = "GSCRN-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code='" & Trim(vRecCode) & "' ", con)
                Dt = New DataTable
                Da.Fill(Dt)
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vmovno = Trim(Dt.Rows(0)(0).ToString)

                    End If

                    Dt.Clear()
                End If



            End If

            Dt.Dispose()
            Da.Dispose()



            If Val(movno) <> 0 Then
                move_record(movno)
            ElseIf Val(vmovno) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Credit Note", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim SalAc_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim TtRetMtrs As Single = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotRetPcs As Single, vTotRetMtrs As Single
        Dim WftCnt_ID As Integer = 0
        Dim EntID As String = 0
        Dim Dup_PcNo As String = ""
        Dim PcsChkCode As String = ""
        Dim Nr As Integer = 0
        Dim LtNo As String = ""
        Dim LtCd As String = ""
        Dim Usr_ID As Integer = 0
        Dim vDelvTo_IdNo As Integer = 0
        Dim vTCS_AssVal_EditSTS As Integer = 0
        Dim vTCS_Tax_Sts As Integer = 0
        Dim vTCSAmtRndOff_STS As Integer = 0

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Sales_Return_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ClothSales_Sales_Return_Entry, New_Entry, Me, con, "ClothSales_Return_Head", "ClothSales_Return_Code", NewCode, "ClothSales_Return_Date", "(ClothSales_Return_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, ClothSales_Return_No desc", dtp_Date.Value.Date) = False Then Exit Sub


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



        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        SalAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        If SalAc_ID = 0 And Val(txt_Rate.Text) <> 0 Then
            MessageBox.Show("Invalid Sales A/c Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()
            Exit Sub
        End If
        vDelvTo_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(1).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(0).Value) = "" Then
                        MessageBox.Show("Invalid Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .CurrentCell = .Rows(i).Cells(0)
                            .Focus()
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_PcNo)), "~" & Trim(UCase(.Rows(i).Cells(0).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_PcNo = Trim(Dup_PcNo) & "~" & Trim(UCase(.Rows(i).Cells(0).Value)) & "~"

                End If

            Next

        End With

        NoCalc_Status = False
        Calculation_Details_Total()

        vTotRetPcs = 0 : vTotRetMtrs = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotRetPcs = Val(dgv_Details_Total.Rows(0).Cells(0).Value())
            vTotRetMtrs = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
        End If



        If Val(vTotRetMtrs) <> 0 Then
            If Val(vTotRetMtrs) <> Val(txt_Meters.Text) Then
                MessageBox.Show("Mismatch of Return Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Meters.Enabled And txt_Meters.Visible Then txt_Meters.Focus()
                Exit Sub
            End If
        End If


        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)
        cmd.Parameters.AddWithValue("@InvoiceDate", dtp_Invoice_Date.Value.Date)

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

        If Val(txt_NoOfPcs.Text) = 0 Then
            txt_NoOfPcs.Text = Val(vTotRetPcs)
        End If

        If Val(vTotRetMtrs) = 0 Then
            NoCalc_Status = True
            Calculation_TO_PieceNo()
            NoCalc_Status = False
        End If
        vTCS_AssVal_EditSTS = 0
        If txt_TCS_TaxableValue.Enabled = True Then vTCS_AssVal_EditSTS = 1


        vTCS_Tax_Sts = 0
        If chk_TCS_Tax.Checked = True Then vTCS_Tax_Sts = 1

        vTCSAmtRndOff_STS = 0
        If chk_TCSAmount_RoundOff_STS.Checked = True Then vTCSAmtRndOff_STS = 1


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                If Common_Procedures.settings.Cloth_sales_yarn_purchase_Return_ContinousNo_Status = 1 Then

                    lbl_RefNo.Text = Common_Procedures.get_ClothsalesRT_YarnPurcRT_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, "CRNT", tr)
                Else
                    lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Return_Head", "ClothSales_Return_Code", "For_OrderBy", "ClothSales_Return_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr



            Da = New SqlClient.SqlDataAdapter("select * from ClothSales_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            PcsChkCode = ""
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                End If
            End If
            Dt1.Clear()

            If New_Entry = True Then
                cmd.CommandText = "Insert into ClothSales_Return_Head ( ClothSales_Return_Code ,             Company_IdNo         ,        ClothSales_Return_No   ,                               for_OrderBy                              ,  ClothSales_Return_Date,                        Ledger_IdNo     ,             Invoice_No        ,Invoice_Date,      Cloth_IdNo    ,   PurchaseAc_IdNo  ,             Amount          ,              noof_pcs         ,             pcs_fromno         ,             pcs_tono         ,           ReturnMeters_Return    ,            Return_Meters         ,    Transport_IdNo    ,             Freight          ,                           Note           ,        Total_Return_Pcs     ,       Total_Return_Meters    ,                       ClothSales_Invoice_Code          ,             ClothSales_Invoice_SlNo         , Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment ,  user_idno                       ,    DeliveryTo_IdNo      ,   TradeDisc_Name                 ,         Trade_Discount                ,         CashDisc_Name                ,           Cash_Discount                         ,            Trade_Discount_Perc             ,          Cash_Discount_Perc             ,            Total_Amount    ,           Packing_Name              ,           Packing_Amount           ,        Insurance_Name              ,           Insurance_Amount    ,                 CGST_Amount                        ,SGST_Amount            ,           IGST_Amount                 ,               Total_Taxable_Amount      ,         Item_HSN_Code       ,                 Net_Amount          ,  Item_GST_Perc                             ,              Short_Meters          ,      ClothSales_Return_Type,              Tcs_Name_caption           ,              Tcs_percentage       ,                Tcs_Amount     , TCS_Taxable_Value, EDIT_TCS_TaxableValue ,  Tcs_Tax_Status, TCSAmount_RoundOff_Status, Invoice_Value_Before_TCS , RoundOff_Invoice_Value_Before_TCS ,vehicle_no ,Folding_percentage) " &
                                  "                   Values          ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @EntryDate       , " & Str(Val(Led_ID)) & ", '" & Trim(txt_InvNo.Text) & "',@InvoiceDate, " & Val(Clo_ID) & ", " & Val(SalAc_ID) & ", " & Val(txt_Rate.Text) & ",  " & Val(txt_NoOfPcs.Text) & ", " & Val(txt_PcsNoFrom.Text) & ", " & Val(lbl_PcsNoTo.Text) & ", " & Str(Val(txt_Meters.Text)) & ", " & Str(Val(txt_Meters.Text)) & ", " & Val(Trans_ID) & ", " & Val(txt_Freight.Text) & ", '" & Trim(txt_Note.Text) & "', " & Str(Val(vTotRetPcs)) & ", " & Str(Val(vTotRetMtrs)) & ",   '" & Trim(lbl_ClothSales_Return_Code.Text) & "', " & Val(lbl_ClothSales_Return_SlNo.Text) & ",               ''          ,             0                ," & Val(lbl_UserName.Text) & "  ," & Str(Val(vDelvTo_IdNo)) & " ,  '" & Trim(txt_TradeDic_Name.Text) & "', " & Str(Val(txt_Trade_Disc.Text)) & " , '" & Trim(txt_CashDic_Name.Text) & "',  " & Str(Val(txt_Cash_Disc.Text)) & ", " & Str(Val(lbl_Trade_Disc_Perc.Text)) & " , " & Str(Val(lbl_Cash_Disc_Perc.Text)) & ", " & Str(Val(lbl_Amount.Text)) & ", '" & Trim(txt_Packing_Name.Text) & "', " & Str(Val(txt_Packing.Text)) & ",'" & Trim(txt_Insurance_Name.Text) & "', " & Str(Val(txt_Insurance.Text)) & "," & Str(Val(lbl_CGST_Amount.Text)) & "," & Str(Val(lbl_SGST_Amount.Text)) & "," & Str(Val(lbl_IGST_Amount.Text)) & "," & Str(Val(lbl_Net_Amt.Text)) & ",'" & Trim(lbl_Grid_HSNCode.Text) & "'," & Str(Val(CSng(lbl_Net_Amt.Text))) & "," & Str(Val(lbl_Grid_GstPerc.Text)) & " ," & Str(Val(txt_ShortMtrs.Text)) & ", '" & Trim(cbo_Type.Text) & "', '" & Trim(txt_Tcs_Name.Text) & "', " & Str(Val(txt_TcsPerc.Text)) & ", " & Str(Val(lbl_TcsAmount.Text)) & " , " & Str(Val(txt_TCS_TaxableValue.Text)) & ", " & Str(Val(vTCS_AssVal_EditSTS)) & ", " & Str(Val(vTCS_Tax_Sts)) & ", " & Str(Val(vTCSAmtRndOff_STS)) & ", " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & ",'" & Trim(txt_vehicle_no.Text) & "'," & Str(Val(Txt_folding.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "ClothSales_Return_head", "ClothSales_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "ClothSales_Return_Code, Company_IdNo, for_OrderBy", tr)

                '  Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "ClothSales_Delivery_Return_Details", "ClothSales_Delivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "   Cloth_IdNo,ClothType_IdNo,Fold_Perc,Bales,Order_Pcs,Order_Meters,Rate,Order_Cancel_Meters,ClothSales_Enquiry_No,ClothSales_Enquiry_Code,ClothSales_Enquiry_Slno ,Selection_Type", "Sl_No", "ClothSales_Delivery_Return_Code, For_OrderBy, Company_IdNo, ClothSales_Delivery_Return_No, ClothSales_Delivery_Return_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update ClothSales_Return_Head Set ClothSales_Return_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Cloth_IdNo = " & Val(Clo_ID) & ", PurchaseAc_IdNo = " & Val(SalAc_ID) & ", Amount = " & Val(txt_Rate.Text) & ", Invoice_No = '" & Trim(txt_InvNo.Text) & "',  noof_pcs = " & Val(txt_NoOfPcs.Text) & ",Invoice_Date= @InvoiceDate, pcs_fromno = " & Val(txt_PcsNoFrom.Text) & " , pcs_tono = " & Val(lbl_PcsNoTo.Text) & ", ReturnMeters_Return = " & Val(txt_Meters.Text) & ",Transport_IdNo = " & Val(Trans_ID) & " , Freight = " & Val(txt_Freight.Text) & " , Note = '" & Trim(txt_Note.Text) & "', Total_Return_Pcs = " & Str(Val(vTotRetPcs)) & ", Total_Return_Meters = " & Str(Val(vTotRetMtrs)) & ", ClothSales_Invoice_Code = '" & Trim(lbl_ClothSales_Return_Code.Text) & "', ClothSales_Invoice_SlNo = " & Val(lbl_ClothSales_Return_SlNo.Text) & " , User_IdNo = " & Val(lbl_UserName.Text) & " ,DeliveryTo_IdNo=" & Str(Val(vDelvTo_IdNo)) & " , TradeDisc_Name = '" & Trim(txt_TradeDic_Name.Text) & "', Trade_Discount =  " & Str(Val(txt_Trade_Disc.Text)) & " , CashDisc_Name ='" & Trim(txt_CashDic_Name.Text) & "'  , Cash_Discount = " & Str(Val(txt_Cash_Disc.Text)) & " , Trade_Discount_Perc = " & Str(Val(lbl_Trade_Disc_Perc.Text)) & " ,Total_Amount=" & Str(Val(lbl_Amount.Text)) & "  , Cash_Discount_Perc = " & Str(Val(lbl_Cash_Disc_Perc.Text)) & " , Packing_Name ='" & Trim(txt_Packing_Name.Text) & "', Packing_Amount = " & Str(Val(txt_Packing.Text)) & " , Insurance_Name = '" & Trim(txt_Insurance_Name.Text) & "' , Insurance_Amount =  " & Str(Val(txt_Insurance.Text)) & ",Item_GST_Perc=" & Str(Val(lbl_Grid_GstPerc.Text)) & ",CGST_Amount =" & Str(Val(lbl_CGST_Amount.Text)) & ",SGST_Amount =" & Str(Val(lbl_SGST_Amount.Text)) & ",IGST_Amount =" & Str(Val(lbl_IGST_Amount.Text)) & ",Total_Taxable_Amount =" & Str(Val(lbl_AssessableValue.Text)) & ",Net_Amount=" & Str(Val(CSng(lbl_Net_Amt.Text))) & " ,Short_Meters=" & Str(Val(txt_ShortMtrs.Text)) & " , ClothSales_Return_Type = '" & Trim(cbo_Type.Text) & "',Tcs_Name_caption = '" & Trim(txt_Tcs_Name.Text) & "', Tcs_percentage=" & Str(Val(txt_TcsPerc.Text)) & ",Tcs_Amount= " & Str(Val(lbl_TcsAmount.Text)) & " , TCS_Taxable_Value = " & Str(Val(txt_TCS_TaxableValue.Text)) & ", EDIT_TCS_TaxableValue = " & Str(Val(vTCS_AssVal_EditSTS)) & " , Tcs_Tax_Status = " & Str(Val(vTCS_Tax_Sts)) & " , TCSAmount_RoundOff_Status = " & Str(Val(vTCSAmtRndOff_STS)) & " , Invoice_Value_Before_TCS = " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", RoundOff_Invoice_Value_Before_TCS = " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & ",Vehicle_No='" & Trim(txt_vehicle_no.Text) & "' ,Folding_percentage=" & Str(Val(Txt_folding.Text)) & ", E_Invoice_IRNO = '" & Trim(txt_eInvoiceNo.Text) & "' , E_Invoice_QR_Image =  @QrCode  , E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & "  ,  E_Invoice_Cancelled_Status = " & eiCancel.ToString & " ,  E_Invoice_Cancellation_Reason = '" & txt_EInvoiceCancellationReson.Text & "'  ,    EWB_No = '" & txt_EWBNo.Text & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_Return_Head set  Return_Meters = " & Str(Val(txt_Meters.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_Invoice_Details set Return_Meters = a.Return_Meters - b.Return_Meters from ClothSales_Invoice_Details a, ClothSales_Return_Head b Where b.ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and a.ClothSales_Invoice_Code = b.ClothSales_Invoice_Code and a.ClothSales_Invoice_SlNo = b.ClothSales_Invoice_SlNo"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "ClothSales_Return_head", "ClothSales_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "ClothSales_Return_Code, Company_IdNo, for_OrderBy", tr)

            If Trim(lbl_ClothSales_Return_Code.Text) <> "" Then
                Nr = 0
                cmd.CommandText = "Update ClothSales_Invoice_Details set Return_Meters = Return_Meters + " & Str(Val(txt_Meters.Text)) & " Where ClothSales_Invoice_Code = '" & Trim(lbl_ClothSales_Return_Code.Text) & "' and ClothSales_Invoice_SlNo = " & Str(Val(lbl_ClothSales_Return_SlNo.Text))
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    Throw New ApplicationException("Mismatch of PartyName & ClothSales Invoice Details")
                    Exit Sub
                End If
            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "SalesReturn : RefNo. " & Trim(lbl_RefNo.Text)
            If Trim(txt_InvNo.Text) <> "" Then
                PBlNo = Trim(txt_InvNo.Text)
            Else
                PBlNo = Trim(lbl_RefNo.Text)
            End If


            LtNo = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.LotCode.Sales_Return_GST)
            LtCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.LotCode.Sales_Return_GST) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Create_Status = 1 and Weaver_Piece_Checking_Code = ''"
            'cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Create_Status = 1 and Weaver_Piece_Checking_Code = ''"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Then

                        Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_ClothReceipt_Date = @EntryDate, Lot_Code = '" & Trim(LtCd) & "' , Lot_No = '" & Trim(LtNo) & "' , Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", ReceiptMeters_Receipt = " & Val(.Rows(i).Cells(1).Value) & ", Create_Status = 1 where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                        'cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Weaver_ClothReceipt_Date = @EntryDate, Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", ReceiptMeters_Receipt = " & Val(.Rows(i).Cells(1).Value) & ", Create_Status = 1 where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code ,            Company_IdNo          ,      Weaver_ClothReceipt_No   ,                               for_OrderBy                              , Weaver_ClothReceipt_Date,        Lot_Code     ,          Lot_No     ,           Cloth_IdNo    ,             Sl_No     ,                    Piece_No            ,                               PieceNo_OrderBy                                   ,       ReceiptMeters_Receipt         ,                  Receipt_Meters     , Create_Status,                                            StockOff_IdNo   ,                                            WareHouse_IdNo  ) " &
                                                                "  Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",          @EntryDate     , '" & Trim(LtCd) & "', '" & Trim(LtNo) & "', " & Str(Val(Clo_ID)) & ",  " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(0).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", " & Val(.Rows(i).Cells(1).Value) & ", " & Val(.Rows(i).Cells(1).Value) & ",       1      , " & Str(Val(Common_Procedures.CommonLedger.OwnSort_Ac)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " ) "
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next

            End With

            If Trim(PcsChkCode) = "" Then

                cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Val(txt_Meters.Text) <> 0 Then
                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,                               StockOff_IdNo               ,                               DeliveryTo_Idno             ,       ReceivedFrom_Idno ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     ,               UnChecked_Meters    ,  Meters_Type1, Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ) " &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @EntryDate , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ",  " & Str(Val(txt_Meters.Text)) & ",       0      ,       0     ,       0     ,       0     ,       0      ) "
                    cmd.ExecuteNonQuery()
                End If

            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            AcPos_ID = Led_ID

            vLed_IdNos = AcPos_ID & "|" & SalAc_ID & "|24|25|26|" & Common_Procedures.CommonLedger.TCS_RECEIVABLE_AC
            vVou_Amts = Val(CSng(lbl_Net_Amt.Text)) & "|" & -1 * (Val(CSng(lbl_Net_Amt.Text)) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text) - Val(lbl_IGST_Amount.Text) - Val(lbl_TcsAmount.Text)) & "|" & -1 * Val(lbl_CGST_Amount.Text) & "|" & -1 * Val(lbl_SGST_Amount.Text) & "|" & -1 * Val(lbl_IGST_Amount.Text) & "|" & -1 * Val(lbl_TcsAmount.Text)
            If Common_Procedures.Voucher_Updation(con, "Gst.CloSale.Ret", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(dtp_Date.Text), "Inv No : " & Trim(txt_InvNo.Text) & ", Mtrs : " & Trim(Format(Val(txt_Meters.Text), "#########0.00")), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(dtp_Date.Text), AcPos_ID, Trim(txt_InvNo.Text), 0, Val(CSng(lbl_Net_Amt.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)


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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, cbo_Type, txt_Rate, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) ", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And cbo_PartyName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_Type.Text) = "INVOICE" Then
                Txt_folding.Enabled = False
                If MessageBox.Show("Do you want to select Cloth Sales Invoice Details ", "FOR CLOTH SALES INVOICE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                Else
                    If txt_InvNo.Enabled Then txt_InvNo.Focus() Else cbo_SalesAc.Focus()
                End If
            Else
                Txt_folding.Enabled = True
                txt_InvNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_Type.Text) = "INVOICE" Then
                Txt_folding.Enabled = False
                If MessageBox.Show("Do you want to select Cloth Sales Invoice:", "FOR CLOTH SALES INVOICE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                Else
                    If txt_InvNo.Enabled Then txt_InvNo.Focus() Else cbo_SalesAc.Focus()
                End If
            Else
                Txt_folding.Enabled = True
                txt_InvNo.Focus()
            End If
            get_Ledger_TotalSales()
        End If
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
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

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_Transport, txt_NoOfPcs, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, txt_NoOfPcs, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0 )")
    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then


            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, msk_Invoice_Date, cbo_DeliveryTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, cbo_DeliveryTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Get_vehicle_from_Transport()
        End If

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_DeliveryTo, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Get_vehicle_from_Transport()
        End If

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Get_vehicle_from_Transport()
        End If

    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Transport_Creation
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

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Clo_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clo_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothSales_Return_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothSales_Return_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothSales_Return_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Clo_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Cloth_IdNo = " & Str(Val(Clo_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Cloth_Name  from ClothSales_Return_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  a.ClothSales_Return_Code NOT LIKE '" & Trim(Pk_Condition) & "%'   AND  a.ClothSales_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ClothSales_Return_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("ClothSales_Return_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothSales_Return_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Invoice_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("noof_pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Return_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_TYPE = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_TYPE = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_TYPE = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
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
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Dim TotMtrs As Single = 0

        Try
            Calculation_Details_Total()

            With dgv_Details_Total
                If .RowCount > 0 Then
                    TotMtrs = Val(.Rows(0).Cells(1).Value)
                End If
            End With
            txt_Meters.Text = Format(Val(TotMtrs), "#########0.00")

            dgv_Details_CellLeave(sender, e)

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL END EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Try
            With dgv_Details

                If e.RowIndex = 0 Then
                    .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)

                Else
                    If Val(.CurrentRow.Cells(0).Value) = 0 Then
                        .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
                    End If

                End If

            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .CurrentCell.ColumnIndex = 1 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If
            End With
        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS DCELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim TotMtrs As Single = 0

        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 1 Then
                        Calculation_Details_Total()

                        With dgv_Details_Total
                            If .RowCount > 0 Then
                                TotMtrs = Val(.Rows(0).Cells(1).Value)
                            End If
                        End With
                        txt_Meters.Text = Format(Val(TotMtrs), "#########0.00")

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL VALUE CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim PcsFrmNo As Integer = 0
        Dim NewCode As String = ""
        Dim PcsChkCode As String = ""

        Try

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code from ClothSales_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                PcsChkCode = ""
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                        PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                    End If
                End If
                Dt1.Clear()

                If Trim(PcsChkCode) <> "" Then
                    MessageBox.Show("Piece Checking prepared", "DOES NOT DELETE PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If

                With dgv_Details

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    PcsFrmNo = Val(txt_PcsNoFrom.Text)
                    If Val(PcsFrmNo) = 0 Then PcsFrmNo = 1

                    For i = 0 To .Rows.Count - 1
                        If i = 0 Then
                            .Rows(i).Cells(0).Value = Val(PcsFrmNo)
                        Else
                            .Rows(i).Cells(0).Value = Val(.Rows(i - 1).Cells(0).Value) + 1
                        End If
                    Next

                End With

                Calculation_Details_Total()

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

        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details

                If e.RowIndex = 0 Then
                    .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)

                Else
                    If Val(.CurrentRow.Cells(0).Value) = 0 Then
                        .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
                    End If

                End If

            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS ROWS ADD....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Calculation_Details_Total()
        Dim TotPcs As Double = 0
        Dim TotMtrs As Double = 0

        Try

            If NoCalc_Status = True Then Exit Sub

            TotPcs = 0
            TotMtrs = 0
            With dgv_Details

                For i = 0 To .RowCount - 1
                    If Val(.Rows(i).Cells(1).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                        TotMtrs = TotMtrs + Val(.Rows(i).Cells(1).Value)
                    End If
                Next

            End With

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(0).Value = Val(TotPcs)
                .Rows(0).Cells(1).Value = Format(Val(TotMtrs), "########0.00")
            End With

            If Val(TotMtrs) <> 0 Then txt_Meters.Text = Format(Val(TotMtrs), "#########0.00")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TOTAL CALCULATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_PcsNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsNoFrom.KeyDown
        If e.KeyCode = 40 Then
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True

        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_ReceiptMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meters.KeyDown
        Dim TotMtrs As Double = 0

        Try
            If e.KeyCode = 40 Then
                SendKeys.Send("{TAB}")

            ElseIf e.KeyCode = 38 Then
                SendKeys.Send("+{TAB}")

            ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then
                TotMtrs = 0
                With dgv_Details_Total
                    If .RowCount > 0 Then
                        TotMtrs = Val(.Rows(0).Cells(1).Value)
                    End If
                End With
                If Val(TotMtrs) <> 0 Then e.Handled = True : e.SuppressKeyPress = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE RECEIPT METERS KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub txt_ReceiptMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        Dim TotMtrs As Double = 0

        Try

            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

            With dgv_Details_Total
                TotMtrs = 0
                If .RowCount > 0 Then
                    TotMtrs = Val(.Rows(0).Cells(1).Value)
                End If
            End With
            If Val(TotMtrs) <> 0 Then e.Handled = True

            If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE RECEIPT METERS KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub txt_PcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsNoFrom.KeyPress
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

            If Asc(e.KeyChar) = 13 Then
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE PCSNO KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_NoofPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE NOOFPCS KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyCode = 40 Then dtp_Date.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_meters_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meters.LostFocus
        With txt_Meters
            .Text = Format(Val(.Text), "#########0.00")
        End With
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub Calculation_TO_PieceNo()

        lbl_PcsNoTo.Text = ""

        If Val(txt_NoOfPcs.Text) > 0 Then

            If dgv_Details.RowCount > 0 Then
                txt_PcsNoFrom.Text = Val(dgv_Details.Rows(0).Cells(0).Value)
            Else
                If Val(txt_PcsNoFrom.Text) = 0 Then txt_PcsNoFrom.Text = "1"
            End If
            If Val(txt_PcsNoFrom.Text) = 0 Then txt_PcsNoFrom.Text = "1"

            lbl_PcsNoTo.Text = Val(txt_PcsNoFrom.Text) + Val(txt_NoOfPcs.Text) - 1

        End If

    End Sub

    Private Sub txt_NoOfPcs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If NoCalc_Status = False Then
            Calculation_TO_PieceNo()
        End If
    End Sub

    Private Sub txt_PcsNoFrom_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_PcsNoFrom.TextChanged
        Dim i As Integer = 0
        Dim PcFrmNo As Integer = 0

        Try

            If NoCalc_Status = True Then Exit Sub

            Calculation_TO_PieceNo()

            With dgv_Details
                If .Rows.Count > 0 Then

                    PcFrmNo = Val(txt_PcsNoFrom.Text)
                    If PcFrmNo = 0 Then PcFrmNo = 1

                    .Rows(0).Cells(0).Value = Val(PcFrmNo)

                    For i = 1 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = Val(.Rows(i - 1).Cells(0).Value) + 1
                    Next

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE PCSNOFROM CHANGED....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE TXT_DETAILS ENTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then

                    If .CurrentCell.ColumnIndex = 1 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE TXT_DETAILS KEYPRESS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            dgv_Details_KeyUp(sender, e)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE TXT_DETAILS KEYUP...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            If e.Control = True And e.KeyValue = 13 Then
                If txt_Meters.Enabled And txt_Meters.Visible Then
                    txt_Meters.Focus()

                End If

            ElseIf e.KeyValue = 46 Then
                With dgv_Details
                    If .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells(1).Value = ""

                    End If

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYDOWN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0, Ent_Amt As Single = 0, Ent_Rate As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT CLOTH SALES DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 1 Then
            CompIDCondt = ""
        End If

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, d.net_amount, c.Cloth_Name, b.Noof_Pcs as Ent_Pcs, b.Return_Meters as Ent_Meters, b.Amount as Ent_Rate, b.Amount as Ent_Amount from ClothSales_Invoice_Details a INNER JOIN ClothSales_Invoice_Head d ON d.ClothSales_Invoice_Code = a.ClothSales_Invoice_Code LEFT OUTER JOIN ClothSales_Return_Head b ON b.ClothSales_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.ClothSales_Invoice_Code = b.ClothSales_Invoice_Code and a.ClothSales_Invoice_SlNo = b.ClothSales_Invoice_SlNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo   Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " d.ledger_Idno = " & Str(Val(LedIdNo)) & " and ( (a.Meters - a.Return_Meters) > 0 or (b.Return_Meters ) > 0 ) order by a.ClothSales_Invoice_Date, a.for_orderby, a.ClothSales_Invoice_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    Ent_Pcs = 0
                    Ent_Mtrs = 0
                    Ent_Rate = 0
                    Ent_Amt = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Meters").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_Meters").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Amount").ToString) = False Then
                        Ent_Amt = Val(Dt1.Rows(i).Item("Ent_Amount").ToString)
                    End If

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Invoice_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Invoice_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    '.Rows(n).Cells(4).Value = Dt1.Rows(i).Item("SalesAcName").ToString
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Pcs").ToString)
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "########0.00") '- Val(Dt1.Rows(i).Item("Return_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Net_Amount").ToString)
                    .Rows(n).Cells(8).Value = ""
                    Txt_folding.Text = Format(Val(Dt1.Rows(i).Item("Fold_Perc").ToString), "########0.00")
                    If (Ent_Mtrs) > 0 Then
                        .Rows(n).Cells(8).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next
                    End If

                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("ClothSales_Invoice_Code").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("ClothSales_Invoice_SlNo").ToString

                    .Rows(n).Cells(11).Value = Ent_Pcs
                    .Rows(n).Cells(12).Value = 0 'Ent_Mtrs

                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Rate").ToString
                    .Rows(n).Cells(14).Value = Ent_Rate

                Next

            End If
            Dt1.Clear()

        End With

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
        Select_Invoice(e.RowIndex)
    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown

        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                    Select_Invoice(dgv_Selection.CurrentCell.RowIndex)

                    e.Handled = True

                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE SELECTION KEYDOWN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Select_Invoice(ByVal RwIndx As Integer)
        Dim i As Integer = 0
        Dim j As Integer = 0

        Try

            With dgv_Selection

                If .RowCount > 0 And RwIndx >= 0 Then

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(8).Value = ""
                        For j = 0 To .Columns.Count - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Black
                        Next
                    Next

                    .Rows(RwIndx).Cells(8).Value = 1

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                    Close_ClothInvoice_Selection()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE SELECT INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Try
            Close_ClothInvoice_Selection()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE CLOSE SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Close_ClothInvoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        Try
            dgv_Details.Rows.Clear()

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                    txt_InvNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                    msk_Invoice_Date.Text = dgv_Selection.Rows(i).Cells(2).Value
                    dtp_Invoice_Date.Text = dgv_Selection.Rows(i).Cells(2).Value
                    cbo_Cloth.Text = dgv_Selection.Rows(i).Cells(3).Value

                    'cbo_SalesAc.Text = dgv_Selection.Rows(i).Cells(4).Value

                    If Val(dgv_Selection.Rows(i).Cells(11).Value) <> 0 Then
                        txt_NoOfPcs.Text = Val(dgv_Selection.Rows(i).Cells(11).Value)
                    Else
                        txt_NoOfPcs.Text = Val(dgv_Selection.Rows(i).Cells(5).Value)
                    End If

                    If Val(txt_NoOfPcs.Text) = 0 Then
                        txt_NoOfPcs.Text = ""
                    End If

                    If Val(dgv_Selection.Rows(i).Cells(12).Value) <> 0 Then
                        txt_Meters.Text = Format(Val(dgv_Selection.Rows(i).Cells(12).Value), "#########0.00")
                    Else
                        txt_Meters.Text = Format(Val(dgv_Selection.Rows(i).Cells(6).Value), "#########0.00")
                    End If
                    If Val(txt_Meters.Text) = 0 Then
                        txt_Meters.Text = ""
                    End If

                    lbl_ClothSales_Return_Code.Text = dgv_Selection.Rows(i).Cells(9).Value
                    lbl_ClothSales_Return_SlNo.Text = dgv_Selection.Rows(i).Cells(10).Value

                End If

            Next

            Calculation_Details_Total()

            pnl_Back.Enabled = True
            pnl_Selection.Visible = False
            If txt_InvNo.Enabled And txt_InvNo.Visible Then txt_InvNo.Focus() Else cbo_SalesAc.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE CLOSE INVOICE SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ClothSales_Sales_Return_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from ClothSales_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(NewCode) & "'", con)
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

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If

                Else
                    PrintDocument1.Print()

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


    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Amount_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Rate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Meters_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Meters.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_SalesAc, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
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


    Private Sub PrintDocument1_BeginPrint(sender As Object, e As PrintEventArgs) Handles PrintDocument1.BeginPrint
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, ch.*, d.Ledger_Name as TransportName,  Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, f.Ledger_mainName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code  from ClothSales_Return_Head a " &
                                               "INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo " &
                                               " INNER JOIN Cloth_Head ch ON a.Cloth_idno = ch.Cloth_idno " &
                                                " Left outer JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_IdNo " &
                                               "LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo " &
                                              " LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo " &
                                               " Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo  " &
                                              "LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo " &
                                               " LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Return_Code = '" & Trim(NewCode) & "' ", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.*,d.ClothType_name ,e.Count_Name from ClothSales_Invoice_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno LEFT OUTER JOIN Count_Head e ON b.Cloth_WarpCount_IdNo = e.Count_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & prn_HdDt.Rows(0).Item("ClothSales_Invoice_Code").ToString & "' Order by a.Sl_No", con)
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

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
       Printing_GST_Format1(e)
    End Sub
    Private Sub Printing_GST_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim VechDesc1 As String = "", VechDesc2 As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim vLine_Pen As Pen
        Dim nSrt_Mtrs As String = 0


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then '---- Rajendra Textiles (Somanur)
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then '---- Rajendra Textiles (Somanur)
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = 10
                .Right = 65
                .Top = 50 ' 60
                .Bottom = 40
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With

        Else
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = 20 ' 40
                .Right = 50
                .Top = 20 '40 '50 ' 60
                .Bottom = 40
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With

        End If

        pFont = New Font("Calibri", 9, FontStyle.Bold)
        'pFont = New Font("Calibri", 10, FontStyle.Regular)

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
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
        '    NoofItems_PerPage = 6 ' 4 
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
        '    NoofItems_PerPage = 15 ' 4 
        'Else
        '    NoofItems_PerPage = 7 ' 4 
        'End If
        NoofItems_PerPage = 9 '13

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 30 : ClAr(2) = 260 : ClAr(3) = 90 : ClAr(4) = 55 : ClAr(5) = 0 : ClAr(6) = 0 : ClAr(7) = 95 : ClAr(8) = 90
        ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        'ClAr(1) = 30 : ClAr(2) = 210 : ClAr(3) = 80 : ClAr(4) = 50 : ClAr(5) = 50 : ClAr(6) = 50 : ClAr(7) = 80 : ClAr(8) = 80
        'ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 16 '16.65 ' 17.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        vLine_Pen = New Pen(Color.Black, 2)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 5
                Else
                    If vNoofHsnCodes > 1 Then NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                Printing_GST_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen)

                NoofDets = 0
                CurY = CurY - 5

                If prn_HdDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_HdDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1


                            Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True, vLine_Pen)


                            e.HasMorePages = True
                            Return

                        End If

                        'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                        '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        'Else
                        '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        'End If

                        'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                        '    ItmNm1 = prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString
                        'Else
                        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)
                        'End If


                        ItmNm2 = ""
                        If Len(ItmNm1) > 40 Then
                            For I = 40 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 40
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        CurY = CurY + TxtHgt + 5
                        NoofDets = NoofDets + 1





                        Common_Procedures.Print_To_PrintDocument(e, "1", LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("ITEM_HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(0).Item("ITEM_GST_PERC").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("ITEM_GST_PERC").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Return_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Short_Meters").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Return_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Amount").ToString), "######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        If Val(prn_HdDt.Rows(0).Item("Folding_percentage").ToString) = 0 Or Val(prn_HdDt.Rows(0).Item("Folding_percentage").ToString) = 100 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Return_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Amount").ToString), "######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                        Else

                            flperc = 100 - Val(prn_HdDt.Rows(0).Item("Folding_percentage").ToString)

                            flmtr = Format((Val(prn_HdDt.Rows(0).Item("Return_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Short_Meters").ToString)) * flperc / 100, "#########0.00")

                            flmtr = Math.Abs(Val(flmtr))
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1152" Then
                                flmtr = Common_Procedures.Meter_RoundOff(flmtr)
                            End If

                            CurY = CurY + TxtHgt
                            NoofDets = NoofDets + 1
                            If Val(flperc) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Less", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Add", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(flmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                            CurY = CurY + TxtHgt
                            NoofDets = NoofDets + 1
                            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY)

                            If Val(flperc) > 0 Then
                                fmtr = Format((Val(prn_HdDt.Rows(0).Item("Return_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Short_Meters").ToString)) - Val(flmtr), "#########0.00")
                            Else
                                fmtr = Format((Val(prn_HdDt.Rows(0).Item("Return_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Short_Meters").ToString)) + Val(flmtr), "#########0.00")
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(fmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(fmtr) * Val(prn_HdDt.Rows(0).Item("Amount").ToString), "######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                        End If
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True, vLine_Pen)

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

    Private Sub Printing_GST_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByRef NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
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
        Dim Rate_PCMETER As String = ""
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim I As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothSales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

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

       p1Font = New Font("Calibri", 14, FontStyle.Bold)

        'Common_Procedures.Print_To_PrintDocument(e, "SALES RETURN / DEBIT NOTE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

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


        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            If Val(lbl_Company.Tag) = 1 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            If InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHY") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHI") > 0 Then                                    '---- Ganapathy Spinning textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GSM_LOGO, Drawing.Image), LMargin + 20, CurY, 112, 80)
            ElseIf InStr(1, Trim(UCase(Cmp_Name)), "LOGU") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "LOGA") > 0 Then                                          '---- Logu textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_LogaTex, Drawing.Image), LMargin + 20, CurY, 112, 80)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY - 10, 120, 90)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then '---- Bannari amman textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.REVISED_LOGO_7___2_, Drawing.Image), LMargin + 20, CurY - 10, 130, 110)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- m.s textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.ms_logo_2, Drawing.Image), LMargin + 20, CurY - 10, 130, 110)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then                   '---- Arul Kumaran Textiles (Somanur)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_ArulKumaran, Drawing.Image), LMargin + 20, CurY - 5, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 10, CurY - 5, 120, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1045" Then '---- Kesavalogu textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KeasavLogu, Drawing.Image), LMargin + 10, CurY - 5, 120, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then '---- KRG TEXTILES
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
                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 20, CurY + 5, 110, 100)

                            End If

                        End Using

                    End If

                End If

            End If
        End If


        '---------------

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 120, CurY + 10, 115, 115)

                        End If

                    End Using

                End If

            End If

        End If

        '-----------------

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Red)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 7
        If Desc <> "" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)
            End If
        End If

        strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
        If PrintWidth > strWidth Then
            If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt - 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont)
                End If
            End If

            NoofItems_PerPage = NoofItems_PerPage - 1

        Else

            If Cmp_Add1 <> "" Then
                CurY = CurY + TxtHgt - 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
                End If

            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
                End If
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)

            If Cmp_StateNm <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm & "  " & Cmp_StateCode, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_EMail <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_GSTIN_No <> "" Then
                CurY = CurY + TxtHgt - 1
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, p1Font)
            End If
            If Cmp_PhNo <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
            End If


        Else

            CurY = CurY + TxtHgt
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)
            End If

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
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            End If

            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)
            End If


            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            End If

            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)
            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
                strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                CurX = CurX + strWidth
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Green)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
                End If
                strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
                CurX = CurX + strWidth
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont, Brushes.Green)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)
                End If

            End If
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "SALES RETURN / CREDIT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
            CurY = CurY + TxtHgt - 5
        End If
        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            CurY = CurY + TxtHgt + 2

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString), LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, LMargin + ClAr(1) + ClAr(2), CurY, 0, PageWidth, pFont)

        End If
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30
            W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
            S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

            W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
            S2 = e.Graphics.MeasureString("TRANSPORT NAME     ", pFont).Width
            'S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

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
            If Len(ItmNm1) > 40 Then
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
            If Len(ItmNm1) > 40 Then
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
            Common_Procedures.Print_To_PrintDocument(e, "CREDIT NOTE NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothSales_Return_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CREDIT NOTE DATE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Return_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("EWB_NO").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "EWAY BILL NO", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EWB_NO").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            End If
            If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
            LnAr(3) = CurY

            W2 = e.Graphics.MeasureString("DOCUMENT THROUGH   : ", pFont).Width
            S2 = e.Graphics.MeasureString("DATE & TIME OF SUPPLY  :", pFont).Width

            LnAr(4) = CurY


            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY + (TxtHgt \ 2), 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY + (TxtHgt \ 2), 2, ClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "HSN", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "GST", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METRE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METRES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + (TxtHgt \ 2), 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vLine_Pen As Pen)
        Dim p1Font As Font, p2Font As Font, p3Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim SubClAr(15) As Single
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String, ItmNm4 As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 7
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))



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

            ItmNm1 = ""
            If Trim(prn_HdDt.Rows(0).Item("note").ToString) <> "" Then
                ItmNm1 = "NOTE : " & Trim(prn_HdDt.Rows(0).Item("note").ToString)
            End If

            ItmNm2 = ""
            If Len(ItmNm1) > 60 Then
                For I = 60 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 60
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If

            ItmNm3 = ""
            If Len(ItmNm2) > 60 Then
                For I = 60 To 1 Step -1
                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 60
                ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
            End If

            ItmNm4 = ""
            If Len(ItmNm3) > 60 Then
                For I = 60 To 1 Step -1
                    If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 60
                ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
            End If

            pFont = New Font("Calibri", 9, FontStyle.Bold)
            CurY1 = CurY1 + TxtHgt - 3
            If prn_HdDt.Rows(0).Item("note").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + 10, CurY1, 0, 0, pFont)
            End If
            If ItmNm2 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + 35, CurY1, 0, 0, pFont)
            End If
            If ItmNm3 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm3, LMargin + 35, CurY1, 0, 0, pFont)
            End If
            If ItmNm4 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm4, LMargin + 35, CurY1, 0, 0, pFont)
            End If


            CurY1 = CurY1 + 10

            p3Font = New Font("Calibri", 10, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)

                CurY1 = CurY1 + TxtHgt
                CurY1 = CurY1 + TxtHgt
                CurY1 = CurY1 + TxtHgt
                CurY1 = CurY1 + TxtHgt
                ' CurY1 = CurY1 + TxtHgt
                e.Graphics.DrawLine(vLine_Pen, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1)

                CurY1 = CurY1 + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, BankNm1 & " , " & BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
                CurY1 = CurY1 + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, BankNm3 & " , " & BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
                CurY1 = CurY1 + TxtHgt + 10
                e.Graphics.DrawLine(vLine_Pen, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1)

            Else
                If BankNm1 <> "" Then
                    CurY1 = CurY1 + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
                End If
                If BankNm2 <> "" Then
                    CurY1 = CurY1 + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
                End If
                If BankNm3 <> "" Then
                    CurY1 = CurY1 + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
                End If
                If BankNm4 <> "" Then
                    CurY1 = CurY1 + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
                End If
            End If

            'CurY1 = CurY1 + TxtHgt + 10


            CurY = CurY - 5
            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString) & " @ " & Trim(Val(prn_HdDt.Rows(0).Item("Trade_Discount").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString) & " @ " & Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Insurance_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Insurance_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Insurance_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Insurance_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                CurY = CurY - 15
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "TAXABLE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If


            '----Gst
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CGST @ ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Item_GST_Perc").ToString) / 2 & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SGST @ ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Item_GST_Perc").ToString) / 2 & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "IGST @ ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Item_GST_Perc").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                ' CurX = LMargin + 410
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("tcs_name_Caption").ToString) & "   @", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                '  CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TCS_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                ' CurX = LMargin + 750
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            End If
            TtAmt = Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Tcs_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), "#########0.00")

            rndoff = 0
            rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

            CurY = CurY + TxtHgt
            If Val(rndoff) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                If Val(rndoff) >= 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If


            If CurY1 > CurY Then CurY = CurY1
            If CurY < 731 Then CurY = 731

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            'LnAr(8) = CurY


            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            CurY = CurY + 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                BmsInWrds = Trim(UCase(BmsInWrds))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            '=============GST SUMMARY============
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" Then '---- KalaiMahal Textiles 
            '    vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            '    If vNoofHsnCodes <> 0 Then
            '        Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), vLine_Pen)
            '    End If

            'End If

            '==========================

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt

            p2Font = New Font("Webdings", 8, FontStyle.Bold)
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit.", LMargin + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods.", LMargin + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction. ", LMargin + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY



            CurY = CurY + 5
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 7, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct and the amount indicated represents the price actually charged and that there is no flow additional consideration", PageWidth - 10, CurY, 1, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "directly or indirectly from the buyer", LMargin + 20, CurY + 10, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, Brushes.Red)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1116" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1380" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1446" Then
                If Val(Common_Procedures.User.IdNo) <> 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            '   CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(vLine_Pen, PageWidth, LnAr(1), PageWidth, CurY)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to asiatextilestirupur@yahoo.in", LMargin + 10, CurY, 0, 0, p1Font)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Trade_Disc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Trade_Disc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Trade_Disc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Trade_Disc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Cash_Disc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Cash_Disc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Insurance_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Insurance.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Packing_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Packing.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Cash_Disc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cash_Disc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
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

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

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

    Private Sub msk_Invoice_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Invoice_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Invoice_Date.Text
            vmskSelStrt = msk_Invoice_Date.SelectionStart
        End If
    End Sub

    Private Sub msk_Invoice_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Invoice_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Invoice_Date.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_Invoice_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Invoice_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Invoice_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Invoice_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If
    End Sub

    Private Sub dtp_Invoice_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Invoice_Date.TextChanged
        If IsDate(dtp_Invoice_Date.Text) = True Then
            msk_Invoice_Date.Text = dtp_Invoice_Date.Text
            msk_Invoice_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Invoice_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Invoice_Date.LostFocus

        If IsDate(msk_Invoice_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Invoice_Date.Value = Convert.ToDateTime(msk_Invoice_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_Date, cbo_PartyName, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_PartyName, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            cbo_PartyName.Focus()
        End If
    End Sub

    Private Sub txt_NoOfPcs_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.TextChanged

    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
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


            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            '-----------TOTAL SALES

            cmd.Connection = Con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@entrydate", dtp_Date.Value.Date)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_PartyName.Text)

            If Led_ID <> 0 Then

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%' OR a.Voucher_Code LIKE 'GYPSL-%') "
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

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(vPrevYrCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%' OR a.Voucher_Code LIKE 'GYPSL-%') "
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

    Private Sub txt_TCS_TaxableValue_TextChanged(sender As System.Object, e As System.EventArgs)
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

            'Else
            '    txt_addless.Focus()

        End If
    End Sub

    Private Sub chk_TCS_Tax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCS_Tax.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub chk_TCSAmount_RoundOff_STS_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCSAmount_RoundOff_STS.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TcsPerc_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TcsPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TcsPerc_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TcsPerc.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub txt_folding_perc_TextChanged(sender As Object, e As EventArgs) Handles Txt_folding.TextChanged
        NetAmount_Calculation()

    End Sub

    Private Sub Txt_folding_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Txt_folding.KeyPress
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True



        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE PCSNO KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
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



    End Sub

    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from ClothSales_Return_Head Where ClothSales_Return_Code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from ClothSales_Return_Head Where ClothSales_Return_Code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) > 0"
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




            Cmd.CommandText = "Insert into e_Invoice_Head (e_Invoice_No ,        e_Invoice_date ,           Buyer_IdNo ,    Consignee_IdNo ,     Assessable_Value             ,   CGST ,           SGST  ,     IGST   ,  Cess   ,   State_Cess ,                       Round_Off           ,  Nett_Invoice_Value  ,   Ref_Sales_Code      ,                   Other_Charges               ,          Dispatcher_IdNo ) " &
                                            "Select  ClothSales_Return_No ,    ClothSales_Return_Date,       Ledger_IdNo,    Ledger_IdNo,         Total_Taxable_Amount, CGST_Amount, SGST_Amount, IGST_Amount ,   0,          0,           RoundOff_Invoice_Value_Before_TCS    ,        Net_Amount,     '" & Trim(NewCode) & "',          ISNULL(TCS_Amount,0) as OtherCharges,         0     from ClothSales_Return_Head where ClothSales_Return_Code = '" & Trim(NewCode) & "'"
            'Cmd.CommandText = "Insert into e_Invoice_Head (e_Invoice_No ,        e_Invoice_date ,           Buyer_IdNo ,    Consignee_IdNo ,     Assessable_Value  ,   CGST ,           SGST  ,     IGST   ,  Cess   ,   State_Cess ,                       Round_Off           ,  Nett_Invoice_Value  ,   Ref_Sales_Code      ,                   Other_Charges               ,          Dispatcher_IdNo ) " &
            '                                "Select  ClothSales_Return_No ,    ClothSales_Return_Date,       Ledger_IdNo,    Ledger_IdNo,         Total_Taxable_Amount, CGST_Amount, SGST_Amount, IGST_Amount ,   0,          0,           RoundOff_Invoice_Value_Before_TCS    ,        Net_Amount,     '" & Trim(NewCode) & "',          ISNULL(TCS_Amount,0) as OtherCharges,         0     from ClothSales_Return_Head where ClothSales_Return_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            'Cmd.CommandText = "truncate table entrytemp"
            'Cmd.ExecuteNonQuery()

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()




            Cmd.CommandText = "Insert into e_Invoice_Details  ( Sl_No     ,     IsService       ,          Product_Description     ,                HSN_Code        ,       Batch_Details       ,   Quantity    ,          Unit    ,           Unit_Price  ,                                        Total_Amount ,                                                                                          Discount  ,           Assessable_Amount ,               GST_Rate ,      SGST_Amount , IGST_Amount , CGST_Amount , Cess_rate ,  Cess_Amount  , CessNonAdvlAmount ,  State_Cess_Rate , State_Cess_Amount , StateCessNonAdvlAmount ,         Other_Charge ,    Total_Item_Value ,        AttributesDetails ,           Ref_Sales_Code) " &
                                                  " Select      1   ,           0 as IsServc,         Ch.Cloth_Name as producDescription ,    a.Item_HSN_Code ,            '' as batchdetails,    a.Return_Meters  ,      'MTR' as UOM,        a.Amount,     (a.Total_Amount + a.Packing_Amount + a.Insurance_Amount + a.Freight )  as Total_Amount ,       (a.Trade_Discount_Perc + a.Cash_Discount_Perc ) as DiscountAmount,    a.Total_Taxable_Amount ,     a.Item_GST_Perc, 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt,                0 as OthChrg,       0 as TotItemVal,       '' as AttributesDetails, '" & Trim(NewCode) & "'  " &
                               " from ClothSales_Return_Head a LEFT OUTER JOIN cloth_Head ch on ch.Cloth_Idno = a.cloth_idno    " &
                                " Where a.ClothSales_Return_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()


            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub
        End Try


        Dim vType As String = ""

        'If Trim(UCase(vEntryType)) = "CRNT" Then
        '    vType = "CRN"
        'ElseIf Trim(UCase(vEntryType)) = "DRNT" Then
        '    vType = "DBN"
        'Else
        '    vType = "INV"
        'End If

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "ClothSales_Return_Head", "ClothSales_Return_Code", Trim(Pk_Condition), "CRN")

    End Sub
    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        If Len(Trim(txt_EInvoiceCancellationReson.Text)) = 0 Then
            MsgBox("Please provide the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "ClothSales_Return_Head", "ClothSales_Return_Code", con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)

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

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub

    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click

        btn_GENERATEEWB.Enabled = True
        btn_Generate_eInvoice.Enabled = True

        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - grp_EInvoice.Width) / 2
        Grp_EWB.Top = (Me.Height - grp_EInvoice.Height) / 2 + 200

    End Sub
    Private Sub txt_eInvoiceNo_TextChanged(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.TextChanged
        If Trim(txt_eInvoiceNo.Text) <> "" Then
            chk_Einvoice_No_Sts.Checked = True
        Else
            chk_Einvoice_No_Sts.Checked = False
        End If
    End Sub
    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        If Trim(txt_EWBNo.Text) <> "" Then
            chk_Ewb_No_Sts.Checked = True
        Else
            chk_Ewb_No_Sts.Checked = False
        End If
    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select EWB_NO from ClothSales_Return_Head where ClothSales_Return_Code = '" & NewCode & "'", con)
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

        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  , [SubSupplyType]  , [SubSupplyDesc]  ,   [DocType]  ,	       [EWBGenDocNo]   ,           [EWBDocDate]        ,    [FromGSTIN]    ,   [FromTradeName]   ,           [FromAddress1]            ,              [FromAddress2]         ,     [FromPlace]   ,    [FromPINCode]     , 	[FromStateCode]  ,  [ActualFromStateCode]  ,    [ToGSTIN]       ,    [ToTradeName]  ,                                                                    [ToAddress1]                                                                      ,																	[ToAddress2]																	   ,												[ToPlace]									    ,										[ToPINCode]								       ,    [ToStateCode] ,										 [ActualToStateCode]										   ,[TransactionType] , [OtherValue]  ,	    [Total_value]       ,	[CGST_Value]   ,    [SGST_Value] ,  [IGST_Value]    ,[CessValue],[CessNonAdvolValue],	[TransporterID]    ,	[TransporterName] ,	[TransportDOCNo] ,	[TransportDOCDate]    ,	[TotalInvValue]    ,	[TransMode]    ,	[VehicleNo]     ,	[VehicleType]   ,		[InvCode]        ,			[ShippedToGSTIN]			    , [ShippedToTradeName] )   " &
                                "SELECT             'O'        ,       '1'        ,           ''     ,     'INV'    ,  a.ClothSales_Return_No ,   a.ClothSales_Return_Date    , C.Company_GSTINNo ,   C.Company_Name    ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,   C.Company_City  , C.Company_PinCode    ,    FS.State_Code   ,       FS.State_Code     , L.Ledger_GSTINNo   , L.Ledger_MainName , (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee,    TS.State_Code , (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode ,           1      ,		0		  , A.Total_Taxable_Amount  ,   A.CGST_Amount  ,  A.SGST_Amount  , A.IGST_Amount    ,   0       ,		0           ,	t.Ledger_GSTINNo    ,		t.Ledger_Name ,    Null as lrno  ,	    Null as lrdate    ,	a.Net_Amount       ,     '1'  AS TrMode ,	a.Vehicle_No	,			'R'		, '" & Trim(NewCode) & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN , tDELV.Ledger_MainName as ShippedTo_LedgerName    " &
                                "from ClothSales_Return_Head a  " &
                                "inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo    " &
                                "Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo   " &
                                "Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo  " &
                                "left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo   " &
                                "Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo " &
                                "left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  " &
                                "left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo  " &
                                "where a.ClothSales_Return_Code ='" & Trim(NewCode) & "'"



        CMD.ExecuteNonQuery()


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim dt1 As New DataTable

        'CMD.CommandText = "Insert into e_Invoice_Details  ( Sl_No     ,     IsService       ,          Product_Description     ,                HSN_Code        ,       Batch_Details       ,   Quantity    ,          Unit    ,           Unit_Price  ,                                        Total_Amount ,                                                                                          Discount  ,           Assessable_Amount ,               GST_Rate ,      SGST_Amount , IGST_Amount , CGST_Amount , Cess_rate ,  Cess_Amount  , CessNonAdvlAmount ,  State_Cess_Rate , State_Cess_Amount , StateCessNonAdvlAmount ,         Other_Charge ,    Total_Item_Value ,        AttributesDetails ,           Ref_Sales_Code) " &
        '                                          " Select      1   ,           0 as IsServc,         Ch.Cloth_Name as producDescription ,    a.Item_HSN_Code ,            '' as batchdetails,    a.Return_Meters  ,      'MTR' as UOM,        a.Amount,     (a.Total_Amount + a.Packing_Amount + a.Insurance_Amount + a.Freight )  as Total_Amount ,       (a.Trade_Discount_Perc + a.Cash_Discount_Perc ) as DiscountAmount,    a.Total_Taxable_Amount ,     a.Item_GST_Perc, 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt,                0 as OthChrg,       0 as TotItemVal,       '' as AttributesDetails, '" & Trim(NewCode) & "'  " &
        '                       " from ClothSales_Return_Head a LEFT OUTER JOIN cloth_Head ch on ch.Cloth_Idno = a.cloth_idno    " &
        '                        " Where a.ClothSales_Return_Code = '" & Trim(NewCode) & "'"
        'CMD.ExecuteNonQuery()

        'da = New SqlClient.SqlDataAdapter(" Select  I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.Item_GST_Percentage,sum(SD.Taxable_Value) As TaxableAmt,sum(SD.Meters) as Qty,Min(Sl_No), 'MTR' AS Units " &
        '                                  " from ClothSales_Invoice_Details SD Inner Join Cloth_Head I On SD.Cloth_IdNo = I.Cloth_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
        '                                  " Where SD.ClothSales_Return_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage", con)
        'dt1 = New DataTable
        'da.Fill(dt1)

        'For I = 0 To dt1.Rows.Count - 1

        '    CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode) " &
        '                      " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",'MTR'          ," & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "')"

        '    CMD.ExecuteNonQuery()

        'Next


        CMD.CommandText = "Insert into EWB_Details (   [SlNo]   ,  [Product_Name]    ,	 [Product_Description] ,     [HSNCode]       , 	[Quantity]     ,   [QuantityUnit]    ,      Tax_Perc     ,[CessRate]    ,	[CessNonAdvol]  ,   	[TaxableAmount]     ,       InvCode) " &
                              " select                   1     ,   Ch.Cloth_Name    ,     IG.ItemGroup_Name   ,   a.Item_HSN_Code   , a.Return_Meters  ,      'MTR'         ,  a.Item_GST_Perc  ,     0        ,          0      ,  a.Total_Taxable_Amount   ,   '" & NewCode & "'  " &
                              " from ClothSales_Return_Head a LEFT OUTER JOIN cloth_Head ch on ch.Cloth_Idno = a.cloth_idno  Inner Join ItemGroup_Head IG on ch.ItemGroup_IdNo = IG.ItemGroup_IdNo  " &
                              " Where a.ClothSales_Return_Code = '" & Trim(NewCode) & "'"

        CMD.ExecuteNonQuery()


        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "ClothSales_Return_Head", "ewb_no", "ClothSales_Return_Code", Pk_Condition)
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

        EWB.CancelEWB(txt_EWBNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "ClothSales_Invoice_Head", "EWB_No", "ClothSales_Invoice_Code")

    End Sub
    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 0)
    End Sub
    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 1)
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub btn_Print_Click(sender As Object, e As EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub cbo_PartyName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_PartyName.SelectedIndexChanged

    End Sub
End Class