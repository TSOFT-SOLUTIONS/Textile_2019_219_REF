Imports System.IO

Public Class Invoice_GST_Format_2
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private No_Calc_Status As Boolean = False
    Private Pk_Condition As String = "GSINV-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Public CHk_Details_Cnt As Integer = 0

    Private Print_PDF_Status As Boolean = False

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Pnl_Details As New DataGridViewTextBoxEditingControl
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


    Private vmskOldText As String = ""
    Private vmskSelStrt As Integer = -1
    Private vmskLrText As String = ""
    Private vmskLrStrt As Integer = -1

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
        No_Calc_Status = False

        txt_Invoice_Prefix_No.Text = ""
        lbl_Invoice_No.Text = ""
        lbl_Invoice_No.ForeColor = Color.Black
        dtp_Date.Text = ""
        msk_Date.SelectedText = 0

        vmskOldText = ""
        vmskSelStrt = -1
        vmskLrText = ""
        vmskLrStrt = -1

        Cbo_Party_Name.Text = ""
        Cbo_Party_Name.Tag = ""
        Cbo_SetNo.Tag = ""
        Cbo_SetNo.Text = ""
        cbo_Invoice_Suffix_No.Text = ""
        cbo_VendorName.Text = ""
        cbo_DelieveryTo.Text = ""
        cbo_OnAccount.Text = ""
        cbo_Transport.Text = ""
        cbo_Vechile.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        txt_CGST_Perc.Text = "2.5"
        txt_SGST_Perc.Text = "2.5"
        txt_IGST_Perc.Text = ""
        lbl_Assessable_Value.Text = ""
        lbl_RoundOff_Amt.Text = ""
        lbl_Net_Amount.Text = ""
        lbl_Gross_Amount.Text = ""
        lbl_Amount_In_Words.Text = "Rupees  :  "
        'cbo_Sizing_Charges_Account.Text = ""
        'cbo_Sizing_Charges_Account.Tag = ""
        cbo_Sizing_Charges_Account.Text = "SALES A/C"

        txt_AddLess_Caption.Text = "Add\Less"
        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

        txt_Freight.Text = ""
        txt_AddLess.Text = ""
        lbl_RoundOff_Amt.Text = ""
        lbl_Net_Amount.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        cbo_Grid_DiscType.Text = ""


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

        chk_TCS_Tax.Checked = False

        lbl_Invoice_Value_Before_TCS.Text = ""
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Pnl_Selection_Details.Rows.Add()
        dgv_Pnl_Selection_Details.Rows.Clear()


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
        Dim msk As MaskedTextBox
        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Black
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msk = Me.ActiveControl
            msk.selectionstart = 1
        End If

        If Me.ActiveControl.Name <> cbo_Grid_DiscType.Name Then
            cbo_Grid_DiscType.Visible = False
        End If


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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Pnl_Selection_Details.CurrentCell) Then dgv_Pnl_Selection_Details.CurrentCell.Selected = False
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

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Invoice_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.InVoice_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_Invoice_Prefix_No.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                lbl_Invoice_No.Text = dt1.Rows(0).Item("Invoice_RefNo").ToString
                cbo_Invoice_Suffix_No.Text = dt1.Rows(0).Item("Invoice_SuffixNo").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Invoice_Date").ToString
                msk_Date.Text = dtp_Date.Text

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                Cbo_Party_Name.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                Cbo_Party_Name.Tag = Trim(Cbo_Party_Name.Text)
                Cbo_SetNo.Text = dt1.Rows(0).Item("SetCode_ForSelection").ToString

                cbo_OnAccount.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("OnAccount_IdNo").ToString))
                cbo_VendorName.Text = Common_Procedures.Vendor_IdNoToName(con, Val(dt1.Rows(0).Item("Vendor_IdNo").ToString))
                cbo_DelieveryTo.Text = Common_Procedures.Delivery_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Transport_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_Sizing_Charges_Account.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Sizing_Charges_Account").ToString))

                Cbo_Tax_Type.Text = dt1.Rows(0).Item("Tax_Type").ToString
                cbo_Vechile.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("Add_Less").ToString), "##########0.00")
                txt_AddLess_Caption.Text = dt1.Rows(0).Item("Add_Less_Caption").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "##########0.00")

                txt_CGST_Perc.Text = Format(Val(dt1.Rows(0).Item("CGST_Percentage").ToString), "########0.00")
                txt_SGST_Perc.Text = Format(Val(dt1.Rows(0).Item("SGST_Percentage").ToString.ToString), "########0.00")
                txt_IGST_Perc.Text = Format(Val(dt1.Rows(0).Item("IGST_Percentage").ToString.ToString), "########0.00")
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("CGST_Amount").ToString), "##########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("SGST_Amount").ToString), "##########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("IGST_Amount").ToString), "##########0.00")

                lbl_Assessable_Value.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "##########0.00")
                lbl_RoundOff_Amt.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00")
                lbl_Net_Amount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "##########0.00")

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


                da2 = New SqlClient.SqlDataAdapter("select a.*, ch.count_Name, Mh.Mill_Name from Invoice_Details a Left outer join Count_Head ch on a.Count_IdNo = ch.count_IdNo Left outer join MIll_Head mh ON a.Mill_IdNo = mh.Mill_IdNo where a.Invoice_Code = '" & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    dgv_Details.Rows.Clear()

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_Details.Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Ref_Date").ToString), "dd-MM-yyyy")
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Set_IdNo").ToString
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ends_IdNo").ToString
                            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("count_Name").ToString
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Mill_Name").ToString
                            .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Warp_Kgs").ToString
                            .Rows(n).Cells(7).Value = dt2.Rows(i).Item("Warp_Rate").ToString
                            .Rows(n).Cells(8).Value = dt2.Rows(i).Item("Warp_Amount").ToString
                            .Rows(n).Cells(9).Value = dt2.Rows(i).Item("Rewinding_Kgs").ToString
                            .Rows(n).Cells(10).Value = dt2.Rows(i).Item("Rewinding_Rate").ToString
                            .Rows(n).Cells(11).Value = dt2.Rows(i).Item("Rewinding_Amount").ToString
                            .Rows(n).Cells(12).Value = dt2.Rows(i).Item("No_Of_Beams").ToString
                            .Rows(n).Cells(13).Value = dt2.Rows(i).Item("Packing_Rate").ToString
                            .Rows(n).Cells(14).Value = dt2.Rows(i).Item("Packing_Amount").ToString
                            .Rows(n).Cells(15).Value = dt2.Rows(i).Item("Winding_Beams").ToString
                            .Rows(n).Cells(16).Value = dt2.Rows(i).Item("Winding_Rate").ToString
                            .Rows(n).Cells(17).Value = dt2.Rows(i).Item("Winding_Amount").ToString
                            .Rows(n).Cells(18).Value = dt2.Rows(i).Item("Other_Charges").ToString
                            .Rows(n).Cells(19).Value = dt2.Rows(i).Item("Discount_Type").ToString
                            .Rows(n).Cells(20).Value = dt2.Rows(i).Item("Discount_Rate").ToString
                            .Rows(n).Cells(21).Value = dt2.Rows(i).Item("Discount_Amount").ToString
                            .Rows(n).Cells(22).Value = dt2.Rows(i).Item("Total_Amount").ToString
                            .Rows(n).Cells(23).Value = dt2.Rows(i).Item("set_Code").ToString
                        Next i



                    End If
                    get_Ledger_TotalSales()
                End With

                NetAmount_Calculation()

            End If

            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NewCode As String = ""
        Dim Nr As Long = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '     If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Invoice_Entry, New_Entry, Me, con, "Invoice_Head", "Invoice_Code", NewCode, "Invoice_Date", "(Invoice_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Receipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr

            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_company.Tag), Trim(NewCode), tr)

            Nr = 0
            cmd.CommandText = "Update Specification_Head set invoice_code = '', invoice_increment = invoice_increment - 1 Where invoice_code = '" & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()
            If Nr = 0 Then
                Throw New ApplicationException("Select Set Details - Mismatch of PartyName and Set Details")
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

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

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

            '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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
                    MessageBox.Show("Select Invoice No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_Invoice_No.Text = Trim(UCase(inpno))

                    SetCdSel = Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_company.Tag))

                    da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.count_name from Specification_Head a, Ledger_Head b, count_head c where a.setcode_forSelection = '" & Trim(SetCdSel) & "' and a.invoice_code = '' and a.Ledger_IdNo = b.Ledger_IdNo and a.Count_IdNo = c.Count_IdNo", con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then

                        Cbo_SetNo.Text = SetCdSel
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Invoice_No.Text))

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Invoice_No.Text))

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
        'Dim SetCdSel As String

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

            lbl_Invoice_No.Text = NewID
            lbl_Invoice_No.ForeColor = Color.Red
            msk_Date.Text = Date.Today.ToShortDateString

            da = New SqlClient.SqlDataAdapter("select top 1 * from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Invoice_RefNo desc", con)
            dt1 = New DataTable
            da.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_Invoice_Prefix_No.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString

                cbo_Invoice_Suffix_No.Text = dt1.Rows(0).Item("Invoice_SuffixNo").ToString

                If Val(dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False

                If IsDBNull(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If

            End If

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then '---- Ganesh karthik Sizing (Somanur)
            '    SetCdSel = Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_company.Tag))

            '    da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.count_name from Specification_Head a, Ledger_Head b, count_head c where a.setcode_forSelection = '" & Trim(SetCdSel) & "' and a.invoice_code = '' and a.Ledger_IdNo = b.Ledger_IdNo and a.Count_IdNo = c.Count_IdNo", con)
            '    dt1 = New DataTable
            '    da.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                '        Cbo_SetNo.Text = SetCdSel
                '        get_Set_Details(SetCdSel)
                'get_RateDetails()
                NetAmount_Calculation()

            End If
            dt1.Clear()
            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            dt.Dispose()
            da.Dispose()

        End Try

        If txt_Invoice_Prefix_No.Enabled And txt_Invoice_Prefix_No.Visible Then txt_Invoice_Prefix_No.Focus()
        msk_Date.SelectionStart = 0

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
        Dim vInvoNo As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim OnAc_id As Integer = 0
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim vSetCd As String
        Dim vSetNo As String
        Dim vSetDte As Date
        Dim VouBil As String = ""
        Dim UserIdNo As Integer = 0
        Dim vPrevRefNo As String = ""
        Dim vPrevRefDte As Date
        Dim vOrdByRefNo As String = ""
        Dim VndrNm_Id As Integer = 0
        Dim LedTo_ID As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim Trspt_IdNo As Integer = 0
        Dim Siz_Charges_Acc As Integer = 0
        Dim vWrp_Amt As Integer = 0
        Dim vRW_Amt As Integer = 0
        Dim vPkng_Amt As Integer = 0
        Dim vWdng_Amt As Integer = 0
        Dim vDisc_Amt As Integer = 0
        Dim vTot_Amt As Integer = 0
        Dim cnt_id As Integer
        Dim mil_id As Integer
        Dim nSetcode As String
        Dim Total_OtherCharges As String = 0
        Dim Total_RewindingCharges As String = 0
        Dim vEInvAckDate As String = ""

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        UserIdNo = Common_Procedures.User.IdNo

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Invoice_Entry, New_Entry, Me, con, "Invoice_Head", "Invoice_Code", NewCode, "Invoice_Date", "(Invoice_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Invoice_RefNo desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows!...", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_company.Tag) = 0 Then
            MessageBox.Show("Select Company Selection!...", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Enter Valid Date!.....", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of Financial Range!....", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Party_Name.Text)

        VndrNm_Id = Common_Procedures.Vendor_AlaisNameToIdNo(con, cbo_VendorName.Text)
        LedTo_ID = Common_Procedures.Delivery_AlaisNameToIdNo(con, cbo_DelieveryTo.Text)
        Trspt_IdNo = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)
        Siz_Charges_Acc = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing_Charges_Account.Text)

        For i = 0 To dgv_Details.Rows.Count - 1

            cnt_id = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value)
            mil_id = Common_Procedures.Mill_NameToIdNo(con, dgv_Details.Rows(i).Cells(5).Value)

            Total_OtherCharges = Format(Val(Total_OtherCharges) + Val(dgv_Details.Rows(i).Cells(18).Value), "##########0.000")
            Total_RewindingCharges = Format(Val(Total_RewindingCharges) + Val(dgv_Details.Rows(i).Cells(11).Value), "#########0.00")
        Next

        If led_id = 0 Then
            MessageBox.Show("Select Ledger Name!....", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If Cbo_Party_Name.Enabled Then Cbo_Party_Name.Focus()
            Exit Sub
        End If

        'If Trim(Cbo_SetNo.Text) = "" Then
        '    MessageBox.Show("Select Set No!....", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If Cbo_SetNo.Enabled Then Cbo_SetNo.Focus()
        '    Exit Sub
        'End If


        vSetCd = ""
        vSetNo = ""
        vSetDte = #1/1/2000#
        da = New SqlClient.SqlDataAdapter("select * from Specification_Head where setcode_forSelection = '" & Trim(Cbo_SetNo.Text) & "'", con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            vSetCd = dt1.Rows(0).Item("Set_Code").ToString
            vSetNo = dt1.Rows(0).Item("set_no").ToString
            vSetDte = dt1.Rows(0).Item("set_date")

            If DateDiff(DateInterval.Day, vSetDte, dtp_Date.Value.Date) < 0 Then
                MessageBox.Show("Invoice Invocie Date - Should not less than Set Date (" & vSetDte.ToShortDateString & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()
                Exit Sub
            End If

        End If
        dt1.Clear()

        vPrevRefNo = ""
        vPrevRefDte = #1/1/2000#
        vOrdByRefNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Invoice_No.Text))
        da = New SqlClient.SqlDataAdapter("select top 1 * from Invoice_Head where for_orderby < " & Str(Val(vOrdByRefNo)) & " and company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Invoice_RefNo desc", con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            vPrevRefNo = dt1.Rows(0).Item("Invoice_RefNo").ToString
            vPrevRefDte = dt1.Rows(0).Item("Invoice_Date")

            If DateDiff(DateInterval.Day, vPrevRefDte, dtp_Date.Value.Date) < 0 Then
                MessageBox.Show("Invoice Date - Invoice Date Should not less than Previous Invocie Date " & Chr(13) & "(Invocie No : " & Trim(vPrevRefNo) & "     Invocie Date : " & vPrevRefDte.ToShortDateString & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()
                Exit Sub
            End If

        End If
        dt1.Clear()

        vPrevRefNo = ""
        vPrevRefDte = #1/1/2000#
        vOrdByRefNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Invoice_No.Text))
        da = New SqlClient.SqlDataAdapter("select top 1 * from Invoice_Head where for_orderby > " & Str(Val(vOrdByRefNo)) & " and company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Invoice_RefNo", con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            vPrevRefNo = dt1.Rows(0).Item("Invoice_RefNo").ToString
            vPrevRefDte = dt1.Rows(0).Item("Invoice_Date")

            If DateDiff(DateInterval.Day, vPrevRefDte, dtp_Date.Value.Date) > 0 Then
                MessageBox.Show("Invoice Date - Invocie Date Should not greater than next Invocie Date " & Chr(13) & "(Invocie No : " & Trim(vPrevRefNo) & "     Invocie Date : " & vPrevRefDte.ToShortDateString & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()
                Exit Sub
            End If

        End If
        dt1.Clear()


        If Val(lbl_Net_Amount.Text) = 0 Then lbl_Net_Amount.Text = "0.00"
        If Val(lbl_CGST_Amount.Text) = 0 Then lbl_CGST_Amount.Text = "0.00"
        If Val(lbl_SGST_Amount.Text) = 0 Then lbl_SGST_Amount.Text = "0.00"
        If Val(lbl_IGST_Amount.Text) = 0 Then lbl_IGST_Amount.Text = "0.00"

        No_Calc_Status = False
        'NetAmount_Calculation()

        'With dgv_Total_Details
        'If dgv_Total_Details.Rows.Count > 0 Then
        'Total_OtherCharges = Val(.Rows(0).Cells(18).Value)
        '        vRW_Amt = Val(.Rows(0).Cells(11).Value())
        '        vPkng_Amt = Val(.Rows(0).Cells(14).Value())
        '        vWdng_Amt = Val(.Rows(0).Cells(17).Value())
        '        vDisc_Amt = Val(.Rows(0).Cells(21).Value())
        '        vTot_Amt = Val(.Rows(0).Cells(22).Value())
        '  End If
        '  End With


        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(msk_Date.Text))

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

        Dim vTCS_AssVal_EditSTS As Integer = 0
        Dim vTCS_Tax_Sts As Integer = 0
        Dim vTCSAmtRndOff_STS As Integer = 0


        vTCS_AssVal_EditSTS = 0
        If txt_TCS_TaxableValue.Enabled = True Then vTCS_AssVal_EditSTS = 1

        vTCS_Tax_Sts = 0
        If chk_TCS_Tax.Checked = True Then vTCS_Tax_Sts = 1

        vTCSAmtRndOff_STS = 0
        If chk_TCSAmount_RoundOff_STS.Checked = True Then vTCSAmtRndOff_STS = 1

        OnAc_id = 0 'Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_OnAccount.Text)


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Else
                lbl_Invoice_No.Text = Common_Procedures.get_MaxCode(con, "Invoice_Head", "Invoice_Code", "For_OrderBy", "(Invoice_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_company.Tag), Common_Procedures.FnYearCode, tr)
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            vInvoNo = Trim(txt_Invoice_Prefix_No.Text) & Trim(lbl_Invoice_No.Text) & Trim(cbo_Invoice_Suffix_No.Text)

            cmd.Connection = con
            cmd.Transaction = tr



            If New_Entry = True Then

                cmd.CommandText = "Insert into Invoice_Head ( User_IdNo            ,    Invoice_Code       ,               Company_IdNo        ,                Invoice_SuffixNo            ,           Invoice_RefNo             ,         Invoice_No      ,                               for_OrderBy                                    ,                     Invoice_PrefixNo              , Invoice_Date ,      Ledger_IdNo   ,        SetCode_ForSelection   ,          Set_Code     ,          Set_No       ,     OnAccount_IdNo  ,               Tax_Type            ,             Transport_Mode        ,          Vehicle_No             ,  Vendor_IdNo           ,   DeliveryTo_IdNo    ,              AddLess_Caption            ,            Add_Less          ,            Freight           ,            Assessable_Value           ,           CGST_Percentage      ,             CGST_Amount          ,            SGST_Percentage     ,             SGST_Amount          ,            IGST_Percentage     ,            IGST_Amount           ,             Net_Amount        , add_Less_Caption                         ,        Total_OtherCharges      , Sizing_Charges_Account       ,               Total_RewindingCharges      ,     RoundOff_Amount            ,          E_Invoice_IRNO     ,   E_Invoice_QR_Image  ,          Tcs_Name_caption           ,              Tcs_percentage       ,                Tcs_Amount    ,               TCS_Taxable_Value,                  EDIT_TCS_TaxableValue ,                       Tcs_Tax_Status,                  TCSAmount_RoundOff_Status,         Invoice_Value_Before_TCS ,                              RoundOff_Invoice_Value_Before_TCS ) " &
                                  "Values                   (" & Str(UserIdNo) & " ,'" & Trim(NewCode) & "', " & Str(Val(lbl_company.Tag)) & " , '" & Trim(cbo_Invoice_Suffix_No.Text) & "' , '" & Trim(lbl_Invoice_No.Text) & "' , '" & Trim(vInvoNo) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Invoice_No.Text))) & " , '" & Trim(UCase(txt_Invoice_Prefix_No.Text)) & "' , @InvoiceDate , " & Val(led_id) & ", '" & Trim(Cbo_SetNo.Text) & "', '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "', " & Val(OnAc_id) & ", '" & Trim(Cbo_Tax_Type.Text) & "' ,'" & Trim(cbo_Transport.Text) & "' ,'" & Trim(cbo_Vechile.Text) & "' , " & Val(VndrNm_Id) & " , " & Val(LedTo_ID) & ",'" & Trim(txt_AddLess_Caption.Text) & "' ," & Val(txt_AddLess.Text) & " ," & Val(txt_Freight.Text) & " ," & Val(lbl_Assessable_Value.Text) & " ," & Val(txt_CGST_Perc.Text) & " ," & Val(lbl_CGST_Amount.Text) & " ," & Val(txt_SGST_Perc.Text) & " ," & Val(lbl_SGST_Amount.Text) & " ," & Val(txt_IGST_Perc.Text) & " ," & Val(lbl_IGST_Amount.Text) & " ," & Val(lbl_Net_Amount.Text) & ", '" & Trim(txt_AddLess_Caption.Text) & "', " & Val(Total_OtherCharges) & ", " & Val(Siz_Charges_Acc) & " ,  " & Str(Val(Total_RewindingCharges)) & " ," & Val(lbl_RoundOff_Amt.Text) & " ,      '" & Trim(txt_IR_No.Text) & "' ,     @QrCode , '" & Trim(txt_Tcs_Name.Text) & "', " & Str(Val(txt_TcsPerc.Text)) & ", " & Str(Val(lbl_TcsAmount.Text)) & " , " & Str(Val(txt_TCS_TaxableValue.Text)) & ", " & Str(Val(vTCS_AssVal_EditSTS)) & ", " & Str(Val(vTCS_Tax_Sts)) & ", " & Str(Val(vTCSAmtRndOff_STS)) & ", " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Invoice_Head set Invoice_Date = @InvoiceDate , User_IdNo = " & Str(UserIdNo) & " , Ledger_IdNo = " & Val(led_id) & " , Invoice_SuffixNo  = '" & Trim(cbo_Invoice_Suffix_No.Text) & "', Invoice_No = '" & Trim(vInvoNo) & "', Invoice_RefNo =  '" & Trim(lbl_Invoice_No.Text) & "' , Invoice_PrefixNo = '" & Trim(UCase(txt_Invoice_Prefix_No.Text)) & "' ,SetCode_ForSelection = '" & Trim(Cbo_SetNo.Text) & "', Set_Code ='" & Trim(vSetCd) & "', Set_No = '" & Trim(vSetNo) & "', OnAccount_IdNo = " & Val(OnAc_id) & " , Tax_Type =  '" & Trim(Cbo_Tax_Type.Text) & "' , Transport_IdNo = '" & Trim(Trspt_IdNo) & "', Vehicle_No = '" & Trim(cbo_Vechile.Text) & "' , Vendor_IdNo = " & Val(VndrNm_Id) & " , DeliveryTo_IdNo = " & Val(LedTo_ID) & " ,AddLess_Caption = '" & Trim(txt_AddLess_Caption.Text) & "' ,Add_Less = " & Val(txt_AddLess.Text) & " , Freight = " & Val(txt_Freight.Text) & " , Assessable_Value = " & Val(lbl_Assessable_Value.Text) & " , CGST_Percentage = " & Val(txt_CGST_Perc.Text) & " , CGST_Amount = " & Val(lbl_CGST_Amount.Text) & " , SGST_Percentage = " & Val(txt_SGST_Perc.Text) & " , SGST_Amount = " & Val(lbl_SGST_Amount.Text) & " , IGST_Percentage = " & Val(txt_IGST_Perc.Text) & " , IGST_Amount = " & Val(lbl_IGST_Amount.Text) & " , Net_Amount = " & Val(lbl_Net_Amount.Text) & ", add_Less_Caption = '" & Trim(txt_AddLess_Caption.Text) & "', Total_OtherCharges = " & Val(Total_OtherCharges) & ", Sizing_Charges_Account =  " & Val(Siz_Charges_Acc) & ",Total_RewindingCharges = " & Str(Val(Total_RewindingCharges)) & " , RoundOff_Amount = " & Val(lbl_RoundOff_Amt.Text) & " , E_Invoice_IRNO = '" & Trim(txt_IR_No.Text) & "' , E_Invoice_QR_Image =  @QrCode , E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & " , E_Invoice_Cancelled_Status = " & eiCancel.ToString & " ,  E_Invoice_Cancellation_Reason = '" & txt_EInvoiceCancellationReson.Text & "'  ,    EWB_No = '' , EWB_Date = '" & txt_EWB_Date.Text & "',EWB_Valid_Upto = '" & txt_EWB_ValidUpto.Text & "',EWB_Cancelled = " & EWBCancel.ToString & " ,  EWBCancellation_Reason = '" & txt_EWB_Canellation_Reason.Text & "' , Tcs_Name_caption = '" & Trim(txt_Tcs_Name.Text) & "', Tcs_percentage=" & Str(Val(txt_TcsPerc.Text)) & ",Tcs_Amount= " & Str(Val(lbl_TcsAmount.Text)) & " , TCS_Taxable_Value = " & Str(Val(txt_TCS_TaxableValue.Text)) & ", EDIT_TCS_TaxableValue = " & Str(Val(vTCS_AssVal_EditSTS)) & " , Tcs_Tax_Status = " & Str(Val(vTCS_Tax_Sts)) & " , TCSAmount_RoundOff_Status = " & Str(Val(vTCSAmtRndOff_STS)) & " , Invoice_Value_Before_TCS = " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", RoundOff_Invoice_Value_Before_TCS = " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & " Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Specification_Head set   invoice_code = '', invoice_increment = invoice_increment - 1 Where invoice_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If

            cmd.CommandText = "DELETE FROM Invoice_Details WHERE Company_IdNo = " & Val(lbl_company.Tag) & " AND Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()




            With dgv_Details

                SNo = 0

                For i = 0 To .Rows.Count - 1

                    SNo = SNo + 1

                    If .Rows(i).Cells(7).Value > 0 Then

                        cnt_id = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)
                        mil_id = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(5).Value, tr)



                        nSetcode = Trim(.Rows(i).Cells(23).Value)

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(msk_Date.Text))
                        cmd.Parameters.AddWithValue("@RefDate", Convert.ToDateTime(.Rows(i).Cells(1).Value))

                        cmd.CommandText = "INSERT INTO Invoice_Details (                Company_IdNo      ,   Invoice_Code         ,               Invoice_No            ,                           for_OrderBy                                        ,  Invoice_Date   ,        Sl_No          ,  Ref_Date ,                Set_IdNo                 ,            Ends_IdNo                     ,             Count_IdNo   ,     Mill_IdNo           ,                       Warp_Kgs           ,                    Warp_Rate              ,                Warp_Amount                ,                    Rewinding_Kgs          ,            Rewinding_Rate                ,                 Rewinding_Amount           ,                No_Of_Beams                ,                   Packing_Rate            ,                Packing_Amount              ,                   Winding_Beams            ,                  Winding_Rate              ,                         Winding_Amount     ,                Other_Charges               ,                    Discount_Type         ,            Discount_Rate                   ,                 Discount_Amount            ,                 Total_Amount                   ,                set_Code  ) " &
                                          "VALUES                      (" & Str(Val(lbl_company.Tag)) & " ,'" & Trim(NewCode) & "' , '" & Trim(lbl_Invoice_No.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Invoice_No.Text))) & " ,   @InvoiceDate  , " & Str(Val(SNo)) & " , @RefDate  , '" & Trim(.Rows(i).Cells(2).Value) & "' , '" & Trim(.Rows(i).Cells(3).Value) & "'  , " & Str(Val(cnt_id)) & "  ," & Str(Val(mil_id)) & " ," & Str(Val(.Rows(i).Cells(6).Value)) & " , " & Str(Val(.Rows(i).Cells(7).Value)) & " , " & Str(Val(.Rows(i).Cells(8).Value)) & " , " & Str(Val(.Rows(i).Cells(9).Value)) & " ," & Str(Val(.Rows(i).Cells(10).Value)) & " ," & Str(Val(.Rows(i).Cells(11).Value)) & " ," & Str(Val(.Rows(i).Cells(12).Value)) & " ," & Str(Val(.Rows(i).Cells(13).Value)) & " ," & Str(Val(.Rows(i).Cells(14).Value)) & "  , " & Str(Val(.Rows(i).Cells(15).Value)) & " , " & Str(Val(.Rows(i).Cells(16).Value)) & " , " & Str(Val(.Rows(i).Cells(17).Value)) & " , " & Str(Val(.Rows(i).Cells(18).Value)) & " , '" & Trim(.Rows(i).Cells(19).Value) & "' , " & Str(Val(.Rows(i).Cells(20).Value)) & " , " & Str(Val(.Rows(i).Cells(21).Value)) & " , " & Str(Val(.Rows(i).Cells(22).Value)) & ", '" & Trim(nSetcode) & "'     ) "
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "Update Specification_Head set invoice_code = '" & Trim(NewCode) & "', invoice_increment = invoice_increment + 1 Where invoice_code = '' and set_Code = '" & Trim(nSetcode) & "' and Ledger_IdNo = " & Str(Val(led_id) & " ")
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            Cr_ID = Siz_Charges_Acc

            If Val(OnAc_id) <> 0 Then
                Dr_ID = Val(OnAc_id)
            Else
                Dr_ID = Val(led_id)
            End If


            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            Dim vNetAmt As String = Format(Val(CSng(lbl_Net_Amount.Text)), "#############0.00")
            Dim vCGSTAmt As String = Format(Val(CSng(lbl_CGST_Amount.Text)), "#############0.00")
            Dim vSGSTAmt As String = Format(Val(CSng(lbl_SGST_Amount.Text)), "#############0.00")
            Dim vIGSTAmt As String = Format(Val(CSng(lbl_IGST_Amount.Text)), "#############0.00")

            '---GST
            Dim vGSTPerc As String = ""
            Dim vCGST_AcIdNo As String = ""
            Dim vSGST_AcIdNo As String = ""
            Dim vIGST_AcIdNo As String = ""

            If Val(vIGSTAmt) <> 0 Then
                vGSTPerc = Val(txt_IGST_Perc.Text)
            Else
                vGSTPerc = (Val(txt_CGST_Perc.Text) + Val(txt_SGST_Perc.Text))
            End If

            vCGST_AcIdNo = Common_Procedures.get_FieldValue(con, "GST_AccountSettings_Head", "OP_CGST_Ac_IdNo", "(GST_Percentage = " & Str(Val(vGSTPerc)) & ")", , tr)
            vSGST_AcIdNo = Common_Procedures.get_FieldValue(con, "GST_AccountSettings_Head", "OP_SGST_Ac_IdNo", "(GST_Percentage = " & Str(Val(vGSTPerc)) & ")", , tr)
            vIGST_AcIdNo = Common_Procedures.get_FieldValue(con, "GST_AccountSettings_Head", "OP_IGST_Ac_IdNo", "(GST_Percentage = " & Str(Val(vGSTPerc)) & ")", , tr)

            If Val(vCGST_AcIdNo) = 0 Then vCGST_AcIdNo = 24
            If Val(vSGST_AcIdNo) = 0 Then vSGST_AcIdNo = 25
            If Val(vIGST_AcIdNo) = 0 Then vIGST_AcIdNo = 26

            vLed_IdNos = Dr_ID & "|" & Cr_ID & "|" & Trim(Val(vCGST_AcIdNo)) & "|" & Trim(Val(vSGST_AcIdNo)) & "|" & Trim(Val(vIGST_AcIdNo)) & "|30"

            vVou_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - (Val(vCGSTAmt) + Val(vSGSTAmt) + Val(vIGSTAmt) + Val(lbl_RoundOff_Amt.Text)) & "|" & Val(vCGSTAmt) & "|" & Val(vSGSTAmt) & "|" & Val(vIGSTAmt) & "|" & Val(lbl_RoundOff_Amt.Text)

            If Common_Procedures.Voucher_Updation(con, "Gst.Siz.Inv", Val(lbl_company.Tag), Trim(NewCode), Trim(vInvoNo), Convert.ToDateTime(dtp_Date.Text), "Bill.No : " & Trim(vInvoNo) & "", vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If



            '---Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_company.Tag), dtp_Date.Text, led_id, Trim(vInvoNo), 0, Val(CSng(lbl_Net_Amount.Text)), "DR", Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            If Val(Common_Procedures.User.IdNo) = 1 Then
                If chk_Printed.Visible = True Then
                    If chk_Printed.Enabled = True Then
                        Update_PrintOut_Status(tr)
                    End If
                End If
            End If

            tr.Commit()

            dt1.Dispose()
            da.Dispose()
            tr.Dispose()
            cmd.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            'If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
            'If New_Entry = True Then
            'new_record()
            'Else
            move_record(lbl_Invoice_No.Text)
            'End If
            'Else

            'move_record(lbl_Invoice_No.Text)
            'End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally



            If Cbo_Party_Name.Enabled And Cbo_Party_Name.Visible Then Cbo_Party_Name.Focus()


        End Try


    End Sub

    Private Sub Invoice_GST_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Party_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Party_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
        Me.Top = Me.Top - 30

        con.Open()

        cbo_Invoice_Suffix_No.Items.Clear()
        cbo_Invoice_Suffix_No.Items.Add("")
        cbo_Invoice_Suffix_No.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_Invoice_Suffix_No.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))

        Cbo_Tax_Type.Items.Clear()
        Cbo_Tax_Type.Items.Add("")
        Cbo_Tax_Type.Items.Add("GST")
        Cbo_Tax_Type.Items.Add("NO TAX")

        cbo_Grid_DiscType.Items.Clear()
        cbo_Grid_DiscType.Items.Add("")
        cbo_Grid_DiscType.Items.Add("%")
        cbo_Grid_DiscType.Items.Add("KG")



        txt_AddLess_Caption.Text = "Add\Less"

        Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Da.Fill(Dt1)
        Cbo_Party_Name.DataSource = Dt1
        Cbo_Party_Name.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select setcode_forSelection from Specification_Head where invoice_code = '' order by setcode_forSelection", con)
        Da.Fill(Dt2)
        Cbo_SetNo.DataSource = Dt2
        Cbo_SetNo.DisplayMember = "setcode_forSelection"


        Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or a.Ledger_IdNo = 1 or b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Da.Fill(Dt3)
        cbo_OnAccount.DataSource = Dt3
        cbo_OnAccount.DisplayMember = "Ledger_DisplayName"

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        'btn_UserModification.Visible = False
        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            'btn_UserModification.Visible = True
            chk_Printed.Enabled = True
        End If







        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Invoice_Prefix_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Invoice_Suffix_No.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Party_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OnAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VendorName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelieveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Tax_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_Caption.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_DiscType.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_OnAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_Charges_Account.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Tcs_Name.Enter, AddressOf ControlGotFocus
        AddHandler txt_TcsPerc.Enter, AddressOf ControlGotFocus
        AddHandler txt_TCS_TaxableValue.Enter, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Invoice_Prefix_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Invoice_Suffix_No.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Party_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OnAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VendorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelieveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Tax_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGST_Perc.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_OnAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_DiscType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_Charges_Account.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tcs_Name.Leave, AddressOf ControlLostFocus
        AddHandler txt_TcsPerc.Leave, AddressOf ControlLostFocus
        AddHandler txt_TCS_TaxableValue.Leave, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_IGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler cbo_Sizing_Charges_Account.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Tcs_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TcsPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TCS_TaxableValue.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_IGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler cbo_Sizing_Charges_Account.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_TcsPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TCS_TaxableValue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tcs_Name.KeyPress, AddressOf TextBoxControlKeyPress

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

    Private Sub cbo_partyname_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Party_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        Cbo_Party_Name.Tag = Trim(Cbo_Party_Name.Text)
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Party_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Party_Name, msk_Date, Cbo_Tax_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Party_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Party_Name, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Statement :", "FOR Statement SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            End If
            If Trim(UCase(Cbo_Party_Name.Tag)) <> Trim(UCase(Cbo_Party_Name.Text)) Then
                Cbo_Party_Name.Tag = Trim(Cbo_Party_Name.Text)
                'get_RateDetails()
            End If
            NetAmount_Calculation()
            Cbo_Tax_Type.Focus()

            get_Ledger_TotalSales()
        End If
    End Sub

    Private Sub cbo_OnAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OnAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")
    End Sub

    Private Sub cbo_onAccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OnAccount, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")
        If e.KeyCode = 38 And cbo_OnAccount.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
            If cbo_DelieveryTo.Visible = True Then
                cbo_DelieveryTo.Focus()
            Else
                Cbo_SetNo.Focus()
            End If
        End If

        If e.KeyCode = 40 And cbo_OnAccount.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If dgv_Details.Visible = True Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Freight.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_onAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OnAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OnAccount, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10  or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Freight.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Party_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Party_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_OnAccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_OnAccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_selection.Click
        Dim SetCdSel As String = ""
        pnl_Selection.Visible = True
        pnl_back.Enabled = False
        get_set_Details(SetCdSel)
        'save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub cbo_setno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_SetNo.GotFocus
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try
            Cbo_SetNo.Tag = Cbo_SetNo.Text
            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Party_Name.Text)

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

    Private Sub cbo_setno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_SetNo.KeyDown
        Dim Led_ID As Integer = 0
        Dim Condt As String = ""
        Dim NewCode As String = ""

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Party_Name.Text)

            Condt = ""
            If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
                Condt = " Company_IdNo = " & Str(Val(lbl_company.Tag))
            End If

            If Val(Led_ID) <> 0 Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " ( ( Ledger_IdNo = 0 or Ledger_IdNo = " & Str(Val(Led_ID)) & " ) and (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') )"
            Else
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " (invoice_code = '' or invoice_code = '" & Trim(NewCode) & "') "
            End If

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_SetNo, Nothing, Nothing, "Specification_Head", "setcode_forSelection", "(" & Condt & ")", "(set_code = '')")

            If e.KeyCode = 38 And Cbo_SetNo.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
                If cbo_VendorName.Visible = True Then
                    cbo_VendorName.Focus()
                Else
                    Cbo_Tax_Type.Focus()
                End If
            End If

            If e.KeyCode = 40 And Cbo_SetNo.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
                If cbo_DelieveryTo.Visible = True Then
                    cbo_DelieveryTo.Focus()
                Else
                    cbo_OnAccount.Focus()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_setno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_SetNo.KeyPress
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Party_Name.Text)

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

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_SetNo, Nothing, "Specification_Head", "setcode_forSelection", "(" & Condt & ")", "(set_code = '')")

            If Asc(e.KeyChar) = 13 Then
                If cbo_DelieveryTo.Visible = True Then
                    cbo_DelieveryTo.Focus()
                Else
                    cbo_OnAccount.Focus()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub txt_sizing1rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
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

    Private Sub get_Set_Details_Multiple(ByVal SelcSetCd As String)
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
        Dim vInterStateStatus As Boolean = False
        New_Code = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'cbo_partyname.Text = ""
        'cbo_setno.Text = ""

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Count_Name,c.Count_Gst_Perc from Specification_Head a  Left Outer Join Ledger_Rate_Details d ON  a.Ledger_IdNo = D.Ledger_IdNo, Ledger_Head b, count_head c where a.setcode_forSelection = '" & Trim(SelcSetCd) & "' and (a.invoice_code = '' or a.invoice_code = '" & Trim(New_Code) & "') and a.Ledger_IdNo = b.Ledger_IdNo and a.Count_IdNo = c.Count_IdNo    ", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            Cbo_Party_Name.Text = Dt1.Rows(0).Item("Ledger_Name").ToString

            If Val(Common_Procedures.settings.InvoiceEntry_Set_SetDate_To_InvoiceDate) = 1 Then
                dtp_Date.Text = Dt1.Rows(0).Item("Set_Date").ToString
            End If

            eds = Split(Dt1.Rows(0).Item("ends_name").ToString, ",")

            wwg = Split(Dt1.Rows(0).Item("warp_weight").ToString, ",")

            WarpWgt = 0
            If UBound(wwg) >= 0 Then WarpWgt = WarpWgt + Val(wwg(0))
            If UBound(wwg) >= 1 Then WarpWgt = WarpWgt + Val(wwg(1))
            If UBound(wwg) >= 2 Then WarpWgt = WarpWgt + Val(wwg(2))


            End_Id = Val(Dt1.Rows(0).Item("ends_name").ToString)
            Cnt_Id = Val(Dt1.Rows(0).Item("Count_Idno").ToString)

            'If Trim(UCase(Cbo_Tax_Type.Text)) = "GST" Then
            '    lbl_CGSTPerc.Text = ""
            '    lbl_SGSTPerc.Text = ""
            '    lbl_IGst_Amount.Text = ""
            '    Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)
            '    vInterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_company.Tag), Led_Id)

            '    If vInterStateStatus = True Then
            '        lbl_IGSTPerc.Text = Format(Val(Dt1.Rows(0)("Count_Gst_Perc").ToString), "###########0.00")

            '    Else
            '        lbl_CGSTPerc.Text = Format(Val(Dt1.Rows(0)("Count_Gst_Perc").ToString / 2), "###########0.00")
            '        lbl_SGSTPerc.Text = Format(Val(Dt1.Rows(0)("Count_Gst_Perc").ToString / 2), "###########0.00")

            '    End If

            'End If

            'get_RateDetails()

            ' Call NetAmount_Calculation()

        End If
        Dt1.Dispose()

        Da1.Dispose()

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

    Private Sub txt_DiscountRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        'If Common_Procedures.userright_check(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Invoice_Entry, New_Entry) = False Then Exit Sub

        printing_invoice()

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palsladam)
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1006" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Prakash Sizing (Somanur)
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'SetCdSel = Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_company.Tag))

        prn_HdDt = New DataTable
        dt2 = New DataTable
        dt3 = New DataTable
        prn_PageNo = 0
        prn_Count = 0

        Try
            'If Common_Procedures.settings.CustomerCode = "1282" Then
            da1 = New SqlClient.SqlDataAdapter("Select a.*, b.*, c.*,Vh.*,id.*,id.Rewinding_Amount as Rewinding, id.packing_Amount as Packing ,Ch.Count_Name, mh.Mill_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, vsh.State_Name as vendor_State_Name, vsh.State_Code as Vendor_State_Code, dph.Ledger_Name as Delivery_Name ,dph.Ledger_Address1 as Delivery_Address1, dph.Ledger_Address2 as Delivery_Address2, dph.Ledger_Address3 as Delivery_Address3, dph.Ledger_Address4 as Delivery_Address4, dph.Ledger_GSTinNo as Delivery_GST_No from Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_Idno = Csh.State_IdNo INNER JOIN Ledger_Head c ON (case when a.OnAccount_IdNo <> 0 then a.OnAccount_IdNo else a.Ledger_IdNo end) = c.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Vendor_Head VH ON vh.Vendor_IdNo = a.Vendor_IdNo LEFT OUTER JOIN State_Head vsh ON vh.State = vsh.State_IdNo LEFT OUTER JOIN Delivery_Party_Head dph ON Dph.Ledger_IdNo = a.DeliveryTo_IdNo Left outer join invoice_Details id ON a.Invoice_code = id.Invoice_code left outer join Count_Head ch on id.Count_IdNo = ch.count_IdNo LEFT OUTER JOIN Mill_Head mh ON id.Mill_IdNo = mh.Mill_IdNo where a.company_idno = " & Str(Val(lbl_company.Tag)) & " and a.Invoice_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            'da2 = New SqlClient.SqlDataAdapter("Select a.*, b.* from Invoice_Details a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  where a.company_idno = " & Str(Val(lbl_company.Tag)) & " and a.Invoice_Code = '" & Trim(NewCode) & "'", con)
            'prn_DetDt = New DataTable
            'da1.Fill(prn_DetDt)


            da2 = New SqlClient.SqlDataAdapter("Select  Mh.Mill_Name from Specification_Head sh LEFT OUTER JOIN Mill_Head mh ON sh.Mill_IdNo = mh.Mill_IdNo  where sh.SetCode_ForSelection = '" & Trim(Cbo_SetNo.Text) & "'", con)
            dt2 = New DataTable
            da2.Fill(dt2)


            da3 = New SqlClient.SqlDataAdapter("Select sh.*, ch.Count_Name from Specification_Head sh LEFT OUTER JOIN Count_Head ch ON sh.Count_IdNo = ch.Count_IdNo  where sh.SetCode_ForSelection = '" & Trim(Cbo_SetNo.Text) & "'", con)
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
        Else
            Printing_Format1_GST(e)
        End If
        'If prn_Status = 1 Then
        '        Printing_Format1(e)
        '    Else
        '        Printing_Format3(e)
        '    End If
        'Else
        '    If Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-2" Then
        '        Printing_Format2(e)
        '    ElseIf Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-3" Then
        '        Printing_Format3(e)
        '    ElseIf Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-4" Then
        '        Printing_Format4(e)
        '    Else
        '        Printing_Format1(e)
        '    End If
        'End If

    End Sub

    Private Sub Printing_Format1_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim S As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""


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
            TxtHgt = 18.5 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Avinashi)
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
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
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
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Kalaimagal Sizing (Palladam)
            '  e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.AADHAVAN, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1284" Then '----- SHREE VEL SIZING (PALLADAM)
            '  e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_VelSizing_Palladam, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
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
                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 10, 90, 100)

                            End If

                        End Using
                    End If
                End If

            End If
        End If

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 22, FontStyle.Bold)
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
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
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
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Led_TinNo = " TIN NO :  " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            End If
        End If

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "Billed & Shipped To  : ", LMargin + 10, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Then '---- Prakash Sizing (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
        End If
        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Led_GstNo = "GSTIN :  " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Led_TinNo = " TIN NO : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
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
        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
        '    Common_Procedures.Print_To_PrintDocument(e, "GST-" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
        '    Common_Procedures.Print_To_PrintDocument(e, "SIZING/" & Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
        'Else
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Invoice_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, p1Font)
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then '---- Meenashi Sizing (Somanur)
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

        NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "##########0.00")

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
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
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
        BankNm1 = "" : BankNm2 = ""


        If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY
        '=============GST SUMMARY============
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1036" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1112" Then '---- Kalaimagal Sizing (Avinashi)
            Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
        End If
        '=========================

        CurY = CurY
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
        Else
            p1Font = New Font("Calibri", 10, FontStyle.Underline)
        End If

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
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        End If


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Avinashi)
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
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
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
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
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
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, Brushes.Green)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, br)
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Then '---- Prakash Sizing (Somanur)
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
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

        NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "##########0.00")

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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1036" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" Then '---- Kalaimagal Sizing (Avinashi)
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
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
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
        If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "TIN No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
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
            If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "TIN No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, CurX + 20, CurY + 5, 0, 0, pFont)
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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Party_Name.Text)
            If Led_IdNo = 0 Then Exit Sub

            vSetCd = ""
            vSetNo = ""
            Da = New SqlClient.SqlDataAdapter("select * from Specification_Head where setcode_forSelection = '" & Trim(Cbo_SetNo.Text) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                vSetCd = Dt1.Rows(0).Item("Set_Code").ToString
                vSetNo = Dt1.Rows(0).Item("set_no").ToString
            End If
            Dt1.Clear()

            MailTxt = "INVOICE " & vbCrLf & vbCrLf

            'MailTxt = MailTxt & "INV.NO:" & Trim(lbl_Invoice_No.Text) & vbCrLf & "DATE:" & Trim(dtp_date.Text) & vbCrLf & vbCrLf & "SET.NO:" & Trim(vSetNo) & vbCrLf & "AMOUNT:" & Trim(lbl_NetAmount.Text)

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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Party_Name.Text)
            If Led_IdNo = 0 Then Exit Sub

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            vSetCd = ""
            vSetNo = ""
            Da = New SqlClient.SqlDataAdapter("select * from Specification_Head where setcode_forSelection = '" & Trim(Cbo_SetNo.Text) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                vSetCd = Dt1.Rows(0).Item("Set_Code").ToString
                vSetNo = Dt1.Rows(0).Item("set_no").ToString
            End If
            Dt1.Clear()
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then
                smstxt = "INVOICE " & vbCrLf & vbCrLf
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
                'smstxt = smstxt & "INV.NO:" & Trim(lbl_Invoice_No.Text) & vbCrLf & "DATE:" & Trim(dtp_date.Text) & vbCrLf & "SET.NO:" & Trim(vSetNo) & vbCrLf & "TOTAL AMOUNT:" & Trim(lbl_NetAmount.Text)
            Else
                '    smstxt = smstxt & "INV.NO:" & Trim(lbl_Invoice_No.Text) & vbCrLf & "DATE:" & Trim(dtp_date.Text) & vbCrLf & vbCrLf & "SET.NO:" & Trim(vSetNo) & vbCrLf & "AMOUNT:" & Trim(lbl_NetAmount.Text)
            End If

            smstxt = smstxt & vbCrLf & " Thanks! " & vbCrLf
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                smstxt = smstxt & "GKT SIZING "
            Else '
                smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_company.Tag))
            End If

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
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

    Private Sub get_RateDetails(ByVal n As Integer)
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim cnt_Id As Integer

        Dim LedID As Integer


        LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Party_Name.Text)

        If LedID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select a.* from Ledger_Rate_Head a where a.Ledger_IdNo = " & Val(LedID) & " ", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                If dgv_Pnl_Selection_Details.Rows(n).Cells(12).Value = "1" Then
                    'For i = 0 To Dt.Rows.Count - 1

                    'n = dgv_Details.Rows.Add()

                    dgv_Details.Rows(n).Cells(13).Value = Format(Val(Dt.Rows(0).Item("Packing_Charge").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(16).Value = Format(Val(Dt.Rows(0).Item("Welding_Charge").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(10).Value = Format(Val(Dt.Rows(0).Item("Rewinding_Charge").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(19).Value = Trim(Dt.Rows(0).Item("Discount_Type").ToString)
                    dgv_Details.Rows(n).Cells(20).Value = (Dt.Rows(0).Item("Discount_Rate").ToString)

                    'Next
                End If
            End If



            For i = 0 To dgv_Details.Rows.Count - 1

                cnt_Id = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(0).Cells(4).Value)

            Next

            Da1 = New SqlClient.SqlDataAdapter("select a.* from Ledger_Rate_Details a Where Ledger_IdNo = " & Val(LedID) & " and Count_IdNo = " & Val(cnt_Id) & " ", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt.Rows.Count > 0 Then

                dgv_Details.Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(0).Item("Rate").ToString), "########0.00")

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
            pnl_back.Enabled = True
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

    Private Sub cbo_Filter_SetNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_SetNo.GotFocus

        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
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
        If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "TIN No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
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
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
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
            Common_Procedures.Print_To_PrintDocument(e, "TO : " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
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
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
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

    Private Sub Cbo_Tax_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Tax_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub Cbo_Tax_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Tax_Type.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_partyname_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Party_Name.LostFocus
        If Trim(UCase(Cbo_Party_Name.Tag)) <> Trim(UCase(Cbo_Party_Name.Text)) Then
            Cbo_Party_Name.Tag = Cbo_Party_Name.Text
            'get_RateDetails()
            NetAmount_Calculation()
        End If
    End Sub

    Private Sub cbo_partyname_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Party_Name.SelectedIndexChanged
        If Trim(UCase(Cbo_Party_Name.Tag)) <> Trim(UCase(Cbo_Party_Name.Text)) Then
            Cbo_Party_Name.Tag = Cbo_Party_Name.Text
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

            EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)


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

    Private Sub cbo_InvoiceSufixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Invoice_Suffix_No.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Invoice_Suffix_No, txt_Invoice_Prefix_No, msk_Date, "", "", "", "")
        If e.KeyCode = 38 And cbo_Invoice_Suffix_No.DroppedDown = False And (e.Control = True And e.KeyCode = 38) Then
            If msk_Date.Visible Then
                msk_Date.Focus()
            Else
                Cbo_Party_Name.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Invoice_Suffix_No.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Invoice_Suffix_No, msk_Date, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            msk_Date.Focus()
        End If
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Invoice_Suffix_No.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            'Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Invoice_Suffix_No.Name
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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

    '    EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
    '        Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
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
        Dim LnAr(16) As Single
        Dim W1 As Single, W2 As Single = 0, W3 As Single = 0, S1 As Single
        Dim CurY1 As Single = 0, CurY2 As Single
        Dim C1 As Single, C2 As Single, C3 As Single, C4 As Single, C5 As Single, C6 As Single, C7 As Single, C8 As Single, C9 As Single, C10 As Single, C11 As Single, C12 As Single
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
        Dim SNo As Integer = 0
        Dim n As Integer = 0
        Dim ItmNm1 As String = ""
        Dim EndsNm1 As String = ""
        Dim EndsNm2 As String = ""
        Dim ItmNm2 As String = ""
        Dim j As Integer = 0
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            pFont = New Font("Calibri", 11, FontStyle.Regular)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            pFont = New Font("Calibri", 11, FontStyle.Bold)
        Else
            pFont = New Font("Calibri", 9, FontStyle.Regular)
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Avinashi)
            TxtHgtInc = 5.5
            NoofItems_PerPage = 8 '13 ' 15
        Else
            TxtHgtInc = 0
            NoofItems_PerPage = 10
        End If

        Erase LnAr
        LnAr = New Single(16) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 30 ' 80
        C2 = 58 '280 330
        C3 = 48 '115 120
        C4 = 45 '45 90
        C5 = 145 ' 160
        C6 = 58 ' 
        C7 = 45 ' 50
        C8 = 45 '60
        C9 = 55
        C10 = 55
        C11 = 55
        C12 = PageWidth - (LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10 + C11)

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
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
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


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY + 10, 100, 100)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Kalaimagal Sizing (Palladam)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.AADHAVAN, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1284" Then '----- SHREE VEL SIZING (PALLADAM)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_VelSizing_Palladam, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- BRT Sizing (somanur)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_BRT, Drawing.Image), LMargin + 20, CurY + 20, 130, 110)
        'End If


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




        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then '---- Guru Sizing (Somanur)
            p1Font = New Font("Calibri", 22, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + TxtHgt + 6
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No, LMargin + 10, CurY, 2, 0, p1Font)
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

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then
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
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Led_TinNo = " TIN NO :  " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            End If
        End If

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

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
        'If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
        'End If

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + C3 + C4 + C5 + W2, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DIRECT", LMargin + C1 + C2 + C3 + C4 + C5 + W2 + 15, CurY1, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + W2 + 15, CurY1, 0, 0, p1Font)

        CurY1 = CurY1 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + C3 + C4 + C5 + W2, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + C2 + C3 + C4 + C5 + W2 + 15, CurY1, 0, 0, p1Font)


        CurY1 = CurY1 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge(Y/N) ", LMargin + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "No", LMargin + W1 + 30, CurY1, 0, 0, p1Font)



        Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + C3 + C4 + C5 + W2, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + W2 + 10, CurY1, 0, 0, p1Font)



        ' If Trim(prn_HdDt.Rows(0).Item("Invoice_Date").ToString) <> "" Then
        'Else
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
        'End If
        CurY1 = CurY1 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "State ", LMargin + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 20, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Led_StateNm, LMargin + W1 + 30, CurY1, 0, 0, p1Font)



        Common_Procedures.Print_To_PrintDocument(e, "SAC Code", LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + C3 + C4 + C5 + W2, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim("998821").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + W2 + 15, CurY1, 0, 0, p1Font)

        CurY = CurY1 + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5, LnAr(3), LMargin + C1 + C2 + C3 + C4 + C5, LnAr(4))
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


        CurY1 = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER  (BILLED TO)", LMargin + 10, CurY1, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY1, 0, 0, p1Font)
        CurY = CurY1 + TxtHgt + 5


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        CurY = CurY + 5
        CurY2 = CurY
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, C2, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Vendor_Name").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY, 0, 0, p1Font)
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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address1").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address1").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address2").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address2").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        End If


        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Delivery_Name").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("GST_No").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + 10, CurY2, 0, 0, pFont)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_Address3").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address3").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address3").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        End If



        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Delivery_Name").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Name").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + 10, CurY2, 0, 0, p1Font)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_Address4").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address4").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_Address4").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        End If


        If prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_StateCap & Led_StateNm, LMargin + 10, CurY1, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Delivery_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Delivery_Address1").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("Vendor_State_Name").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Ven_StateCap & Ven_StateNm, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_StateCap & Led_StateNm, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_GST_No").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, pFont)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("GST_No").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 20, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("GST_No").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, p1Font)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, p1Font)
        End If




        If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Phone No." & Trim(Str(Val(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString))), LMargin + 10, CurY1, 0, 0, p1Font)
            End If
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" And prn_HdDt.Rows(0).Item("vendor_PhoneNO").ToString <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Phone No." & Trim(Str(Val(prn_HdDt.Rows(0).Item("vendor_PhoneNO").ToString))), LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, p1Font)
        ElseIf prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString <> "" And Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) = "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Phone No." & Trim(Str(Val(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString))), LMargin + C1 + C2 + C3 + C4 + C5 + 10, CurY2, 0, 0, p1Font)
        End If

        If CurY1 > CurY2 Then
            CurY = CurY1 + TxtHgt + 7
        Else
            CurY = CurY2 + TxtHgt + 7
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5, LnAr(4), LMargin + C1 + C2 + C3 + C4 + C5, LnAr(6))


        p1Font = New Font("Calibri", 8.5, FontStyle.Bold)



        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "SL", LMargin, CurY, 2, C1, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1, CurY, 2, C2, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + C1 + C2, CurY, 2, C3, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + C1 + C2 + C3, CurY, 2, C4, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT &", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "QTY IN", LMargin + C1 + C2 + C3 + C4 + C5, CurY, 2, C6, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + C1 + C2 + C3 + C4 + C5 + C6, CurY, 2, C7, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "R/W", LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7, CurY, 2, C8, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "PACKING", LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8, CurY, 2, C9, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "WELDING", LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9, CurY, 2, C10, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "OTHR", LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10, CurY, 2, C11, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10 + C11, CurY, 2, C12, p1Font)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, C1, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Description", LMargin + C1, CurY, 2, C2, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " ", LMargin, CurY + C1, 2, C2, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Code", LMargin + C1 + C2 + 16, CurY, 2, C3, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "", LMargin + C1 + C2 + C3, CurY, 2, C4, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + C1 + C2 + C3 + C4, CurY, 2, C5, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "(KGS)", LMargin + C1 + C2 + C3 + C4 + C5, CurY, 2, C6, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "(CRGS)", LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7, CurY, 2, C8, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "(CRGS)", LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8, CurY, 2, C9, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "(CRGS)", LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9, CurY, 2, C10, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "(CRGS)", LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10, CurY, 2, C11, p1Font)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        NoofDets = 0

        'CurY = CurY + TxtHgt - 8
        'p2Font = New Font("Calibri", 12, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821", LMargin + C1 + 10, CurY, 2, C2, p2Font)

        CurY = CurY + TxtHgt - 3
        p1Font = New Font("Calibri", 10, FontStyle.Bold)


        'Common_Procedures.Print_To_PrintDocument(e, " Warping & Sizing Charges ", LMargin + C1 + 10, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt - 3

        'Common_Procedures.Print_To_PrintDocument(e, "( Textile Manufacturing Services ) ", LMargin + C1 + 10, CurY, 0, 0, pFont)

        NoofDets = NoofDets + 1


        SNo = 0
        SNo = SNo + 1

        CurY = CurY - TxtHgt


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then '---- Meenashi Sizing (Somanur)
        '    If (prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) > 0 Then
        '        CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
        '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Text1").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Rate1").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        '    End If





        For i = 0 To prn_HdDt.Rows.Count - 1

            ItmNm1 = Trim(prn_HdDt.Rows(i).Item("mill_Name").ToString)
            'If Trim(prn_HdDt.Rows(i).Item("mill_Name").ToString) <> "" Then
            '    ItmNm1 = Trim(prn_HdDt.Rows(i).Item("mill_Name").ToString) & " " & Trim(prn_HdDt.Rows(i).Item("Count_Name").ToString)
            'End If

            ItmNm2 = ""
            If Len(ItmNm1) > 26 Then
                For j = 26 To 1 Step -1
                    If Mid$(Trim(ItmNm1), j, 1) = " " Or Mid$(Trim(ItmNm1), j, 1) = "," Or Mid$(Trim(ItmNm1), j, 1) = "." Or Mid$(Trim(ItmNm1), j, 1) = "-" Or Mid$(Trim(ItmNm1), j, 1) = "/" Or Mid$(Trim(ItmNm1), j, 1) = "_" Or Mid$(Trim(ItmNm1), j, 1) = "(" Or Mid$(Trim(ItmNm1), j, 1) = ")" Or Mid$(Trim(ItmNm1), j, 1) = "\" Or Mid$(Trim(ItmNm1), j, 1) = "[" Or Mid$(Trim(ItmNm1), j, 1) = "]" Or Mid$(Trim(ItmNm1), j, 1) = "{" Or Mid$(Trim(ItmNm1), j, 1) = "}" Then Exit For
                Next j
                If j = 0 Then j = 26

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - j)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), j - 1)
            End If

            EndsNm1 = Trim(prn_HdDt.Rows(i).Item("Ends_IdNo").ToString)
            'If Trim(prn_HdDt.Rows(i).Item("mill_Name").ToString) <> "" Then
            '    ItmNm1 = Trim(prn_HdDt.Rows(i).Item("mill_Name").ToString) & " " & Trim(prn_HdDt.Rows(i).Item("Count_Name").ToString)
            'End If

            EndsNm2 = ""
            If Len(EndsNm1) > 7 Then
                For j = 7 To 1 Step -1
                    If Mid$(Trim(EndsNm1), j, 1) = " " Or Mid$(Trim(EndsNm1), j, 1) = "," Or Mid$(Trim(EndsNm1), j, 1) = "." Or Mid$(Trim(EndsNm1), j, 1) = "-" Or Mid$(Trim(EndsNm1), j, 1) = "/" Or Mid$(Trim(EndsNm1), j, 1) = "_" Or Mid$(Trim(EndsNm1), j, 1) = "(" Or Mid$(Trim(EndsNm1), j, 1) = ")" Or Mid$(Trim(EndsNm1), j, 1) = "\" Or Mid$(Trim(EndsNm1), j, 1) = "[" Or Mid$(Trim(EndsNm1), j, 1) = "]" Or Mid$(Trim(EndsNm1), j, 1) = "{" Or Mid$(Trim(EndsNm1), j, 1) = "}" Then Exit For
                Next j
                If j = 0 Then j = 7

                EndsNm2 = Microsoft.VisualBasic.Right(Trim(EndsNm1), Len(EndsNm1) - j)
                EndsNm1 = Microsoft.VisualBasic.Left(Trim(EndsNm1), j - 1)
            End If

            p2Font = New Font("Calibri", 8, FontStyle.Regular)

            If (prn_HdDt.Rows(i).Item("warp_Amount").ToString) > 0 Then
                CurY = CurY + TxtHgt + TxtHgtInc + TxtHgtInc
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(i).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(i).Item("Ref_Date").ToString), "dd-MM-yy"), LMargin + C1 + 5, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(i).Item("Set_IdNo").ToString), LMargin + C1 + C2 + 10, CurY, 0, 0, p2Font)
                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(i).Item("Ends_IdNo").ToString), LMargin + C1 + C2 + C3 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(EndsNm1).ToString, LMargin + C1 + C2 + C3 + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1).ToString, LMargin + C1 + C2 + C3 + C4 + 8, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(i).Item("Warp_Kgs").ToString), "#########0.00"), LMargin + C1 + C2 + C3 + C4 + C5 + C6 - 10, CurY, 1, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(i).Item("Warp_Rate").ToString), "#########0.00"), LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 - 5, CurY, 1, 0, p2Font)
                'If Val(prn_HdDt.Rows(i).Item("Rewinding").ToString) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(i).Item("Rewinding").ToString), LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 - 10, CurY, 1, 0, p2Font)
                'End If
                'If Val(prn_HdDt.Rows(i).Item("Packing").ToString) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(i).Item("Packing").ToString), LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 - 10, CurY, 1, 0, p2Font)
                'End If
                'If Val(prn_HdDt.Rows(i).Item("Winding_Amount").ToString) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(i).Item("Winding_Amount").ToString), LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10 - 10, CurY, 1, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(i).Item("Other_Charges").ToString), LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10 + C10 - 10, CurY, 1, 0, p2Font)
                'End If
                If Val(prn_HdDt.Rows(i).Item("Total_Amount").ToString) > 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(i).Item("Total_Amount").ToString), "############0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
                End If

                If Trim(ItmNm2) <> "" Then
                    CurY = CurY + TxtHgt
                    'p2Font = New Font("Calibri", 7.5, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, p2Font)
                    NoofDets = NoofDets + 1
                End If

                If Trim(EndsNm2) <> "" Then
                    If Trim(ItmNm2) = "" Then
                        CurY = CurY + TxtHgt
                    End If
                    'p2Font = New Font("Calibri", 7.5, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(EndsNm2), LMargin + C1 + C2 + C3 + 10, CurY, 0, 0, p2Font)
                    NoofDets = NoofDets + 1
                End If


                NoofDets = NoofDets + 1

            End If

        Next





        NetAmt = Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString) + Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("TCS_Amount").ToString), "##########0.00")

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
            e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10, CurY, PageWidth, CurY)
            LnAr(7) = CurY




            'If Val(prn_HdDt.Rows(0).Item("Total_OtherCharges").ToString) > 0 Then
            '    CurY = CurY + TxtHgt - 10
            '    'p2Font = New Font("Calibri", 12, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "Other Charges", LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_OtherCharges").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            'End If



            CurY = CurY + TxtHgt - 5
            p2Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GROSS TOTAL  ", LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            NoofDets = NoofDets + 1
        End If


        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) > 0 Then
            ' CurY = CurY + TxtHgt + TxtHgtInc
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CGST  " & prn_HdDt.Rows(0).Item("CGST_Percentage") & " %".ToString, LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "CGST  ".ToString, LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) > 0 Then
            'CurY = CurY + TxtHgt + TxtHgtInc
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SGST  " & prn_HdDt.Rows(0).Item("SGST_Percentage") & " %".ToString, LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        Else
            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt + TxtHgtInc
            Common_Procedures.Print_To_PrintDocument(e, "SGST  ", LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If


        If Val(prn_HdDt.Rows(0).Item("TCS_Amount").ToString) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TCS  " & (prn_HdDt.Rows(0).Item("TCS_Percentage") & " %".ToString), LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("TCS_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
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
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        CurY = CurY + TxtHgt - 10
        p2Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + C1 + C2 + C3 + C4 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p2Font)

        If Val(prn_HdDt.Rows(0).Item("Total_RewindingCharges").ToString) > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_RewindingCharges").ToString), LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + 10, CurY, 0, 0, p1Font)
        End If
        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sizing_Weight1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Weight3").ToString), "##########0.000"), LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, p1Font)
        strHeight = e.Graphics.MeasureString("A", p2Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2, CurY, LMargin + C1 + C2, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3, CurY, LMargin + C1 + C2 + C3, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4, CurY, LMargin + C1 + C2 + C3 + C4, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5, CurY, LMargin + C1 + C2 + C3 + C4 + C5, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5 + C6, CurY, LMargin + C1 + C2 + C3 + C4 + C5 + C6, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7, CurY, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8, CurY, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9, CurY, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10, CurY, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10, LnAr(6))
        e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10 + C11, CurY, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10 + C11, LnAr(6))
        'e.Graphics.DrawLine(Pens.Black, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10 + C11, CurY, LMargin + C1 + C2 + C3 + C4 + C5 + C6 + C7 + C8 + C9 + C10 + C11, LnAr(6))



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
        Common_Procedures.Print_To_PrintDocument(e, "Interest will be charged @24% P.A if the payment is not received with in 15 days from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
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
            TxtHgt = 18 ' 19.4 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Avinashi)
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
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
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


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1043" Then '---- Selvanayaki Sizing (Karumanthapatti)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY + 10, 100, 100)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Kalaimagal Sizing (Palladam)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.AADHAVAN, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1284" Then '----- SHREE VEL SIZING (PALLADAM)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_VelSizing_Palladam, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- BRT Sizing (somanur)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_BRT, Drawing.Image), LMargin + 20, CurY + 20, 130, 110)
        'End If


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
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No, LMargin + 10, CurY, 2, 0, p1Font)
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

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then
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
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Led_TinNo = " TIN NO :  " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            End If
        End If

        W1 = e.Graphics.MeasureString("Broker Phone No   :", pFont).Width
        W2 = e.Graphics.MeasureString("Transporter GSTIN :", pFont).Width
        S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        'p1Font = New Font("Calibri", 12, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, " TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt - 13
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


        CurY = CurY + TxtHgt + 6
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY



        CurY1 = CurY + 10

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


        CurY1 = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER  (BILLED TO)", LMargin + 10, CurY1, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C1 + C2 + (C3 / 3) + 10, CurY1, 0, 0, p1Font)
        CurY = CurY1 + TxtHgt + 5


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
        CurY = CurY + TxtHgt - 13

        W1 = e.Graphics.MeasureString("Set No  : ", pFont).Width

        Common_Procedures.Print_To_PrintDocument(e, "Set No : ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Set_No").ToString, LMargin + W1 + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Ends  : ", LMargin + C1, CurY, 2, C2, pFont)
        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(0).Item("ends_Name").ToString, LMargin + C1 + W1, CurY, 2, C2, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Count  : ", LMargin + C1 + C2 + (C3 / 4), CurY, 2, C4, pFont)
        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(0).Item("Count_Name").ToString, LMargin + C1 + C2 + (C3 / 4) + W1, CurY, 2, C4, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Meters  : ", LMargin + C1 + C2 + C4 + C5, CurY, 2, C6, pFont)
        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(0).Item("Warp_Meters").ToString, LMargin + C1 + C2 + C4 + C5 + W1, CurY, 2, C6, p1Font)

        CurY = CurY + TxtHgt + 6

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


        CurY = CurY + TxtHgt - 10
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

        CurY = CurY + TxtHgt + 10
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

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "Transport_IdNo = 0")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, Nothing, "Transport_Head", "Transport_Name", "", "Transport_IdNo = 0")
        If e.KeyCode = 40 And cbo_Transport.DroppedDown = False Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Vechile.Visible Then
                cbo_Vechile.Focus()
            Else
                txt_CGST_Perc.Focus()
            End If
        End If
        If e.KeyCode = 38 And cbo_Transport.DroppedDown = False Or (e.Control = True And e.KeyValue = 38) Then
            If txt_AddLess.Visible = True Then
                txt_AddLess.Focus()
            Else
                txt_Freight.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)", False)
        If Asc(e.KeyChar) = 13 Then
            If cbo_Vechile.Visible Then
                cbo_Vechile.Focus()
            Else
                txt_CGST_Perc.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Invoice_Head", "Vechile_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, Cbo_Tax_Type, Nothing, "Invoice_Head", "Vechile_No", "", "")
        If e.KeyCode = 38 And cbo_Vechile.DroppedDown = False Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Transport.Visible = True Then
                cbo_Transport.Focus()
            Else
                txt_AddLess.Focus()
            End If
        End If
        If e.KeyCode = 40 And cbo_Vechile.DroppedDown = False Or (e.Control = True And e.KeyValue = 40) Then
            If txt_CGST_Perc.Visible = True Then
                txt_CGST_Perc.Focus()
            Else
                txt_SGST_Perc.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, Nothing, "Invoice_Head", "Vechile_No", "", "", False)
        If Asc(e.KeyChar) = 13 Then
            If txt_CGST_Perc.Visible = True Then
                txt_CGST_Perc.Focus()
            Else
                txt_SGST_Perc.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VendorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VendorName, Nothing, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
        If e.KeyCode = 40 And cbo_VendorName.DroppedDown = False Or (e.Control = True And e.KeyValue = 40) Then
            If Cbo_SetNo.Visible Then
                Cbo_SetNo.Focus()
            Else
                cbo_DelieveryTo.Focus()
            End If
        End If
        If e.KeyCode = 38 And cbo_VendorName.DroppedDown = False Or (e.Control = True And e.KeyValue = 38) Then
            If Cbo_Tax_Type.Visible Then
                Cbo_Tax_Type.Focus()
            Else
                Cbo_Party_Name.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VendorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VendorName, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Cbo_SetNo.Visible Then
                Cbo_SetNo.Focus()
            Else
                cbo_DelieveryTo.Focus()
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelieveryTo, Nothing, Nothing, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        If (e.KeyCode = 40 And cbo_DelieveryTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_OnAccount.Visible Then
                cbo_OnAccount.Focus()
            Else
                If dgv_Details.Visible = True Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If
        If e.KeyCode = 38 And cbo_DelieveryTo.DroppedDown = False Or (e.Control = True And e.KeyValue = 38) Then
            If Cbo_SetNo.Visible Then
                Cbo_SetNo.Focus()
            Else
                cbo_Vechile.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DelieveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelieveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelieveryTo, Nothing, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_OnAccount.Visible Then
                cbo_OnAccount.Focus()
            Else
                If dgv_Details.Visible = True Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If
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

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        'If e.KeyCode = 40 Then
        '    e.Handled = True : e.SuppressKeyPress = True
        '    Cbo_Party_Name.Focus()
        'End If

        'If e.KeyCode = 38 Then
        '    e.Handled = True : e.SuppressKeyPress = True
        '    cbo_Invoice_Suffix_No.Focus()
        'End If


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

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus
        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 12 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If
        End If
    End Sub

    'Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
    '    msk_Date.Focus()
    '    msk_Date.SelectionStart = 0
    'End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If Asc(e.KeyCode) = 17 And e.Control = False And vcbo_KeyDwnVal = e.KeyCode Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_Date_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        msk_Date.Text = dtp_Date.Text
    End Sub

    Private Sub Cbo_Tax_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Tax_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Tax_Type, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If cbo_VendorName.Visible Then
                cbo_VendorName.Focus()
            Else
                Cbo_SetNo.Focus()
            End If
        End If
    End Sub

    Private Sub Cbo_Tax_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Tax_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Tax_Type, Cbo_Party_Name, Nothing, "", "", "", "")
        If e.KeyCode = 40 And Cbo_Tax_Type.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_VendorName.Visible = True Then
                cbo_VendorName.Focus()
            Else
                Cbo_SetNo.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Invoice_Prefix_No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Invoice_Prefix_No.KeyDown
        If e.KeyCode = 40 Then
            If cbo_Invoice_Suffix_No.Visible Then
                cbo_Invoice_Suffix_No.Focus()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Invoice_Prefix_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Invoice_Prefix_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_Invoice_Suffix_No.Visible = True Then
                cbo_Invoice_Suffix_No.Focus()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim Rect As Rectangle


        With dgv_Details

            If .Visible = True Then

                If .Rows.Count > 0 Then

                    If Val(.Rows(0).Cells(0).Value) = 0 Then
                        .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                    End If




                    If e.ColumnIndex = 19 Then

                        If cbo_Grid_DiscType.Visible = False Or Val(cbo_Grid_DiscType.Tag) <> e.RowIndex Then

                            cbo_Grid_DiscType.Tag = -1
                            'Da = New SqlClient.SqlDataAdapter("SELECT Discount_Type from Invoice_Details ORDER BY Discount_Type", con)
                            'Dt = New DataTable
                            'Da.Fill(Dt1)
                            'cbo_Grid_DiscType.DataSource = Dt1
                            'cbo_Grid_DiscType.DisplayMember = "Discount_Type"
                            ''cbo_Grid_DiscType.ValueMember = ""

                            Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                            cbo_Grid_DiscType.Left = .Left + Rect.Left
                            cbo_Grid_DiscType.Top = .Top + Rect.Top

                            cbo_Grid_DiscType.Width = Rect.Width
                            cbo_Grid_DiscType.Height = Rect.Height
                            cbo_Grid_DiscType.Text = .CurrentCell.Value

                            cbo_Grid_DiscType.DropDownHeight = 106

                            cbo_Grid_DiscType.Tag = Val(e.RowIndex)
                            cbo_Grid_DiscType.Visible = True

                            cbo_Grid_DiscType.BringToFront()
                            cbo_Grid_DiscType.Focus()

                            'cbo_Grid_DiscType.Items.Clear()
                            'cbo_Grid_DiscType.Items.Add("")
                            'cbo_Grid_DiscType.Items.Add("%")
                            'cbo_Grid_DiscType.Items.Add("KG")
                        End If
                    Else
                        cbo_Grid_DiscType.Visible = False

                    End If

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details

                If .Visible Then

                    If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 9 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "##########0.000")
                        Else
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""

                        End If
                        If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 14 Or .CurrentCell.ColumnIndex = 15 Or .CurrentCell.ColumnIndex = 16 Or .CurrentCell.ColumnIndex = 17 Or .CurrentCell.ColumnIndex = 18 Or .CurrentCell.ColumnIndex = 20 Or .CurrentCell.ColumnIndex = 21 Or .CurrentCell.ColumnIndex = 22 Then
                            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "##########0.00")
                            Else
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                            End If
                        End If
                        'NetAmount_Calculation()
                    End If
                End If
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        NetAmount_Calculation()
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        'vcbo_KeyDwnVal = e.KeyValue
        'Try

        '    With dgv_Details

        '        If e.KeyCode = 38 Then
        '            If .CurrentCell.ColumnIndex <= 1 Then
        '                If .CurrentCell.RowIndex = 0 Then
        '                    cbo_OnAccount.Focus()
        '                Else
        '                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
        '                End If
        '            End If
        '        End If

        '        If e.KeyCode = 40 Then
        '            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
        '                If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
        '                    cbo_Transport.Focus()
        '                Else
        '                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
        '                End If
        '            End If
        '        End If

        '    End With
        'Catch ex As Exception

        'End Try
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

                'Total_Calculation()

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        'Dim n As Integer
        'With dgv_Details
        '    n = .RowCount
        '    .Rows(n - 1).Cells(0).Value = Val(n)
        'End With
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.PaleGreen
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details

            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then
                    If .CurrentCell.ColumnIndex <= 6 And Trim(.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> "" Then
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

                    If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 12 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 14 Or .CurrentCell.ColumnIndex = 15 Or .CurrentCell.ColumnIndex = 16 Or .CurrentCell.ColumnIndex = 17 Or .CurrentCell.ColumnIndex = 18 Or .CurrentCell.ColumnIndex = 20 Or .CurrentCell.ColumnIndex = 21 Or .CurrentCell.ColumnIndex = 22 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

            'NetAmount_Calculation()


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = True Or e.KeyCode = 17 Then
            Dim f As New Transport_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
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

                    If .CurrentCell.ColumnIndex >= 22 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            txt_Freight.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                        'ElseIf Val(.CurrentRow.Cells(22).Value) <> 0 Then
                        '    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            txt_Freight.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If
                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            cbo_OnAccount.Focus()

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

    Private Sub cbo_Grid_DiscType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_DiscType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Grid_DiscType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_DiscType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_DiscType, Nothing, Nothing, "", "", "", "")
        With dgv_Details
            If e.KeyCode = 38 And cbo_Grid_DiscType.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
                If .Visible = True Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If
            End If
            If e.KeyCode = 40 And cbo_Grid_DiscType.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
                If .Visible = True Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End If
        End With
    End Sub

    Private Sub cbo_Grid_DiscType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_DiscType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_DiscType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Visible = True Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(dgv_Details.CurrentCell.ColumnIndex + 1)
            End If
        End If
    End Sub

    Private Sub cbo_Grid_DiscType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_DiscType.TextChanged
        Try
            If cbo_Grid_DiscType.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_DiscType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 19 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_DiscType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyCode = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            Else
                cbo_OnAccount.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        NetAmount_Calculation()

        If Asc(e.KeyChar) = 13 Then
            If txt_AddLess.Visible = True Then
                txt_AddLess.Focus()
            Else
                cbo_Transport.Focus()
            End If
        End If

    End Sub

    Private Sub txt_IGST_Perc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_IGST_Perc.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            e.Handled = True
            If MessageBox.Show("Do you want to Save?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txt_AddLess_Caption_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess_Caption.LostFocus
        txt_AddLess_Caption.BackColor = Color.SkyBlue
        txt_AddLess_Caption.ForeColor = Color.Black
    End Sub

    Private Sub txt_IGST_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_IGST_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        NetAmount_Calculation()

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to Save?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        NetAmount_Calculation()
    End Sub

    Private Sub txt_CGST_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CGST_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SGST_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SGST_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        NetAmount_Calculation()
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub Total_Calculation()
        Dim NtAmt As Single
        Dim AssAmt As Single = 0
        Dim CGSTAmt As Single = 0
        Dim SGSTAmt As Single = 0
        Dim IGSTAmt As Single = 0
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim Led_IdNo As Integer

        Dim vWrp_Amt As Single = 0
        Dim vRW_Amt As Single = 0
        Dim vPkng_Amt As Single = 0
        Dim vWdng_Amt As Single = 0
        Dim vDisc_Amt As Single = 0
        Dim vTot_Amt As Single = 0
        Dim SNo As Integer = 0



        If FrmLdSTS = True Or No_Calc_Status = True Then Exit Sub


        lbl_Assessable_Value.Text = Format(Val(txt_AddLess.Text) + Val(txt_Freight.Text), "#########0.00")


        lbl_CGST_Amount.Text = 0
        lbl_SGST_Amount.Text = 0
        lbl_IGST_Amount.Text = 0


        If Trim(Cbo_Tax_Type.Text) = "GST" Then

            Led_IdNo = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "Ledger_Name = '" & Trim(Cbo_Party_Name.Text) & "'"))
            Get_State_Code(Led_IdNo, Ledger_State_Code, Company_State_Code)


            If Trim(Company_State_Code) = Trim(Ledger_State_Code) Then
                '-CGST 
                lbl_CGST_Amount.Text = Format(Val((lbl_Assessable_Value.Text) * Val(txt_CGST_Perc.Text)), "#########0.00")
                '-SGST 
                lbl_SGST_Amount.Text = Format(Val((lbl_Assessable_Value.Text) * Val(txt_SGST_Perc.Text)) / 100, "#########0.00")

            ElseIf Trim(Company_State_Code) <> Trim(Ledger_State_Code) Then
                '-IGST 
                lbl_IGST_Amount.Text = Format(Val(lbl_Assessable_Value.Text) * Val(txt_IGST_Perc.Text) / 100, "#########0.00")
            End If
        Else

            lbl_CGST_Amount.Text = 0
            lbl_SGST_Amount.Text = 0
            lbl_IGST_Amount.Text = 0

        End If

        NtAmt = Val(lbl_Assessable_Value.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)

        lbl_Net_Amount.Text = Format(Val(NtAmt), "##########0.00")
        lbl_Net_Amount.Text = Common_Procedures.Currency_Format(Val(lbl_Net_Amount.Text))


        lbl_Amount_In_Words.Text = "Rupees  :  "
        If Val(CSng(lbl_Net_Amount.Text)) <> 0 Then
            lbl_Amount_In_Words.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_Net_Amount.Text)))
        End If

        'NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim GrsAmt As Single = 0
        Dim Tax_Amt As Single
        Dim vNet_Amt As String
        Dim AssAmt As Single = 0
        Dim GSTGrsPrc As Single = 0
        Dim CGSTAmt As Single = 0
        Dim SGSTAmt As Single = 0
        Dim IGSTAmt As Single = 0
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim Led_IdNo As Integer = 0
        Dim vInterStateStatus As Boolean = False
        Dim Sno As Integer
        Dim Tot_Amt As String

        Dim vTCS_AssVal As String = 0
        Dim vTOT_SalAmt As String = 0
        Dim vTCS_Amt As String = 0
        Dim vInvAmt_Bfr_TCS As String = 0

        If FrmLdSTS = True Then Exit Sub
        If No_Calc_Status = True Then Exit Sub


        With dgv_Details

            For i = 0 To .Rows.Count - 1

                'n = .Rows.Add()

                .Rows(i).Cells(8).Value = Val(.Rows(i).Cells(6).Value) * Val(.Rows(i).Cells(7).Value)
                '.Rows(i).Cells(11).Value = Val(.Rows(i).Cells(9).Value) * Val(.Rows(i).Cells(10).Value)
                .Rows(i).Cells(14).Value = Val(.Rows(i).Cells(12).Value) * Val(.Rows(i).Cells(13).Value)
                .Rows(i).Cells(17).Value = Val(.Rows(i).Cells(15).Value) * Val(.Rows(i).Cells(16).Value)

                If Trim(UCase(cbo_Grid_DiscType.Text)) = "KG" Then
                    .Rows(i).Cells(21).Value = Format(Val(.Rows(i).Cells(6).Value) * Val(.Rows(i).Cells(20).Value), "#########0.00")
                Else
                    .Rows(i).Cells(21).Value = Format(Val(.Rows(i).Cells(8).Value) * Val(.Rows(i).Cells(20).Value) / 100, "#########0.00")
                End If
                .Rows(i).Cells(22).Value = Val(.Rows(i).Cells(8).Value) + Val(.Rows(i).Cells(11).Value) + Val(.Rows(i).Cells(14).Value) + Val(.Rows(i).Cells(17).Value) + Val(.Rows(i).Cells(18).Value) - Val(.Rows(i).Cells(21).Value)
            Next i

        End With


        Tot_Amt = 0


        With dgv_Details
            For j = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(j).Cells(0).Value = Sno
                If Val(.Rows(j).Cells(22).Value) <> 0 Then
                    Tot_Amt = Format(Val(Tot_Amt) + Val(.Rows(j).Cells(22).Value), "##########0.000")
                End If
            Next
        End With

        lbl_Gross_Amount.Text = Format(Val(Tot_Amt), "########0.00")


        'lbl_CGST_Amount.Text = "0.00"
        'lbl_SGST_Amount.Text = "0.00"
        'lbl_IGST_Amount.Text = "0.00"
        'txt_CGST_Perc.Text = 0
        'txt_SGST_Perc.Text = 0
        'txt_IGST_Perc.Text = 0

        'With dgv_Total_Details
        '    If .Rows.Count > 0 Then
        '        GrsAmt = .Rows(0).Cells(22).Value
        '    End If
        'End With

        lbl_Assessable_Value.Text = Val(txt_AddLess.Text) + Val(txt_Freight.Text) + Val(lbl_Gross_Amount.Text)

        AssAmt = Format(Val(lbl_Assessable_Value.Text), "##########0.00")

        GST_Calculation()

        Tax_Amt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)



        If Val(lbl_TotalSales_Amount_Current_Year.Text) = 0 Then lbl_TotalSales_Amount_Current_Year.Text = "0.00"
        If Val(lbl_TotalSales_Amount_Previous_Year.Text) = 0 Then lbl_TotalSales_Amount_Previous_Year.Text = "0.00"

        Dim vTCS_StartDate As Date = #9/30/2020#
        Dim vMIN_TCS_assval As String = "5000000"

        If chk_TCS_Tax.Checked = True Then

            If DateDiff("d", vTCS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                If txt_TCS_TaxableValue.Enabled = False Then

                    vTOT_SalAmt = Format(Val(AssAmt) + Val(Tax_Amt), "###########0")

                    vTCS_AssVal = 0

                    If Common_Procedures.settings.CustomerCode = "1277" Then 'SRINATH WEAVING MILLS LLP  (PALLADAM) , Desikanathar 
                        vTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf Val(CDbl(lbl_TotalSales_Amount_Previous_Year.Text)) > Val(vMIN_TCS_assval) Then
                        If Common_Procedures.settings.CustomerCode <> "1066" Then ''Southern Cot Spinners
                            vTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                        End If

                    ElseIf Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) > Val(vMIN_TCS_assval) Then
                        vTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf (Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt)) > Val(vMIN_TCS_assval) Then
                        vTCS_AssVal = Format(Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt) - Val(vMIN_TCS_assval), "############0")

                    End If
                    txt_TCS_TaxableValue.Text = Format(Val(vTCS_AssVal), "############0.00")

                    If Val(txt_TCS_TaxableValue.Text) > 0 Then
                        If Val(txt_TcsPerc.Text) = 0 Then
                            txt_TcsPerc.Text = "0.1" ' "0.075"
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

        vInvAmt_Bfr_TCS = Format(Val(AssAmt) + Val(Tax_Amt), "###########0.00")
        lbl_Invoice_Value_Before_TCS.Text = Format(Val(vInvAmt_Bfr_TCS), "###########0")
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = Format(Val(lbl_Invoice_Value_Before_TCS.Text) - Val(vInvAmt_Bfr_TCS), "###########0.00")


        vNet_Amt = Format(Val(AssAmt) + Val(Tax_Amt) + Val(lbl_TcsAmount.Text), "##########0.00")

        lbl_Net_Amount.Text = Format(Val(vNet_Amt), "########0")

        lbl_RoundOff_Amt.Text = Format(Val(lbl_Net_Amount.Text) - Val(vNet_Amt), "#########0.00")

    End Sub


    Private Sub lbl_Net_Amount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_Net_Amount.TextChanged
        lbl_Amount_In_Words.Text = "Rupees  :  "
        If Val(lbl_Net_Amount.Text) <> 0 Then
            lbl_Amount_In_Words.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(lbl_Net_Amount.Text))
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_selection()
    End Sub

    Private Sub Close_selection()
        Dim n As Integer
        Dim sno As Integer
        Dim EdsNm As String, DupEdsNm As String
        Dim CntNm As String, DupCntNm As String

        dgv_Details.Rows.Clear()


        EdsNm = "" : DupEdsNm = ""
        CntNm = "" : DupCntNm = ""

        pnl_back.Enabled = True

        For i = 0 To dgv_Pnl_Selection_Details.RowCount - 1

            If Val(dgv_Pnl_Selection_Details.Rows(i).Cells(12).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(2).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(1).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(2).Value
                'dgv_Details.Rows(n).Cells(3).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(5).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(6).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(23).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(13).Value

                'dgv_Details.Rows(n).Cells(6).Value = dgv_Pnl_Selection_Details.Rows(n).Cells(14).Value
                dgv_Details.Rows(n).Cells(7).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(15).Value
                'dgv_Details.Rows(n).Cells(8).Value = dgv_Pnl_Selection_Details.Rows(n).Cells(16).Value
                'dgv_Details.Rows(n).Cells(9).Value = dgv_Pnl_Selection_Details.Rows(n).Cells(17).Value
                'dgv_Details.Rows(n).Cells(10).Value = dgv_Pnl_Selection_Details.Rows(n).Cells(18).Value
                dgv_Details.Rows(n).Cells(11).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(19).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(20).Value
                dgv_Details.Rows(n).Cells(13).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(21).Value
                'dgv_Details.Rows(n).Cells(14).Value = dgv_Pnl_Selection_Details.Rows(n).Cells(22).Value
                dgv_Details.Rows(n).Cells(15).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(23).Value
                dgv_Details.Rows(n).Cells(16).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(24).Value
                'dgv_Details.Rows(n).Cells(17).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(25).Value
                dgv_Details.Rows(n).Cells(18).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(26).Value
                dgv_Details.Rows(n).Cells(19).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(27).Value
                dgv_Details.Rows(n).Cells(20).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(28).Value
                dgv_Details.Rows(n).Cells(21).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(29).Value

                'dgv_Details.Rows(n).Cells(9).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(10).Value
                'dgv_Details.Rows(n).Cells(10).Value = dgv_Pnl_Selection_Details.Rows(i).Cells(11).Value


                'If InStr(1, Trim(LCase(DupEdsNm)), "~" & Trim(LCase(dgv_Pnl_Selection_Details.Rows(i).Cells(8).Value)) & "~") = 0 Then
                '    EdsNm = Trim(EdsNm) & IIf(Trim(EdsNm) <> "", ", ", "") & Trim(dgv_Pnl_Selection_Details.Rows(i).Cells(8).Value)
                '    DupEdsNm = Trim(DupEdsNm) & "~" & Trim(dgv_Pnl_Selection_Details.Rows(i).Cells(8).Value) & "~"
                'End If

                'If InStr(1, Trim(LCase(DupCntNm)), "~" & Trim(LCase(dgv_Pnl_Selection_Details.Rows(i).Cells(9).Value)) & "~") = 0 Then
                '    CntNm = Trim(CntNm) & IIf(Trim(CntNm) <> "", ", ", "") & Trim(dgv_Pnl_Selection_Details.Rows(i).Cells(9).Value)
                '    DupCntNm = Trim(DupCntNm) & "~" & Trim(dgv_Pnl_Selection_Details.Rows(i).Cells(9).Value) & "~"
                'End If


            End If
            If Trim(Common_Procedures.settings.CustomerCode) <> "1282" Then
                get_RateDetails(n)
            End If


        Next

        'lbl_Ends.Text = Trim(EdsNm)
        'lbl_CountName.Text = Trim(CntNm)

        'Total_Calculation()
        'get_RateDetails()
        'NetAmount_Calculation()

        pnl_back.Enabled = True
        pnl_Selection.Visible = False
        If Cbo_Tax_Type.Enabled And Cbo_Tax_Type.Visible Then Cbo_Tax_Type.Focus()



    End Sub
    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub


    Private Sub get_set_Details(ByVal SelcSetCd As String)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Amt As Single = 0
        Dim Ent_ReAmount As Single = 0
        Dim Ent_ReKgs As Single = 0
        Dim Ent_PackBeams As Single = 0
        Dim Ent_PackRate As Single = 0
        Dim Ent_PackAmt As Single = 0
        Dim Ent_WelBeams As Single = 0
        Dim Ent_WelRate As Single = 0
        Dim Ent_WelAmount As Single = 0
        Dim Ent_otherCharges As Single = 0
        Dim Ent_DisType As String
        Dim Ent_DisRate As String
        Dim Ent_DisAmount As String
        Dim Ent_TotalAmount As Single = 0
        Dim Ent_SetCode As String


        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Party_Name.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PARTY NAME...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Cbo_Party_Name.Enabled And Cbo_Party_Name.Visible Then Cbo_Party_Name.Focus()
            Exit Sub
        End If


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Pnl_Selection_Details


            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, lh.Ledger_Name, ch.Count_Name, mh.Mill_Name, eid.Warp_Rate as EntRate, eid.Warp_Amount as EntAmount, Eid.Rewinding_Kgs as EntRewindkgs, Eid.Rewinding_Amount as EntRewindAmount, Eid.No_Of_Beams as EntPavuBeam, Eid.Packing_Rate as EntPackRate, eid.Packing_Amount as EntPackAmount, Eid.Winding_Beams as EntWelBeams, Eid.Winding_Rate as EntwelRate, Eid.Winding_Amount as entWelAmount, Eid.Other_Charges as EntOtherCharges, Eid.Discount_Type as entDiscountType, eid.Discount_Rate as EntDiscountRate, eid.Discount_Amount as EntDiscountAmount, eid.Total_Amount as EntTotalAmount, eid.Set_Code as EntSetCode from Specification_Head a  Left Outer Join Ledger_Head lh ON  a.Ledger_IdNo = lh.Ledger_IdNo LEFT OUTER JOIN Count_Head ch ON a.Count_IdNo = ch.count_IdNo LEFT OUTER JOIN mill_head mh ON a.Mill_IdNo = mh.Mill_IdNo Left outer join Invoice_Details Eid ON Eid.Invoice_Code = '" & Trim(NewCode) & "' and a.set_Code = Eid.set_Code  where  a.Ledger_IdNo = " & Trim(LedIdNo) & "  and a.Invoice_Code = '" & Trim(NewCode) & "' Order by a.set_Date, For_OrderBy, set_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()



                    Ent_Rate = 0
                    Ent_Amt = 0
                    Ent_ReKgs = 0
                    Ent_ReAmount = 0
                    Ent_PackBeams = 0
                    Ent_PackRate = 0
                    Ent_PackAmt = 0
                    Ent_WelBeams = 0
                    Ent_WelRate = 0
                    Ent_WelAmount = 0
                    Ent_otherCharges = 0
                    Ent_DisType = ""
                    Ent_DisRate = 0
                    Ent_DisAmount = 0
                    Ent_TotalAmount = 0
                    Ent_SetCode = ""


                    If IsDBNull(Dt1.Rows(i).Item("EntRate").ToString) = False Then
                        Ent_Rate = Val(Dt1.Rows(i).Item("EntRate").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntAmount").ToString) = False Then
                        Ent_Amt = Dt1.Rows(i).Item("EntAmount").ToString
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntRewindkgs").ToString) = False Then
                        Ent_ReKgs = Val(Dt1.Rows(i).Item("EntRewindkgs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntRewindAmount").ToString) = False Then
                        Ent_ReAmount = Val(Dt1.Rows(i).Item("EntRewindAmount").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntPavuBeam").ToString) = False Then
                        Ent_PackBeams = Val(Dt1.Rows(i).Item("EntPavuBeam").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntPackRate").ToString) = False Then
                        Ent_PackRate = Val(Dt1.Rows(i).Item("EntPackRate").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntPackAmount").ToString) = False Then
                        Ent_PackAmt = Dt1.Rows(i).Item("EntPackAmount").ToString
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntWelBeams").ToString) = False Then
                        Ent_WelBeams = Val(Dt1.Rows(i).Item("EntWelBeams").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntWelRate").ToString) = False Then
                        Ent_WelRate = Val(Dt1.Rows(i).Item("EntWelRate").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntWelAmount").ToString) = False Then
                        Ent_WelAmount = Val(Dt1.Rows(i).Item("EntWelAmount").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntOtherCharges").ToString) = False Then
                        Ent_otherCharges = Val(Dt1.Rows(i).Item("EntOtherCharges").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("entDiscountType").ToString) = False Then
                        Ent_DisType = Trim(Dt1.Rows(i).Item("entDiscountType").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntDiscountRate").ToString) = False Then
                        Ent_DisRate = Val(Dt1.Rows(i).Item("EntDiscountRate").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntDiscountAmount").ToString) = False Then
                        Ent_DisAmount = Val(Dt1.Rows(i).Item("EntDiscountAmount").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntTotalAmount").ToString) = False Then
                        Ent_TotalAmount = Val(Dt1.Rows(i).Item("EntTotalAmount").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("EntSetCode").ToString) = False Then
                        Ent_SetCode = Val(Dt1.Rows(i).Item("EntSetCode").ToString)
                    End If


                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("set_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("set_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("ends_name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("total_warping_net_weight").ToString
                    '.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Warp_Weight").ToString
                    '.Rows(n).Cells(8).Value = Val(Ent_Amt) 'Dt1.Rows(i).Item("Rewinding_Weight").ToString
                    .Rows(n).Cells(12).Value = "1"
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("set_Code").ToString

                    '.Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Warp_Weight").ToString
                    .Rows(n).Cells(15).Value = Val(Ent_Rate) 'Dt1.Rows(i).Item("Warp_Rate").ToString
                    .Rows(n).Cells(16).Value = Val(Ent_Amt) 'Dt1.Rows(i).Item("EntAmount").ToString
                    .Rows(n).Cells(17).Value = Val(Ent_ReKgs) 'Dt1.Rows(i).Item("Rewinding_weight").ToString
                    '.Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Rewinding_Rate").ToString
                    .Rows(n).Cells(19).Value = Val(Ent_ReAmount) 'Dt1.Rows(i).Item("Rewinding_Amount").ToString
                    .Rows(n).Cells(20).Value = Val(Ent_PackBeams) 'Dt1.Rows(i).Item("Total_Pavu_Beam").ToString
                    .Rows(n).Cells(21).Value = Val(Ent_PackRate) 'Dt1.Rows(i).Item("Packing_Rate").ToString
                    .Rows(n).Cells(22).Value = Val(Ent_PackAmt) 'Dt1.Rows(i).Item("Packing_Amount").ToString
                    .Rows(n).Cells(23).Value = Val(Ent_WelBeams) 'Dt1.Rows(i).Item("Welding_Beams").ToString
                    .Rows(n).Cells(24).Value = Val(Ent_WelRate) 'Dt1.Rows(i).Item("Welding_Rate").ToString
                    .Rows(n).Cells(25).Value = Val(Ent_WelAmount) 'Dt1.Rows(i).Item("Welding_Amount").ToString
                    .Rows(n).Cells(26).Value = Val(Ent_otherCharges) ' Dt1.Rows(i).Item("Other_Charges").ToString
                    .Rows(n).Cells(27).Value = Trim(Ent_DisType)
                    .Rows(n).Cells(28).Value = Val(Ent_DisRate)
                    .Rows(n).Cells(29).Value = Val(Ent_DisAmount)
                    '.Rows(n).Cells(30).Value = Trim(Ent_SetCode)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()


            Da = New SqlClient.SqlDataAdapter("select a.*, lh.Ledger_Name, ch.Count_Name, mh.Mill_Name from Specification_Head a  Left Outer Join Ledger_Head lh ON  a.Ledger_IdNo = lh.Ledger_IdNo LEFT OUTER JOIN Count_Head ch ON a.Count_IdNo = ch.count_IdNo LEFT OUTER JOIN mill_head mh ON a.Mill_IdNo = mh.Mill_IdNo where a.Ledger_IdNo = " & Trim(LedIdNo) & " and a.Invoice_Code = '' Order by a.set_Date, For_OrderBy, set_no ", con)
            Dt1 = New DataTable
            NR = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("set_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("set_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("ends_name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("total_warping_net_weight").ToString
                    '.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("warp_weight").ToString
                    '.Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Rewinding_Weight").ToString
                    .Rows(n).Cells(12).Value = ""
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("set_Code").ToString

                    '.Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Warp_Weight").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Warp_Rate").ToString
                    '.Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Warp_Amount").ToString
                    '.Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Rewinding_weight").ToString
                    '.Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Rewinding_Rate").ToString
                    .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("Rewinding_Amount").ToString
                    .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("Total_Pavu_Beam").ToString
                    .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("Packing_Rate").ToString
                    '.Rows(n).Cells(22).Value = Dt1.Rows(i).Item("Packing_Amount").ToString
                    .Rows(n).Cells(23).Value = Dt1.Rows(i).Item("Welding_Beams").ToString
                    .Rows(n).Cells(24).Value = Dt1.Rows(i).Item("Welding_Rate").ToString
                    '.Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Welding_Amount").ToString
                    .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Other_Charges").ToString
                Next

            End If
            Dt1.Clear()

            'get_RateDetails()

            Call NetAmount_Calculation()
        End With

        pnl_Selection.Visible = True
        pnl_back.Enabled = False
        dgv_Pnl_Selection_Details.Focus()

    End Sub

    Private Sub Select_Statement(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Pnl_Selection_Details

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(12).Value = (Val(.Rows(RwIndx).Cells(12).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(12).Value) = 0 Then
                    .Rows(RwIndx).Cells(12).Value = ""
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next
                Else
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next
                End If
                If Val(dgv_Pnl_Selection_Details.CurrentRow.Cells(12).Value) = 1 Then
                    cnt = cnt + 1
                Else
                    cnt = cnt - 1
                End If

            End If

        End With

    End Sub

    Private Sub dgv_Pnl_Selection_Details_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Pnl_Selection_Details.CellClick
        Select_Statement(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer = 0
        Dim j As Integer = 0

        Try

            With dgv_Pnl_Selection_Details

                If .RowCount > 0 And RwIndx >= 0 Then

                    .Rows(RwIndx).Cells(12).Value = (Val(.Rows(RwIndx).Cells(12).Value) + 1) Mod 2
                    If Val(.Rows(RwIndx).Cells(12).Value) = 1 Then

                        '.Rows(RwIndx).Cells(7).Value = 1

                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(RwIndx).Cells(12).Value = ""
                        'For i = 0 To .Rows.Count - 1
                        For i = 0 To .Columns.Count - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                        Next
                        'Next

                    End If
                    'Close_Warping_Selection()

                    ' Close_selection()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE SELECT INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Pnl_Selection_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Pnl_Selection_Details.Enter

    End Sub

    Private Sub dgv_Pnl_Selection_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Pnl_Selection_Details.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Pnl_Selection_Details.CurrentCell.RowIndex >= 0 Then

                n = dgv_Pnl_Selection_Details.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub GST_Calculation()

        If Trim(UCase(Cbo_Tax_Type.Text)) <> "NO TAX" Then
            lbl_IGST_Amount.Text = Format(Val(lbl_Assessable_Value.Text) * Val(txt_IGST_Perc.Text) / 100, "#########0.00")
        End If

        '-CGST 
        lbl_CGST_Amount.Text = "0.00"
        If Trim(UCase(Cbo_Tax_Type.Text)) <> "NO TAX" Then
            lbl_CGST_Amount.Text = Format(Val(lbl_Assessable_Value.Text) * Val(txt_CGST_Perc.Text) / 100, "#########0.00")
        End If

        '-SGST 
        lbl_SGST_Amount.Text = "0.00"
        If Trim(UCase(Cbo_Tax_Type.Text)) <> "NO TAX" Then
            lbl_SGST_Amount.Text = Format(Val(lbl_Assessable_Value.Text) * Val(txt_SGST_Perc.Text) / 100, "#########0.00")
        End If

    End Sub

    Private Sub dgtxt_Pnl_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Pnl_Details.Enter
        dgv_Pnl_Selection_Details.EditingControl.BackColor = Color.PaleGreen
        dgv_Pnl_Selection_Details.EditingControl.ForeColor = Color.Blue
        dgv_Pnl_Selection_Details.SelectAll()
    End Sub


    Private Sub cbo_Sizing_Charges_Account_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_Charges_Account.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "")
    End Sub

    Private Sub cbo_Sizing_Charges_Account_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_Charges_Account.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_Charges_Account, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "")
        If e.KeyCode = 38 And cbo_Sizing_Charges_Account.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
            If cbo_DelieveryTo.Visible = True Then
                cbo_DelieveryTo.Focus()
            Else
                Cbo_SetNo.Focus()
            End If
        End If

        If e.KeyCode = 40 And cbo_Sizing_Charges_Account.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If dgv_Details.Visible = True Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Freight.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Sizing_Charges_Account_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing_Charges_Account.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_Charges_Account, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Freight.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Sizing_Charges_Account_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_Charges_Account.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Sizing_Charges_Account.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    'E-Invoice

    Private Sub btn_EInvoice_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click

        grp_EInvoice.Visible = True
        grp_EInvoice.BringToFront()
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        'grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2
        grp_EInvoice.Top = 3

    End Sub

    Private Sub btn_CheckConnectivity1_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity1.Click

        Dim einv As New eInvoice(Val(lbl_company.Tag))
        einv.GetAuthToken(rtbeInvoiceResponse)

        'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

    End Sub

    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

            Cmd.CommandText = "truncate table entrytemp"

            Cmd.ExecuteNonQuery()


            Cmd.CommandText = "Insert into e_Invoice_Details (                 Sl_No,         IsService    ,    Product_Description                  ,    HSN_Code              ,     Batch_Details ,         Quantity        ,          Unit  ,       Unit_Price   ,                                         Total_Amount     ,                                Discount        ,                                       Assessable_Amount                              ,                              GST_Rate                                                                         ,      SGST_Amount ,   IGST_Amount  , CGST_Amount ,  Cess_rate,  Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount, Other_Charge, Total_Item_Value, AttributesDetails      , Ref_Sales_Code )" &
                                                                      " Select  b.Sl_No  ,   1 as IsServc,    Mh.Mill_Name as producDescription ,    '998821' as HSN_Code  , '' as batchdetails,            b.Warp_Kgs ,             'KGS' as UOM,     b.Warp_Rate  ,   ( b.Total_Amount + (CASE WHEN b.sl_no = 1 then (a.Add_Less + a.Freight) else 0 end ) ) ,        0    ,        ( b.Total_Amount + (CASE WHEN b.sl_no = 1 then (a.Add_Less + a.Freight) else 0 end ) )  ,  (CASE WHEN a.IGST_Percentage<>0 THEN a.IGST_Percentage ELSE (CGST_Percentage+SGST_Percentage) END) as GstPerc , 0 AS SgstAmt,   0 AS CgstAmt,   0 AS igstAmt,   0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                                                                      " from  Invoice_Head a INNER JOIN Invoice_Details b On a.InVoice_Code = b.InVoice_Code " &
                                                                      " Left OUter Join Mill_Head Mh on Mh.Mill_Idno = b.Mill_Idno " &
                                                                      " Left Outer Join Count_Head CH oN ch.Count_Idno = b.Count_Idno " &
                                                                      " Where a.InVoice_Code = '" & Trim(NewCode) & "'"
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
        einv.GenerateIRN(Val(lbl_company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Invoice_Head", "InVoice_Code", Trim(Pk_Condition), "INV")

    End Sub

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        If Len(Trim(txt_EInvoiceCancellationReson.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Invoice_Head", "InVoice_Code", con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)

    End Sub

    Private Sub btn_Refresh_E_Invoice_Info_Click(sender As Object, e As EventArgs) Handles btn_Get_QR_Code.Click

        'Dim CMD As New SqlClient.SqlCommand
        'CMD.Connection = con

        'CMD.CommandText = "DELETE FROM " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_refresh where IRN = '" & txt_eInvoiceNo.Text & "'"
        'CMD.ExecuteNonQuery()

        'CMD.CommandText = " INSERT INTO " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_Refresh ([IRN] ,[ACK_No] , [DOC_No] , [SEARCH_BY]  , [COMPANY_IDNO],[Update_Table] ,[Update_table_Unique_Code],[COMPANYGROUP_IDNO] ) VALUES " &
        '                  "('" & txt_eInvoiceNo.Text & "' ,'','','I'," & Val(Common_Procedures.CompIdNo).ToString & ",'Invoice_Head', 'E_Invoice_IRNO'," & Val(Common_Procedures.CompGroupIdNo).ToString & ")"
        'CMD.ExecuteNonQuery()

        'Shell(Application.StartupPath & "\Refresh_IRN.EXE")



        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim einv As New eInvoice(Val(lbl_company.Tag))
        einv.GetIRNDetails(txt_eInvoiceNo.Text, NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Invoice_Head", "InVoice_Code", "INV")

    End Sub

    Private Sub txt_eInvoiceNo_TextChanged(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.TextChanged
        txt_IR_No.Text = txt_eInvoiceNo.Text
    End Sub

    Private Sub btn_refresh_Click(sender As Object, e As EventArgs) Handles btn_refresh.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

        Else
            txt_AddLess.Focus()

        End If
    End Sub

    Private Sub chk_TCS_Tax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCS_Tax.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub chk_TCSAmount_RoundOff_STS_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCSAmount_RoundOff_STS.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub get_Ledger_TotalSales()
        Dim Led_ID As Integer
        Dim NewCode As String
        Dim vOrdbyNo As String

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_Invoice_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Party_Name.Text)
            vOrdbyNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_Invoice_No.Text))

            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"

            Common_Procedures.get_TotalSales_Value_of_Party(con, Val(lbl_company.Tag), Common_Procedures.FnYearCode, Pk_Condition, NewCode, Led_ID, vOrdbyNo, dtp_Date, lbl_TotalSales_Amount_Current_Year, lbl_TotalSales_Amount_Previous_Year)

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE GETTIG TOTAL SALES....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub txt_TcsPerc_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TcsPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TcsPerc_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TcsPerc.TextChanged
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
End Class

