Public Class Yarn_Conversion_Bill

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GJCBL-" ' "CNINV-"
    Private Pk_Condition2 As String = "JINCN-" '"INVCN-"
    Private NoFo_STS As Integer = 0
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
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

        cbo_EntType.Text = "DIRECT"

        dtp_Date.Text = ""
        dtp_DesDate.Text = ""
        cbo_PartyName.Text = ""

        cbo_SalesAc.Text = ""
        cbo_CountName.Text = ""
        cbo_Count.Text = ""
        cbo_Filter_Count.Text = ""
        cbo_BagKg.Text = "BAG"
        cbo_Agent.Text = ""
        cbo_Vechile.Text = ""
        cbo_Conetype.Text = ""
        txt_InvoiceBag.Text = ""
        txt_InvWgt.Text = ""
        txt_Description.Text = ""
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
        cbo_TaxType.Text = ""
        lbl_Grid_HsnCode.Text = ""
        txt_Freight.Text = ""
        txt_AddLess.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        txt_TotalChippam.Text = ""
        txt_DesTime.Text = ""
        txt_rate.Text = ""
        cbo_DeliveryTo.Text = ""
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

            da1 = New SqlClient.SqlDataAdapter("select a.* from Jobwork_Yarn_Conversion_Bill_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvNo.Text = dt1.Rows(0).Item("Jobwork_Yarn_Conversion_Bill_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Jobwork_Yarn_Conversion_Bill_date").ToString
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_CountName.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Count_IdNo").ToString))
                cbo_Count.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Des_Count_IdNo").ToString))
                cbo_Conetype.Text = Common_Procedures.Conetype_IdNoToName(con, Val(dt1.Rows(0).Item("Cone_Type_Idno").ToString))
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
                ' lbl_Totalbags.Text = dt1.Rows(0).Item("Invoice_Bags").ToString
                lbl_ReceiptCode.Text = dt1.Rows(0).Item("Cotton_Delivery_Code").ToString

                txt_ClthDetail_Name.Text = dt1.Rows(0).Item("Yarn_Details").ToString
                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString

                cbo_DeliveryTo.Text = Common_Procedures.Despatch_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))


                lbl_grid_GstPerc.Text = dt1.Rows(0).Item("GST_Percentage").ToString
                lbl_Grid_HsnCode.Text = dt1.Rows(0).Item("HSN_Code").ToString



                da2 = New SqlClient.SqlDataAdapter("Select a.* from Jobwork_Yarn_Conversion_Bill_Details a  Where a.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
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

            End If

            Grid_Cell_DeSelect()
            If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()

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
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DELIVERY ADDRESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Conetype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CONETYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Conetype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Cotton_Invoice_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Cotton_Invoice_HelpRequested(ByVal sender As Object, ByVal hlpevent As System.Windows.Forms.HelpEventArgs) Handles Me.HelpRequested

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ClthDetail_Name.GotFocus, AddressOf ControlGotFocus
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
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelAddress1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryAddress.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BagNoSelection.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BagKg.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntType.LostFocus, AddressOf ControlLostFocus

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
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelAddress1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryAddress.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BagNoSelection.LostFocus, AddressOf ControlLostFocus

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
        ' AddHandler txt_BaleNos.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DeliveryAddress.KeyDown, AddressOf TextBoxControlKeyDown
        '   AddHandler txt_ClthDetail_Name.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvoiceBag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvWgt.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Description.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommBag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BaleNos.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalChippam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_DesDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DeliveryAddress.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DesTime.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_BaleNos.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvoicePrefixNo.KeyPress, AddressOf TextBoxControlKeyPress


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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        ' Print_PDF_Status = False
        Print_record()
    End Sub

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim CmpName As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Jobwork_Yarn_Conversion_Bill_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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
                    PrintDocument1.Print()

                Else
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as Agent_name ,SH.* ,Lsh.State_Name as Ledger_State_Name ,Lsh.State_Code as Ledger_State_Code from Jobwork_Yarn_Conversion_Bill_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.Ledger_State_Idno = Lsh.State_IDno INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo LEFT OUTER JOIN State_Head SH ON c.Company_State_IdNo = SH.State_Idno LEFT OUTER JOIN Ledger_Head D ON a.Agent_IdNo = d.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            'prn_HdDt = New DataTable
            'da1.Fill(prn_HdDt)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, C.*, c.area_idno as Ledger_AreaIdNo, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, SDAH.Party_Name as DeliveryTo_LedgerName, SDAH.Address1 as DeliveryTo_LedgerAddress1, SDAH.Address2 as DeliveryTo_LedgerAddress2, SDAH.Address3 as DeliveryTo_LedgerAddress3, SDAH.Address4 as DeliveryTo_LedgerAddress4, SDAH.Gstin_No as DeliveryTo_LedgerGSTinNo, SDAH.Phone_No as DeliveryTo_LedgerPhoneNo, ' ' as DeliveryTo_PanNo, SDAH.Area_IdNo as PlaceOF_AreaIdNo, SDAST.State_Name as DeliveryTo_State_Name, SDAST.State_Code as DeliveryTo_State_Code from Jobwork_Yarn_Conversion_Bill_Head a " &
                                          "INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo " &
                                          "LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo " &
                                          "INNER JOIN Ledger_Head c ON  a.Ledger_IdNo = c.Ledger_IdNo " &
                                          " LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo  " &
                                          "Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo " &
                                          "Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo " &
                                          " LEFT OUTER JOIN Sales_DeliveryAddress_Head SDAH ON SDAH.Party_IdNo = a.DeliveryTo_IdNo " &
                                          " LEFT OUTER JOIN State_Head SDAST ON SDAST.State_IdNo = SDAH.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)

            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, C.*, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, f.Ledger_Name as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo, f.Area_IdNo as PlaceOF_AreaIdNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code, SDAH.*, LSH.State_name as Party_statename, LSH.State_Code as Party_StateCode from Jobwork_Yarn_Conversion_Bill_Head a " & _
            '                              "INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo " & _
            '                              "LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo " & _
            '                              "INNER JOIN Ledger_Head c ON  a.Ledger_IdNo = c.Ledger_IdNo " & _
            '                              " LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo  " & _
            '                              "Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo " & _
            '                              "Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo " & _
            '                              "LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo  " & _
            '                              " LEFT OUTER JOIN Sales_DeliveryAddress_Head SDAH ON SDAH.Party_IdNo = a.DeliveryTo_IdNo " & _
            '                              " LEFT OUTER JOIN Sales_DeliveryAddress_Head SDA ON SDA.State_IdNo = Lsh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)


            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, C.Count_Description as Des_count_Name, c.Count_Name from Jobwork_Yarn_Conversion_Bill_Head a INNER JOIN Count_Head b on a.Des_Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Count_Head c on a.Count_idno = c.Count_idno  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Jobwork_Yarn_Conversion_Bill_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da2.Dispose()

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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Then 'Kalaimagal Textile (Palladam)
            Printing_Format2(e)
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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

            'cmd.CommandText = "Delete from Stock_Jobwork_Cotton_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_HankYarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "update Cotton_Order_Details set Invoice_Weight = a.Invoice_Weight - b.Invoice_Weight, Invoice_bags = a.Invoice_bags - b.Invoice_Bags from Cotton_Order_Details a, Jobwork_Yarn_Conversion_Bill_Head b where b.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Cotton_Order_Code = b.Cotton_Order_Code and a.Cotton_Order_Details_Slno = b.Cotton_Order_Details_Slno"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Cotton_Delivery_Head set Jobwork_Yarn_Conversion_Bill_Code = '' Where Jobwork_Yarn_Conversion_Bill_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Cotton_Packing_Details set Jobwork_Yarn_Conversion_Bill_Code = '',Cotton_Invoice_Increment = Cotton_Invoice_Increment - 1  Where Jobwork_Yarn_Conversion_Bill_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Jobwork_Yarn_Conversion_Bill_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Jobwork_Yarn_Conversion_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select top 1 Jobwork_Yarn_Conversion_Bill_No from Jobwork_Yarn_Conversion_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby, Jobwork_Yarn_Conversion_Bill_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Jobwork_Yarn_Conversion_Bill_No from Jobwork_Yarn_Conversion_Bill_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby, Jobwork_Yarn_Conversion_Bill_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Jobwork_Yarn_Conversion_Bill_No from Jobwork_Yarn_Conversion_Bill_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby desc, Jobwork_Yarn_Conversion_Bill_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Jobwork_Yarn_Conversion_Bill_No from Jobwork_Yarn_Conversion_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby desc, Jobwork_Yarn_Conversion_Bill_No desc", con)
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

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Then '----------- sathuragiri
                lbl_InvNo.Text = Common_Procedures.get_CotConv_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            Else
                lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Jobwork_Yarn_Conversion_Bill_Head", "Jobwork_Yarn_Conversion_Bill_Code", "For_OrderBy", "Entry_VAT_GST_Type ='GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            End If

            lbl_InvNo.ForeColor = Color.Red

            ' If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

            Da1 = New SqlClient.SqlDataAdapter("select top 1 a.* from Jobwork_Yarn_Conversion_Bill_Head a where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Jobwork_Yarn_Conversion_Bill_No desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                If Dt1.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then txt_InvoicePrefixNo.Text = Dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                If Dt1.Rows(0).Item("Entry_Type").ToString <> "" Then cbo_EntType.Text = Dt1.Rows(0).Item("Entry_Type").ToString
                If Dt1.Rows(0).Item("SalesAc_IdNo").ToString <> "" Then cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                If Dt1.Rows(0).Item("Discount_Percentage").ToString <> "" Then txt_DiscPerc.Text = Val(Dt1.Rows(0).Item("Discount_Percentage").ToString)
                If Dt1.Rows(0).Item("Vat_Type").ToString <> "" Then cbo_TaxType.Text = Dt1.Rows(0).Item("Vat_Type").ToString

            End If

            Dt1.Clear()

            If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim nCotmovno As String, inpno As String
        Dim nCotInvCode As String = ""
        Dim nJbwrkInvCode As String = ""
        Dim nJbwrkMovno As String = ""

        Try

            inpno = InputBox("Enter Inv No.", "FOR FINDING...")

            nJbwrkInvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Jobwork_Yarn_Conversion_Bill_No from Jobwork_Yarn_Conversion_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(nJbwrkInvCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            nJbwrkMovno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    nJbwrkMovno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            nCotmovno = ""
            If Trim(Common_Procedures.settings.CustomerCode) = "1262" Then '--------sathuragiri
                nCotInvCode = "GCNIN-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)
                Da = New SqlClient.SqlDataAdapter("select Cotton_Invoice_No from Cotton_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code = '" & Trim(nCotInvCode) & "'", con)
                Dt = New DataTable
                Da.Fill(Dt)

                nCotmovno = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        nCotmovno = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If
                Dt.Clear()
            End If

            If Val(nJbwrkMovno) <> 0 Then
                move_record(nJbwrkMovno)

            ElseIf Val(nCotmovno) <> 0 Then
                MessageBox.Show("This Invoice No. is in Cotton Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show("Inv No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim njbwrkmovno As String, inpno As String
        Dim njbwrkInvCode As String = ""
        Dim nCotMovno As String = ""
        Dim nCotInvCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Inv No.", "FOR NEW INV NO. INSERTION...")

            njbwrkInvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Jobwork_Yarn_Conversion_Bill_No from Jobwork_Yarn_Conversion_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(njbwrkInvCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            njbwrkmovno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    njbwrkmovno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()




            nCotMovno = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Then '---- Sathuragiri

                nCotInvCode = "GCNIN-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Cotton_Invoice_No from Cotton_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Invoice_Code = '" & Trim(nCotInvCode) & "'", con)
                Dt = New DataTable
                Da.Fill(Dt)

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        nCotMovno = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If
                Dt.Clear()

            End If

            If Val(njbwrkmovno) <> 0 Then
                move_record(njbwrkmovno)

            ElseIf Val(nCotMovno) <> 0 Then

                MessageBox.Show("Already this Invoice No. in Cotton Invoice", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid INV No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvNo.Text = Trim(UCase(inpno))

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
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

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

        If (Trim(UCase(cbo_EntType.Text)) <> "DIRECT" And Trim(UCase(cbo_EntType.Text)) <> "PACKING" And Trim(UCase(cbo_EntType.Text)) <> "ORDER" And Trim(UCase(cbo_EntType.Text)) <> "DELIVERY") Then
            MessageBox.Show("Invalid Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        If Val(Cnt_ID) = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        DesCnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)
        'If Val(DesCnt_ID) = 0 Then
        '    MessageBox.Show("Invalid  Description Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()
        '    Exit Sub
        'End If

        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        Col_ID = Common_Procedures.ConeType_NameToIdNo(con, cbo_Conetype.Text)

        SalesAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        vDelvTo_IdNo = Common_Procedures.Despatch_NameToIdNo(con, cbo_DeliveryTo.Text)

        If vDelvTo_IdNo = 0 Then

            cbo_DeliveryTo.Text = cbo_PartyName.Text
            vDelvTo_IdNo = Common_Procedures.Despatch_NameToIdNo(con, cbo_DeliveryTo.Text)
        End If

        If SalesAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()
            Exit Sub
        End If
        'If Val(txt_TotalChippam.Text) = 0 Then
        '    MessageBox.Show("Invalid Chippam", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_TotalChippam.Enabled And txt_TotalChippam.Visible Then txt_TotalChippam.Focus()
        '    Exit Sub
        'End If
        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    'If Trim(.Rows(i).Cells(1).Value) = "" Then
                    '    MessageBox.Show("Invalid BagNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(1)
                    '    End If
                    '    Exit Sub
                    'End If


                    If Val(.Rows(i).Cells(2).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        '    MessageBox.Show("Invalid Tax A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_TaxType.Enabled And cbo_TaxType.Visible Then cbo_TaxType.Focus()
        '    Exit Sub
        'End If

        If (Trim(cbo_TaxType.Text) = "" Or Trim(cbo_TaxType.Text) = "-NIL-") Then
            MessageBox.Show("Invalid Tax Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_TaxType.Enabled And cbo_TaxType.Visible Then cbo_TaxType.Focus()
            Exit Sub
        End If
        'NoFo_STS = 0
        'If chk_Less_Comm.Checked = True Then NoFo_STS = 1

        NoCalc_Status = False
        Total_Calculation()

        vTotBgsNo = 0 : vTotWgt = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBgsNo = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(2).Value())

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1262" Then      '----- sathuragiri
                    lbl_InvNo.Text = Common_Procedures.get_CotConv_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                Else
                    lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Jobwork_Yarn_Conversion_Bill_Head", "Jobwork_Yarn_Conversion_Bill_Code", "For_OrderBy", "Entry_VAT_GST_Type ='GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@InvDate", dtp_Date.Value.Date)
            cmd.Parameters.AddWithValue("@DesDate", dtp_DesDate.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Jobwork_Yarn_Conversion_Bill_Head ( Entry_VAT_GST_Type ,      Jobwork_Yarn_Conversion_Bill_Code                   ,               Company_IdNo       ,           Jobwork_Yarn_Conversion_Bill_No    ,                               for_OrderBy                             , Jobwork_Yarn_Conversion_Bill_date ,         Ledger_IdNo      ,   Count_IdNo            ,     Cone_Type_Idno         ,         SalesAc_IdNo    ,    Des_Count_IdNo           ,   Agent_IdNo             ,              Description              ,           Com_Bag                   ,           Invoice_Bags          ,      Invoice_Weight            ,                  Rate       ,    Amount                        ,  Discount_Percentage                ,              Discount_Amount         ,                      Freight_Amount          ,              AddLess_Amount       ,               RoundOff_Amount       ,                  Net_Amount               ,   Total_Bags          ,        Total_Weight     ,  Vechile_No                        ,   Total_Chippam                     ,       Des_Date         , Des_Time_Text                     ,                     Dc_No     ,                 Bale_Nos         ,     Delivery_Address                    ,                Delivery_Address1    ,            Cotton_Order_Code      ,    Cotton_Order_details_SlNo         ,                Commission_Type ,                    Entry_Type    ,Yarn_Details                              ,               Invoice_PrefixNo                   , Cotton_Delivery_Code               ,   CGST_Amount                          , SGST_Amount                               , IGST_Amount                    , Taxable_Amount                        ,   GST_Percentage                    ,                    HSN_Code                ,       DeliveryTo_IdNo     ,Vat_Type        ) " &
                                    "     Values                  (   'GST'             ,'" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",      @InvDate       , " & Str(Val(Led_ID)) & " , " & Str(Val(Cnt_ID)) & " , " & Str(Val(Col_ID)) & " ,  " & Val(SalesAc_ID) & ", " & Str(Val(DesCnt_ID)) & ", " & Str(Val(Agt_Idno)) & ",   '" & Trim(txt_Description.Text) & "',   " & Str(Val(txt_CommBag.Text)) & ", " & Val(txt_InvoiceBag.Text) & ",   " & Val(txt_InvWgt.Text) & " ,  " & Val(txt_rate.Text) & " , " & Str(Val(lbl_Amount.Text)) & ",  " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ",  " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & " , " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Val(vTotBgsNo) & "," & Str(Val(vTotWgt)) & ",   '" & Trim(cbo_Vechile.Text) & "'  ," & Val(txt_TotalChippam.Text) & "  , @DesDate               , '" & Trim(txt_DesTime.Text) & "'  , '" & Trim(txt_DcNo.Text) & "' , '" & Trim(txt_BaleNos.Text) & "' , '" & Trim(txt_DeliveryAddress.Text) & "', '" & Trim(txt_DelAddress1.Text) & "', '" & Trim(lbl_OrderCode.Text) & "', " & Val(lbl_OrderDetailSlNo.Text) & ", '" & Trim(cbo_BagKg.Text) & "' , '" & Trim(cbo_EntType.Text) & "' , '" & Trim(txt_ClthDetail_Name.Text) & "' ,  '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' ,'" & Trim(lbl_ReceiptCode.Text) & "', " & Str(Val(lbl_CGstAmount.Text)) & " , " & Str(Val(lbl_SGstAmount.Text)) & "  ," & Str(Val(lbl_IGstAmount.Text)) & " ," & Str(Val(lbl_Assessable.Text)) & "," & Str(Val(lbl_grid_GstPerc.Text)) & ",'" & Trim(lbl_Grid_HsnCode.Text) & "     '," & Str(Val(vDelvTo_IdNo)) & ",'" & Trim(cbo_TaxType.Text) & "') "
                cmd.ExecuteNonQuery()

                Nr = 0
                'cmd.CommandText = "Update Cotton_Order_Details set Invoice_Weight = Invoice_Weight + " & Str(Val(txt_InvWgt.Text)) & ", Invoice_bags = Invoice_Bags+  " & Val(lbl_Totalbags.Text) & "  Where Cotton_Order_code = '" & Trim(lbl_OrderCode.Text) & "' and Cotton_Order_Details_Slno = " & Str(Val(lbl_OrderDetailSlNo.Text)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                'Nr = cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Jobwork_Yarn_Conversion_Bill_Head set  Entry_VAT_GST_Type = 'GST', Jobwork_Yarn_Conversion_Bill_date = @InvDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ",  Entry_Type = '" & Trim(cbo_EntType.Text) & "', Cone_Type_Idno = " & Str(Val(Col_ID)) & ", Count_IdNo = " & Str(Val(Cnt_ID)) & ",SalesAc_IdNo = " & Str(Val(SalesAc_ID)) & ", Agent_IdNo = " & Str(Val(Agt_Idno)) & ", Des_Count_idNo = " & Val(DesCnt_ID) & ",   Com_Bag =  " & Str(Val(txt_CommBag.Text)) & " ,      Invoice_Bags  = " & Val(txt_InvoiceBag.Text) & " ,   Invoice_Weight  =  " & Val(txt_InvWgt.Text) & " ,  Rate =  " & Val(txt_rate.Text) & ",Amount = " & Str(Val(lbl_Amount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ",  Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", Vechile_No = '" & Trim(cbo_Vechile.Text) & "' , AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Total_Bags = " & Val(vTotBgsNo) & ",Total_Weight  = " & Str(Val(vTotWgt)) & ", Total_Chippam =  " & Str(Val(txt_TotalChippam.Text)) & " ,      Des_Date  = @DesDate ,   Des_Time_Text  =  '" & Trim(txt_DesTime.Text) & "' , Dc_No = '" & Trim(txt_DcNo.Text) & "' , Bale_Nos = '" & Trim(txt_BaleNos.Text) & "' ,Cotton_Delivery_Code ='" & Trim(lbl_ReceiptCode.Text) & "' ,   Invoice_PrefixNo = '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' , Yarn_Details =  '" & Trim(txt_ClthDetail_Name.Text) & "', Delivery_Address =  '" & Trim(txt_DeliveryAddress.Text) & "' ,Delivery_Address1 = '" & Trim(txt_DelAddress1.Text) & "', Cotton_order_Code=  '" & Trim(lbl_OrderCode.Text) & "' , Cotton_Order_details_SlNo =  " & Val(lbl_OrderDetailSlNo.Text) & " ,Commission_Type = '" & Trim(cbo_BagKg.Text) & "', CGST_Amount = " & Val(lbl_CGstAmount.Text) & " ,SGST_Amount = " & Val(lbl_SGstAmount.Text) & " , IGST_Amount = " & Val(lbl_IGstAmount.Text) & " , Taxable_Amount = " & Val(lbl_Assessable.Text) & ",GST_Percentage=" & Str(Val(lbl_grid_GstPerc.Text)) & " ,HSN_Code=' " & Trim(lbl_Grid_HsnCode.Text) & "', DeliveryTo_IdNo = " & Str(Val(vDelvTo_IdNo)) & ",Vat_Type = '" & Trim(cbo_TaxType.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "update Cotton_Order_Details set Invoice_Weight = a.Invoice_Weight - b.Invoice_Weight, Invoice_bags = a.Invoice_bags - b.Invoice_Bags from Cotton_Order_Details a, Jobwork_Yarn_Conversion_Bill_Head b where b.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Cotton_Order_Code = b.Cotton_Order_Code and a.Cotton_Order_Details_Slno = b.Cotton_Order_Details_Slno"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Cotton_Packing_Details set Jobwork_Yarn_Conversion_Bill_Code = '', Cotton_Invoice_Increment = Cotton_Invoice_Increment - 1  Where Jobwork_Yarn_Conversion_Bill_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()
                'cmd.CommandText = "Update Cotton_Packing_Details set Jobwork_Yarn_Conversion_Bill_Code = ''  Where Jobwork_Yarn_Conversion_Bill_Code =  '" & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()
                'cmd.CommandText = "Update Cotton_Delivery_Head set Jobwork_Yarn_Conversion_Bill_Code = '' Where Jobwork_Yarn_Conversion_Bill_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()

            End If

            Partcls = Trim((cbo_PartyName.Text))
            PBlNo = Trim(lbl_InvNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_InvNo.Text)

            cmd.CommandText = "Delete from Jobwork_Yarn_Conversion_Bill_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Stock_HankYarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Stock_Jobwork_Cotton_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" And Trim(lbl_ReceiptCode.Text) <> "" Then
            '    cmd.CommandText = "Update Cotton_Delivery_Head set Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' where Cotton_Delivery_Code = '" & Trim(lbl_ReceiptCode.Text) & "' and Ledger_IdNo = " & Str(Val(Led_ID))
            '    Nr = cmd.ExecuteNonQuery()
            '    If Nr = 0 Then
            '        Throw New ApplicationException("Mismatch of Party & Delivery Details")
            '        'tr.Rollback()
            '        'MessageBox.Show("Mismatch of Party & Receipt Details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '        Exit Sub
            '    End If
            'End If

            With dgv_Details

                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Sno = Sno + 1


                        stk_ID = 0
                        stk_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(7).Value), tr)


                        cmd.CommandText = "Insert into Jobwork_Yarn_Conversion_Bill_Details ( Jobwork_Yarn_Conversion_Bill_Code ,               Company_IdNo       ,   Jobwork_Yarn_Conversion_Bill_No    ,                     for_OrderBy                                            ,              Jobwork_Yarn_Conversion_Bill_date,             Sl_No     ,                                    Bag_No            ,                Weight                     ,Bag_Code                   , Cotton_Packing_Code                  ,    Cotton_Delivery_Code           ,     Cotton_Delivery_Details_Slno   , StockfROM_IdNo  ) " &
                                            "     Values                 (   '" & Trim(Pk_Condition) & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",       @InvDate            ,  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ",  '" & Trim(.Rows(i).Cells(3).Value) & "', '" & Trim(.Rows(i).Cells(4).Value) & "',  '" & Trim(.Rows(i).Cells(5).Value) & "' , " & Val(.Rows(i).Cells(6).Value) & " , " & Val(stk_ID) & " ) "
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(cbo_EntType.Text)) = "PACKING" Then
                            Nr = 0
                            cmd.CommandText = "Update Cotton_Packing_Details set Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' , Cotton_Invoice_Increment = Cotton_Invoice_Increment + 1 Where Bag_Code = '" & Trim(.Rows(i).Cells(3).Value) & "' AND  Cotton_packing_Code= '" & Trim(.Rows(i).Cells(4).Value) & "' and Count_IdNo  =  " & Str(Val(Cnt_ID)) & " and Cone_Type_Idno =  " & Str(Val(Col_ID)) & ""
                            Nr = cmd.ExecuteNonQuery()
                        End If


                        'If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
                        '    Nr = 0
                        '    cmd.CommandText = "Update Cotton_Packing_Details set Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' , Cotton_Invoice_Increment = Cotton_Invoice_Increment + 1 Where Bag_Code = '" & Trim(.Rows(i).Cells(3).Value) & "' AND  Cotton_packing_Code= '" & Trim(.Rows(i).Cells(4).Value) & "' and Count_IdNo  =  " & Str(Val(Cnt_ID)) & " and Cone_Type_Idno =  " & Str(Val(Col_ID)) & ""
                        '    Nr = cmd.ExecuteNonQuery()
                        'End If
                        'If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then

                        '    Nr = 0
                        '    cmd.CommandText = "Update Cotton_dELIVERY_Head set Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Where Cotton_Delivery_Code = '" & Trim(.Rows(i).Cells(5).Value) & "' AND  Cotton_Delivery_Details_Slno = " & Val(.Rows(i).Cells(6).Value) & "  "
                        '    Nr = cmd.ExecuteNonQuery()
                        'End If
                        ' If Trim(UCase(cbo_EntType.Text)) <> "DELIVERY" Then



                        'End If

                    End If

                Next

            End With
            'cmd.CommandText = "Insert into Stock_Jobwork_Cotton_Processing_Details ( Reference_Code                        ,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,     Party_Bill_No   ,  Entry_ID         ,Particulars        ,   Sl_No      ,     DeliveryTo_Idno         ,     ReceivedFrom_Idno   ,       Count_IdNo    ,       Cone_Type_Idno      ,           Chippam               ,      Bale                    ,   Weight                                 ) " &
            '"   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",    @InvDate   , '" & Trim(PBlNo) & "', '" & Trim(EntID) & "',   '" & Trim(Partcls) & "' ,   " & Str(Val(Sno)) & ", " & Str(Val(Led_ID)) & ",   0                     , " & Str(Val(Cnt_ID)) & "," & Str(Val(Col_ID)) & "   ," & (-1 * Val(txt_TotalChippam.Text)) & ", " & -1 * Val(vTotBgsNo) & "," & Str(-1 * Val(txt_InvWgt.Text)) & " )"
            'cmd.ExecuteNonQuery()

            'If Trim(UCase(cbo_EntType.Text)) <> "DELIVERY" Then

            '    EntID = Trim(Pk_Condition) & Trim(lbl_InvNo.Text)
            '    PBlNo = Trim(lbl_InvNo.Text)
            '    Partcls = Trim(cbo_PartyName.Text)

            '    Da = New SqlClient.SqlDataAdapter("select count(Bag_No) as bags ,sum(Weight) as wgt , StockfROM_IdNo from Jobwork_Yarn_Conversion_Bill_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' group by StockfROM_IdNo ", con)
            '    Dt1 = New DataTable
            '    Da.SelectCommand.Transaction = tr
            '    Da.Fill(Dt1)

            '    Sno = 0

            '    If Dt1.Rows.Count > 0 Then
            '        For I = 0 To Dt1.Rows.Count - 1
            '            Sno = Sno + 1
            '            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code                        ,             Company_IdNo                 ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,  Particulars ,    Party_Bill_No   ,      Entry_ID      ,     Sl_No      , Count_idNo      ,        Cone_Type_Idno            ,       Bags                                               ,         Weight                                         ,                   StockAt_IdNo   ) " & _
            '                                                           "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",    @InvDate   , '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', '" & Trim(EntID) & "' ," & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & "," & Str(Val(Col_ID)) & ", " & Str(-1 * Val(Dt1.Rows(I).Item("bags").ToString)) & "  ," & Str(-1 * Val(Dt1.Rows(I).Item("wgt").ToString)) & " ," & Str(Val(Dt1.Rows(I).Item("StockfROM_IdNo").ToString)) & " )"
            '            cmd.ExecuteNonQuery()
            '        Next I
            '    End If
            '    Dt1.Clear()

            'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code                        ,             Company_IdNo                 ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,   Particulars ,   Party_Bill_No   ,   Entry_ID          ,             Sl_No      ,            Count_idNo      ,        Cone_Type_Idno  ,  Bags              ,         Weight                                 ) " & _
            '                                                     "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",    @InvDate   , '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "','" & Trim(EntID) & "',  " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & "," & Str(Val(Col_ID)) & ", " & Str(-1 * Val(vTotBgsNo)) & "  ," & Str(-1 * Val(vTotWgt)) & " )"
            'cmd.ExecuteNonQuery()

            'End If

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            If Trim(UCase(cbo_BagKg.Text)) = "BAG" Then

                ComAmt = Val(vTotBgsNo) * Val(txt_CommBag.Text)

            Else
                ComAmt = Val(txt_InvWgt.Text) * Val(txt_CommBag.Text)

            End If


            If Val(Agt_Idno) <> 0 Then

                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date,      Ledger_IdNo    ,           Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,             Amount                         ,   Commission_Amount       ,Commission_Type               ,  Weight                       ,Commission_For ,   NoOfBags          ,  Commission_Rate) " &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",   @InvDate  , " & Str(Led_ID) & ", " & Str(Val(Agt_Idno)) & "   , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  " & Str(Val(CSng(lbl_NetAmount.Text))) & ",   " & Str(Val(ComAmt)) & ",'" & Trim(cbo_BagKg.Text) & "', " & Val(txt_InvWgt.Text) & ",'YARN'         , " & Val(vTotBgsNo) & "," & Val(txt_CommBag.Text) & ") "
                cmd.ExecuteNonQuery()

            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
                vLed_IdNos = Led_ID & "|" & SalesAc_ID & "|24|25|26"
                vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(CSng(lbl_CGstAmount.Text)) - Val(CSng(lbl_SGstAmount.Text)) - Val(CSng(lbl_IGstAmount.Text))) & "|" & Val(CSng(lbl_CGstAmount.Text)) & "|" & Val(CSng(lbl_SGstAmount.Text)) & "|" & Val(CSng(lbl_IGstAmount.Text))
                If Common_Procedures.Voucher_Updation(con, "Yarn.Inv", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_InvNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_InvNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If

            vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
            vVou_Amts = Val(ComAmt) & "|" & -1 * Val(ComAmt)
            If Common_Procedures.Voucher_Updation(con, "AgComm.GInv", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_InvNo.Text), dtp_Date.Value.Date, "Inv No : " & Trim(lbl_InvNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(lbl_InvNo.Text), Agt_Idno, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()
            move_record(lbl_InvNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
                Condt = "a.Jobwork_Yarn_Conversion_Bill_date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Jobwork_Yarn_Conversion_Bill_date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Jobwork_Yarn_Conversion_Bill_date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Cone_Type_Idno = " & Str(Val(CnTy_Id)) & " "
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Count_IdNo = " & Str(Val(Cnt_IdNo)) & " "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name , d.Cone_Type_Name ,e.Count_Name  from Jobwork_Yarn_Conversion_Bill_Head a INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cone_Type_Head d ON a.Cone_Type_Idno = d.Cone_Type_Idno  LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jobwork_Yarn_Conversion_Bill_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Jobwork_Yarn_Conversion_Bill_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from Jobwork_Yarn_Conversion_Bill_Head a INNER JOIN Jobwork_Yarn_Conversion_Bill_Details b ON a.Jobwork_Yarn_Conversion_Bill_Code = b.Jobwork_Yarn_Conversion_Bill_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jobwork_Yarn_Conversion_Bill_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Jobwork_Yarn_Conversion_Bill_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Jobwork_Yarn_Conversion_Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Jobwork_Yarn_Conversion_Bill_date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cone_Type_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Invoice_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")
                    ' dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_NetWeight").ToString), "########0.000")

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
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        dgv_Details.CurrentCell.Selected = False
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

    Private Sub txt_AddLess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess.KeyDown
        If e.KeyValue = 40 Then
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then


                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
            Else
                cbo_Vechile.Focus()
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
                cbo_Vechile.Focus()
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


        If NoCalc_Status = True Then Exit Sub
        lbl_Amount.Text = Format(Val(txt_InvWgt.Text) * Val(txt_rate.Text), "########0.00")

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

            ' lbl_Grid_HsnCode.Text = ""
            lbl_Grid_HsnCode.Text = "998821"

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


        BlAmt = Val(lbl_Assessable.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text)

        lbl_NetAmount.Text = Format(Val(BlAmt), "##########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))
        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(BlAmt), "#########0.00")


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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_SalesAc, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

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
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_Conetype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Conetype.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")
    End Sub
    Private Sub cbo_Conetype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Conetype.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Conetype, cbo_CountName, cbo_SalesAc, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")


    End Sub

    Private Sub cbo_Conetype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Conetype.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Conetype, Nothing, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_EntType.Text) = "PACKING" Then
                If MessageBox.Show("Do you want to select Pack  :", "FOR PACKING SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Pack_Selection_Click(sender, e)
                End If
            Else
                cbo_SalesAc.Focus()
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

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Jobwork_Yarn_Conversion_Bill_Head", "Vechile_No", "", "(Vechile_No <> '')")
    End Sub
    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, txt_AddLess, txt_TotalChippam, "Jobwork_Yarn_Conversion_Bill_Head", "Vechile_No", "", "(Vechile_No <> '')")

    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, txt_TotalChippam, "Jobwork_Yarn_Conversion_Bill_Head", "Vechile_No", "", "(Vechile_No <> '')", False)

    End Sub


    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_Conetype, cbo_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, cbo_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ConeType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ConeType, cbo_Filter_Count, btn_Filter_Show, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")

    End Sub

    Private Sub cbo_Filter_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ConeType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ConeType, btn_Filter_Show, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")

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
            MessageBox.Show("Invalid Type", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

                Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Count_Name, d.Ledger_Name as agentname, d.Yarn_Comm_Bag , e.Cone_Type_Name,   h.Total_bags as Ent_Bags , h.Invoice_Weight as Ent_Invoice_Weight from Cotton_Order_Head a INNER JOIN Cotton_Order_details b ON a.Cotton_Order_Code = b.Cotton_Order_Code  LEFT OUTER JOIN Count_Head c ON b.Count_IdNo = c.Count_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Cone_Type_Head e ON b.Cone_Type_Idno = e.Cone_Type_Idno LEFT OUTER JOIN Jobwork_Yarn_Conversion_Bill_Head h ON h.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and b.Cotton_Order_Code = h.Cotton_Order_Code and b.Cotton_Order_Details_Slno = h.Cotton_Order_Details_Slno Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " b.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Weight -  b.Invoice_Weight) > 0 or h.Invoice_Weight > 0 ) order by a.Cotton_Order_Date, a.for_orderby, a.Cotton_Order_No", con)
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
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cone_Type_Name").ToString
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

                Da = New SqlClient.SqlDataAdapter("select a.*,  c.Count_Name, d.Ledger_Name as agentname, d.Yarn_Comm_Bag, e.Cone_Type_Name from Cotton_Delivery_Head a  LEFT OUTER JOIN Count_Head c ON A.Count_IdNo = c.Count_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Cone_Type_Head e ON A.Cone_Type_Idno = e.Cone_Type_Idno  Where  A.Jobwork_Yarn_Conversion_Bill_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & "  order by a.Cotton_Delivery_Date, a.for_orderby, a.Cotton_Delivery_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)


                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cotton_Delivery_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Count_Name").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cone_Type_Name").ToString
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
                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next
                End If

                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Count_Name, d.Ledger_Name as agentname, e.Cone_Type_Name, d.Yarn_Comm_Bag   from Cotton_Delivery_Head a  LEFT OUTER JOIN Count_Head c ON A.Count_IdNo = c.Count_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Cone_Type_Head e ON A.Cone_Type_Idno = e.Cone_Type_Idno  Where  A.Jobwork_Yarn_Conversion_Bill_Code =  '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & "  order by a.Cotton_Delivery_Date, a.for_orderby, a.Cotton_Delivery_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cotton_Delivery_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Count_Name").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cone_Type_Name").ToString
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
                cbo_Agent.Text = dgv_Selection.Rows(i).Cells(9).Value
                cbo_Vechile.Text = dgv_Selection.Rows(i).Cells(10).Value
                dtp_DesDate.Text = dgv_Selection.Rows(i).Cells(11).Value
                txt_DesTime.Text = dgv_Selection.Rows(i).Cells(12).Value
                txt_TotalChippam.Text = dgv_Selection.Rows(i).Cells(16).Value
                txt_BaleNos.Text = dgv_Selection.Rows(i).Cells(13).Value
                txt_DeliveryAddress.Text = dgv_Selection.Rows(i).Cells(14).Value
                txt_DelAddress1.Text = dgv_Selection.Rows(i).Cells(15).Value
                txt_CommBag.Text = dgv_Selection.Rows(i).Cells(17).Value

                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Cotton_Delivery_Details a LEFT OUTER JOIN Cotton_Packing_Details B ON b.Jobwork_Yarn_Conversion_Bill_Code = 'CNDEL-' + '" & Trim(dgv_Selection.Rows(i).Cells(7).Value) & "' and a.Bag_Code = b.Bag_Code Where a.Cotton_Delivery_Code = '" & Trim(dgv_Selection.Rows(i).Cells(7).Value) & "' and b.Cotton_Delivery_Return_Code = '' Order by a.sl_no", con)
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
                            .Rows(n).Cells(6).Value = dt2.Rows(j).Item("Jobwork_Yarn_Conversion_Bill_Code").ToString
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
        If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()

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
                MessageBox.Show("Invalid Count Name", "DOES NOT SELECT PACKING SELECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
                Exit Sub
            End If

            CnTy_IdNo = Common_Procedures.ConeType_NameToIdNo(con, cbo_Conetype.Text)

            If CnTy_IdNo = 0 Then
                MessageBox.Show("Invalid ConeType Name", "DOES NOT SELECT PACKING SELECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

                Da = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details  A LEFT OUTER JOIN Jobwork_Yarn_Conversion_Bill_Details b ON a.Cotton_Packing_Code = b.Cotton_Packing_Code and a.Bag_Code = b.Bag_Code where a.Jobwork_Yarn_Conversion_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Cone_Type_Idno = " & Str(Val(CnTy_IdNo)) & " order by  a.sl_no,a.Cotton_Packing_Date, a.for_orderby, a.Cotton_Packing_No", con)
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

                Da = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details a   where a.Jobwork_Yarn_Conversion_Bill_Code  = '' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Cone_Type_Idno = " & Str(Val(CnTy_IdNo)) & " order by  a.cotton_packing_code, a.sl_no, a.Cotton_packing_Date, a.for_orderby, a.Cotton_packing_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                'Da = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details a   where a.Jobwork_Yarn_Conversion_Bill_Code  = '' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Cone_Type_Idno = " & Str(Val(CnTy_IdNo)) & " order by a.Cotton_Packing_Date, a.for_orderby ,  a.Cotton_Packing_No ", con)
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

    Private Sub cbo_Grid_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, cbo_SalesAc, cbo_Agent, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_EntType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntType, dtp_Date, cbo_PartyName, "", "", "", "")
    End Sub

    Private Sub cbo_EntType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntType, cbo_PartyName, "", "", "", "")
    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, cbo_Agent, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
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
    Private Sub txt_ClthDetail_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ClthDetail_Name.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            cbo_SalesAc.Focus()
        End If
    End Sub

    Private Sub txt_ClthDetail_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ClthDetail_Name.KeyPress
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
            cbo_Count.Enabled = True
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
            cbo_Count.Enabled = True
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
            cbo_Count.Enabled = False
            cbo_Agent.Enabled = False
            cbo_Vechile.Enabled = False
            txt_TotalChippam.Enabled = False
            dtp_DesDate.Enabled = False
            txt_DeliveryAddress.Enabled = False
            txt_DelAddress1.Enabled = False
            txt_BaleNos.Enabled = False
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim ItmNm1 As String, ItmNm2 As String
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
            .Left = 40 ' 30 '60
            .Right = 70
            .Top = 40 ' 60
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

                            ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Yarn_Details").ToString)
                            'ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Des_Count_Name").ToString)
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

                            NoofDets = NoofDets + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(NoofDets)), LMargin + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
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

                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Jobwork_Yarn_Conversion_Bill_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Jobwork_Yarn_Conversion_Bill_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Jobwork_Yarn_Conversion_Bill_date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, pFont)

            'If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "ELECTRONIC REF.NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
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

            CurY2 = CurY2 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
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

            If Val(prn_HdDt.Rows(0).Item("Dc_no").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("Des_Time_Text").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, FormatDateTime(prn_HdDt.Rows(0).Item("Des_Time_Text").ToString), LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Des_Date").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "DATE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, FormatDateTime(prn_HdDt.Rows(0).Item("Des_Date").ToString), LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, FormatDateTime(prn_HdDt.Rows(0).Item("Des_Date").ToString) & " " & prn_HdDt.Rows(0).Item("Des_Time_Text").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
            End If

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
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            '   Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY1, 0, 0, p1Font)
            'Right Side

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Addless_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
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



            '----------------------------------------------------------------------


            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 13, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Spinning) )", LMargin + 100, CurY, 0, 0, p1Font)
            End If
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
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        NoofItems_PerPage = 4 ' 12

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35) ' S.NO
        ClArr(2) = 75       'COUNT
        ClArr(3) = 180      'DESCRIPTION OF GOODS
        ClArr(4) = 70       'HSN CODE
        ClArr(5) = 55       'GST %
        ClArr(6) = 70       'NO.OF.BAG
        ClArr(7) = 70       'TOTAL WGT
        ClArr(8) = 80       'RATE/KG
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))  'AMOUNT


        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, vLine_Pen)

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

                                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Yarn_Details").ToString)
                            'ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Des_Count_Name").ToString)
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

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    'Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets)
                    Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


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

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByRef NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
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
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CONVERSION BILL", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Then '---- Kalaimagal OE (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 20, CurY + 10, 100, 90)
        End If

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
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Jobwork_Yarn_Conversion_Bill_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Jobwork_Yarn_Conversion_Bill_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Jobwork_Yarn_Conversion_Bill_date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, pFont)

            'If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "ELECTRONIC REF.NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
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

            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


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
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF.BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'CurY = CurY + 10
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            'End If
            'p1Font = New Font("Calibri", 8, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
            CurY1 = CurY1 + TxtHgt
            CurY1 = CurY1 + TxtHgt
            CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
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

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            CurY = CurY + 5
            BmsInWrds = ""
            If (Val(prn_CGST_Amount) + Val(prn_SGST_Amount) + Val(prn_IGST_Amount)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_CGST_Amount) + Val(prn_SGST_Amount) + Val(prn_IGST_Amount))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            'Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Bank Details : " & BankNm1 & ", " & BankNm2 & ", " & BankNm3 & ", " & BankNm4, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "GOODS CLEARED UNDER EXEMPTION NOTIFICATION NO 30/2004 DT 09.07.2004 ", LMargin, CurY, 2, PageWidth, pFont)




            CurY = CurY + TxtHgt - 5


            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt + 3
            Common_Procedures.Print_To_PrintDocument(e, "We are  responsible for yarn in yarn shape only not in fabric stage", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases when goods leave our permission", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Interest at value of 24% will be charge from the due date", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "All Payment should be made by A\c Payee Cheque or Draft", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            'CurY = CurY + TxtHgt - 5
            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
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

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(vLine_Pen, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_TaxType, cbo_BagKg, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_BagKg, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Delivery_Party_Creation

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

    Private Sub txt_BaleNos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BaleNos.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to Save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()

            Else
                dtp_Date.Focus()

            End If

        End If
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
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        With dgv_Details

            pnl_Back.Enabled = True

            dgv_Details.Rows.Clear()

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

                End If
                Total_Calculation()
            Next
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

    '        da = New SqlClient.SqlDataAdapter("select DISTINCT b.Ledger_Name as agentname, b.Yarn_Comm_Bag from Jobwork_Yarn_Conversion_Bill_Head a LEFT OUTER JOIN ledger_head b ON a.Agent_IdNo = b.Ledger_IdNo where  b.ledger_Name = '" & Trim(cbo_Agent.Text) & "'", con)
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

                        MxId = Common_Procedures.get_MaxIdNo(con, "EntryTempSub", "Int1", "")

                        Cmd.CommandText = "Insert into EntryTempSub ( Int1, Name1, Name2, Name3) Values (" & Str(Val(MxId)) & ", '" & Trim(.Rows(RwIndx).Cells(8).Value) & "', '" & Trim(.Rows(RwIndx).Cells(1).Value) & "', " & Str(Val(.Rows(RwIndx).Cells(3).Value)) & " ) "
                        Cmd.ExecuteNonQuery()

                    Else

                        .Rows(RwIndx).Cells(3).Value = ""
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                        Next

                        Cmd.CommandText = "Delete from EntryTempSub where Name1 = '" & Trim(.Rows(RwIndx).Cells(8).Value) & "' and Name2 = '" & Trim(.Rows(RwIndx).Cells(3).Value) & "'"
                        Cmd.ExecuteNonQuery()

                    End If

                End If

            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

    End Sub
End Class