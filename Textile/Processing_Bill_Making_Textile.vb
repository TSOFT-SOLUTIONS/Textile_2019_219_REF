Public Class Processing_Bill_Making_Textile
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPBM-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Count As Integer
    Public CHk_Details_Cnt As Integer = 0
    Private prn_InpOpts As String = ""
    Private NoCalc_Status As Boolean = False
    Private prn_DetAr(200, 12) As String
    Private prn_DetMxIndx As Integer
    Private DetIndx As Integer
    Private prn_HdDt_VAT As New DataTable
    Private prn_DetDt_VAT As New DataTable
    Private DetSNo As Integer
    Private prn_DetDt_VAT1 As New DataTable

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
        cbo_TaxType.Text = " "

        txt_DiscPerc.Text = ""
        txt_discAmount.Text = ""
        txt_before_addless_tax.Text = ""
        txt_Freight_Name.Text = "Freight"
        txt_Freight_Charge.Text = ""
        lbl_Taxable_Value.Text = ""
        txt_CGST_Percentage.Text = ""   ' "2.5"
        txt_SGST_Percentage.Text = ""   ' "2.5"
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


        Cbo_RateFor.Text = "METER"
        lbl_Taxable_Value.Text = ""


        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()

        Grid_DeSelect()


        cbo_itemgrey.Visible = False
        cbo_itemfp.Visible = False
        txt_discAmount.ReadOnly = True

        cbo_itemgrey.Tag = -1
        cbo_itemfp.Tag = -1

        cbo_itemgrey.Text = ""
        cbo_itemfp.Text = ""

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


        If Me.ActiveControl.Name <> cbo_itemfp.Name Then
            cbo_itemfp.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_itemgrey.Name Then
            cbo_itemgrey.Visible = False
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

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.Ledger_Name as VatAC_Name from Textile_Processing_BillMaking_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.VatAc_IdNo = c.Ledger_IdNo   Where a.ClothProcess_BillMaking_Code = '" & Trim(NewCode) & "'", con)
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then
            lbl_RefNo.Text = dt1.Rows(0).Item("ClothProcess_BillMaking_No").ToString
            dtp_Date.Text = dt1.Rows(0).Item("ClothProcess_BillMaking_Date").ToString
            cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
            txt_billNo.Text = dt1.Rows(0).Item("Bill_No").ToString



            lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

            '------------------
            cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
            cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
            Cbo_RateFor.Text = dt1.Rows(0).Item("Rate_For").ToString

            cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
            ' If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"

            txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
            txt_discAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "########0.00")
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
            lbl_billamount.Text = Format(Val(dt1.Rows(0).Item("Bill_Amount").ToString), "########0.00")
            txt_tds.Text = dt1.Rows(0).Item("Tds_Perc").ToString
            lbl_tds.Text = Format(Val(dt1.Rows(0).Item("Tds_Amount").ToString), "##########0")
            '   lbl_tds.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Tds_Amount").ToString))
            txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
            ' lbl_netamount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")

            lbl_netamount.Text = Common_Procedures.Currency_Format(Val(CSng(dt1.Rows(0).Item("Net_Amount").ToString)))


            '---------------


            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name as Grey_Item_name, C.Cloth_Name as Fp_Item_Name , d.Colour_Name, f.Process_Name from Textile_Processing_BillMaking_Details a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Item_Idno LEFT OUTER JOIN Cloth_Head C ON c.Cloth_Idno = a.Item_To_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno where a.Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
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
                    dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Grey_Item_Name").ToString
                    dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Fp_Item_Name").ToString
                    dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("BillMaking_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("BillMaking_Weight").ToString), "########0.000")
                    dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Cloth_Processing_Receipt_Code").ToString
                    dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Cloth_Processing_Receipt_SlNo").ToString

                    dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Colour_Name").ToString
                    dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Process_Name").ToString

                Next i

            End If

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

                .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
            End With

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

    Private Sub Processing_Bill_Making_Textile_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_itemfp.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_itemfp.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_itemgrey.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_itemgrey.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_vataccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_vataccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Processing_Bill_Making_Textile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        'da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'FP' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
        'da.Fill(dt1)
        'cbo_itemfp.DataSource = dt1
        'cbo_itemfp.DisplayMember = "Processed_Item_Name"

        'da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 12 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        'da.Fill(dt3)
        'cbo_vataccount.DataSource = dt3
        'cbo_vataccount.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        'da.Fill(dt6)
        'cbo_Ledger.DataSource = dt6
        'cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'GREY' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
        'da.Fill(dt7)
        'cbo_itemgrey.DataSource = dt7
        'cbo_itemgrey.DisplayMember = "Processed_Item_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Transport.DataSource = dt1
        cbo_Transport.DisplayMember = "Ledger_DisplayName"


        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("")
        cbo_TaxType.Items.Add("GST-5")
        cbo_TaxType.Items.Add("NO TAX")

        Cbo_RateFor.Items.Clear()
        Cbo_RateFor.Items.Add(" ")
        Cbo_RateFor.Items.Add("METER")
        Cbo_RateFor.Items.Add("KG")


        cbo_itemfp.Visible = False
        cbo_itemfp.Visible = False


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_itemfp.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_itemgrey.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vataccount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_before_addless_tax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filter_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_othercharges.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_tds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_vat.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_ItemGrey.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_itemfp.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_itemgrey.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vataccount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_before_addless_tax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_filter_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_othercharges.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_tds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_vat.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_ItemGrey.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        '   AddHandler txt_before_addless_tax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Note.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_othercharges.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_vat.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filter_billNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_before_addless_tax.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_Note.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_othercharges.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_billNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_vat.KeyPress, AddressOf TextBoxControlKeyPress
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


        AddHandler Cbo_RateFor.Enter, AddressOf ControlGotFocus
        AddHandler Cbo_RateFor.Leave, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub processing_Bill_Making_Textile_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Processing_Bill_Making_Textile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 6 Then
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
                                txt_billNo.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 6)

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
        Dim Qa As Windows.Forms.DialogResult

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Processing_Bill_Making, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Processing_Bill_Making, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

            cmd.CommandText = "Update Textile_Processing_Receipt_Details set Cloth_Processing_BillMaking_Code = '', Cloth_Processing_BillMaking_Increment = Cloth_Processing_BillMaking_Increment - 1 Where Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Textile_Processing_BillMaking_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Textile_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'GREY' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
            da.Fill(dt2)
            cbo_Filter_ItemGrey.DataSource = dt2
            cbo_Filter_ItemGrey.DisplayMember = "Processed_Item_Name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemGrey.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemGrey.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_BillMaking_No from Textile_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothProcess_BillMaking_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_BillMaking_No from Textile_Processing_BillMaking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothProcess_BillMaking_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_BillMaking_No from Textile_Processing_BillMaking_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothProcess_BillMaking_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_BillMaking_No from Textile_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothProcess_BillMaking_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_BillMaking_Head", "ClothProcess_BillMaking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            Dim Da1 As New SqlClient.SqlDataAdapter
            Dim Dt1 As New DataTable
            Dim Dt2 As New DataTable


            'Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.* from Textile_Processing_BillMaking_Head a  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and ClothProcess_BillMaking_Code like  '" & Trim(Pk_Condition) & "%'  Order by a.for_Orderby desc, a.ClothProcess_BillMaking_NO desc", con)
            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.* from Textile_Processing_BillMaking_Head a  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by a.for_Orderby desc, a.ClothProcess_BillMaking_NO desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                If IsDBNull(Dt1.Rows(0).Item("Freight_Name").ToString) = False Then
                    If Trim(Dt1.Rows(0).Item("Freight_Name").ToString) <> "" Then txt_Freight_Name.Text = Dt1.Rows(0).Item("Freight_Name").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("Tax_Type").ToString) = False Then
                    If Trim(Dt1.Rows(0).Item("Tax_Type").ToString) <> "" Then cbo_TaxType.Text = Dt1.Rows(0).Item("Tax_Type").ToString
                End If
                If Dt1.Rows(0).Item("Rate_For").ToString <> "" Then Cbo_RateFor.Text = Dt1.Rows(0).Item("Rate_For").ToString
            End If


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

            Da = New SqlClient.SqlDataAdapter("select ClothProcess_BillMaking_No from Textile_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code = '" & Trim(RecCode) & "'", con)
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

            Da = New SqlClient.SqlDataAdapter("select ClothProcess_BillMaking_No from Textile_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotAmt As Single, vTotMtrs As Single
        Dim Proc_ID As Integer = 0
        Dim cOLOR_ID As Integer = 0
        Dim vTotWeight As Single
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

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Processing_Bill_Making, New_Entry) = False Then Exit Sub

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
                If Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid GREY Item", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub
                    End If


                    If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid FP Item", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)

                        End If
                        Exit Sub
                    End If

                    If Val(dgv_Details.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
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

        If Trim(Cbo_RateFor.Text) = "" Then
            MessageBox.Show("Invalid Rate For", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Cbo_RateFor.Enabled Then Cbo_RateFor.Focus()
            Exit Sub
        End If

        'vatac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_vataccount.Text)
        'If vatac_id = 0 Then
        '    MessageBox.Show("Invalid Vat A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_vataccount.Enabled Then cbo_vataccount.Focus()
        '    Exit Sub
        'End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        NoCalc_Status = False
        Total_Calculation()
        NetAmount_Calculation()

        vTotMtrs = 0 : vTotWeight = 0 : vTotAmt = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If


        tr = con.BeginTransaction


        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_BillMaking_Head", "ClothProcess_BillMaking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@BillMakingDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Textile_Processing_BillMaking_Head(ClothProcess_BillMaking_Code,             Company_IdNo             , ClothProcess_BillMaking_No,                               for_OrderBy,                               ClothProcess_BillMaking_Date,           Ledger_IdNo,                   Bill_No,                     AddLess_BeforeTax_Amount      ,        Assesable_Amount             , VatAc_IdNo,  Vat_Amount,   Vat_Percentage,                  Other_Charges,                    Bill_Amount,              Tds_Perc,                             Tds_Amount,                                     Net_Amount,                 Total_Meters,                  Total_Weight,              Total_Amount   ,         User_idNo              ,      AddLess_AfterTax_Amount              , 	           Freight_Name  ,    	                    Remarks           ,           CGST_Percentage            ,	            CGST_Amount           ,             SGST_Percentage          ,             SGST_Amount           ,          IGST_Percentage         ,                IGST_Amount            ,         Discount_Percentage       ,    	     Discount_Amount         ,     Transport_IdNo       ,           Vehicle_No                 ,            Tax_Type               ,                 Freight_charge             ,      Rate_For      )    " &
                                                                            " Values ('" & Trim(NewCode) & "'     , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @BillMakingDate,            " & Str(Val(Led_ID)) & ",'" & Trim(txt_billNo.Text) & "'," & Val(txt_before_addless_tax.Text) & "," & Val(lbl_Taxable_Value.Text) & "  ,    0     ,    0     ,        0         ,     " & Val(txt_othercharges.Text) & ", " & Val(lbl_billamount.Text) & ", " & Str(Val(txt_tds.Text)) & ", " & Str(Val(lbl_tds.Text)) & "         ,   " & Str(Val(CDbl(lbl_netamount.Text))) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotWeight)) & "," & Val(vTotAmt) & " , " & Val(lbl_UserName.Text) & "  , " & Val(txt_After_Addless_tax.Text) & "    ,'" & Trim(txt_Freight_Name.Text) & "' ,'" & Trim(txt_Remarks.Text) & "' ," & Val(txt_CGST_Percentage.Text) & " ,  " & Val(lbl_CGST_Amount.Text) & " ," & Val(txt_SGST_Percentage.Text) & " ," & Val(lbl_SGST_Amount.Text) & "  ," & Val(txt_IGST_Percentage.Text) & " ," & Val(lbl_IGST_Amount.Text) & "  ," & Val(txt_DiscPerc.Text) & "     ,   " & Val(txt_discAmount.Text) & "    , " & Str(Val(Trans_ID)) & " ,  '" & Trim(cbo_VehicleNo.Text) & "' , '" & Trim(cbo_TaxType.Text) & " ', " & Val(txt_Freight_Charge.Text) & " ,'" & Trim(Cbo_RateFor.Text) & "')"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Textile_Processing_BillMaking_Head Set ClothProcess_BillMaking_Date = @BillMakingDate, Ledger_IdNo = " & Val(Led_ID) & ", Bill_No = '" & Trim(txt_billNo.Text) & "' , Assesable_Amount = " & Val(lbl_Taxable_Value.Text) & ",VatAc_IdNo = 0 ,Vat_Amount = 0 , Vat_Percentage = 0 , Other_Charges = " & Val(txt_othercharges.Text) & " , Bill_Amount = " & Val(lbl_billamount.Text) & ",Tds_Perc = " & Val(txt_tds.Text) & " ,Tds_Amount = " & Val(lbl_tds.Text) & ",Net_Amount = " & Str(Val(CDbl(lbl_netamount.Text))) & ", Total_Meters = " & Val(vTotMtrs) & ",Total_Weight = " & Val(vTotWeight) & " , Total_Amount = " & Val(vTotAmt) & " ,User_idNo = " & Val(lbl_UserName.Text) & " , AddLess_AfterTax_Amount =  " & Val(txt_After_Addless_tax.Text) & "  ,  Freight_Name='" & Trim(txt_Freight_Name.Text) & "'  ,   Remarks=  '" & Trim(txt_Remarks.Text) & "' ,   CGST_Percentage =" & Val(txt_CGST_Percentage.Text) & " ,CGST_Amount=" & Val(lbl_CGST_Amount.Text) & "  , SGST_Percentage =" & Val(txt_SGST_Percentage.Text) & ",SGST_Amount =" & Val(lbl_SGST_Amount.Text) & "  ,IGST_Percentage=" & Val(txt_IGST_Percentage.Text) & " ,   IGST_Amount=" & Val(lbl_IGST_Amount.Text) & "   , Discount_Percentage =" & Val(txt_DiscPerc.Text) & " ,Discount_Amount =  " & Val(txt_discAmount.Text) & " ,  AddLess_BeforeTax_Amount  =" & Val(txt_After_Addless_tax.Text) & ",  Transport_IdNo=" & Str(Val(Trans_ID)) & ",  Vehicle_No='" & Trim(cbo_VehicleNo.Text) & "', Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Freight_charge=" & Val(txt_Freight_Charge.Text) & " ,Rate_for='" & Trim(Cbo_RateFor.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Textile_Processing_Receipt_Details set Cloth_Processing_BillMaking_Code = '', Cloth_Processing_BillMaking_Increment = Cloth_Processing_BillMaking_Increment - 1 Where Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Textile_Processing_BillMaking_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Bill : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then
                        Sno = Sno + 1
                        itgry_id = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(11).Value, tr)
                        Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(12).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Textile_Processing_BillMaking_Details( Cloth_Processing_BillMaking_Code, Company_IdNo, Cloth_Processing_BillMaking_No, for_OrderBy, Cloth_Processing_BillMaking_Date, Sl_No , Dc_Rc_No , Item_Idno, Item_To_Idno, BillMaking_Meters, BillMaking_Weight, Rate, Amount , Cloth_Processing_Receipt_Code , Cloth_Processing_Receipt_Slno , Colour_IdNo , Processing_Idno  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @BillMakingDate," & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(itgry_id)) & ", " & Str(Val(Itfp_ID)) & "," & Val(.Rows(i).Cells(4).Value) & ", " & Val(.Rows(i).Cells(5).Value) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " , '" & Trim(.Rows(i).Cells(8).Value) & "', " & Str(Val(.Rows(i).Cells(9).Value)) & " , " & Str(Val(Col_ID)) & "  ," & Str(Val(Proc_ID)) & " )"
                        cmd.ExecuteNonQuery()

                        ' If Val(vTotMtrs) > 0 Then
                        'cmd.CommandText = "Insert into Stock_Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date,Ledger_IdNo, Party_Bill_No, Sl_No, Particulars, Item_IdNo,Colour_IdNo,Rack_IdNo,Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @BillMakingDate, " & Str(Val(Led_ID)) & ", '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ",'" & Trim(Partcls) & "' ," & Val(itgry_id) & "," & Val(Col_ID) & ", 0, " & Val(.Rows(i).Cells(4).Value) & " )"
                        'cmd.ExecuteNonQuery()
                        ''End If

                        cmd.CommandText = "Update Textile_Processing_Receipt_Details set Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "', Cloth_Processing_BillMaking_Increment = Cloth_Processing_BillMaking_Increment + 1 Where Cloth_Processing_Receipt_Code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Cloth_Processing_Receipt_Slno = " & Str(Val(.Rows(i).Cells(9).Value))
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()



            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()



    End Sub

    'Private Sub Item_Grey()
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt As New DataTable
    '    Dim GITID As Integer



    '    GITID = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_itemgrey.Text)



    '    If GITID <> 0 Then

    '        Da = New SqlClient.SqlDataAdapter("select * from Processed_Item_Head where Processed_Item_IdNo = " & Str(Val(GITID)) & " and Processed_Item_Type= 'GREY' ", con)
    '        Da.Fill(Dt)


    '        If Dt.Rows.Count > 0 Then

    '            dgv_Details.CurrentRow.Cells(8).Value = Dt.Rows(0).Item("Meter_Qty").ToString

    '        End If


    '        Dt.Clear()
    '        Dt.Dispose()
    '        Da.Dispose()

    '    End If

    'End Sub

    Private Sub Total_Calculation()
        Dim vTotPcs As String, vTotMtrs As String, vtotweight As String, vtotamt As String

        Dim i As Integer
        Dim sno As Integer


        vTotPcs = 0 : vTotMtrs = 0 : vtotweight = 0 : sno = 0

        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then
                    '.Rows(i).Cells(9).Value = Val(dgv_Details.Rows(i).Cells(7).Value) * Val(dgv_Details.Rows(i).Cells(8).Value)

                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(5).Value)
                    vtotamt = vtotamt + Val(dgv_Details.Rows(i).Cells(7).Value)
                End If
            Next
        End With

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(4).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(5).Value = Format(Val(vtotweight), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vtotamt), "#########0.000")
        lbl_Taxable_Value.Text = Format(Val(vtotamt), "#########0.000")

        NetAmount_Calculation()

    End Sub
    Private Sub Amount_Calculation()
        Dim vtotweight As String, vtotamt As Single

        Dim vtotMetrs As String

        Dim i As Integer
        Dim sno As Integer

        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub


        vtotweight = 0 : sno = 0 : vtotMetrs = 0

        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Then

                    vtotMetrs = Val(dgv_Details.Rows(i).Cells(4).Value)
                    vtotweight = Val(dgv_Details.Rows(i).Cells(5).Value)                                           '5 weight

                    If Trim(UCase(Cbo_RateFor.Text)) = "KG" Then

                        vtotamt = vtotweight * Val(dgv_Details.Rows(i).Cells(6).Value)                             '6 rate

                    ElseIf Trim(UCase(Cbo_RateFor.Text)) = "METER" Then

                        vtotamt = vtotMetrs * Val(dgv_Details.Rows(i).Cells(6).Value)



                    End If


                        dgv_Details.Rows(i).Cells(7).Value = Format(Val(vtotamt), "#########0.00")


                End If

            Next
        End With
        Total_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()

        '-----------------------

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
                vTotAmt = Format(Val(.Rows(0).Cells(7).Value), "###########0.00")

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, txt_billNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

        'If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    If dgv_Details.Rows.Count > 0 Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

        '    Else
        '        txt_billNo.Focus()

        '    End If
        'End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select order:", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

                'ElseIf dgv_Details.Rows.Count > 0 Then
                '    dgv_Details.Focus()
                '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_billNo.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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
        With dgv_Details

            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
            If .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If

        End With
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

                If cbo_itemgrey.Visible = False Or Val(cbo_itemgrey.Tag) <> e.RowIndex Then

                    cbo_itemfp.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'GREY ' order by Processed_item_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_itemgrey.DataSource = Dt1
                    cbo_itemgrey.DisplayMember = "Processed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_itemgrey.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_itemgrey.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_itemgrey.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_itemgrey.Height = rect.Height  ' rect.Height
                    cbo_itemgrey.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_itemgrey.Tag = Val(e.RowIndex)
                    cbo_itemgrey.Visible = True

                    cbo_itemgrey.BringToFront()
                    cbo_itemgrey.Focus()



                End If


            Else

                cbo_itemgrey.Visible = False

            End If

            If e.ColumnIndex = 3 Then

                If cbo_itemfp.Visible = False Or Val(cbo_itemfp.Tag) <> e.RowIndex Then

                    cbo_itemfp.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_itemfp.DataSource = Dt2
                    cbo_itemfp.DisplayMember = "Procesed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_itemfp.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_itemfp.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_itemfp.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_itemfp.Height = rect.Height  ' rect.Height

                    cbo_itemfp.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_itemfp.Tag = Val(e.RowIndex)
                    cbo_itemfp.Visible = True

                    cbo_itemfp.BringToFront()
                    cbo_itemfp.Focus()


                End If

            Else

                cbo_itemfp.Visible = False


            End If



        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If

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
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then

                    Amount_Calculation()

                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown



        With dgv_Details

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True


                    cbo_Ledger.Focus()
                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True

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
        If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 4 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 5 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 6 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 7 Then

            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
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
    Private Sub cbo_itemgrey_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemgrey.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

    End Sub

    Private Sub cbo_itemgrey_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemgrey.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_itemgrey, Nothing, cbo_itemfp, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_itemgrey.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_itemgrey.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_itemgrey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_itemgrey.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_itemgrey, cbo_itemfp, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_itemgrey_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemgrey.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_itemgrey.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub Cbo_itemgrey_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemgrey.TextChanged
        Try
            If cbo_itemgrey.Visible Then
                With dgv_Details
                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                    If Val(cbo_itemgrey.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_itemgrey.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_itemfp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemfp.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

    End Sub


    Private Sub cbo_itemfp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemfp.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_itemfp, cbo_itemgrey, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_itemfp.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_itemfp.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_itemfp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_itemfp.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_itemfp, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_itemfp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemfp.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_itemfp.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_itemfp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemfp.TextChanged
        Try
            If cbo_itemfp.Visible Then
                With dgv_Details
                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                    If Val(cbo_itemfp.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_itemfp.Text)
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
        Dim Led_IdNo As Integer, procit_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            procit_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothProcess_BillMaking_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothProcess_BillMaking_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothProcess_BillMaking_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemGrey.Text) <> "" Then
                procit_IdNo = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_Filter_ItemGrey.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If


            If Val(procit_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.ClothProcess_BillMaking_Code IN (select z1.Cloth_Processing_BillMaking_Code from Textile_Processing_BillMaking_Details z1 where z1.Item_Idno = " & Str(Val(procit_IdNo)) & ")"
            End If

            If Trim(txt_filter_billNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Bill_No = '" & Trim(txt_filter_billNo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Textile_Processing_BillMaking_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_BillMaking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ClothProcess_BillMaking_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("ClothProcess_BillMaking_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothProcess_BillMaking_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    ' dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                    ' dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Process_Name").ToString
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Delivery_Pcs").ToString)
                    ' dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Delivery_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, txt_filter_billNo, cbo_Filter_ItemGrey, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemGrey, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ItemGrey_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemGrey.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'GREY')", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ItemGrey_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemGrey.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemGrey, cbo_Filter_PartyName, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'GREY')", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ItemGrey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemGrey.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemGrey, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'GREY')", "(Processed_Item_idno = 0)")

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



        'If Asc(e.KeyChar) = 13 Then
        '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        save_record()
        '    Else
        '        dtp_Date.Focus()
        '    End If
        'End If
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    'Private Sub txt_addless_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_before_addless_tax.KeyPress
    '    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    'End Sub

    'Private Sub txt_addless_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_before_addless_tax.TextChanged
    '    NetAmount_Calculation()
    'End Sub

    'Private Sub txt_vat_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_vat.KeyPress
    '    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    'End Sub

    'Private Sub txt_vat_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_vat.TextChanged
    '    NetAmount_Calculation()
    'End Sub

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

    Private Sub txt_tds_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_tds.TextChanged
        NetAmount_Calculation()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String


        ' If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Cotton_Sales_GST, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Textile_Processing_BillMaking_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and clothProcess_BillMaking_code = '" & Trim(NewCode) & "'", con)
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
    Private Sub PrintPreview_Shown(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ts As ToolStrip = CType(sender.Controls(1), ToolStrip)

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

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name as Grey_Item_name, C.Cloth_Name as Fp_Item_Name , d.Rate As Ent_Rate from Textile_Processing_Receipt_Details a LEFT OUTER JOIN cLOTH_Head b ON  b.cLOTH_IdNo = a.Item_Idno LEFT OUTER JOIN cLOTH_Head C ON c.cLOTH_IdNo = a.Item_To_Idno  LEFT OUTER JOIN Textile_Processing_BillMaking_Details d ON d.Cloth_Processing_Receipt_Code = a.Cloth_Processing_Receipt_Code and d.Cloth_Processing_Receipt_Slno = a.Cloth_Processing_Receipt_Slno where a.Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Cloth_Processing_Receipt_Date, a.for_orderby, a.Cloth_Processing_Receipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    Ent_Rate = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then
                        Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Processing_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Grey_Item_name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Fp_Item_Name").ToString
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(7).Value = "1"
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Code").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString
                    .Rows(n).Cells(10).Value = Ent_Rate

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name as Grey_Item_name, C.Cloth_Name as Fp_Item_Name , d.Rate As Ent_Rate , E.Colour_Name, f.Process_Name from Textile_Processing_Receipt_Details a LEFT OUTER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Item_Idno LEFT OUTER JOIN Cloth_Head C ON c.Cloth_Idno = a.Item_To_Idno  LEFT OUTER JOIN Textile_Processing_BillMaking_Details d ON d.Cloth_Processing_BillMaking_Code = a.Cloth_Processing_Receipt_Code  LEFT OUTER JOIN Colour_Head E ON E.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno where a.Cloth_Processing_BillMaking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Cloth_Processing_Receipt_Date, a.for_orderby, a.Cloth_Processing_Receipt_No", con)
            Dt1 = New DataTable
            NR = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Processing_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Grey_Item_name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Fp_Item_Name").ToString
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(7).Value = ""
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Code").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString
                    .Rows(n).Cells(10).Value = 0

                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Colour_Name").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Process_Name").ToString
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

                .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(7).Value = ""

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

            If Val(dgv_Selection.Rows(i).Cells(7).Value) = 1 Then

                ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(6).Value * dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(8).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value

                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(11).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(12).Value

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
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
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



        'If Val(txt_DiscPerc.Text) <> 0 Then
        '    txt_discAmount.ReadOnly = True
        'Else
        '    txt_discAmount.ReadOnly = False
        'End If


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
    Private Sub txt_discAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_discAmount.KeyDown
        If e.KeyValue = 38 Then
            If txt_DiscPerc.Enabled = True Then

                txt_DiscPerc.Focus()

            Else

                dgv_Details.Focus()


            End If

        ElseIf e.KeyValue = 40 Then

            txt_before_addless_tax.Focus()


        End If

    End Sub


    Private Sub txt_discAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_discAmount.KeyPress

        'If Val(txt_DiscPerc.Text) <> 0 Then
        '    txt_discAmount.ReadOnly = True
        'ElseIf Val(txt_DiscPerc.Text) = 0 Then
        '    txt_discAmount.ReadOnly = False
        'End If

        If Asc(e.KeyChar) = 13 Then
            txt_before_addless_tax.Focus()
        End If
    End Sub

    Private Sub txt_discAmount_TextChanged(sender As Object, e As EventArgs) Handles txt_discAmount.TextChanged
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

    Private Sub txt_billNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_billNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Transport.Focus()
        End If
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, Cbo_RateFor, Nothing, "", "", "", "")

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

    'Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
    '    NetAmount_Calculation()
    'End Sub

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

    Private Sub cbo_TaxType_TextChanged(sender As Object, e As EventArgs) Handles cbo_TaxType.TextChanged

        If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
            cbo_TaxType.Tag = cbo_TaxType.Text
            NetAmount_Calculation()
        End If

    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Processing_BillMaking_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, Cbo_RateFor, "Textile_Processing_BillMaking_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, Cbo_RateFor, "Textile_Processing_BillMaking_Head", "Vehicle_No", "", "", False)
    End Sub

    Private Sub txt_After_Addless_tax_TextChanged(sender As Object, e As EventArgs) Handles txt_After_Addless_tax.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub dgtxt_details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_details.TextChanged
        Try
            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_details.Text)

                    End If


                End If


            End With

            'With dgv_Details_Total
            '    If .Rows(.CurrentCell.RowIndex).Cells(7).Value <> 0 Then
            '        NetAmount_Calculation()
            '    End If

            'End With

            '-------------

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Ledger_TextChanged(sender As Object, e As EventArgs) Handles cbo_Ledger.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub btn_Print_Click(sender As Object, e As EventArgs) Handles btn_Print.Click
        print_record()
    End Sub
    'Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim NewCode As String
    '    Dim W1 As Single = 0

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    prn_HdDt.Clear()
    '    prn_DetDt.Clear()
    '    prn_DetIndx = 0
    '    prn_DetSNo = 0
    '    prn_PageNo = 0

    '    prn_Count = 0

    '    Try

    '        ''      da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* , e.Ledger_Name as Agent_Name ,SH.* ,Lsh.State_Name as Ledger_State_Name ,Lsh.State_Code as Ledger_State_Code, f.Ledger_MainName as DelName , f.Ledger_Address1 as DelAdd1 ,f.Ledger_Address2 as DelAdd2, f.Ledger_Address3 as DelAdd3 ,f.Ledger_Address4 as DelAdd4, f.Pan_No DelPanNo, f.Ledger_GSTinNo as DelGSTinNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code from Textile_Processing_BillMaking_Head a " &
    '        '                                         "  INNER JOIN Company_Head b        ON a.Company_IdNo        = b.Company_IdNo " &
    '        '                                         "  INNER JOIN Ledger_Head c         ON a.Ledger_IdNo         = c.Ledger_IdNo " &
    '        '                                         "  LEFT OUTER JOIN Ledger_Head e    ON e.Ledger_IdNo         = a.Agent_IdNo " &
    '        '                                         "  LEFT OUTER JOIN State_Head Lsh   ON c.Ledger_State_Idno   = Lsh.State_IDno " &
    '        '                                         "  LEFT OUTER JOIN Ledger_Head f    ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo " &
    '        '                                         "  LEFT OUTER JOIN State_HEad DSH   on f.Ledger_State_IdNo = DSH.State_IdNo " &
    '        '                                         "  LEFT OUTER JOIN State_Head SH    ON b.Company_State_IdNo  = SH.State_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.clothProcess_BillMaking_no = '" & Trim(NewCode) & "'", con)


    '        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* ,f.Ledger_Name as transportname  ,SH.* ,Lsh.State_Name as Ledger_State_Name ,Lsh.State_Code as Ledger_State_Code 		  from Textile_Processing_BillMaking_Head a INNER JOIN Company_Head b        ON a.Company_IdNo        = b.Company_IdNo  INNER JOIN Ledger_Head c         ON a.Ledger_IdNo         = c.Ledger_IdNo    LEFT OUTER JOIN State_Head Lsh   ON c.Ledger_State_Idno   = Lsh.State_IDno    LEFT OUTER JOIN Ledger_Head f    ON a.Transport_IdNo = f.Transport_IdNo       LEFT OUTER JOIN State_HEad DSH   on f.Ledger_State_IdNo = DSH.State_IdNo    LEFT OUTER JOIN State_Head SH    ON b.Company_State_IdNo  = SH.State_Idno		where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.clothProcess_BillMaking_Code = '" & Trim(NewCode) & "'", con)
    '        prn_HdDt = New DataTable
    '        da1.Fill(prn_HdDt)

    '        If prn_HdDt.Rows.Count > 0 Then

    '            'da2 = New SqlClient.SqlDataAdapter("select a.* , b.CLOTH_nAME  from Textile_Processing_BillMaking_DETAILS a INNER JOIN CLOTH_HEAD b ON a.CLOTH_IdNo = b.CLOTH_IdNo  where a.clothProcess_BillMaking_code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.clothProcess_BillMaking_no", con)


    '            da2 = New SqlClient.SqlDataAdapter("select a.* , b.CLOTH_nAME as item_Grey,c.cloth_name as item_Fp    from Textile_Processing_BillMaking_DETAILS a   left outer join  CLOTH_HEAD b ON a.Item_Idno = b.CLOTH_IdNo    left outer join   CLOTH_HEAD c on a.Item_To_Idno = c.CLOTH_IdNo where a.Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Cloth_Processing_BillMaking_No", con)
    '            prn_DetDt = New DataTable
    '            da2.Fill(prn_DetDt)


    '        Else
    '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End If

    '        da1.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub
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
        DetIndx = 1 ' 0 '1
        DetSNo = 0
        prn_DetMxIndx = 0
        prn_Count = 0

        Try

            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sales_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "' ", con)
            'prn_HdDt_VAT = New DataTable
            'da1.Fill(prn_HdDt_VAT)

            'If prn_HdDt_VAT.Rows.Count > 0 Then

            '    da2 = New SqlClient.SqlDataAdapter("select a.* from Sales_Details a  where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Sales_No", con)
            '    prn_DetDt_VAT = New DataTable
            '    da2.Fill(prn_DetDt_VAT)

            'Else
            '    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            'End If
            'da1.Dispose()

            '----------------------GST GST GST-----------------------------------------------
            Try

                ''da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
                'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, b.Pan_No Ledger_PanNo, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, g.Ledger_Name as DelName ,g.Ledger_Address1 as DelAdd1 ,g.Ledger_Address2 as DelAdd2, g.Ledger_Address3 as DelAdd3 ,g.Ledger_Address4 as DelAdd4, g.Ledger_GSTinNo as DelGSTinNo, g.Pan_No as DelPanNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code , Tr.Transport_Name as TransportName from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo LEFT OUTER JOIN Transport_Head Tr on Tr.Transport_Idno = a.Transport_Idno LEFT OUTER JOIN Ledger_Head g ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = g.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on g.State_IdNo = DSH.State_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
                'prn_HdDt = New DataTable
                'da1.Fill(prn_HdDt)





                da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* ,f.Ledger_Name as transportname  ,SH.* ,Lsh.State_Name as Ledger_State_Name ,Lsh.State_Code as Ledger_State_Code 		  from Textile_Processing_BillMaking_Head a INNER JOIN Company_Head b        ON a.Company_IdNo        = b.Company_IdNo  INNER JOIN Ledger_Head c         ON a.Ledger_IdNo         = c.Ledger_IdNo    LEFT OUTER JOIN State_Head Lsh   ON c.Ledger_State_Idno   = Lsh.State_IDno    LEFT OUTER JOIN Ledger_Head f    ON a.Transport_IdNo = f.LEDGER_IDNO       LEFT OUTER JOIN State_HEad DSH   on f.Ledger_State_IdNo = DSH.State_IdNo    LEFT OUTER JOIN State_Head SH    ON b.Company_State_IdNo  = SH.State_Idno		where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.clothProcess_BillMaking_Code = '" & Trim(NewCode) & "'", con)
                prn_HdDt = New DataTable
                da1.Fill(prn_HdDt)


                If prn_HdDt.Rows.Count > 0 Then

                    'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, b.Item_Name_tamil,b.Item_Description as print_name, c.Unit_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on a.unit_idno = c.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                    'prn_DetDt = New DataTable
                    '    da2.Fill(prn_DetDt)


                    '  'da2 = New SqlClient.SqlDataAdapter("select a.* , b.CLOTH_nAME  from Textile_Processing_BillMaking_DETAILS a INNER JOIN CLOTH_HEAD b ON a.CLOTH_IdNo = b.CLOTH_IdNo  where a.clothProcess_BillMaking_code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.clothProcess_BillMaking_no", con)


                    da2 = New SqlClient.SqlDataAdapter("select a.* , b.CLOTH_nAME as item_Grey,c.cloth_name as item_Fp    from Textile_Processing_BillMaking_DETAILS a   left outer join  CLOTH_HEAD b ON a.Item_Idno = b.CLOTH_IdNo    left outer join   CLOTH_HEAD c on a.Item_To_Idno = c.CLOTH_IdNo where a.Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Cloth_Processing_BillMaking_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)


                    If prn_DetDt.Rows.Count > 0 Then

                        prn_DetMxIndx = 0
                        For I = 0 To prn_DetDt.Rows.Count - 1

                            'If Trim(prn_DetDt.Rows(I).Item("Item_Name_Tamil").ToString) <> "" Then
                            '    ItmNm1 = Trim(prn_DetDt.Rows(I).Item("Item_Name_Tamil").ToString)

                            'ElseIf Trim(prn_DetDt.Rows(I).Item("print_name").ToString) <> "" Then
                            '    ItmNm1 = Trim(prn_DetDt.Rows(I).Item("print_name").ToString)
                            'Else
                            '    ItmNm1 = Trim(prn_DetDt.Rows(I).Item("Item_Name").ToString)
                            'End If
                            'ItmNm2 = ""
                            'If Len(ItmNm1) > 30 Then
                            '    For K = 30 To 1 Step -1
                            '        If Mid$(Trim(ItmNm1), K, 1) = " " Or Mid$(Trim(ItmNm1), K, 1) = "," Or Mid$(Trim(ItmNm1), K, 1) = "." Or Mid$(Trim(ItmNm1), K, 1) = "-" Or Mid$(Trim(ItmNm1), K, 1) = "/" Or Mid$(Trim(ItmNm1), K, 1) = "_" Or Mid$(Trim(ItmNm1), K, 1) = "(" Or Mid$(Trim(ItmNm1), K, 1) = ")" Or Mid$(Trim(ItmNm1), K, 1) = "\" Or Mid$(Trim(ItmNm1), K, 1) = "[" Or Mid$(Trim(ItmNm1), K, 1) = "]" Or Mid$(Trim(ItmNm1), K, 1) = "{" Or Mid$(Trim(ItmNm1), K, 1) = "}" Then Exit For
                            '    Next K
                            '    If K = 0 Then K = 30
                            '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - K)
                            '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), K - 1)
                            'End If

                            prn_DetMxIndx = prn_DetMxIndx + 1
                            'If txt_StartingSlNo.Visible = True Then
                            '    prn_DetAr(prn_DetMxIndx, 1) = prn_DetDt.Rows(I).Item("Sl_No").ToString
                            '   Else
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(Val(I) + 1)
                            '      End If

                            'prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm1)
                            prn_DetAr(prn_DetMxIndx, 2) = prn_DetDt.Rows(I).Item("dc_rc_no").ToString
                            prn_DetAr(prn_DetMxIndx, 3) = prn_DetDt.Rows(I).Item("item_Grey").ToString
                            prn_DetAr(prn_DetMxIndx, 4) = prn_DetDt.Rows(I).Item("item_Fp").ToString
                            prn_DetAr(prn_DetMxIndx, 5) = Format(Val(prn_DetDt.Rows(I).Item("BillMaking_Meters").ToString), "########0.000")
                            prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_DetDt.Rows(I).Item("billmaking_Weight").ToString), "########0.000")
                            prn_DetAr(prn_DetMxIndx, 7) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Rate").ToString), "########0.00"))
                            prn_DetAr(prn_DetMxIndx, 8) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Amount").ToString), "########0.00"))
                            'prn_DetAr(prn_DetMxIndx, 9) = ""

                            'prn_DetAr(prn_DetMxIndx, 10) = prn_DetDt.Rows(I).Item("Dc_No").ToString
                            'prn_DetAr(prn_DetMxIndx, 11) = prn_DetDt.Rows(I).Item("Item_Description").ToString




                            'If Trim(ItmNm2) <> "" Then
                            '    prn_DetMxIndx = prn_DetMxIndx + 1
                            '    prn_DetAr(prn_DetMxIndx, 1) = ""
                            '    prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm2)
                            '    prn_DetAr(prn_DetMxIndx, 3) = ""
                            '    prn_DetAr(prn_DetMxIndx, 4) = ""
                            '    prn_DetAr(prn_DetMxIndx, 5) = ""
                            '    prn_DetAr(prn_DetMxIndx, 6) = ""
                            '    prn_DetAr(prn_DetMxIndx, 7) = "ITEM_2ND_LINE"
                            'End If

                            '                            If Trim(prn_DetDt.Rows(I).Item("Serial_No").ToString) <> "" Then

                            '                                Erase BlNoAr
                            '                                BlNoAr = New String(20) {}

                            '                                m1 = 0
                            '                                bln = "S/No : " & Trim(prn_DetDt.Rows(I).Item("Serial_No").ToString)

                            'LOOP1:
                            '                                If Len(bln) > 47 Then
                            '                                    For K = 47 To 1 Step -1
                            '                                        If Mid$(bln, K, 1) = " " Or Mid$(bln, K, 1) = "," Or Mid$(bln, K, 1) = "/" Or Mid$(bln, K, 1) = "\" Or Mid$(bln, K, 1) = "-" Or Mid$(bln, K, 1) = "." Or Mid$(bln, K, 1) = "&" Or Mid$(bln, K, 1) = "_" Then Exit For
                            '                                    Next K
                            '                                    If K = 0 Then K = 47
                            '                                    m1 = m1 + 1
                            '                                    BlNoAr(m1) = Microsoft.VisualBasic.Left(Trim(bln), K)
                            '                                    'BlNoAr(m1) = Microsoft.VisualBasic.Left(Trim(bln), K - 1)
                            '                                    bln = Microsoft.VisualBasic.Right(bln, Len(bln) - K)
                            '                                    If Len(bln) <= 47 Then
                            '                                        m1 = m1 + 1
                            '                                        BlNoAr(m1) = bln
                            '                                    Else
                            '                                        GoTo LOOP1
                            '                                    End If

                            '                                Else
                            '                                    m1 = m1 + 1
                            '                                    BlNoAr(m1) = bln

                            '                                End If

                            '                                For K = 1 To m1
                            '                                    prn_DetMxIndx = prn_DetMxIndx + 1
                            '                                    prn_DetAr(prn_DetMxIndx, 1) = ""
                            '                                    prn_DetAr(prn_DetMxIndx, 2) = Trim(BlNoAr(K))
                            '                                    prn_DetAr(prn_DetMxIndx, 3) = ""
                            '                                    prn_DetAr(prn_DetMxIndx, 4) = ""
                            '                                    prn_DetAr(prn_DetMxIndx, 5) = ""
                            '                                    prn_DetAr(prn_DetMxIndx, 6) = ""
                            '                                    prn_DetAr(prn_DetMxIndx, 7) = "SERIALNO"
                            '                                Next K

                            '                            End If

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
            '--------------------------------------------------------------------------------------------------
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format_GST_1061(e)

        '   Printing_GST_Format1(e)

    End Sub
    Private Sub Printing_GST_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
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
        Dim Cmp_Name As String = ""
        Dim Wgt_Bag As String = ""
        Dim BagNo1 As String = "", BagNo2 As String = ""

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 45
            .Top = 20
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)
        'pFont = New Font("Calibri", 11, FontStyle.Regular)

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

        NoofItems_PerPage = 9 '3 ' 5
