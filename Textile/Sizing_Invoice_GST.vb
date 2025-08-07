Imports System.IO
Imports Newtonsoft.Json
Imports RestSharp
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Net
Imports System.Windows.Forms
Imports TaxProEInvoice.API
Public Class Sizing_Invoice_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GSINV-"
    Private Pk_ConditionTDS As String = "GSTDS-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Public CHk_Details_Cnt As Integer = 0

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private Print_PDF_Status As Boolean = False

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private prn_HdDt As New DataTable
    Private dt2 As New DataTable
    Private dt3 As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_Status As Integer
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_Count As Integer = 0
    Private cnt As Integer = 0
    Public vmskGrText As String = ""
    Public vmskGrStrt As Integer = -1

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        Cbo_Tax_Type.Text = "GST"

        New_Entry = False
        Insert_Entry = False
        pnl_back.Enabled = True
        pnl_Filter.Visible = False
        Print_PDF_Status = False

        txt_InvoicePrefixNo.Text = ""
        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black
        dtp_date.Text = ""
        cbo_partyname.Text = ""
        cbo_partyname.Tag = ""
        cbo_setno.Tag = ""
        cbo_setno.Text = ""
        cbo_InvoiceSufixNo.Text = ""
        cbo_VendorName.Text = ""
        txt_GrTime.Text = ""
        msk_GrDate.Text = ""
        txt_Tds.Text = ""
        lbl_Tds_Amount.Text = ""

        vmskGrText = ""
        vmskGrStrt = -1
        cbo_DelieveryTo.Text = ""

        cbo_OnAccount.Text = ""

        cbo_DiscountType.Text = "PAISE/KG"


        If Common_Procedures.settings.CustomerCode = "1351" Then
            txt_sizingparticulars1.Text = "ENDS SIZING CHARGES"
        Else
            txt_sizingparticulars1.Text = "SIZING CHARGES"
        End If



        lbl_SizingQty1.Text = "0.000"
        txt_SizingRate1.Text = "0.00"
        lbl_SizingAmount1.Text = "0.00"

        txt_sizingparticulars2.Text = "SIZING CHARGES"
        lbl_SizingQty2.Text = "0.000"
        txt_SizingRate2.Text = "0.00"
        lbl_SizingAmount2.Text = "0.00"

        txt_sizingparticulars3.Text = "SIZING CHARGES"
        lbl_SizingQty3.Text = "0.000"
        txt_SizingRate3.Text = "0.00"
        lbl_SizingAmount3.Text = "0.00"

        txt_samplesparticulars.Text = "SAMPLE SET"
        txt_SampleSetAmount.Text = "0.00"

        txt_weldingparticulars.Text = "BEAM WELDING CHARGES"
        txt_WeldingRate.Text = "0.00"
        lbl_WeldingAmount.Text = "0.00"
        txt_WeldingBeam.Text = ""

        txt_otherchargeparticulars.Text = "OTHER CHARGES"
        txt_OtherChargesAmount.Text = "0.00"

        txt_rewindingparticulars.Text = "REWINDING CHARGES"
        txt_RewindingQuantity.Text = "0.000"
        txt_RewindingRate.Text = "0.00"
        lbl_RewindingAmount.Text = "0.00"

        txt_discountparticulars.Text = "CASH DISCOUNT"
        cbo_DiscountType.Text = "PERCENTAGE"

        If (Common_Procedures.settings.CustomerCode) = "1042" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1310" Then
            cbo_DiscountType.Text = "PAISE/KG"
        End If

        txt_DiscountRate.Text = "0.00"
        lbl_DiscountAmount.Text = "0.00"

        txt_vanrentparticulars.Text = "VAN RENT"
        txt_VanRentAmount.Text = "0.00"
        txt_PackingBeam.Text = ""

        lbl_Assessable_Value.Text = "0.00"


        lbl_CGSTPerc.Text = ""
        lbl_CGSTAmount.Text = "0.00"

        lbl_SGSTPerc.Text = ""
        lbl_SGSTAmount.Text = "0.00"

        lbl_IGSTPerc.Text = ""
        lbl_IGSTAmount.Text = "0.00"

        txt_packingparticulars.Text = "PACKING CHARGES"
        txt_PackingRate.Text = "0.00"
        lbl_PackingAmount.Text = "0.00"

        lbl_NetAmount.Text = "0.00"

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

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

        If Filter_Status = False Then

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate

            cbo_Filter_PartyName.Text = ""
            cbo_Filter_OnAccount.Text = ""
            cbo_Filter_SetNo.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_OnAccount.SelectedIndex = -1
            cbo_Filter_SetNo.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        dtp_date.Enabled = True
        If Val(Common_Procedures.settings.InvoiceEntry_Set_SetDate_To_InvoiceDate) = 1 Then
            dtp_date.Enabled = False
        End If
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

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

        'If Me.ActiveControl.Name <> cbo_ItemName.Name Then
        '    cbo_ItemName.Visible = False
        'End If
        'If Me.ActiveControl.Name <> cbo_PackingType.Name Then
        '    cbo_PackingType.Visible = False
        'End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
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

        If Not IsNothing(dgv_BackDetails.CurrentCell) Then dgv_BackDetails.CurrentCell.Selected = False
        'dgv_Details_Total.CurrentCell.Selected = False
        'dgv_lter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim NoofSets As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Invoice_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.InVoice_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Invoice_RefNo").ToString
                cbo_InvoiceSufixNo.Text = dt1.Rows(0).Item("Invoice_SuffixNo").ToString
                dtp_date.Text = dt1.Rows(0).Item("Invoice_Date").ToString
                cbo_partyname.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_partyname.Tag = Trim(cbo_partyname.Text)

                'If InStr(1, Trim(UCase(dt1.Rows(0).Item("SetCode_ForSelection").ToString)), "SZSPC-") = 0 Then
                'cbo_setno.Text = dt1.Rows(0).Item("SetCode_ForSelection").ToString & "/SZSPC-"
                'Else
                cbo_setno.Text = dt1.Rows(0).Item("SetCode_ForSelection").ToString
                'End If


                cbo_OnAccount.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("OnAccount_IdNo").ToString))

                NoofSets = 1

                If Val(dt1.Rows(0).Item("Sizing_Weight2").ToString) <> 0 Then NoofSets = NoofSets + 1
                If Val(dt1.Rows(0).Item("Sizing_Weight3").ToString) <> 0 Then NoofSets = NoofSets + 1
                Design_Details_Grid(NoofSets)

                txt_sizingparticulars1.Text = dt1.Rows(0).Item("Sizing_Text1").ToString
                lbl_SizingQty1.Text = Format(Val(dt1.Rows(0).Item("Sizing_Weight1").ToString), "#########0.000")
                txt_SizingRate1.Text = Format(Val(dt1.Rows(0).Item("Sizing_Rate1").ToString), "#########0.00")
                lbl_SizingAmount1.Text = Format(Val(dt1.Rows(0).Item("Sizing_Amount1").ToString), "#########0.00")

                txt_sizingparticulars2.Text = dt1.Rows(0).Item("Sizing_Text2").ToString
                lbl_SizingQty2.Text = Format(Val(dt1.Rows(0).Item("Sizing_Weight2").ToString), "#########0.000")
                txt_SizingRate2.Text = Format(Val(dt1.Rows(0).Item("Sizing_Rate2").ToString), "#########0.00")
                lbl_SizingAmount2.Text = Format(Val(dt1.Rows(0).Item("Sizing_Amount2").ToString), "#########0.00")

                txt_sizingparticulars3.Text = dt1.Rows(0).Item("Sizing_Text3").ToString
                lbl_SizingQty3.Text = Format(Val(dt1.Rows(0).Item("Sizing_Weight3").ToString), "#########0.000")
                txt_SizingRate3.Text = Format(Val(dt1.Rows(0).Item("Sizing_Rate3").ToString), "#########0.00")
                lbl_SizingAmount3.Text = Format(Val(dt1.Rows(0).Item("Sizing_Amount3").ToString), "#########0.00")

                lbl_Assessable_Value.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")

                lbl_CGSTPerc.Text = Format(Val(dt1.Rows(0).Item("CGST_Percentage").ToString), "#########0.00")
                lbl_CGSTAmount.Text = Format(Val(dt1.Rows(0).Item("CGST_Amount").ToString), "#########0.00")

                lbl_SGSTPerc.Text = Format(Val(dt1.Rows(0).Item("SGST_Percentage").ToString), "#########0.00")
                lbl_SGSTAmount.Text = Format(Val(dt1.Rows(0).Item("SGST_Amount").ToString), "#########0.00")

                lbl_IGSTPerc.Text = Format(Val(dt1.Rows(0).Item("IGST_Percentage").ToString), "#########0.00")
                lbl_IGSTAmount.Text = Format(Val(dt1.Rows(0).Item("IGST_Amount").ToString), "#########0.00")

                Cbo_Tax_Type.Text = dt1.Rows(0).Item("Tax_Type").ToString

                txt_samplesparticulars.Text = dt1.Rows(0).Item("SampleSet_Text").ToString
                txt_SampleSetAmount.Text = Format(Val(dt1.Rows(0).Item("SampleSet_Amount").ToString), "#########0.00")

                txt_vanrentparticulars.Text = dt1.Rows(0).Item("VanRent_Text").ToString
                txt_VanRentAmount.Text = Format(Val(dt1.Rows(0).Item("Vanrent_Amount").ToString), "#########0.00")

                txt_PackingBeam.Text = Val(dt1.Rows(0).Item("Packing_Beam").ToString)
                txt_packingparticulars.Text = dt1.Rows(0).Item("Packing_Text").ToString
                txt_PackingRate.Text = Format(Val(dt1.Rows(0).Item("Packing_Rate").ToString), "#########0.00")
                lbl_PackingAmount.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")


                txt_rewindingparticulars.Text = dt1.Rows(0).Item("Rewinding_Text").ToString
                txt_RewindingQuantity.Text = Format(Val(dt1.Rows(0).Item("Rewinding_Weight").ToString), "#########0.000")
                txt_RewindingRate.Text = Format(Val(dt1.Rows(0).Item("Rewinding_Rate").ToString), "#########0.00")
                lbl_RewindingAmount.Text = Format(Val(dt1.Rows(0).Item("Rewinding_Amount").ToString), "#########0.00")


                txt_WeldingBeam.Text = Val(dt1.Rows(0).Item("Welding_Beam").ToString)
                txt_weldingparticulars.Text = dt1.Rows(0).Item("Welding_Text").ToString
                txt_WeldingRate.Text = Format(Val(dt1.Rows(0).Item("Welding_Rate").ToString), "#########0.00")
                lbl_WeldingAmount.Text = Format(Val(dt1.Rows(0).Item("Welding_Amount").ToString), "#########0.00")

                txt_otherchargeparticulars.Text = dt1.Rows(0).Item("OtherCharges_Text").ToString
                txt_OtherChargesAmount.Text = Format(Val(dt1.Rows(0).Item("OtherCharges_Amount").ToString), "#########0.00")
                cbo_Vechile.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                txt_discountparticulars.Text = dt1.Rows(0).Item("Discount_Text").ToString
                cbo_DiscountType.Text = dt1.Rows(0).Item("Discount_Type").ToString
                txt_DiscountRate.Text = Format(Val(dt1.Rows(0).Item("Discount_Percentage").ToString), "#########0.000")
                lbl_DiscountAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                cbo_Transport_Mode.Text = dt1.Rows(0).Item("Transport_mode").ToString
                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "#########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                cbo_VendorName.Text = Common_Procedures.Vendor_IdNoToName(con, Val(dt1.Rows(0).Item("Vendor_IdNo").ToString))
                cbo_DelieveryTo.Text = Common_Procedures.Delivery_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))

                msk_GrDate.Text = dt1.Rows(0).Item("Gr_Date").ToString
                txt_GrTime.Text = dt1.Rows(0).Item("Gr_Time").ToString







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

                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EWB_Canellation_Reason.Text = Trim(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)

                txt_Tds.Text = dt1.Rows(0).Item("Tds_Perc").ToString
                lbl_Tds_Amount.Text = dt1.Rows(0).Item("Tds_Perc_Calc").ToString

            Else

                new_record()

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If cbo_partyname.Visible And cbo_partyname.Enabled Then cbo_partyname.Focus()

    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NewCode As String = ""
        Dim Nr As Long = 0
        Dim UID As Single
        Dim vUsrNm As String = "", vAcPwd As String = "", vUnAcPwd As String = ""
        Dim vOrdByNo As String = 0
        Dim NewCode2 As String = ""
        Dim NewCode3 As String = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '----- KALAIMAGAL TEXTILES (AVINASHI)
            Common_Procedures.Password_Input = ""
            Dim g As New Admin_Password
            g.ShowDialog()

            UID = 1
            Common_Procedures.get_Admin_Name_PassWord_From_DB(vUsrNm, vAcPwd, vUnAcPwd)

            vAcPwd = Common_Procedures.Decrypt(Trim(vAcPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))
            vUnAcPwd = Common_Procedures.Decrypt(Trim(vUnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))

            If Trim(Common_Procedures.Password_Input) <> Trim(vAcPwd) And Trim(Common_Procedures.Password_Input) <> Trim(vUnAcPwd) Then
                MessageBox.Show("Invalid Admin Password", "ADMIN PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_INVOICE, New_Entry, Me, con, "Invoice_Head", "Invoice_Code", NewCode, "Invoice_Date", "(Invoice_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            NewCode2 = Trim(Pk_ConditionTDS) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            NewCode3 = Trim(Pk_ConditionTDS) & Trim(NewCode)

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

            cmd.Connection = con
            cmd.Transaction = tr

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Invoice_Head", "Invoice_Code", Val(lbl_company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Invoice_Code, Company_IdNo, for_OrderBy, SetCode_ForSelection, Set_Code, Warp_Code, Total_RewindingCharges, Total_OtherCharges", tr)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode2), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode3), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_company.Tag), Trim(NewCode), tr)


            Common_Procedures.Voucher_Deletion(con, Val(lbl_company.Tag), Trim(NewCode2), tr)
            Nr = 0
            cmd.CommandText = "Update Specification_Head set invoice_code = '', invoice_increment = invoice_increment - 1 Where invoice_code = '" & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()
            If Nr <> 1 Then
                Throw New ApplicationException("Invalid Set Details - Mismatch of PartyName and Set Details")
                Exit Sub
            End If

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Dt1.Dispose()
            Da1.Dispose()
            cmd.Dispose()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_SetNo.Text = ""
            cbo_Filter_OnAccount.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_SetNo.SelectedIndex = -1
            cbo_Filter_OnAccount.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim movno As String, inpno As String
        Dim NewCode As String
        Dim SetCdSel As String

        Try

            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_INVOICE, New_Entry, Me) = False Then Exit Sub

            inpno = InputBox("Enter New Invoice.No.", "FOR INSERTION...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "Select Invoice_RefNo from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code = '" & Trim(NewCode) & "'"

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
                    MessageBox.Show("Invalid Invoice No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvoiceNo.Text = Trim(UCase(inpno))

                    SetCdSel = Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_company.Tag))

                    da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.count_name from Specification_Head a, Ledger_Head b, count_head c where a.setcode_forSelection = '" & Trim(SetCdSel) & "' and a.invoice_code = '' and a.Ledger_IdNo = b.Ledger_IdNo and a.Count_IdNo = c.Count_IdNo", con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then

                        cbo_setno.Text = SetCdSel
                        get_Set_Details(SetCdSel)

                    End If

                    dt1.Clear()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "Select top 1 Invoice_RefNo from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Invoice_RefNo"
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

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Invoice_RefNo from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Invoice_RefNo desc"
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Invoice_RefNo from Invoice_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Invoice_RefNo"
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Invoice_RefNo from Invoice_head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_company.Tag)) & " and  Invoice_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Invoice_RefNo desc"
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0
        Dim SetCdSel As String

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            dt = New DataTable
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_InvoiceNo.Text = NewID
            lbl_InvoiceNo.ForeColor = Color.Red


            da = New SqlClient.SqlDataAdapter("select top 1 * from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Invoice_RefNo desc", con)
            dt1 = New DataTable
            da.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString

                cbo_InvoiceSufixNo.Text = dt1.Rows(0).Item("Invoice_SuffixNo").ToString

                txt_sizingparticulars1.Text = dt1.Rows(0).Item("Sizing_Text1").ToString

                txt_sizingparticulars2.Text = dt1.Rows(0).Item("Sizing_Text2").ToString

                txt_sizingparticulars3.Text = dt1.Rows(0).Item("Sizing_Text3").ToString


                txt_samplesparticulars.Text = dt1.Rows(0).Item("SampleSet_Text").ToString

                txt_vanrentparticulars.Text = dt1.Rows(0).Item("VanRent_Text").ToString

                txt_packingparticulars.Text = dt1.Rows(0).Item("Packing_Text").ToString

                txt_rewindingparticulars.Text = dt1.Rows(0).Item("Rewinding_Text").ToString

                txt_weldingparticulars.Text = dt1.Rows(0).Item("Welding_Text").ToString

                txt_otherchargeparticulars.Text = dt1.Rows(0).Item("OtherCharges_Text").ToString

                txt_discountparticulars.Text = dt1.Rows(0).Item("Discount_Text").ToString

                cbo_DiscountType.Text = dt1.Rows(0).Item("Discount_Type").ToString

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then '---- Ganesh karthik Sizing (Somanur)
                SetCdSel = Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_company.Tag))

                da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.count_name from Specification_Head a, Ledger_Head b, count_head c where a.setcode_forSelection = '" & Trim(SetCdSel) & "' and a.invoice_code = '' and a.Ledger_IdNo = b.Ledger_IdNo and a.Count_IdNo = c.Count_IdNo", con)
                dt1 = New DataTable
                da.Fill(dt1)

                If dt1.Rows.Count > 0 Then

                    cbo_setno.Text = SetCdSel
                    get_Set_Details(SetCdSel)
                    get_RateDetails()
                    NetAmount_Calculation()

                End If
                dt1.Clear()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            dt.Dispose()
            da.Dispose()

            If cbo_partyname.Enabled And cbo_partyname.Visible Then cbo_partyname.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Invoice.No", "FOR FINDING...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Invoice_RefNo from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Invoice.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String = ""
        Dim NewCode2 As String = ""
        Dim NewCode3 As String = ""
        Dim vINVoNo As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim OnAc_id As Integer = 0
        Dim vSetCd As String, vSetNo As String
        Dim vSetDte As Date
        Dim VouBil As String = ""
        Dim UserIdNo As Integer = 0
        Dim vPrevRefNo As String = ""
        Dim vPrevRefDte As Date
        Dim vOrdByRefNo As String = ""
        Dim VndrNm_Id As Integer = 0
        Dim LedTo_ID As Integer = 0
        Dim vOrdByNo As String = ""
        Dim vGrDt As String = ""
        Dim vEInvAckDate As String = ""
        Dim vINV_No As String = "", vINV_SubNo As String = ""

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        UserIdNo = Common_Procedures.User.IdNo


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_INVOICE, New_Entry, Me, con, "Invoice_Head", "Invoice_Code", NewCode, "Invoice_Date", "(Invoice_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Invoice_RefNo desc", dtp_date.Value.Date) = False Then Exit Sub


        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()
            Exit Sub
        End If

        If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()
            Exit Sub
        End If


        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)
        VndrNm_Id = Common_Procedures.Vendor_AlaisNameToIdNo(con, cbo_VendorName.Text)
        LedTo_ID = Common_Procedures.Delivery_AlaisNameToIdNo(con, cbo_DelieveryTo.Text)

        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_partyname.Enabled Then cbo_partyname.Focus()
            Exit Sub
        End If

        If Trim(cbo_setno.Text) = "" Then
            MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_setno.Enabled Then cbo_setno.Focus()
            Exit Sub
        End If
        vGrDt = ""
        If Trim(msk_GrDate.Text) <> "" Then
            If IsDate(msk_GrDate.Text) = True Then
                vGrDt = Trim(msk_GrDate.Text)
            End If
        End If
        OnAc_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_OnAccount.Text)

        vSetCd = ""
        vSetNo = ""
        vSetDte = #1/1/2000#
        da = New SqlClient.SqlDataAdapter("select * from Specification_Head where setcode_forSelection = '" & Trim(cbo_setno.Text) & "'", con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            vSetCd = dt1.Rows(0).Item("Set_Code").ToString
            vSetNo = dt1.Rows(0).Item("set_no").ToString
            vSetDte = dt1.Rows(0).Item("set_date")

            If DateDiff(DateInterval.Day, vSetDte, dtp_date.Value.Date) < 0 Then
                MessageBox.Show("Invoice Invocie Date - Should not less than Set Date (" & vSetDte.ToShortDateString & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()
                Exit Sub
            End If

        End If
        dt1.Clear()

        vPrevRefNo = ""
        vPrevRefDte = #1/1/2000#
        vOrdByRefNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))
        da = New SqlClient.SqlDataAdapter("select top 1 * from Invoice_Head where for_orderby < " & Str(Val(vOrdByRefNo)) & " and company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Invoice_RefNo desc", con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            vPrevRefNo = dt1.Rows(0).Item("Invoice_RefNo").ToString
            vPrevRefDte = dt1.Rows(0).Item("Invoice_Date")

            If DateDiff(DateInterval.Day, vPrevRefDte, dtp_date.Value.Date) < 0 Then
                MessageBox.Show("Invoice Date - Invoice Date Should not less than Previous Invocie Date " & Chr(13) & "(Invocie No : " & Trim(vPrevRefNo) & "     Invocie Date : " & vPrevRefDte.ToShortDateString & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()
                Exit Sub
            End If

        End If
        dt1.Clear()

        vPrevRefNo = ""
        vPrevRefDte = #1/1/2000#
        vOrdByRefNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))
        da = New SqlClient.SqlDataAdapter("select top 1 * from Invoice_Head where for_orderby > " & Str(Val(vOrdByRefNo)) & " and company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Invoice_RefNo", con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            vPrevRefNo = dt1.Rows(0).Item("Invoice_RefNo").ToString
            vPrevRefDte = dt1.Rows(0).Item("Invoice_Date")

            If DateDiff(DateInterval.Day, vPrevRefDte, dtp_date.Value.Date) > 0 Then
                MessageBox.Show("Invoice Date - Invocie Date Should not greater than next Invocie Date " & Chr(13) & "(Invocie No : " & Trim(vPrevRefNo) & "     Invocie Date : " & vPrevRefDte.ToShortDateString & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()
                Exit Sub
            End If

        End If
        dt1.Clear()


        If Val(lbl_NetAmount.Text) = 0 Then lbl_NetAmount.Text = "0.00"
        If Val(lbl_CGSTAmount.Text) = 0 Then lbl_CGSTAmount.Text = "0.00"
        If Val(lbl_SGSTAmount.Text) = 0 Then lbl_SGSTAmount.Text = "0.00"
        If Val(lbl_IGSTAmount.Text) = 0 Then lbl_IGSTAmount.Text = "0.00"

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@InvoiceDate", dtp_date.Value.Date)

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
                '-----

            Else
                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Invoice_Head", "Invoice_Code", "For_OrderBy", "(Invoice_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_company.Tag), Common_Procedures.FnYearCode, tr)

            End If

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)

                vINV_No = Trim(lbl_InvoiceNo.Text)
                vINV_SubNo = Replace(Trim(vINV_No), Trim(Val(vINV_No)), "")

                vINVoNo = Trim(txt_InvoicePrefixNo.Text) & Trim(Format(Val(vINV_No), "######0000")) & Trim(vINV_SubNo) & Trim(cbo_InvoiceSufixNo.Text)


            Else

                vINVoNo = Trim(txt_InvoicePrefixNo.Text) & Trim(lbl_InvoiceNo.Text) & Trim(cbo_InvoiceSufixNo.Text)


            End If

            cmd.Connection = con
            cmd.Transaction = tr


            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

            Debug.Print(lbl_Tds_Amount.Text)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Invoice_Head ( User_IdNo            ,    Invoice_Code       ,               Company_IdNo      ,            Invoice_SuffixNo            ,  Invoice_RefNo                          ,      Invoice_No          ,         for_OrderBy       , Invoice_PrefixNo                    , Invoice_Date,           Ledger_IdNo   ,        SetCode_ForSelection   ,          Set_Code     ,          Set_No       ,     OnAccount_IdNo  ,               Sizing_Text1                    ,         Sizing_Weight1          ,          Sizing_Rate1            ,           Sizing_Amount1           ,                 Sizing_Text2               ,             Sizing_Weight2      ,            Sizing_Rate2         ,             Sizing_Amount2         ,               Sizing_Text3                 ,             Sizing_Weight3      ,             Sizing_Rate3         ,             Sizing_Amount3         ,              SampleSet_Text                 ,             SampleSet_Amount         ,                 VanRent_Text               ,             VanRent_Amount         ,             Packing_Beam         ,               Packing_Text                 ,              Packing_Rate        ,             Packing_Amount         ,               Rewinding_Text                 ,             Rewinding_Weight           ,             Rewinding_Rate         ,             Rewinding_Amount         ,             Welding_Beam         ,               Welding_Text                 ,             Welding_Rate         ,             Welding_Amount         ,               OtherCharges_Text                ,             OtherCharges_Amount         ,              Discount_Text                 ,               Discount_Type          ,             Discount_Percentage   ,           Discount_Amount           ,             Assessable_Value          ,             CGST_Percentage   ,             CGST_Amount         ,              SGST_Percentage  ,             SGST_Amount         ,              IGST_Percentage   ,             IGST_Amount         ,             Net_Amount          ,               Tax_Type              , Transport_Mode                         ,  Vehicle_No                 ,   Vendor_IdNo          , DeliveryTo_IdNo              ,        Gr_Time                   ,         Gr_Date    ,                  Tds_Perc      ,                  Tds_Perc_Calc        ,  E_Invoice_IRNO     ,   E_Invoice_QR_Image   ) " &
                                  "            Values       (" & Str(UserIdNo) & " ,'" & Trim(NewCode) & "', " & Str(Val(lbl_company.Tag)) & ", '" & Trim(cbo_InvoiceSufixNo.Text) & "', '" & Trim(lbl_InvoiceNo.Text) & "'     ,   '" & Trim(vInvoNo) & "', " & Str(Val(vOrdByNo)) & ", '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "', @InvoiceDate,      " & Val(led_id) & ", '" & Trim(cbo_setno.Text) & "', '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "', " & Val(OnAc_id) & ", '" & Trim(txt_sizingparticulars1.Text) & "', " & Val(lbl_SizingQty1.Text) & ", " & Val(txt_SizingRate1.Text) & ", " & Val(lbl_SizingAmount1.Text) & ", '" & Trim(txt_sizingparticulars2.Text) & "', " & Val(lbl_SizingQty2.Text) & "," & Val(txt_SizingRate2.Text) & ", " & Val(lbl_SizingAmount2.Text) & ", '" & Trim(txt_sizingparticulars3.Text) & "', " & Val(lbl_SizingQty3.Text) & ", " & Val(txt_SizingRate3.Text) & ", " & Val(lbl_SizingAmount3.Text) & ",  '" & Trim(txt_samplesparticulars.Text) & "', " & Val(txt_SampleSetAmount.Text) & ", '" & Trim(txt_vanrentparticulars.Text) & "', " & Val(txt_VanRentAmount.Text) & ", " & Val(txt_PackingBeam.Text) & ", '" & Trim(txt_packingparticulars.Text) & "', " & Val(txt_PackingRate.Text) & ", " & Val(lbl_PackingAmount.Text) & ", '" & Trim(txt_rewindingparticulars.Text) & "', " & Val(txt_RewindingQuantity.Text) & ", " & Val(txt_RewindingRate.Text) & ", " & Val(lbl_RewindingAmount.Text) & ", " & Val(txt_WeldingBeam.Text) & ", '" & Trim(txt_weldingparticulars.Text) & "', " & Val(txt_WeldingRate.Text) & ", " & Val(lbl_WeldingAmount.Text) & ", '" & Trim(txt_otherchargeparticulars.Text) & "', " & Val(txt_OtherChargesAmount.Text) & ", '" & Trim(txt_discountparticulars.Text) & "', '" & Trim(cbo_DiscountType.Text) & "', " & Val(txt_DiscountRate.Text) & ", " & Val(lbl_DiscountAmount.Text) & ", " & Val(lbl_Assessable_Value.Text) & ", " & Val(lbl_CGSTPerc.Text) & ", " & Val(lbl_CGSTAmount.Text) & ", " & Val(lbl_SGSTPerc.Text) & ", " & Val(lbl_SGSTAmount.Text) & ",  " & Val(lbl_IGSTPerc.Text) & ", " & Val(lbl_IGSTAmount.Text) & ", " & Val(lbl_NetAmount.Text) & " , '" & Trim(Cbo_Tax_Type.Text) & "','" & Trim(cbo_Transport_Mode.Text) & "' ,'" & Trim(cbo_Vechile.Text) & "', " & Val(VndrNm_Id) & " , " & Val(LedTo_ID) & "," & Str(Val(txt_GrTime.Text)) & ", '" & Trim(vGrDt) & "'," & Str(Val(txt_Tds.Text)) & ",  " & Str(Val(lbl_Tds_Amount.Text)) & " ,      '" & Trim(txt_IR_No.Text) & "' ,     @QrCode  )"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Invoice_Head", "Invoice_Code", Val(lbl_company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Invoice_Code, Company_IdNo, for_OrderBy, SetCode_ForSelection, Set_Code, Warp_Code, Total_RewindingCharges, Total_OtherCharges", tr)

                nr = 0
                cmd.CommandText = "Update Specification_Head set invoice_code = '', invoice_increment = invoice_increment - 1 Where invoice_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                If nr <> 0 Then
                    Throw New ApplicationException("invalid set details - mismatch of partyname and set details")
                    Exit Sub
                End If

                cmd.CommandText = "Update Invoice_Head set Invoice_Date = @InvoiceDate, User_IdNo=" & Str(UserIdNo) & " , Ledger_IdNo = " & Val(led_id) & ", SetCode_ForSelection = '" & Trim(cbo_setno.Text) & "',Invoice_SuffixNo  = '" & Trim(cbo_InvoiceSufixNo.Text) & "',Invoice_No = '" & Trim(vINVoNo) & "', Invoice_RefNo =  '" & Trim(lbl_InvoiceNo.Text) & "'  ,  Invoice_PrefixNo = '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' , Set_Code = '" & Trim(vSetCd) & "', Set_No = '" & Trim(vSetNo) & "', OnAccount_IdNo = " & Val(OnAc_id) & ", Sizing_Text1 = '" & Trim(txt_sizingparticulars1.Text) & "', Sizing_Weight1 = " & Val(lbl_SizingQty1.Text) & ", Sizing_Rate1 = " & Val(txt_SizingRate1.Text) & ", Sizing_Amount1 = " & Val(lbl_SizingAmount1.Text) & ", Sizing_Text2 = '" & Trim(txt_sizingparticulars2.Text) & "', Sizing_Weight2 = " & Val(lbl_SizingQty2.Text) & ", Sizing_Rate2 = " & Val(txt_SizingRate2.Text) & ", Sizing_Amount2 = " & Val(lbl_SizingAmount2.Text) & ", Sizing_Text3 = '" & Trim(txt_sizingparticulars3.Text) & "', Sizing_Weight3 = " & Val(lbl_SizingQty3.Text) & ", Sizing_Rate3 = " & Val(txt_SizingRate3.Text) & ", Sizing_Amount3 = " & Val(lbl_SizingAmount3.Text) & ", SampleSet_Text ='" & Trim(txt_samplesparticulars.Text) & "', SampleSet_Amount = " & Val(txt_SampleSetAmount.Text) & ", VanRent_Text = '" & Trim(txt_vanrentparticulars.Text) & "', VanRent_Amount = " & Val(txt_VanRentAmount.Text) & ", Packing_Beam = " & Val(txt_PackingBeam.Text) & ", Packing_Text = '" & Trim(txt_packingparticulars.Text) & "', Packing_Rate = " & Val(txt_PackingRate.Text) & ", Packing_Amount = " & Val(lbl_PackingAmount.Text) & ", Rewinding_Text = '" & Trim(txt_rewindingparticulars.Text) & "', Rewinding_Weight = " & Val(txt_RewindingQuantity.Text) & ", Rewinding_Rate = " & Val(txt_RewindingRate.Text) & ", Rewinding_Amount = " & Val(lbl_RewindingAmount.Text) & ", Welding_Beam = " & Val(txt_WeldingBeam.Text) & ", Welding_Text = '" & Trim(txt_weldingparticulars.Text) & "', Welding_Rate = " & Val(txt_WeldingRate.Text) & ", Welding_Amount = " & Val(lbl_WeldingAmount.Text) & ", OtherCharges_Text = '" & Trim(txt_otherchargeparticulars.Text) & "', OtherCharges_Amount = " & Val(txt_OtherChargesAmount.Text) & ", Discount_Text = '" & Trim(txt_discountparticulars.Text) & "', Discount_Type = '" & Trim(cbo_DiscountType.Text) & "', Discount_Percentage = " & Val(txt_DiscountRate.Text) & ", Discount_Amount = " & Val(lbl_DiscountAmount.Text) & ", Assessable_Value =  " & Val(lbl_Assessable_Value.Text) & ", CGST_Percentage =  " & Val(lbl_CGSTPerc.Text) & ",CGST_Amount = " & Val(lbl_CGSTAmount.Text) & " ,   SGST_Percentage =  " & Val(lbl_SGSTPerc.Text) & ",SGST_Amount = " & Val(lbl_SGSTAmount.Text) & " ,  iGST_Percentage =  " & Val(lbl_IGSTPerc.Text) & ",iGST_Amount = " & Val(lbl_IGSTAmount.Text) & " ,  Net_Amount = " & Val(lbl_NetAmount.Text) & ", Tax_Type =  '" & Trim(Cbo_Tax_Type.Text) & "',Transport_Mode = '" & Trim(cbo_Transport_Mode.Text) & "',Vehicle_No = '" & Trim(cbo_Vechile.Text) & "',Vendor_IdNo = " & Val(VndrNm_Id) & " , DeliveryTo_IdNo = " & Val(LedTo_ID) & " ,Gr_Time=" & Str(Val(txt_GrTime.Text)) & ", Gr_date='" & Trim(vGrDt) & "',Tds_Perc=" & Str(Val(txt_Tds.Text)) & ", Tds_Perc_Calc=" & Str(Val(lbl_Tds_Amount.Text)) & "  , E_Invoice_IRNO = '" & Trim(txt_IR_No.Text) & "' , E_Invoice_QR_Image =  @QrCode , E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & " , " &
                " E_Invoice_Cancelled_Status = " & eiCancel.ToString & " ,  E_Invoice_Cancellation_Reason = '" & txt_EInvoiceCancellationReson.Text & "'  ,    EWB_No = '' , EWB_Date = '" & txt_EWB_Date.Text & "',EWB_Valid_Upto = '" & txt_EWB_ValidUpto.Text & "',EWB_Cancelled = " & EWBCancel.ToString & " ,  EWBCancellation_Reason = '" & txt_EWB_Canellation_Reason.Text & "' Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " And Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Invoice_Head", "Invoice_Code", Val(lbl_company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Invoice_Code, Company_IdNo, for_OrderBy, SetCode_ForSelection, Set_Code, Warp_Code, Total_RewindingCharges, Total_OtherCharges", tr)



            nr = 0
            cmd.CommandText = "Update Specification_Head set invoice_code = '" & Trim(NewCode) & "', invoice_increment = invoice_increment + 1 Where invoice_code = '' and setcode_forSelection = '" & Trim(cbo_setno.Text) & "' and Set_Code = '" & Trim(vSetCd) & "' and Ledger_IdNo = " & Str(Val(led_id))
            nr = cmd.ExecuteNonQuery()
            If nr <> 1 Then
                Throw New ApplicationException("Invalid Set Details - Mismatch of PartyName and Set Details")
                Exit Sub
            End If

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Cr_ID = Common_Procedures.CommonLedger.SIZING_JOBWORK_CHARGES_AC ' 2
            If Val(OnAc_id) <> 0 Then
                Dr_ID = Val(OnAc_id)
            Else
                Dr_ID = Val(led_id)
            End If




            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            Dim vNetAmt As Double = Format(Val(CSng(lbl_NetAmount.Text)), "#############0.00")
            Dim vCGSTAmt As Double = Format(Val(CSng(lbl_CGSTAmount.Text)), "#############0.00")
            Dim vSGSTAmt As Double = Format(Val(CSng(lbl_SGSTAmount.Text)), "#############0.00")
            Dim vIGSTAmt As Double = Format(Val(CSng(lbl_IGSTAmount.Text)), "#############0.00")

            '---GST
            Dim vGSTPerc As String = ""
            Dim vCGST_AcIdNo As String = ""
            Dim vSGST_AcIdNo As String = ""
            Dim vIGST_AcIdNo As String = ""

            If Val(vIGSTAmt) <> 0 Then
                vGSTPerc = Val(lbl_IGSTPerc.Text)
            Else
                vGSTPerc = (Val(lbl_CGSTPerc.Text) + Val(lbl_SGSTPerc.Text))
            End If

            vCGST_AcIdNo = Common_Procedures.get_FieldValue(con, "GST_AccountSettings_Head", "OP_CGST_Ac_IdNo", "(GST_Percentage = " & Str(Val(vGSTPerc)) & ")", , tr)
            vSGST_AcIdNo = Common_Procedures.get_FieldValue(con, "GST_AccountSettings_Head", "OP_SGST_Ac_IdNo", "(GST_Percentage = " & Str(Val(vGSTPerc)) & ")", , tr)
            vIGST_AcIdNo = Common_Procedures.get_FieldValue(con, "GST_AccountSettings_Head", "OP_IGST_Ac_IdNo", "(GST_Percentage = " & Str(Val(vGSTPerc)) & ")", , tr)

            If Val(vCGST_AcIdNo) = 0 Then vCGST_AcIdNo = 24
            If Val(vSGST_AcIdNo) = 0 Then vSGST_AcIdNo = 25
            If Val(vIGST_AcIdNo) = 0 Then vIGST_AcIdNo = 26





            vLed_IdNos = Dr_ID & "|" & Cr_ID & "|" & Trim(Val(vCGST_AcIdNo)) & "|" & Trim(Val(vSGST_AcIdNo)) & "|" & Trim(Val(vIGST_AcIdNo))

            vVou_Amts = -1 * vNetAmt & "|" & vNetAmt - (vCGSTAmt + vSGSTAmt + vIGSTAmt) & "|" & vCGSTAmt & "|" & vSGSTAmt & "|" & vIGSTAmt

            If Common_Procedures.Voucher_Updation(con, "Gst.Siz.Inv", Val(lbl_company.Tag), Trim(NewCode), Trim(lbl_InvoiceNo.Text), Convert.ToDateTime(dtp_date.Text), "Bill.No : " & Trim(vINVoNo) & "", vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Sizing_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            NewCode3 = Trim(Pk_ConditionTDS) & Trim(NewCode)
            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode3), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            '---Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_company.Tag), dtp_date.Text, led_id, Trim(vINVoNo), 0, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Sizing_Software, SaveAll_STS)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If


            '--------TDS 
            NewCode2 = Trim(Pk_ConditionTDS) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_company.Tag), Trim(NewCode2), tr)

            vLed_IdNos = ""
            vVou_Amts = ""
            ErrMsg = ""

            vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Receivable_Ac) & "|" & Dr_ID
            vVou_Amts = -1 * Val(CSng(lbl_Tds_Amount.Text)) & "|" & Val(CSng(lbl_Tds_Amount.Text))
            If Common_Procedures.Voucher_Updation(con, "Siz.Inv.Tds", Val(lbl_company.Tag), Trim(NewCode2), Trim(lbl_InvoiceNo.Text), Convert.ToDateTime(dtp_date.Text), " Inv No : " & Trim(vINVoNo), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Sizing_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If
            '----------TDS

            If Val(Common_Procedures.User.IdNo) = 1 Then
                If chk_Printed.Visible = True Then
                    If chk_Printed.Enabled = True Then
                        Update_PrintOut_Status(tr)
                    End If
                End If
            End If

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If


            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_InvoiceNo.Text)
                End If

            Else
                move_record(lbl_InvoiceNo.Text)

            End If

        Catch ex As Exception
            tr.Rollback()
            Timer1.Enabled = False
            SaveAll_STS = False

            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da.Dispose()
            tr.Dispose()
            cmd.Dispose()

            If cbo_partyname.Enabled And cbo_partyname.Visible Then cbo_partyname.Focus()

        End Try


    End Sub
    Private Sub GraceTime_Calculation()

        msk_GrDate.Text = ""
        If IsDate(dtp_date.Text) = True And Val(txt_GrTime.Text) >= 0 Then
            msk_GrDate.Text = DateAdd("d", Val(txt_GrTime.Text), Convert.ToDateTime(dtp_date.Text))
        End If

    End Sub
    Private Sub Invoice_GST_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_partyname.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_partyname.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_OnAccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_OnAccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VendorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VendorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then

                lbl_company.Text = ""
                lbl_company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                lbl_company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                lbl_company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_company.Text

                FrmLdSTS = False
                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub


    Private Sub Invoice_GST_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Cmp_Cond As String = ""

        Me.Text = ""

        TableLayoutPanel1.Tag = ""
        Design_Details_Grid(1)

        con.Open()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1097" Then '---- Ganapathy Murugan Sizing (Somanur)
            txt_SampleSetAmount.TabStop = False
            txt_VanRentAmount.TabStop = False
            txt_PackingBeam.TabStop = False
            txt_RewindingQuantity.TabStop = False
            txt_WeldingBeam.TabStop = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- BRT SIZING MILLS
            lbl_DelieveryTo.Visible = True
            cbo_DelieveryTo.Visible = True
        End If


        cbo_InvoiceSufixNo.Items.Clear()
        cbo_InvoiceSufixNo.Items.Add("")
        cbo_InvoiceSufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_InvoiceSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))

        Cbo_Tax_Type.Items.Clear()
        Cbo_Tax_Type.Items.Add("")
        Cbo_Tax_Type.Items.Add("GST")
        Cbo_Tax_Type.Items.Add("NO TAX")


        cbo_DiscountType.Items.Clear()
        cbo_DiscountType.Items.Add("")
        cbo_DiscountType.Items.Add("PERCENTAGE")
        cbo_DiscountType.Items.Add("PAISE/KG")


        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            btn_SaveAll.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1006" Then
            lbl_vendorname_Caption.Visible = True
            cbo_VendorName.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1363" Then '---- Balaji karadvavi

            lbl_grdate.Visible = True
            lbl_grtime.Visible = True
            txt_GrTime.Visible = True
            msk_GrDate.Visible = True
            dtp_GrDate.Visible = True


        End If

        Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Da.Fill(Dt1)
        cbo_partyname.DataSource = Dt1
        cbo_partyname.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select setcode_forSelection from Specification_Head where invoice_code = '' order by setcode_forSelection", con)
        Da.Fill(Dt2)
        cbo_setno.DataSource = Dt2
        cbo_setno.DisplayMember = "setcode_forSelection"


        Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or a.Ledger_IdNo = 1 or b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Da.Fill(Dt3)
        cbo_OnAccount.DataSource = Dt3
        cbo_OnAccount.DisplayMember = "Ledger_DisplayName"

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            chk_Printed.Enabled = True
        End If

        btn_UserModification.Visible = False
        If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
            btn_UserModification.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Then
            lbl_EIRn_Caption.Visible = True
            txt_IR_No.Visible = True
        Else

            lbl_EIRn_Caption.Visible = False
            txt_IR_No.Visible = False
        End If


        AddHandler txt_InvoicePrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_InvoiceSufixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_partyname.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_setno.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OnAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_setno.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Tax_Type.GotFocus, AddressOf ControlGotFocus
        ''AddHandler txt_sizingparticulars2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PackingBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PackingRate.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_vat1particular.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_vat2particular.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Tds.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Tds_Amount.GotFocus, AddressOf ControlGotFocus



        AddHandler lbl_CGSTPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_SGSTPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VanRentAmount.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_vanrentparticulars.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SampleSetAmount.GotFocus, AddressOf ControlGotFocus
        ' AddHandler txt_samplesparticulars.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SizingRate1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SizingRate2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SizingRate3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SampleSetAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscountRate.GotFocus, AddressOf ControlGotFocus
        ' AddHandler txt_OtherChargesAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OtherChargesAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeldingBeam.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_weldingparticulars.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeldingRate.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_rewindingparticulars.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RewindingQuantity.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RewindingRate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        'AddHandler dtp_lter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_OnAccount.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_GrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrTime.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Close.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport_Mode.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VendorName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelieveryTo.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IR_No.GotFocus, AddressOf ControlGotFocus




        AddHandler cbo_InvoiceSufixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvoicePrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_partyname.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_setno.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OnAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_setno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SampleSetAmount.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PackingBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PackingRate.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_vat1particular.GotFocus, AddressOf ControlLostFocus
        'AddHandler txt_vat2particular.GotFocus, AddressOf ControlLostFocus
        AddHandler msk_GrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GrTime.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Tds.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Tds_Amount.LostFocus, AddressOf ControlLostFocus


        AddHandler lbl_CGSTPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_SGSTPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VanRentAmount.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_vanrentparticulars.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_SampleSetAmount.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_samplesparticulars.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_SizingRate1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SizingRate2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SizingRate3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_discountparticulars.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscountRate.LostFocus, AddressOf ControlLostFocus
        '  AddHandler txt_otherchargeparticulars.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_OtherChargesAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeldingBeam.LostFocus, AddressOf ControlLostFocus
        ' AddHandler txt_weldingparticulars.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_WeldingRate.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Tax_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RewindingQuantity.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RewindingRate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DiscountType.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_OnAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport_Mode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VendorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelieveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IR_No.LostFocus, AddressOf ControlLostFocus


        '  AddHandler msk_GrDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GrTime.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_sizingparticulars3.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_PackingRate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_pt2particular.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler lbl_CGSTPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_SGSTPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VanRentAmount.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_vanrentparticulars.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SampleSetAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PackingBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SizingRate1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SizingRate2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SizingRate3.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_discountparticulars.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler txt_otherchargeparticulars.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OtherChargesAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeldingBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_weldingparticulars.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeldingRate.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_rewindingparticulars.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RewindingQuantity.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RewindingRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_IR_No.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        '  AddHandler msk_GrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GrTime.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_IR_No.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvoicePrefixNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_sizingparticulars3.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_PackingBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PackingRate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler lbl_CGSTPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_SGSTPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VanRentAmount.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_vanrentparticulars.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SampleSetAmount.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_samplesparticulars.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SizingRate1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SizingRate2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SizingRate3.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_discountparticulars.KeyPress, AddressOf TextBoxControlKeyPress

        'AddHandler txt_otherchargeparticulars.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OtherChargesAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeldingBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_weldingparticulars.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeldingRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_rewindingparticulars.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RewindingQuantity.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RewindingRate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress



        lbl_company.Text = ""
        lbl_company.Tag = 0
        lbl_company.Visible = False
        Common_Procedures.CompIdNo = 0

        ' Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub

    Private Sub Invoice_GST_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Invoice_GST_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            lbl_company.Tag = 0
            lbl_company.Text = ""
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
                            lbl_company.Tag = Val(dt1.Rows(0)(0).ToString)
                            lbl_company.Text = Trim(dt1.Rows(0)(1).ToString)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_partyname_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_partyname.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        cbo_partyname.Tag = Trim(cbo_partyname.Text)
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_partyname, cbo_Transport_Mode, Cbo_Tax_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_partyname.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Led_Id As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_partyname, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")

        Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)

        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_partyname.Tag)) <> Trim(UCase(cbo_partyname.Text)) Then
                cbo_partyname.Tag = Trim(cbo_partyname.Text)
                get_RateDetails()

                If Led_Id <> 0 Then
                    da = New SqlClient.SqlDataAdapter("select a.Tds_Perc from Ledger_Head a where a.Ledger_IdNo = " & Str(Val(Led_Id)) & " ", con)
                    da.Fill(dt1)
                    If dt1.Rows.Count > 0 Then
                        txt_Tds.Text = Format(Val(dt1.Rows(0).Item("Tds_Perc").ToString), "########0.00")
                    End If
                    dt1.Clear()
                End If

                NetAmount_Calculation()


            End If



            Cbo_Tax_Type.Focus()

        End If
    End Sub

    Private Sub cbo_OnAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OnAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")
    End Sub

    Private Sub cbo_onAccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OnAccount, cbo_setno, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")

        If e.KeyCode = 40 And cbo_OnAccount.DroppedDown = False Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_VendorName.Visible Then
                cbo_VendorName.Focus()
            ElseIf txt_GrTime.Visible Then
                txt_GrTime.Focus()
            ElseIf txt_IR_No.Visible Then
                txt_IR_No.Focus()
            Else

                txt_SizingRate1.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_onAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OnAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OnAccount, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10  or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_VendorName.Visible Then
                cbo_VendorName.Focus()
            ElseIf txt_GrTime.Visible Then
                txt_GrTime.Focus()

            ElseIf txt_IR_No.Visible Then
                txt_IR_No.Focus()
            Else
                txt_SizingRate1.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_partyname.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_OnAccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_OnAccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub cbo_setno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_setno.GotFocus
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try
            cbo_setno.Tag = cbo_setno.Text
            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)

            Condt = ""
            If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
                Condt = " Company_IdNo = " & Str(Val(lbl_company.Tag))
            End If

            If Val(Led_ID) <> 0 Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " ( ( Ledger_IdNo = 0 or Ledger_IdNo = " & Str(Val(Led_ID)) & " ) and (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') )"
            Else
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') "
            End If

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Specification_Head", "setcode_forSelection", "(" & Condt & ")", "(set_code = '')")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_setno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_setno.KeyDown
        Dim Led_ID As Integer = 0
        Dim Condt As String = ""
        Dim NewCode As String = ""

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)

            Condt = ""
            If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
                Condt = " Company_IdNo = " & Str(Val(lbl_company.Tag))
            End If

            If Val(Led_ID) <> 0 Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " ( ( Ledger_IdNo = 0 or Ledger_IdNo = " & Str(Val(Led_ID)) & " ) and (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') )"
            Else
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') "
            End If

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_setno, Nothing, cbo_OnAccount, "Specification_Head", "setcode_forSelection", "(" & Condt & ")", "(set_code = '')")

            If e.KeyCode = 38 Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                    cbo_DelieveryTo.Focus()
                ElseIf cbo_VendorName.Visible Then
                    cbo_VendorName.Focus()
                Else
                    cbo_Vechile.Focus()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_setno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_setno.KeyPress
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)

            Condt = ""
            If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
                Condt = " Company_IdNo = " & Str(Val(lbl_company.Tag))
            End If
            Condt = ""

            If Val(Led_ID) <> 0 Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " ( ( Ledger_IdNo = 0 or Ledger_IdNo = " & Str(Val(Led_ID)) & " ) and (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') )"
            Else
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') "
            End If

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_setno, Nothing, "Specification_Head", "setcode_forSelection", "(" & Condt & ")", "(set_code = '')")

            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_setno.Tag)) <> Trim(UCase(cbo_setno.Text)) Then
                    get_Set_Details(cbo_setno.Text)
                End If
                cbo_OnAccount.Focus()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub txt_sizing1rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SizingRate1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    'Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
    '    Try
    '        With cbo_Filter_PartyName
    '            If e.KeyValue = 38 And .DroppedDown = False Then
    '                e.Handled = True
    '                dtp_FilterTo_date.Focus()
    '                'SendKeys.Send("+{TAB}")
    '            ElseIf e.KeyValue = 40 And .DroppedDown = False Then
    '                e.Handled = True
    '                btn_filtershow.Focus()
    '                'SendKeys.Send("{TAB}")
    '            ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
    '                .DroppedDown = True
    '            End If
    '        End With

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try
    'End Sub

    'Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim Condt As String
    '    Dim FindStr As String

    '    With cbo_Filter_PartyName

    '        If Asc(e.KeyChar) = 13 Then

    '            If Trim(.Text) <> "" Then
    '                If .DroppedDown = True Then
    '                    If Trim(.SelectedText) <> "" Then
    '                        .Text = .SelectedText
    '                    Else
    '                        If .Items.Count > 0 Then
    '                            .SelectedIndex = 0
    '                            .SelectedItem = .Items(0)
    '                            .Text = .GetItemText(.SelectedItem)
    '                        End If
    '                    End If
    '                End If
    '            End If

    '            btn_filtershow.Focus()

    '        Else

    '            Condt = ""
    '            FindStr = ""

    '            If Asc(e.KeyChar) = 8 Then
    '                If .SelectionStart <= 1 Then
    '                    .Text = ""
    '                End If

    '                If Trim(.Text) <> "" Then
    '                    If .SelectionLength = 0 Then
    '                        FindStr = .Text.Substring(0, .Text.Length - 1)
    '                    Else
    '                        FindStr = .Text.Substring(0, .SelectionStart - 1)
    '                    End If
    '                End If

    '            Else
    '                If .SelectionLength = 0 Then
    '                    FindStr = .Text & e.KeyChar
    '                Else
    '                    FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
    '                End If

    '            End If

    '            FindStr = LTrim(FindStr)

    '            Condt = "(a.ledger_idno = 0 or b.AccountsGroup_IdNo = 10)"
    '            If Trim(FindStr) <> "" Then
    '                Condt = " b.AccountsGroup_IdNo = 10 and (a.Ledger_DisplayName like '" & FindStr & "%' or a.Ledger_DisplayName like '% " & FindStr & "%') "
    '            End If

    '            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where " & Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = b.Ledger_IdNo Order by a.Ledger_DisplayName", con)
    '            da.Fill(dt)

    '            .DataSource = dt
    '            .DisplayMember = "Ledger_DisplayName"


    '            .Text = Trim(FindStr)

    '            .SelectionStart = FindStr.Length

    '            e.Handled = True

    '        End If

    '    End With

    'End Sub

    'Private Sub dtp_FilterTo_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FilterTo_date.KeyDown
    '    If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    '    If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    'End Sub


    'Private Sub dtp_FilterTo_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FilterTo_date.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        SendKeys.Send("{TAB}")
    '    End If
    'End Sub


    'Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable
    '    Dim dt2 As New DataTable
    '    Dim n As Integer
    '    Dim Led_IdNo As Integer, Itm_IdNo As Integer
    '    Dim Condt As String = ""

    '    Try

    '        Condt = ""
    '        Led_IdNo = 0
    '        Itm_IdNo = 0

    '        If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
    '            Condt = "a.Empty_BeamBagCone_Delivery_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
    '        ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
    '            Condt = "a.Empty_BeamBagCone_Delivery_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
    '        ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
    '            Condt = "a. Empty_BeamBagCone_Delivery_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
    '        End If

    '        If Trim(cbo_Filter_PartyName.Text) <> "" Then
    '            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
    '        End If

    '        If Val(Led_IdNo) <> 0 Then
    '            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
    '        End If

    '        da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Empty_BeamBagCone_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Empty_BeamBagCone_Delivery_No", con)
    '        da.Fill(dt2)

    '        dgv_filter.Rows.Clear()

    '        If dt2.Rows.Count > 0 Then

    '            For i = 0 To dt2.Rows.Count - 1

    '                n = dgv_filter.Rows.Add()

    '                dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Empty_BeamBagCone_Delivery_No").ToString
    '                dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
    '                dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
    '                dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Empty_beam").ToString
    '                dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Empty_Bags").ToString
    '                dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Empty_Cones").ToString

    '            Next i

    '        End If

    '        dt2.Clear()
    '        dt2.Dispose()
    '        da.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    '    If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    'End Sub

    'Private Sub dtp_FilterFrom_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FilterFrom_date.KeyDown
    '    If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    '    If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    'End Sub


    'Private Sub dtp_FilterFrom_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FilterFrom_date.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        SendKeys.Send("{TAB}")
    '    End If
    'End Sub

    'Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
    '    Open_FilterEntry()
    'End Sub

    'Private Sub btn_filtershow_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_filtershow.KeyDown
    '    If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    '    If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    'End Sub

    'Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        cbo_Filter_PartyName.Focus()
    '    End If
    'End Sub


    'Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
    '    pnl_back.Enabled = True
    '    pnl_filter.Visible = False
    '    Filter_Status = False
    'End Sub

    'Private Sub dgv_filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellDoubleClick
    '    Open_FilterEntry()
    'End Sub

    'Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
    '    If e.KeyCode = 13 Then
    '        Open_FilterEntry()
    '    End If
    'End Sub

    ''Private Sub Open_FilterEntry()
    ''    Dim movno As String

    ''    movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

    ''    If Val(movno) <> 0 Then
    ''        Filter_Status = True
    ''        move_record(movno)
    ''        pnl_back.Enabled = True
    ''        pnl_filter.Visible = False
    ''    End If

    ''End Sub


    Private Sub txt_sizing2rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SizingRate2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_sizing3rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SizingRate3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_warpingrate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeldingRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_vanrentamount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VanRentAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_vat1quantity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_vat1rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_vat2beam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_vat2quantity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_weldingrate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeldingRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub get_Set_Details(ByVal SelcSetCd As String)
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim eds() As String, wwg() As String
        Dim New_Code As String
        Dim WarpWgt As Single
        Dim LedRt As Single = 0
        Dim End_Id As Integer = 0
        Dim Cnt_Id As Integer = 0
        Dim Led_Id As Integer = 0
        Dim TotWgt As Integer
        Dim vInterStateStatus As Boolean = False
        New_Code = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'cbo_partyname.Text = ""
        'cbo_setno.Text = ""
        lbl_SizingQty1.Text = "0.000"
        lbl_SizingQty2.Text = "0.000"
        lbl_SizingQty3.Text = "0.000"
        txt_SizingRate1.Text = "0.00"
        txt_SizingRate2.Text = "0.00"
        txt_SizingRate3.Text = "0.00"
        txt_PackingBeam.Text = "0"
        txt_RewindingQuantity.Text = "0.000"

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Count_Name,c.Count_Gst_Perc from Specification_Head a  Left Outer Join Ledger_Rate_Details d ON  a.Ledger_IdNo = D.Ledger_IdNo, Ledger_Head b, count_head c where a.setcode_forSelection = '" & Trim(SelcSetCd) & "' and (a.invoice_code = '' or a.invoice_code = '" & Trim(New_Code) & "') and a.Ledger_IdNo = b.Ledger_IdNo and a.Count_IdNo = c.Count_IdNo    ", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            cbo_partyname.Text = Dt1.Rows(0).Item("Ledger_Name").ToString

            If Val(Common_Procedures.settings.InvoiceEntry_Set_SetDate_To_InvoiceDate) = 1 Then
                dtp_date.Text = Dt1.Rows(0).Item("Set_Date").ToString
            End If

            eds = Split(Dt1.Rows(0).Item("ends_name").ToString, ",")

            wwg = Split(Dt1.Rows(0).Item("warp_weight").ToString, ",")

            WarpWgt = 0
            If UBound(wwg) >= 0 Then WarpWgt = WarpWgt + Val(wwg(0))
            If UBound(wwg) >= 1 Then WarpWgt = WarpWgt + Val(wwg(1))
            If UBound(wwg) >= 2 Then WarpWgt = WarpWgt + Val(wwg(2))

            Call Design_Details_Grid(UBound(eds) + 1)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
                txt_sizingparticulars1.Text = Dt1.Rows(0).Item("Count_Name").ToString & IIf(InStr(1, UCase(Dt1.Rows(0).Item("Count_Name").ToString), "S") = 0, "s", "") & " - " & Dt1.Rows(0).Item("ends_name").ToString & " SIZING CHARGES"
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                txt_sizingparticulars1.Text = Dt1.Rows(0).Item("Count_Name").ToString & IIf(InStr(1, UCase(Dt1.Rows(0).Item("Count_Name").ToString), "S") = 0, "s", "") & " - " & eds(0)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1351" Then
                txt_sizingparticulars1.Text = Dt1.Rows(0).Item("Count_Name").ToString & IIf(InStr(1, UCase(Dt1.Rows(0).Item("Count_Name").ToString), "S") = 0, "s", "") & " - " & eds(0) & " ENDS SIZING CHARGES"
            Else
                txt_sizingparticulars1.Text = Dt1.Rows(0).Item("Count_Name").ToString & IIf(InStr(1, UCase(Dt1.Rows(0).Item("Count_Name").ToString), "S") = 0, "s", "") & " - " & eds(0) & " SIZING CHARGES"
            End If

            lbl_SizingQty1.Text = "0.000"
            If UBound(wwg) >= 0 Then lbl_SizingQty1.Text = Format(Val(wwg(0)), "########0.000")

            If UBound(eds) >= 1 Then
                txt_sizingparticulars2.Text = Dt1.Rows(0).Item("Count_Name").ToString & IIf(InStr(1, UCase(Dt1.Rows(0).Item("Count_Name").ToString), "S") = 0, "s", "") & " - " & eds(1) & " SIZING CHARGES"
                lbl_SizingQty2.Text = "0.000"
                If UBound(wwg) >= 1 Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- Meenashi Sizing (Somanur)
                        lbl_SizingQty1.Text = Format(Val(lbl_SizingQty1.Text) + Val(wwg(1)), "########0.000")
                    Else
                        lbl_SizingQty2.Text = Format(Val(wwg(1)), "########0.000")
                    End If
                End If

            End If

            If UBound(eds) >= 2 Then
                txt_sizingparticulars3.Text = Dt1.Rows(0).Item("Count_Name").ToString & IIf(InStr(1, UCase(Dt1.Rows(0).Item("Count_Name").ToString), "S") = 0, "s", "") & " - " & eds(2) & " SIZING CHARGES"
                lbl_SizingQty3.Text = "0.000"
                If UBound(wwg) >= 2 Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- Meenashi Sizing (Somanur) and BRT SIZING
                        lbl_SizingQty1.Text = Format(Val(lbl_SizingQty1.Text) + Val(wwg(2)), "########0.000")
                    Else
                        lbl_SizingQty3.Text = Format(Val(wwg(2)), "########0.000")
                    End If
                End If

            End If

            txt_PackingBeam.Text = Val(Dt1.Rows(0).Item("Total_Pavu_Beam").ToString)

            TotWgt = 0
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then

                TotWgt = Format(Val(Dt1.Rows(0).Item("Total_Baby_Net_Weight").ToString), "########0.000")
                If TotWgt < 100 Then
                    TotWgt = TotWgt - 2
                ElseIf TotWgt = 0 Then
                    TotWgt = 0
                Else
                    TotWgt = TotWgt - 3
                End If

                txt_RewindingQuantity.Text = Format(Val(TotWgt), "########0.000")

            Else

                txt_RewindingQuantity.Text = Format(Val(Dt1.Rows(0).Item("Total_Baby_Net_Weight").ToString), "########0.000")

            End If

            End_Id = Val(Dt1.Rows(0).Item("ends_name").ToString)
            Cnt_Id = Val(Dt1.Rows(0).Item("Count_Idno").ToString)

            If Trim(UCase(Cbo_Tax_Type.Text)) = "GST" Then

                lbl_CGSTPerc.Text = ""
                lbl_SGSTPerc.Text = ""
                lbl_IGSTAmount.Text = ""

                Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)
                vInterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_company.Tag), Led_Id)

                If vInterStateStatus = True Then

                    lbl_IGSTPerc.Text = 5
                    'lbl_IGSTPerc.Text = Format(Val(Dt1.Rows(0)("Count_Gst_Perc").ToString), "###########0.00")

                Else
                    lbl_CGSTPerc.Text = 2.5
                    lbl_SGSTPerc.Text = 2.5
                    'lbl_CGSTPerc.Text = Format(Val(Dt1.Rows(0)("Count_Gst_Perc").ToString / 2), "###########0.00")
                    'lbl_SGSTPerc.Text = Format(Val(Dt1.Rows(0)("Count_Gst_Perc").ToString / 2), "###########0.00")

                End If

            Else

                lbl_CGSTPerc.Text = ""
                lbl_SGSTPerc.Text = ""
                lbl_IGSTAmount.Text = ""

            End If

            Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, (cbo_partyname.Text))
            Da = New SqlClient.SqlDataAdapter("select a.* from Ledger_Rate_Details a where " & Val(End_Id) & " between a.Ends_From and a.Ends_To and a.Ledger_IdNo=" & Val(Led_Id) & " and a.Count_Idno = " & Val(Cnt_Id) & "    ", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                txt_SizingRate1.Text = Format(Val(Dt2.Rows(0).Item("Rate").ToString), "########0.00")
            End If
            Dt2.Clear()

            get_RateDetails()

            Call NetAmount_Calculation()

        End If
        Dt1.Dispose()

        Da1.Dispose()

    End Sub

    Private Sub Design_Details_Grid(ByVal NoofSet As Integer)

        If Val(TableLayoutPanel1.Tag) = NoofSet Then Exit Sub

        TableLayoutPanel1.Tag = NoofSet

        With TableLayoutPanel1

            .Visible = False

            If NoofSet >= 3 Then
                .RowStyles(0).Height = 23
                txt_sizingparticulars1.Visible = True
                lbl_SizingQty1.Visible = True
                txt_SizingRate1.Visible = True
                lbl_SizingAmount1.Visible = True

                .RowStyles(1).Height = 23
                txt_sizingparticulars2.Visible = True
                lbl_SizingQty2.Visible = True
                txt_SizingRate2.Visible = True
                lbl_SizingAmount2.Visible = True

                .RowStyles(2).Height = 23
                txt_sizingparticulars3.Visible = True
                lbl_SizingQty3.Visible = True
                txt_SizingRate3.Visible = True
                lbl_SizingAmount3.Visible = True

            ElseIf NoofSet = 2 Then
                .RowStyles(0).Height = 23
                txt_sizingparticulars1.Visible = True
                lbl_SizingQty1.Visible = True
                txt_SizingRate1.Visible = True
                lbl_SizingAmount1.Visible = True

                .RowStyles(1).Height = 23
                txt_sizingparticulars2.Visible = True
                lbl_SizingQty2.Visible = True
                txt_SizingRate2.Visible = True
                lbl_SizingAmount2.Visible = True

                .RowStyles(2).Height = 0
                txt_sizingparticulars3.Visible = False
                lbl_SizingQty3.Visible = False
                txt_SizingRate3.Visible = False
                lbl_SizingAmount3.Visible = False

            Else
                .RowStyles(0).Height = 23
                txt_sizingparticulars1.Visible = True
                lbl_SizingQty1.Visible = True
                txt_SizingRate1.Visible = True
                lbl_SizingAmount1.Visible = True

                .RowStyles(1).Height = 0
                txt_sizingparticulars2.Visible = False
                lbl_SizingQty2.Visible = False
                txt_SizingRate2.Visible = False
                lbl_SizingAmount2.Visible = False

                .RowStyles(2).Height = 0
                txt_sizingparticulars3.Visible = False
                lbl_SizingQty3.Visible = False
                txt_SizingRate3.Visible = False
                lbl_SizingAmount3.Visible = False

            End If

            '.RowStyles(3).Height = 0
            'txt_vat1particular.Visible = False
            'lbl_VatGross1.Visible = False
            'lbl_VatAmount1.Visible = False
            'FlowLayoutPanel1.Visible = False
            'FlowLayoutPanel2.Visible = False

            '.RowStyles(4).Height = 0
            'txt_vat2particular.Visible = False
            'lbl_VatGross2.Visible = False
            'lbl_VatAmount2.Visible = False
            'FlowLayoutPanel3.Visible = False
            'FlowLayoutPanel4.Visible = False

            .Visible = True

        End With

    End Sub

    Private Sub txt_SizingRate1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SizingRate1.LostFocus
        txt_SizingRate1.Text = Format(Val(txt_SizingRate1.Text), "##########0.00")
    End Sub

    Private Sub txt_SizingRate1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_SizingRate1.TextChanged
        lbl_SizingAmount1.Text = Format(Val(lbl_SizingQty1.Text) * Val(txt_SizingRate1.Text), "##########0.00")
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)
            lbl_SizingAmount1.Text = Format(Val(lbl_SizingAmount1.Text), "###########0")
            lbl_SizingAmount1.Text = Format(Val(lbl_SizingAmount1.Text), "###########0.00")
        End If
    End Sub

    Private Sub txt_SizingRate2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SizingRate2.LostFocus
        txt_SizingRate2.Text = Format(Val(txt_SizingRate2.Text), "##########0.00")
    End Sub

    Private Sub txt_SizingRate2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_SizingRate2.TextChanged
        lbl_SizingAmount2.Text = Format(Val(lbl_SizingQty2.Text) * Val(txt_SizingRate2.Text), "##########0.00")
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)
            lbl_SizingAmount2.Text = Format(Val(lbl_SizingAmount2.Text), "###########0")
            lbl_SizingAmount2.Text = Format(Val(lbl_SizingAmount2.Text), "###########0.00")
        End If
    End Sub

    Private Sub txt_SizingRate3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SizingRate3.LostFocus
        txt_SizingRate3.Text = Format(Val(txt_SizingRate3.Text), "##########0.00")
    End Sub

    Private Sub txt_SizingRate3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_SizingRate3.TextChanged
        lbl_SizingAmount3.Text = Format(Val(lbl_SizingQty3.Text) * Val(txt_SizingRate3.Text), "##########0.00")
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)
            lbl_SizingAmount3.Text = Format(Val(lbl_SizingAmount3.Text), "###########0")
            lbl_SizingAmount3.Text = Format(Val(lbl_SizingAmount3.Text), "###########0.00")
        End If
    End Sub

    Private Sub lbl_SizingQty1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SizingQty1.TextChanged
        txt_SizingRate1_TextChanged(sender, e)
    End Sub

    Private Sub lbl_SizingQty2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SizingQty2.TextChanged
        txt_SizingRate2_TextChanged(sender, e)
    End Sub

    Private Sub lbl_SizingQty3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SizingQty3.TextChanged
        txt_SizingRate3_TextChanged(sender, e)
    End Sub

    Private Sub lbl_SizingAmount1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SizingAmount1.TextChanged

        NetAmount_Calculation()
    End Sub

    Private Sub lbl_SizingAmount2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SizingAmount2.TextChanged

        NetAmount_Calculation()
    End Sub

    Private Sub lbl_SizingAmount3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SizingAmount3.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_VatAmount1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_VatAmount2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SampleSetAmount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SampleSetAmount.LostFocus
        txt_SampleSetAmount.Text = Format(Val(txt_SampleSetAmount.Text), "##########0.00")
    End Sub

    Private Sub txt_SampleSetAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_SampleSetAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_VanRentAmount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_VanRentAmount.LostFocus
        txt_VanRentAmount.Text = Format(Val(txt_VanRentAmount.Text), "##########0.00")
    End Sub

    Private Sub txt_VanRentAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_VanRentAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_PackingAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_PackingAmount.TextChanged
        'NetAmount_Calculation()
    End Sub

    Private Sub lbl_RewindingAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_RewindingAmount.TextChanged
        'NetAmount_Calculation()
    End Sub

    Private Sub lbl_WeldingAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_WeldingAmount.TextChanged
        'NetAmount_Calculation()
    End Sub

    Private Sub txt_OtherChargesAmount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_OtherChargesAmount.LostFocus
        txt_OtherChargesAmount.Text = Format(Val(txt_OtherChargesAmount.Text), "##########0.00")
    End Sub

    Private Sub txt_OtherChargesAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_OtherChargesAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_DiscountAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_DiscountAmount.TextChanged
        'NetAmount_Calculation()
    End Sub

    Private Sub cbo_DiscountType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DiscountType.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_WeldingRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WeldingRate.LostFocus
        txt_WeldingRate.Text = Format(Val(txt_WeldingRate.Text), "##########0.00")
    End Sub

    Private Sub txt_WeldingRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_WeldingRate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub Get_State_Code1111111(ByVal Ledger_IDno As Integer, ByRef Ledger_State_Code As String, ByRef Company_State_Code As String)
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

            da = New SqlClient.SqlDataAdapter("Select * from Company_Head a LEFT OUTER JOIN State_Head b ON a.Company_State_IdNo = b.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_company.Tag)), con)
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

    Private Sub txt_WeldingBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeldingBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_WeldingBeam_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_WeldingBeam.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_RewindingRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RewindingRate.LostFocus
        txt_RewindingRate.Text = Format(Val(txt_RewindingRate.Text), "##########0.00")
    End Sub

    Private Sub txt_RewindingRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_RewindingRate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_RewindingQuantity_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RewindingQuantity.LostFocus
        txt_RewindingQuantity.Text = Format(Val(txt_RewindingQuantity.Text), "##########0.000")
    End Sub

    Private Sub txt_RewindingQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_RewindingQuantity.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_PackingRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_PackingRate.LostFocus
        txt_PackingRate.Text = Format(Val(txt_PackingRate.Text), "##########0.00")
    End Sub

    Private Sub txt_PackingRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_PackingRate.TextChanged
        NetAmount_Calculation()
    End Sub


    Private Sub txt_PackingBeam_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_PackingBeam.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_PackingBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PackingBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_VatGrossPerc1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then lbl_CGSTPerc.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            If txt_SizingRate3.Visible And txt_SizingRate3.Enabled Then
                txt_SizingRate3.Focus()
            ElseIf txt_SizingRate2.Visible And txt_SizingRate2.Enabled Then
                txt_SizingRate2.Focus()
            Else
                txt_SizingRate1.Focus()
            End If
            'SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub cbo_DiscountType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DiscountType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DiscountType, txt_DiscountRate, "", "", "", "")
    End Sub

    Private Sub cbo_DiscountType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DiscountType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DiscountType, txt_OtherChargesAmount, txt_DiscountRate, "", "", "", "")
    End Sub

    Private Sub txt_VatPerc1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        lbl_CGSTPerc.Text = Format(Val(lbl_CGSTPerc.Text), "##########0.00")
    End Sub

    Private Sub txt_VatPerc2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        lbl_SGSTPerc.Text = Format(Val(lbl_SGSTPerc.Text), "##########0.00")
    End Sub

    Private Sub txt_DiscountRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DiscountRate.LostFocus
        txt_DiscountRate.Text = Format(Val(txt_DiscountRate.Text), "##########0.000")
    End Sub

    Private Sub txt_DiscountRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscountRate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_DiscountRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscountRate.KeyDown
        'If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then txt_OtherChargesAmount.Focus() 'SendKeys.Send("+{TAB}")
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        Print_PDF_Status = False
    End Sub

    Private Sub txt_DiscountRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscountRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '    save_record()
            'End If
            txt_Tds.Focus()

        End If
    End Sub

    Private Sub txt_Tds_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_Tds.KeyDown
        If e.KeyCode = 38 Then txt_OtherChargesAmount.Focus()
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If


    End Sub
    Private Sub txt_Tds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Tds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub
    Private Sub txt_Tds_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Tds.TextChanged
        NetAmount_Calculation()
    End Sub


    Private Sub NetAmount_Calculation()
        Dim GrsAmt As Single
        Dim Tot As Single
        Dim AssAmt As Single = 0
        Dim GSTGrsPrc As Single = 0
        Dim CGSTAmt As Single = 0
        Dim SGSTAmt As Single = 0
        Dim IGSTAmt As Single = 0
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim Led_IdNo As Integer = 0
        Dim vInterStateStatus As Boolean = False
        Dim tdsamt As Double = 0
        If FrmLdSTS = True Then Exit Sub

        lbl_CGSTAmount.Text = "0.00"
        lbl_SGSTAmount.Text = "0.00"
        lbl_IGSTAmount.Text = "0.00"
        'lbl_CGSTPerc.Text = 0
        'lbl_SGSTPerc.Text = 0
        'lbl_IGSTPerc.Text = 0
        GrsAmt = 0

        GrsAmt = Val(lbl_SizingAmount1.Text) + Val(lbl_SizingAmount2.Text) + Val(lbl_SizingAmount3.Text)

        lbl_PackingAmount.Text = Format(Val(txt_PackingBeam.Text) * Val(txt_PackingRate.Text), "#########0.00")
        lbl_RewindingAmount.Text = Format(Val(txt_RewindingQuantity.Text) * Val(txt_RewindingRate.Text), "#########0.00")
        lbl_WeldingAmount.Text = Format(Val(txt_WeldingBeam.Text) * Val(txt_WeldingRate.Text), "#########0.00")

        If Trim(UCase(cbo_DiscountType.Text)) = "PAISE/KG" Then
            lbl_DiscountAmount.Text = Format((Val(lbl_SizingQty1.Text) + Val(lbl_SizingQty2.Text) + Val(lbl_SizingQty3.Text)) * Val(txt_DiscountRate.Text), "#########0.00")

        Else
            lbl_DiscountAmount.Text = Format((Val(lbl_SizingAmount1.Text) + Val(lbl_SizingAmount2.Text) + Val(lbl_SizingAmount3.Text)) * Val(txt_DiscountRate.Text) / 100, "#########0.00")

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)

            lbl_PackingAmount.Text = Format(Val(lbl_PackingAmount.Text), "###########0")
            lbl_PackingAmount.Text = Format(Val(lbl_PackingAmount.Text), "###########0.00")

            lbl_RewindingAmount.Text = Format(Val(lbl_RewindingAmount.Text), "###########0")
            lbl_RewindingAmount.Text = Format(Val(lbl_RewindingAmount.Text), "###########0.00")

            lbl_WeldingAmount.Text = Format(Val(lbl_WeldingAmount.Text), "###########0")
            lbl_WeldingAmount.Text = Format(Val(lbl_WeldingAmount.Text), "###########0.00")

            lbl_DiscountAmount.Text = Format(Val(lbl_DiscountAmount.Text), "###########0")
            lbl_DiscountAmount.Text = Format(Val(lbl_DiscountAmount.Text), "###########0.00")

        End If

        lbl_Assessable_Value.Text = Format(Val(GrsAmt) + Val(txt_SampleSetAmount.Text) + Val(txt_VanRentAmount.Text) + Val(lbl_PackingAmount.Text) + Val(lbl_RewindingAmount.Text) + Val(lbl_WeldingAmount.Text) + Val(txt_OtherChargesAmount.Text) - Val(lbl_DiscountAmount.Text), "##########0.00")

        tdsamt = Format((Val(lbl_Assessable_Value.Text)) * Val(txt_Tds.Text) / 100, "########0")
        lbl_Tds_Amount.Text = Format(Val(tdsamt), "########0.00")


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
        '    lbl_Assessable_Value.Text = Format(Val(lbl_Assessable_Value.Text), "###########0")
        '    lbl_Assessable_Value.Text = Format(Val(lbl_Assessable_Value.Text), "###########0.00")
        'End If

        '-IGST
        lbl_IGSTAmount.Text = "0.00"
        If Trim(UCase(Cbo_Tax_Type.Text)) <> "NO TAX" Then
            lbl_IGSTAmount.Text = Format(Val(lbl_Assessable_Value.Text) * Val(lbl_IGSTPerc.Text) / 100, "#########0.00")
        End If

        '-CGST 
        lbl_CGSTAmount.Text = "0.00"
        If Trim(UCase(Cbo_Tax_Type.Text)) <> "NO TAX" Then
            lbl_CGSTAmount.Text = Format(Val(lbl_Assessable_Value.Text) * Val(lbl_CGSTPerc.Text) / 100, "#########0.00")
        End If

        '-SGST 
        lbl_SGSTAmount.Text = "0.00"
        If Trim(UCase(Cbo_Tax_Type.Text)) <> "NO TAX" Then
            lbl_SGSTAmount.Text = Format(Val(lbl_Assessable_Value.Text) * Val(lbl_SGSTPerc.Text) / 100, "#########0.00")
        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
        '    lbl_IGSTAmount.Text = Format(Val(lbl_IGSTAmount.Text), "###########0")
        '    lbl_IGSTAmount.Text = Format(Val(lbl_IGSTAmount.Text), "###########0.00")

        '    lbl_CGSTAmount.Text = Format(Val(lbl_CGSTAmount.Text), "###########0")
        '    lbl_CGSTAmount.Text = Format(Val(lbl_CGSTAmount.Text), "###########0.00")

        '    lbl_SGSTAmount.Text = Format(Val(lbl_SGSTAmount.Text), "###########0")
        '    lbl_SGSTAmount.Text = Format(Val(lbl_SGSTAmount.Text), "###########0.00")

        'End If

        Tot = Format(Val(GrsAmt) + Val(lbl_CGSTAmount.Text) + Val(lbl_SGSTAmount.Text) + Val(lbl_IGSTAmount.Text) + Val(txt_SampleSetAmount.Text) + Val(txt_VanRentAmount.Text) + Val(lbl_PackingAmount.Text) + Val(lbl_RewindingAmount.Text) + Val(lbl_WeldingAmount.Text) + Val(txt_OtherChargesAmount.Text) - Val(lbl_DiscountAmount.Text), "###########0")
        lbl_NetAmount.Text = Format(Val(Tot), "#########0.00")
        'lbl_NetAmount.Text = Format(Val(Tot) - Val(lbl_Tds_Amount.Text), "#########0.00")

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_INVOICE, New_Entry) = False Then Exit Sub
        printing_invoice()

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palladam)
        '    pnl_Print.Visible = True
        '    pnl_back.Enabled = False
        '    If btn_Print_Invoice.Enabled And btn_Print_Invoice.Visible Then
        '        btn_Print_Invoice.Focus()
        '    End If
        'Else
        '    printing_invoice()

        'End If

    End Sub

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim Def_PrntrNm As String = ""

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Invoice_Code = '" & Trim(NewCode) & "'", con)
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
            Exit Sub

        End Try

        prn_InpOpts = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1006" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Prakash Sizing (Somanur)
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "12")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")
        End If

        If Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-2" Or Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-4" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then '---- WinTraack Textiles Private Limited(Sizing Unit)
                Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- WinTraack Textiles Private Limited(Sizing Unit)
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next

            Else

                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next

            End If


        Else

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        End If


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then
                    Def_PrntrNm = PrintDocument1.PrinterSettings.PrinterName
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                    PrintDocument1.PrinterSettings.PrinterName = Trim(Def_PrntrNm)

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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                'ppd.ClientSize = New Size(600, 600)

                AddHandler ppd.Shown, AddressOf PrintPreview_Shown
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
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim SetCdSel As String = ""

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'SetCdSel = Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_company.Tag))

        prn_HdDt = New DataTable
        dt2 = New DataTable
        dt3 = New DataTable
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, b.*, c.*,Vh.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, vsh.State_Name as vendor_State_Name, vsh.State_Code as Vendor_State_Code, dph.Ledger_Name as Delivery_Name ,dph.Ledger_Address1 as Delivery_Address1, dph.Ledger_Address2 as Delivery_Address2, dph.Ledger_Address3 as Delivery_Address3, dph.Ledger_Address4 as Delivery_Address4, dph.Ledger_GSTinNo as Delivery_GST_No    from Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_Idno = Csh.State_IdNo INNER JOIN Ledger_Head c ON (case when a.OnAccount_IdNo <> 0 then a.OnAccount_IdNo else a.Ledger_IdNo end) = c.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Vendor_Head VH ON vh.Vendor_IdNo = a.Vendor_IdNo LEFT OUTER JOIN State_Head vsh ON vh.State = vsh.State_IdNo LEFT OUTER JOIN Delivery_Party_Head dph ON Dph.Ledger_IdNo = a.DeliveryTo_IdNo where a.company_idno = " & Str(Val(lbl_company.Tag)) & " and a.Invoice_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)


            da2 = New SqlClient.SqlDataAdapter("Select  Mh.Mill_Name from Specification_Head sh LEFT OUTER JOIN Mill_Head mh ON sh.Mill_IdNo = mh.Mill_IdNo  where sh.SetCode_ForSelection = '" & Trim(cbo_setno.Text) & "'", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            da3 = New SqlClient.SqlDataAdapter("Select sh.*, ch.Count_Name from Specification_Head sh LEFT OUTER JOIN Count_Head ch ON sh.Count_IdNo = ch.Count_IdNo  where sh.SetCode_ForSelection = '" & Trim(cbo_setno.Text) & "'", con)
            dt3 = New DataTable
            da3.Fill(dt3)

            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_EndPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.EndPrint
        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            chk_Printed.Checked = True
            Update_PrintOut_Status()
        End If
    End Sub


    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1106" Then
            Printing_Format2_GST(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Printing_Format6_GST(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1006" Then
            Printing_Format7_GST(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1363" Then
            Printing_Format1363_GST(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            Printing_Format1087_GST(e)
        Else
            Printing_Format1_GST(e)
        End If


    End Sub

    Private Sub Printing_Format1_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font, p2Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0, CurY1 As Single, CurY2 As Single
        Dim TxtHgt As Single = 0, TxtHgtInc As Single = 0, strHeight As Single = 0, strWidth As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String, Cmp_UAMNO As String
        Dim W1 As Single, N1 As Single
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single
        Dim AmtInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim NoofDets As Integer = 0, NoofItems_PerPage As Integer = 0
        Dim V1 As String = ""
        Dim V2 As String = ""
        Dim CenLn As Single
        Dim NetAmt As String = 0, RndOff As String = 0
        Dim Juris As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim S As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BnkDetAr() As String
        Dim BInc As Integer
        Dim LnAr(16) As Single
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim I As Integer = 0


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40  ' 50
            .Right = 50  '50
            .Top = 35
            .Bottom = 50 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            pFont = New Font("Calibri", 11, FontStyle.Regular)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            pFont = New Font("Calibri", 11, FontStyle.Bold)
        Else
            pFont = New Font("Calibri", 10, FontStyle.Regular)
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            TxtHgt = 17.5 '18 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then 'Prakash Sizing 
            TxtHgt = 17.5 '18 ' 19
        Else
            TxtHgt = 18 '18.5 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Avinashi)
            TxtHgtInc = 5.5
            NoofItems_PerPage = 8 '13 ' 15
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            NoofItems_PerPage = 8
        Else
            TxtHgtInc = 0
            NoofItems_PerPage = 10
        End If

        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 80 ' 70
        C2 = 330 ' 350
        C3 = 120 ' 105
        C4 = 90
        C5 = PageWidth - (LMargin + C1 + C2 + C3 + C4)

        CenLn = C1 + C2 + (C3 \ 2)

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
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
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt - 0.1, 1, 0, pFont)
        End If



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_UAMNO = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)

            Cmp_Add1 = prn_HdDt.Rows(0).Item("Sizing_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Sizing_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Sizing_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Sizing_Address4").ToString
            If Trim(prn_HdDt.Rows(0).Item("Sizing_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Sizing_PhoneNo").ToString
            End If

        Else

            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If


        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_panNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
                Cmp_StateNm = Cmp_StateNm & "     " & Cmp_StateCode
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_UAM_No").ToString) <> "" Then
            Cmp_UAMNO = "UDYAM No. : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 110, CurY + 5, 90, 90)

                        End If

                        End Using
                    End If
                End If

            End If
        'End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY + 10, 100, 100)
            'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            '    If InStr(1, Trim(UCase(Cmp_Name)), "SRI BHAGAVAN TEXTILES") > 0 Then 'SRI BHAGAVAN TEXTILES - PALLADAM
            '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.SRI_BHAGAVAN_TEX_LOGO, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
            '    Else
            '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
            '    End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Kalaimagal Sizing (Palladam)
            'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.AADHAVAN, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1284" Then '----- SHREE VEL SIZING (PALLADAM)
            '  e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_VelSizing_Palladam, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1363" Then '----- somanur sizing 
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Balaji_textile_venkatachalapathy, Drawing.Image), LMargin + 10, CurY + 10, 90, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '----- Prakash Tex 
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Prakash_logo, Drawing.Image), LMargin + 10, CurY + 10, 90, 90)
        Else
            If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 15, CurY + 10, 100, 100)

                            End If

                        End Using

                    End If

                End If

            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1031" And Trim(prn_HdDt.Rows(0).Item("Company_Type").ToString) = "ACCOUNT" Then '---- SRI RAM SIZING

            CurY = CurY + TxtHgt - 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
                p1Font = New Font("Calibri", 22, FontStyle.Bold)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then '---- ATHAVAN TEXS SIZING UNIT (SOMANUR)  or  HARI RAM COTTON SIZING UNIT (SOMANUR)
                p1Font = New Font("Brush Script MT", 30, FontStyle.Bold Or FontStyle.Italic)
            Else
                p1Font = New Font("Calibri", 18, FontStyle.Bold)
            End If


            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt
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
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "    " & Cmp_CstNo, CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1031" Then

            CurY = CurY + TxtHgt - 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
                p1Font = New Font("Calibri", 22, FontStyle.Bold)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then '---- ATHAVAN TEXS SIZING UNIT (SOMANUR)  or  HARI RAM COTTON SIZING UNIT (SOMANUR)
                p1Font = New Font("Brush Script MT", 30, FontStyle.Bold Or FontStyle.Italic)
            Else
                p1Font = New Font("Calibri", 18, FontStyle.Bold)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Red)

            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            End If
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight + 1
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            End If


            CurY = CurY + TxtHgt + 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt + 1
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


            CurY = CurY + TxtHgt + 1
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_CstNo), pFont).Width
            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            End If

            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)
            End If


            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            End If

            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "    " & Cmp_CstNo, CurX, CurY, 0, 0, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "    " & Cmp_CstNo, CurX, CurY, 0, 0, pFont)
            End If



            If Trim(Cmp_UAMNO) <> "" Then
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, p1Font)
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
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

                If Trim(ItmNm2) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
                End If


            End If

        End If


        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth, CurY, 1, 0, pFont)
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("PAn_No").ToString) <> "" Then
                Led_TinNo = " PAN NO :  " & Trim(prn_HdDt.Rows(0).Item("PAn_No").ToString)
            End If


            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Billed & Shipped To  : ", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 16, FontStyle.Bold)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Sizing (Somanur)
                Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font, Brushes.Red)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then '---- Ganesh karthik Sizing (Somanur)
                Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
            End If
            If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
                'Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
                Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
            Else
                'Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
                Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
                Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
                Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
                Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Led_GstNo = "GSTIN :  " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
                End If
                If Trim(prn_HdDt.Rows(0).Item("pan_no").ToString) <> "" Then
                    Led_TinNo = " PAN NO :  " & Trim(prn_HdDt.Rows(0).Item("PAn_No").ToString)
                End If
            End If
        End If

        'Common_Procedures.Print_To_PrintDocument(e, "JOB WORK BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        ''Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        W1 = e.Graphics.MeasureString("INVOICE NO : ", pFont).Width
        N1 = e.Graphics.MeasureString("To    : ", pFont).Width

        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & Led_Name, LMargin + N1 + 10, CurY - TxtHgt, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 1
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
        '    Inv_No = prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString
        '    InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & Trim(Format(Val(Inv_No), "######0000")) & Trim(InvSubNo) & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)

        'Else
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)

        ''End If

        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
        '    Common_Procedures.Print_To_PrintDocument(e, "GST-" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
        '    Common_Procedures.Print_To_PrintDocument(e, "SIZING/" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'End If



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString <> "" Then
            strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, pFont).Width
            Common_Procedures.Print_To_PrintDocument(e, "CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + N1 + 10 + strWidth + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Led_GstNo & "    " & Led_TinNo, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, LMargin + CenLn, LnAr(2))
        LnAr(4) = CurY
        LnAr(5) = CurY


        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "No.of ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Particulars", LMargin + C1, CurY, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Quantity ", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Rate Per", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "Beams ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " ", LMargin, CurY + C1, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "in Kgs", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Kgs", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(Rs)", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        NoofDets = 0
        If Common_Procedures.settings.CustomerCode <> "1378" Then
            CurY = CurY + TxtHgt - 8
            p2Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821", LMargin + C1 + 10, CurY, 2, C2, p2Font)
            CurY = CurY + TxtHgt - 3
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Warping & Sizing) )", LMargin + C1 + 10, CurY, 2, C2, p1Font)

        End If


        NoofDets = NoofDets + 1

        CurY = CurY + TxtHgtInc + 2

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
            If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

        Else
            If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                If Common_Procedures.settings.CustomerCode = "1378" Then
                    p2Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, p2Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If
            If (prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate2").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If
            If (prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text3").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate3").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1

            End If

        End If



        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("welding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Welding_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Discount_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc

            If Trim(UCase(prn_HdDt.Rows(0).Item("Discount_Type").ToString)) = "PERCENTAGE" Then
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %"
                V2 = ""

            Else

                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString)
                If Val(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) = Val(Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString) Then
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString
                Else
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.000").ToString
                End If

            End If

            Common_Procedures.Print_To_PrintDocument(e, V1, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, V2, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1

        End If

        NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "##########0.00")
        'NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Tds_Perc_calc").ToString), "##########0.00")

        RndOff = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt), "##########0.00")
        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
            '    CurY = CurY + TxtHgt + 10
            '    If Val(RndOff) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '        NoofDets = NoofDets + 1
            '    End If
            'End If
            CurY = CurY + TxtHgt + TxtHgtInc + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt + TxtHgtInc - 10
            p2Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE VALUE  ", LMargin + C1 + C2 - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  ".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  ", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "IGST  " & prn_HdDt.Rows(0).Item("IGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "IGST            %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        'If Val(prn_HdDt.Rows(0).Item("Tds_perc_Calc").ToString) <> 0 Then
        '    CurY = CurY + TxtHgt + TxtHgtInc
        '    Common_Procedures.Print_To_PrintDocument(e, "TDS    " & (prn_HdDt.Rows(0).Item("Tds_perc").ToString) & "%", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tds_perc_Calc").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        '    NoofDets = NoofDets + 1
        'End If

        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt + TxtHgtInc
        Next

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        'End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        CurY = CurY + TxtHgt - 10
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p2Font)


        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)


        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)

        strHeight = e.Graphics.MeasureString("A", p2Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2, CurY, LMargin + C1 + C2, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3, CurY, LMargin + C1 + C2 + C3, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, LMargin + C1 + C2 + C3 + C4, LnAr(5))

        AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & AmtInWrds, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY


        If Common_Procedures.settings.CustomerCode <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1374" Then

            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), "&")

                BInc = -1

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm2 = Trim(BnkDetAr(BInc))
                End If


                NoofItems_PerPage = NoofItems_PerPage + 1

            End If

            'If NoofDets <= 8 Then
            '    For I = NoofDets + 1 To 8
            '        CurY = CurY + TxtHgt + 10
            '        NoofDets = NoofDets + 1
            '    Next
            'End If
            CurY = CurY + 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(BankNm1) <> "" Then
                CurY = CurY + TxtHgt + 5
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm2) <> "" Then
                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If
            'If Common_Procedures.settings.CustomerCode <> "1102" Then
            '    CurY = CurY + 5
            '    If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            '    End If
            'End If
        End If
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY
        '=============GST SUMMARY============
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1036" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1112" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then '---- Kalaimagal Sizing (Avinashi)
            Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
        End If
        '=========================

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(15) = CurY

        CurY1 = CurY
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
        Else
            p1Font = New Font("Calibri", 10, FontStyle.Underline)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY1, 0, 0, p1Font)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1348" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1374" Then '---- Ganesh Karthi Sizing 
            CurY1 = CurY1 + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "Kindly send as your payment at the earliest by means of a draft.", LMargin + 40, CurY1, 0, 0, pFont)
        End If
        CurY1 = CurY1 + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 22% will be charged from the date of invoice.", LMargin + 40, CurY1, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then            '---- Ganesh Karthi Sizing 
            Common_Procedures.Print_To_PrintDocument(e, "1. Appropriate rate of interest @ 24% will be charged", LMargin + 30, CurY1, 0, 0, pFont)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "for overdue invoice more than 30 days.", LMargin + 30, CurY1, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged for overdue invoice more than 30 days.", LMargin + 40, CurY1, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY1, 0, 0, pFont)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1348" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1374" Then '---- Ganesh Karthi Sizing 
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY1, 0, 0, pFont)

        End If
        Juris = Common_Procedures.settings.Jurisdiction
        If Trim(Juris) = "" Then Juris = "COIMBATORE"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then
            Juris = "COIMBATORE"
        End If


        CurY1 = CurY1 + TxtHgt
        If Common_Procedures.settings.CustomerCode = "1102" Or Common_Procedures.settings.CustomerCode = "1348" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            Common_Procedures.Print_To_PrintDocument(e, "2. subject to " & Juris & " jurisdiction only.", LMargin + 30, CurY1, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "subject to " & Juris & " jurisdiction only.", LMargin + 40, CurY1, 0, 0, pFont)
        End If

        'If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
        'End If

        If Common_Procedures.settings.CustomerCode = "1102" Or Common_Procedures.settings.CustomerCode = "1348" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            Erase BnkDetAr
            BankNm1 = "" : BankNm2 = "" : BankNm3 = "" : BankNm4 = ""
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

                NoofItems_PerPage = NoofItems_PerPage + 1

            End If

            CurY2 = CurY

            p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 390, CurY2, 0, 0, p1Font)

            If Trim(BankNm1) <> "" Then
                CurY2 = CurY2 + TxtHgt
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm2) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm3) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm4) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

        End If

        If CurY1 > CurY2 Then
            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)
            LnAr(11) = CurY1
        ElseIf CurY2 > CurY1 Then
            CurY2 = CurY2 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY2, PageWidth, CurY2)
            LnAr(11) = CurY2
        Else
            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)
            LnAr(11) = CurY1
        End If

        If CurY1 > CurY2 Then
            CurY = CurY1
        Else
            CurY = CurY2
        End If



        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If

        CurY = CurY + 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then '---- ATHAVAN TEXS SIZING UNIT (SOMANUR)  or  HARI RAM COTTON SIZING UNIT (SOMANUR)
            p1Font = New Font("Brush Script MT", 14, FontStyle.Bold Or FontStyle.Italic)
        ElseIf Common_Procedures.settings.CustomerCode = "1378" Then
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Else

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        End If


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
            e.Graphics.DrawLine(Pens.Black, LMargin + 190, CurY, LMargin + 190, LnAr(11))
        End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
        '    e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
        'End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1074" Or Trim(Common_Procedures.settings.CustomerCode) = "1031" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then '-- MITHUN SIZING MILLS,SRI RAM SIZING
            e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
        Else
            'COMMENTED
            If Common_Procedures.settings.CustomerCode = "1351" Then ' MAHAA GHANPATHY SIZING MILL
                e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
                e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
                'Else
                '    e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(15))
            End If
        End If
        e.HasMorePages = False

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count < Len(Trim(prn_InpOpts)) Then


                If Val(prn_InpOpts) <> "0" Then
                    prn_PageNo = 0

                    e.HasMorePages = True
                    Return
                End If

            End If

        End If

    End Sub

    Private Sub Printing_Format2_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font, p2Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0
        Dim TxtHgt As Single = 0, TxtHgtInc As Single = 0, strHeight As Single = 0, strWidth As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim LnAr(15) As Single
        Dim W1 As Single, N1 As Single
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single
        Dim AmtInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim NoofDets As Integer = 0, NoofItems_PerPage As Integer = 0
        Dim V1 As String = ""
        Dim V2 As String = ""
        Dim CenLn As Single
        Dim NetAmt As String = 0, RndOff As String = 0
        Dim Juris As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40  ' 50
            .Right = 50  '50
            .Top = 35
            .Bottom = 50 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            pFont = New Font("Calibri", 11, FontStyle.Regular)
        Else
            pFont = New Font("Calibri", 10, FontStyle.Regular)
        End If
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            TxtHgt = 18 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Else
            TxtHgt = 18.5 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Avinashi)
            TxtHgtInc = 5.5
            NoofItems_PerPage = 8 '13 ' 15
        Else
            TxtHgtInc = 0
            NoofItems_PerPage = 10
        End If

        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 80 ' 70
        C2 = 330 ' 350
        C3 = 120 ' 105
        C4 = 90
        C5 = PageWidth - (LMargin + C1 + C2 + C3 + C4)

        CenLn = C1 + C2 + (C3 \ 2)

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
                Cmp_StateNm = Cmp_StateNm & "     " & Cmp_StateCode
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        Dim br = New SolidBrush(Color.FromArgb(191, 43, 133))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY + 10, 100, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Red)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Green)
        End If
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, br)
        End If


        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, br)
        End If


        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & Cmp_CstNo), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, br)
        End If

        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, br)
        End If


        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, br)
        End If

        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & Cmp_CstNo, CurX, CurY, 0, 0, pFont, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & Cmp_CstNo, CurX, CurY, 0, 0, pFont, br)
        End If


        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, br)
        End If



        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "Billed & Shipped To  : ", LMargin + 10, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Sizing (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        W1 = e.Graphics.MeasureString("INVOICE NO : ", pFont).Width
        N1 = e.Graphics.MeasureString("To    : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + N1 + 10, CurY - TxtHgt, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            Common_Procedures.Print_To_PrintDocument(e, "GST-" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, "SIZING/" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString <> "" Then
            strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, pFont).Width
            Common_Procedures.Print_To_PrintDocument(e, "CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + N1 + 10 + strWidth + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, LMargin + CenLn, LnAr(2))
        LnAr(4) = CurY
        LnAr(5) = CurY


        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "No.of ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Particulars", LMargin + C1, CurY, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Quantity ", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Rate Per", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "Beams ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " ", LMargin, CurY + C1, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "in Kgs", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Kgs", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(Rs)", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        NoofDets = 0

        CurY = CurY + TxtHgt - 8
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821", LMargin + C1 + 10, CurY, 2, C2, p2Font)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Warping & Sizing) )", LMargin + C1 + 10, CurY, 2, C2, p1Font)

        NoofDets = NoofDets + 1

        CurY = CurY + TxtHgtInc + 1

        If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If (prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate2").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If (prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text3").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate3").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1

        End If

        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("welding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Welding_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Discount_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc

            If Trim(UCase(prn_HdDt.Rows(0).Item("Discount_Type").ToString)) = "PERCENTAGE" Then
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %"
                V2 = ""

            Else

                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString)
                If Val(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) = Val(Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString) Then
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString
                Else
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.000").ToString
                End If

            End If

            Common_Procedures.Print_To_PrintDocument(e, V1, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, V2, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1

        End If

        NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Tds_Perc_Calc").ToString), "##########0.00")

        RndOff = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt), "##########0.00")
        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
            '    CurY = CurY + TxtHgt + 10
            '    If Val(RndOff) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '        NoofDets = NoofDets + 1
            '    End If
            'End If
            CurY = CurY + TxtHgt + TxtHgtInc + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt + TxtHgtInc - 10
            p2Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE VALUE  ", LMargin + C1 + C2 - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  ".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  ", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "IGST  " & prn_HdDt.Rows(0).Item("IGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "IGST            %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If Val(prn_HdDt.Rows(0).Item("Tds_perc_Calc").ToString) <> 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "TDS    " & (prn_HdDt.Rows(0).Item("Tds_perc").ToString) & "%", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tds_perc_Calc").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt + TxtHgtInc
        Next

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        CurY = CurY + TxtHgt - 10
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p2Font)
        strHeight = e.Graphics.MeasureString("A", p2Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2, CurY, LMargin + C1 + C2, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3, CurY, LMargin + C1 + C2 + C3, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, LMargin + C1 + C2 + C3 + C4, LnAr(5))

        AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & AmtInWrds, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY

        CurY = CurY + 5
        If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
        End If
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY
        '=============GST SUMMARY============
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1036" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1112" Then '---- Kalaimagal Sizing (Avinashi)
            Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
        End If
        '=========================

        CurY = CurY
        p1Font = New Font("Calibri", 10, FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY, 0, 0, p1Font)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then '---- Ganesh Karthi Sizing 
            CurY = CurY + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "Kindly send as your payment at the earliest by means of a draft.", LMargin + 40, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 22% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh Karthi Sizing 
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged for overdue invoice more than 30 days.", LMargin + 40, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then '---- Ganesh Karthi Sizing 
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY, 0, 0, pFont)

        End If
        Juris = Common_Procedures.settings.Jurisdiction
        If Trim(Juris) = "" Then Juris = "TIRUPUR"

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "subject to " & Juris & " jurisdiction only.", LMargin + 40, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, Brushes.Green)
        End If


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont, br)

        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))


        e.Graphics.DrawLine(Pens.Black, LMargin + 190, CurY, LMargin + 190, LnAr(11))
        'e.Graphics.DrawLine(Pens.Black, LMargin + 200, CurY, LMargin + 200, LnAr(11))

        e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
        'e.Graphics.DrawLine(Pens.Black, LMargin + 410, CurY, LMargin + 410, LnAr(11))

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font, p2Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim LnAr(15) As Single
        Dim W1 As Single, N1 As Single
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single
        Dim AmtInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim NoofDets As Integer = 0, NoofItems_PerPage As Integer = 0
        Dim V1 As String = ""
        Dim V2 As String = ""
        Dim CenLn As Single
        Dim NetAmt As Single = 0, RndOff As Single = 0
        Dim Juris As String
        Dim CmpNmAddSTS As Boolean = False
        Dim Cmp_Email As String = ""
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim S As String

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then '---- WinTraack Textiles Private Limited(Sizing Unit)
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        Else
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
            .Left = 20  ' 50 
            .Right = 55
            .Top = 35   '30
            .Bottom = 35 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 12, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 19.5 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then
            NoofItems_PerPage = 13 ' 14 ' 15
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1074" Then
            NoofItems_PerPage = 12 ' 14 ' 15

        Else
            NoofItems_PerPage = 13 ' 14 ' 15

        End If

        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 70  ' 80 ' 70
        C2 = 340 ' 375 ' 350
        C3 = 105 ' 120 ' 105
        C4 = 95  ' 110 '95
        C5 = PageWidth - (LMargin + C1 + C2 + C3 + C4)

        CenLn = C1 + C2 + (C3 \ 2)

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
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
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt - 0.1, 1, 0, pFont)
        End If

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_Email = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_PanNo = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Else

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
                Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            End If

        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "EMail : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        CmpNmAddSTS = False

        If Trim(UCase(prn_HdDt.Rows(0).Item("Company_Type").ToString)) <> "UNACCOUNT" Then

            If Trim(Cmp_Name) <> "" And Microsoft.VisualBasic.Len(Trim(Cmp_Name)) > 1 Then
                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 16, FontStyle.Bold)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Red)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
                End If

                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
                CmpNmAddSTS = True
            End If

            If Trim(Cmp_Add1) <> "" Then
                CurY = CurY + strHeight - 5
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
                    p1Font = New Font("Calibri", 14, FontStyle.Bold)

                Else
                    If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                        p1Font = New Font("Calibri", 14, FontStyle.Bold)

                    Else
                        p1Font = New Font("Calibri", 12, FontStyle.Regular)

                    End If

                End If
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Green)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
                End If

                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
                CmpNmAddSTS = True

            End If

            If Trim(Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt + 5
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Green)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
                End If

                CmpNmAddSTS = True
            End If

            If Trim(Cmp_PhNo) <> "" Then
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Green)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
                End If

                CmpNmAddSTS = True
            End If
            If Trim(Cmp_Email) <> "" Then
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Green)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, LMargin, CurY, 2, PrintWidth, p1Font)
                End If

                CmpNmAddSTS = True
            End If

            If Trim(Cmp_TinNo) <> "" Or Trim(Cmp_CstNo) <> "" Or Trim(Cmp_PanNo) <> "" Then
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font, Brushes.Green)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Green)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth, CurY, 1, 0, p1Font, Brushes.Green)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth, CurY, 1, 0, p1Font)

                End If
                CmpNmAddSTS = True
            End If

            If CmpNmAddSTS = True Then
                CurY = CurY + strHeight
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If
        End If

        LnAr(2) = CurY

        If CmpNmAddSTS = False Then
            NoofItems_PerPage = NoofItems_PerPage + 4
        End If

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "To  : ", LMargin + 10, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- GKT
            Common_Procedures.Print_To_PrintDocument(e, "SIZING BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        End If
        'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        W1 = e.Graphics.MeasureString("BILL NO : ", pFont).Width
        N1 = e.Graphics.MeasureString("To    : ", pFont).Width

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + N1 + 10, CurY - TxtHgt, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BILL NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If prn_HdDt.Rows(0).Item("pan_no").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PAN NO : " & prn_HdDt.Rows(0).Item("pan_no").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, LMargin + CenLn, LnAr(2))
        LnAr(4) = CurY
        LnAr(5) = CurY


        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "No.of ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Particulars", LMargin + C1, CurY, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Quantity ", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Rate Per", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "Beams ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " ", LMargin, CurY + C1, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "in Kgs", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Kgs", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(Rs)", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        NoofDets = 0

        If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If (prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate2").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If (prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text3").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate3").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("vat_Amount1").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Text1").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If


        If (prn_HdDt.Rows(0).Item("welding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Welding_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Discount_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10

            If Trim(UCase(prn_HdDt.Rows(0).Item("Discount_Type").ToString)) = "PERCENTAGE" Then
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %"
                V2 = ""
            Else
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString)
                If Val(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) = Val(Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString) Then
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString
                Else
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.000").ToString
                End If

            End If

            Common_Procedures.Print_To_PrintDocument(e, V1, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, V2, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "IGST  " & prn_HdDt.Rows(0).Item("IGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        Erase BnkDetAr
        BankNm1 = "" : BankNm2 = "" : BankNm3 = "" : BankNm4 = ""
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

            NoofItems_PerPage = NoofItems_PerPage + 1

        End If

        If NoofDets <= 8 Then
            For I = NoofDets + 1 To 8
                CurY = CurY + TxtHgt + 10
                NoofDets = NoofDets + 1
            Next
        End If

        If Trim(BankNm1) <> "" Then
            CurY = CurY + TxtHgt + 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + C1 + 20, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + C1 + 20, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm3) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + C1 + 20, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm4) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + C1 + 20, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt + 10
        Next

        NetAmt = Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString)

        RndOff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        CurY = CurY + TxtHgt - 10
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p2Font)
        strHeight = e.Graphics.MeasureString("A", p2Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2, CurY, LMargin + C1 + C2, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3, CurY, LMargin + C1 + C2 + C3, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, LMargin + C1 + C2 + C3 + C4, LnAr(5))

        AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Rupees  :  " & AmtInWrds, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1028" Then '---- Chinnu Sizing (Palladam)
            Common_Procedures.Print_To_PrintDocument(e, "1. Payment Immediate.", LMargin + 40, CurY, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then '---- WinTraack Textiles Private Limited(Sizing Unit)
            Common_Procedures.Print_To_PrintDocument(e, "1. Kindly send as your payment at the earliest by means of a draft.", LMargin + 40, CurY, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1074" Then '---- Midhun Sizing (Avinasi)

            Common_Procedures.Print_To_PrintDocument(e, "1. Discount Details : Payment between 31 to 45 days Rs.1 per kg will be added to Invoice Amount,", LMargin + 40, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "Payment above 45 days no discounts, so the discount given will be added to ", LMargin + 183, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Amount.", LMargin + 183, CurY, 0, 0, pFont)


        Else
            Common_Procedures.Print_To_PrintDocument(e, "1. Kindly send as your payment with in 30 days.", LMargin + 40, CurY, 0, 0, pFont)
        End If

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY, 0, 0, pFont)


        Juris = Common_Procedures.settings.Jurisdiction
        If Trim(Juris) = "" Then Juris = "TIRUPUR"
        'If Trim(Juris) = "" Then Juris = prn_HdDt.Rows(0).Item("Company_Address3").ToString

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "2. Subject to " & Juris & " jurisdiction only.", LMargin + 40, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        If Trim(UCase(prn_HdDt.Rows(0).Item("Company_Type").ToString)) <> "UNACCOUNT" Then

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, Brushes.Red)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            End If


        Else
            CurY = CurY + 10

        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, p1Font)
        End If
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 290, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + 230, CurY, LMargin + 230, LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + 480, CurY, LMargin + 480, LnAr(10))

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then
            '  e.Graphics.DrawLine(Pens.Black, LMargin + 230, CurY, LMargin + 230, LnAr(10))
            '  e.Graphics.DrawLine(Pens.Black, LMargin + 530, CurY, LMargin + 530, LnAr(10))
        End If

        e.HasMorePages = False

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count < Len(Trim(prn_InpOpts)) Then


                If Val(prn_InpOpts) <> "0" Then
                    prn_PageNo = 0

                    e.HasMorePages = True
                    Return
                End If

            End If

        End If

    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim NoofItems_PerPage As Integer
        Dim AmtInWrds As String = ""
        Dim PrnHeading As String = ""
        Dim I As Integer, NoofDets As Integer
        Dim time As String = ""
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single
        Dim V1 As String = ""
        Dim V2 As String = ""
        Dim CenLn As Single
        Dim NetAmt As Single = 0, RndOff As Single = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'PageSetupDialog1.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40 ' 65
            .Right = 0 ' 50
            .Top = 50 ' 65
            .Bottom = 0 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

        TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        NoofItems_PerPage = 5

        PageWidth = 780

        C1 = 110  ' 80 ' 70
        C2 = 410 ' 375 ' 350
        C3 = 70 ' 120 ' 105
        C4 = 90  ' 110 '95
        C5 = 760 - (LMargin + C1 + C2 + C3 + C4 + 20)

        CenLn = C1 + C2 + (C3 \ 2)

        Try

            'For I = 100 To 1100 Step 300

            '    CurY = I
            '    For J = 1 To 850 Step 40

            '        CurX = J
            '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
            '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

            '        CurX = J + 20
            '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

            '    Next

            'Next

            'For I = 200 To 800 Step 250

            '    CurX = I
            '    For J = 1 To 1200 Step 40

            '        CurY = J
            '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

            '        CurY = J + 20
            '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

            '    Next

            'Next

            'e.HasMorePages = False
            CurX = LMargin + 65 ' 40  '150
            CurY = TMargin + 80 ' 122 ' 100
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX + 20, CurY, 0, 0, pFont)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) <> "" Then

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX + 20, CurY, 0, 0, pFont)

            End If

            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, CurX + 20, CurY, 0, 0, pFont)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, CurX + 20, CurY, 0, 0, pFont)

            End If

            'CurY = CurY + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, CurX + 20, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            If prn_HdDt.Rows(0).Item("pan_no").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PAN NO : " & prn_HdDt.Rows(0).Item("pan_no").ToString, CurX + 20, CurY + 5, 0, 0, pFont)
            End If

            CurX = LMargin + 560
            CurY = TMargin + 100
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_No").ToString, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 560
            CurY = TMargin + 130
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 560
            CurY = TMargin + 220
            If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If
            If (prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate2").ToString, LMargin + C1 + C2 + C3 + C4 - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If
            If (prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text3").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate3").ToString, LMargin + C1 + C2 + C3 + C4 - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If (prn_HdDt.Rows(0).Item("vat_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Text1").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Amount1").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If (prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Amount2").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If (prn_HdDt.Rows(0).Item("welding_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Welding_Beam").ToString), LMargin + 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Amount").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If (prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If (prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If (prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString, PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If (prn_HdDt.Rows(0).Item("Discount_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10

                If Trim(UCase(prn_HdDt.Rows(0).Item("Discount_Type").ToString)) = "PERCENTAGE" Then
                    V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %"
                    V2 = ""
                Else
                    V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString)
                    If Val(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) = Val(Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString) Then
                        V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString
                    Else
                        V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.000").ToString
                    End If

                End If

                Common_Procedures.Print_To_PrintDocument(e, V1, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, V2, LMargin + C1 + C2 + C3 + C4 - 15, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "IGST  " & prn_HdDt.Rows(0).Item("IGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            If NoofDets <= 8 Then
                For I = NoofDets + 1 To 8
                    CurY = CurY + TxtHgt + 10
                    NoofDets = NoofDets + 1
                Next
            End If

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt + 10
            Next

            NetAmt = Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString)

            RndOff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt)
            CurY = CurY + TxtHgt + 10
            If Val(RndOff) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 2, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + 45, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 2, CurY, 1, 0, p1Font)
            'strHeight = e.Graphics.MeasureString("A", p1Font).Height

            AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

            CurX = LMargin + 120
            CurY = TMargin + 600
            Common_Procedures.Print_To_PrintDocument(e, AmtInWrds, CurX, CurY, 0, 0, pFont)

            'CurX = LMargin + 200
            'CurY = TMargin + 450
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) & "    Duplicate for Book No . B1", CurX, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub
    Private Sub btn_EMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EMail.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Led_IdNo As Integer
        Dim MailTxt As String
        Dim vSetCd As String, vSetNo As String


        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)
            If Led_IdNo = 0 Then Exit Sub

            vSetCd = ""
            vSetNo = ""
            da = New SqlClient.SqlDataAdapter("select * from Specification_Head where setcode_forSelection = '" & Trim(cbo_setno.Text) & "'", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                vSetCd = dt1.Rows(0).Item("Set_Code").ToString
                vSetNo = dt1.Rows(0).Item("set_no").ToString
            End If
            dt1.Clear()

            MailTxt = "INVOICE " & vbCrLf & vbCrLf

            MailTxt = MailTxt & "INV.NO:" & Trim(lbl_InvoiceNo.Text) & vbCrLf & "DATE:" & Trim(dtp_date.Text) & vbCrLf & vbCrLf & "SET.NO:" & Trim(vSetNo) & vbCrLf & "AMOUNT:" & Trim(lbl_NetAmount.Text)

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            EMAIL_Entry.vSubJect = "Invoice for SetNo : " & Trim(vSetNo)
            EMAIL_Entry.vMessage = Trim(MailTxt)

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim Led_IdNo As Integer = 0
        Dim vSetCd As String, vSetNo As String

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)
            If Led_IdNo = 0 Then Exit Sub

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            vSetCd = ""
            vSetNo = ""
            da = New SqlClient.SqlDataAdapter("select * from Specification_Head where setcode_forSelection = '" & Trim(cbo_setno.Text) & "'", con)
            dt1 = New DataTable
            da.Fill(dt1)

            If Dt1.Rows.Count > 0 Then
                vSetCd = Dt1.Rows(0).Item("Set_Code").ToString
                vSetNo = Dt1.Rows(0).Item("set_no").ToString
            End If

            dt1.Clear()
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then
                smstxt = "INVOICE " & vbCrLf & vbCrLf
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                smstxt = smstxt & "INV.NO:" & Trim(txt_InvoicePrefixNo.Text) & Trim(lbl_InvoiceNo.Text) & vbCrLf & "DATE:" & Trim(dtp_date.Text) & vbCrLf & "SET.NO:" & Trim(vSetNo) & vbCrLf & "TAXABLE AMOUNT:" & Trim(lbl_Assessable_Value.Text) & vbCrLf & "GST " & (Val(lbl_CGSTAmount.Text) + Val(lbl_SGSTAmount.Text)) & vbCrLf & "TOTAL AMOUNT:" & Trim(lbl_NetAmount.Text)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
                smstxt = smstxt & "INV.NO:" & Trim(lbl_InvoiceNo.Text) & vbCrLf & "DATE:" & Trim(dtp_date.Text) & vbCrLf & "SET.NO:" & Trim(vSetNo) & vbCrLf & "TOTAL AMOUNT:" & Trim(lbl_NetAmount.Text)
            Else
                smstxt = smstxt & "INV.NO:" & Trim(lbl_InvoiceNo.Text) & vbCrLf & "DATE:" & Trim(dtp_date.Text) & vbCrLf & vbCrLf & "SET.NO:" & Trim(vSetNo) & vbCrLf & "AMOUNT:" & Trim(lbl_NetAmount.Text)
            End If



            smstxt = smstxt & vbCrLf & " Thanks! " & vbCrLf
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                smstxt = smstxt & "GKT SIZING "
            Else '
                smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_company.Tag))
            End If

            If Common_Procedures.settings.CustomerCode = "1102" Then
                Sms_Entry.vSmsPhoneNo = Trim(PhNo) & "," & "9361188135"
            Else
                Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            End If

            Sms_Entry.vSmsMessage = Trim(smstxt)

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        prn_Status = 1
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
        prn_Status = 2
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub get_RateDetails()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt2 As New DataTable

        Dim LedID As Integer


        LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)

        If LedID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select a.* from Ledger_Rate_Head a where a.Ledger_IdNo = " & Val(LedID) & " ", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                txt_PackingRate.Text = Format(Val(Dt.Rows(0).Item("Packing_Charge").ToString), "########0.00")
                txt_WeldingRate.Text = Format(Val(Dt.Rows(0).Item("Welding_Charge").ToString), "########0.00")
                txt_RewindingRate.Text = Format(Val(Dt.Rows(0).Item("Rewinding_Charge").ToString), "########0.00")
                txt_DiscountRate.Text = Format(Val(Dt.Rows(0).Item("Discount_Rate").ToString), "########0.00")
                cbo_DiscountType.Text = (Dt.Rows(0).Item("Discount_Type").ToString)

            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            NetAmount_Calculation()

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
        Dim Led_IdNo As Integer, Acc_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Acc_IdNo = 0


            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Invoice_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Invoice_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Invoice_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_OnAccount.Text) <> "" Then
                Acc_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_OnAccount.Text)
            End If

            'If Trim(cbo_Filter_OnAccount.Text) <> "" Then
            '    Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_OnAccount.Text)
            'End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Acc_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.OnAccount_IdNo = " & Str(Val(Acc_IdNo))
            End If

            If Trim(cbo_Filter_SetNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.SetCode_ForSelection = '" & Trim(cbo_Filter_SetNo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Invoice_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_company.Tag)) & " and a.Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Invoice_RefNo", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Invoice_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Invoice_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = (dt2.Rows(i).Item("SetCode_ForSelection").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("OnAccount_IdNo").ToString))
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.000")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub








    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub txt_GrTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GrTime.TextChanged
        GraceTime_Calculation()
    End Sub
    Private Sub txt_GrTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GrTime.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub dtp_GrDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_GrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_GrDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_GrDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_GrDate.TextChanged
        If IsDate(dtp_GrDate.Text) = True Then

            msk_GrDate.Text = dtp_GrDate.Text
            msk_GrDate.SelectionStart = 0
        End If
    End Sub
    Private Sub dtp_GrDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_GrDate.ValueChanged
        msk_GrDate.Text = dtp_GrDate.Text
    End Sub

    Private Sub dtp_GrDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_GrDate.Enter
        msk_GrDate.Focus()
        msk_GrDate.SelectionStart = 0
    End Sub


    Private Sub msk_grDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_GrDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskGrText = ""
        vmskGrStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskGrText = msk_GrDate.Text
            vmskGrStrt = msk_GrDate.SelectionStart
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_SizingRate1.Focus()

        End If

    End Sub

    Private Sub msk_GrDate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_GrDate.KeyPress
        If Asc(e.KeyChar) = 13 Then

            txt_SizingRate1.Focus()

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


    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub cbo_Filter_SetNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_SetNo.GotFocus

        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)

            Condt = ""
            If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
                Condt = " Company_IdNo = " & Str(Val(lbl_company.Tag))
            End If

            If Val(Led_ID) <> 0 Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " ( ( Ledger_IdNo = 0 or Ledger_IdNo = " & Str(Val(Led_ID)) & " ) and (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') )"
            Else
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') "
            End If

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Specification_Head", "setcode_forSelection", "(" & Condt & ")", "(set_code = '')")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Filter_SetNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_SetNo.KeyDown
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)

            Condt = ""
            If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
                Condt = " Company_IdNo = " & Str(Val(lbl_company.Tag))
            End If

            If Val(Led_ID) <> 0 Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " ( ( Ledger_IdNo = 0 or Ledger_IdNo = " & Str(Val(Led_ID)) & " ) and (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') )"
            Else
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') "
            End If

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_SetNo, cbo_Filter_PartyName, cbo_Filter_OnAccount, "Specification_Head", "setcode_forSelection", "(" & Condt & ")", "(set_code = '')")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Filter_SetNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_SetNo.KeyPress
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)

            Condt = ""
            If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
                Condt = " Company_IdNo = " & Str(Val(lbl_company.Tag))
            End If

            If Val(Led_ID) <> 0 Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " ( ( Ledger_IdNo = 0 or Ledger_IdNo = " & Str(Val(Led_ID)) & " ) and (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') )"
            Else
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') "
            End If

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_SetNo, cbo_Filter_OnAccount, "Specification_Head", "setcode_forSelection", "(" & Condt & ")", "(set_code = '')")


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_OnAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_OnAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")
    End Sub
    Private Sub cbo_Filter_OnAccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_OnAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_OnAccount, cbo_Filter_SetNo, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")

    End Sub

    Private Sub cbo_Filter_OnAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_OnAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_OnAccount, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_SetNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_SetNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)  '---- Kalaimagal Sizing (Palladam)
        Dim pFont As Font, p1Font, p2Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Des As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim LnAr(15) As Single
        Dim W1 As Single, N1 As Single
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single
        Dim AmtInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim NoofDets As Integer = 0, NoofItems_PerPage As Integer = 0
        Dim V1 As String = ""
        Dim V2 As String = ""
        Dim CenLn As Single
        Dim NetAmt As Single = 0, RndOff As Single = 0
        Dim Juris As String
        Dim CmpNmAddSTS As Boolean = False
        Dim Cmp_Email As String = ""
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim S As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20  ' 50 
            .Right = 70 ' 55
            .Top = 35   '30
            .Bottom = 35 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 12, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 19.4 ' 19.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 10 ' 12 ' 14 ' 15

        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 70  ' 80 ' 70
        C2 = 340 ' 375 ' 350
        C3 = 105 ' 120 ' 105
        C4 = 95  ' 110 '95
        C5 = PageWidth - (LMargin + C1 + C2 + C3 + C4)

        CenLn = C1 + C2 + (C3 \ 2)

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
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

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_PanNo = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = "" : Cmp_Des = ""

        ' Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Des = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "MOBILE NO : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-MAIL : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)


        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 10, CurY - 5, 120, 100)
        End If

        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Des, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + (TxtHgt \ 2)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + (TxtHgt \ 2)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, Cmp_FaxNo, PageWidth - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY


        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "To  : ", LMargin + 10, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- GKT
            Common_Procedures.Print_To_PrintDocument(e, "SIZING BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        End If
        'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        W1 = e.Graphics.MeasureString("BILL NO : ", pFont).Width
        N1 = e.Graphics.MeasureString("To    : ", pFont).Width

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + N1 + 10, CurY - TxtHgt, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BILL NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If prn_HdDt.Rows(0).Item("pan_no").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PAN NO : " & prn_HdDt.Rows(0).Item("pan_no").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, LMargin + CenLn, LnAr(2))
        LnAr(4) = CurY
        LnAr(5) = CurY


        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "No.of ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Particulars", LMargin + C1, CurY, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Quantity ", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Rate Per", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "Beams ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " ", LMargin, CurY + C1, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "in Kgs", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Kgs", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(Rs)", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        NoofDets = 0

        If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If (prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate2").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If (prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text3").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate3").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("vat_Amount1").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Text1").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vat_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If


        If (prn_HdDt.Rows(0).Item("welding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Welding_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Discount_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10

            If Trim(UCase(prn_HdDt.Rows(0).Item("Discount_Type").ToString)) = "PERCENTAGE" Then
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %"
                V2 = ""
            Else
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString)
                If Val(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) = Val(Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString) Then
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString
                Else
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.000").ToString
                End If

            End If

            Common_Procedures.Print_To_PrintDocument(e, V1, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, V2, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "IGST  " & prn_HdDt.Rows(0).Item("IGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        Erase BnkDetAr
        BankNm1 = "" : BankNm2 = "" : BankNm3 = "" : BankNm4 = ""
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

            NoofItems_PerPage = NoofItems_PerPage + 1

        End If

        If NoofDets <= 8 Then
            For I = NoofDets + 1 To 8
                CurY = CurY + TxtHgt + 10
                NoofDets = NoofDets + 1
            Next
        End If

        If Trim(BankNm1) <> "" Then
            CurY = CurY + TxtHgt + 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + C1 + 20, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + C1 + 20, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm3) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + C1 + 20, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm4) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + C1 + 20, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt + 10
        Next

        NetAmt = Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString)

        RndOff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        CurY = CurY + TxtHgt - 10
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p2Font)
        strHeight = e.Graphics.MeasureString("A", p2Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2, CurY, LMargin + C1 + C2, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3, CurY, LMargin + C1 + C2 + C3, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, LMargin + C1 + C2 + C3 + C4, LnAr(5))

        AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Rupees  :  " & AmtInWrds, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1028" Then '---- Chinnu Sizing (Palladam)
            Common_Procedures.Print_To_PrintDocument(e, "1. Payment Immediate.", LMargin + 40, CurY, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then '---- WinTraack Textiles Private Limited(Sizing Unit)
            Common_Procedures.Print_To_PrintDocument(e, "1. Kindly send as your payment at the earliest by means of a draft.", LMargin + 40, CurY, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1074" Then '---- Midhun Sizing (Avinasi)

            Common_Procedures.Print_To_PrintDocument(e, "1. Discount Details : Payment between 31 to 45 days Rs.1 per kg will be added to Invoice Amount,", LMargin + 40, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "Payment above 45 days no discounts, so the discount given will be added to ", LMargin + 183, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Amount.", LMargin + 183, CurY, 0, 0, pFont)


        Else
            Common_Procedures.Print_To_PrintDocument(e, "1. Kindly send as your payment with in 30 days.", LMargin + 40, CurY, 0, 0, pFont)
        End If

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY, 0, 0, pFont)


        Juris = Common_Procedures.settings.Jurisdiction
        If Trim(Juris) = "" Then Juris = "TIRUPUR"
        'If Trim(Juris) = "" Then Juris = prn_HdDt.Rows(0).Item("Company_Address3").ToString

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "2. Subject to " & Juris & " jurisdiction only.", LMargin + 40, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        If Trim(UCase(prn_HdDt.Rows(0).Item("Company_Type").ToString)) <> "UNACCOUNT" Then

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, Brushes.Red)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            End If


        Else
            CurY = CurY + 10

        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, p1Font)
        End If
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 290, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + 230, CurY, LMargin + 230, LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + 480, CurY, LMargin + 480, LnAr(10))

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then
            '  e.Graphics.DrawLine(Pens.Black, LMargin + 230, CurY, LMargin + 230, LnAr(10))
            '  e.Graphics.DrawLine(Pens.Black, LMargin + 530, CurY, LMargin + 530, LnAr(10))
        End If

        e.HasMorePages = False

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count < Len(Trim(prn_InpOpts)) Then


                If Val(prn_InpOpts) <> "0" Then
                    prn_PageNo = 0

                    e.HasMorePages = True
                    Return
                End If

            End If

        End If

    End Sub

    Private Sub Printing_Format12_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_FaxNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_Des As String
        Dim S As String

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
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "DUPLICATE"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
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


        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_PanNo = "" : Cmp_FaxNo = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = "" : Cmp_Des = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Des = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "MOBILE NO : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_FaxNo").ToString) <> "" Then
            Cmp_FaxNo = "FAX NO : " & prn_HdDt.Rows(0).Item("Company_FaxNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "E-MAIL : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        p1Font = New Font("Courier New", 15, FontStyle.Bold)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        p1Font = New Font("Courier New", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)


        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Courier New", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 10, CurY - 5, 120, 100)


        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Des, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, Cmp_FaxNo, PageWidth - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, PageWidth - 10, CurY, 1, 0, pFont)



        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30
            W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("DespatchTo : ", pFont).Width - 5
            S2 = e.Graphics.MeasureString("Sent Through: ", pFont).Width


            CurY = CurY + 10
            p1Font = New Font("Courier New", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO : " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("ClothSales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            'Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothSales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            'End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Courier New", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("pan_no").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("pan_no").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_CstNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " CST : " & prn_HdDt.Rows(0).Item("Ledger_CstNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
            LnAr(3) = CurY

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Agent Name ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Lr.No  ", LMargin + C1 - 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 - 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
                W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "Date :" & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W2 + W3 + 20, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Order No ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
                Dim Lng As Integer = Microsoft.VisualBasic.Len(Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString))
                If Lng <= 20 Then
                    p1Font = New Font("Courier New", 12, FontStyle.Regular)
                ElseIf Lng <= 30 Then
                    p1Font = New Font("Courier New", 10, FontStyle.Regular)
                ElseIf Lng <= 40 Then
                    p1Font = New Font("Courier New", 7, FontStyle.Regular)
                ElseIf Lng <= 50 Then
                    p1Font = New Font("Courier New", 6, FontStyle.Regular)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "Despatch To", LMargin + C1 - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + W2 + 10, CurY, 0, 0, p1Font)


            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString) <> "" Then
                Dim Lng As Integer = Microsoft.VisualBasic.Len(Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString))
                If Lng <= 30 Then
                    p1Font = New Font("Courier New", 12, FontStyle.Regular)
                ElseIf Lng <= 40 Then
                    p1Font = New Font("Courier New", 10, FontStyle.Regular)
                ElseIf Lng <= 50 Then
                    p1Font = New Font("Courier New", 7, FontStyle.Regular)
                ElseIf Lng <= 60 Then
                    p1Font = New Font("Courier New", 6, FontStyle.Regular)
                End If
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 - 5, CurY, 0, 0, p1Font)

            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Sent Through ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString) <> "" Then
                Dim Lng As Integer = Microsoft.VisualBasic.Len(Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString))
                If Lng <= 30 Then
                    p1Font = New Font("Courier New", 12, FontStyle.Regular)
                ElseIf Lng <= 40 Then
                    p1Font = New Font("Courier New", 10, FontStyle.Regular)
                ElseIf Lng <= 50 Then
                    p1Font = New Font("Courier New", 7, FontStyle.Regular)
                ElseIf Lng <= 60 Then
                    p1Font = New Font("Courier New", 6, FontStyle.Regular)
                End If
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 - 5, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No. ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt - 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            If Trim(prn_HdDt.Rows(0).Item("Packing_Type").ToString) = "ROLL" Then
                Common_Procedures.Print_To_PrintDocument(e, "ROLLS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE\", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + 10

            'p1Font = New Font("Calibri", 8, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Cbo_Tax_Type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Tax_Type.SelectedIndexChanged
        NetAmount_Calculation()
    End Sub

    Private Sub Cbo_Tax_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Tax_Type.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub Cbo_Tax_Type_KeyDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Tax_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Tax_Type, cbo_partyname, cbo_Vechile, "", "", "", "")
    End Sub

    Private Sub Cbo_Tax_Type_KeyPress(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Tax_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Tax_Type, cbo_Vechile, "", "", "", "")
    End Sub

    Private Sub cbo_partyname_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_partyname.LostFocus
        If Trim(UCase(cbo_partyname.Tag)) <> Trim(UCase(cbo_partyname.Text)) Then
            cbo_partyname.Tag = cbo_partyname.Text
            get_RateDetails()
            NetAmount_Calculation()
        End If
    End Sub

    Private Sub cbo_partyname_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_partyname.SelectedIndexChanged
        If Trim(UCase(cbo_partyname.Tag)) <> Trim(UCase(cbo_partyname.Text)) Then
            cbo_partyname.Tag = cbo_partyname.Text
            NetAmount_Calculation()
        End If
    End Sub

    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim TxtHgt As Single
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer = 0
        Dim NoofItems_Increment As Integer
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String
        Dim prn_DetIndx As Integer = 0

        Try
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
                pFont = New Font("Calibri", 10, FontStyle.Bold)
            Else
                pFont = New Font("Calibri", 10, FontStyle.Regular)
            End If

            NoofItems_PerPage = 3 ' 5

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 105 : SubClAr(2) = 130 : SubClAr(3) = 45 : SubClAr(4) = 80 : SubClAr(5) = 45 : SubClAr(6) = 80 : SubClAr(7) = 45 : SubClAr(8) = 80
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            'SubClAr(1) = 140 : SubClAr(2) = 130 : SubClAr(3) = 60 : SubClAr(4) = 95 : SubClAr(5) = 60 : SubClAr(6) = 90 : SubClAr(7) = 60
            'SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

            TxtHgt = 18.75 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY)
            LnAr2 = CurY
            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TAX AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            Da = New SqlClient.SqlDataAdapter("select * from Invoice_Head where Invoice_Code = '" & Trim(EntryCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0
                CurY = CurY - 20

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    ItmNm1 = "998821" ' Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

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

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Assessable_Value").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Assessable_Value").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)

                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1



                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Assessable_Value").ToString)
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
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)
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
            If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
                BmsInWrds = Replace(Trim(BmsInWrds), "", "")
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub txt_InvoicePrefixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_InvoicePrefixNo.KeyDown
        On Error Resume Next
        'If e.KeyValue = 38 Then txt_Packing.Focus()
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_InvoiceSufixNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_InvoiceSufixNo, Nothing, cbo_partyname, "", "", "", "")
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_InvoiceSufixNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_InvoiceSufixNo, cbo_partyname, "", "", "", "")
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_InvoiceSufixNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            'Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_InvoiceSufixNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            'f.MdiParent = MDIParent1
            'f.Show()

        End If
    End Sub

    Private Sub Update_PrintOut_Status(Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim vPrnSTS As Integer = 0


        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            vPrnSTS = 0
            If chk_Printed.Checked = True Then
                vPrnSTS = 1
            End If

            cmd.CommandText = "Update Invoice_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If chk_Printed.Checked = True Then
                chk_Printed.Visible = True
                If Val(Common_Procedures.User.IdNo) = 1 Then
                    chk_Printed.Enabled = True
                End If
            End If

            cmd.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub

    Private Sub PrintPreview_Shown(ByVal sender As Object, ByVal e As System.EventArgs)
        'Capture the click events for the toolstrip in the dialog box when the dialog is shown
        Dim ts As ToolStrip = CType(sender.Controls(1), ToolStrip)
        AddHandler ts.ItemClicked, AddressOf PrintPreview_Toolstrip_ItemClicked
    End Sub

    Private Sub PrintPreview_Toolstrip_ItemClicked(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs)
        'If it is the print button that was clicked: run the printdialog
        If LCase(e.ClickedItem.Name) = LCase("printToolStripButton") Then

            Try
                chk_Printed.Checked = True
                chk_Printed.Visible = True
                Update_PrintOut_Status()

            Catch ex As Exception
                MsgBox("Print Error: " & ex.Message)

            End Try
        End If
    End Sub

    'Private Sub Printing_Format5_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim EntryCode As String
    '    Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font, p1Font As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim W1 As Single = 0
    '    Dim SNo As Integer
    '    Dim Cmp_Name As String = ""
    '    Dim Wgt_Bag As String = ""
    '    Dim BagNo1 As String = "", BagNo2 As String = ""

    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
    '            e.PageSettings.PaperSize = ps
    '            Exit For
    '        End If
    '    Next

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 30
    '        .Right = 45
    '        .Top = 35
    '        .Bottom = 40
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Calibri", 10, FontStyle.Bold)

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    With PrintDocument1.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With
    '    If PrintDocument1.DefaultPageSettings.Landscape = True Then
    '        With PrintDocument1.DefaultPageSettings.PaperSize
    '            PrintWidth = .Height - TMargin - BMargin
    '            PrintHeight = .Width - RMargin - LMargin
    '            PageWidth = .Height - TMargin
    '            PageHeight = .Width - RMargin
    '        End With
    '    End If

    '    NoofItems_PerPage = 5


    '    Erase LnAr
    '    Erase ClArr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    'prn_WgtPerBag_Col_STS = False ' True
    '    'If prn_HdDt.Rows.Count > 0 Then

    '    '    ItmNm1 = Trim(prn_DetDt.Rows(0).Item("Count_Name").ToString) & " " & Trim(prn_DetDt.Rows(0).Item("Count_Description").ToString) & IIf(Trim(prn_DetDt.Rows(0).Item("Mill_Name").ToString) <> "", " - " & Trim(prn_DetDt.Rows(0).Item("Mill_Name").ToString), "")

    '    '    prn_WgtPerBag_Col_STS = True
    '    '    If InStr(1, Trim(UCase(ItmNm1)), "WASTE") > 0 Then
    '    '        prn_WgtPerBag_Col_STS = False
    '    '    End If

    '    'End If

    '    'If prn_WgtPerBag_Col_STS = True Then
    '    'ClArr(1) = 30 : ClArr(2) = 150 : ClArr(3) = 130 : ClArr(4) = 77 : ClArr(5) = 50 : ClArr(6) = 60 : ClArr(7) = 70 : ClArr(8) = 60
    '    'ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))
    '    'Else
    '    ClArr(1) = 30 : ClArr(2) = 140 : ClArr(3) = 125 : ClArr(4) = 77 : ClArr(5) = 75 : ClArr(6) = 0 : ClArr(7) = 95 : ClArr(8) = 75
    '    ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

    '    'End If


    '    'TxtHgt = e.Graphics.MeasureString("A", pFont).Height
    '    TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

    '    EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) = 0 Then
    '                NoofItems_PerPage = NoofItems_PerPage + 1
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then
    '                NoofItems_PerPage = NoofItems_PerPage + 1
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) = 0 Then
    '                NoofItems_PerPage = NoofItems_PerPage + 1
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then
    '                NoofItems_PerPage = NoofItems_PerPage + 1
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then
    '                NoofItems_PerPage = NoofItems_PerPage + 1
    '            End If
    '            If NoofItems_PerPage >= 10 Then
    '                TxtHgt = 18.5
    '            End If


    '            Printing_Format5_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


    '            NoofDets = 0
    '            CHk_Details_Cnt = 0
    '            CurY = CurY - 10

    '            'CurY = CurY + TxtHgt
    '            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

    '            If prn_DetDt.Rows.Count > 0 Then

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    If NoofDets >= NoofItems_PerPage Then
    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        Printing_Format5_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

    '                        e.HasMorePages = True
    '                        Return

    '                    End If

    '                    prn_DetSNo = prn_DetSNo + 1

    '                    ItmNm1 = prn_HdDt.Rows(0).Item("Yarn_Description").ToString
    '                    If Trim(ItmNm1) = "" Then
    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString) & " " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString) & IIf(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString) <> "", " - " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString), "")
    '                    End If
    '                    ItmNm2 = ""
    '                    If Len(ItmNm1) > 35 Then
    '                        For I = 35 To 1 Step -1
    '                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                        Next I
    '                        If I = 0 Then I = 35
    '                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                    End If

    '                    BagNo1 = ""
    '                    BagNo2 = ""
    '                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Bag_No").ToString) <> "" Then
    '                        BagNo1 = "BAG NOs. : " & prn_DetDt.Rows(prn_DetIndx).Item("Bag_No").ToString
    '                        BagNo2 = ""
    '                        If Len(BagNo1) > 25 Then
    '                            For I = 25 To 1 Step -1
    '                                If Mid$(Trim(BagNo1), I, 1) = " " Or Mid$(Trim(BagNo1), I, 1) = "," Or Mid$(Trim(BagNo1), I, 1) = "." Or Mid$(Trim(BagNo1), I, 1) = "-" Or Mid$(Trim(BagNo1), I, 1) = "/" Or Mid$(Trim(BagNo1), I, 1) = "_" Or Mid$(Trim(BagNo1), I, 1) = "\" Or Mid$(Trim(BagNo1), I, 1) = "[" Or Mid$(Trim(BagNo1), I, 1) = "]" Or Mid$(Trim(BagNo1), I, 1) = "{" Or Mid$(Trim(BagNo1), I, 1) = "}" Then Exit For
    '                            Next I
    '                            If I = 0 Then I = 25
    '                            BagNo2 = Microsoft.VisualBasic.Right(Trim(BagNo1), Len(BagNo1) - I)
    '                            BagNo1 = Microsoft.VisualBasic.Left(Trim(BagNo1), I - 1)
    '                        End If
    '                    End If

    '                    CurY = CurY + TxtHgt

    '                    SNo = SNo + 1
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), "#######0"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)


    '                    Wgt_Bag = "0"
    '                    If prn_WgtPerBag_Col_STS = True Then
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
    '                            Wgt_Bag = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString) / Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), "#########0.000")
    '                        End If
    '                    End If
    '                    If Val(Wgt_Bag) <> 0 Then
    '                        Common_Procedures.Print_To_PrintDocument(e, Val(Wgt_Bag), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
    '                    End If

    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString)), PageWidth - 10, CurY, 1, 0, pFont)

    '                    NoofDets = NoofDets + 1

    '                    If Trim(ItmNm2) <> "" Then
    '                        CurY = CurY + TxtHgt - 5
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                        NoofDets = NoofDets + 1
    '                    End If


    '                    p1Font = New Font("Calibri", 9, FontStyle.Bold)

    '                    If Trim(BagNo1) <> "" Then
    '                        CurY = CurY + TxtHgt + TxtHgt - 10
    '                        NoofDets = NoofDets + 2
    '                        Common_Procedures.Print_To_PrintDocument(e, BagNo1, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
    '                        If Trim(BagNo2) = "" Then CurY = CurY + TxtHgt : NoofDets = NoofDets + 1
    '                    End If

    '                    W1 = e.Graphics.MeasureString("BAG NOs. : ", p1Font).Width

    '                    If Trim(BagNo2) <> "" Then
    '                        CurY = CurY + TxtHgt - 5
    '                        NoofDets = NoofDets + 1
    '                        Common_Procedures.Print_To_PrintDocument(e, BagNo2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
    '                        CurY = CurY + TxtHgt : NoofDets = NoofDets + 1
    '                    End If


    '                    prn_DetIndx = prn_DetIndx + 1

    '                    CHk_Details_Cnt = prn_DetIndx

    '                Loop

    '            End If

    '            Printing_Format5_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

    '            If Trim(prn_InpOpts) <> "" Then
    '                If prn_Count < Len(Trim(prn_InpOpts)) Then


    '                    If Val(prn_InpOpts) <> "0" Then
    '                        prn_DetIndx = 0
    '                        prn_DetSNo = 0
    '                        prn_PageNo = 0

    '                        e.HasMorePages = True
    '                        Return
    '                    End If

    '                End If
    '            End If

    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub

    'Private Sub Printing_Format5_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
    '    Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
    '    Dim strHeight As Single
    '    Dim LedNmAr(10) As String
    '    Dim Cmp_Desc As String, Cmp_Email As String
    '    Dim Cen1 As Single = 0
    '    Dim W1 As Single = 0, S1 As Single = 0
    '    Dim W2 As Single = 0
    '    Dim LInc As Integer = 0
    '    Dim prn_OriDupTri As String = ""
    '    Dim S As String = ""
    '    Dim CurX As Single = 0
    '    Dim strWidth As Single = 0
    '    Dim BlockInvNoY As Single = 0
    '    Dim Trans_Nm As String = ""
    '    Dim Indx As Integer = 0
    '    Dim HdWd As Single = 0
    '    Dim H1 As Single = 0
    '    Dim W3 As Single = 0
    '    Dim CurY1 As Single = 0
    '    Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
    '    Dim i As Integer = 0
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim Y1 As Single = 0, Y2 As Single = 0
    '    Dim vDelvPanNo As String = ""
    '    Dim vLedPanNo As String = ""
    '    Dim vHeading As String = ""
    '    Dim vTransGSTNo As String = ""

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    prn_Count = prn_Count + 1

    '    prn_OriDupTri = ""
    '    If Trim(prn_InpOpts) <> "" Then
    '        If prn_Count <= Len(Trim(prn_InpOpts)) Then

    '            S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

    '            If Val(S) = 1 Then
    '                prn_OriDupTri = "ORIGINAL FOR BUYER"
    '            ElseIf Val(S) = 2 Then
    '                prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
    '            ElseIf Val(S) = 3 Then
    '                prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
    '            ElseIf Val(S) = 4 Then
    '                prn_OriDupTri = "EXTRA COPY"
    '            Else
    '                If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
    '                    prn_OriDupTri = Trim(prn_InpOpts)
    '                End If
    '            End If

    '        End If

    '    End If

    '    If PageNo <= 1 Then
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If


    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY

    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
    '    Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
    '    Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

    '    Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

    '    If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
    '        Cmp_Add1 = "OFFICE : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
    '        Cmp_Add2 = "FACTORY : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

    '    Else

    '        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
    '        If Trim(Cmp_Add1) <> "" Then
    '            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
    '                Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
    '            Else
    '                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
    '            End If
    '        Else
    '            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
    '        End If

    '        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
    '        If Trim(Cmp_Add2) <> "" Then
    '            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
    '                Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
    '            Else
    '                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
    '            End If
    '        Else
    '            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
    '        End If

    '    End If

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
    '    End If

    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "PAN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
    '        Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
    '        Cmp_PanCap = "PAN : "
    '        Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
    '    End If

    '    '***** GST START *****
    '    If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
    '        Cmp_StateCap = "STATE : "
    '        Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
    '        Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
    '        Cmp_GSTIN_Cap = "GSTIN : "
    '        Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
    '    End If
    '    '***** GST END *****

    '    CurY = CurY + TxtHgt - 15

    '    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1290" Then '---- ARJUNA Textiles (SOMANUR)
    '    '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Arjuna_Tex, Drawing.Image), LMargin + 15, CurY + 5, 110, 100)
    '    'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1292" Then
    '    '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.sarveswara_logo, Drawing.Image), LMargin + 15, CurY + 5, 110, 100)
    '    'End If

    '    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1290" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1292" Then '---- ARJUNA Textiles (SOMANUR)
    '    'p1Font = New Font("Copperplate Gothic", 24, FontStyle.Bold)
    '    'Else
    '    p1Font = New Font("Calibri", 22, FontStyle.Bold)
    '    ' End If


    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
    '    ItmNm2 = ""
    '    If Trim(ItmNm1) <> "" Then
    '        ItmNm1 = "(" & Trim(ItmNm1) & ")"
    '        If Len(ItmNm1) > 85 Then
    '            For i = 85 To 1 Step -1
    '                If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
    '            Next i
    '            If i = 0 Then i = 85
    '            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
    '            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
    '        End If
    '    End If

    '    If Trim(ItmNm1) <> "" Then
    '        CurY = CurY + strHeight - 5
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
    '        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
    '    End If

    '    If Trim(ItmNm2) <> "" Then
    '        CurY = CurY + strHeight - 3
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
    '        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    End If

    '    CurY = CurY + strHeight
    '    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1290" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1292" Then '---- Arjuna Textiles
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 140, CurY, 0, PrintWidth - 140, pFont, True)
    '    ' CurY = CurY + TxtHgt
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 140, CurY, 0, PrintWidth - 140, pFont, True)

    '    ' Else
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

    '    '  End If



    '    '***** GST START *****
    '    CurY = CurY + TxtHgt

    '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
    '    strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
    '    If PrintWidth > strWidth Then
    '        CurX = LMargin + (PrintWidth - strWidth) / 2
    '    Else
    '        CurX = LMargin
    '    End If

    '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
    '    strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

    '    strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
    '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
    '    strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

    '    strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
    '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
    '    strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


    '    CurY = CurY + TxtHgt + 5
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Y1 = CurY + 0.5
    '    Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
    '    Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


    '    vHeading = "TAX INVOICE"

    '    CurY = CurY + TxtHgt - 15
    '    p1Font = New Font("Calibri", 16, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)

    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try


    '        BlockInvNoY = CurY
    '        C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

    '        W1 = e.Graphics.MeasureString("Broker Phone No   :", pFont).Width
    '        W2 = e.Graphics.MeasureString("Transporter GSTIN :", pFont).Width
    '        S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

    '        CurY1 = CurY + 10

    '        'Left Side
    '        Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
    '        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
    '        End If

    '        Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)

    '        CurY1 = CurY1 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


    '        Common_Procedures.Print_To_PrintDocument(e, "Transporter Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W2 + 30, CurY1, 0, PageWidth - (LMargin + C2 + W2 + 30), pFont)


    '        CurY1 = CurY1 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


    '        vTransGSTNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_GSTinNo", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)) & ")")
    '        Common_Procedures.Print_To_PrintDocument(e, "Transporter GSTIN", LMargin + C2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, vTransGSTNo, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)


    '        CurY1 = CurY1 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
    '            strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
    '        End If


    '        Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)


    '        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1066" Then '---- SOUTHERN COT SPINNERS
    '        '    Common_Procedures.Print_To_PrintDocument(e, "Date & Time Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
    '        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
    '        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
    '        'End If


    '        CurY1 = CurY1 + TxtHgt
    '        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1290" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1292" Then '---- ARJUNA Textiles (SOMANUR)
    '        'Common_Procedures.Print_To_PrintDocument(e, "Broker Name", LMargin + 10, CurY1, 0, 0, pFont)
    '        ' Else
    '        Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY1, 0, 0, pFont)
    '        ' End If
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


    '        Common_Procedures.Print_To_PrintDocument(e, "LR No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
    '            strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C2 + W2 + 35 + strWidth, CurY1, 0, 0, pFont)
    '        End If



    '        CurY1 = CurY1 + TxtHgt
    '        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1290" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1292" Then '---- ARJUNA Textiles (SOMANUR)
    '        'Common_Procedures.Print_To_PrintDocument(e, "Broker Phone No", LMargin + 10, CurY1, 0, 0, pFont)
    '        '  Else
    '        Common_Procedures.Print_To_PrintDocument(e, "Agent Phone No", LMargin + 10, CurY1, 0, 0, pFont)
    '        '  End If
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_PhoneNo").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
    '        End If

    '        CurY = CurY1 + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        Y1 = CurY + 0.5
    '        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
    '        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


    '        CurY1 = CurY + TxtHgt - 10

    '        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - (LMargin + C2), pFont)
    '        CurY = CurY1 + TxtHgt + 5


    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY
    '        CurY = CurY + 10

    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, C2, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, PageWidth - (LMargin + C2 + 10), p1Font)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt

    '        vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

    '        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

    '            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
    '                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
    '            End If

    '            If Trim(vLedPanNo) <> "" Then
    '                strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
    '                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
    '            End If

    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
    '            vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
    '        Else
    '            vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
    '        End If

    '        If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
    '            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
    '                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
    '            End If
    '            If Trim(vDelvPanNo) <> "" Then
    '                strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
    '                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
    '            End If
    '        End If


    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY
    '        LnAr(4) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))


    '        Y1 = CurY + 0.5
    '        Y2 = CurY + TxtHgt - 5 + TxtHgt + 15
    '        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

    '        CurY = CurY + TxtHgt - 5

    '        Common_Procedures.Print_To_PrintDocument(e, "SNo", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2) + ClAr(3), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY - TxtHgt + 5, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 5, 2, ClAr(5), pFont)

    '        If prn_WgtPerBag_Col_STS = True Then
    '            Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY - TxtHgt + 5, 2, ClAr(6), pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 5, 2, ClAr(6), pFont)
    '        End If

    '        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY - TxtHgt + 5, 2, ClAr(7), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "KGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 5, 2, ClAr(7), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)


    '        CurY = CurY + TxtHgt + 15
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(6) = CurY

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format5_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '    Dim p1Font As Font
    '    Dim BmsInWrds As String
    '    Dim I As Integer
    '    Dim vTaxPerc As Single = 0
    '    Dim BankNm1 As String = ""
    '    Dim BankNm2 As String = ""
    '    Dim BankNm3 As String = ""
    '    Dim BankNm4 As String = ""
    '    Dim BankNm5 As String = ""
    '    Dim LftCurY As Single = 0, RgtCurY As Single = 0, vPCurY As Single = 0
    '    Dim TaxAmt As Single = 0
    '    Dim TOT As Single = 0
    '    Dim Y1 As Single = 0, Y2 As Single = 0
    '    Dim w1 As Single = 0
    '    Dim w2 As Single = 0, C1 As Single = 0
    '    Dim Jurs As String = ""
    '    Dim vNoofHsnCodes As Integer = 0
    '    Dim BInc As Integer
    '    Dim BnkDetAr() As String
    '    Dim Rup1 As String = "", Rup2 As String = ""
    '    Dim M As Integer = 0


    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage
    '            CurY = CurY + TxtHgt
    '        Next

    '        CurY = CurY + TxtHgt

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(11) = CurY

    '        ''CurY = CurY + TxtHgt - 10
    '        ''If is_LastPage = True Then
    '        ''    If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then
    '        ''        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
    '        ''        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), "##########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
    '        ''        Common_Procedures.Print_To_PrintDocument(e, "Total Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
    '        ''        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString)), PageWidth - 10, CurY, 1, 0, pFont)

    '        ''    Else
    '        ''        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
    '        ''        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), "##########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
    '        ''        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Weight").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
    '        ''        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString)), PageWidth - 10, CurY, 1, 0, pFont)

    '        ''    End If

    '        ''End If
    '        ''CurY = CurY + TxtHgt + 5

    '        ''e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(5) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
    '        If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))

    '        Else
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))

    '        End If
    '        If Val(ClAr(5)) <> 0 Then
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
    '        End If
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
    '        Else
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
    '        End If

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
    '        Erase BnkDetAr
    '        If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
    '            BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

    '            BInc = -1

    '            BInc = BInc + 1
    '            If UBound(BnkDetAr) >= BInc Then
    '                BankNm1 = Trim(BnkDetAr(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BnkDetAr) >= BInc Then
    '                BankNm2 = Trim(BnkDetAr(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BnkDetAr) >= BInc Then
    '                BankNm3 = Trim(BnkDetAr(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BnkDetAr) >= BInc Then
    '                BankNm4 = Trim(BnkDetAr(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BnkDetAr) >= BInc Then
    '                BankNm5 = Trim(BnkDetAr(BInc))
    '            End If

    '        End If


    '        vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

    '        LftCurY = CurY


    '        Y1 = LftCurY + 0.55
    '        Y2 = LftCurY + TxtHgt - 15 + TxtHgt + 5
    '        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

    '        LftCurY = LftCurY + TxtHgt - 15
    '        Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, LftCurY, 0, 0, pFont)

    '        LftCurY = LftCurY + TxtHgt - 15 + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LftCurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LftCurY)
    '        LnAr(9) = LftCurY
    '        LftCurY = LftCurY + TxtHgt - 10

    '        p1Font = New Font("Calibri", 8, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only; If any running fault or", LMargin + 10, LftCurY, 0, 0, p1Font)

    '        LftCurY = LftCurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "quality defect noted in yarn please inform with first fabric roll at once. We will", LMargin + 25, LftCurY, 0, 0, p1Font)

    '        LftCurY = LftCurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "accept only one roll at defect otherwise we do not hold ourself responsible.", LMargin + 25, LftCurY, 0, 0, p1Font)

    '        LftCurY = LftCurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, LftCurY, 0, 0, p1Font)

    '        LftCurY = LftCurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "3. Interest at the rate of 24% per annum will be charge from the due date.", LMargin + 10, LftCurY, 0, 0, p1Font)

    '        LftCurY = LftCurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "4. All Payment should be made by A/C payee cheque or RTGS/NEFT.", LMargin + 10, LftCurY, 0, 0, p1Font)

    '        LftCurY = LftCurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "5. Subject to " & Trim(UCase(Common_Procedures.settings.Jurisdiction)) & " Jurisdiction Only. ", LMargin + 10, LftCurY, 0, 0, p1Font)
    '        'LftCurY = LftCurY + TxtHgt

    '        '---- Right Side - Amount Details

    '        RgtCurY = LnAr(5)



    '        If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
    '            RgtCurY = RgtCurY + TxtHgt - 15
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) & "%", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString)), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '            End If
    '            RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)
    '        End If


    '        If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
    '            RgtCurY = RgtCurY + TxtHgt - 15
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString)), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '            End If
    '            RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)
    '        End If



    '        If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
    '            RgtCurY = RgtCurY + TxtHgt - 15
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString))), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '            End If
    '            RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)
    '        End If


    '        If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
    '            RgtCurY = RgtCurY + TxtHgt - 15
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("AddLess_Name").ToString), LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString))), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '            End If
    '            RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)
    '        End If

    '        'If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
    '        RgtCurY = RgtCurY + TxtHgt - 15
    '        If is_LastPage = True Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Taxable Value  ", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString)), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        End If
    '        RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)
    '        'End If


    '        RgtCurY = RgtCurY + TxtHgt - 15

    '        If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString)), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        End If


    '        RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)

    '        RgtCurY = RgtCurY + TxtHgt - 15
    '        If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString)), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        End If

    '        RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)


    '        RgtCurY = RgtCurY + TxtHgt - 15
    '        If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        End If


    '        RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)


    '        RgtCurY = RgtCurY + TxtHgt - 15
    '        If Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Add : " & Trim(prn_HdDt.Rows(0).Item("Cess_Name").ToString) & "  @ " & Format(Val(prn_HdDt.Rows(0).Item("Cess_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString)), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        End If
    '        RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)


    '        'RgtCurY = RgtCurY + TxtHgt - 15
    '        'Common_Procedures.Print_To_PrintDocument(e, "Total  TAX Amount", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        'RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)

    '        RgtCurY = RgtCurY + TxtHgt - 15
    '        If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, RgtCurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 10, RgtCurY, 1, 0, pFont)
    '        End If

    '        RgtCurY = RgtCurY + TxtHgt + 4 - 0.5
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), RgtCurY, PageWidth, RgtCurY)
    '        vPCurY = RgtCurY

    '        CurY = IIf(RgtCurY > LftCurY, RgtCurY, LftCurY)


    '        If (CurY - RgtCurY) > TxtHgt Then
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
    '            vPCurY = CurY
    '        End If

    '        Y1 = vPCurY + 0.55
    '        Y2 = CurY + TxtHgt + TxtHgt + TxtHgt - 10
    '        Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)

    '        p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 + 12, CurY + 5, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 6, 1, 0, p1Font)

    '        CurY = CurY + TxtHgt

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
    '        LnAr(10) = CurY

    '        'Y1 = CurY - TxtHgt - TxtHgt + 15 + 0.55
    '        'Y2 = CurY
    '        Y1 = CurY + 0.55
    '        Y2 = CurY + TxtHgt + TxtHgt - 10
    '        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, Y2)
    '        'CurY = CurY + TxtHgt - 15
    '        Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY + 5, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 10, CurY + 5, 1, 0, pFont)





    '        CurY = CurY + TxtHgt + 10 - 0.5

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, LnAr(10))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        CurY = CurY + 5




    '        If is_LastPage = True Then
    '            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

    '            BmsInWrds = Trim(StrConv(BmsInWrds, VbStrConv.ProperCase))

    '            p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, " " & BmsInWrds, LMargin + 10, CurY, 0, 0, p1Font)


    '        End If

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        CurY = CurY + TxtHgt - 15
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "Payment Terms : " & Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString), LMargin + 10, CurY, 0, 0, p1Font)

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        LnAr(14) = CurY

    '        p1Font = New Font("Calibri", 7.5, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)

    '        p1Font = New Font("Calibri", 16, FontStyle.Bold Or FontStyle.Underline)

    '        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY + 5, 0, 0, p1Font)


    '        CurY = CurY + TxtHgt - 5
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)

    '        CurY = CurY + 15
    '        p1Font = New Font("Courier New", 16, FontStyle.Bold)

    '        Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm5, LMargin + 10, CurY, 0, 0, p1Font)

    '        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4) + ClAr(5), pFont)

    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(14))

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(14))
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    Private Sub Printing_Format6_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font, p2Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0
        Dim TxtHgt As Single = 0, TxtHgtInc As Single = 0, strHeight As Single = 0, strWidth As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim LnAr(15) As Single
        Dim W1 As Single, W2 As Single = 0, W3 As Single = 0, S1 As Single
        Dim CurY1 As Single = 0, CurY2 As Single
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single, C6 As Single
        Dim AmtInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim NoofDets As Integer = 0, NoofItems_PerPage As Integer = 0
        Dim V1 As String = ""
        Dim V2 As String = ""
        Dim CenLn As Single
        Dim NetAmt As String = 0, RndOff As String = 0
        Dim Juris As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim S As String = ""
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Led_StateCap As String = ""
        Dim Led_StateNm As String = ""
        Dim Led_StateCode As String = ""
        Dim Ven_StateCap As String = ""
        Dim Ven_StateNm As String = ""
        Dim Ven_StateCode As String = ""
        Dim SNo As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40  ' 50
            .Right = 50  '50
            .Top = 35
            .Bottom = 50 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            pFont = New Font("Calibri", 11, FontStyle.Regular)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            pFont = New Font("Calibri", 11, FontStyle.Bold)
        Else
            pFont = New Font("Calibri", 10, FontStyle.Regular)
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            TxtHgt = 18 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Else
            TxtHgt = 17 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Avinashi)
            TxtHgtInc = 5.5
            NoofItems_PerPage = 8 '13 ' 15
        Else
            TxtHgtInc = 0
            NoofItems_PerPage = 7
        End If

        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 50 ' 80
        C2 = 280 ' 330
        C3 = 115 ' 120
        C4 = 85 ' 90
        C5 = 85 ' 180
        C6 = PageWidth - (LMargin + C1 + C2 + C3 + C4 + C5)

        CenLn = C1 + C2 + (C3 \ 2)


        If (Trim(UCase(Common_Procedures.settings.CustomerCode))) = "1006" Then
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            CurY = TMargin - 20
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
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
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt - 0.1, 1, 0, pFont)
        End If



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_panNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
                Cmp_StateNm = Cmp_StateNm & "     " & Cmp_StateCode
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                Cmp_GSTIN_Cap = "GSTIN/UID : "
            Else
                Cmp_GSTIN_Cap = "GSTIN : "
            End If

            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY + 10, 100, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Kalaimagal Sizing (Palladam)
            ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.AADHAVAN, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1284" Then '----- SHREE VEL SIZING (PALLADAM)
            'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_VelSizing_Palladam, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- BRT Sizing (somanur)
            ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_BRT, Drawing.Image), LMargin + 20, CurY + 20, 130, 110)
        End If


        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 22, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + TxtHgt + 6
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No & Cmp_CstNo, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No & Cmp_CstNo, LMargin + 10, CurY, 2, 0, p1Font)
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, PageWidth - 10, CurY, 1, 0, p1Font)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1006" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Tax Is Payable On Reverse Charge : NO", LMargin + 10, CurY, 2, 0, p1Font)
        End If

        'p1Font = New Font("Calibri", 11, FontStyle.Bold)
        'strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        'strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width

        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        End If

        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)
        End If


        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        End If

        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & Cmp_CstNo, CurX, CurY, 0, 0, pFont, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)
        End If


        'CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
        End If


        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth, CurY, 1, 0, pFont)
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Led_StateCap = "STATE : "
                Led_StateNm = prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString
                If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString) <> "" Then
                    Led_StateCode = prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString
                    Led_StateNm = Led_StateCode & "  " & Led_StateNm
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Vendor_State_Name").ToString) <> "" Then
                Ven_StateCap = "STATE : "
                Ven_StateNm = prn_HdDt.Rows(0).Item("Vendor_State_Name").ToString
                If Trim(prn_HdDt.Rows(0).Item("Vendor_State_Code").ToString) <> "" Then
                    Ven_StateCode = prn_HdDt.Rows(0).Item("Vendor_State_Code").ToString
                    Ven_StateNm = Ven_StateCode & "  " & Ven_StateNm
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("pan_no").ToString) <> "" Then
                Led_TinNo = " PAN NO :  " & Trim(prn_HdDt.Rows(0).Item("pan_no").ToString)
            End If
        End If

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        W1 = e.Graphics.MeasureString("Broker Phone No   :", pFont).Width
        W2 = e.Graphics.MeasureString("Transporter GSTIN :", pFont).Width
        S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

        CurY1 = CurY + 10

        'Left Side
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
        End If
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C1 + C2 + (C3 / 3) + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + (C3 / 3) + W2, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + C2 + (C3 / 3) + W2 + 15, CurY1, 0, 0, p1Font)

        CurY1 = CurY1 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C1 + C2 + (C3 / 3) + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + (C3 / 3) + W2, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + C2 + (C3 / 3) + W2 + 15, CurY1, 0, 0, p1Font)


        CurY1 = CurY1 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge(Y/N) ", LMargin + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "No", LMargin + W1 + 30, CurY1, 0, 0, p1Font)



        Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C1 + C2 + (C3 / 3) + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + (C3 / 3) + W2, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + C2 + (C3 / 3) + W2 + 10, CurY1, 0, 0, p1Font)
        ' If Trim(prn_HdDt.Rows(0).Item("Invoice_Date").ToString) <> "" Then
        'Else
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
        'End If
        CurY1 = CurY1 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "State ", LMargin + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Led_StateNm, LMargin + W1 + 30, CurY1, 0, 0, p1Font)

        CurY = CurY1 + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + (C3 / 3), LnAr(3), LMargin + C1 + C2 + (C3 / 3), LnAr(4))
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


        CurY1 = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER  (BILLED TO)", LMargin + 10, CurY1, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C1 + C2 + (C3 / 3) + 10, CurY1, 0, 0, p1Font)
        CurY = CurY1 + TxtHgt + 5


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        CurY = CurY + 5
        CurY2 = CurY
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, C2, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Vendor_Name").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY, 0, 0, p1Font)
        End If


        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) <> "" Then
            CurY1 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Delivery_Name").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Vendor_Address2").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" Then
            CurY2 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address1").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address1").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If


        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Delivery_Name").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Vendor_Address4").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_Address2").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address2").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address2").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If


        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Delivery_Name").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("GST_No").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_Address3").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address3").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address3").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If



        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Delivery_Name").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Name").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, p1Font)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_Address4").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address4").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address4").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If


        If prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_StateCap & Led_StateNm, LMargin + 10, CurY1, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Delivery_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Delivery_Address1").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_State_Name").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Ven_StateCap & Ven_StateNm, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_StateCap & Led_StateNm, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If

        'vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then
            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                'Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY1, 0, 0, p1Font)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 20, CurY1, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + 10, CurY1, 0, 0, p1Font)
            End If

            If Trim(vLedPanNo) <> "" Then
                strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, p1Font)
            End If

        End If


        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Delivery_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Delivery_Address3").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Delivery_Address4").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_GST_No").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("GST_No").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 20, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("GST_No").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, p1Font)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, p1Font)
        End If



        'If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
        '    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
        'Else
        '    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
        'End If

        'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
        '    If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
        '        Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
        '    End If
        '    If Trim(vDelvPanNo) <> "" Then
        '        strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
        '    End If
        'End If

        If CurY1 > CurY2 Then
            CurY = CurY1 + TxtHgt + 7
        Else
            CurY = CurY2 + TxtHgt + 7
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + (C3 / 3), LnAr(4), LMargin + C1 + C2 + (C3 / 3), LnAr(6))


        p1Font = New Font("Calibri", 10, FontStyle.Bold)



        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "Sl No ", LMargin, CurY, 2, C1, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Product", LMargin + C1, CurY, 2, C2, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "SAC ", LMargin + C1 + C2 + 16, CurY, 2, C3, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Qty in", LMargin + C1 + C2 + C3, CurY, 2, C4, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Rate Per", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + C1 + C2 + C3 + C4 + C5, CurY, 2, C6, p1Font)

        CurY = CurY + TxtHgt - 5
        'Common_Procedures.Print_To_PrintDocument(e, "Beams ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Description", LMargin + C1, CurY, 2, C2, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " ", LMargin, CurY + C1, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Code", LMargin + C1 + C2 + 16, CurY, 2, C3, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Kgs", LMargin + C1 + C2 + C3, CurY, 2, C4, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Kgs", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "(Rs)", LMargin + C1 + C2 + C3 + C4 + C5, CurY, 2, C6, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        NoofDets = 0

        'CurY = CurY + TxtHgt - 8
        'p2Font = New Font("Calibri", 12, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821", LMargin + C1 + 10, CurY, 2, C2, p2Font)

        CurY = CurY + TxtHgt - 3
        p1Font = New Font("Calibri", 10, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, " Warping & Sizing Charges ", LMargin + C1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 3

        Common_Procedures.Print_To_PrintDocument(e, "( Textile Manufacturing Services ) ", LMargin + C1 + 10, CurY, 0, 0, pFont)

        NoofDets = NoofDets + 1


        SNo = 0
        SNo = SNo + 1

        CurY = CurY + TxtHgtInc + 2


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
            If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

        Else
            If (dt2.Rows(0).Item("Mill_Name").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(dt2.Rows(0).Item("Mill_Name").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, SNo, LMargin, CurY, 2, C1, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString) & " - " & "NO.OF.BEAM - " & Trim(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                    'If Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString) > 0 Then

                    '    CurY = CurY + TxtHgt
                    '    Common_Procedures.Print_To_PrintDocument(e, "NO.OF.BEAMS-" & Trim(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                    'End If

                    'If Val(prn_HdDt.Rows(0).Item("SetCode_ForSelection").ToString) > 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "SET NO : " & Trim(prn_HdDt.Rows(0).Item("set_no").ToString), LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    'End If
                    NoofDets = NoofDets + 1

                End If
            End If



            If (prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text2").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate2").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If
            If (prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text3").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate3").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If



        End If



        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("welding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Welding_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Discount_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc

            If Trim(UCase(prn_HdDt.Rows(0).Item("Discount_Type").ToString)) = "PERCENTAGE" Then
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %"
                V2 = ""

            Else

                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString)
                If Val(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) = Val(Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString) Then
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString
                Else
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.000").ToString
                End If

            End If

            Common_Procedures.Print_To_PrintDocument(e, V1, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, V2, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1

        End If

        NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Tds_perc_calc").ToString), "##########0.00")

        RndOff = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt), "##########0.00")
        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
            '    CurY = CurY + TxtHgt + 10
            '    If Val(RndOff) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '        NoofDets = NoofDets + 1
            '    End If
            'End If
            CurY = CurY + TxtHgt + TxtHgtInc + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            'CurY = CurY + TxtHgt + TxtHgtInc - 10
            CurY = CurY + TxtHgt - 10
            p2Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GROSS TOTAL  ", LMargin + C1 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
            ' CurY = CurY + TxtHgt + TxtHgtInc
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  ".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
            'CurY = CurY + TxtHgt + TxtHgtInc
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If


        If Val(prn_HdDt.Rows(0).Item("Tds_Perc_Calc").ToString) > 0 Then
            'CurY = CurY + TxtHgt + TxtHgtInc
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TDS  " & prn_HdDt.Rows(0).Item("Tds_Perc") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tds_Perc_Calc").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        'If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) > 0 Then
        '    'CurY = CurY + TxtHgt + TxtHgtInc
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "IGST  " & prn_HdDt.Rows(0).Item("IGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        '    NoofDets = NoofDets + 1
        'Else
        '    CurY = CurY + TxtHgt
        '    'CurY = CurY + TxtHgt + TxtHgtInc
        '    Common_Procedures.Print_To_PrintDocument(e, "IGST   %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
        '    NoofDets = NoofDets + 1
        'End If

        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt + TxtHgtInc
        Next

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        CurY = CurY + TxtHgt - 10
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, p1Font)
        strHeight = e.Graphics.MeasureString("A", p2Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + (C3 / 3), CurY, LMargin + C1 + C2 + (C3 / 3), LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3, CurY, LMargin + C1 + C2 + C3, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, LMargin + C1 + C2 + C3 + C4, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5, CurY, LMargin + C1 + C2 + C3 + C4 + C5, LnAr(6))

        AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & AmtInWrds, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY

        'CurY = CurY + 5
        'If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
        'End If

        Erase BnkDetAr
        BankNm1 = "" : BankNm2 = "" : BankNm3 = "" : BankNm4 = ""
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

            NoofItems_PerPage = NoofItems_PerPage + 1

        End If

        'If NoofDets <= 8 Then
        '    For I = NoofDets + 1 To 8
        '        CurY = CurY + TxtHgt + 10
        '        NoofDets = NoofDets + 1
        '    Next
        'End If
        p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(BankNm1) <> "" Then
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm3) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm4) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        CurY = CurY + TxtHgt + 4
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY
        '=============GST SUMMARY============
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1036" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" Then '---- Kalaimagal Sizing (Avinashi)
        '    Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
        'End If
        '=========================

        CurY = CurY

        p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)



        Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY, 0, 0, p1Font)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then '---- Ganesh Karthi Sizing 
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Interest will be charged 24% annum if the payment is not received with in 15 days from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        'End If
        'CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 22% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh Karthi Sizing 
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged for overdue invoice more than 30 days.", LMargin + 40, CurY, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then '---- Ganesh Karthi Sizing 
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY, 0, 0, pFont)

        'End If
        Juris = Common_Procedures.settings.Jurisdiction
        If Trim(Juris) = "" Then Juris = "TIRUPUR"

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "E & O.E Subject " & Juris & " jurisdiction only.", LMargin + 40, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 700, CurY, 1, 0, p1Font)



        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))


        e.Graphics.DrawLine(Pens.Black, LMargin + 190, CurY, LMargin + 190, LnAr(11))
        'e.Graphics.DrawLine(Pens.Black, LMargin + 200, CurY, LMargin + 200, LnAr(11))

        e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
        'e.Graphics.DrawLine(Pens.Black, LMargin + 410, CurY, LMargin + 410, LnAr(11))

        e.HasMorePages = False

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count < Len(Trim(prn_InpOpts)) Then


                If Val(prn_InpOpts) <> "0" Then
                    prn_PageNo = 0

                    e.HasMorePages = True
                    Return
                End If

            End If

        End If

    End Sub

    Private Sub Printing_Format7_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font, p2Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0
        Dim TxtHgt As Single = 0, TxtHgtInc As Single = 0, strHeight As Single = 0, strWidth As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim LnAr(15) As Single
        Dim W1 As Single, W2 As Single = 0, W3 As Single = 0, S1 As Single
        Dim CurY1 As Single = 0, CurY2 As Single
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single, C6 As Single
        Dim AmtInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim NoofDets As Integer = 0, NoofItems_PerPage As Integer = 0
        Dim V1 As String = ""
        Dim V2 As String = ""
        Dim CenLn As Single
        Dim NetAmt As String = 0, RndOff As String = 0
        Dim Juris As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim S As String = ""
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim vTransGSTNo As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Led_StateCap As String = ""
        Dim Led_StateNm As String = ""
        Dim Led_StateCode As String = ""
        Dim Ven_StateCap As String = ""
        Dim Ven_StateNm As String = ""
        Dim Ven_StateCode As String = ""
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
            .Left = 30  ' 50
            .Right = 50  '50
            .Top = 30
            .Bottom = 50 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            pFont = New Font("Calibri", 11, FontStyle.Regular)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            pFont = New Font("Calibri", 11, FontStyle.Bold)
        Else
            pFont = New Font("Calibri", 10, FontStyle.Regular)
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            TxtHgt = 18 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Else
            TxtHgt = 18 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Avinashi)
            TxtHgtInc = 5.5
            NoofItems_PerPage = 8 '13 ' 15
        Else
            TxtHgtInc = 0
            NoofItems_PerPage = 9
        End If

        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 50 ' 80
        C2 = 280 ' 330
        C3 = 115 ' 120
        C4 = 85 ' 90
        C5 = 85 ' 180
        C6 = PageWidth - (LMargin + C1 + C2 + C3 + C4 + C5)

        CenLn = C1 + C2 + (C3 \ 2)


        If (Trim(UCase(Common_Procedures.settings.CustomerCode))) = "1006" Then
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            CurY = TMargin - 20
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
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
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt - 0.1, 1, 0, pFont)
        End If



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
                Cmp_StateNm = Cmp_StateNm & "     " & Cmp_StateCode
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                Cmp_GSTIN_Cap = "GSTIN/UID : "
            Else
                Cmp_GSTIN_Cap = "GSTIN : "
            End If

            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY + 10, 100, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Kalaimagal Sizing (Palladam)
            ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.AADHAVAN, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1284" Then '----- SHREE VEL SIZING (PALLADAM)
            ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_VelSizing_Palladam, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- BRT Sizing (somanur)
            ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_BRT, Drawing.Image), LMargin + 20, CurY + 20, 130, 110)
        End If


        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 22, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + TxtHgt + 6
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No & Cmp_CstNo, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No & Cmp_CstNo, LMargin + 10, CurY, 2, 0, p1Font)
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, PageWidth - 10, CurY, 1, 0, p1Font)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1006" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Tax Is Payable On Reverse Charge : NO", LMargin + 10, CurY, 2, 0, p1Font)
        End If

        'p1Font = New Font("Calibri", 11, FontStyle.Bold)
        'strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        'strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width

        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        End If

        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)
        End If


        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        End If

        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)
        End If


        'CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
        End If


        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth, CurY, 1, 0, pFont)
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Led_StateCap = "STATE : "
                Led_StateNm = prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString
                If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString) <> "" Then
                    Led_StateCode = prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString
                    Led_StateNm = Led_StateCode & "  " & Led_StateNm
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Vendor_State_Name").ToString) <> "" Then
                Ven_StateCap = "STATE : "
                Ven_StateNm = prn_HdDt.Rows(0).Item("Vendor_State_Name").ToString
                If Trim(prn_HdDt.Rows(0).Item("Vendor_State_Code").ToString) <> "" Then
                    Ven_StateCode = prn_HdDt.Rows(0).Item("Vendor_State_Code").ToString
                    Ven_StateNm = Ven_StateCode & "  " & Ven_StateNm
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("pan_no").ToString) <> "" Then
                Led_TinNo = " PAN NO :  " & Trim(prn_HdDt.Rows(0).Item("pan_no").ToString)
            End If
        End If

        W1 = e.Graphics.MeasureString("Broker Phone No   :", pFont).Width
        W2 = e.Graphics.MeasureString("Transporter GSTIN :", pFont).Width
        S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

        CurY = CurY + strHeight - 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        'p1Font = New Font("Calibri", 12, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, " TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt - 15
        Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY, 0, 0, p1Font)
        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString, LMargin + W1 + 30, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString, LMargin + W1 + 30, CurY, 0, 0, p1Font)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C1 + C2 + (C3 / 3) + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + (C3 / 3) + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + C2 + (C3 / 3) + W2 + 15, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY1 = CurY + 5

        'Left Side
        'p1Font = New Font("Calibri", 10, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        'If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
        'End If

        'Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C1 + C2 + (C3 / 3) + 10, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + (C3 / 3) + W2, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + C2 + (C3 / 3) + W2 + 15, CurY1, 0, 0, p1Font)

        'CurY1 = CurY1 + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C1 + C2 + (C3 / 3) + 10, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + (C3 / 3) + W2, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + C2 + (C3 / 3) + W2 + 15, CurY1, 0, 0, p1Font)


        'CurY1 = CurY1 + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge(Y/N) ", LMargin + 10, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "No", LMargin + W1 + 30, CurY1, 0, 0, p1Font)



        'Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C1 + C2 + (C3 / 3) + 10, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + (C3 / 3) + W2, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + C2 + (C3 / 3) + W2 + 10, CurY1, 0, 0, p1Font)
        '' If Trim(prn_HdDt.Rows(0).Item("Invoice_Date").ToString) <> "" Then
        ''Else
        ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
        ''End If
        'CurY1 = CurY1 + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "State ", LMargin + 10, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, Led_StateNm, LMargin + W1 + 30, CurY1, 0, 0, p1Font)

        'CurY = CurY1 + TxtHgt + 5
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(4) = CurY

        'e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + (C3 / 3), LnAr(3), LMargin + C1 + C2 + (C3 / 3), LnAr(4))
        'Y1 = CurY + 0.5
        'Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


        CurY1 = CurY + TxtHgt - 15

        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER  (BILLED TO)", LMargin + 10, CurY1, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C1 + C2 + (C3 / 3) + 10, CurY1, 0, 0, p1Font)
        CurY = CurY1 + TxtHgt


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        CurY = CurY + 5

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, C2, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Vendor_Name").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY, 0, 0, p1Font)
        End If


        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) <> "" Then
            CurY1 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If


        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" Then
            CurY2 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address1").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address1").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If


        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_Address2").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address2").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address2").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If


        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If


        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_Address3").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address3").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address3").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If



        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_Address4").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address4").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address4").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If


        If prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_StateCap & Led_StateNm, LMargin + 10, CurY1, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_State_Name").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Ven_StateCap & Ven_StateNm, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_StateCap & Led_StateNm, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, pFont)
        End If

        'vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then
            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                'Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY1, 0, 0, p1Font)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 20, CurY1, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + 10, CurY1, 0, 0, p1Font)
            End If

            If Trim(vLedPanNo) <> "" Then
                strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, p1Font)
            End If

        End If


        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("GST_No").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 20, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("GST_No").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, p1Font)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + C1 + C2 + (C3 / 3) + 10, CurY2, 0, 0, p1Font)
        End If



        'If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
        '    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
        'Else
        '    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
        'End If

        'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
        '    If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
        '        Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
        '    End If
        '    If Trim(vDelvPanNo) <> "" Then
        '        strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
        '    End If
        'End If

        If CurY1 > CurY2 Then
            CurY = CurY1 + TxtHgt + 7
        Else
            CurY = CurY2 + TxtHgt + 7
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurY = CurY + TxtHgt - 18

        W1 = e.Graphics.MeasureString("Set No  : ", pFont).Width

        Common_Procedures.Print_To_PrintDocument(e, "Set No : ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Set_No").ToString, LMargin + W1 + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Ends  : ", LMargin + C1, CurY, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(0).Item("ends_Name").ToString, LMargin + C1 + W1, CurY, 2, C2, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Count  : ", LMargin + C1 + C2 + (C3 / 4), CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(0).Item("Count_Name").ToString, LMargin + C1 + C2 + (C3 / 4) + W1, CurY, 2, C4, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Meters  : ", LMargin + C1 + C2 + C4 + C5, CurY, 2, C6, pFont)
        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(0).Item("Warp_Meters").ToString, LMargin + C1 + C2 + C4 + C5 + W1, CurY, 2, C6, p1Font)

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + (C3 / 3), LnAr(3), LMargin + C1 + C2 + (C3 / 3), LnAr(6))


        p1Font = New Font("Calibri", 10, FontStyle.Bold)



        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "BEAMS", LMargin, CurY, 2, C1, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "PURTICULARS", LMargin + C1, CurY, 2, C2, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "SAC ", LMargin + C1 + C2 + 16, CurY, 2, C3, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "QTY IN", LMargin + C1 + C2 + C3, CurY, 2, C4, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "RATE PER", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + C1 + C2 + C3 + C4 + C5, CurY, 2, C6, p1Font)

        CurY = CurY + TxtHgt - 5
        'Common_Procedures.Print_To_PrintDocument(e, "Beams ", LMargin, CurY, 2, C1, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Description", LMargin + C1, CurY, 2, C2, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " ", LMargin, CurY + C1, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + C1 + C2 + 16, CurY, 2, C3, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "KGS", LMargin + C1 + C2 + C3, CurY, 2, C4, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "KGS", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "(RS)", LMargin + C1 + C2 + C3 + C4 + C5, CurY, 2, C6, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY

        NoofDets = 0

        'CurY = CurY + TxtHgt - 8
        'p2Font = New Font("Calibri", 12, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821", LMargin + C1 + 10, CurY, 2, C2, p2Font)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " Warping & Sizing Charges ", LMargin + C1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 3

        Common_Procedures.Print_To_PrintDocument(e, "( Textile Manufacturing Services ) ", LMargin + C1 + 10, CurY, 0, 0, pFont)

        NoofDets = NoofDets + 1


        SNo = 0
        SNo = SNo + 1

        CurY = CurY + TxtHgtInc + 2


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
            If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

        Else
            'If (dt2.Rows(0).Item("Mill_Name").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(dt2.Rows(0).Item("Mill_Name").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, SNo, LMargin, CurY, 2, C1, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)


                'If Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString) > 0 Then

                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "NO.OF.BEAMS-" & Trim(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                'End If

                'If Val(prn_HdDt.Rows(0).Item("SetCode_ForSelection").ToString) > 0 Then
                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "SET NO : " & Trim(prn_HdDt.Rows(0).Item("set_no").ToString), LMargin + C1 + 10, CurY, 0, 0, p1Font)
                ''End If
                'NoofDets = NoofDets + 1

            End If
        End If



        If (prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text2").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate2").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If (prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text3").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate3").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If



        ' End If


        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("welding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Welding_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 + C5 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Discount_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc

            If Trim(UCase(prn_HdDt.Rows(0).Item("Discount_Type").ToString)) = "PERCENTAGE" Then
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %"
                V2 = ""

            Else

                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString)
                If Val(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) = Val(Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString) Then
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString
                Else
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.000").ToString
                End If

            End If

            Common_Procedures.Print_To_PrintDocument(e, V1, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, V2, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1

        End If

        NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Tds_Perc_Calc").ToString), "##########0.00")

        RndOff = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt), "##########0.00")
        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
            '    CurY = CurY + TxtHgt + 10
            '    If Val(RndOff) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '        NoofDets = NoofDets + 1
            '    End If
            'End If
            CurY = CurY + TxtHgt + TxtHgtInc + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5, CurY, PageWidth, CurY)
            LnAr(9) = CurY

            'CurY = CurY + TxtHgt + TxtHgtInc - 10
            CurY = CurY + TxtHgt - 10
            p2Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GROSS TOTAL  ", LMargin + C1 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
            ' CurY = CurY + TxtHgt + TxtHgtInc
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  ".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
            'CurY = CurY + TxtHgt + TxtHgtInc
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If


        If Val(prn_HdDt.Rows(0).Item("Tds_Perc_Calc").ToString) > 0 Then
            'CurY = CurY + TxtHgt + TxtHgtInc
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TDS  " & prn_HdDt.Rows(0).Item("Tds_Perc") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tds_Perc_Calc").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        'If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) > 0 Then
        '    'CurY = CurY + TxtHgt + TxtHgtInc
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "IGST  " & prn_HdDt.Rows(0).Item("IGST_Percentage") & " %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        '    NoofDets = NoofDets + 1
        'Else
        '    CurY = CurY + TxtHgt
        '    'CurY = CurY + TxtHgt + TxtHgtInc
        '    Common_Procedures.Print_To_PrintDocument(e, "IGST   %".ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
        '    NoofDets = NoofDets + 1
        'End If

        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt + TxtHgtInc
        Next

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        CurY = CurY + TxtHgt - 10
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, p1Font)
        strHeight = e.Graphics.MeasureString("A", p2Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(7))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + (C3 / 3), CurY, LMargin + C1 + C2 + (C3 / 3), LnAr(7))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3, CurY, LMargin + C1 + C2 + C3, LnAr(7))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, LMargin + C1 + C2 + C3 + C4, LnAr(7))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5, CurY, LMargin + C1 + C2 + C3 + C4 + C5, LnAr(7))

        AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & AmtInWrds, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        'CurY = CurY + 5
        'If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
        'End If

        Erase BnkDetAr
        BankNm1 = "" : BankNm2 = "" : BankNm3 = "" : BankNm4 = ""
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

            NoofItems_PerPage = NoofItems_PerPage + 1

        End If

        'If NoofDets <= 8 Then
        '    For I = NoofDets + 1 To 8
        '        CurY = CurY + TxtHgt + 10
        '        NoofDets = NoofDets + 1
        '    Next
        'End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(BankNm1) <> "" Then
            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm2) <> "" Then
            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm3) <> "" Then
            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        If Trim(BankNm4) <> "" Then
            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
            NoofDets = NoofDets + 1
        End If

        CurY = CurY + TxtHgt + 7
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY
        '=============GST SUMMARY============
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1036" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" Then '---- Kalaimagal Sizing (Avinashi)
        '    Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
        'End If
        '=========================

        CurY = CurY

        p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)

        CurY = CurY + TxtHgt - 20
        Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY, 0, 0, p1Font)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then '---- Ganesh Karthi Sizing 
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "Interest will be charged 24% annum if the payment is not received with in 15 days from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        ''End If
        CurY = CurY + TxtHgt
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
        Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 22% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh Karthi Sizing 
        '    Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged for overdue invoice more than 30 days.", LMargin + 40, CurY, 0, 0, pFont)
        '    'Else
        '    '    Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
        'End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then '---- Ganesh Karthi Sizing 
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY, 0, 0, pFont)

        'End If
        Juris = Common_Procedures.settings.Jurisdiction
        If Trim(Juris) = "" Then Juris = "COIMBATORE"

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Subject " & Juris & " jurisdiction only.", LMargin + 40, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "E & O.E Subject ", PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(13) = CurY

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 700, CurY, 1, 0, p1Font)



        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(14) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))


        e.Graphics.DrawLine(Pens.Black, LMargin + 190, CurY, LMargin + 190, LnAr(13))
        'e.Graphics.DrawLine(Pens.Black, LMargin + 200, CurY, LMargin + 200, LnAr(11))

        e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(13))
        'e.Graphics.DrawLine(Pens.Black, LMargin + 410, CurY, LMargin + 410, LnAr(11))

        e.HasMorePages = False

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count < Len(Trim(prn_InpOpts)) Then


                If Val(prn_InpOpts) <> "0" Then
                    prn_PageNo = 0

                    e.HasMorePages = True
                    Return
                End If

            End If

        End If

    End Sub

    Private Sub Cbo_Tax_Type_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Tax_Type.KeyPress

    End Sub
    Private Sub Cbo_Tax_Type_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Tax_Type.KeyDown

    End Sub

    Private Sub cbo_Transport_Mode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport_Mode.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Invoice_Head", "Transport_Mode", "", "")
    End Sub

    Private Sub cbo_Transport_Mode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport_Mode.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport_Mode, dtp_date, cbo_partyname, "Invoice_Head", "Transport_Mode", "", "")
    End Sub

    Private Sub cbo_Transport_Mode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport_Mode.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport_Mode, cbo_partyname, "Invoice_Head", "Transport_Mode", "", "", False)
    End Sub


    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Invoice_Head", "Vechile_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, Cbo_Tax_Type, cbo_setno, "Invoice_Head", "Vechile_No", "", "")
        'If e.KeyCode = 40 Then
        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
        '        cbo_VendorName.Focus()
        '    Else
        '        cbo_setno.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, cbo_setno, "Invoice_Head", "Vechile_No", "", "", False)
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
        '        cbo_VendorName.Focus()
        '    Else
        '        cbo_setno.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VendorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VendorName, Nothing, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
        If e.KeyCode = 40 And cbo_DelieveryTo.DroppedDown = False And (e.Control = True And e.KeyValue = 40) Then
            If cbo_DelieveryTo.Visible Then
                cbo_DelieveryTo.Focus()
            Else
                cbo_setno.Focus()
            End If
        End If
        If e.KeyCode = 38 And cbo_DelieveryTo.DroppedDown = False Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Vechile.Visible Then
                cbo_Vechile.Focus()
            Else
                Cbo_Tax_Type.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VendorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VendorName, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_DelieveryTo.Visible Then
                cbo_DelieveryTo.Focus()
            Else
                cbo_setno.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_VendorName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Vendor_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_VendorName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_DelieveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelieveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DelieveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelieveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelieveryTo, cbo_VendorName, txt_SizingRate1, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        'If e.KeyCode = 40 And cbo_DelieveryTo.DroppedDown = False Or (e.Control = True And e.KeyValue = 40) Then
        '    If cbo_setno.Visible Then
        '        cbo_setno.Focus()
        '    Else
        '        cbo_OnAccount.Focus()
        '    End If
        'End If
        'If e.KeyCode = 38 And cbo_DelieveryTo.DroppedDown = False Or (e.Control = True And e.KeyValue = 38) Then
        '    If cbo_VendorName.Visible Then
        '        cbo_VendorName.Focus()
        '    Else
        '        cbo_Vechile.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_DelieveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelieveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelieveryTo, txt_SizingRate1, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    If cbo_setno.Visible Then
        '        cbo_setno.Focus()
        '    Else
        '        cbo_OnAccount.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_DelieveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelieveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Delivery_Party_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DelieveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_UserModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
    Private Sub Printing_Format1363_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font, p2Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0, CurY1 As Single, CurY2 As Single
        Dim TxtHgt As Single = 0, TxtHgtInc As Single = 0, strHeight As Single = 0, strWidth As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim W1 As Single, N1 As Single
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single
        Dim AmtInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim NoofDets As Integer = 0, NoofItems_PerPage As Integer = 0
        Dim V1 As String = ""
        Dim V2 As String = ""
        Dim CenLn As Single
        Dim NetAmt As String = 0, RndOff As String = 0
        Dim Juris As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim S As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BnkDetAr() As String
        Dim BInc As Integer
        Dim LnAr(16) As Single
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40  ' 50
            .Right = 50  '50
            .Top = 35
            .Bottom = 50 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            pFont = New Font("Calibri", 11, FontStyle.Regular)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            pFont = New Font("Calibri", 11, FontStyle.Bold)
            'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1363" Then '---- Guru Sizing (Somanur)
            '    pFont = New Font("Times new roman", 11, FontStyle.Bold)
        Else
            pFont = New Font("Calibri", 10, FontStyle.Regular)
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            TxtHgt = 18 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1363" Then
            TxtHgt = 17.5
        Else
            TxtHgt = 18.5 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Avinashi)
            TxtHgtInc = 5.5
            NoofItems_PerPage = 8 '13 ' 15

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1363" Then
            TxtHgtInc = 5.5
            NoofItems_PerPage = 7 '13 ' 15
        Else

            TxtHgtInc = 0
            NoofItems_PerPage = 10
        End If

        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 80 ' 70
        C2 = 330 ' 350
        C3 = 120 ' 105
        C4 = 90
        C5 = PageWidth - (LMargin + C1 + C2 + C3 + C4)

        CenLn = C1 + C2 + (C3 \ 2)
        If (Common_Procedures.settings.CustomerCode = "1363") Then
            CurY = TMargin + 140 - TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Else
            CurY = TMargin
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
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
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt - 0.1, 1, 0, pFont)
        End If



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_panNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
                Cmp_StateNm = Cmp_StateNm & "     " & Cmp_StateCode
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY + 10, 100, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Kalaimagal Sizing (Palladam)
            'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.AADHAVAN, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1284" Then '----- SHREE VEL SIZING (PALLADAM)
            ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_VelSizing_Palladam, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
            ' ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1363" Then '----- somanur sizing 
            '   e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Balaji_textile_venkatachalapathy, Drawing.Image), LMargin + 10, CurY + 10, 90, 100)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1031" And Trim(prn_HdDt.Rows(0).Item("Company_Type").ToString) = "ACCOUNT" Then '---- SRI RAM SIZING

            CurY = CurY + TxtHgt - 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
                p1Font = New Font("Calibri", 22, FontStyle.Bold)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then '---- ATHAVAN TEXS SIZING UNIT (SOMANUR)  or  HARI RAM COTTON SIZING UNIT (SOMANUR)
                p1Font = New Font("Brush Script MT", 30, FontStyle.Bold Or FontStyle.Italic)
            Else
                p1Font = New Font("Calibri", 18, FontStyle.Bold)
            End If


            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & Cmp_CstNo), pFont).Width
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
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & Cmp_CstNo, CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1031" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1363" Then

            CurY = CurY + TxtHgt - 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
                p1Font = New Font("Calibri", 22, FontStyle.Bold)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then '---- ATHAVAN TEXS SIZING UNIT (SOMANUR)  or  HARI RAM COTTON SIZING UNIT (SOMANUR)
                p1Font = New Font("Brush Script MT", 30, FontStyle.Bold Or FontStyle.Italic)
            Else
                p1Font = New Font("Calibri", 18, FontStyle.Bold)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Red)

            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            End If
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            End If


            CurY = CurY + TxtHgt
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            End If


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
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            End If

            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)
            End If


            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            End If

            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & Cmp_CstNo, CurX, CurY, 0, 0, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & Cmp_CstNo, CurX, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
            End If

        End If


        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth, CurY, 1, 0, pFont)
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("pan_no").ToString) <> "" Then
                Led_TinNo = " PAN NO :  " & Trim(prn_HdDt.Rows(0).Item("pan_no").ToString)
            End If


            CurY = CurY + strHeight


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Billed & Shipped To  : ", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 16, FontStyle.Bold)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Then '---- Prakash Sizing (Somanur)
                Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font, Brushes.Red)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
                Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
            End If
            If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
                'Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
                Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
            Else
                'Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
                Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
                Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
                Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
                Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Led_GstNo = "GSTIN :  " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
                End If
                If Trim(prn_HdDt.Rows(0).Item("pan_no").ToString) <> "" Then
                    Led_TinNo = " PAN NO : " & Trim(prn_HdDt.Rows(0).Item("pan_no").ToString)
                End If
            End If
        End If

        'Common_Procedures.Print_To_PrintDocument(e, "JOB WORK BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        ''Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        W1 = e.Graphics.MeasureString("INVOICE NO : ", pFont).Width
        N1 = e.Graphics.MeasureString("To    : ", pFont).Width

        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & Led_Name, LMargin + N1 + 10, CurY - TxtHgt, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 1
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            Inv_No = prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & Trim(Format(Val(Inv_No), "######0000")) & Trim(InvSubNo) & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)

        Else
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)

        End If
        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
        '    Common_Procedures.Print_To_PrintDocument(e, "GST-" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
        '    Common_Procedures.Print_To_PrintDocument(e, "SIZING/" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'End If



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString <> "" Then
            strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, pFont).Width
            Common_Procedures.Print_To_PrintDocument(e, "CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + N1 + 10 + strWidth + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, LMargin + CenLn, LnAr(2))
        LnAr(4) = CurY
        LnAr(5) = CurY


        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "No.of ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Particulars", LMargin + C1, CurY, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Quantity ", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Rate Per", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "Beams ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " ", LMargin, CurY + C1, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "in Kgs", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Kgs", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(Rs)", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        NoofDets = 0

        CurY = CurY + TxtHgt - 8
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821", LMargin + C1 + 10, CurY, 2, C2, p2Font)

        CurY = CurY + TxtHgt - 3
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Warping & Sizing) )", LMargin + C1 + 10, CurY, 2, C2, p1Font)

        NoofDets = NoofDets + 1

        CurY = CurY + TxtHgtInc + 2

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
            If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

        Else
            If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If
            If (prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate2").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If
            If (prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text3").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate3").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1

            End If

        End If



        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("welding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Welding_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Discount_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc

            If Trim(UCase(prn_HdDt.Rows(0).Item("Discount_Type").ToString)) = "PERCENTAGE" Then
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %"
                V2 = ""

            Else

                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString)
                If Val(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) = Val(Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString) Then
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString
                Else
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.000").ToString
                End If

            End If

            Common_Procedures.Print_To_PrintDocument(e, V1, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, V2, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1

        End If

        NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Tds_Perc_Calc").ToString), "##########0.00")

        RndOff = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt), "##########0.00")
        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
            '    CurY = CurY + TxtHgt + 10
            '    If Val(RndOff) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '        NoofDets = NoofDets + 1
            '    End If
            'End If
            CurY = CurY + TxtHgt + TxtHgtInc + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt + TxtHgtInc - 10
            p2Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE VALUE  ", LMargin + C1 + C2 - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  ".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  ", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "IGST  " & prn_HdDt.Rows(0).Item("IGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "IGST            %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If Val(prn_HdDt.Rows(0).Item("Tds_perc_Calc").ToString) <> 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "TDS    " & (prn_HdDt.Rows(0).Item("Tds_perc").ToString) & "%", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tds_perc_Calc").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt + TxtHgtInc
        Next

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If



        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        CurY = CurY + TxtHgt - 10
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
        strHeight = e.Graphics.MeasureString("A", p2Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2, CurY, LMargin + C1 + C2, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3, CurY, LMargin + C1 + C2 + C3, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, LMargin + C1 + C2 + C3 + C4, LnAr(5))

        AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & AmtInWrds, LMargin + 10, CurY, 0, 0, p1Font)

        ' CurY = CurY + TxtHgt + 10
        '  e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY


        If Common_Procedures.settings.CustomerCode <> "1102" And Common_Procedures.settings.CustomerCode <> "1363" Then

            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), "&")

                BInc = -1

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm2 = Trim(BnkDetAr(BInc))
                End If


                NoofItems_PerPage = NoofItems_PerPage + 1

            End If

            'If NoofDets <= 8 Then
            '    For I = NoofDets + 1 To 8
            '        CurY = CurY + TxtHgt + 10
            '        NoofDets = NoofDets + 1
            '    Next
            'End If
            CurY = CurY + 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(BankNm1) <> "" Then
                CurY = CurY + TxtHgt + 5
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm2) <> "" Then
                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If
            'If Common_Procedures.settings.CustomerCode <> "1102" Then
            '    CurY = CurY + 5
            '    If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            '    End If
            'End If
        End If
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY
        '=============GST SUMMARY============
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1036" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1112" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then '---- Kalaimagal Sizing (Avinashi)
            Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
        End If
        '=========================

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(15) = CurY

        CurY1 = CurY
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
        Else
            p1Font = New Font("Calibri", 10, FontStyle.Underline)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY1, 0, 0, p1Font)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1348" And Common_Procedures.settings.CustomerCode <> "1363" Then '---- Ganesh Karthi Sizing 
            CurY1 = CurY1 + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "Kindly send as your payment at the earliest by means of a draft.", LMargin + 40, CurY1, 0, 0, pFont)
        End If
        CurY1 = CurY1 + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 22% will be charged from the date of invoice.", LMargin + 40, CurY1, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Or Common_Procedures.settings.CustomerCode = "1363" Then            '---- Ganesh Karthi Sizing 
            Common_Procedures.Print_To_PrintDocument(e, "1. Appropriate rate of interest @ 24% will be charged", LMargin + 10, CurY1, 0, 0, pFont)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "for overdue invoice more than 30 days.", LMargin + 10, CurY1, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged for overdue invoice more than 30 days.", LMargin + 40, CurY1, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY1, 0, 0, pFont)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1348" And Common_Procedures.settings.CustomerCode <> "1363" Then '---- Ganesh Karthi Sizing 
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY1, 0, 0, pFont)

        End If
        Juris = Common_Procedures.settings.Jurisdiction
        If Trim(Juris) = "" Then Juris = "COIMBATORE"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then
            Juris = "COIMBATORE"
        End If


        CurY1 = CurY1 + TxtHgt
        If Common_Procedures.settings.CustomerCode = "1363" Then
            Common_Procedures.Print_To_PrintDocument(e, "2. subject to " & Juris & " jurisdiction only.", LMargin + 10, CurY1, 0, 0, pFont)
            'ElseIf Common_Procedures.settings.CustomerCode = "1363" Then

            '    Common_Procedures.Print_To_PrintDocument(e, "subject to " & Juris & " jurisdiction only.", LMargin + 20, CurY1, 0, 0, pFont)
        Else

            Common_Procedures.Print_To_PrintDocument(e, "subject to " & Juris & " jurisdiction only.", LMargin + 40, CurY1, 0, 0, pFont)
        End If
        '  e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
            CurY1 = CurY1 + TxtHgt
            p2Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DUE DAYS   :" & Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & "  Days".ToString, LMargin + 40, CurY1, 0, 0, p2Font)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DUE DATE   : " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 40, CurY1, 0, 0, p2Font)
            'If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            'End If
            ' CurY1 = CurY1 + TxtHgt - 10
        End If
        If Common_Procedures.settings.CustomerCode = "1102" Or Common_Procedures.settings.CustomerCode = "1348" Or Common_Procedures.settings.CustomerCode = "1363" Then
            Erase BnkDetAr
            BankNm1 = "" : BankNm2 = "" : BankNm3 = "" : BankNm4 = ""
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

                NoofItems_PerPage = NoofItems_PerPage + 1

            End If

            CurY2 = CurY

            p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 390, CurY2, 0, 0, p1Font)

            If Trim(BankNm1) <> "" Then
                CurY2 = CurY2 + TxtHgt
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm2) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm3) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm4) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

        End If

        If CurY1 > CurY2 Then
            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)
            LnAr(11) = CurY1
        ElseIf CurY2 > CurY1 Then
            CurY2 = CurY2 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY2, PageWidth, CurY2)
            LnAr(11) = CurY2
        Else
            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)
            LnAr(11) = CurY1
        End If

        If CurY1 > CurY2 Then
            CurY = CurY1
        Else
            CurY = CurY2
        End If



        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If

        CurY = CurY + 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then '---- ATHAVAN TEXS SIZING UNIT (SOMANUR)  or  HARI RAM COTTON SIZING UNIT (SOMANUR)
            p1Font = New Font("Brush Script MT", 14, FontStyle.Bold Or FontStyle.Italic)
        Else
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Or Common_Procedures.settings.CustomerCode = "1363" Then
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        End If


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
            e.Graphics.DrawLine(Pens.Black, LMargin + 190, CurY, LMargin + 190, LnAr(11))
        End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
        '    e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
        'End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1074" Or Trim(Common_Procedures.settings.CustomerCode) = "1031" Or Trim(Common_Procedures.settings.CustomerCode) = "1363" Then '-- MITHUN SIZING MILLS,SRI RAM SIZING
            e.Graphics.DrawLine(Pens.Black, LMargin + 380, LnAr(15), LMargin + 380, CurY)
        Else
            'COMMENTED
            If Common_Procedures.settings.CustomerCode = "1351" Then ' MAHAA GHANPATHY SIZING MILL
                e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
                e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
                'Else
                '    e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(15))
            End If
        End If
        e.HasMorePages = False

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count < Len(Trim(prn_InpOpts)) Then


                If Val(prn_InpOpts) <> "0" Then
                    prn_PageNo = 0

                    e.HasMorePages = True
                    Return
                End If

            End If

        End If

    End Sub

    Private Sub btn_EInvoice_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click

        grp_EInvoice.Visible = True
        grp_EInvoice.BringToFront()
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        'grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2
        grp_EInvoice.Top = 3

        btn_CheckConnectivity1.Enabled = False
        btn_CheckConnectivity1.Visible = False
    End Sub

    Private Sub btn_CheckConnectivity1_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity1.Click

        Dim einv As New eInvoice(Val(lbl_company.Tag))
        einv.GetAuthToken(rtbeInvoiceResponse)

        'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

    End Sub

    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Invoice_Head Where InVoice_Code = '" & NewCode & "'"
        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Invoice_Head Where InVoice_Code = '" & NewCode & "' and Len(E_Invoice_IRNO) >0"
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

            Cmd.CommandText = "Delete from e_Invoice_Head  where Ref_Sales_Code = '" & NewCode & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Delete from e_Invoice_Details  where Ref_Sales_Code = '" & NewCode & "'"
            Cmd.ExecuteNonQuery()



            Cmd.CommandText = "Insert into e_Invoice_Head  (e_Invoice_No  , e_Invoice_date          , Buyer_IdNo , Consignee_IdNo , Assessable_Value    , CGST             , SGST            , IGST     , Cess, State_Cess,   Round_Off ,       Nett_Invoice_Value,      Ref_Sales_Code          , Other_Charges     )" &
                                              "Select    Invoice_No  ,     Invoice_Date,       Ledger_IdNo,     DeliveryTo_IdNo,  Assessable_Value , CGST_Amount,      SGST_Amount,    IGST_Amount ,      0  ,    0         , RoundOff_Amount ,    Net_Amount         , '" & Trim(NewCode) & "',    0 from Invoice_Head where InVoice_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""

            Cmd.ExecuteNonQuery()

            'Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                  ,    HSN_Code              ,          Batch_Details     ,                       Quantity,                                   Unit        ,              Unit_Price   ,                     Total_Amount           ,   Discount        , Assessable_Amount  ,                              GST_Rate                                                             , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
            '                                                          " Select  1 as Sl_No  , 1 as IsServc,    a.Sizing_Text1 as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,     ( a.Sizing_Weight1 + a.Sizing_Weight2 + a.Sizing_Weight3 ),     'KGS' as UOM,     a.Sizing_Rate1  ,  (a.Assessable_Value - a.Discount_Amount)  , a.Discount_Amount , a.Assessable_Value ,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
            '                                                          " from  Invoice_Head a  " &
            '                                                          " Where a.InVoice_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()


            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                  ,    HSN_Code              ,     Batch_Details ,        Quantity        ,          Unit  ,       Unit_Price   ,  Total_Amount     ,   Discount        , Assessable_Amount                     ,                              GST_Rate                                                                          , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  1 as Sl_No  , 1 as IsServc,    a.Sizing_Text1 as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,      a.Sizing_Weight1 ,     'KGS' as UOM,     a.Sizing_Rate1 ,   a.Sizing_Amount1, a.Discount_Amount , (a.Sizing_Amount1 - a.Discount_Amount),  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a  " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                  ,    HSN_Code              ,     Batch_Details ,        Quantity        ,          Unit  ,       Unit_Price   ,  Total_Amount     ,   Discount       , Assessable_Amount,                              GST_Rate                                                                          , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  2 as Sl_No  , 1 as IsServc,    a.Sizing_Text2 as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,      a.Sizing_Weight2 ,     'KGS' as UOM,     a.Sizing_Rate2 ,   a.Sizing_Amount2, 0 as Disc_Amount , a.Sizing_Amount2 ,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a  " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "' and a.Sizing_Amount2 <> 0"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                  ,    HSN_Code              ,     Batch_Details ,        Quantity        ,          Unit  ,       Unit_Price   ,  Total_Amount     ,   Discount       , Assessable_Amount,                              GST_Rate                                                                          , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  3 as Sl_No  , 1 as IsServc,    a.Sizing_Text3 as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,      a.Sizing_Weight3 ,     'KGS' as UOM,     a.Sizing_Rate3 ,   a.Sizing_Amount3, 0 as Disc_Amount , a.Sizing_Amount3 ,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a  " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "' and a.Sizing_Amount3 <> 0"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                    ,    HSN_Code              ,     Batch_Details ,        Quantity,          Unit  ,       Unit_Price ,  Total_Amount       ,   Discount       , Assessable_Amount  ,                              GST_Rate                                                                          , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  4 as Sl_No  , 1 as IsServc,    a.SampleSet_Text as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,      0 as qty  ,     'KGS' as UOM,     0 as Rate    ,   a.SampleSet_Amount, 0 as Disc_Amount , a.SampleSet_Amount ,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a  " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "' and a.SampleSet_Amount <> 0"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                    ,    HSN_Code              ,     Batch_Details ,        Quantity,          Unit  ,       Unit_Price ,  Total_Amount       ,   Discount       , Assessable_Amount  ,                              GST_Rate                                                                          , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  5 as Sl_No  , 1 as IsServc,    a.VanRent_Text as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,      0 as qty  ,     'KGS' as UOM,     0 as Rate    ,   a.VanRent_Amount, 0 as Disc_Amount , a.VanRent_Amount,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a  " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "' and a.VanRent_Amount <> 0"
            Cmd.ExecuteNonQuery()


            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                  ,    HSN_Code              ,     Batch_Details ,        Quantity     ,          Unit  ,       Unit_Price   ,  Total_Amount     ,   Discount       , Assessable_Amount,                              GST_Rate                                                                          , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  6 as Sl_No  , 1 as IsServc,    a.Packing_Text as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,      a.Packing_Beam ,     'NOS' as UOM,     a.Packing_Rate ,   a.Packing_Amount, 0 as Disc_Amount , a.Packing_Amount ,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a  " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "' and a.Packing_Amount <> 0"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                  ,    HSN_Code              ,     Batch_Details ,        Quantity     ,          Unit  ,       Unit_Price   ,  Total_Amount     ,   Discount       , Assessable_Amount,                              GST_Rate                                                                          , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  7 as Sl_No  , 1 as IsServc,    a.Rewinding_Text as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,      a.Rewinding_Weight ,     'KGS' as UOM,     a.Rewinding_Rate,   a.Rewinding_Amount, 0 as Disc_Amount , a.Rewinding_Amount,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a  " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "' and a.Rewinding_Amount <> 0"
            Cmd.ExecuteNonQuery()


            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                  ,    HSN_Code              ,     Batch_Details ,        Quantity     ,          Unit  ,       Unit_Price   ,  Total_Amount     ,   Discount       , Assessable_Amount,                              GST_Rate                                                                          , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  8 as Sl_No  , 1 as IsServc,    a.Welding_Text as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,      a.Welding_Beam,     'NOS' as UOM,     a.Welding_Rate,   a.Welding_Amount, 0 as Disc_Amount , a.Welding_Amount,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a  " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "' and a.Welding_Amount <> 0"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,       IsService    ,    Product_Description                    ,    HSN_Code              ,     Batch_Details ,        Quantity,          Unit  ,       Unit_Price ,  Total_Amount       ,   Discount       , Assessable_Amount  ,                              GST_Rate                                                                          , SGST_Amount , IGST_Amount  , CGST_Amount ,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  9 as Sl_No  , 1 as IsServc,    a.OtherCharges_Text as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,      0 as qty  ,     'KGS' as UOM,     0 as Rate    ,   a.OtherCharges_Amount, 0 as Disc_Amount , a.OtherCharges_Amount,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a  " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "' and a.OtherCharges_Amount <> 0"
            Cmd.ExecuteNonQuery()


            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try


        Dim einv As New eInvoice(Val(lbl_company.Tag))
        einv.GenerateIRN(Val(lbl_company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Invoice_Head", "InVoice_Code", Pk_Condition)

    End Sub

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        If Len(Trim(txt_EInvoiceCancellationReson.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Invoice_Head", "InVoice_Code", con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)

    End Sub

    Private Sub btn_Refresh_E_Invoice_Info_Click(sender As Object, e As EventArgs) Handles btn_Get_QR_Code.Click

        'Dim CMD As New SqlClient.SqlCommand
        'CMD.Connection = con

        'CMD.CommandText = "DELETE FROM " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_refresh where IRN = '" & txt_eInvoiceNo.Text & "'"
        'CMD.ExecuteNonQuery()

        'CMD.CommandText = " INSERT INTO " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_Refresh ([IRN] ,[ACK_No] , [DOC_No] , [SEARCH_BY]  , [COMPANY_IDNO],[Update_Table] ,[Update_table_Unique_Code] ) VALUES " &
        '                  "('" & txt_eInvoiceNo.Text & "' ,'','','I'," & Val(Common_Procedures.CompGroupIdNo).ToString & ",'Invoice_Head', 'E_Invoice_IRNO')"
        'CMD.ExecuteNonQuery()

        'Shell(Application.StartupPath & "\Refresh_IRN.EXE")

        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'Dim einv1 As New eInvoice1(Val(lbl_Company.Tag))
        'einv1.RefresheInvoiceInfoByIRN(txt_IR_No.Text, NewCode, Con, rtbeInvoiceResponse, pb_IRNQRC, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "ClothSales_Invoice_Head", "ClothSales_Invoice_Code")



        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim einv As New eInvoice(Val(lbl_company.Tag))
        einv.GetIRNDetails(txt_eInvoiceNo.Text, NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Invoice_Head", "InVoice_Code", "INV")


    End Sub

    Private Sub txt_eInvoiceNo_TextChanged(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.TextChanged
        txt_IR_No.Text = txt_eInvoiceNo.Text
    End Sub

    Private Sub btn_refresh_Click(sender As Object, e As EventArgs) Handles btn_refresh.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select E_Invoice_QR_Image,E_Invoice_IRNO,E_Invoice_ACK_No,E_Invoice_ACK_Date,E_Invoice_Cancelled_Status FROM Invoice_Head WHERE InVoice_Code = '" & NewCode & "'", con)

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

    Private Sub btn_Generate_EWB_Click(sender As Object, e As EventArgs) Handles btn_Generate_EWB.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Invoice_Head Where InVoice_Code = '" & NewCode & "'"
        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Invoice_Head Where InVoice_Code = '" & NewCode & "' and (Len(EWay_Bill_No) >0 or Len(E_Invoice_IRNO) = 0 OR E_Invoice_IRNO IS NULL )"
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


            Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]                ,	[TransID]        ,	[TransMode] ,	[TransDocNo] ,[TransDocDate]  ,	[VehicleNo]  , [Distance],	[VehType]  ,	[TransName]         , [InvCode] )   Select A.E_Invoice_IRNO  ,  t.Ledger_GSTINNo,        '1'    ,        ''   ,   ''     ,       a.Vechile_No     , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  D.Distance ELSE L.Distance END),      'R'    ,  t.Ledger_Mainname     ,'" & NewCode & "' " &
                                                       " from Invoice_Head a INNER JOIN Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D on a.DeliveryTo_IdNo = D.Ledger_IdNo LEFT OUTER JOIN Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Where a.InVoice_Code = '" & NewCode & "'"

            Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try


        Dim einv As New eInvoice(Val(lbl_company.Tag))
        einv.GenerateEWBByIRN(NewCode, rtbeInvoiceResponse, txt_eWayBill_No, txt_EWB_Date, txt_EWB_ValidUpto, con, "Invoice_Head", "InVoice_Code", txt_EWB_Canellation_Reason, txt_EWB_Cancel_Status, Pk_Condition)

        Cmd.CommandText = "DELETE FROM EWB_By_IRN WHERE INVCODE = '" & NewCode & "'"
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub btn_Cancel_EWB_Click(sender As Object, e As EventArgs) Handles btn_Cancel_EWB.Click

        If Len(Trim(txt_EWB_Canellation_Reason.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_company.Tag))

        einv.Cancel_EWB_IRN(NewCode, txt_eWayBill_No.Text, rtbeInvoiceResponse, txt_eInvoice_CancelStatus, con, "Invoice_Head", "InVoice_Code", txt_EWB_Canellation_Reason.Text)

    End Sub

    'Private Sub txt_eWayBill_No_TextChanged(sender As Object, e As EventArgs) Handles txt_eWayBill_No.TextChanged
    '    txt_EWayBillNo.Text = txt_eWayBill_No.Text
    'End Sub
    Private Sub Btn_Qr_Code_Add_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Add.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pic_IRN_QRCode_Image.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub Btn_Qr_Code_Close_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Close.Click
        pic_IRN_QRCode_Image.BackgroundImage = Nothing
    End Sub

    Private Sub cbo_partyname_TextChanged(sender As Object, e As EventArgs) Handles cbo_partyname.TextChanged
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt1 As New DataTable
        'Dim Led_Id As Integer = 0

        'Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)

        'txt_Tds.Text = ""
        'If Led_Id <> 0 Then
        '    da = New SqlClient.SqlDataAdapter("select a.Tds_Perc from Ledger_Head a where a.Ledger_IdNo = " & Str(Val(Led_Id)) & " ", con)
        '    da.Fill(dt1)
        '    If dt1.Rows.Count > 0 Then
        '        txt_Tds.Text = Format(Val(dt1.Rows(0).Item("Tds_Perc").ToString), "########0.00")
        '    End If
        '    dt1.Clear()
        'End If
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
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub txt_InvoicePrefixNo_TextChanged(sender As Object, e As EventArgs) Handles txt_InvoicePrefixNo.TextChanged

    End Sub

    Private Sub cbo_InvoiceSufixNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_InvoiceSufixNo.SelectedIndexChanged

    End Sub

    Private Sub Printing_Format1087_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font, p2Font As Font, p3font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0, CurY1 As Single, CurY2 As Single
        Dim TxtHgt As Single = 0, TxtHgtInc As Single = 0, strHeight As Single = 0, strWidth As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String, Cmp_UAMNO As String
        Dim W1 As Single, N1 As Single
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single
        Dim AmtInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim NoofDets As Integer = 0, NoofItems_PerPage As Integer = 0
        Dim V1 As String = ""
        Dim V2 As String = ""
        Dim CenLn As Single
        Dim NetAmt As String = 0, RndOff As String = 0
        Dim Juris As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim S As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BnkDetAr() As String
        Dim BInc As Integer
        Dim LnAr(16) As Single
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim I As Integer = 0


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40  ' 50
            .Right = 50  '50
            .Top = 35
            .Bottom = 50 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            pFont = New Font("Calibri", 11, FontStyle.Regular)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            pFont = New Font("Calibri", 11, FontStyle.Bold)
        Else
            pFont = New Font("Arial", 8, FontStyle.Regular)
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            TxtHgt = 17.5 '18 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then 'Prakash Sizing 
            TxtHgt = 17.5 '18 ' 19
        Else
            TxtHgt = 18 '18.5 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Avinashi)
            TxtHgtInc = 5.5
            NoofItems_PerPage = 8 '13 ' 15
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            NoofItems_PerPage = 8
        Else
            TxtHgtInc = 0
            NoofItems_PerPage = 10
        End If

        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 80 ' 70
        C2 = 330 ' 350
        C3 = 120 ' 105
        C4 = 90
        C5 = PageWidth - (LMargin + C1 + C2 + C3 + C4)

        CenLn = C1 + C2 + (C3 \ 2)

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
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
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt - 0.1, 1, 0, pFont)
        End If



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_UAMNO = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)

            Cmp_Add1 = prn_HdDt.Rows(0).Item("Sizing_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Sizing_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Sizing_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Sizing_Address4").ToString
            If Trim(prn_HdDt.Rows(0).Item("Sizing_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Sizing_PhoneNo").ToString
            End If

        Else

            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If


        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_panNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
                Cmp_StateNm = Cmp_StateNm & "     " & Cmp_StateCode
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_UAM_No").ToString) <> "" Then
            Cmp_UAMNO = "UDYAM No. : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 110, CurY + 5, 90, 90)

                        End If

                    End Using
                End If
            End If

        End If
        'End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY + 10, 100, 100)
            'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            '    If InStr(1, Trim(UCase(Cmp_Name)), "SRI BHAGAVAN TEXTILES") > 0 Then 'SRI BHAGAVAN TEXTILES - PALLADAM
            '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.SRI_BHAGAVAN_TEX_LOGO, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
            '    Else
            '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
            '    End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Kalaimagal Sizing (Palladam)
            'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.AADHAVAN, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1284" Then '----- SHREE VEL SIZING (PALLADAM)
            '  e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_VelSizing_Palladam, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1363" Then '----- somanur sizing 
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Balaji_textile_venkatachalapathy, Drawing.Image), LMargin + 10, CurY + 10, 90, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '----- Prakash Tex 
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Prakash_logo, Drawing.Image), LMargin + 10, CurY + 10, 90, 90)
        Else
            If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 5, 130, 130)

                            End If

                        End Using

                    End If

                End If

            End If
        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1031" Then

            CurY = CurY + TxtHgt - 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
                p1Font = New Font("Calibri", 22, FontStyle.Bold)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then '---- ATHAVAN TEXS SIZING UNIT (SOMANUR)  or  HARI RAM COTTON SIZING UNIT (SOMANUR)
                p1Font = New Font("Brush Script MT", 30, FontStyle.Bold Or FontStyle.Italic)
            Else
                p1Font = New Font("Americana Std", 20, FontStyle.Bold)
            End If



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Red)

            Else
                Common_Procedures.Print_To_PrintDocument(e, StrConv(Cmp_Name, VbStrConv.ProperCase), LMargin, CurY, 2, PrintWidth, p1Font)
            End If
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight + 1
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            End If


            CurY = CurY + TxtHgt + 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt + 1
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


            CurY = CurY + TxtHgt + 1
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_CstNo), pFont).Width
            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

            p1Font = New Font("Arial", 8, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            End If

            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)
            End If


            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            End If

            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "    " & Cmp_CstNo, CurX, CurY, 0, 0, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "    " & Cmp_CstNo, CurX, CurY, 0, 0, pFont)
            End If



            If Trim(Cmp_UAMNO) <> "" Then
                CurY = CurY + TxtHgt
                p1Font = New Font("Arial", 8, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_UAMNO), LMargin, CurY, 2, PrintWidth, p1Font)
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
                p1Font = New Font("Arial", 8, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

                If Trim(ItmNm2) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
                End If


            End If

        End If


        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth, CurY, 1, 0, pFont)
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("PAn_No").ToString) <> "" Then
                Led_TinNo = " PAN NO :  " & Trim(prn_HdDt.Rows(0).Item("PAn_No").ToString)
            End If


            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Billed & Shipped To  : ", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Arial", 16, FontStyle.Bold)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Sizing (Somanur)
                Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font, Brushes.Red)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then '---- Ganesh karthik Sizing (Somanur)
                Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
            End If
            If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
                'Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
                Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
            Else
                'Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
                Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
                Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
                Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
                Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Led_GstNo = "GSTIN :  " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
                End If
                If Trim(prn_HdDt.Rows(0).Item("pan_no").ToString) <> "" Then
                    Led_TinNo = " PAN NO :  " & Trim(prn_HdDt.Rows(0).Item("PAn_No").ToString)
                End If
            End If
        End If

        'Common_Procedures.Print_To_PrintDocument(e, "JOB WORK BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        ''Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        W1 = e.Graphics.MeasureString("INVOICE NO : ", pFont).Width
        N1 = e.Graphics.MeasureString("To    : ", pFont).Width

        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Else
            p1Font = New Font("Arial", 12, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & Led_Name, LMargin + N1 + 10, CurY - TxtHgt, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 1
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        p1Font = New Font("Arial", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
        '    Inv_No = prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString
        '    InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & Trim(Format(Val(Inv_No), "######0000")) & Trim(InvSubNo) & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)

        'Else
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)

        ''End If

        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
        '    Common_Procedures.Print_To_PrintDocument(e, "GST-" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
        '    Common_Procedures.Print_To_PrintDocument(e, "SIZING/" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'End If



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString <> "" Then
            strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, pFont).Width
            Common_Procedures.Print_To_PrintDocument(e, "CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + N1 + 10 + strWidth + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Led_GstNo & "    " & Led_TinNo, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + CenLn, CurY, LMargin + CenLn, LnAr(2))
        LnAr(4) = CurY
        LnAr(5) = CurY


        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "No.of ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Particulars", LMargin + C1, CurY, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Quantity ", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Rate Per", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "Beams ", LMargin, CurY, 2, C1, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " ", LMargin, CurY + C1, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "in Kgs", LMargin + C1 + C2, CurY, 2, C3, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Kgs", LMargin + C1 + C2 + C3, CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(Rs)", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY



        NoofDets = 0
        If Common_Procedures.settings.CustomerCode <> "1378" Then
            CurY = CurY + TxtHgt - 8
            p2Font = New Font("Arial", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821", LMargin + C1 + 10, CurY, 2, C2, p2Font)
            CurY = CurY + TxtHgt - 3
            p1Font = New Font("Arial", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Warping & Sizing) )", LMargin + C1 + 10, CurY, 2, C2, p1Font)

        End If


        NoofDets = NoofDets + 1

        CurY = CurY + TxtHgtInc + 2

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
            If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If

        Else
            If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                If Common_Procedures.settings.CustomerCode = "1378" Then
                    p2Font = New Font("Arial", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, p2Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If
            If (prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate2").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1
            End If
            If (prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) > 0 Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Text3").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate3").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                NoofDets = NoofDets + 1

            End If

        End If



        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("welding_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Welding_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Welding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sampleset_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vanrent_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Discount_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc

            If Trim(UCase(prn_HdDt.Rows(0).Item("Discount_Type").ToString)) = "PERCENTAGE" Then
                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %"
                V2 = ""

            Else

                V1 = Trim(prn_HdDt.Rows(0).Item("Discount_Text").ToString)
                If Val(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) = Val(Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString) Then
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.00").ToString
                Else
                    V2 = Format(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString), "#########0.000").ToString
                End If

            End If

            Common_Procedures.Print_To_PrintDocument(e, V1, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, V2, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1

        End If

        NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "##########0.00")
        'NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Tds_Perc_calc").ToString), "##########0.00")

        RndOff = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt), "##########0.00")
        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
            '    CurY = CurY + TxtHgt + 10
            '    If Val(RndOff) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '        NoofDets = NoofDets + 1
            '    End If
            'End If
            CurY = CurY + TxtHgt + TxtHgtInc + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt + TxtHgtInc - 10
            p2Font = New Font("Arial", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE VALUE  ", LMargin + C1 + C2 - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  ".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  ", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "IGST  " & prn_HdDt.Rows(0).Item("IGST_Percentage") & " %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "IGST            %".ToString, LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        'If Val(prn_HdDt.Rows(0).Item("Tds_perc_Calc").ToString) <> 0 Then
        '    CurY = CurY + TxtHgt + TxtHgtInc
        '    Common_Procedures.Print_To_PrintDocument(e, "TDS    " & (prn_HdDt.Rows(0).Item("Tds_perc").ToString) & "%", LMargin + C1 + C2 - 10, CurY, 1, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tds_perc_Calc").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        '    NoofDets = NoofDets + 1
        'End If

        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt + TxtHgtInc
        Next

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1166" Then '---- Gomathi Sizing Mill (Vanjipalayam)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        'End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        CurY = CurY + TxtHgt - 10
        p2Font = New Font("Arial", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p2Font)


        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)


        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)

        strHeight = e.Graphics.MeasureString("A", p2Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2, CurY, LMargin + C1 + C2, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3, CurY, LMargin + C1 + C2 + C3, LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, LMargin + C1 + C2 + C3 + C4, LnAr(5))

        AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        p1Font = New Font("Arial", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & AmtInWrds, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY


        If Common_Procedures.settings.CustomerCode <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1374" Then

            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), "&")

                BInc = -1

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm2 = Trim(BnkDetAr(BInc))
                End If


                NoofItems_PerPage = NoofItems_PerPage + 1

            End If

            'If NoofDets <= 8 Then
            '    For I = NoofDets + 1 To 8
            '        CurY = CurY + TxtHgt + 10
            '        NoofDets = NoofDets + 1
            '    Next
            'End If
            CurY = CurY + 5
            p1Font = New Font("Arial", 11, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(BankNm1) <> "" Then
                CurY = CurY + TxtHgt + 5
                p1Font = New Font("Arial", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm2) <> "" Then
                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If
            'If Common_Procedures.settings.CustomerCode <> "1102" Then
            '    CurY = CurY + 5
            '    If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            '    End If
            'End If
        End If
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY
        '=============GST SUMMARY============
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1036" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1112" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then '---- Kalaimagal Sizing (Avinashi)
            Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
        End If
        '=========================

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(15) = CurY

        CurY1 = CurY
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
        Else
            p1Font = New Font("Arial", 10, FontStyle.Underline)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY1, 0, 0, p1Font)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1348" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1374" Then '---- Ganesh Karthi Sizing 
            CurY1 = CurY1 + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "Kindly send as your payment at the earliest by means of a draft.", LMargin + 40, CurY1, 0, 0, pFont)
        End If
        CurY1 = CurY1 + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 22% will be charged from the date of invoice.", LMargin + 40, CurY1, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then            '---- Ganesh Karthi Sizing 
            Common_Procedures.Print_To_PrintDocument(e, "1. Appropriate rate of interest @ 24% will be charged", LMargin + 30, CurY1, 0, 0, pFont)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "for overdue invoice more than 30 days.", LMargin + 30, CurY1, 0, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged for overdue invoice more than 30 days.", LMargin + 40, CurY1, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY1, 0, 0, pFont)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1348" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1374" Then '---- Ganesh Karthi Sizing 
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY1, 0, 0, pFont)

        End If
        Juris = Common_Procedures.settings.Jurisdiction
        If Trim(Juris) = "" Then Juris = "COIMBATORE"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then
            Juris = "COIMBATORE"
        End If


        CurY1 = CurY1 + TxtHgt
        If Common_Procedures.settings.CustomerCode = "1102" Or Common_Procedures.settings.CustomerCode = "1348" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            Common_Procedures.Print_To_PrintDocument(e, "2. subject to " & Juris & " jurisdiction only.", LMargin + 30, CurY1, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "subject to " & Juris & " jurisdiction only.", LMargin + 40, CurY1, 0, 0, pFont)
        End If

        'If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
        'End If

        If Common_Procedures.settings.CustomerCode = "1102" Or Common_Procedures.settings.CustomerCode = "1348" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            Erase BnkDetAr
            BankNm1 = "" : BankNm2 = "" : BankNm3 = "" : BankNm4 = ""
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

                NoofItems_PerPage = NoofItems_PerPage + 1

            End If

            CurY2 = CurY

            p1Font = New Font("Arial", 9, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 390, CurY2, 0, 0, p1Font)

            If Trim(BankNm1) <> "" Then
                CurY2 = CurY2 + TxtHgt
                p1Font = New Font("Arial", 8, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm2) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm3) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

            If Trim(BankNm4) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 390, CurY2, 0, 0, p1Font)
                NoofDets = NoofDets + 1
            End If

        End If

        If CurY1 > CurY2 Then
            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)
            LnAr(11) = CurY1
        ElseIf CurY2 > CurY1 Then
            CurY2 = CurY2 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY2, PageWidth, CurY2)
            LnAr(11) = CurY2
        Else
            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)
            LnAr(11) = CurY1
        End If

        If CurY1 > CurY2 Then
            CurY = CurY1
        Else
            CurY = CurY2
        End If



        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If

        CurY = CurY + 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1348" Then '---- ATHAVAN TEXS SIZING UNIT (SOMANUR)  or  HARI RAM COTTON SIZING UNIT (SOMANUR)
            p1Font = New Font("Brush Script MT", 14, FontStyle.Bold Or FontStyle.Italic)
        ElseIf Common_Procedures.settings.CustomerCode = "1378" Then
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Else

            p1Font = New Font("Arial", 9, FontStyle.Bold)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1374" Then
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        End If


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
            e.Graphics.DrawLine(Pens.Black, LMargin + 190, CurY, LMargin + 190, LnAr(11))
        End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
        '    e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
        'End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1074" Or Trim(Common_Procedures.settings.CustomerCode) = "1031" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then '-- MITHUN SIZING MILLS,SRI RAM SIZING
            e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
        Else
            'COMMENTED
            If Common_Procedures.settings.CustomerCode = "1351" Then ' MAHAA GHANPATHY SIZING MILL
                e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1354" Then
                e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(11))
                'Else
                '    e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(15))
            End If
        End If
        e.HasMorePages = False

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count < Len(Trim(prn_InpOpts)) Then


                If Val(prn_InpOpts) <> "0" Then
                    prn_PageNo = 0

                    e.HasMorePages = True
                    Return
                End If

            End If

        End If

    End Sub


End Class

