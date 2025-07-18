Public Class YarnProcessing_BillMaking
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNPBM-"
    Private Pk_Condition2 As String = "YPRTS-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private NoCalc_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_HdDt_VAT As New DataTable
    Private prn_DetDt_VAT As New DataTable
    Private DetSNo As Integer
    Private prn_DetDt_VAT1 As New DataTable
    Private prn_DetAr(200, 12) As String
    Private prn_DetMxIndx As Integer
    Private DetIndx As Integer
    Private prn_Count As Integer

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        NoCalc_Status = True

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        txt_billNo.Text = ""
        cbo_vataccount.Text = ""
        txt_vat.Text = ""
        lbl_vat.Text = ""
        txt_filter_billNo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))



        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_TaxType.Text = "GST-5"

        txt_DiscPerc.Text = ""
        txt_discAmount.Text = ""
        txt_before_addless_tax.Text = ""
        txt_Freight_Name.Text = "Freight"
        txt_Freight_Charge.Text = ""
        lbl_Taxable_Value.Text = ""
        txt_CGST_Percentage.Text = "" '"2.5"
        txt_SGST_Percentage.Text = "" ' "2.5"
        txt_IGST_Percentage.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        txt_After_Addless_tax.Text = ""
        txt_othercharges.Text = ""
        lbl_billamount.Text = ""
        txt_tds.Text = ""
        lbl_tds.Text = ""
        lbl_netamount.Text = ""
        txt_Remarks.Text = ""




        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()

        Grid_DeSelect()

        cbo_Count.Visible = False
        cbo_MillName.Visible = False
        cbo_Grid_RateFor.Visible = False
        cbo_Colour.Visible = False

        cbo_Count.Tag = -1
        cbo_MillName.Tag = -1
        cbo_Grid_RateFor.Tag = -1
        cbo_Colour.Tag = -1

        cbo_Count.Text = ""
        cbo_MillName.Text = ""
        cbo_Grid_RateFor.Text = ""
        cbo_Colour.Text = ""

        NoCalc_Status = False

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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


        If Me.ActiveControl.Name <> cbo_MillName.Name Then
            cbo_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Count.Name Then
            cbo_Count.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_RateFor.Name Then
            cbo_Grid_RateFor.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
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

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        'Try

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.Ledger_Name as VatAC_Name from YarnProcessing_BillMaking_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.VatAc_IdNo = c.Ledger_IdNo   Where a.YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'", con)
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then
            lbl_RefNo.Text = dt1.Rows(0).Item("YarnProcessing_BillMaking_No").ToString
            dtp_Date.Text = dt1.Rows(0).Item("YarnProcessing_BillMaking_Date").ToString
            cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString





            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, C.Mill_Name, d.Colour_Name from YarnProcessing_BillMaking_Details a INNER JOIN Count_Head b ON  b.Count_Idno = a.Count_Idno LEFT OUTER JOIN Mill_Head C ON c.Mill_Idno = a.Mill_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo  where a.YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_Details.Rows.Clear()
            SNo = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Details.Rows.Add()

                    SNo = SNo + 1
                    dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                    dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Dc_Rc_No").ToString
                    dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString
                    dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Colour_Name").ToString
                    dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("BillMaking_Bag").ToString)
                    dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("BillMaking_Cone").ToString)
                    dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("BillMaking_Weight").ToString), "########0.000")
                    dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Rate_For").ToString
                    dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("YarnProcessing_Receipt_Code").ToString
                    dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("YarnProcessing_Receipt_SlNo").ToString


                Next i

            End If

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Bag").ToString)
                .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Cone").ToString)
                .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
            End With

            '--------------

            lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

            txt_billNo.Text = dt1.Rows(0).Item("Bill_No").ToString

            cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
            cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

            cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
            ' If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"

            txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
            txt_discAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
            txt_before_addless_tax.Text = Format(Val(dt1.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), "#########0.00")
            txt_othercharges.Text = dt1.Rows(0).Item("Other_Charges").ToString
            If IsDBNull(dt1.Rows(0).Item("Freight_Name").ToString) = False Then
                If Trim(dt1.Rows(0).Item("Freight_Name").ToString) <> "" Then
                    txt_Freight_Name.Text = dt1.Rows(0).Item("Freight_Name").ToString
                End If
            End If
            txt_Freight_Charge.Text = Format(Val(dt1.Rows(0).Item("Freight_charge").ToString), "#########0.00")
            lbl_Taxable_Value.Text = Format(Val(dt1.Rows(0).Item("Assesable_Amount").ToString), "#########0.00")


            txt_CGST_Percentage.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
            lbl_CGST_Amount.Text = dt1.Rows(0).Item("CGST_Amount").ToString
            txt_SGST_Percentage.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
            lbl_SGST_Amount.Text = dt1.Rows(0).Item("SGST_Amount").ToString
            txt_IGST_Percentage.Text = dt1.Rows(0).Item("IGST_Percentage").ToString
            lbl_IGST_Amount.Text = dt1.Rows(0).Item("IGST_Amount").ToString


            txt_After_Addless_tax.Text = dt1.Rows(0).Item("AddLess_AfterTax_Amount").ToString
            lbl_billamount.Text = Format(Val(dt1.Rows(0).Item("Bill_Amount").ToString), "#########0.00")
            txt_tds.Text = dt1.Rows(0).Item("Tds_Perc").ToString
            lbl_tds.Text = dt1.Rows(0).Item("Tds_Amount").ToString
            txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
            'lbl_netamount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")

            lbl_netamount.Text = Common_Procedures.Currency_Format(Val(CSng(dt1.Rows(0).Item("Net_Amount").ToString)))

            '-------------

            Grid_DeSelect()

            dt2.Clear()

            dt2.Dispose()
            da2.Dispose()

        End If

        dt1.Clear()
        dt1.Dispose()
        da1.Dispose()
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()



        NoCalc_Status = False

    End Sub

    Private Sub YarnProcessing_BillMaking_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_vataccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_vataccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub YarnProcessing_BillMaking_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable

        Me.Text = ""

        con.Open()


        cbo_MillName.Visible = False
        cbo_MillName.Visible = False


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        cbo_Grid_RateFor.Items.Clear()
        cbo_Grid_RateFor.Items.Add("")
        cbo_Grid_RateFor.Items.Add("BAG")
        cbo_Grid_RateFor.Items.Add("KG")

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Transport.DataSource = dt1
        cbo_Transport.DisplayMember = "Ledger_DisplayName"
        cbo_Transport.SelectedIndex = -1



        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("")
        cbo_TaxType.Items.Add("GST-5")
        cbo_TaxType.Items.Add("NO TAX")


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vataccount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_before_addless_tax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filter_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_othercharges.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_tds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_vat.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vataccount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_before_addless_tax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_filter_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_othercharges.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_tds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_vat.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_before_addless_tax.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Note.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_othercharges.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_vat.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filter_billNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_before_addless_tax.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Note.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_othercharges.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_billNo.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_vat.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filter_billNo.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress



        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_before_addless_tax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_othercharges.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Charge.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGST_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGST_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IGST_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_After_Addless_tax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_tds.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_tds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_before_addless_tax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_othercharges.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_Charge.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGST_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGST_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IGST_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_After_Addless_tax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_tds.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_tds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub YarnProcessing_BillMaking_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub YarnProcessing_BillMaking_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                Else
                    Close_Form()

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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 4 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_billNo.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                '    save_record()
                                'Else
                                '    dtp_Date.Focus()
                                'End If
                                txt_DiscPerc.Focus()

                            Else

                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If
                        End If
                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_billNo.Focus()

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
        Dim Qa As Windows.Forms.DialogResult

        '   If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Processing_Bill_Making, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Processing_Bill_Making, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "Update YarnProcessing_Receipt_Details set YarnProcessing_BillMaking_Code = '', YarnProcessing_BillMaking_Increment = YarnProcessing_BillMaking_Increment - 1 Where YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from YarnProcessing_BillMaking_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from YarnProcessing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'"
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

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then



            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Count.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Count.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_BillMaking_No from YarnProcessing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, YarnProcessing_BillMaking_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_BillMaking_No from YarnProcessing_BillMaking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, YarnProcessing_BillMaking_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_BillMaking_No from YarnProcessing_BillMaking_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, YarnProcessing_BillMaking_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_BillMaking_No from YarnProcessing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, YarnProcessing_BillMaking_No desc", con)
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
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "YarnProcessing_BillMaking_Head", "YarnProcessing_BillMaking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select YarnProcessing_BillMaking_No from YarnProcessing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Processing_Bill_Making, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Processing_Bill_Making, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select YarnProcessing_BillMaking_No from YarnProcessing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Col_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotCns As Single, vTotBags As Single
        Dim Cnt_ID As Integer = 0
        Dim vTotWeight As Single, vTotAmt As Single

        Dim Tr_ID As Integer = 0
        Dim itgry_id As Integer = 0, vatac_id As Integer = 0
        Dim PcsChkCode As String = ""
        Dim Trans_ID As Integer = 0



        'Dim Proc_ID As Integer = 0
        'Dim Lot_ID As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Processing_Bill_Making, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Trim(.Rows(i).Cells(2).Value) <> "" Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub
                    End If


                    If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)

                        End If
                        Exit Sub
                    End If

                    If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                        MessageBox.Show("Invalid Weight..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                        Exit Sub
                    End If


                End If

            Next
        End With

        If Trim(txt_billNo.Text) = "" Then
            MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_billNo.Enabled Then txt_billNo.Focus()
            Exit Sub
        End If

        'vatac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_vataccount.Text)
        'If vatac_id = 0 Then
        '    MessageBox.Show("Invalid Vat A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_vataccount.Enabled Then cbo_vataccount.Focus()
        '    Exit Sub
        'End If

        NoCalc_Status = False
        Total_Calculation()
        ' NetAmount_Calculation()

        vTotBags = 0 : vTotWeight = 0 : vTotCns = 0 : vTotAmt = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTotBags = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotCns = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
        End If


        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)


        tr = con.BeginTransaction


        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "YarnProcessing_BillMaking_Head", "YarnProcessing_BillMaking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@BillMakingDate", dtp_Date.Value.Date)

            If New_Entry = True Then



                cmd.CommandText = "Insert into YarnProcessing_BillMaking_Head(  YarnProcessing_BillMaking_Code,              Company_IdNo           ,      YarnProcessing_BillMaking_No     ,                                 for_OrderBy                               ,        YarnProcessing_BillMaking_Date,         Ledger_IdNo        ,               Bill_No          ,     VatAc_IdNo ,        Vat_Amount     ,   Vat_Percentage  ,          Total_Bag        ,          Total_Cone          ,           Total_Weight         ,           Total_Amount      ,              User_IdNo           ,        Transport_IdNo         ,               Vehicle_No           ,               Tax_Type               ,       Discount_Percentage       ,      Discount_Amount           ,               AddLess_BeforeTax_Amount  ,          Other_Charges           ,             Freight_charge            ,             Assesable_Amount             ,          Freight_Name               ,           CGST_Percentage            ,            CGST_Amount             ,              SGST_Percentage           ,            SGST_Amount              ,            IGST_Percentage           ,             IGST_Amount              ,              Bill_Amount          ,         AddLess_AfterTax_Amount           ,          Tds_Perc            ,          Tds_Amount       ,            Remarks               ,           Net_Amount           ) " &
                                                                     " Values ('" & Trim(NewCode) & "',              " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'      ,   " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",                     @BillMakingDate    ,    " & Str(Val(Led_ID)) & ",'" & Trim(txt_billNo.Text) & "',       0       ,            0          ,           0        ,  " & Str(Val(vTotBags)) & ", " & Str(Val(vTotCns)) & ",      " & Val(vTotWeight) & "      ,      " & Val(vTotAmt) & "  ,    " & Val(lbl_UserName.Text) & "  ,  " & Str(Val(Trans_ID)) & "  , '" & Trim(cbo_VehicleNo.Text) & "'  ,'" & Trim(cbo_TaxType.Text) & "'     ," & Val(txt_DiscPerc.Text) & "  , " & Val(txt_discAmount.Text) & ", " & Val(txt_before_addless_tax.Text) & ", " & Val(txt_othercharges.Text) & ",  " & Val(txt_Freight_Charge.Text) & ",  " & Val(lbl_Taxable_Value.Text) & "     , ' " & Trim(txt_Freight_Name.Text) & " ', " & Val(txt_CGST_Percentage.Text) & " , " & Val(lbl_CGST_Amount.Text) & "  ,  " & Val(txt_SGST_Percentage.Text) & ", " & Val(lbl_SGST_Amount.Text) & "  , " & Val(txt_IGST_Percentage.Text) & "  ,  " & Val(lbl_IGST_Amount.Text) & "  ,   " & Val(lbl_billamount.Text) & " ," & Val(txt_After_Addless_tax.Text) & "   , " & Val(txt_tds.Text) & "    ," & Val(lbl_tds.Text) & " ,'" & Trim(txt_Remarks.Text) & "'  ," & Str(Val(CDbl(lbl_netamount.Text))) & "  )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update YarnProcessing_BillMaking_Head set YarnProcessing_BillMaking_Date = @BillMakingDate, Ledger_IdNo = " & Val(Led_ID) & ", Bill_No = '" & Trim(txt_billNo.Text) & "' ,VatAc_IdNo = 0 ,Vat_Amount = 0, Vat_Percentage = 0 , Total_Bag = " & Val(vTotBags) & ",Total_Cone = " & Val(vTotCns) & " , Total_Weight = " & Val(vTotWeight) & " ,Total_Amount = " & Val(vTotAmt) & ", User_idNo =  " & Val(lbl_UserName.Text) & ",  Transport_IdNo=" & Str(Val(Trans_ID)) & ",  Vehicle_No='" & Trim(cbo_VehicleNo.Text) & "', Tax_Type = '" & Trim(cbo_TaxType.Text) & "',Discount_Percentage  =" & Val(txt_DiscPerc.Text) & "     ,      Discount_Amount=  " & Val(txt_discAmount.Text) & "    ,AddLess_BeforeTax_Amount =  " & Val(txt_before_addless_tax.Text) & ",Other_Charges= " & Val(txt_othercharges.Text) & ",Freight_Name='" & Trim(txt_Freight_Name.Text) & "',Freight_charge  = " & Val(txt_Freight_Charge.Text) & " ,Assesable_Amount = " & Val(lbl_Taxable_Value.Text) & "  ,   CGST_Percentage =" & Val(txt_CGST_Percentage.Text) & " ,CGST_Amount=" & Val(lbl_CGST_Amount.Text) & "  , SGST_Percentage =" & Val(txt_SGST_Percentage.Text) & ",SGST_Amount =" & Val(lbl_SGST_Amount.Text) & "  ,IGST_Percentage=" & Val(txt_IGST_Percentage.Text) & " ,   IGST_Amount=" & Val(lbl_IGST_Amount.Text) & "   , AddLess_AfterTax_Amount =  " & Val(txt_After_Addless_tax.Text) & "  , Bill_Amount = " & Val(lbl_billamount.Text) & " , Tds_Perc = " & Val(txt_tds.Text) & "  , Tds_Amount = " & Val(lbl_tds.Text) & " , Remarks= '" & Trim(txt_Remarks.Text) & "'  ,   Net_Amount = " & Str(Val(CDbl(lbl_netamount.Text))) & "   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update YarnProcessing_Receipt_Details set YarnProcessing_BillMaking_Code = '', YarnProcessing_BillMaking_Increment = YarnProcessing_BillMaking_Increment - 1 Where YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from YarnProcessing_BillMaking_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Bill : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(7).Value) <> 0 Then
                        Sno = Sno + 1
                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into YarnProcessing_BillMaking_Details( YarnProcessing_BillMaking_Code, Company_IdNo, yARNProcessing_BillMaking_No, for_OrderBy, YarnProcessing_BillMaking_Date, Sl_No , Dc_Rc_No , Count_Idno, Mill_Idno, Colour_IdNo ,  BillMaking_Bag, BillMaking_Cone , BillMaking_Weight, Rate_For ,Rate  ,   Amount  , YarnProcessing_Receipt_Code , YarnProcessing_Receipt_SlNo  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @BillMakingDate," & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mill_ID)) & "," & Val(Col_ID) & " ," & Val(.Rows(i).Cells(5).Value) & ", " & Val(.Rows(i).Cells(6).Value) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ",  '" & Trim(.Rows(i).Cells(8).Value) & "', " & Str(Val(.Rows(i).Cells(9).Value)) & " ," & Str(Val(.Rows(i).Cells(10).Value)) & " ,'" & Trim(.Rows(i).Cells(11).Value) & "', " & Str(Val(.Rows(i).Cells(12).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        'If Val(vTotWeight) > 0 Then
                        '    cmd.CommandText = "Insert into Stock_Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date,Ledger_IdNo, Party_Bill_No, Sl_No, Particulars, Item_IdNo,Colour_IdNo,Rack_IdNo,Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @BillMakingDate, " & Str(Val(Led_ID)) & ", '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ",'" & Trim(Partcls) & "' ," & Val(itgry_id) & "," & Val(Col_ID) & ", 0, " & Val(.Rows(i).Cells(4).Value) & " )"
                        '    cmd.ExecuteNonQuery()
                        'End If

                        cmd.CommandText = "Update YarnProcessing_Receipt_Details set YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "', YarnProcessing_BillMaking_Increment = YarnProcessing_BillMaking_Increment + 1 Where YarnProcessing_Receipt_Code = '" & Trim(.Rows(i).Cells(11).Value) & "' and YarnProcessing_Receipt_SlNo = " & Str(Val(.Rows(i).Cells(12).Value))
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With


            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            vLed_IdNos = Led_ID & "|" & Common_Procedures.CommonLedger.Processing_Charges_Ac & "|" & vatac_id
            vVou_Amts = Val(lbl_billamount.Text) & "|" & -1 * (Val(lbl_billamount.Text) - Val(lbl_vat.Text)) & "|" & -1 * Val(lbl_vat.Text)
            If Common_Procedures.Voucher_Updation(con, "YarnProc.Bill", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(txt_billNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            If Val(lbl_tds.Text) <> 0 Then
                vLed_IdNos = Led_ID & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                vVou_Amts = -1 * Val(lbl_tds.Text) & "|" & Val(lbl_tds.Text)
                If Common_Procedures.Voucher_Updation(con, "YarnProc.Tds", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No. : " & Trim(txt_billNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                    Throw New ApplicationException(ErrMsg)
                    Exit Sub
                End If
            End If

            'Bill Posting
            Dim VouBil As String = ""
            '  VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Value.Date, Led_ID, Trim(txt_billNo.Text), 0, Val(CSng(lbl_netamount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Value.Date, Led_ID, Trim(txt_billNo.Text), 0, Val(CDbl(lbl_netamount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()



    End Sub

    Private Sub Total_Calculation()
        Dim vTotBags As Single, vTotCns As Single, vtotweight As Single, vTotAmt As Single

        Dim i As Integer
        Dim sno As Integer


        vTotBags = 0 : vTotCns = 0 : vtotweight = 0 : sno = 0 : vTotAmt = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then


                    vTotBags = vTotBags + Val(dgv_Details.Rows(i).Cells(5).Value)
                    vTotCns = vTotCns + Val(dgv_Details.Rows(i).Cells(6).Value)
                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(7).Value)
                    vTotAmt = vTotAmt + Val(dgv_Details.Rows(i).Cells(10).Value)
                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(5).Value = Val(vTotBags)
        dgv_Details_Total.Rows(0).Cells(6).Value = Val(vTotCns)
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vtotweight), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(10).Value = Format(Val(vTotAmt), "#########0.000")


        NetAmount_Calculation()

    End Sub
    Private Sub Amount_Calculation()
        Dim vtotamt As Single

        Dim i As Integer
        Dim sno As Integer


        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub


        sno = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then
                    If Trim(dgv_Details.Rows(i).Cells(8).Value) = "BAG" Then

                        vtotamt = Val(dgv_Details.Rows(i).Cells(5).Value) * Val(dgv_Details.Rows(i).Cells(9).Value)
                    ElseIf Trim(dgv_Details.Rows(i).Cells(8).Value) = "KG" Then
                        vtotamt = Val(dgv_Details.Rows(i).Cells(7).Value) * Val(dgv_Details.Rows(i).Cells(9).Value)


                    End If
                    dgv_Details.Rows(i).Cells(10).Value = Format(Val(vtotamt), "#########0.00")
                End If
            Next
        End With
        Total_Calculation()

    End Sub


    Private Sub NetAmount_Calculation()


        Dim vTaxableAmt As String
        Dim vTotAmt As Single
        Dim vCashDisc As String
        Dim vCashDiscAmt As String
        Dim vBfAddlessAmt As String
        Dim vOthrChrge As String
        Dim vFrigtchrge As String
        Dim vbillAmt As String
        Dim vAfAddlessAmt As String

        vTaxableAmt = 0 : vTotAmt = 0 : vCashDisc = 0 : vCashDiscAmt = 0

        vBfAddlessAmt = 0 : vOthrChrge = 0 : vFrigtchrge = 0 : vbillAmt = 0 : vAfAddlessAmt = 0


        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub

        With dgv_Details_Total
            If .Rows.Count > 0 Then
                vTotAmt = Format(Val(.Rows(0).Cells(10).Value), "###########0.00")

            End If
        End With

        If Val(txt_DiscPerc.Text) <> 0 Then
            txt_discAmount.Text = Format((Val(vTotAmt) * Val(txt_DiscPerc.Text) / 100), "######0.00")
        End If


        lbl_Taxable_Value.Text = Format((Val(vTotAmt) - Val(txt_discAmount.Text) + Val(txt_before_addless_tax.Text) + Val(txt_othercharges.Text) + Val(txt_Freight_Charge.Text)), "#######0.00")




        Dim Led_IdNo As Integer = 0
        Dim InterStateStatus As Boolean = False


        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), Led_IdNo)

        If Val(Led_IdNo) <> 0 And Trim(UCase(cbo_TaxType.Text)) = "GST-5" Then

            If InterStateStatus = False Then

                txt_CGST_Percentage.Text = "2.5"
                txt_SGST_Percentage.Text = "2.5"
                txt_IGST_Percentage.Text = ""

            Else

                txt_CGST_Percentage.Text = ""
                txt_SGST_Percentage.Text = ""

                txt_IGST_Percentage.Text = "5"


            End If

        Else
            txt_CGST_Percentage.Text = ""
            txt_SGST_Percentage.Text = ""

            txt_IGST_Percentage.Text = ""

        End If


        lbl_CGST_Amount.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_CGST_Percentage.Text) / 100, "##########0.00")
        lbl_SGST_Amount.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_SGST_Percentage.Text) / 100, "###########0.00")
        lbl_IGST_Amount.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_IGST_Percentage.Text) / 100, "##########0.00")



        lbl_billamount.Text = Format((Val(lbl_Taxable_Value.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text) + Val(txt_After_Addless_tax.Text)), "#########0.00")




        If Val(txt_tds.Text) <> 0 Then

            lbl_tds.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_tds.Text) / 100, "########0.00")

        Else
            lbl_tds.Text = ""

        End If

        lbl_netamount.Text = Format(Val(lbl_billamount.Text) - Val(lbl_tds.Text), "#########0.00")

        lbl_netamount.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_netamount.Text)))


        '-------------


    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            'If dgv_Details.Rows.Count > 0 Then
            '    dgv_Details.Focus()
            '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(8)

            'Else
            '    txt_billNo.Focus()

            'End If

            txt_billNo.Focus()
        End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select order:", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

                'ElseIf dgv_Details.Rows.Count > 0 Then
                '    dgv_Details.Focus()
                '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(8)

            Else
                txt_billNo.Focus()

            End If

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




    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If



            If e.ColumnIndex = 2 Then

                If cbo_Count.Visible = False Or Val(cbo_Count.Tag) <> e.RowIndex Then

                    cbo_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Count.DataSource = Dt1
                    cbo_Count.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Count.Left = .Left + rect.Left
                    cbo_Count.Top = .Top + rect.Top

                    cbo_Count.Width = rect.Width
                    cbo_Count.Height = rect.Height
                    cbo_Count.Text = .CurrentCell.Value

                    cbo_Count.Tag = Val(e.RowIndex)
                    cbo_Count.Visible = True

                    cbo_Count.BringToFront()
                    cbo_Count.Focus()



                End If


            Else

                cbo_Count.Visible = False

            End If

            If e.ColumnIndex = 3 Then

                If cbo_MillName.Visible = False Or Val(cbo_MillName.Tag) <> e.RowIndex Then

                    cbo_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head  order by Mill_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_MillName.DataSource = Dt2
                    cbo_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_MillName.Left = .Left + rect.Left
                    cbo_MillName.Top = .Top + rect.Top
                    cbo_MillName.Width = rect.Width
                    cbo_MillName.Height = rect.Height

                    cbo_MillName.Text = .CurrentCell.Value

                    cbo_MillName.Tag = Val(e.RowIndex)
                    cbo_MillName.Visible = True

                    cbo_MillName.BringToFront()
                    cbo_MillName.Focus()


                End If

            Else

                cbo_MillName.Visible = False


            End If
            If e.ColumnIndex = 4 Then
                If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                    cbo_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Colour.DataSource = Dt2
                    cbo_Colour.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Colour.Left = .Left + rect.Left
                    cbo_Colour.Top = .Top + rect.Top
                    cbo_Colour.Width = rect.Width
                    cbo_Colour.Height = rect.Height

                    cbo_Colour.Text = .CurrentCell.Value

                    cbo_Colour.Tag = Val(e.RowIndex)
                    cbo_Colour.Visible = True

                    cbo_Colour.BringToFront()
                    cbo_Colour.Focus()



                End If


            Else


                cbo_Colour.Visible = False


            End If


            If e.ColumnIndex = 8 Then

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



            If .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim i As Integer
        Dim vTotMtrs As Single
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 10 Then
                    Total_Calculation()
                End If
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 9 Then
                    Amount_Calculation()

                End If
                If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then
                    get_MillCount_Details()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress


        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub
    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

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

            Total_Calculation()
        End If

    End Sub

    Private Sub cbo_vataccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_vataccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_idno = 0 or AccountsGroup_IdNo = 12) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_vataccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vataccount, txt_before_addless_tax, txt_vat, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_idno = 0 or AccountsGroup_IdNo = 12) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_vataccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vataccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vataccount, txt_vat, "Ledger_AlaisHead", "Ledger_DisplayName", "  ( Ledger_idno = 0 or AccountsGroup_IdNo = 12)  ", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus, cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, Nothing, cbo_MillName, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, cbo_MillName, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.TextChanged
        Try
            If cbo_Count.Visible Then
                With dgv_Details
                    If Val(cbo_Count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Count.Text)
                    End If
                End With
            End If

        Catch ex As Exception


        End Try
    End Sub
    Private Sub cbo_itemfp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")

    End Sub


    Private Sub cbo_itemfp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName, cbo_Count, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_itemfp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_itemfp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_itemfp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.TextChanged
        Try
            If cbo_MillName.Visible Then
                With dgv_Details
                    If Val(cbo_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_MillName, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub


    Private Sub cbo_colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Colour.Name
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
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_billno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_billNo.KeyDown
        If e.KeyValue = 38 Then
            cbo_Ledger.Focus()
        ElseIf e.KeyValue = 40 Then
            cbo_Transport.Focus()
        End If
    End Sub

    Private Sub txt_billNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_billNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Transport.Focus()
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgv_Details_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.GotFocus
        dgv_Details.Focus()
        ' dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.YarnProcessing_BillMaking_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.YarnProcessing_BillMaking_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.YarnProcessing_BillMaking_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_Count.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_Count.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If
            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Count_IdNo = " & Str(Val(Cnt_IdNo))
            End If



            If Trim(txt_filter_billNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Bill_No = '" & Trim(txt_filter_billNo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,c.* , d.Count_Name , E.Mill_Name from YarnProcessing_BillMaking_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN YarnProcessing_BillMaking_Details c ON c.YarnProcessing_BillMaking_Code = a.YarnProcessing_BillMaking_Code INNER JOIN Count_Head d ON d.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head e ON c.Mill_Idno = e.Mill_idNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_BillMaking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.YarnProcessing_BillMaking_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("YarnProcessing_BillMaking_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("YarnProcessing_BillMaking_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Mill_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("BillMaking_Bag").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("BillMaking_Cone").ToString)

                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("BillMaking_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, txt_filter_billNo, cbo_Filter_Count, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Count, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ItemGrey_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ItemGrey_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Count.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Count, cbo_Filter_PartyName, btn_Filter_Show, "Count_Head", "Count_Name", "", "(Count_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ItemGrey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Count, btn_Filter_Show, "Count_Head", "Count_Name", "", "(Count_idno = 0)")

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

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub



    Private Sub txt_vat_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_vat.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_vat_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_vat.TextChanged
        NetAmount_Calculation()
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

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection


            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, C.Mill_Name,E.Colour_Name  from YarnProcessing_Receipt_Details a LEFT OUTER JOIN Count_Head b ON  b.Count_IdNo = a.Count_Idno LEFT OUTER JOIN Mill_Head C ON c.Mill_IdNo = a.Mill_Idno LEFT OUTER JOIN Colour_Head E ON E.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN YarnProcessing_BillMaking_Details d ON d.YarnProcessing_Receipt_Code = a.YarnProcessing_Receipt_Code and d.YarnProcessing_Receipt_SlNo = a.YarnProcessing_Receipt_SlNo where a.YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.YarnProcessing_Receipt_Date, a.for_orderby, a.YarnProcessing_Receipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)


            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    Ent_Rate = 0


                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("YarnProcessing_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("YarnProcessing_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Colour_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Receipt_Bag").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Receipt_cone").ToString)
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Weight").ToString) + Val(Dt1.Rows(i).Item("Excess_Short_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(9).Value = "1"
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("YarnProcessing_Receipt_Code").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("YarnProcessing_Receipt_SlNo").ToString
                    '.Rows(n).Cells(11).Value = Ent_Rate

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, C.Mill_Name,  E.Colour_Name from YarnProcessing_Receipt_Details a LEFT OUTER JOIN Count_Head b ON  b.Count_Idno = a.Count_Idno LEFT OUTER JOIN Mill_Head C ON c.Mill_Idno = a.Mill_Idno  LEFT OUTER JOIN YarnProcessing_BillMaking_Details d ON d.YarnProcessing_BillMaking_Code = a.YarnProcessing_Receipt_Code  LEFT OUTER JOIN Colour_Head E ON E.Colour_IdNo = a.Colour_IdNo  where a.YarnProcessing_BillMaking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.YarnProcessing_Receipt_Date, a.for_orderby, a.YarnProcessing_Receipt_No", con)
            Dt1 = New DataTable
            NR = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("YarnProcessing_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("YarnProcessing_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Colour_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Receipt_Bag").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Receipt_Cone").ToString)
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Weight").ToString) + Val(Dt1.Rows(i).Item("Excess_Short_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(9).Value = ""
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("YarnProcessing_Receipt_Code").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("YarnProcessing_Receipt_SlNo").ToString
                  
                Next

            End If
            Dt1.Clear()

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

                .Rows(RwIndx).Cells(9).Value = (Val(.Rows(RwIndx).Cells(9).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(9).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(9).Value = ""

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

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(9).Value) = 1 Then

                ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                

                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(11).Value

                Dt1.Clear()

                Total_Calculation()


                '  Exit For

            End If

        Next

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                dgv_Details.CurrentCell.Selected = True

            Else
                txt_billNo.Focus()

            End If
        End If

    End Sub



    Private Sub cbo_vataccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_vataccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
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
                .Rows(.CurrentCell.RowIndex).Cells.Item(8).Value = Trim(cbo_Grid_RateFor.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub
    Private Sub cbo_Grid_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.TextChanged
        Try
            If cbo_Grid_RateFor.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                With dgv_Details
                    If Val(cbo_Grid_RateFor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 8 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RateFor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3).Value)

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

                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = .Rows(.CurrentRow.Index).Cells(5).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(7).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(5).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 6 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(7).Value = Format(.Rows(.CurrentRow.Index).Cells(6).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub
    Private Sub txt_before_addless_tax_TextChanged(sender As Object, e As EventArgs) Handles txt_before_addless_tax.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub txt_Freight_Charge_TextChanged(sender As Object, e As EventArgs) Handles txt_Freight_Charge.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_billNo, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

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
    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_VehicleNo, Nothing, "", "", "", "")

        If (e.KeyValue = 40 And cbo_TaxType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

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
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_DiscPerc.Focus()


            End If
        End If
    End Sub

    Private Sub cbo_TaxType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.LostFocus
        If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
            cbo_TaxType.Tag = cbo_TaxType.Text
            NetAmount_Calculation()
        End If
    End Sub
    Private Sub cbo_TaxType_TextChanged(sender As Object, e As EventArgs) Handles cbo_TaxType.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnProcessing_BillMaking_Head", "Vehicle_No", "", "")

    End Sub
    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, cbo_TaxType, "YarnProcessing_BillMaking_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, cbo_TaxType, "YarnProcessing_BillMaking_Head", "Vehicle_No", "", "", False)
    End Sub

    Private Sub txt_DiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPerc.KeyDown
        If e.KeyValue = 38 Then

            dgv_Details.Focus()

        End If

        If e.KeyValue = 40 Then
            txt_before_addless_tax.Focus()
        End If

    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            txt_before_addless_tax.Focus()
        End If
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged


        If Val(txt_DiscPerc.Text) = 0 Then

            txt_discAmount.ReadOnly = False
        Else
            txt_discAmount.ReadOnly = True
        End If

        NetAmount_Calculation()


    End Sub
    Private Sub txt_discAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_discAmount.KeyPress
        If Asc(e.KeyChar) = 13 Then

            txt_before_addless_tax.Focus()

        End If
    End Sub

    Private Sub txt_discAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_discAmount.KeyDown
        If e.KeyValue = 38 Then

            dgv_Details.Focus()

        ElseIf e.KeyValue = 40 Then

            txt_before_addless_tax.Focus()

        End If
    End Sub

    Private Sub txt_Before_AddLess_Tax_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_before_addless_tax.KeyDown
        If e.KeyValue = 38 Then
            txt_DiscPerc.Focus()
        End If
        If e.KeyValue = 40 Then
            txt_othercharges.Focus()
        End If

    End Sub

    Private Sub txt_Before_AddLess_Tax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_before_addless_tax.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
            '    cbo_VatAc.Focus()
            'Else
            '    txt_AssessableValue.Focus()
            'End If
            txt_othercharges.Focus()
        End If

    End Sub

    Private Sub txt_Before_AddLess_Tax_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_before_addless_tax.LostFocus
        If Val(txt_before_addless_tax.Text) <> 0 Then
            txt_before_addless_tax.Text = Format(Val(txt_before_addless_tax.Text), "#########0.00")
        Else
            txt_before_addless_tax.Text = ""
        End If
    End Sub
    Private Sub txt_othercharges_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_othercharges.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Freight_Charge.Focus()
        End If

    End Sub
    Private Sub txt_othercharges_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_othercharges.KeyDown

        If e.KeyValue = 38 Then
            txt_before_addless_tax.Focus()

        ElseIf e.KeyValue = 40 Then

            txt_Freight_Charge.Focus()

        End If

    End Sub

    Private Sub txt_othercharges_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_othercharges.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight_Charge.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then

            txt_After_Addless_tax.Focus()
        End If
    End Sub
    Private Sub txt_Freight_Charge_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Freight_Charge.KeyDown
        If e.KeyValue = 38 Then

            txt_othercharges.Focus()

        ElseIf e.KeyValue = 40 Then
            txt_After_Addless_tax.Focus()

        End If
    End Sub

    Private Sub txt_Freight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight_Charge.LostFocus
        If Val(txt_Freight_Charge.Text) <> 0 Then
            txt_Freight_Charge.Text = Format(Val(txt_Freight_Charge.Text), "#########0.00")
        Else
            txt_Freight_Charge.Text = ""
        End If

    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight_Charge.TextChanged
        'Total_Calculation()
        NetAmount_Calculation()
    End Sub
    Private Sub txt_After_Addless_tax_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_After_Addless_tax.KeyDown
        If e.KeyValue = 38 Then
            txt_Freight_Charge.Focus()
        ElseIf e.KeyValue = 40 Then
            txt_tds.Focus()


        End If
    End Sub

    Private Sub txt_After_Addless_tax_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_After_Addless_tax.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then

            txt_tds.Focus()
        End If
    End Sub
    Private Sub txt_After_Addless_tax_TextChanged(sender As Object, e As EventArgs) Handles txt_After_Addless_tax.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub txt_tds_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_tds.KeyDown
        If e.KeyCode = 38 Then

            txt_After_Addless_tax.Focus()

        ElseIf e.KeyCode = 40 Then
            txt_Remarks.Focus()

        End If


    End Sub


    Private Sub txt_tds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_tds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then

            txt_Remarks.Focus()
        End If
    End Sub
    Private Sub txt_tds_TextChanged(sender As Object, e As EventArgs) Handles txt_tds.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub txt_remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            txt_tds.Focus()

        End If
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else

                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else

                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Ledger_TextChanged(sender As Object, e As EventArgs) Handles cbo_Ledger.TextChanged
        NetAmount_Calculation()
    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String


        ' If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Cotton_Sales_GST, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Try

            da1 = New SqlClient.SqlDataAdapter("select * from YarnProcessing_BillMaking_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'", con)
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

        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "123")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")


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
                AddHandler ppd.Shown, AddressOf PrintPreview_Shown
                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub
    Private Sub btn_Print_Click(sender As Object, e As EventArgs) Handles btn_Print.Click
        print_record()
    End Sub
    Private Sub PrintPreview_Shown(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ts As ToolStrip = CType(sender.Controls(1), ToolStrip)

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        Dim I As Integer, K As Integer
        Dim ItmNm1 As String, ItmNm2 As String
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt_VAT.Clear()
        prn_DetDt_VAT.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetDt_VAT1.Clear()
        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 1 '0 '1
        DetSNo = 0
        prn_DetMxIndx = 0
        prn_Count = 0

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* ,f.Ledger_Name as transportname  ,SH.* ,Lsh.State_Name as Ledger_State_Name ,Lsh.State_Code as Ledger_State_Code 		  from YarnProcessing_BillMaking_Head a INNER JOIN Company_Head b        ON a.Company_IdNo        = b.Company_IdNo  INNER JOIN Ledger_Head c         ON a.Ledger_IdNo         = c.Ledger_IdNo    LEFT OUTER JOIN State_Head Lsh   ON c.Ledger_State_Idno   = Lsh.State_IDno    LEFT OUTER JOIN Ledger_Head f    ON a.Transport_IdNo = f.LEDGER_IDNO       LEFT OUTER JOIN State_HEad DSH   on f.Ledger_State_IdNo = DSH.State_IdNo    LEFT OUTER JOIN State_Head SH    ON b.Company_State_IdNo  = SH.State_Idno		where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'", con)
            ' da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.Ledger_Name as VatAC_Name from YarnProcessing_BillMaking_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.VatAc_IdNo = c.Ledger_IdNo   Where a.YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)


                If prn_HdDt.Rows.Count > 0 Then



                ' '  da2 = New SqlClient.SqlDataAdapter("select a.* , b.CLOTH_nAME as item_Grey,c.cloth_name as item_Fp    from Textile_Processing_BillMaking_DETAILS a   left outer join  CLOTH_HEAD b ON a.Item_Idno = b.CLOTH_IdNo    left outer join   CLOTH_HEAD c on a.Item_To_Idno = c.CLOTH_IdNo where a.Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Cloth_Processing_BillMaking_No", con)
                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Count_Name, C.Mill_Name, d.Colour_Name from YarnProcessing_BillMaking_Details a INNER JOIN Count_Head b ON  b.Count_Idno = a.Count_Idno LEFT OUTER JOIN Mill_Head C ON c.Mill_Idno = a.Mill_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo  where a.YarnProcessing_BillMaking_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)


                    If prn_DetDt.Rows.Count > 0 Then

                        prn_DetMxIndx = 0
                        For I = 0 To prn_DetDt.Rows.Count - 1


                            prn_DetMxIndx = prn_DetMxIndx + 1

                            prn_DetAr(prn_DetMxIndx, 1) = Trim(Val(I) + 1)

                        prn_DetAr(prn_DetMxIndx, 2) = prn_DetDt.Rows(I).Item("dc_rc_no").ToString
                        prn_DetAr(prn_DetMxIndx, 3) = prn_DetDt.Rows(I).Item("Count_Name").ToString
                        prn_DetAr(prn_DetMxIndx, 4) = prn_DetDt.Rows(I).Item("Mill_Name").ToString
                        prn_DetAr(prn_DetMxIndx, 5) = prn_DetDt.Rows(I).Item("Colour_Name").ToString
                        prn_DetAr(prn_DetMxIndx, 6) = Val(prn_DetDt.Rows(I).Item("BillMaking_Bag").ToString)
                        '  prn_DetAr(prn_DetMxIndx, 7) = Val(prn_DetDt.Rows(I).Item("BillMaking_Cone").ToString)
                        prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_DetDt.Rows(I).Item("BillMaking_Weight").ToString), "########0.000")
                        prn_DetAr(prn_DetMxIndx, 8) = prn_DetDt.Rows(I).Item("Rate_For").ToString
                        prn_DetAr(prn_DetMxIndx, 9) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Rate").ToString), "########0.00"))
                        prn_DetAr(prn_DetMxIndx, 10) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Amount").ToString), "########0.00"))



                    Next I

                    End If

                Else
                    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Finally
                da1.Dispose()
                da2.Dispose()

            End Try

    End Sub
    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format_GST_1061(e)

        '   Printing_GST_Format1(e)

    End Sub

    Private Sub Printing_Format_GST_1061(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ITEM_DETAILS As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ItmNm3 As String, ItmNm4 As String
        Dim K As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 65
            .Right = 50

            .Top = 40 ' 65

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
        TxtHgt = 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height
        NoofItems_PerPage = 15 ' 19  


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClArr(1) = 35 : ClArr(2) = 40 : ClArr(3) = 130 : ClArr(4) = 130 : ClArr(5) = 70 : ClArr(6) = 35 : ClArr(7) = 70 : ClArr(8) = 60 : ClArr(9) = 65
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))




        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("freight_charge").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("Other_Charges").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1


                Printing_Format_GST_1061_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    CurY = CurY - 10


                    NoofDets = 0

                    '    CurY = CurY - TxtHgt - 10

                    If prn_DetMxIndx > 0 Then

                        Do While DetIndx <= prn_DetMxIndx

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_Format_GST_1061_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                e.HasMorePages = True

                                Return

                            End If

                            ITEM_DETAILS = ""

                            CurY = CurY + TxtHgt



                            ItmNm1 = Trim(prn_DetAr(DetIndx, 3))
                            ItmNm2 = ""

                            If Len(ItmNm1) > 25 Then
                                For K = 25 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), K, 1) = " " Or Mid$(Trim(ItmNm1), K, 1) = "," Or Mid$(Trim(ItmNm1), K, 1) = "." Or Mid$(Trim(ItmNm1), K, 1) = "-" Or Mid$(Trim(ItmNm1), K, 1) = "/" Or Mid$(Trim(ItmNm1), K, 1) = "_" Or Mid$(Trim(ItmNm1), K, 1) = "(" Or Mid$(Trim(ItmNm1), K, 1) = ")" Or Mid$(Trim(ItmNm1), K, 1) = "\" Or Mid$(Trim(ItmNm1), K, 1) = "[" Or Mid$(Trim(ItmNm1), K, 1) = "]" Or Mid$(Trim(ItmNm1), K, 1) = "{" Or Mid$(Trim(ItmNm1), K, 1) = "}" Then Exit For
                                Next K
                                If K = 0 Then K = 25
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - K)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), K - 1)
                            End If


                            ItmNm3 = Trim(prn_DetAr(DetIndx, 4))
                            ItmNm4 = ""
                            If Len(ItmNm3) > 15 Then
                                For K = 15 To 1 Step -1
                                    If Mid$(Trim(ItmNm3), K, 1) = " " Or Mid$(Trim(ItmNm3), K, 1) = "," Or Mid$(Trim(ItmNm3), K, 1) = "." Or Mid$(Trim(ItmNm3), K, 1) = "-" Or Mid$(Trim(ItmNm3), K, 1) = "/" Or Mid$(Trim(ItmNm3), K, 1) = "_" Or Mid$(Trim(ItmNm3), K, 1) = "(" Or Mid$(Trim(ItmNm3), K, 1) = ")" Or Mid$(Trim(ItmNm3), K, 1) = "\" Or Mid$(Trim(ItmNm3), K, 1) = "[" Or Mid$(Trim(ItmNm3), K, 1) = "]" Or Mid$(Trim(ItmNm3), K, 1) = "{" Or Mid$(Trim(ItmNm3), K, 1) = "}" Then Exit For
                                Next K
                                If K = 0 Then K = 15
                                ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - K)
                                ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), K - 1)
                            End If



                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + 10, CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(DetIndx, 2)), LMargin + ClArr(1), CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2), CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 5), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 15, CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 15, CurY + 5, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 9), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 5, CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 10), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY + 5, 1, 0, pFont)
                            ' Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11), CurY + 5, 1, 0, pFont)



                            NoofDets = NoofDets + 1
                            If Trim(prn_DetAr(DetIndx, 3)) <> "" Or Trim(ItmNm2) <> "" Or Trim(prn_DetAr(DetIndx, 4)) <> "" Or Trim(ItmNm4) <> "" Then
                                CurY = CurY + TxtHgt
                                NoofDets = NoofDets + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2), CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY + 5, 0, 0, pFont)
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format_GST_1061_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 0 '1
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

    Private Sub Printing_Format_GST_1061_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String
        Dim PnAr() As String
        Dim LedNmAr(10) As String
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
        Dim Cmp_PanCap As String
        Dim Cmp_Panno As String




        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        'dt2 = New DataTable
        'da2.Fill(dt2)
        ''If dt2.Rows.Count > NoofItems_PerPage Then
        ''    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        ''End If
        'dt2.Clear()

        prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                    PrintDocument1.DefaultPageSettings.Color = True
                    PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
                    e.PageSettings.Color = True
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                End If

            End If
        End If

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        p1Font = New Font("Calibri", 12, FontStyle.Regular)

        Common_Procedures.Print_To_PrintDocument(e, "BILL PASS STATEMENT", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)




        'CurY = CurY + TxtHgt '+ 10
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_Panno = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****



        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        If Trim(Cmp_Desc) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 9, FontStyle.Bold Or FontStyle.Italic)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Desc), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

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
        If Trim(prn_HdDt.Rows(0).Item("Company_Type").ToString) <> "UNACCOUNT" Then
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        End If
        CurX = CurX + strWidth
        If Trim(prn_HdDt.Rows(0).Item("Company_Type").ToString) <> "UNACCOUNT" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try


            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""


            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
            Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

            Led_State = Trim(prn_HdDt.Rows(0).Item("State_Name").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = " PHONE NO : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Company_Type").ToString) <> "UNACCOUNT" Then
                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
            End If




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

            If Trim(Led_Add4) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add4
            End If

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


            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY + 5

            '------------------- Invoice No Block

            CurY = CurY + TxtHgt - 30



            p1Font = New Font("Calibri", 14, FontStyle.Bold)


            '  Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, BlockInvNoY - 20, 0, 0, pFont)


            ' Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 100, BlockInvNoY - 20, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("YarnProcessing_BillMaking_No").ToString, LMargin + W2 + 115, BlockInvNoY - 20, 0, 0, p1Font)

            'If Trim(prn_HdDt.Rows(0).Item("bill_no").ToString) <> "" Then
            'Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + Cen1 + 60, BlockInvNoY - 20, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, BlockInvNoY - 20, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("bill_no").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY - 20, 0, 0, pFont)
            '  End If



            'Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, BlockInvNoY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 100, BlockInvNoY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("YarnProcessing_BillMaking_Date").ToString), "dd-MM-yyyy"), LMargin + W2 + 115, BlockInvNoY, 0, 0, pFont)



            'If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Transport", LMargin + Cen1 + 60, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY, 0, 0, pFont)
            'End If

            'BlockInvNoY = BlockInvNoY + TxtHgt

            'If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 60, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY, 0, 0, pFont)
            'End If



            'CurY = CurY + TxtHgt + 20
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            ''   CurY=CurY+TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "BILLED TO : ", LMargin + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "SHIPPED TO : ", LMargin + Cen1 + 60, CurY + 5, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt
            'ItmNm1 = Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)


            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width



            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("YarnProcessing_BillMaking_No").ToString, LMargin + Cen1 + W1 + 50, CurY, 0, 0, p1Font)


            ' Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + Cen1 + W1, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("YarnProcessing_BillMaking_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 50, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '   Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)



            If Trim(prn_HdDt.Rows(0).Item("bill_no").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("bill_no").ToString, LMargin + Cen1 + W1 + 50, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)



            If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Transport", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + Cen1 + W1 + 50, CurY, 0, 0, pFont)
            End If



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)



            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 50, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(8)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(8)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))

            CurY = CurY + TxtHgt - 10


            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "REC", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " NO ", LMargin + ClAr(1), CurY + 15, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "FOR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + 15, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 2, ClAr(10), pFont)




            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_GST_1061_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 0, 0, pFont)
            If is_LastPage = True Then

                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("TOTAL_BAG").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("TOTAL_WEIGHT").ToString), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("TOTAL_AMOUNT").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, LnAr(3))





            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt - 10
                    'If Val(prn_PageNo) = 1 Then
                    p1Font = New Font("Calibri", 14, FontStyle.Bold Or FontStyle.Underline)
                    '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 12, FontStyle.Bold)
                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt + 5
                        '     Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt + 5 - 3
                        '       Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt + 5 - 3
                        '     Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt + 5 - 3
                        '  Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                End If

            End If


            CurY = CurY - 10

            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("freight_charge").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("freight_charge").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Before Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Before Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)

                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Other_Charges").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("Other_Charges").ToString) > 0 Then
                        '    Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        'Else
                        Common_Procedures.Print_To_PrintDocument(e, "Other Charges", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Other_Charges").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)

                End If
            End If

            '   vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("freight_charge").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Other_Charges").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assesable_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, p1Font)
                End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Format(Val(txt_CGST_Percentage.Text), "##########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Format(Val(txt_SGST_Percentage.Text), "##########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Format(Val(txt_IGST_Percentage.Text), "##########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt - 20
            If Val(prn_HdDt.Rows(0).Item("AddLess_AfterTax_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If Val(prn_HdDt.Rows(0).Item("AddLess_AfterTax_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "After Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "After Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)
                    End If

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_afterTax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("tds_AMount").ToString) <> 0 Then
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "TDS @ " & Format(Val(txt_tds.Text), "##########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, pFont)

                    Dim VTds_AMount As String

                    VTds_AMount = Format(Val(prn_HdDt.Rows(0).Item("tds_AMount").ToString), "########0")

                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(VTds_AMount)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If


            'If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
            '    End If
            'End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 30, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)

                Dim VAmount As String
                VAmount = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "#########0")


                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(VAmount)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, LnAr(5))

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

            '''=============GST SUMMARY============
            ''vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            ''If vNoofHsnCodes <> 0 Then
            ''    Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            ''End If
            ''==========================

            ''If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1318" Then  '--- Pranav Plastic
            ''    p1Font = New Font("Calibri", 9.5, FontStyle.Regular)
            '' CurY = CurY + TxtHgt - 10
            ''Common_Procedures.Print_To_PrintDocument(e, "We Declare that this Invoice Shows the actual price of the goods described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, p1Font)
            ''    CurY = CurY + TxtHgt
            ''End If
            'p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            ''  Common_Procedures.Print_To_PrintDocument(e, "Declaration: ", LMargin + 5, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt
            'p1Font = New Font("Calibri", 10, FontStyle.Regular)
            ''    Common_Procedures.Print_To_PrintDocument(e, "We Declare that this Invoice Shows the actual price of the goods described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(11) = CurY
            'CurY = CurY + TxtHgt - 8
            ''e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the particulars given above are true and correct", PageWidth - 15, CurY, 1, 0, p1Font)
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            '  Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions: ", LMargin + 5, CurY, 0, 0, p1Font)
            ' Common_Procedures.Print_To_PrintDocument(e, "", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt - 2

            'Common_Procedures.Print_To_PrintDocument(e, "above are true and correct", PageWidth - 15, CurY, 1, 0, p1Font)
            'CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, p1Font)



            p1Font = New Font("Calibri", 8, FontStyle.Regular)
            ' Common_Procedures.Print_To_PrintDocument(e, "Our Responsibility Cases after the goods have been delivered", LMargin + 5, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "to the carriers. No claims for breakage or shortage during transit", LMargin + 5, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            '  Common_Procedures.Print_To_PrintDocument(e, "enterinaed. Interest 21% will be changed on amount not paid within", LMargin + 5, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Jurs = StrConv(Common_Procedures.settings.Jurisdiction, vbProperCase)
            If Trim(Jurs) = "" Then Jurs = "Tirupur"
            '    Common_Procedures.Print_To_PrintDocument(e, "30days from the date of invoice.Subject to " & Jurs & " Jurisdiction.", LMargin + 5, CurY, 0, 0, p1Font)
            ' CurY = CurY + TxtHgt - 12

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)

            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            '  e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, LnAr(11))
            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            Jurs = StrConv(Common_Procedures.settings.Jurisdiction, vbProperCase)
            If Trim(Jurs) = "" Then Jurs = "Tirupur"

            'Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Jurs & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)

            'If Print_PDF_Status = True Then
            '    CurY = CurY + TxtHgt - 15
            '    p1Font = New Font("Calibri", 9, FontStyle.Regular)
            '    Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)
            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub txt_discAmount_TextChanged(sender As Object, e As EventArgs) Handles txt_discAmount.TextChanged
        NetAmount_Calculation()
    End Sub
End Class