8:

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 30 : ClArr(2) = 50 : ClArr(3) = 180 : ClArr(4) = 150 : ClArr(5) = 80 : ClArr(6) = 80 : ClArr(7) = 70  ': ClArr(8) = 100
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        'ClArr(1) = 30 : ClArr(2) = 100 : ClArr(3) = 200 : ClArr(4) = 75 : ClArr(5) = 50 : ClArr(6) = 50 : ClArr(7) = 75 : ClArr(8) = 75
        'ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.6 ' 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0
                CHk_Details_Cnt = 0
                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        'ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("cloth_Name").ToString)
                        'ItmNm2 = ""
                        'If Len(ItmNm1) > 35 Then
                        '    For I = 35 To 1 Step -1
                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        '    Next I
                        '    If I = 0 Then I = 35
                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        'End If

                        'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Bag_No").ToString) <> "" Then
                        '    BagNo1 = "BAG NOs. : " & prn_DetDt.Rows(prn_DetIndx).Item("Bag_No").ToString
                        '    BagNo2 = ""
                        '    If Len(BagNo1) > 25 Then
                        '        For I = 25 To 1 Step -1
                        '            If Mid$(Trim(BagNo1), I, 1) = " " Or Mid$(Trim(BagNo1), I, 1) = "," Or Mid$(Trim(BagNo1), I, 1) = "." Or Mid$(Trim(BagNo1), I, 1) = "-" Or Mid$(Trim(BagNo1), I, 1) = "/" Or Mid$(Trim(BagNo1), I, 1) = "_" Or Mid$(Trim(BagNo1), I, 1) = "\" Or Mid$(Trim(BagNo1), I, 1) = "[" Or Mid$(Trim(BagNo1), I, 1) = "]" Or Mid$(Trim(BagNo1), I, 1) = "{" Or Mid$(Trim(BagNo1), I, 1) = "}" Then Exit For
                        '        Next I
                        '        If I = 0 Then I = 25
                        '        BagNo2 = Microsoft.VisualBasic.Right(Trim(BagNo1), Len(BagNo1) - I)
                        '        BagNo1 = Microsoft.VisualBasic.Left(Trim(BagNo1), I - 1)
                        '    End If
                        'End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("dc_rc_no").ToString, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("item_Grey").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("item_Fp").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("BillMaking_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("billmaking_Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 5, CurY, 2, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)


                        'Wgt_Bag = "0"
                        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" Then '---- SOUTHERN COT SPINNERS
                        '    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                        '        Wgt_Bag = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString) / Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), "#########0.000")
                        '    End If
                        'End If
                        'If Val(Wgt_Bag) <> 0 Then
                        '    Common_Procedures.Print_To_PrintDocument(e, Val(Wgt_Bag), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        'End If

                        '' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("total_Weight").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("total_AMOUNT").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If


                        p1Font = New Font("Calibri", 9, FontStyle.Bold)

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1286" Then
                            If Trim(BagNo1) <> "" Then
                                CurY = CurY + TxtHgt + TxtHgt - 10
                                NoofDets = NoofDets + 2
                                Common_Procedures.Print_To_PrintDocument(e, BagNo1, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                                If Trim(BagNo2) = "" Then CurY = CurY + TxtHgt : NoofDets = NoofDets + 1
                            End If

                            W1 = e.Graphics.MeasureString("BAG NOs. : ", p1Font).Width

                            If Trim(BagNo2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                NoofDets = NoofDets + 1
                                Common_Procedures.Print_To_PrintDocument(e, BagNo2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                                CurY = CurY + TxtHgt : NoofDets = NoofDets + 1
                            End If
                        End If


                        prn_DetIndx = prn_DetIndx + 1
                        CHk_Details_Cnt = prn_DetIndx
                    Loop

                End If

                Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

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

    Private Sub Printing_GST_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
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
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

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

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else

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

        CurY = CurY + TxtHgt - 15

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1256" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1286" Then
            p1Font = New Font("Calibri", 22, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 30, FontStyle.Bold)
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
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

        'If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

        '    If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
        '        Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
        '        If Not imageData Is Nothing Then
        '            Using ms As New MemoryStream(imageData, 0, imageData.Length)
        '                ms.Write(imageData, 0, imageData.Length)

        '                If imageData.Length > 0 Then

        '                    pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

        '                    e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 110, CurY + 35, 80, 80)

        '                End If

        '            End Using
        '        End If
        '    End If

        'End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 5
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 3
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

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

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


        'If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
        '    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

        '    ItmNm2 = ""
        '    If Len(ItmNm1) > 35 Then
        '        For i = 35 To 1 Step -1
        '            If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
        '        Next i
        '        If i = 0 Then i = 35

        '        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
        '        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
        '    End If

        '    CurY = CurY + TxtHgt + 2
        '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
        '    Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

        '    If Trim(ItmNm2) <> "" Then
        '        CurY = CurY + TxtHgt
        '        Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
        '        Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
        '    End If


        'End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        vHeading = "BILL PASS STATEMENT"

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try


            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)       :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_BillMaking_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportname").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_BillMaking_date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Tamil Nadu", LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            'End If

            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY1 = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY = CurY1 + TxtHgt + 5


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            '   Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If



                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 25, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 80, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 80, LnAr(3))

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 5 + TxtHgt + 15
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            '***** GST START *****

            CurY = CurY + TxtHgt - 5




            Common_Procedures.Print_To_PrintDocument(e, "SNo", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "REC NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NO(GREY)", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM(FP)", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)


            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BankNm5 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Rup1 As String = "", Rup2 As String = ""
        Dim M As Integer = 0


        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            '    Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), "##########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Weight").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("TOTAL_AMOUNT").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)

                'If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                'Else
                '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                'End If
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
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

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm5 = Trim(BnkDetAr(BInc))
                End If

            End If






            Y1 = CurY
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assesable_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt

            CurY1 = CurY
            '***************************************
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or", LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(txt_CGST_Percentage.Text), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(txt_SGST_Percentage.Text), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "quality defect noted in yarn please inform with firat fabric roll at once. We will", LMargin + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "accept only one roll at defect otherwise we do not hold ourself responsible.", LMargin + 25, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(txt_IGST_Percentage.Text), "##########0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)



            Common_Procedures.Print_To_PrintDocument(e, "Total  TAX Amount", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "3. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("TDS_AMOUNT").ToString) <> 0 Then

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

                Common_Procedures.Print_To_PrintDocument(e, "Tds" & "  @ " & (prn_HdDt.Rows(0).Item("tds_perc").ToString) & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("TdS_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                CurY = CurY + TxtHgt
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)



            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "4. All Payment should be made by A/C payer cheque or draft.", LMargin + 10, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 + 10, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "5. Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " Jurisdiction Only. ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            LnAr(10) = CurY

            Y1 = CurY + 0.55
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, Y2)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 10, CurY, 1, 0, pFont)

            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            ''e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            CurY = CurY + 5




            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                    BmsInWrds = Trim(UCase(BmsInWrds))
                Else
                    BmsInWrds = Trim(StrConv(BmsInWrds, VbStrConv.ProperCase))
                End If

                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & BmsInWrds, LMargin + 10, CurY, 0, 0, p1Font)


            End If



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



            LnAr(14) = CurY

            p1Font = New Font("Calibri", 7.5, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Bank Details : ", LMargin + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + 5
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm5, LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(14))


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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
        Dim VItemGryNm1 As String, VItemGryNm2 As String
        Dim VItemGryNm3 As String, VItemGryNm4 As String
        Dim VItemFpNm1 As String, VItemFpNm2 As String
        Dim VItemFpNm3 As String, VItemFpNm4 As String
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
        NoofItems_PerPage = 18 '15 ' 19  


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClArr(1) = 35 : ClArr(2) = 50 : ClArr(3) = 150 : ClArr(4) = 150 : ClArr(5) = 70 : ClArr(6) = 70 : ClArr(7) = 70
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))




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

                    'CurY = CurY - 10

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



                            VItemGryNm1 = Trim(prn_DetAr(DetIndx, 3))
                            VItemGryNm2 = ""
                            VItemGryNm3 = ""
                            VItemGryNm4 = ""

                            If Len(VItemGryNm1) > 20 Then
                                For K = 20 To 1 Step -1
                                    If Mid$(Trim(VItemGryNm1), K, 1) = " " Or Mid$(Trim(VItemGryNm1), K, 1) = "," Or Mid$(Trim(VItemGryNm1), K, 1) = "." Or Mid$(Trim(VItemGryNm1), K, 1) = "-" Or Mid$(Trim(VItemGryNm1), K, 1) = "/" Or Mid$(Trim(VItemGryNm1), K, 1) = "_" Or Mid$(Trim(VItemGryNm1), K, 1) = "(" Or Mid$(Trim(VItemGryNm1), K, 1) = ")" Or Mid$(Trim(VItemGryNm1), K, 1) = "\" Or Mid$(Trim(VItemGryNm1), K, 1) = "[" Or Mid$(Trim(VItemGryNm1), K, 1) = "]" Or Mid$(Trim(VItemGryNm1), K, 1) = "{" Or Mid$(Trim(VItemGryNm1), K, 1) = "}" Then Exit For
                                Next K
                                If K = 0 Then K = 20
                                VItemGryNm2 = Microsoft.VisualBasic.Right(Trim(VItemGryNm1), Len(VItemGryNm1) - K)
                                VItemGryNm1 = Microsoft.VisualBasic.Left(Trim(VItemGryNm1), K - 1)
                            End If



                            If Len(VItemGryNm2) > 20 Then
                                For K = 20 To 1 Step -1
                                    If Mid$(Trim(VItemGryNm2), K, 1) = " " Or Mid$(Trim(VItemGryNm2), K, 1) = "," Or Mid$(Trim(VItemGryNm2), K, 1) = "." Or Mid$(Trim(VItemGryNm2), K, 1) = "-" Or Mid$(Trim(VItemGryNm2), K, 1) = "/" Or Mid$(Trim(VItemGryNm2), K, 1) = "_" Or Mid$(Trim(VItemGryNm2), K, 1) = "(" Or Mid$(Trim(VItemGryNm2), K, 1) = ")" Or Mid$(Trim(VItemGryNm2), K, 1) = "\" Or Mid$(Trim(VItemGryNm2), K, 1) = "[" Or Mid$(Trim(VItemGryNm2), K, 1) = "]" Or Mid$(Trim(VItemGryNm2), K, 1) = "{" Or Mid$(Trim(VItemGryNm2), K, 1) = "}" Then Exit For
                                Next K
                                If K = 0 Then K = 20
                                VItemGryNm3 = Microsoft.VisualBasic.Right(Trim(VItemGryNm2), Len(VItemGryNm2) - K)
                                VItemGryNm2 = Microsoft.VisualBasic.Left(Trim(VItemGryNm2), K - 1)
                            End If




                            VItemFpNm1 = Trim(prn_DetAr(DetIndx, 4))
                            VItemFpNm2 = ""
                            VItemFpNm3 = ""
                            VItemFpNm4 = ""

                            If Len(VItemFpNm1) > 20 Then
                                For K = 20 To 1 Step -1
                                    If Mid$(Trim(VItemFpNm1), K, 1) = " " Or Mid$(Trim(VItemFpNm1), K, 1) = "," Or Mid$(Trim(VItemFpNm1), K, 1) = "." Or Mid$(Trim(VItemFpNm1), K, 1) = "-" Or Mid$(Trim(VItemFpNm1), K, 1) = "/" Or Mid$(Trim(VItemFpNm1), K, 1) = "_" Or Mid$(Trim(VItemFpNm1), K, 1) = "(" Or Mid$(Trim(VItemFpNm1), K, 1) = ")" Or Mid$(Trim(VItemFpNm1), K, 1) = "\" Or Mid$(Trim(VItemFpNm1), K, 1) = "[" Or Mid$(Trim(VItemFpNm1), K, 1) = "]" Or Mid$(Trim(VItemFpNm1), K, 1) = "{" Or Mid$(Trim(VItemFpNm1), K, 1) = "}" Then Exit For
                                Next K
                                If K = 0 Then K = 20
                                VItemFpNm2 = Microsoft.VisualBasic.Right(Trim(VItemFpNm1), Len(VItemFpNm1) - K)
                                VItemFpNm1 = Microsoft.VisualBasic.Left(Trim(VItemFpNm1), K - 1)
                            End If

                            If Len(VItemFpNm2) > 20 Then
                                For K = 20 To 1 Step -1
                                    If Mid$(Trim(VItemFpNm2), K, 1) = " " Or Mid$(Trim(VItemFpNm2), K, 1) = "," Or Mid$(Trim(VItemFpNm2), K, 1) = "." Or Mid$(Trim(VItemFpNm2), K, 1) = "-" Or Mid$(Trim(VItemFpNm2), K, 1) = "/" Or Mid$(Trim(VItemFpNm2), K, 1) = "_" Or Mid$(Trim(VItemFpNm2), K, 1) = "(" Or Mid$(Trim(VItemFpNm2), K, 1) = ")" Or Mid$(Trim(VItemFpNm2), K, 1) = "\" Or Mid$(Trim(VItemFpNm2), K, 1) = "[" Or Mid$(Trim(VItemFpNm2), K, 1) = "]" Or Mid$(Trim(VItemFpNm2), K, 1) = "{" Or Mid$(Trim(VItemFpNm2), K, 1) = "}" Then Exit For
                                Next K
                                If K = 0 Then K = 20
                                VItemFpNm3 = Microsoft.VisualBasic.Right(Trim(VItemFpNm2), Len(VItemFpNm2) - K)
                                VItemFpNm2 = Microsoft.VisualBasic.Left(Trim(VItemFpNm2), K - 1)
                            End If




                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + 10, CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(DetIndx, 2)), LMargin + ClArr(1), CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(VItemGryNm1), LMargin + ClArr(1) + ClArr(2), CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(VItemFpNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY + 5, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 5), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY + 5, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY + 5, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY + 5, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY + 5, 1, 0, pFont)


                            NoofDets = NoofDets + 1
                            If Trim(prn_DetAr(DetIndx, 3)) <> "" Or Trim(VItemGryNm2) <> "" Or Trim(prn_DetAr(DetIndx, 4)) <> "" Or Trim(VItemFpNm2) <> "" Then

                                CurY = CurY + TxtHgt
                                NoofDets = NoofDets + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(VItemGryNm2), LMargin + ClArr(1) + ClArr(2), CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(VItemFpNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 0, 0, pFont)

                            End If

                            NoofDets = NoofDets + 1
                            If Trim(prn_DetAr(DetIndx, 3)) <> "" Or Trim(VItemGryNm3) <> "" Or Trim(prn_DetAr(DetIndx, 4)) <> "" Or Trim(VItemFpNm3) <> "" Then

                                CurY = CurY + TxtHgt
                                NoofDets = NoofDets + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(VItemGryNm3), LMargin + ClArr(1) + ClArr(2), CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(VItemFpNm3), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 0, 0, pFont)

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
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
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

            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "BILLED TO : ", LMargin + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "SHIPPED TO : ", LMargin + Cen1 + 60, CurY + 5, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt
            'ItmNm1 = Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

            '  Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + Cen1 + W1, CurY, 0, 0, p1Font)



            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width



            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_BillMaking_No").ToString, LMargin + Cen1 + W1 + 50, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_BillMaking_date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 50, CurY, 0, 0, pFont)




            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)


            If Trim(prn_HdDt.Rows(0).Item("bill_no").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("bill_no").ToString, LMargin + Cen1 + W1 + 50, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Transport", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + Cen1 + W1 + 50, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)


            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + Cen1 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 50, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + Cen1 + W1, CurY, 0, 0, pFont)


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
            Common_Procedures.Print_To_PrintDocument(e, "REC NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM(GREY)", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM(FP)", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)



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
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("TOTAL_meters").ToString), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("TOTAL_weight").ToString), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("TOTAL_weight").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("TOTAL_AMOUNT").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
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


            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt - 10
                    'If Val(prn_PageNo) = 1 Then
                    p1Font = New Font("Calibri", 14, FontStyle.Bold Or FontStyle.Underline)
                    '   Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 12, FontStyle.Bold)
                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt + 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt + 5 - 3
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt + 5 - 3
                        '  Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
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
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("freight_charge").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("freight_charge").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Before Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Before Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Other_Charges").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("Other_Charges").ToString) > 0 Then
                        '    Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        'Else
                        Common_Procedures.Print_To_PrintDocument(e, "Other Charges", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Other_Charges").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

                End If
            End If

            '   vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("freight_charge").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Other_Charges").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assesable_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, p1Font)
                End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Format(Val(txt_CGST_Percentage.Text), "##########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Format(Val(txt_SGST_Percentage.Text), "##########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Format(Val(txt_IGST_Percentage.Text), "##########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt - 20
            If Val(prn_HdDt.Rows(0).Item("AddLess_AfterTax_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If Val(prn_HdDt.Rows(0).Item("AddLess_AfterTax_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "After Add  ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "After Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    End If

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_afterTax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("tds_AMount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Dim VTdsAmnt As String

                    Common_Procedures.Print_To_PrintDocument(e, "TDS @ " & Format(Val(txt_tds.Text), "##########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                    VTdsAmnt = Format(Val(prn_HdDt.Rows(0).Item("tds_AMount").ToString), "########0")

                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(VTdsAmnt)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("tds_AMount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If
            End If


            'If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
            '    End If
            'End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Dim VAmount As String
                VAmount = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0")
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(VAmount)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, p1Font)
                'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))

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

            ''=============GST SUMMARY============
            'vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            'If vNoofHsnCodes <> 0 Then
            '    Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            'End If
            '==========================

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1318" Then  '--- Pranav Plastic
            '    p1Font = New Font("Calibri", 9.5, FontStyle.Regular)
            ' CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "We Declare that this Invoice Shows the actual price of the goods described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, p1Font)
            '    CurY = CurY + TxtHgt
            'End If
            '   p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            '  Common_Procedures.Print_To_PrintDocument(e, "Declaration: ", LMargin + 5, CurY, 0, 0, p1Font)
            '  CurY = CurY + TxtHgt
            '  p1Font = New Font("Calibri", 10, FontStyle.Regular)
            '  Common_Procedures.Print_To_PrintDocument(e, "We Declare that this Invoice Shows the actual price of the goods described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, p1Font)

            ' CurY = CurY + TxtHgt
            '  e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            ' LnAr(11) = CurY
            '   CurY = CurY + TxtHgt - 8
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the particulars given above are true and correct", PageWidth - 15, CurY, 1, 0, p1Font)
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            '  Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions: ", LMargin + 5, CurY, 0, 0, p1Font)
            '  Common_Procedures.Print_To_PrintDocument(e, "", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt - 2

            'Common_Procedures.Print_To_PrintDocument(e, "above are true and correct", PageWidth - 15, CurY, 1, 0, p1Font)
            'CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)



            p1Font = New Font("Calibri", 8, FontStyle.Regular)
            '  Common_Procedures.Print_To_PrintDocument(e, "Our Responsibility Cases after the goods have been delivered", LMargin + 5, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            '  Common_Procedures.Print_To_PrintDocument(e, "to the carriers. No claims for breakage or shortage during transit", LMargin + 5, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "enterinaed. Interest 21% will be changed on amount not paid within", LMargin + 5, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Jurs = StrConv(Common_Procedures.settings.Jurisdiction, vbProperCase)
            If Trim(Jurs) = "" Then Jurs = "Tirupur"
            '   Common_Procedures.Print_To_PrintDocument(e, "30days from the date of invoice.Subject to " & Jurs & " Jurisdiction.", LMargin + 5, CurY, 0, 0, p1Font)
            ' CurY = CurY + TxtHgt - 12
            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)

            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            '  e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, LnAr(11))
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


    Private Sub Cbo_RateFor_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_RateFor.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub Cbo_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_RateFor.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_RateFor, cbo_VehicleNo, cbo_TaxType, "", "", "", "")

    End Sub

    Private Sub Cbo_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_RateFor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_RateFor, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(Cbo_RateFor.Text) = "" Then
                MessageBox.Show("Invalid Rate For ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If Cbo_RateFor.Enabled And Cbo_RateFor.Visible Then Cbo_RateFor.Focus()
                Exit Sub

            Else

                cbo_TaxType.Focus()
            End If

        End If

    End Sub

    Private Sub Cbo_RateFor_TextChanged(sender As Object, e As EventArgs) Handles Cbo_RateFor.TextChanged

        Amount_Calculation()
    End Sub
End Class
