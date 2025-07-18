Public Class SizingSoft_Invoice_VAT
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SZINV-"
    Private PkCondition_GST As String = "GSINV-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Private Print_PDF_Status As Boolean = False

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private prn_HdDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_Status As Integer
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_Count As Integer = 0

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_back.Enabled = True
        pnl_Filter.Visible = False
        Print_PDF_Status = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black
        dtp_date.Text = ""
        cbo_partyname.Text = ""
        cbo_partyname.Tag = ""
        cbo_setno.Text = ""

        cbo_OnAccount.Text = ""

        txt_sizingparticulars1.Text = "SIZING CHARGES"
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
        txt_DiscountRate.Text = "0.00"
        lbl_DiscountAmount.Text = "0.00"

        txt_vanrentparticulars.Text = "VAN RENT"
        txt_VanRentAmount.Text = "0.00"
        txt_PackingBeam.Text = ""

        chk_vat1.Checked = False
        txt_VatGrossPerc1.Text = ""
        txt_vat1particular.Text = "VAT @ 5% ON STARCH & CHEMICALS"
        lbl_VatGross1.Text = ""
        txt_VatPerc1.Text = "5"
        lbl_VatAmount1.Text = "0.00"

        chk_vat2.Checked = False
        txt_VatGrossPerc2.Text = ""
        txt_vat2particular.Text = "VAT @ 14.5% ON MUTTON TALLOW"
        lbl_VatGross2.Text = ""
        txt_VatPerc2.Text = "14.5"
        lbl_VatAmount2.Text = "0.00"

        txt_packingparticulars.Text = "PACKING CHARGES"
        txt_PackingRate.Text = "0.00"
        lbl_PackingAmount.Text = "0.00"

        lbl_NetAmount.Text = "0.00"

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

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

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Invoice_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.InVoice_Code = '" & Trim(NewCode) & "' and a.Invoice_Code NOT LIke '" & Trim(PkCondition_GST) & "%'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Invoice_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Invoice_Date").ToString
                cbo_partyname.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_partyname.Tag = Trim(cbo_partyname.Text)
                cbo_setno.Text = dt1.Rows(0).Item("SetCode_ForSelection").ToString

                cbo_OnAccount.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("OnAccount_IdNo").ToString))

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

                If Val(dt1.Rows(0).Item("Vat_Status1").ToString) = 1 Then chk_vat1.Checked = True
                txt_vat1particular.Text = dt1.Rows(0).Item("Vat_Text1").ToString
                txt_VatGrossPerc1.Text = Format(Val(dt1.Rows(0).Item("Vat_Assessable_Percentage1").ToString), "#########0.00")
                lbl_VatGross1.Text = Format(Val(dt1.Rows(0).Item("Vat_Assessable_Value1").ToString), "#########0.00")
                txt_VatPerc1.Text = Format(Val(dt1.Rows(0).Item("Vat_Percentage1").ToString), "#########0.00")
                lbl_VatAmount1.Text = Format(Val(dt1.Rows(0).Item("Vat_Amount1").ToString), "#########0.00")

                If Val(dt1.Rows(0).Item("Vat_Status2").ToString) = 1 Then chk_vat2.Checked = True
                txt_vat2particular.Text = dt1.Rows(0).Item("Vat_Text2").ToString
                txt_VatGrossPerc2.Text = Format(Val(dt1.Rows(0).Item("Vat_Assessable_Percentage2").ToString), "#########0.00")
                lbl_VatGross2.Text = Format(Val(dt1.Rows(0).Item("Vat_Assessable_Value2").ToString), "#########0.00")
                txt_VatPerc2.Text = Format(Val(dt1.Rows(0).Item("Vat_Percentage2").ToString), "#########0.00")
                lbl_VatAmount2.Text = Format(Val(dt1.Rows(0).Item("Vat_Amount2").ToString), "#########0.00")

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

                txt_discountparticulars.Text = dt1.Rows(0).Item("Discount_Text").ToString
                cbo_DiscountType.Text = dt1.Rows(0).Item("Discount_Type").ToString
                txt_DiscountRate.Text = Format(Val(dt1.Rows(0).Item("Discount_Percentage").ToString), "#########0.000")
                lbl_DiscountAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")

                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "#########0.00")


                chk_Printed.Checked = False
                chk_Printed.Enabled = False
                chk_Printed.Visible = False
                If Val(dt1.Rows(0).Item("PrintOut_Status").ToString) = 1 Then
                    chk_Printed.Checked = True
                    chk_Printed.Visible = True
                    If Val(Common_Procedures.User.IdNo) = 1 Then
                        chk_Printed.Enabled = True
                    End If
                End If

            Else

                new_record()

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_partyname.Visible And cbo_partyname.Enabled Then cbo_partyname.Focus()

    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Invoice_Entry, New_Entry, Me, con, "Invoice_Head", "Invoice_Code", NewCode, "Invoice_Date", "(Invoice_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Receipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)

            cmd.CommandText = "Update Specification_Head set invoice_code = '', invoice_increment = invoice_increment - 1 Where invoice_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Dt1.Dispose()
            Da1.Dispose()
            cmd.Dispose()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

            inpno = InputBox("Enter New Invoice.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Invoice_No from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Invoice No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 Invoice_No from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Invoice_Code NOT LIke '" & Trim(PkCondition_GST) & "%' Order by for_Orderby, Invoice_No"
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

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Invoice_No from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Invoice_Code NOT LIke '" & Trim(PkCondition_GST) & "%'  Order by for_Orderby desc, Invoice_No desc"
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Invoice_No from Invoice_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Invoice_Code NOT LIke '" & Trim(PkCondition_GST) & "%' Order by for_Orderby, Invoice_No"
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

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Invoice_No from Invoice_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_company.Tag)) & " and  Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Invoice_Code NOT LIke '" & Trim(PkCondition_GST) & "%' Order by for_Orderby desc,Invoice_No desc"
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

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0
        Dim SetCdSel As String

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Invoice_Code NOT LIke '" & Trim(PkCondition_GST) & "%'", con)
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


            da = New SqlClient.SqlDataAdapter("select top 1 * from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Invoice_Code NOT LIke '" & Trim(PkCondition_GST) & "%' Order by for_Orderby desc, invoice_no desc", con)
            dt1 = New DataTable
            da.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_sizingparticulars1.Text = dt1.Rows(0).Item("Sizing_Text1").ToString

                txt_sizingparticulars2.Text = dt1.Rows(0).Item("Sizing_Text2").ToString

                txt_sizingparticulars3.Text = dt1.Rows(0).Item("Sizing_Text3").ToString

                If Val(dt1.Rows(0).Item("Vat_Status1").ToString) = 1 Then chk_vat1.Checked = True
                txt_vat1particular.Text = dt1.Rows(0).Item("Vat_Text1").ToString
                txt_VatGrossPerc1.Text = Format(Val(dt1.Rows(0).Item("Vat_Assessable_Percentage1").ToString), "#########0.00")
                txt_VatPerc1.Text = Format(Val(dt1.Rows(0).Item("Vat_Percentage1").ToString), "#########0.00")

                If Val(dt1.Rows(0).Item("Vat_Status2").ToString) = 1 Then chk_vat2.Checked = True
                txt_vat2particular.Text = dt1.Rows(0).Item("Vat_Text2").ToString
                txt_VatGrossPerc2.Text = Format(Val(dt1.Rows(0).Item("Vat_Assessable_Percentage2").ToString), "#########0.00")
                txt_VatPerc2.Text = Format(Val(dt1.Rows(0).Item("Vat_Percentage2").ToString), "#########0.00")

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

                End If
                dt1.Clear()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            dt.Dispose()
            da.Dispose()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Invoice.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Invoice_No from Invoice_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Invoice.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim OnAc_id As Integer = 0
        Dim vSetCd As String, vSetNo As String
        Dim Vat1Sts As Integer, Vat2STS As Integer
        Dim VouBil As String = ""


        NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Invoice_Entry, New_Entry, Me, con, "invoice_Head", "Invoice_Code", NewCode, "Invoice_Date", "(Invoice_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Invoice_No desc", dtp_date.Value.Date) = False Then Exit Sub

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Invoice_Entry, New_Entry) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()
            Exit Sub
        End If

        If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)

        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_partyname.Enabled Then cbo_partyname.Focus()
            Exit Sub
        End If

        If Trim(cbo_setno.Text) = "" Then
            MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_setno.Enabled Then cbo_setno.Focus()
            Exit Sub
        End If

        OnAc_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_OnAccount.Text)

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

        Vat1Sts = 0
        If chk_vat1.Checked = True Then Vat1Sts = 1

        Vat2STS = 0
        If chk_vat2.Checked = True Then Vat2STS = 1

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Invoice_Head", "Invoice_Code", "For_OrderBy", "(Invoice_Code NOT LIke '" & Trim(PkCondition_GST) & "%' )", Val(lbl_company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@InvoiceDate", dtp_date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Invoice_Head(Invoice_Code, Company_IdNo, Invoice_No, for_OrderBy, Invoice_Date, Ledger_IdNo, SetCode_ForSelection, Set_Code, Set_No, OnAccount_IdNo, Sizing_Text1, Sizing_Weight1, Sizing_Rate1, Sizing_Amount1, Sizing_Text2, Sizing_Weight2, Sizing_Rate2, Sizing_Amount2, Sizing_Text3, Sizing_Weight3, Sizing_Rate3, Sizing_Amount3, Vat_Status1, Vat_Text1, Vat_Assessable_Percentage1, Vat_Assessable_Value1, Vat_Percentage1, Vat_Amount1, Vat_Status2, Vat_Text2, Vat_Assessable_Percentage2, Vat_Assessable_Value2, Vat_Percentage2, Vat_Amount2, SampleSet_Text, SampleSet_Amount, VanRent_Text, VanRent_Amount, Packing_Beam, Packing_Text, Packing_Rate, Packing_Amount, Rewinding_Text, Rewinding_Weight, Rewinding_Rate, Rewinding_Amount, Welding_Beam, Welding_Text, Welding_Rate, Welding_Amount, OtherCharges_Text, OtherCharges_Amount, Discount_Text, Discount_Type, Discount_Percentage, Discount_Amount, Net_Amount ) " &
                                    "  Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @InvoiceDate, " & Val(led_id) & ", '" & Trim(cbo_setno.Text) & "', '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "', " & Val(OnAc_id) & ", '" & Trim(txt_sizingparticulars1.Text) & "', " & Val(lbl_SizingQty1.Text) & "," & Val(txt_SizingRate1.Text) & "," & Val(lbl_SizingAmount1.Text) & ", '" & Trim(txt_sizingparticulars2.Text) & "', " & Val(lbl_SizingQty2.Text) & "," & Val(txt_SizingRate2.Text) & "," & Val(lbl_SizingAmount2.Text) & ", '" & Trim(txt_sizingparticulars3.Text) & "', " & Val(lbl_SizingQty3.Text) & "," & Val(txt_SizingRate3.Text) & "," & Val(lbl_SizingAmount3.Text) & ", " & Val(Vat1Sts) & ", '" & Trim(txt_vat1particular.Text) & "', " & Val(txt_VatGrossPerc1.Text) & ", " & Val(lbl_VatGross1.Text) & ", " & Val(txt_VatPerc1.Text) & ", " & Val(lbl_VatAmount1.Text) & ", " & Val(Vat2STS) & ", '" & Trim(txt_vat2particular.Text) & "', " & Val(txt_VatGrossPerc2.Text) & ", " & Val(lbl_VatGross2.Text) & ", " & Val(txt_VatPerc2.Text) & ", " & Val(lbl_VatAmount2.Text) & ", '" & Trim(txt_samplesparticulars.Text) & "', " & Val(txt_SampleSetAmount.Text) & ", '" & Trim(txt_vanrentparticulars.Text) & "', " & Val(txt_VanRentAmount.Text) & ", " & Val(txt_PackingBeam.Text) & ", '" & Trim(txt_packingparticulars.Text) & "', " & Val(txt_PackingRate.Text) & ", " & Val(lbl_PackingAmount.Text) & ", '" & Trim(txt_rewindingparticulars.Text) & "', " & Val(txt_RewindingQuantity.Text) & ", " & Val(txt_RewindingRate.Text) & ", " & Val(lbl_RewindingAmount.Text) & ", " & Val(txt_WeldingBeam.Text) & ", '" & Trim(txt_weldingparticulars.Text) & "', " & Val(txt_WeldingRate.Text) & ", " & Val(lbl_WeldingAmount.Text) & ", '" & Trim(txt_otherchargeparticulars.Text) & "', " & Val(txt_OtherChargesAmount.Text) & ", '" & Trim(txt_discountparticulars.Text) & "', '" & Trim(cbo_DiscountType.Text) & "', " & Val(txt_DiscountRate.Text) & ", " & Val(lbl_DiscountAmount.Text) & ", " & Val(lbl_NetAmount.Text) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Specification_Head set invoice_code = '', invoice_increment = invoice_increment - 1 Where invoice_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Invoice_Head set Invoice_Date = @InvoiceDate, Ledger_IdNo = " & Val(led_id) & ", SetCode_ForSelection = '" & Trim(cbo_setno.Text) & "', Set_Code = '" & Trim(vSetCd) & "', Set_No = '" & Trim(vSetNo) & "', OnAccount_IdNo = " & Val(OnAc_id) & ", Sizing_Text1 = '" & Trim(txt_sizingparticulars1.Text) & "', Sizing_Weight1 = " & Val(lbl_SizingQty1.Text) & ", Sizing_Rate1 = " & Val(txt_SizingRate1.Text) & ", Sizing_Amount1 = " & Val(lbl_SizingAmount1.Text) & ", Sizing_Text2 = '" & Trim(txt_sizingparticulars2.Text) & "', Sizing_Weight2 = " & Val(lbl_SizingQty2.Text) & ", Sizing_Rate2 = " & Val(txt_SizingRate2.Text) & ", Sizing_Amount2 = " & Val(lbl_SizingAmount2.Text) & ", Sizing_Text3 = '" & Trim(txt_sizingparticulars3.Text) & "', Sizing_Weight3 = " & Val(lbl_SizingQty3.Text) & ", Sizing_Rate3 = " & Val(txt_SizingRate3.Text) & ", Sizing_Amount3 = " & Val(lbl_SizingAmount3.Text) & ", Vat_Status1 = " & Val(Vat1Sts) & ", Vat_Text1 = '" & Trim(txt_vat1particular.Text) & "', Vat_Assessable_Percentage1 = " & Val(txt_VatGrossPerc1.Text) & ", Vat_Assessable_Value1 = " & Val(lbl_VatGross1.Text) & ", Vat_Percentage1 = " & Val(txt_VatPerc1.Text) & ", Vat_Amount1 = " & Val(lbl_VatAmount1.Text) & ", Vat_Status2 = " & Val(Vat2STS) & ", Vat_Text2 = '" & Trim(txt_vat2particular.Text) & "', Vat_Assessable_Percentage2 = " & Val(txt_VatGrossPerc2.Text) & ", Vat_Assessable_Value2 = " & Val(lbl_VatGross2.Text) & ", Vat_Percentage2 = " & Val(txt_VatPerc2.Text) & ", Vat_Amount2 = " & Val(lbl_VatAmount2.Text) & ", SampleSet_Text ='" & Trim(txt_samplesparticulars.Text) & "', SampleSet_Amount = " & Val(txt_SampleSetAmount.Text) & ", VanRent_Text = '" & Trim(txt_vanrentparticulars.Text) & "', VanRent_Amount = " & Val(txt_VanRentAmount.Text) & ", Packing_Beam = " & Val(txt_PackingBeam.Text) & ", Packing_Text = '" & Trim(txt_packingparticulars.Text) & "', Packing_Rate = " & Val(txt_PackingRate.Text) & ", Packing_Amount = " & Val(lbl_PackingAmount.Text) & ", Rewinding_Text = '" & Trim(txt_rewindingparticulars.Text) & "', Rewinding_Weight = " & Val(txt_RewindingQuantity.Text) & ", Rewinding_Rate = " & Val(txt_RewindingRate.Text) & ", Rewinding_Amount = " & Val(lbl_RewindingAmount.Text) & ", Welding_Beam = " & Val(txt_WeldingBeam.Text) & ", Welding_Text = '" & Trim(txt_weldingparticulars.Text) & "', Welding_Rate = " & Val(txt_WeldingRate.Text) & ", Welding_Amount = " & Val(lbl_WeldingAmount.Text) & ", OtherCharges_Text = '" & Trim(txt_otherchargeparticulars.Text) & "', OtherCharges_Amount = " & Val(txt_OtherChargesAmount.Text) & ", Discount_Text = '" & Trim(txt_discountparticulars.Text) & "', Discount_Type = '" & Trim(cbo_DiscountType.Text) & "', Discount_Percentage = " & Val(txt_DiscountRate.Text) & ", Discount_Amount = " & Val(lbl_DiscountAmount.Text) & ", Net_Amount = " & Val(lbl_NetAmount.Text) & " Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            nr = 0
            cmd.CommandText = "Update Specification_Head set invoice_code = '" & Trim(NewCode) & "', invoice_increment = invoice_increment + 1 Where invoice_code = '' and setcode_forSelection = '" & Trim(cbo_setno.Text) & "' and Set_Code = '" & Trim(vSetCd) & "' and Ledger_IdNo = " & Str(Val(led_id))
            nr = cmd.ExecuteNonQuery()
            If nr = 0 Then
                Throw New ApplicationException("Invalid Set Details - Mismatch of PartyName and Set Details")
                Exit Sub
            End If


            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Cr_ID = 2
            If Val(OnAc_id) <> 0 Then
                Dr_ID = Val(OnAc_id)
            Else
                Dr_ID = Val(led_id)
            End If

            cmd.CommandText = "Insert into Voucher_Head(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) " &
                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Siz.Invoice', @InvoiceDate, " & Str(Val(Cr_ID)) & ", " & Str(Val(Dr_ID)) & ", " & Str(Val(lbl_NetAmount.Text)) & ", 'Bill No. : " & Trim(lbl_InvoiceNo.Text) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " &
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Siz.Invoice', @InvoiceDate, 1, " & Str(Val(Cr_ID)) & ", " & Str(Val(lbl_NetAmount.Text)) & ", 'Bill No. : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " &
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Siz.Invoice', @InvoiceDate, 2, " & Str(Val(Dr_ID)) & ", " & Str(-1 * Val(lbl_NetAmount.Text)) & ", 'Bill No. : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            cmd.ExecuteNonQuery()

            '---Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_company.Tag), dtp_date.Text, led_id, Trim(lbl_InvoiceNo.Text), 0, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
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

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1017" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '---- Sri Bhagavan Sizing (Palladam)
            '    If New_Entry = True Then
            '        new_record()
            '    End If
            'Else
            '    move_record(lbl_InvoiceNo.Text)
            'End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da.Dispose()
            tr.Dispose()
            cmd.Dispose()

            If cbo_partyname.Enabled And cbo_partyname.Visible Then cbo_partyname.Focus()


        End Try


    End Sub

    Private Sub Invoice_VAT_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_partyname.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_partyname.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_OnAccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_OnAccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Invoice_VAT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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


        'btn_UserModification.Visible = False
        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            'btn_UserModification.Visible = True
            chk_Printed.Enabled = True
        End If

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_partyname.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_setno.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OnAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_setno.GotFocus, AddressOf ControlGotFocus
        ''AddHandler txt_sizingparticulars1.GotFocus, AddressOf ControlGotFocus
        ''AddHandler txt_sizingparticulars2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PackingBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PackingRate.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_vat1particular.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_vat2particular.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatGrossPerc1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatGrossPerc2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatPerc1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatPerc2.GotFocus, AddressOf ControlGotFocus
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

        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Close.GotFocus, AddressOf ControlGotFocus


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
        AddHandler txt_VatGrossPerc1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VatGrossPerc2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VatPerc1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VatPerc2.LostFocus, AddressOf ControlLostFocus
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
        'AddHandler txt_rewindingparticulars.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RewindingQuantity.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RewindingRate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DiscountType.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_OnAccount.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Close.GotFocus, AddressOf ControlGotFocus





        AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_sizingparticulars3.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_PackingRate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_pt2particular.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VatGrossPerc1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VatGrossPerc2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VatPerc1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VatPerc2.KeyDown, AddressOf TextBoxControlKeyDown
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

        'AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_sizingparticulars3.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_PackingBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PackingRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VatGrossPerc1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VatGrossPerc2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VatPerc1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VatPerc2.KeyPress, AddressOf TextBoxControlKeyPress
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

    Private Sub Invoice_VAT_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Invoice_VAT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                        con.Dispose()
                        Common_Procedures.Last_Closed_FormName = Me.Name
                    End If

                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
                    con.Dispose()
                    Common_Procedures.Last_Closed_FormName = Me.Name
                End If

            Else

                Me.Close()
                con.Dispose()
                Common_Procedures.Last_Closed_FormName = Me.Name
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_partyname_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_partyname.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        cbo_partyname.Tag = Trim(cbo_partyname.Text)
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_partyname, dtp_date, cbo_setno, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_partyname.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_partyname, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            get_RateDetails()
            cbo_setno.Focus()
        End If
    End Sub

    Private Sub cbo_OnAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OnAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")
    End Sub

    Private Sub cbo_onAccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OnAccount, cbo_setno, txt_SizingRate1, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")
    End Sub



    Private Sub cbo_onAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OnAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OnAccount, txt_SizingRate1, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10  or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0 or Ledger_IdNo = 1)")

    End Sub


    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
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

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub cbo_setno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_setno.GotFocus
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    'Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyDown
    '    If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    '    If e.KeyCode = 38 Then txt_DiscountRate.Focus() '   SendKeys.Send("+{TAB}")
    'End Sub

    Private Sub cbo_setno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_setno.KeyDown
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_setno, cbo_partyname, cbo_OnAccount, "Specification_Head", "setcode_forSelection", "(" & Condt & ")", "(set_code = '')")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_setno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_setno.KeyPress
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
                get_Set_Details(cbo_setno.Text)
                cbo_OnAccount.Focus()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
    '        MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
    '        MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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




    Private Sub txt_vat1quantity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatGrossPerc1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub



    Private Sub txt_vat1rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatGrossPerc2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub



    Private Sub txt_vat2beam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatPerc1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub



    Private Sub txt_vat2quantity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatPerc2.KeyPress
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

        New_Code = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.count_name from Specification_Head a  Left Outer Join Ledger_Rate_Details d ON  a.Ledger_IdNo = D.Ledger_IdNo, Ledger_Head b, count_head c where a.setcode_forSelection = '" & Trim(SelcSetCd) & "' and (a.invoice_code = '' or a.invoice_code = '" & Trim(New_Code) & "') and a.Ledger_IdNo = b.Ledger_IdNo and a.Count_IdNo = c.Count_IdNo    ", con)
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

            txt_sizingparticulars1.Text = Dt1.Rows(0).Item("Count_Name").ToString & IIf(InStr(1, UCase(Dt1.Rows(0).Item("Count_Name").ToString), "S") = 0, "s", "") & " - " & eds(0) & " SIZING CHARGES"
            lbl_SizingQty1.Text = "0.000"
            If UBound(wwg) >= 0 Then lbl_SizingQty1.Text = Format(Val(wwg(0)), "########0.000")

            If UBound(eds) >= 1 Then
                txt_sizingparticulars2.Text = Dt1.Rows(0).Item("Count_Name").ToString & IIf(InStr(1, UCase(Dt1.Rows(0).Item("Count_Name").ToString), "S") = 0, "s", "") & " - " & eds(1) & " SIZING CHARGES"
                lbl_SizingQty2.Text = "0.000"
                If UBound(wwg) >= 1 Then lbl_SizingQty2.Text = Format(Val(wwg(1)), "########0.000")
            End If

            If UBound(eds) >= 2 Then
                txt_sizingparticulars3.Text = Dt1.Rows(0).Item("Count_Name").ToString & IIf(InStr(1, UCase(Dt1.Rows(0).Item("Count_Name").ToString), "S") = 0, "s", "") & " - " & eds(2) & " SIZING CHARGES"
                lbl_SizingQty3.Text = "0.000"
                If UBound(wwg) >= 2 Then lbl_SizingQty3.Text = Format(Val(wwg(2)), "########0.000")
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
            Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, (cbo_partyname.Text))
            Da = New SqlClient.SqlDataAdapter("select a.* from Ledger_Rate_Details a where " & Val(End_Id) & " between a.Ends_From and a.Ends_To and a.Ledger_IdNo=" & Val(Led_Id) & " and a.Count_Idno = " & Val(Cnt_Id) & "    ", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)

            If Dt2.Rows.Count > 0 Then
                txt_SizingRate1.Text = Format(Val(Dt2.Rows(0).Item("Rate").ToString), "########0.00")
            End If
            Vat1_Calculation()
            Vat2_Calculation()

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

            .RowStyles(3).Height = 0
            txt_vat1particular.Visible = False
            lbl_VatGross1.Visible = False
            lbl_VatAmount1.Visible = False
            FlowLayoutPanel1.Visible = False
            FlowLayoutPanel2.Visible = False

            .RowStyles(4).Height = 0
            txt_vat2particular.Visible = False
            lbl_VatGross2.Visible = False
            lbl_VatAmount2.Visible = False
            FlowLayoutPanel3.Visible = False
            FlowLayoutPanel4.Visible = False

            .Visible = True

        End With

    End Sub

    Private Sub chk_vat1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_vat1.CheckedChanged
        Vat1_Calculation()
    End Sub

    Private Sub Vat1_Calculation()
        Dim GrsAmt As Single
        Dim AssAmt As Single
        Dim VtGrsPrc As Single
        Dim VtAmt As Single

        lbl_VatGross1.Text = "0.00"
        lbl_VatAmount1.Text = "0.00"

        GrsAmt = Val(lbl_SizingAmount1.Text) + Val(lbl_SizingAmount2.Text) + Val(lbl_SizingAmount3.Text)

        VtGrsPrc = Val(txt_VatGrossPerc1.Text)
        'If Val(VtGrsPrc) = 0 Then VtGrsPrc = 100

        AssAmt = Format(GrsAmt * VtGrsPrc / 100, "#########0")
        lbl_VatGross1.Text = Format(Val(AssAmt), "#########0.00")

        If chk_vat1.Checked = True Then

            VtAmt = Format(Val(lbl_VatGross1.Text) * Val(txt_VatPerc1.Text) / 100, "#########0")
            lbl_VatAmount1.Text = Format(Val(VtAmt), "#########0.00")

        End If

        Call NetAmount_Calculation()

    End Sub

    Private Sub chk_vat2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_vat2.CheckedChanged
        Vat2_Calculation()
    End Sub

    Private Sub Vat2_Calculation()
        Dim GrsAmt As Single
        Dim AssAmt As Single
        Dim VtGrsPrc As Single
        Dim VtAmt As Single

        lbl_VatGross2.Text = "0.00"
        lbl_VatAmount2.Text = "0.00"

        GrsAmt = Val(lbl_SizingAmount1.Text) + Val(lbl_SizingAmount2.Text) + Val(lbl_SizingAmount3.Text)

        VtGrsPrc = Val(txt_VatGrossPerc2.Text)
        'If Val(VtGrsPrc) = 0 Then VtGrsPrc = 100

        AssAmt = Format(GrsAmt * VtGrsPrc / 100, "#########0")
        lbl_VatGross2.Text = Format(Val(AssAmt), "#########0.00")

        If chk_vat2.Checked = True Then
            VtAmt = Format(Val(lbl_VatGross2.Text) * Val(txt_VatPerc2.Text) / 100, "#########0")
            lbl_VatAmount2.Text = Format(Val(VtAmt), "#########0.00")
        End If

        Call NetAmount_Calculation()

    End Sub

    Private Sub txt_SizingRate1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SizingRate1.LostFocus
        txt_SizingRate1.Text = Format(Val(txt_SizingRate1.Text), "##########0.00")
    End Sub

    Private Sub txt_SizingRate1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_SizingRate1.TextChanged
        lbl_SizingAmount1.Text = Format(Val(lbl_SizingQty1.Text) * Val(txt_SizingRate1.Text), "##########0.00")
    End Sub

    Private Sub txt_SizingRate2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SizingRate2.LostFocus
        txt_SizingRate2.Text = Format(Val(txt_SizingRate2.Text), "##########0.00")
    End Sub

    Private Sub txt_SizingRate2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_SizingRate2.TextChanged
        lbl_SizingAmount2.Text = Format(Val(lbl_SizingQty2.Text) * Val(txt_SizingRate2.Text), "##########0.00")
    End Sub

    Private Sub txt_SizingRate3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SizingRate3.LostFocus
        txt_SizingRate3.Text = Format(Val(txt_SizingRate3.Text), "##########0.00")
    End Sub

    Private Sub txt_SizingRate3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_SizingRate3.TextChanged
        lbl_SizingAmount3.Text = Format(Val(lbl_SizingQty3.Text) * Val(txt_SizingRate3.Text), "##########0.00")
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
        Vat1_Calculation()
        Vat2_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_SizingAmount2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SizingAmount2.TextChanged
        Vat1_Calculation()
        Vat2_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_SizingAmount3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SizingAmount3.TextChanged
        Vat1_Calculation()
        Vat2_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_VatAmount1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_VatAmount1.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_VatAmount2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_VatAmount2.TextChanged
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
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_RewindingAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_RewindingAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_WeldingAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_WeldingAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_OtherChargesAmount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_OtherChargesAmount.LostFocus
        txt_OtherChargesAmount.Text = Format(Val(txt_OtherChargesAmount.Text), "##########0.00")
    End Sub

    Private Sub txt_OtherChargesAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_OtherChargesAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_DiscountAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_DiscountAmount.TextChanged
        NetAmount_Calculation()
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

    Private Sub txt_VatGrossPerc1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_VatGrossPerc1.LostFocus
        txt_VatGrossPerc1.Text = Format(Val(txt_VatGrossPerc1.Text), "##########0.00")
    End Sub

    Private Sub txt_VatGrossPerc1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_VatGrossPerc1.TextChanged
        Vat1_Calculation()
    End Sub

    Private Sub txt_VatGrossPerc2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_VatGrossPerc2.LostFocus
        txt_VatGrossPerc2.Text = Format(Val(txt_VatGrossPerc2.Text), "##########0.00")
    End Sub

    Private Sub txt_VatGrossPerc2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_VatGrossPerc2.TextChanged
        Vat2_Calculation()
    End Sub

    Private Sub lbl_VatGross1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_VatGross1.TextChanged
        txt_VatPerc1_TextChanged(sender, e)
    End Sub



    Private Sub txt_VatPerc1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_VatPerc1.TextChanged
        Dim VtAmt As Single
        lbl_VatAmount1.Text = "0.00"
        If chk_vat1.Checked = True Then
            VtAmt = Format(Val(lbl_VatGross1.Text) * Val(txt_VatPerc1.Text) / 100, "#########0")
            lbl_VatAmount1.Text = Format(Val(VtAmt) / 100, "#########0.00")
        End If
    End Sub

    Private Sub lbl_VatGross2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_VatGross2.TextChanged
        txt_VatPerc2_TextChanged(sender, e)
    End Sub





    Private Sub txt_VatPerc2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_VatPerc2.TextChanged
        Dim VtAmt As Single
        lbl_VatAmount2.Text = "0.00"
        If chk_vat2.Checked = True Then
            VtAmt = Format(Val(lbl_VatGross2.Text) * Val(txt_VatPerc2.Text) / 100, "#########0")
            lbl_VatAmount2.Text = Format(Val(VtAmt), "#########0.00")
        End If
    End Sub



    Private Sub txt_PackingBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PackingBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub


    Private Sub txt_VatGrossPerc1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VatGrossPerc1.KeyDown
        If e.KeyCode = 40 Then txt_VatPerc1.Focus() ' SendKeys.Send("{TAB}")
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

    Private Sub txt_VatGrossPerc1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatGrossPerc1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub



    Private Sub txt_VatGrossPerc2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatGrossPerc2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub chk_vat1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_vat1.KeyPress
        If Asc(e.KeyChar) = 13 Then txt_VatGrossPerc1.Focus() 'SendKeys.Send("{TAB}")
    End Sub

    Private Sub chk_vat2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_vat2.KeyPress
        If Asc(e.KeyChar) = 13 Then txt_VatGrossPerc2.Focus() 'SendKeys.Send("{TAB}")
    End Sub


    Private Sub cbo_DiscountType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DiscountType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DiscountType, txt_DiscountRate, "", "", "", "")
    End Sub

    Private Sub cbo_DiscountType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DiscountType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DiscountType, txt_OtherChargesAmount, txt_DiscountRate, "", "", "", "")
    End Sub

    Private Sub txt_VatPerc1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_VatPerc1.LostFocus
        txt_VatPerc1.Text = Format(Val(txt_VatPerc1.Text), "##########0.00")
    End Sub

    Private Sub txt_VatPerc2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_VatPerc2.LostFocus
        txt_VatPerc2.Text = Format(Val(txt_VatPerc2.Text), "##########0.00")
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
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub NetAmount_Calculation()
        Dim GrsAmt As Single
        Dim Tot As Single

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

        Tot = Format(Val(GrsAmt) + Val(lbl_VatAmount1.Text) + Val(lbl_VatAmount2.Text) + Val(txt_SampleSetAmount.Text) + Val(txt_VanRentAmount.Text) + Val(lbl_PackingAmount.Text) + Val(lbl_RewindingAmount.Text) + Val(lbl_WeldingAmount.Text) + Val(txt_OtherChargesAmount.Text) - Val(lbl_DiscountAmount.Text), "##########0")

        lbl_NetAmount.Text = Format(Val(Tot), "#########0.00")

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palladam)
            pnl_Print.Visible = True
            pnl_back.Enabled = False
            If btn_Print_Invoice.Enabled And btn_Print_Invoice.Visible Then
                btn_Print_Invoice.Focus()
            End If
        Else
            printing_invoice()

        End If


    End Sub

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim Def_PrntrNm As String = ""

        NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Invoice_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

        prn_InpOpts = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Then '---- Prakash Sizing (Somanur)
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")
        End If

        If Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-2" Or Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-4" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then '---- WinTraack Textiles Private Limited(Sizing Unit)
                Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                ' ppd.ClientSize = New Size(600, 600)

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
        Dim NewCode As String

        NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt = New DataTable
        prn_PageNo = 0
        prn_Count = 0

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*  from Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON (case when a.OnAccount_IdNo <> 0 then a.OnAccount_IdNo else a.Ledger_IdNo end) = c.Ledger_IdNo  where a.company_idno = " & Str(Val(lbl_company.Tag)) & " and a.Invoice_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(prn_HdDt)


            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then
            If prn_Status = 1 Then
                Printing_Format1(e)
            Else
                Printing_Format3(e)
            End If
        Else
            If Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-2" Then
                Printing_Format2(e)
            ElseIf Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-3" Then
                Printing_Format3(e)
            ElseIf Trim(UCase(Common_Procedures.settings.InvoicePrint_Format)) = "FORMAT-4" Then
                Printing_Format4(e)

            Else
                Printing_Format1(e)
            End If
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font, p2Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GstNo As String, Cmp_PanNo As String
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
            .Right = 40
            .Top = 35
            .Bottom = 40 ' 50

            '.Left = 50  ' 50
            '.Right = 90  '60
            '.Top = 40
            '.Bottom = 40 ' 50
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

        TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then
            NoofItems_PerPage = 10 ' 15
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1426" Then
            NoofItems_PerPage = 16
        Else
            NoofItems_PerPage = 12 ' 15
        End If


        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 80 ' 70
        C2 = 340 ' 350
        C3 = 115 ' 105
        C4 = 90 '95
        C5 = PageWidth - (LMargin + C1 + C2 + C3 + C4)

        CenLn = C1 + C2 + (C3 \ 2)

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GstNo = "" : Cmp_PanNo = ""

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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN No : " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1426" Then '---- USHARANI SIZING (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GstNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, PageWidth, CurY, 1, 0, pFont)

        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth, CurY, 1, 0, pFont)

        End If

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY


        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "To  : ", LMargin + 10, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "JOB WORK BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1426" Then '---- USHARANI SIZING (Somanur)
            If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
            End If
        Else
            If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "TIN No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
            End If
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

        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
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

        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt + 10
        Next


        NetAmt = Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString)

        RndOff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then
        CurY = CurY + TxtHgt
        'Else
        CurY = CurY + TxtHgt
        'End If
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1426" Then

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Kindly send as your payment at the earliest by means of a draft.", LMargin + 40, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY, 0, 0, pFont)

            Juris = Common_Procedures.settings.Jurisdiction
            If Trim(Juris) = "" Then Juris = "TIRUPUR"

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "subject to " & Juris & " jurisdiction only.", LMargin + 40, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY
        End If
        LnAr(10) = CurY

        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" Then
        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 300, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        'End If
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)

        'CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + 200, CurY, LMargin + 200, LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + 460, CurY, LMargin + 460, LnAr(10))

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        e.HasMorePages = False

    End Sub
    Private Sub Printing_Format1_Halfsheet(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font, p2Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1006" Then '----DIVYA SIZING MILLS
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
        Else
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        End If



        With PrintDocument1.DefaultPageSettings.Margins

            .Left = 20  ' 50
            .Right = 130  '90 '60
            .Top = 20
            .Bottom = 40 ' 50
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

        TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then

            NoofItems_PerPage = 10 ' 15
        Else
            NoofItems_PerPage = 12 ' 15
        End If


        Erase LnAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        C1 = 80 ' 70
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then
            C2 = 340 ' 350
        Else
            C2 = 375 ' 350
        End If
        C3 = 120 ' 105
        C4 = 90 '95
        C5 = PageWidth - (LMargin + C1 + C2 + C3 + C4)

        CenLn = C1 + C2 + (C3 \ 2)

        CurY = TMargin
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

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth, CurY, 1, 0, pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" Then
            CurY = CurY + strHeight
        End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY


        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "To  : ", LMargin + 10, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "JOB WORK BILL", LMargin + CenLn, CurY - 7, 2, (PageWidth - LMargin - CenLn), p1Font)
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

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + CenLn + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + CenLn + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + CenLn + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "TIN No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + N1 + 10, CurY - TxtHgt + 5, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt
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

        If (prn_HdDt.Rows(0).Item("Packing_Amount").ToString) > 0 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then
                CurY = CurY + TxtHgt
            Else
                CurY = CurY + TxtHgt + 10
            End If
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Packing_Beam").ToString), LMargin + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If

        If (prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) > 0 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then
                CurY = CurY + TxtHgt
            Else
                CurY = CurY + TxtHgt + 10
            End If
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Text").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Weight").ToString, LMargin + C1 + C2 + C3 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Rate").ToString, LMargin + C1 + C2 + C3 + C4 - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" Then
            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt + 10
            Next

        End If
        NetAmt = Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString)

        RndOff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(NetAmt)
        CurY = CurY + TxtHgt + 10
        If Val(RndOff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(RndOff), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            NoofDets = NoofDets + 1
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then
            CurY = CurY + TxtHgt
        Else
            CurY = CurY + TxtHgt
        End If
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" Then
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Rupees  :  " & AmtInWrds, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY


            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms and Condition :", LMargin + 20, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Kindly send as your payment at the earliest by means of a draft.", LMargin + 40, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Appropriate rate of interest @ 24% will be charged from the date of invoice.", LMargin + 40, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases absolutely as soon as the goods have been handed over to the carriers.", LMargin + 40, CurY, 0, 0, pFont)

            Juris = Common_Procedures.settings.Jurisdiction
            If Trim(Juris) = "" Then Juris = "TIRUPUR"

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "subject to " & Juris & " jurisdiction only.", LMargin + 40, CurY, 0, 0, pFont)
        End If

        ' CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" Then
            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)

        'CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        'e.Graphics.DrawLine(Pens.Black, LMargin + 200, CurY, LMargin + 200, LnAr(10))
        'e.Graphics.DrawLine(Pens.Black, LMargin + 460, CurY, LMargin + 460, LnAr(10))

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

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

        NetAmt = Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString)

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

            If NoofDets <= 8 Then
                For I = NoofDets + 1 To 8
                    CurY = CurY + TxtHgt + 10
                    NoofDets = NoofDets + 1
                Next
            End If

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt + 10
            Next

            NetAmt = Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString)

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
            CurY = TMargin + 550
            Common_Procedures.Print_To_PrintDocument(e, AmtInWrds, CurX, CurY, 0, 0, pFont)

            'CurX = LMargin + 200
            'CurY = TMargin + 450
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) & "    Duplicate for Book No . B1", CurX, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            Da = New SqlClient.SqlDataAdapter("select * from Specification_Head where setcode_forSelection = '" & Trim(cbo_setno.Text) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                vSetCd = Dt1.Rows(0).Item("Set_Code").ToString
                vSetNo = Dt1.Rows(0).Item("set_no").ToString
            End If
            Dt1.Clear()

            MailTxt = "INVOICE " & vbCrLf & vbCrLf

            MailTxt = MailTxt & "INV.NO:" & Trim(lbl_InvoiceNo.Text) & vbCrLf & "DATE:" & Trim(dtp_date.Text) & vbCrLf & vbCrLf & "SET.NO:" & Trim(vSetNo) & vbCrLf & "AMOUNT:" & Trim(lbl_NetAmount.Text)

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            EMAIL_Entry.vSubJect = "Invoice for SetNo : " & Trim(vSetNo)
            EMAIL_Entry.vMessage = Trim(MailTxt)

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            Da = New SqlClient.SqlDataAdapter("select * from Specification_Head where setcode_forSelection = '" & Trim(cbo_setno.Text) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                vSetCd = Dt1.Rows(0).Item("Set_Code").ToString
                vSetNo = Dt1.Rows(0).Item("set_no").ToString
            End If
            Dt1.Clear()

            smstxt = "INVOICE " & vbCrLf & vbCrLf
            smstxt = smstxt & "INV.NO:" & Trim(lbl_InvoiceNo.Text) & vbCrLf & "DATE:" & Trim(dtp_date.Text) & vbCrLf & vbCrLf & "SET.NO:" & Trim(vSetNo) & vbCrLf & "AMOUNT:" & Trim(lbl_NetAmount.Text)

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
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

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Invoice_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_company.Tag)) & " and a.Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Invoice_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Invoice_No").ToString
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
            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Filter_SetNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_SetNo.KeyDown
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Filter_SetNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_SetNo.KeyPress
        Dim Led_ID As Integer = 0
        Dim Condt As String
        Dim NewCode As String

        Try

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
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

        NetAmt = Val(prn_HdDt.Rows(0).Item("Sizing_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("Sizing_Amount3").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString) + Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) + Val(prn_HdDt.Rows(0).Item("SampleSet_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("VanRent_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Rewinding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Welding_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("OtherCharges_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString)

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

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Update_PrintOut_Status(Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim vPrnSTS As Integer = 0


        Try

            NewCode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

End Class

