Public Class Invoice_Garments5_Sales_Return
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GSRG5-"          '"GSTG4-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private dgv_ActCtrlName As String = ""

    Private vcmb_ItmNm As String
    Private vcmb_SizNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private CFrm_STS As Integer
    Private prn_Status As Integer
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private InvPrintFrmt As String = ""
    Private InvPrintFrmt_Letter As Integer = 0
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_DetAr(500, 20) As String
    Private prn_DetSNo As Integer
    Private prn_TOTBOX As Integer
    Private Prn_Cnt_Temp As Integer = 0
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As Panel

        New_Entry = False
        Insert_Entry = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Print.Visible = False
        pnl_BaleSelection.Visible = False

        txt_LrDate.Text = ""
        txt_CashDiscPerc.Text = ""
        txt_CashDiscAmount.Text = ""
        txt_TradeDiscPerc.Text = ""
        txt_TradeDiscAmount.Text = ""
        lbl_NetAmount.Text = ""
        lbl_GrossAmount.Text = ""
        lbl_Assessable.Text = ""
        txt_Charge.Text = ""
        txt_OrderNo.Text = ""
        txt_DcNo.Text = ""
        txt_PreparedBy.Text = ""

        cbo_DocumentThrough.Text = ""

        Panel2.Enabled = True
        cbo_EntType.Enabled = True



        cbo_Style.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_AgentName.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""

            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""

            ElseIf TypeOf obj Is DateTimePicker Then
                obj.text = ""

            ElseIf TypeOf obj Is GroupBox Then
                grpbx = obj
                For Each ctrl1 In grpbx.Controls
                    If TypeOf ctrl1 Is TextBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is ComboBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is DateTimePicker Then
                        ctrl1.text = ""
                    End If
                Next

            ElseIf TypeOf obj Is Panel Then
                pnl1 = obj
                If Trim(UCase(pnl1.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                    For Each ctrl2 In pnl1.Controls
                        If TypeOf ctrl2 Is TextBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is ComboBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is DataGridView Then
                            ctrl2.Rows.Clear()
                        ElseIf TypeOf ctrl2 Is DateTimePicker Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is Panel Then
                            pnl2 = ctrl2
                            If Trim(UCase(pnl2.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                                For Each ctrl3 In pnl2.Controls
                                    If TypeOf ctrl3 Is TextBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is ComboBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is DateTimePicker Then
                                        ctrl3.text = ""
                                    End If
                                Next
                            End If

                        End If

                    Next

                End If

            End If

        Next

        chk_No_CForm.Checked = False
        cbo_EntType.Text = "DIRECT"

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_BaleDetails.Rows.Clear()
        dgv_BaleDetails_Total.Rows.Clear()
        dgv_BaleDetails_Total.Rows.Add()

        dgv_Packing_Selection.Rows.Clear()

        '***** GST START *****
        pnl_GSTTax_Details.Visible = False
        '***** GST END *****

        '***** GST START *****
        lbl_Grid_TradeDiscPerc.Text = ""
        lbl_Grid_TradeDiscAmount.Text = ""
        lbl_Grid_CashDiscPerc.Text = ""
        lbl_Grid_CashDiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""

        dgv_GSTTax_Details.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Add()

        cbo_EntType.Text = "DIRECT"
        cbo_RateFor_Pcs_OR_Box.Text = "PCS"

        cbo_TransportMode.Text = "BY ROAD"
        cbo_PaymentMethod.Text = "CREDIT"

        cbo_TaxType.Text = "GST"

        cbo_Type.Text = "DIRECT"

        txt_SlNo.Text = "1"

        txt_BillDate.Text = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
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
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
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

        If Not IsNothing(dgv_BaleDetails.CurrentCell) Then dgv_BaleDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BaleDetails_Total.CurrentCell) Then dgv_BaleDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Packing_Selection.CurrentCell) Then dgv_Packing_Selection.CurrentCell.Selected = False

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName ,C.Ledger_Name as Agent_Name from Garments_Sales_Return_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Ledger_Head c ON a.Agent_idno = c.Ledger_IdNo where a.Sales_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sales_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("LedgerName").ToString
                cbo_Destination.Text = dt1.Rows(0).Item("Despatch_To").ToString
                cbo_PaymentTerms.Text = dt1.Rows(0).Item("Payment_Terms").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                If Val(dt1.Rows(0).Item("Freight_ToPay_Amount").ToString) <> 0 Then
                    txt_Freight_ToPay.Text = dt1.Rows(0).Item("Freight_ToPay_Amount").ToString
                End If
                cbo_DocumentThrough.Text = dt1.Rows(0).Item("Document_Through").ToString
                txt_DcNo.Text = dt1.Rows(0).Item("Dc_No").ToString
                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_OrderDate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_Weight.Text = Format(Val(dt1.Rows(0).Item("Weight").ToString), "########0.000")
                If Val(dt1.Rows(0).Item("Against_CForm_Status").ToString) = 1 Then chk_No_CForm.Checked = True

                txt_Charge.Text = dt1.Rows(0).Item("Charge").ToString
                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                txt_LrDate.Text = dt1.Rows(0).Item("Lr_Date").ToString
                cbo_VehicleNo.Text = Trim(dt1.Rows(0).Item("Vehicle_No").ToString)

                txt_Noof_Bundles.Text = dt1.Rows(0).Item("Total_Bags").ToString

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")


                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))


                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                txt_TradeDiscPerc.Text = Format(Val(dt1.Rows(0).Item("Extra_Charges").ToString), "########0.00")
                txt_TradeDiscAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Extra_Copies").ToString), "########0.00")

                'cbo_EntType.Text = dt1.Rows(0).Item("Entry_Type").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Entry_Type").ToString


                '***** GST START *****
                txt_Electronic_RefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                cbo_TransportMode.Text = dt1.Rows(0).Item("Transportation_Mode").ToString
                txt_DateTime_Of_Supply.Text = dt1.Rows(0).Item("Date_Time_Of_Supply").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Entry_GST_Tax_Type").ToString
                '***** GST END *****

                '***** GST START *****
                lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGst_Amount").ToString), "########0.00")
                lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGst_Amount").ToString), "########0.00")
                lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGst_Amount").ToString), "########0.00")
                '***** GST END ********

                cbo_AgentName.Text = Trim(dt1.Rows(0).Item("Agent_Name").ToString)
                cbo_RateFor_Pcs_OR_Box.Text = Trim(dt1.Rows(0).Item("Pcs_or_Box").ToString)
                txt_LessFor.Text = Format(Val(dt1.Rows(0).Item("LessFor").ToString), "########0.00")

                txt_PreparedBy.Text = dt1.Rows(0).Item("Prepared_By").ToString
                txt_BillDate.Text = dt1.Rows(0).Item("Bill_date").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name as itemsizename from Garments_Sales_Return_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.Size_idno = c.Size_idno where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()
                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("itemsizename").ToString
                        dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Bags").ToString)

                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                        'dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Sales_Detail_SlNo").ToString
                        'dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Sales_Order_Code").ToString
                        'dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Sales_Order_Detail_SlNo").ToString

                        '***** GST START *****
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Trade_Discount_Perc_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Trade_Discount_Amount_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Perc_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Amount_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Assessable_Value").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("HSN_Code").ToString
                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                        '***** GST END *****

                        dgv_Details.Rows(n).Cells(14).Value = dt2.Rows(i).Item("Style_Name").ToString

                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("SubTotal_Amount").ToString), "########0.00")
                End With

                dt2.Clear()


                '****************

                da2 = New SqlClient.SqlDataAdapter("Select a.* from Sales_Invoice_Bale_Details a Where a.Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_BaleDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_PackingSlip_No").ToString
                            .Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("Item_PackingSlip_Code").ToString

                        Next i

                    End If

                End With

                With dgv_BaleDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Bales").ToString)
                    ' .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                    '.Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                End With

                dt2.Dispose()
                da2.Dispose()
                '*****************



                TotalAmount_Calculation()
            End If

            dt1.Clear()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da2.Dispose()

            dt1.Dispose()
            da1.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub Invoice_Garments5_Sales_Return_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Size.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Invoice_Garments5_Sales_Return_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()


        lbl_LessFor.Visible = False
        txt_LessFor.Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1137" Then 'NATRAJ KNITWEAR
            lbl_LessFor.Visible = True
            txt_LessFor.Visible = True
        End If

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("")
        cbo_TaxType.Items.Add("GST")
        cbo_TaxType.Items.Add("NO TAX")

        cbo_RateFor_Pcs_OR_Box.Items.Clear()
        cbo_RateFor_Pcs_OR_Box.Items.Add("")
        cbo_RateFor_Pcs_OR_Box.Items.Add("PCS")
        cbo_RateFor_Pcs_OR_Box.Items.Add("BOX")
        'cbo_PCsBox.Items.Add("DOZEN")

        cbo_TransportMode.Items.Clear()
        cbo_TransportMode.Items.Add("")
        cbo_TransportMode.Items.Add("BY ROAD")
        cbo_TransportMode.Items.Add("BY AIR")
        cbo_TransportMode.Items.Add("BY SEA")

        cbo_DocumentThrough.Items.Clear()
        cbo_DocumentThrough.Items.Add(" ")
        cbo_DocumentThrough.Items.Add("DIRECT")
        cbo_DocumentThrough.Items.Add("BANK")
        cbo_DocumentThrough.Items.Add("AGENT")


        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("PACKING")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_BaleSelection.Visible = False
        pnl_BaleSelection.Left = (Me.Width - pnl_BaleSelection.Width) \ 2
        pnl_BaleSelection.Top = (Me.Height - pnl_BaleSelection.Height) \ 2
        pnl_BaleSelection.BringToFront()

        cbo_EntType.Items.Clear()
        cbo_EntType.Items.Add("")
        cbo_EntType.Items.Add("DIRECT")
        cbo_EntType.Items.Add("ORDER")

        cbo_PaymentMethod.Items.Clear()
        cbo_PaymentMethod.Items.Add("CASH")
        cbo_PaymentMethod.Items.Add("CREDIT")

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
        dgv_Details.Columns(5).ReadOnly = False

        '***** GST START *****
        pnl_GSTTax_Details.Visible = False
        pnl_GSTTax_Details.Left = (Me.Width - pnl_GSTTax_Details.Width) \ 2
        pnl_GSTTax_Details.Top = ((Me.Height - pnl_GSTTax_Details.Height) \ 2) - 100
        pnl_GSTTax_Details.BringToFront()
        '***** GST END *****

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Destination.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_ToPay.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DocumentThrough.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Noof_Bundles.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PreparedBy.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Noof_Boxs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TradeDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Style.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Charge.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AgentName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RateFor_Pcs_OR_Box.GotFocus, AddressOf ControlGotFocus
        '***** GST START *****
        AddHandler txt_Electronic_RefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateTime_Of_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        '***** GST END *****
        AddHandler txt_LessFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Destination.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_ToPay.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DocumentThrough.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Noof_Bundles.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoofPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PreparedBy.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Noof_Boxs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TradeDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Style.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Charge.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AgentName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RateFor_Pcs_OR_Box.LostFocus, AddressOf ControlLostFocus
        '***** GST START *****
        AddHandler txt_Electronic_RefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateTime_Of_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        '***** GST END *****
        AddHandler txt_LessFor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PreparedBy.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_Noof_Bundles.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler txt_NoofPcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_CashDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Pc_Box.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TradeDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Charge.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoofPcs.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Electronic_RefNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_NoofPcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Pc_Box.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TradeDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Charge.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_Noof_Bundles.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus

        '   AddHandler txt_NoofPcs.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Electronic_RefNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_DateTime_Of_Supply.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_BillDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BillDate.KeyPress, AddressOf TextBoxControlKeyPress



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True

        cbo_Type.Enabled = False


        new_record()

    End Sub

    Private Sub Invoice_Garments5_Sales_ReturnFormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Invoice_Garments_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_BaleSelection.Visible = True Then
                    btn_Close_PackSlip_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_GSTTax_Details.Visible = True Then
                    btn_Close_GSTTax_Details_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim trans As SqlClient.SqlTransaction

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            'cmd.CommandText = "Update Sales_Order_Details Set Sales_Items = a.Sales_Items - b.Noof_Items from Sales_Order_Details a, Garments_Sales_Return_Details b where b.Sales_Code = '" & Trim(NewCode) & "' and b.Entry_Type = 'ORDER' and a.Sales_Order_Code = b.Sales_Order_Code and a.Sales_Order_Detail_SlNo = b.Sales_Order_Detail_SlNo"
            'cmd.ExecuteNonQuery()


            cmd.CommandText = "Update Garments_Item_PackingSlip_Head set Invoice_Code = '', Invoice_Increment = Invoice_Increment - 1 Where Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Garments_Sales_Return_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Invoice_Bale_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Garments_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
            da.Fill(dt2)
            cbo_Filter_ItemName.DataSource = dt2
            cbo_Filter_ItemName.DisplayMember = "item_name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        'Try
        cmd.Connection = con
        cmd.CommandText = "select Sales_No from Garments_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No"
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

        If Trim(movno) <> "" Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single

        'Try

        OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

        da = New SqlClient.SqlDataAdapter("select Sales_No from Garments_Sales_Return_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%'  and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movno = dt.Rows(0)(0).ToString
            End If
        End If

        If Trim(movno) <> "" Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single

        'Try

        OrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

        cmd.Connection = con
        cmd.CommandText = "select Sales_No from Garments_Sales_Return_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc"

        dr = cmd.ExecuteReader

        If dr.HasRows Then
            If dr.Read Then
                If IsDBNull(dr(0).ToString) = False Then
                    movno = dr(0).ToString
                End If
            End If
        End If
        dr.Close()

        If Trim(movno) <> "" Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select Sales_No from Garments_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc", con)
        Dim dt As New DataTable
        Dim movno As String

        'Try
        da.Fill(dt)

        movno = ""
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movno = dt.Rows(0)(0).ToString
            End If
        End If

        If movno <> "" Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Garments_Sales_Return_Head", "Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select a.*, a.Tax_Type, a.Tax_Perc, a.Against_CForm_Status, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from Garments_Sales_Return_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Sales_No desc", con)
            dt2 = New DataTable
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString
                If dt2.Rows(0).Item("Document_Through").ToString <> "" Then cbo_DocumentThrough.Text = dt2.Rows(0).Item("Document_Through").ToString
                If dt2.Rows(0).Item("Pcs_or_Box").ToString <> "" Then cbo_RateFor_Pcs_OR_Box.Text = dt2.Rows(0).Item("Pcs_or_Box").ToString
                If dt2.Rows(0).Item("Transportation_Mode").ToString <> "" Then cbo_TransportMode.Text = dt2.Rows(0).Item("Transportation_Mode").ToString
                If dt2.Rows(0).Item("Against_CForm_Status").ToString <> "" Then
                    If Val(dt2.Rows(0).Item("Against_CForm_Status").ToString) = 1 Then chk_No_CForm.Checked = True
                End If

            End If

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Garments_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Invoice No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter New Invoice No.", "FOR INSERTION...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Garments_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
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

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim led_id As Integer = 0
        Dim trans_id As Integer = 0
        Dim saleac_id As Integer = 0
        Dim txac_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sz_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim vTot_Qty As Single = 0
        Dim itm_GrpId As Integer = 0
        Dim CsParNm As String = ""
        Dim vTotQty As Single = 0
        Dim vforOrdby As Single = 0
        Dim Amt As Single = 0
        Dim L_ID As Integer = 0
        Dim chk_Lab As Integer = 0
        Dim VouBil As String = ""
        Dim AgntId As Integer = 0
        Dim vDelvTo_IdNo As Integer = 0
        Dim vSTYLE_id As Integer
        Dim vSTYLNM As String = ""
        Dim vTotBls As Single
        Dim vBlsTotQty As Single, vBlsTotMtrs As Single

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sales_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        AgntId = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_AgentName.Text)
        vDelvTo_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)

        CsParNm = ""
        If led_id = 0 Then
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CREDIT" Then
                MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
                Exit Sub

            Else
                led_id = 1
                CsParNm = Trim(cbo_Ledger.Text)
            End If
        End If
        If led_id = 1 And Trim(CsParNm) = "" Then
            CsParNm = "Cash"
        End If
        If (Trim(UCase(cbo_EntType.Text)) <> "DIRECT" And Trim(UCase(cbo_EntType.Text)) <> "ORDER") Then
            MessageBox.Show("Invalid Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
            Exit Sub
        End If

        If (Trim(UCase(cbo_Type.Text)) <> "DIRECT" And Trim(UCase(cbo_Type.Text)) <> "PACKING") Then
            MessageBox.Show("Invalid Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
            Exit Sub
        End If

        trans_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        If trans_id = 0 And Trim(cbo_Transport.Text) <> "" Then
            MessageBox.Show("Invalid Transport Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Transport.Enabled Then cbo_Transport.Focus()
            Exit Sub
        End If



        saleac_id = 0
        If saleac_id = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            saleac_id = 22
            'MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If

        txac_id = 0
        'If txac_id = 0 And Val(lbl_VatAmount.Text) <> 0 Then
        '    txac_id = 20
        '    'MessageBox.Show("Invalid Tax A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
        '    'Exit Sub
        'End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(4).Value) <> 0 Then


                    itm_id = Common_Procedures.Item_NameToIdNo1(con, .Rows(i).Cells(1).Value)
                    If itm_id = 0 Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) <> "" Then
                        Sz_id = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(2).Value)
                        If Sz_id = 0 Then
                            MessageBox.Show("Invalid Size Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(2)
                                .CurrentCell.Selected = True
                            End If
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Boxs", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With


        With dgv_BaleDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Or Trim(.Rows(i).Cells(4).Value) = "" Then
                        MessageBox.Show("Invalid BaleNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With


        NoCalc_Status = False
        Amount_Calculation()
        'Total_Calculation()

        vTot_Qty = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTot_Qty = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
        End If

        'If Val(vTot_Qty) = 0 Then
        '    MessageBox.Show("Invalid Invoice Boxs", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
        '    Exit Sub
        'End If


        If dgv_BaleDetails_Total.RowCount > 0 Then
            vTotBls = Val(dgv_BaleDetails_Total.Rows(0).Cells(1).Value())
            vBlsTotQty = Val(dgv_BaleDetails_Total.Rows(0).Cells(2).Value())
            vBlsTotMtrs = Val(dgv_BaleDetails_Total.Rows(0).Cells(3).Value())
        End If


        CFrm_STS = 0
        If chk_No_CForm.Checked = True Then CFrm_STS = 1

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Garments_Sales_Return_Head", "Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)


            If New_Entry = True Then
                cmd.CommandText = "Insert into Garments_Sales_Return_Head(Sales_Code ,             Company_IdNo         ,              Sales_No             ,                               for_OrderBy                                  , Sales_Date,           Ledger_IdNo   ,          SalesAc_IdNo      ,            TaxAc_IdNo    ,              Despatch_To            ,              Payment_Terms           ,         Transport_IdNo    ,            Freight_ToPay_Amount         ,              Document_Through           ,              Lr_No           ,               Lr_Date          ,               Total_Bags            ,             Total_Qty     ,               SubTotal_Amount         , Total_DiscountAmount, Total_TaxAmount,                Gross_Amount           ,               CashDiscount_Perc        ,               CashDiscount_Amount        ,             Assessable_Value         ,               Tax_Type          , Tax_Perc  , Tax_Amount,Freight_Amount   ,                   AddLess_Amount                          ,Round_Off                  ,             Net_Amount                  ,                  Dc_No            ,              Order_No           ,                Order_Date         ,                Weight             , Against_CForm_Status             , Extra_Charges                            ,   Total_Extra_Copies                  ,                 Entry_Type   ,                                Electronic_Reference_No   ,               Transportation_Mode     ,               Date_Time_Of_Supply          ,              Entry_GST_Tax_Type ,                 CGst_Amount          ,                 SGst_Amount          ,               IGst_Amount                       ,Vehicle_No                         ,  Charge                            ,Agent_idno                ,          Pcs_or_Box            ,       LessFor           , DeliveryTo_IdNo     ,                         Prepared_By                              ,   Bill_Date) " &
                                        " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(txac_id)) & ", '" & Trim(cbo_Destination.Text) & "', '" & Trim(cbo_PaymentTerms.Text) & "', " & Str(Val(trans_id)) & ", " & Str(Val(txt_Freight_ToPay.Text)) & ", '" & Trim(cbo_DocumentThrough.Text) & "', '" & Trim(txt_LrNo.Text) & "', '" & Trim(txt_LrDate.Text) & "', " & Str(Val(txt_Noof_Bundles.Text)) & ", " & Str(Val(vTot_Qty)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ",          0          ,        0       , " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(lbl_Assessable.Text)) & ", '" & Trim(cbo_TaxType.Text) & "', 0               ,0, " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & " , '" & Trim(txt_DcNo.Text) & "', '" & Trim(txt_OrderNo.Text) & "', '" & Trim(txt_OrderDate.Text) & "', " & Str(Val(txt_Weight.Text)) & " ,   " & Str(Val(CFrm_STS)) & " ,   " & Str(Val(txt_TradeDiscPerc.Text)) & " ,   " & Str(Val(txt_TradeDiscAmount.Text)) & " , '" & Trim(cbo_Type.Text) & "', '" & Trim(txt_Electronic_RefNo.Text) & "', '" & Trim(cbo_TransportMode.Text) & "', '" & Trim(txt_DateTime_Of_Supply.Text) & "', '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(lbl_CGstAmount.Text)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", " & Str(Val(lbl_IGstAmount.Text)) & " ,'" & Trim(cbo_VehicleNo.Text) & "' ,  " & Str(Val(txt_Charge.Text)) & " , " & Str(Val(AgntId)) & " ,'" & Trim(cbo_RateFor_Pcs_OR_Box.Text) & "', " & Str(Val(txt_LessFor.Text)) & ",            " & Str(Val(vDelvTo_IdNo)) & "       ,'" & Trim(txt_PreparedBy.Text) & "'       ,    '" & Trim(txt_BillDate.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Garments_Sales_Return_Head set Sales_Date = @SalesDate, Ledger_IdNo = " & Str(Val(led_id)) & ", SalesAc_IdNo = " & Str(Val(saleac_id)) & ", TaxAc_IdNo = " & Str(Val(txac_id)) & ", Despatch_To = '" & Trim(cbo_Destination.Text) & "', Payment_Terms = '" & Trim(cbo_PaymentTerms.Text) & "', Transport_IdNo = " & Str(Val(trans_id)) & ", Freight_ToPay_Amount = " & Str(Val(txt_Freight_ToPay.Text)) & ", Document_Through = '" & Trim(cbo_DocumentThrough.Text) & "', Lr_No = '" & Trim(txt_LrNo.Text) & "', Lr_Date = '" & Trim(txt_LrDate.Text) & "', Total_Bags = " & Str(Val(txt_Noof_Bundles.Text)) & ", Total_Qty = " & Str(Val(vTot_Qty)) & ", SubTotal_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Gross_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Tax_Perc = 0, Tax_Amount = 0, Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ",Extra_Charges =  " & Str(Val(txt_TradeDiscPerc.Text)) & ", Entry_Type = '" & Trim(cbo_Type.Text) & "' ,Total_Extra_Copies =  " & Str(Val(txt_TradeDiscAmount.Text)) & ",   Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & " , Dc_No =  '" & Trim(txt_DcNo.Text) & "' , Order_No =  '" & Trim(txt_OrderNo.Text) & "' , Order_Date = '" & Trim(txt_OrderDate.Text) & "',   Weight = " & Str(Val(txt_Weight.Text)) & " , Against_CForm_Status =   " & Str(Val(CFrm_STS)) & ",  Electronic_Reference_No = '" & Trim(txt_Electronic_RefNo.Text) & "' , Charge =   " & Str(Val(txt_Charge.Text)) & "  , Transportation_Mode = '" & Trim(cbo_TransportMode.Text) & "'  ,  Date_Time_Of_Supply = '" & Trim(txt_DateTime_Of_Supply.Text) & "'  , Entry_GST_Tax_Type = '" & Trim(cbo_TaxType.Text) & "',  CGst_Amount = " & Str(Val(lbl_CGstAmount.Text)) & " , SGst_Amount = " & Str(Val(lbl_SGstAmount.Text)) & " , IGst_Amount = " & Str(Val(lbl_IGstAmount.Text)) & " ,Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' ,Agent_idno  =" & Str(Val(AgntId)) & "  ,Pcs_or_Box = '" & Trim(cbo_RateFor_Pcs_OR_Box.Text) & "',LessFor = " & Str(Val(txt_LessFor.Text)) & ", DeliveryTo_IdNo = " & Str(Val(vDelvTo_IdNo)) & " , Prepared_By = '" & Trim(txt_PreparedBy.Text) & "'  ,Bill_Date='" & Trim(txt_BillDate.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Sales_Order_Details Set Sales_Items = a.Sales_Items - b.Noof_Items from Sales_Order_Details a, Garments_Sales_Return_Details b where b.Sales_Code = '" & Trim(NewCode) & "' and b.Entry_Type = 'ORDER' and a.Sales_Order_Code = b.Sales_Order_Code and a.Sales_Order_Detail_SlNo = b.Sales_Order_Detail_SlNo"
                'cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Garments_Item_PackingSlip_Head set Invoice_Code = '', Invoice_Increment = Invoice_Increment - 1 Where Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Garments_Sales_Return_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0
            Dim nr As Integer
            With dgv_Details


                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                        itm_id = Common_Procedures.Item_NameToIdNo1(con, .Rows(i).Cells(1).Value, tr)

                        itm_GrpId = 0
                        vSTYLE_id = 0
                        Sz_id = 0
                        unt_id = 0
                        vSTYLNM = ""

                        da = New SqlClient.SqlDataAdapter("select a.*, sh.Style_Name from Item_Head a LEFT OUTER JOIN Style_Head sh ON a.Item_Style_IdNo = sh.Style_IdNo where a.Item_IdNo = " & Str(Val(itm_id)) & " ", con)
                        da.SelectCommand.Transaction = tr
                        dt = New DataTable
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            itm_GrpId = Val(dt.Rows(0).Item("ItemGroup_IdNo").ToString)
                            vSTYLE_id = Val(dt.Rows(0).Item("Item_Style_IdNo").ToString)
                            Sz_id = Val(dt.Rows(0).Item("Item_Size_IdNo").ToString)
                            unt_id = Val(dt.Rows(0).Item("Unit_IdNo").ToString)
                            vSTYLNM = dt.Rows(0).Item("Style_Name").ToString
                        End If
                        dt.Clear()


                        '**********



                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Garments_Sales_Return_Details ( Sales_Code,            Company_IdNo          ,              Sales_No             ,                                              for_OrderBy                   , Sales_Date,          Ledger_IdNo    ,            SL_No     ,          Item_IdNo      , ItemGroup_IdNo         ,         Unit_IdNo      ,            Size_IdNo   ,                          Bags  ,                               Noof_Items                 ,                 Rate                     ,                 Amount                   ,                    Total_Amount    ,                    Entry_Type ,                Sales_Order_Code ,                   Sales_Order_Detail_SlNo             ,    Trade_Discount_Perc_For_All_Item    , Trade_Discount_Amount_For_All_Item , Cash_Discount_Perc_For_All_Item     ,       Cash_Discount_Amount_For_All_Item                 ,           Assessable_Value        ,                          HSN_Code                    , Tax_Perc                             ,Style_Name,                    Item_Style_IdNo       ) " &
                                                " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & "," & Str(Val(itm_GrpId)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(Sz_id)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & " ," & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " ,'" & Trim(cbo_EntType.Text) & "' ,'" & Trim(.Rows(i).Cells(7).Value) & "' ,  " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & "," & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", '" & Trim(.Rows(i).Cells(12).Value) & "', " & Str(Val(.Rows(i).Cells(13).Value)) & ", '" & Trim(vSTYLNM) & "', " & Str(Val(vSTYLE_id)) & "   )"
                        cmd.ExecuteNonQuery()

                        'If Trim(UCase(cbo_EntType.Text)) = "ORDER" Then

                        '    cmd.CommandText = "Update Sales_Order_Details Set Sales_Items = Sales_Items + " & Str(Val(.Rows(i).Cells(4).Value)) & " where Sales_Order_Code = '" & Trim(.Rows(i).Cells(7).Value) & "' and Sales_Order_Detail_SlNo = " & Str(Val(.Rows(i).Cells(8).Value)) & " and Ledger_IdNo = " & Str(Val(led_id))
                        '    nr = cmd.ExecuteNonQuery()

                        '    If nr = 0 Then
                        '        tr.Rollback()
                        '        MessageBox.Show("Mismatch of ORDER and Party details", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        '        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                        '        Exit Sub
                        '    End If

                        'End If


                        cmd.CommandText = "Insert into Item_Processing_Details (  Reference_Code      ,               Company_IdNo       ,            Reference_No           ,                                 for_OrderBy                                , Reference_Date,        Ledger_IdNo     ,            Party_Bill_No          ,           SL_No      ,           Item_IdNo     ,            Unit_IdNo    ,                    Quantity                               ,          Size_IdNo ) " &
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",    @SalesDate, " & Str(Val(led_id)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " ,   " & Str(Val(Sz_id)) & "  )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With
            '---Tax Details
            cmd.CommandText = "Delete from Garments_Sales_Return_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_GSTTax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Garments_Sales_Return_GST_Tax_Details   (        Sales_Code      ,               Company_IdNo       ,                Sales_No           ,                               for_OrderBy                                  , Sales_Date ,         Ledger_IdNo     ,            Sl_No     ,                    HSN_Code            ,                      Taxable_Amount      ,                      CGST_Percentage     ,                      CGST_Amount         ,                      SGST_Percentage      ,                      SGST_Amount         ,                      IGST_Percentage     ,                      IGST_Amount          ) " &
                                            "          Values                  ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate , " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With


            Sno = 0
            With dgv_BaleDetails

                For i = 0 To .RowCount - 1
                    Sno = Sno + 1

                    If (Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0) And Trim(.Rows(i).Cells(4).Value) <> "" Then


                        cmd.CommandText = "Insert into Sales_Invoice_Bale_Details ( Sales_Invoice_Code ,               Company_IdNo       ,                                                 Sales_Invoice_No    ,                     for_OrderBy                                            ,          Sales_Invoice_Date  ,           Sl_No     ,              Item_PackingSlip_No        ,                  Quantity                ,                     Meters               ,               Item_PackingSlip_Code       ) " &
                                            "   Values                                      (   '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",                             @SalesDate            , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", '" & Trim(.Rows(i).Cells(4).Value) & "'   ) "
                        cmd.ExecuteNonQuery()

                        'cmd.CommandText = "Insert into FinishedProduct_Invoice_Bale_Details ( FinishedProduct_Invoice_Code ,               Company_IdNo       ,     FinishedProduct_Invoice_No    ,                     for_OrderBy                                            , FinishedProduct_Invoice_Date  ,           Sl_No     ,              Item_PackingSlip_No        ,                  Quantity                ,                     Meters               ,               Item_PackingSlip_Code       ) " &
                        '                    "   Values                                      (   '" & Trim(Pk_Condition) & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate            , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", '" & Trim(.Rows(i).Cells(4).Value) & "'   ) "
                        'cmd.ExecuteNonQuery()

                        nr = 0
                        cmd.CommandText = "Update Garments_Item_PackingSlip_Head set Invoice_Code = '" & Trim(NewCode) & "', Invoice_Increment = Invoice_Increment + 1 Where Item_PackingSlip_Code = '" & Trim(.Rows(i).Cells(4).Value) & "' and Ledger_IdNo = " & Str(Val(led_id))
                        nr = cmd.ExecuteNonQuery()

                        If nr = 0 Then
                            MessageBox.Show("Invalid PackingSlip Details - Mismatch of details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tr.Rollback()
                            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                            Exit Sub
                        End If

                    End If

                Next

            End With





            Dim vVouPos_IdNos As String = "", vVouPos_Amts As String = "", vVouPos_ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
                AcPos_ID = 1
            Else
                AcPos_ID = led_id
            End If

            Dim vNetAmt As String = Format(Val(CSng(lbl_NetAmount.Text)), "#############0.00")

            '---GST
            vVouPos_IdNos = AcPos_ID & "|" & saleac_id & "|" & txac_id & "|" & "25|26|27|9|17|24"

            vVouPos_Amts = Val(vNetAmt) & "|" & -1 * Val(vNetAmt) - (Val(lbl_TaxAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(txt_RoundOff.Text)) & "|" & Val(lbl_TaxAmount.Text) & "|" & Val(lbl_CGstAmount.Text) & "|" & Val(lbl_SGstAmount.Text) & "|" & Val(lbl_IGstAmount.Text) & "|" & Val(txt_Freight.Text) & "|" & Val(txt_AddLess.Text) & "|" & Val(txt_RoundOff.Text)

            If Common_Procedures.Voucher_Updation(con, "GarmenSal Ret", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Bill No . : " & Trim(lbl_InvoiceNo.Text), vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVouPos_ErrMsg)
            End If

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            Ac_id = led_id

            'cmd.CommandText = "Insert into Voucher_Head(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) " & _
            '                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SaleSV', @SalesDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SaleSV', @SalesDate, 1, " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SaleSV', @SalesDate, 2, " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text)) - Val(lbl_CGstAmount.Text) - Val(lbl_TaxAmount.Text)  & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'If Val(lbl_TaxAmount.Text) <> 0 Then
            '    cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SaleSV', @SalesDate, 3, " & Str(Val(txac_id)) & ", " & Str(Val(lbl_VatAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            '    cmd.ExecuteNonQuery()
            'End If


            '-----Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(dtp_Date.Text), led_id, Trim(lbl_InvoiceNo.Text), AgntId, Val(vNetAmt), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            move_record(lbl_InvoiceNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            tr.Dispose()
            cmd.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try




    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean
        Dim itm_id As Integer = 0
        Dim Sz_id As Integer = 0

        itm_id = Common_Procedures.Item_NameToIdNo1(con, cbo_ItemName.Text)

        If Val(itm_id) = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        If Trim(cbo_Size.Text) <> "" Then
            Sz_id = Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text)
            If Val(Sz_id) = 0 Then
                MessageBox.Show("Invalid Size", "DOES NOT ADD...", MessageBoxButtons.OK)
                If cbo_Size.Enabled Then cbo_Size.Focus()
                Exit Sub
            End If
        End If

        If Val(txt_NoofPcs.Text) = 0 Then
            MessageBox.Show("Invalid Pcs", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_NoofPcs.Enabled Then txt_NoofPcs.Focus()
            Exit Sub
        End If

        If Val(txt_Rate.Text) = 0 Then
            MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Rate.Enabled Then txt_Rate.Focus()
            Exit Sub
        End If

        If Val(lbl_Amount.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Rate.Enabled Then txt_Rate.Focus()
            Exit Sub
        End If

        'If Val(lbl_ItemStock.Text) <= 0 Or Val(lbl_ItemStock.Text) < Val(txt_box.Text) Then
        '    MessageBox.Show("Invalid Stock" & Chr(13) & "Available Stock : " & lbl_ItemStock.Text, "DOES NOT ADD...", MessageBoxButtons.OK)
        '    If txt_box.Enabled Then txt_box.Focus()
        '    Exit Sub
        'End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_ItemName.Text
                    .Rows(i).Cells(2).Value = cbo_Size.Text
                    .Rows(i).Cells(3).Value = Val(txt_Noof_Boxs.Text)
                    .Rows(i).Cells(4).Value = Val(txt_NoofPcs.Text)
                    .Rows(i).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(6).Value = Format(Val(lbl_Amount.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(lbl_Grid_TradeDiscPerc.Text), "########0.00")
                    .Rows(i).Cells(8).Value = Format(Val(lbl_Grid_TradeDiscAmount.Text), "########0.00")
                    .Rows(i).Cells(9).Value = Format(Val(lbl_Grid_CashDiscPerc.Text), "########0.00")
                    .Rows(i).Cells(10).Value = Format(Val(lbl_Grid_CashDiscAmount.Text), "########0.00")
                    .Rows(i).Cells(11).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")

                    .Rows(i).Cells(12).Value = lbl_Grid_HsnCode.Text
                    .Rows(i).Cells(13).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")
                    .Rows(i).Cells(14).Value = Trim(cbo_Style.Text)

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 7 Then .FirstDisplayedScrollingRowIndex = i - 6

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(2).Value = cbo_Size.Text
                .Rows(n).Cells(3).Value = Val(txt_Noof_Boxs.Text)
                .Rows(n).Cells(4).Value = Val(txt_NoofPcs.Text)
                .Rows(n).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(6).Value = Format(Val(lbl_Amount.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(lbl_Grid_TradeDiscPerc.Text), "########0.00")
                .Rows(n).Cells(8).Value = Format(Val(lbl_Grid_TradeDiscAmount.Text), "########0.00")
                .Rows(n).Cells(9).Value = Format(Val(lbl_Grid_CashDiscPerc.Text), "########0.00")
                .Rows(n).Cells(10).Value = Format(Val(lbl_Grid_CashDiscAmount.Text), "########0.00")
                .Rows(n).Cells(11).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")

                .Rows(n).Cells(12).Value = lbl_Grid_HsnCode.Text
                .Rows(n).Cells(13).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")
                .Rows(n).Cells(14).Value = Trim(cbo_Style.Text)

                .Rows(n).Selected = True

                If n >= 7 Then .FirstDisplayedScrollingRowIndex = n - 6

            End If

        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Size.Text = ""
        cbo_Style.Text = ""
        txt_Noof_Boxs.Text = ""
        txt_NoofPcs.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub txt_NoofPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofPcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Rate.Focus()
        End If
    End Sub

    Private Sub txt_NoofPcs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoofPcs.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus

        With cbo_ItemName
            vcmb_ItmNm = Trim(.Text)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
            cbo_ItemName.Tag = cbo_ItemName.Text
        End With

    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, txt_SlNo, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If Trim(cbo_ItemName.Text) <> "" Then
                ' cbo_Style.Focus()
                txt_Noof_Boxs.Focus()
            Else
                txt_CashDiscPerc.Focus()
            End If

            get_Item_Details()

        End If

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
                Get_PriceList_Rate()
                get_Item_Unit_Rate_TaxPerc()
            End If

            If Trim(cbo_ItemName.Text) <> "" Then
                ' cbo_Style.Focus()
                txt_Noof_Boxs.Focus()

            Else
                txt_CashDiscPerc.Focus()
            End If

            get_Item_Details()

        End If


    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1079" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1103" Then
                'Dim f As New Item_Creation_With_Size

                'Common_Procedures.Master_Return.Form_Name = Me.Name
                'Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
                'Common_Procedures.Master_Return.Return_Value = ""
                'Common_Procedures.Master_Return.Master_Type = ""

                'f.MdiParent = MDIParent1
                'f.Show()
            Else
                Dim f As New Item_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If



        End If
    End Sub

    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus
        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            Get_PriceList_Rate()
        End If

        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            get_Item_Unit_Rate_TaxPerc()
        End If

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : txt_PreparedBy.Focus()
    End Sub

    Private Sub cbo_DocumentThrough_GotFocus(sender As Object, e As EventArgs) Handles cbo_DocumentThrough.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_DocumentThrough_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DocumentThrough.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_DcNo, cbo_TransportMode, "", "", "", "")
    End Sub

    Private Sub cbo_DocumentThrough_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DocumentThrough.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_TransportMode, "", "", "", "", False)
    End Sub


    Private Sub cbo_PaymentTerms_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_PaymentTerms.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_Noof_Bundles, txt_Electronic_RefNo, "Garments_Sales_Return_Head", "Payment_Terms", "", "")
    End Sub

    Private Sub cbo_PaymentTerms_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_PaymentTerms.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_Electronic_RefNo, "Garments_Sales_Return_Head", "Payment_Terms", "", "", False)
    End Sub

    Private Sub cbo_PaymentTerms_GotFocus(sender As Object, e As EventArgs) Handles cbo_PaymentTerms.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Garments_Sales_Return_Head", "Payment_Terms", "", "")
    End Sub


    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim AgNm As String
        Dim Led_Idno As Integer = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    '    If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
        '    '        cbo_Ledger.Tag = cbo_Ledger.Text
        '    '        Amount_Calculation(True)
        '    '    End If

        '    '    If (Trim(UCase(cbo_Type.Text)) <> "DIRECT") Then
        '    '        If MessageBox.Show("Do you want to select PackingSlip..?", "FOR SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '    '            btn_PackSlip_Selection_Click(sender, e)

        '    '        Else
        '    '            txt_OrderNo.Focus()
        '    '        End If

        '    '    End If


        '    Led_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_Ledger.Text))

        '    da = New SqlClient.SqlDataAdapter("select a.* from ledger_head a where a.ledger_idno = " & Str(Val(Led_Idno)) & "  ", con)
        '    dt = New DataTable
        '    da.Fill(dt)

        '    AgNm = ""
        '    trpt_Idno = 0

        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '            AgNm = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0)("Ledger_AgentIdNo").ToString))
        '            trpt_Idno = Val(dt.Rows(0).Item("Transport_IdNo").ToString)
        '        End If
        '    End If

        '    dt.Dispose()
        '    da.Dispose()

        '    If Trim(AgNm) <> "" Then cbo_AgentName.Text = AgNm
        '    If Val(trpt_Idno) <> 0 Then cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(trpt_Idno))


        '    txt_OrderNo.Focus()


        'End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_VehicleNo, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        '  Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_VehicleNo, txt_LrNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        ' Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_LrNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            'Dim f As New Transport_Creation
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

    Private Sub cbo_Destination_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Destination.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Destination, txt_LrDate, txt_Noof_Bundles, "", "", "", "")
    End Sub

    Private Sub cbo_Destination_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Destination.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Destination, txt_Noof_Bundles, "", "", "", "", False)
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        Amount_Calculation(True)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Amount_Calculation(True)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        Amount_Calculation(True)
        NetAmount_Calculation()
    End Sub


    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Amount_Calculation(True)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Noof_Bags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Noof_Bundles.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            cbo_RateFor_Pcs_OR_Box.Focus()

        End If
    End Sub
    Private Sub txt_Noof_Bundles_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Noof_Bundles.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_Destination.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True
            cbo_RateFor_Pcs_OR_Box.Focus()
        End If
    End Sub

    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : cbo_ItemName.Focus()
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_RateFor_Pcs_OR_Box.Focus()
        End If
    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then
            cbo_ItemName.Focus()
            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                        cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                        cbo_Size.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
                        txt_Noof_Boxs.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
                        txt_NoofPcs.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
                        txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                        lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
                        lbl_Grid_TradeDiscPerc.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.00")
                        lbl_Grid_TradeDiscAmount.Text = Format(Val(.Rows(i).Cells(8).Value), "########0.00")
                        lbl_Grid_CashDiscPerc.Text = Format(Val(.Rows(i).Cells(9).Value), "########0.00")
                        lbl_Grid_CashDiscAmount.Text = Format(Val(.Rows(i).Cells(10).Value), "########0.00")
                        lbl_Grid_AssessableValue.Text = Format(Val(.Rows(i).Cells(11).Value), "########0.00")
                        lbl_Grid_HsnCode.Text = .Rows(i).Cells(12).Value
                        lbl_Grid_GstPerc.Text = Format(Val(.Rows(i).Cells(13).Value), "########0.00")
                        cbo_Style.Text = Trim(.Rows(i).Cells(14).Value)



                        'cbo_ItemName.Text = Trim(.Rows(i).Cells(1).Value)
                        'cbo_Size.Text = Trim(.Rows(i).Cells(2).Value)
                        'txt_box.Text = Val(.Rows(i).Cells(3).Value)
                        'txt_Rate.Text = Format(Val(.Rows(i).Cells(4).Value), "########0.00")
                        'lbl_Amount.Text = Format(Val(.Rows(i).Cells(5).Value), "########0.00")

                        Exit For

                    End If

                Next

            End With

        End If
    End Sub


    Private Sub cbo_Size_GotFocus(sender As Object, e As EventArgs) Handles cbo_Size.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "size_head", "size_name", "", "(size_idno = 0)")
        cbo_Size.Tag = cbo_Size.Text
    End Sub

    Private Sub cbo_Size_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Size.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_Style, txt_Noof_Boxs, "size_head", "size_name", "", "(size_idno = 0)")
    End Sub

    Private Sub cbo_Size_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_Noof_Boxs, "size_head", "size_name", "", "(size_idno = 0)")
    End Sub

    Private Sub cbo_Size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Style_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Style.GotFocus
        With cbo_Style
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Style_head", "Style_name", "", "(Style_idno = 0)")
        End With

    End Sub

    Private Sub cbo_Style_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Style.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Style, cbo_ItemName, cbo_Size, "Style_head", "Style_name", "", "(Style_idno = 0)")
    End Sub

    Private Sub cbo_Style_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Style.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Style, cbo_Size, "Style_head", "Style_name", "", "(Style_idno = 0)")
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
        Dim Led_IdNo As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                Itm_IdNo = Common_Procedures.Item_NameToIdNo1(con, cbo_Filter_ItemName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Code IN (select z.Sales_Code from Garments_Sales_Return_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.Sales_No, a.Sales_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Garments_Sales_Return_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            dt1.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True 'SendKeys.Send("+{TAB}")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

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
    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Siz_idno As Integer = 0
        Dim sqft_qty As Single = 0

        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If Trim(UCase(cbo_EntType.Text)) = "ORDER" Then

                        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                            .Rows(.CurrentCell.RowIndex).Cells(6).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(4).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(5).Value), "#########0.00")
                            TotalAmount_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try

    End Sub
    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Panel2.Enabled = True And txt_SlNo.Enabled = True Then


            If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

                txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                cbo_Size.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
                txt_Noof_Boxs.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
                txt_NoofPcs.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
                txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
                lbl_Grid_TradeDiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
                lbl_Grid_TradeDiscAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
                lbl_Grid_CashDiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
                lbl_Grid_CashDiscAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
                lbl_Grid_AssessableValue.Text = Format(Val(dgv_Details.CurrentRow.Cells(11).Value), "########0.00")
                lbl_Grid_HsnCode.Text = dgv_Details.CurrentRow.Cells(12).Value
                lbl_Grid_GstPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0.00")
                cbo_Style.Text = Trim(dgv_Details.CurrentRow.Cells(14).Value)


                'If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()
                If (Trim(UCase(cbo_Type.Text)) <> "DIRECT") Then
                    'If txt_Noof_Boxs.Enabled And txt_Noof_Boxs.Visible Then txt_Noof_Boxs.Focus()
                    If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
                Else
                    If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

                End If


            End If
        End If
    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows.RemoveAt(i)

                    MtchSTS = True

                    Exit For

                End If

            Next

            If MtchSTS = True Then
                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next
            End If

        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Size.Text = ""
        cbo_Style.Text = ""
        txt_Noof_Boxs.Text = ""
        txt_NoofPcs.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        lbl_Grid_TradeDiscAmount.Text = ""
        lbl_Grid_TradeDiscPerc.Text = ""
        lbl_Grid_CashDiscPerc.Text = ""
        lbl_Grid_CashDiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_GstPerc.Text = ""

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index
                .Rows.RemoveAt(n)

                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next

            End With

            TotalAmount_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            cbo_ItemName.Text = ""
            cbo_Size.Text = ""
            cbo_Style.Text = ""
            txt_Noof_Boxs.Text = ""
            txt_NoofPcs.Text = ""
            txt_Rate.Text = ""
            lbl_Amount.Text = ""
            lbl_Grid_CashDiscPerc.Text = ""
            lbl_Grid_CashDiscAmount.Text = ""
            lbl_Grid_AssessableValue.Text = ""
            lbl_Grid_GstPerc.Text = ""

            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            'Dim f As New Ledger_Creation
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_OrderDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OrderDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_OrderDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OrderDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_OrderDate.Text = Date.Today
            txt_OrderDate.SelectionStart = txt_OrderDate.Text.Length
        End If
    End Sub

    Private Sub txt_LrNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LrNo.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : txt_LrDate.Focus()
        If e.KeyCode = 38 Then e.Handled = True : cbo_Transport.Focus()
    End Sub

    Private Sub txt_LrNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LrNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_LrDate.Focus()
        End If
    End Sub

    Private Sub txt_LrDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LrDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then e.Handled = True : cbo_Destination.Focus()
        If e.KeyCode = 38 Then e.Handled = True : txt_LrNo.Focus()
    End Sub

    Private Sub txt_LrDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_LrDate.Text = Date.Today
            txt_LrDate.SelectionStart = txt_LrDate.Text.Length
        End If
    End Sub

    Private Sub txt_Freight_ToPay_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight_ToPay.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : cbo_DocumentThrough.Focus()
        If e.KeyCode = 38 Then e.Handled = True : txt_Weight.Focus()
    End Sub

    Private Sub txt_Freight_ToPay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight_ToPay.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            cbo_DocumentThrough.Focus()
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_PreparedBy.Focus()

            'If txt_LessFor.Visible = False Then
            '    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '        save_record()
            '    Else
            '        dtp_Date.Focus()
            '    End If
            'Else
            '    txt_LessFor.Focus()
            'End If

        End If
    End Sub


    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        prn_Status = 2
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
        prn_Status = 1
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub Get_HSN_CodeWise_GSTTax_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim AssVal_Frgt_Othr_Charges As Double = 0
        Dim LedIdNo As Integer = 0
        Dim ItmIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False

        Try

            If FrmLdSTS = True Then Exit Sub

            LedIdNo = 0
            InterStateStatus = False
            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

            End If

            AssVal_Frgt_Othr_Charges = Val(txt_Freight.Text)

            cmd.Connection = con

            cmd.CommandText = "Truncate table EntryTemp"
            cmd.ExecuteNonQuery()

            With dgv_Details

                If .Rows.Count > 0 Then
                    For i = 0 To .Rows.Count - 1

                        If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(12).Value) <> "" And Val(.Rows(i).Cells(13).Value) <> 0 Then

                            cmd.CommandText = "Insert into EntryTemp (                    Name1                ,                   Currency1            ,                       Currency2                                      ) " &
                                              "            Values    ( '" & Trim(.Rows(i).Cells(12).Value) & "', " & (Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value) + AssVal_Frgt_Othr_Charges) & " ) "
                            cmd.ExecuteNonQuery()

                            AssVal_Frgt_Othr_Charges = 0

                        End If

                    Next
                End If
            End With

            With dgv_GSTTax_Details

                .Rows.Clear()
                Sno = 0

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as Assessable_Value from EntryTemp group by name1, Currency1 Having sum(Currency2) <> 0 ", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1

                        .Rows(n).Cells(0).Value = Sno
                        .Rows(n).Cells(1).Value = dt.Rows(i).Item("HSN_Code").ToString

                        .Rows(n).Cells(2).Value = Format(Val(dt.Rows(i).Item("Assessable_Value").ToString), "############0.00")
                        If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""

                        .Rows(n).Cells(3).Value = ""
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(7).Value = ""
                        If InterStateStatus = True Then
                            .Rows(n).Cells(7).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString), "######0.00")
                            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                        Else
                            .Rows(n).Cells(3).Value = Format(Val(dt.Rows(i)("GST_Percentage").ToString) / 2, "#########0.00")
                            .Rows(n).Cells(5).Value = Format(Val(dt.Rows(i)("GST_Percentage").ToString) / 2, "#########0.00")

                        End If

                        .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "############0.00")
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                        .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "############0.00")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                        .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "############0.00")
                        If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                    Next i

                End If

                dt.Clear()
                dt.Dispose()
                da.Dispose()

            End With

            Total_GSTTax_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Private Sub Total_GSTTax_Calculation()
        Dim Sno As Integer
        Dim TotAss_Val As Single
        Dim TotCGST_amt As Single
        Dim TotSGST_amt As Double
        Dim TotIGST_amt As Double

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotAss_Val = 0 : TotCGST_amt = 0 : TotSGST_amt = 0 : TotIGST_amt = 0

        With dgv_GSTTax_Details

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


        With dgv_GSTTax_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(TotAss_Val), "########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotCGST_amt), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotSGST_amt), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotIGST_amt), "########0.00")
        End With


    End Sub


    Private Sub get_Item_Tax(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0

        Try

            If FrmLdSTS = True Then Exit Sub

            lbl_Grid_GstPerc.Text = ""
            lbl_Grid_HsnCode.Text = ""

            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

                ItmIdNo = Common_Procedures.Item_NameToIdNo1(con, cbo_ItemName.Text)

                lbl_Grid_CashDiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")

                da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                        lbl_Grid_HsnCode.Text = dt.Rows(0)("Item_HSN_Code").ToString
                    End If
                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                        lbl_Grid_GstPerc.Text = dt.Rows(0)("Item_GST_Percentage").ToString
                    End If
                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

            End If

            Amount_Calculation(False)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT GET ITEM TAX DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub get_Item_Unit_Rate_TaxPerc()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            cbo_ItemName.Tag = cbo_ItemName.Text
            da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                'If IsDBNull(dt.Rows(0)("unit_name").ToString) = False Then
                '    cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                'End If
                'If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
                '    txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
                'End If
                get_Item_Tax(False)
            End If
            dt.Dispose()
            da.Dispose()
        End If

    End Sub

    Private Sub Amount_Calculation(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0

        If FrmLdSTS = True Then Exit Sub

        If GridAll_Row_STS = True Then

            With dgv_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        ItmIdNo = Common_Procedures.Item_NameToIdNo1(con, .Rows(i).Cells(1).Value)
                        If ItmIdNo <> 0 Then

                            .Rows(i).Cells(12).Value = ""
                            .Rows(i).Cells(13).Value = ""

                            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                                da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                                dt = New DataTable
                                da.Fill(dt)
                                If dt.Rows.Count > 0 Then

                                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                                        .Rows(i).Cells(12).Value = dt.Rows(0)("Item_HSN_Code").ToString
                                    End If
                                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                                        .Rows(i).Cells(13).Value = Format(Val(dt.Rows(0)("Item_GST_Percentage").ToString), "#########0.00")
                                    End If

                                End If
                                dt.Clear()

                            End If


                            If Trim(UCase(cbo_RateFor_Pcs_OR_Box.Text)) = Trim(UCase("BOX")) Then
                                .Rows(i).Cells(6).Value = Format(Val(.Rows(i).Cells(3).Value) * Val(.Rows(i).Cells(5).Value), "#########0.00")
                            Else
                                .Rows(i).Cells(6).Value = Format(Val(.Rows(i).Cells(4).Value) * Val(.Rows(i).Cells(5).Value), "#########0.00")
                            End If

                            .Rows(i).Cells(7).Value = Format(Val(txt_TradeDiscPerc.Text), "#########0.00")

                            .Rows(i).Cells(8).Value = Format(Val(.Rows(i).Cells(6).Value) * Val(.Rows(i).Cells(7).Value) / 100, "#########0.00")

                            .Rows(i).Cells(9).Value = Format(Val(txt_CashDiscPerc.Text), "#########0.00")

                            .Rows(i).Cells(10).Value = Format(Val(.Rows(i).Cells(6).Value) * Val(.Rows(i).Cells(9).Value) / 100, "#########0.00")

                            .Rows(i).Cells(11).Value = Format(Val(.Rows(i).Cells(6).Value) - Val(.Rows(i).Cells(8).Value) - .Rows(i).Cells(10).Value, "#########0.00")


                        End If


                    End If

                Next

            End With

            TotalAmount_Calculation()

        Else


            If Trim(cbo_ItemName.Text) <> "" Then

                ItmIdNo = Common_Procedures.Item_NameToIdNo1(con, cbo_ItemName.Text)
                If ItmIdNo <> 0 Then

                    lbl_Grid_HsnCode.Text = ""
                    lbl_Grid_GstPerc.Text = ""

                    If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                        da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                        dt = New DataTable
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then

                            If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                                lbl_Grid_HsnCode.Text = dt.Rows(0)("Item_HSN_Code").ToString
                            End If
                            If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                                lbl_Grid_GstPerc.Text = Format(Val(dt.Rows(0)("Item_GST_Percentage").ToString), "#########0.00")
                            End If

                        End If
                        dt.Clear()

                    End If

                End If

            End If


            If Trim(UCase(cbo_RateFor_Pcs_OR_Box.Text)) = Trim(UCase("BOX")) Then
                lbl_Amount.Text = Format(Val(txt_Noof_Boxs.Text) * Val(txt_Rate.Text), "#########0.00")
            Else
                lbl_Amount.Text = Format(Val(txt_NoofPcs.Text) * Val(txt_Rate.Text), "#########0.00")
            End If

            lbl_Grid_TradeDiscPerc.Text = Format(Val(txt_TradeDiscPerc.Text), "#########0.00")

            lbl_Grid_TradeDiscAmount.Text = Format(Val(lbl_Amount.Text) * Val(lbl_Grid_TradeDiscPerc.Text) / 100, "#########0.00")

            lbl_Grid_CashDiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")

            lbl_Grid_CashDiscAmount.Text = Format(Val(lbl_Amount.Text) * Val(lbl_Grid_CashDiscPerc.Text) / 100, "#########0.00")

            lbl_Grid_AssessableValue.Text = Format(Val(lbl_Amount.Text) - Val(lbl_Grid_TradeDiscAmount.Text) - Val(lbl_Grid_CashDiscAmount.Text), "#########0.00")

        End If


    End Sub

    Private Sub TotalAmount_Calculation()
        Dim Sno As Integer = 0
        Dim TotQty As Decimal = 0
        Dim TotGrsAmt As Decimal = 0
        Dim TotCashDiscAmt As Decimal = 0
        Dim TotTradeDiscAmt As Decimal = 0
        Dim TotAssval As Decimal = 0
        Dim TotCGstAmt As Decimal = 0
        Dim TotSGstAmt As Decimal = 0
        Dim TotIGstAmt As Decimal = 0

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotQty = 0
        TotGrsAmt = 0

        TotAssval = 0

        For i = 0 To dgv_Details.RowCount - 1

            Sno = Sno + 1

            dgv_Details.Rows(i).Cells(0).Value = Sno

            If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then
                TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(4).Value)
                TotGrsAmt = TotGrsAmt + Val(dgv_Details.Rows(i).Cells(6).Value)
                TotTradeDiscAmt = TotTradeDiscAmt + Val(dgv_Details.Rows(i).Cells(8).Value)
                TotCashDiscAmt = TotCashDiscAmt + Val(dgv_Details.Rows(i).Cells(10).Value)
                TotAssval = TotAssval + Val(dgv_Details.Rows(i).Cells(11).Value)

            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotQty)
            .Rows(0).Cells(6).Value = Format(Val(TotGrsAmt), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotTradeDiscAmt), "########0.00")
            .Rows(0).Cells(10).Value = Format(Val(TotCashDiscAmt), "########0.00")
            .Rows(0).Cells(11).Value = Format(Val(TotAssval), "########0.00")
        End With

        lbl_GrossAmount.Text = Format(TotGrsAmt, "########0.00")
        txt_TradeDiscAmount.Text = Format(TotTradeDiscAmt, "########0.00")
        txt_CashDiscAmount.Text = Format(TotCashDiscAmt, "########0.00")


        Dim TotBls As Single
        Dim TotMtrs As Single

        Sno = 0
        TotBls = 0 : TotQty = 0 : TotMtrs = 0

        With dgv_BaleDetails
            For i = 0 To .RowCount - 1

                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno

                If Trim(.Rows(i).Cells(1).Value) <> "" And (Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0) Then

                    TotBls = TotBls + 1
                    TotQty = TotQty + Val(.Rows(i).Cells(2).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(3).Value)

                End If

            Next

        End With

        With dgv_BaleDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBls)
            .Rows(0).Cells(2).Value = Val(TotQty)
            .Rows(0).Cells(3).Value = Format(Val(TotMtrs), "########0.00")
        End With


        Get_HSN_CodeWise_GSTTax_Details()

        TotAssval = 0
        TotCGstAmt = 0
        TotSGstAmt = 0
        TotIGstAmt = 0
        With dgv_GSTTax_Details_Total
            If .RowCount > 0 Then
                TotAssval = Val(.Rows(0).Cells(2).Value)
                TotCGstAmt = Val(.Rows(0).Cells(4).Value)
                TotSGstAmt = Val(.Rows(0).Cells(6).Value)
                TotIGstAmt = Val(.Rows(0).Cells(8).Value)
            End If
        End With

        lbl_Assessable.Text = Format(TotAssval, "########0.00")
        lbl_CGstAmount.Text = Format(TotCGstAmt, "########0.00")
        lbl_SGstAmount.Text = Format(TotSGstAmt, "########0.00")
        lbl_IGstAmount.Text = Format(TotIGstAmt, "########0.00")

        NetAmount_Calculation()

    End Sub

    Private Sub Amount_Calculation()
        lbl_Amount.Text = Format(Val(txt_NoofPcs.Text) * Val(txt_Rate.Text), "#########0.00")
    End Sub

    Private Sub Total_Calculation11111()
        Dim I As Integer
        Dim Sno As Integer
        Dim TotQty As Decimal, TotAmt As Decimal

        Sno = 0
        TotQty = 0
        TotAmt = 0

        With dgv_Details

            For I = 0 To .RowCount - 1
                Sno = Sno + 1
                dgv_Details.Rows(I).Cells(0).Value = Sno

                If Trim(.Rows(I).Cells(1).Value) <> "" Or Val(.Rows(I).Cells(4).Value) <> 0 Then

                    TotQty = TotQty + Val(dgv_Details.Rows(I).Cells(4).Value)
                    TotAmt = TotAmt + Val(dgv_Details.Rows(I).Cells(6).Value)

                End If

            Next

        End With

        With dgv_Details_Total
            If .Rows.Count = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotQty)
            .Rows(0).Cells(6).Value = Format(Val(TotAmt), "########0.00")
        End With

        lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        'txt_TradeDiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_TradeDiscPerc.Text) / 100, "#########0.00")

        'txt_CashDiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")

        'lbl_Assessable.Text = Format(Val(lbl_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) - Val(txt_TradeDiscAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text), "#########0.00")

        NtAmt = Val(lbl_GrossAmount.Text) - Val(txt_TradeDiscAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

        txt_RoundOff.Text = Format(Val(lbl_NetAmount.Text) - Val(NtAmt), "#########0.00")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record


        InvPrintFrmt = Common_Procedures.settings.InvoicePrint_Format

        prn_Status = 2
        printing_invoice()

    End Sub

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim CmpName As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Garments_Sales_Return_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Sales_Code = '" & Trim(NewCode) & "'", con)
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

        'CmpName = Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

        'If InvPrintFrmt_Letter <> 1 Then
        '    If prn_Status <> 1 Then
        '        prn_InpOpts = ""
        '        If Trim(UCase(InvPrintFrmt)) <> "FORMAT-6" Then
        '            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "     1. Original" & Space(5) & "    2. Duplicate" & Chr(13) & "     3. Triplicate" & Space(3) & "   4. Transport Copy" & Chr(13) & "     5. Extra Copy  " & Space(1) & "6.All", "FOR INVOICE PRINTING...", "12345")
        '            prn_InpOpts = Replace(Trim(prn_InpOpts), "6", "12345")
        '        End If
        '    End If

        'End If



        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument1.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        ppd.Document.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)


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
        Dim I As Integer, K As Integer
        Dim ItmNm1 As String, ItmNm2 As String
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_Count = 0
        prn_TOTBOX = 0


        ' Try
        'new
        'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, b.Pan_No Ledger_PanNo, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, g.Ledger_Name as DelName ,g.Ledger_Address1 as DelAdd1 ,g.Ledger_Address2 as DelAdd2, g.Ledger_Address3 as DelAdd3 ,g.Ledger_Address4 as DelAdd4, g.Ledger_GSTinNo as DelGSTinNo, g.Pan_No as DelPanNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code from Garments_Sales_Return_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo LEFT OUTER JOIN Ledger_Head g ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = g.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on g.State_IdNo = DSH.State_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
        'g.Ledger_Name as DelName ,g.Ledger_Address1 as DelAdd1 ,g.Ledger_Address2 as DelAdd2, g.Ledger_Address3 as DelAdd3 ,g.Ledger_Address4 as DelAdd4, g.Ledger_GSTinNo as DelGSTinNo, g.Pan_No as DelPanNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code 
        'old
        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.*, e.Ledger_Name AS agent_name, Csh.State_Name as Cmp_State_Name, Csh.State_Code as Cmp_State_Code, Lsh.State_Name as Ledger_State_Name ,LSh.State_Code as LEdger_State_Code, g.Ledger_Name as DelName, g.Ledger_Address1 as DelAdd1 ,g.Ledger_Address2 as DelAdd2, g.Ledger_Address3 as DelAdd3 ,g.Ledger_Address4 as DelAdd4, g.Ledger_GSTinNo as DelGSTinNo, g.Pan_No as DelPanNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code  from Garments_Sales_Return_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh On b.State_IDno = lsh.State_Idno INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo LEFT OUTER JOIN State_Head Csh On csh.State_IDno = c.Company_State_IdNo LEFT OUTER JOIN  Transport_Head D ON A.Transport_IdNo = D.Transport_IdNo LEFT OUTER JOIN Ledger_Head E ON A.Agent_idno = E.Ledger_IdNo  LEFT OUTER JOIN Ledger_Head g ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = g.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on g.State_IdNo = DSH.State_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
        prn_HdDt = New DataTable
        da1.Fill(prn_HdDt)

        If prn_HdDt.Rows.Count > 0 Then

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, b.Item_DisplayName, c.Size_Name, IG.ItemGroup_Name,u.Unit_name from Garments_Sales_Return_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo " &
                                               "  LEFT OUTER JOIN Unit_Head u on b.unit_idno = u.unit_idno  " &
                                               " LEFT OUTER JOIN Size_Head c on a.size_idno = c.size_idno LEFT OUTER JOIN  ItemGroup_Head IG ON b.ItemGroup_IdNo = IG.ItemGroup_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            prn_DetDt = New DataTable
            da2.Fill(prn_DetDt)

            da2.Dispose()

        Else
            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

        da1.Dispose()

        '  Catch ex As Exception
        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '  End Try


        Erase prn_DetAr

        prn_DetAr = New String(500, 20) {}


        '***** GST START *****
        'new
        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code , D.Ledger_Name AS Transport , E.Ledger_Name AS agent_name , g.Ledger_Name as DelName ,g.Ledger_Address1 as DelAdd1 ,g.Ledger_Address2 as DelAdd2, g.Ledger_Address3 as DelAdd3 ,g.Ledger_Address4 as DelAdd4, g.Ledger_GSTinNo as DelGSTinNo, g.Pan_No as DelPanNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code from Garments_Sales_Return_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.Ledger_State_IdNo = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo LEFT OUTER JOIN  Ledger_Head D ON A.Transport_IdNo = D.Ledger_IdNo LEFT OUTER JOIN Ledger_Head E ON A.Agent_idno = E.Ledger_IdNo  LEFT OUTER JOIN Ledger_Head g ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = g.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on g.State_IdNo = DSH.State_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
        'old

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code , D.Ledger_Name AS Transport , E.Ledger_Name AS agent_name , g.Ledger_Name as DelName ,g.Ledger_Address1 as DelAdd1 ,g.Ledger_Address2 as DelAdd2, g.Ledger_Address3 as DelAdd3 ,g.Ledger_Address4 as DelAdd4, g.Ledger_GSTinNo as DelGSTinNo, g.Pan_No as DelPanNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code from Garments_Sales_Return_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.Ledger_State_IdNo = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo LEFT OUTER JOIN  Ledger_Head D ON A.Transport_IdNo = D.Ledger_IdNo LEFT OUTER JOIN Ledger_Head E ON A.Agent_idno = E.Ledger_IdNo  LEFT OUTER JOIN Ledger_Head g ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = g.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on g.State_IdNo = DSH.State_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
        'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code , D.*, E.Ledger_Name AS agent_name from Garments_Sales_Return_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo LEFT OUTER JOIN  Transport_Head D ON A.Transport_IdNo = D.Transport_IdNo LEFT OUTER JOIN Ledger_Head E ON A.Agent_idno = E.Ledger_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
        prn_HdDt = New DataTable
        da1.Fill(prn_HdDt)

        '***** GST END *****
        If prn_HdDt.Rows.Count > 0 Then

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, b.Item_DisplayName, b.Item_Name_tamil, c.Size_Name as ItemSizeName ,u.Unit_name from Garments_Sales_Return_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head u on b.unit_idno = u.unit_idno LEFT OUTER JOIN Size_Head c on a.Size_idno = c.Size_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            prn_DetDt = New DataTable
            da2.Fill(prn_DetDt)

            If prn_DetDt.Rows.Count > 0 Then

                prn_DetMxIndx = 0
                For I = 0 To prn_DetDt.Rows.Count - 1

                    ItmNm1 = Trim(prn_DetDt.Rows(I).Item("Item_DisplayName").ToString)
                    ItmNm2 = ""
                    If Len(ItmNm1) > 30 Then
                        For K = 30 To 1 Step -1
                            If Mid$(Trim(ItmNm1), K, 1) = " " Or Mid$(Trim(ItmNm1), K, 1) = "," Or Mid$(Trim(ItmNm1), K, 1) = "." Or Mid$(Trim(ItmNm1), K, 1) = "-" Or Mid$(Trim(ItmNm1), K, 1) = "/" Or Mid$(Trim(ItmNm1), K, 1) = "_" Or Mid$(Trim(ItmNm1), K, 1) = "(" Or Mid$(Trim(ItmNm1), K, 1) = ")" Or Mid$(Trim(ItmNm1), K, 1) = "\" Or Mid$(Trim(ItmNm1), K, 1) = "[" Or Mid$(Trim(ItmNm1), K, 1) = "]" Or Mid$(Trim(ItmNm1), K, 1) = "{" Or Mid$(Trim(ItmNm1), K, 1) = "}" Then Exit For
                        Next K
                        If K = 0 Then K = 30
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - K)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), K)
                    End If

                    '***** GST START *****
                    prn_DetMxIndx = prn_DetMxIndx + 1
                    prn_DetAr(prn_DetMxIndx, 1) = Trim(Val(I) + 1)
                    prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm1)
                    prn_DetAr(prn_DetMxIndx, 3) = prn_DetDt.Rows(I).Item("HSN_Code").ToString
                    prn_DetAr(prn_DetMxIndx, 4) = Val(prn_DetDt.Rows(I).Item("Tax_Perc").ToString) & " %"
                    prn_DetAr(prn_DetMxIndx, 5) = Val(prn_DetDt.Rows(I).Item("Noof_Items").ToString)
                    prn_DetAr(prn_DetMxIndx, 6) = prn_DetDt.Rows(I).Item("ItemSizeName").ToString
                    prn_DetAr(prn_DetMxIndx, 7) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Rate").ToString), "########0.00"))
                    prn_DetAr(prn_DetMxIndx, 8) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Amount").ToString), "########0.00"))
                    prn_DetAr(prn_DetMxIndx, 10) = Trim(prn_DetDt.Rows(I).Item("Style_Name").ToString)
                    prn_DetAr(prn_DetMxIndx, 9) = ""
                    prn_DetAr(prn_DetMxIndx, 11) = Val(prn_DetDt.Rows(I).Item("Bags").ToString)
                    prn_TOTBOX = prn_TOTBOX + Val(prn_DetDt.Rows(I).Item("Bags").ToString)
                    '***** GST END *****

                    If Trim(ItmNm2) <> "" Then
                        prn_DetMxIndx = prn_DetMxIndx + 1
                        prn_DetAr(prn_DetMxIndx, 1) = ""
                        prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm2)
                        prn_DetAr(prn_DetMxIndx, 3) = ""
                        prn_DetAr(prn_DetMxIndx, 4) = ""
                        prn_DetAr(prn_DetMxIndx, 5) = ""
                        prn_DetAr(prn_DetMxIndx, 6) = ""
                        '***** GST START *****
                        prn_DetAr(prn_DetMxIndx, 7) = ""
                        prn_DetAr(prn_DetMxIndx, 8) = ""
                        prn_DetAr(prn_DetMxIndx, 9) = "ITEM_2ND_LINE"
                        prn_DetAr(prn_DetMxIndx, 10) = ""
                        prn_DetAr(prn_DetMxIndx, 11) = ""
                        '***** GST END *****
                    End If

                    If Trim(prn_DetDt.Rows(I).Item("Serial_No").ToString) <> "" Then

                        Erase BlNoAr
                        BlNoAr = New String(20) {}

                        m1 = 0
                        bln = "S/No : " & Trim(prn_DetDt.Rows(I).Item("Serial_No").ToString)

LOOP1:
                        If Len(bln) > 47 Then
                            For K = 47 To 1 Step -1
                                If Mid$(bln, K, 1) = " " Or Mid$(bln, K, 1) = "," Or Mid$(bln, K, 1) = "/" Or Mid$(bln, K, 1) = "\" Or Mid$(bln, K, 1) = "-" Or Mid$(bln, K, 1) = "." Or Mid$(bln, K, 1) = "&" Or Mid$(bln, K, 1) = "_" Then Exit For
                            Next K
                            If K = 0 Then K = 47
                            m1 = m1 + 1
                            BlNoAr(m1) = Microsoft.VisualBasic.Left(Trim(bln), K)
                            'BlNoAr(m1) = Microsoft.VisualBasic.Left(Trim(bln), K - 1)
                            bln = Microsoft.VisualBasic.Right(bln, Len(bln) - K)
                            If Len(bln) <= 47 Then
                                m1 = m1 + 1
                                BlNoAr(m1) = bln
                            Else
                                GoTo LOOP1
                            End If

                        Else
                            m1 = m1 + 1
                            BlNoAr(m1) = bln

                        End If

                        For K = 1 To m1
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = ""
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(BlNoAr(K))
                            prn_DetAr(prn_DetMxIndx, 3) = ""
                            prn_DetAr(prn_DetMxIndx, 4) = ""
                            prn_DetAr(prn_DetMxIndx, 5) = ""
                            prn_DetAr(prn_DetMxIndx, 6) = ""
                            '***** GST START *****
                            prn_DetAr(prn_DetMxIndx, 7) = ""
                            prn_DetAr(prn_DetMxIndx, 8) = ""
                            prn_DetAr(prn_DetMxIndx, 9) = "SERIALNO"
                            prn_DetAr(prn_DetMxIndx, 10) = ""
                            prn_DetAr(prn_DetMxIndx, 11) = ""
                            '***** GST END *****
                        Next K

                    End If

                Next I

            End If

            Prn_Cnt_Temp = 0


        Else
            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End If

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_GST_Format1(e)
        'Printing_Format2_1005(e)

    End Sub


    Private Sub txt_TradeDiscPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TradeDiscPerc.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub cbo_EntType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntType, dtp_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_EntType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntType, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Qty As Single, Ent_Rate As Single, Ent_PurcRet_Qty As Single
        Dim Ent_DetSlNo As Long

        If Trim(UCase(cbo_EntType.Text)) <> "ORDER" Then
            MessageBox.Show("Invalid Type", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
            Exit Sub
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, e.Size_Name, f.Noof_Items as Ent_Sales_Quantity, f.Rate as Ent_Rate, f.Sales_Detail_SlNo as Ent_Sales_SlNo from Sales_Order_Details a INNER JOIN Item_Head b ON a.Item_idno = b.Item_idno  LEFT OUTER JOIN Size_Head e ON a.Size_IdNo = e.Size_IdNo LEFT OUTER JOIN Garments_Sales_Return_Details F ON f.Sales_Code = '" & Trim(NewCode) & "' and f.Entry_Type = '" & Trim(cbo_EntType.Text) & "' and a.Sales_Order_Code = f.Sales_Order_Code and a.Sales_Order_Detail_SlNo = f.Sales_Order_Detail_SlNo Where a.ledger_idno = " & Str(Val(LedIdNo)) & " and ( (a.Noof_Items  - a.Sales_Items ) > 0 or f.Noof_Items > 0 ) Order by a.For_OrderBy, a.Sales_Order_No, a.Sales_Order_Detail_SlNo", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Ent_Qty = 0 : Ent_Rate = 0 : Ent_DetSlNo = 0 : Ent_PurcRet_Qty = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Sales_SlNo").ToString) = False Then Ent_DetSlNo = Val(Dt1.Rows(i).Item("Ent_Sales_SlNo").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Sales_Quantity").ToString) = False Then Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Sales_Quantity").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    ' If IsDBNull(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString) = False Then Ent_PurcRet_Qty = Val(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString)

                    If (Val(Dt1.Rows(i).Item("Noof_Items").ToString) - Val(Dt1.Rows(i).Item("Sales_Items").ToString) + Ent_Qty) > 0 Then

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Sales_Order_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Item_name").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Size_Name").ToString
                        .Rows(n).Cells(4).Value = (Val(Dt1.Rows(i).Item("Noof_Items").ToString) - Val(Dt1.Rows(i).Item("Sales_Items").ToString) + Ent_Qty)
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Rate").ToString), "########0.00")
                        If Val(Ent_Qty) > 0 Then
                            .Rows(n).Cells(6).Value = "1"
                        Else
                            .Rows(n).Cells(6).Value = ""
                        End If
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Sales_Order_Code").ToString
                        .Rows(n).Cells(8).Value = Val(Dt1.Rows(i).Item("Sales_Order_Detail_SlNo").ToString)
                        .Rows(n).Cells(9).Value = Val(Ent_DetSlNo)
                        .Rows(n).Cells(10).Value = Val(Ent_Qty)
                        .Rows(n).Cells(11).Value = Val(Ent_Rate)
                        .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("Bags").ToString)

                        If Val(Ent_Qty) > 0 Then

                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    End If

                Next

            End If
            Dt1.Clear()

            If .Rows.Count = 0 Then
                n = .Rows.Add()
                .Rows(n).Cells(0).Value = "1"
            End If

        End With

        pnl_Selection.Visible = True
        pnl_Selection.BringToFront()
        pnl_Back.Enabled = False

        dgv_Selection.Focus()
        dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        dgv_Selection.CurrentCell.Selected = True

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Grid_Selection(e.RowIndex)
    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Val(.Rows(RwIndx).Cells(4).Value) = 0 And Trim(.Rows(RwIndx).Cells(7).Value) = "" Then Exit Sub

                'If Val(.Rows(RwIndx).Cells(15).Value) <> 0 Then
                '    MessageBox.Show("Already some items returned, cannot de-select.", "DOES NOT DE-SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    Exit Sub
                'End If

                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 0 Then

                    .Rows(RwIndx).Cells(6).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next

                Else
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        With dgv_Selection

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                    e.Handled = True
                    Grid_Selection(dgv_Selection.CurrentCell.RowIndex)
                End If
            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Ent_Qty As Single, Ent_Rate As Single

        dgv_Details.Rows.Clear()

        NoCalc_Status = True

        sno = 0

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then

                If Val(dgv_Selection.Rows(i).Cells(10).Value) <> 0 Then
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(10).Value)

                Else
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(4).Value)

                End If

                If Val(dgv_Selection.Rows(i).Cells(10).Value) <> 0 Then
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(10).Value)

                Else
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(5).Value)

                End If

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(12).Value
                dgv_Details.Rows(n).Cells(4).Value = Val(Ent_Qty)
                dgv_Details.Rows(n).Cells(5).Value = Val(Ent_Rate)
                dgv_Details.Rows(n).Cells(6).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(8).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value
                '   dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(9).Value

            End If

        Next i

        NoCalc_Status = False



        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        'txt_BillNo.Focus()
        'cbo_EntType.Enabled = False

        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
            dgv_Details.CurrentCell.Selected = True
            cbo_EntType.Enabled = False
            Panel2.Enabled = False
        Else
            txt_CashDiscPerc.Focus()
            'txt_TradeDiscPerc.Focus()

        End If

    End Sub

    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_EntType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntType.TextChanged
        If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
            Panel2.Enabled = True
            dgv_Details.EditMode = DataGridViewEditMode.EditProgrammatically
            dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Else
            Panel2.Enabled = False
            dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
            dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
        End If
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = Nothing
        If dgv_Details.CurrentCell.ColumnIndex = 4 Or dgv_Details.CurrentCell.ColumnIndex = 5 Then
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        End If
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then
                    If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Siz_idno As Integer = 0
        Dim sqft_qty As Single = 0


        With dgv_Details
            If .Visible Then

                If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
                    e.Handled = True
                End If
                If .CurrentCell.ColumnIndex = 4 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = 5 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
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

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            ElseIf ActiveControl.Name = dgv_BaleDetails.Name Then
                dgv1 = dgv_BaleDetails

            ElseIf dgv_BaleDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BaleDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_BaleDetails.Name.ToString)) Then
                dgv1 = dgv_BaleDetails

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= 5 Then

                        If .CurrentCell.RowIndex >= .Rows.Count - 1 Then

                            txt_AddLess.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(4)


                        End If


                    ElseIf .CurrentCell.ColumnIndex < 4 Then
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(4)

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.ColumnIndex <= 4 Then
                        If .CurrentCell.RowIndex = 0 Then
                            If Panel2.Enabled = True And cbo_ItemName.Enabled = True Then
                                cbo_ItemName.Focus()

                            Else
                                cbo_EntType.Focus()

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(5)

                        End If


                    ElseIf .CurrentCell.ColumnIndex > 5 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)

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

    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, txt_OrderDate, txt_Electronic_RefNo, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, txt_Electronic_RefNo, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub cbo_PaymentMethod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentMethod.LostFocus
        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If
    End Sub
    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportMode, txt_BillDate, cbo_VehicleNo, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportMode.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportMode, cbo_VehicleNo, "", "", "", "", False)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_RateFor_Pcs_OR_Box, txt_SlNo, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_SlNo, "", "", "", "", True)
            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
                    cbo_TaxType.Tag = cbo_TaxType.Text
                    Amount_Calculation(True)
                    Amount_Calculation(False)
                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_RateFor_Pcs_OR_Box_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RateFor_Pcs_OR_Box.GotFocus
        cbo_RateFor_Pcs_OR_Box.Tag = cbo_RateFor_Pcs_OR_Box.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_RateFor_Pcs_OR_Box_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RateFor_Pcs_OR_Box.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_Noof_Bundles, txt_SlNo, "", "", "", "")
    End Sub

    Private Sub cbo_RateFor_Pcs_OR_Box_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RateFor_Pcs_OR_Box.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_SlNo, "", "", "", "", True)
        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_RateFor_Pcs_OR_Box_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RateFor_Pcs_OR_Box.LostFocus
        Try
            If Trim(UCase(cbo_RateFor_Pcs_OR_Box.Tag)) <> Trim(UCase(cbo_RateFor_Pcs_OR_Box.Text)) Then
                cbo_RateFor_Pcs_OR_Box.Tag = cbo_RateFor_Pcs_OR_Box.Text
                Amount_Calculation(True)
                Amount_Calculation(False)
                cbo_RateFor_Pcs_OR_Box.Tag = cbo_RateFor_Pcs_OR_Box.Text
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_RateFor_Pcs_OR_Box_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RateFor_Pcs_OR_Box.SelectedIndexChanged
        cbo_RateFor_Pcs_OR_Box_LostFocus(sender, e)
    End Sub

    Private Sub cbo_TaxType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.LostFocus
        Try
            If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
                cbo_TaxType.Tag = cbo_TaxType.Text
                Amount_Calculation(True)
                Amount_Calculation(False)
                cbo_TaxType.Tag = cbo_TaxType.Text
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
        cbo_TaxType_LostFocus(sender, e)
    End Sub


    Private Sub btn_GSTTax_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_GSTTax_Details.Click
        pnl_GSTTax_Details.Visible = True
        pnl_Back.Enabled = False
        pnl_GSTTax_Details.Focus()
    End Sub


    Private Sub btn_Close_GSTTax_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_GSTTax_Details.Click
        pnl_Back.Enabled = True
        pnl_GSTTax_Details.Visible = False
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            Get_PriceList_Rate()
            Amount_Calculation(True)
        End If
    End Sub

    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Garments_Sales_Return_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
        'Da = New SqlClient.SqlDataAdapter("Select * from Garments_Sales_Return_GST_Tax_Details Where Sales_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            NoofHsnCodes = Dt1.Rows.Count
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da.Dispose()

        get_GST_Noof_HSN_Codes_For_Printing = NoofHsnCodes

    End Function


    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Cmd.Connection = con

        Cmd.CommandText = "Truncate table EntryTempSub "
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into EntryTempSub (Meters1, Currency1) select (CGST_Percentage+SGST_Percentage), (CGST_Amount+SGST_Amount) from Garments_Sales_Return_GST_Tax_Details  Where Sales_Code = '" & Trim(EntryCode) & "' and (CGST_Amount+SGST_Amount) <> 0"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into EntryTempSub (Meters1, Currency1) select IGST_Percentage, IGST_Amount from Garments_Sales_Return_GST_Tax_Details  Where Sales_Code = '" & Trim(EntryCode) & "' and IGST_Amount <> 0"
        Cmd.ExecuteNonQuery()

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select Meters1, sum(Currency1) from EntryTempSub Group by Meters1 Having sum(Currency1) <> 0", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Garments_Sales_Return_GST_Tax_Details Where Sales_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
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

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        With cbo_Style
            '  vcmb_SizNm = Trim(.Text)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Garments_Sales_Return_Head", "Vehicle_No", "", "")
        End With

    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_TransportMode, cbo_Transport, "Garments_Sales_Return_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, cbo_Transport, "Garments_Sales_Return_Head", "Vehicle_No", "", "", False)
    End Sub
    Private Sub cbo_AgentName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_AgentName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_AgentName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AgentName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_Electronic_RefNo, txt_SlNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_AgentName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AgentName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_SlNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DateTime_Of_Supply.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True : SendKeys.Send("+{TAB}")
        End If
        If e.KeyValue = 40 Then
            cbo_ItemName.Focus()
        End If

    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DateTime_Of_Supply.KeyPress
        If Asc(e.KeyChar) = 13 Then

            cbo_ItemName.Focus()
        End If

    End Sub

    Private Sub cbo_Style_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Style.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Style_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_LessFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LessFor.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : txt_Freight.Focus()
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub


    Private Sub txt_LessFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LessFor.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub




    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, txt_OrderDate, cbo_TransportMode, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_TransportMode, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
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





    Private Sub cbo_AgentName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_AgentName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "AGENT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_AgentName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Printing_Format2_1005(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 40
            .Top = 40 ' 50 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Bold)
        'pFont = New Font("Calibri", 9, FontStyle.Regular)

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
        TxtHgt = 18.5 '20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 15 '10 

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 35 : ClArr(2) = 170 : ClArr(3) = 70 : ClArr(4) = 40
        ClArr(5) = 55
        ClArr(6) = 60 : ClArr(7) = 60 : ClArr(8) = 70 : ClArr(9) = 70
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                '***** GST START *****
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 7
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                '***** GST END *****

                Printing_Format2_1005_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0

                    CurY = CurY + TxtHgt - 20
                    'Common_Procedures.Print_To_PrintDocument(e, "100% COTTON GOODS HOSIERY", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

                    If prn_Count > 1 Then
                        CurY = CurY
                    Else
                        CurY = CurY - TxtHgt
                    End If

                    If prn_DetMxIndx > 0 Then

                        Do While DetIndx <= prn_DetMxIndx

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_Format2_1005_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                e.HasMorePages = True

                                Return

                            End If

                            CurY = CurY + TxtHgt - 5

                            '***** GST START *****
                            'If DetIndx <> 1 And Val(prn_DetAr(DetIndx, 1)) <> 0 Then
                            '    CurY = CurY + 2
                            'End If
                            '***** GST END *****

                            If Trim(prn_DetAr(DetIndx, 2)) <> "" And Trim(prn_DetAr(DetIndx, 9)) = "SERIALNO" Then
                                CurY = CurY - 3
                                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 25, CurY, 0, 0, p1Font)

                            ElseIf Trim(prn_DetAr(DetIndx, 2)) <> "" And Trim(prn_DetAr(DetIndx, 9)) = "ITEM_2ND_LINE" Then
                                CurY = CurY - 3
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 25, CurY, 0, 0, pFont)


                            Else
                                '***** GST START *****
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 3), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 10), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 2, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 5), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 10, CurY, 1, 0, pFont)
                                '***** GST END *****
                            End If

                            NoofDets = NoofDets + 1

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format2_1005_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 1
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

    Private Sub Printing_Format2_1005_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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
        Dim Yinc As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Garments_Sales_Return_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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
        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
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

        '***** GST START *****
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
        '***** GST END *****

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
            p1Font = New Font("Calibri", 20, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Jeno_Garments, Drawing.Image), LMargin + 15, CurY - 70, 120, 80)

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
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST END *****

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
                PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

                If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
                If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
                If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
                If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
                If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
                If UBound(PnAr) >= 5 Then Led_State = Trim(PnAr(5))
                If UBound(PnAr) >= 6 Then Led_PhNo = Trim(PnAr(6))
                If UBound(PnAr) >= 7 Then Led_GSTTinNo = Trim(PnAr(7))


            Else

                Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
                Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)
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


            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

            W1 = e.Graphics.MeasureString("PAYMENT  TERMS :", pFont).Width
            W2 = e.Graphics.MeasureString("ORDER  DATE : ", pFont).Width + 50
            Yinc = 0
            'Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            'W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            'W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY

            '***** GST START *****
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + 30, CurY, 0, 0, pFont)

            If Trim(LedNmAr(8)) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(8)), LMargin + 30, CurY, 0, 0, pFont)
            End If




            '------------------ Invoice No Block

            '***** GST START *****
            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + Cen1 + W1 + W2, BlockInvNoY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 20, BlockInvNoY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + W2 + 30, BlockInvNoY + Yinc, 0, 0, pFont)

            BlockInvNoY = BlockInvNoY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "ORDER NO.", LMargin + Cen1 + 10, BlockInvNoY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, BlockInvNoY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY + Yinc, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Date.", LMargin + Cen1 + W1 + W2, BlockInvNoY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 20, BlockInvNoY + Yinc, 0, 0, pFont)
            If IsDate(prn_HdDt.Rows(0).Item("Order_Date").ToString) Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Order_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + W2 + 30, BlockInvNoY + Yinc, 0, 0, pFont)
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then
                BlockInvNoY = BlockInvNoY + TxtHgt

                Common_Procedures.Print_To_PrintDocument(e, "LR NO.", LMargin + Cen1 + 10, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY + Yinc, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Date.", LMargin + Cen1 + W1 + W2, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 20, BlockInvNoY + Yinc, 0, 0, pFont)
                If IsDate(prn_HdDt.Rows(0).Item("Lr_Date").ToString) Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Lr_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + W2 + 30, BlockInvNoY + Yinc, 0, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then
                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + Cen1 + 10, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + Cen1 + W1 + 30, BlockInvNoY + Yinc, 0, 0, pFont)
            End If


            If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "GOODS TO", LMargin + Cen1 + 10, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY + Yinc, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then

                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "No.of BUNDLES", LMargin + Cen1 + 10, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bags").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY + Yinc, 0, 0, pFont)

                If Val(prn_HdDt.Rows(0).Item("Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + Cen1 + W1 + W2, BlockInvNoY + Yinc, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 60, BlockInvNoY + Yinc, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weight").ToString, LMargin + Cen1 + W1 + W2 + 60, BlockInvNoY + Yinc, 0, 0, pFont)
                End If


                'Common_Procedures.Print_To_PrintDocument(e, "FREIGHT", LMargin + Cen1 + W1 + W2, BlockInvNoY + Yinc, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 60, BlockInvNoY + Yinc, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString, LMargin + Cen1 + W1 + W2 + 60, BlockInvNoY + Yinc, 0, 0, pFont)

            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)


            If Trim(prn_HdDt.Rows(0).Item("agent_name").ToString) <> "" Then
                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "AGENT", LMargin + Cen1 + 10, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, BlockInvNoY + Yinc, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("agent_name").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY + Yinc, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> "" Then
                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Payment Terms", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
                BlockInvNoY = BlockInvNoY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            End If

            'If Trim(prn_HdDt.Rows(0).Item("Document_Through").ToString) <> "" Then
            '    BlockInvNoY = BlockInvNoY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Doc.Through", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Document_Through").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY, 0, 0, pFont)
            'End If

            '***** GST END *****
            '---------------------------

            CurY = BlockInvNoY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))

            '***** GST START *****
            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "STYLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)



            'If Trim(prn_HdDt.Rows(0).Item("Pcs_or_Box").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Pcs_or_Box").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)
            'Else
            '    'Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Unit_name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)
            'End If

            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY - 5, 2, ClAr(7) + ClAr(8), pFont)

            LnAr(9) = CurY + TxtHgt - 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(9), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(9))

            Common_Procedures.Print_To_PrintDocument(e, "BOXS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt + 3, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt + 3, 2, ClAr(8), pFont)


            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            If Trim(prn_HdDt.Rows(0).Item("Pcs_or_Box").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "/" & Trim(prn_HdDt.Rows(0).Item("Pcs_or_Box").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            '***** GST END *****

            CurY = CurY + TxtHgt + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_1005_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)

        Dim p1Font, pfont1 As Font
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

            ' Common_Procedures.Print_To_PrintDocument(e, "BUNDLES : " & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                'If Trim(prn_HdDt.Rows(0).Item("Pcs_or_Box").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Pcs_or_Box").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                'End If
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))


            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt - 10
                    'If Val(prn_PageNo) = 1 Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                    Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 11, FontStyle.Bold)
                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                End If

            End If


            CurY = CurY - 10

            '***** GST START *****
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Extra_Copies").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                    End If
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Extra_Copies").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Trade Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("Extra_Charges").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Extra_Copies").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1137" Then '----NATRAJ KNIT WEAR (TIRUPUR)
                        Common_Procedures.Print_To_PrintDocument(e, "Scheme Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)

                End If
            End If

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Total_Extra_Copies").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, p1Font)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add : CGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Add : CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If


            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add : SGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Add : SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If


            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add : IGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Add : IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                End If
            End If


            '***** GST END *****

            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            'If Val(prn_HdDt.Rows(0).Item("LessFor").ToString) = 0 Then

            CurY = CurY + TxtHgt - 10

            pfont1 = New Font("Calibri", 10, FontStyle.Bold)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pfont1)

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pfont1)
            End If

            'Else
            '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
            '    CurY = CurY + TxtHgt - 10


            '    Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
            '    End If


            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Less F.O.R", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    If is_LastPage = True Then
            '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
            '        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("LessFor").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
            '    End If
            '    CurY = CurY + TxtHgt + 5
            '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)

            '    CurY = CurY + TxtHgt - 10
            '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "Payable Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
            '    If is_LastPage = True Then
            '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
            '        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(prn_HdDt.Rows(0).Item("LessFor").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, p1Font)
            '    End If

            'End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5))

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

            '***** GST START *****
            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            '***** GST START *****
            '=============GST SUMMARY============
            'vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            'If vNoofHsnCodes <> 0 Then
            '    printing_GST_HSN_Details_Format4(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            'End If

            p1Font = New Font("Calibri", 9, FontStyle.Regular Or FontStyle.Underline)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 8, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "1.Payment Should Be Made Within Due Date.", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "2.Payment Should Be Paid By Cheque Or Draft Payeable at Coimbatore", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "3.Overdue Interest will be charged at 24% from The invoice date.", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "4.Subject to Coimbatore jurisdiction Only ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            '==========================
            '***** GST END *****

            CurY = CurY + 5
            'CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1), CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            Jurs = Common_Procedures.settings.Jurisdiction
            If Trim(Jurs) = "" Then Jurs = "Coimbatore"


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub txt_Noof_Boxs_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Noof_Boxs.KeyDown
        If e.KeyCode = 40 Then
            'e.Handled = True
            ' txt_NoofPcs.Focus()
            If (Trim(UCase(cbo_Type.Text)) <> "DIRECT") Then
                txt_Rate.Focus()
            Else
                txt_NoofPcs.Focus()
            End If

        End If
        If e.KeyCode = 38 Then
            If (Trim(UCase(cbo_Type.Text)) <> "DIRECT") Then
                cbo_TaxType.Focus()
            Else
                cbo_Size.Focus()
            End If
        End If ' e.Handled = True : cbo_Size.Focus()
    End Sub

    Private Sub txt_Noof_Boxs_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Noof_Boxs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True

            If (Trim(UCase(cbo_Type.Text)) <> "DIRECT") Then
                txt_Rate.Focus()
            Else
                txt_NoofPcs.Focus()
            End If

        End If
    End Sub

    Private Sub txt_Freight_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : txt_TradeDiscPerc.Focus()
        If e.KeyCode = 40 Then
            e.Handled = True : txt_PreparedBy.Focus()
            'If txt_LessFor.Visible = False Then
            '    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '        save_record()
            '    Else
            '        dtp_Date.Focus()
            '    End If
            'Else
            '    txt_LessFor.Focus()
            'End If

        End If
    End Sub

    Private Sub Get_PriceList_Rate()
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim Led_ID As Integer
        Dim Itm_ID As Integer


        Try

            Led_ID = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Ledger.Text)))
            Itm_ID = Val(Common_Procedures.Item_NameToIdNo1(con, Trim(cbo_ItemName.Text)))

            If Itm_ID = 0 Or Led_ID = 0 Then Exit Sub


            cmd.Connection = con
            cmd.CommandText = "select a.Rate  FROM Garments_Price_List_Details a INNER JOIN Ledger_Head B ON A.Price_List_IdNo = B.PriceList_IdNo WHERE b.Ledger_IdNo  = " & Val(Led_ID) & " and a.Item_IdNo = " & Val(Itm_ID)
            dr = cmd.ExecuteReader


            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then

                        txt_Rate.Text = Val(dr(0).ToString)

                    End If
                End If

            End If

            dr.Close()



        Catch ex As Exception
            MessageBox.Show(ex.Message, "RATE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub txt_CashDiscPerc_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_CashDiscPerc.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : cbo_ItemName.Focus()
        If e.KeyValue = 40 Then e.Handled = True : txt_TradeDiscPerc.Focus()
    End Sub



    Private Sub txt_Noof_Boxs_TextChanged(sender As Object, e As EventArgs) Handles txt_Noof_Boxs.TextChanged
        Call Amount_Calculation(False)
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
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim vNoofHsnCodes As Integer = 0
        Dim vLine_Pen As Pen

        If prn_PageNo <= 0 Then
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
            .Left = 40
            .Right = 40 ' 65 ' 40
            .Top = 30 '40 ' 50 ' 60
            .Bottom = 40
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

        NoofItems_PerPage = 10 ' 8

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(0) = 0
        ClAr(1) = 35 : ClAr(2) = 170 : ClAr(3) = 70 : ClAr(4) = 40
        ClAr(5) = 55
        ClAr(6) = 60 : ClAr(7) = 60 : ClAr(8) = 70 : ClAr(9) = 70
        ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))

        'ClAr(1) = Val(30) : ClAr(2) = 230 : ClAr(3) = 80 : ClAr(4) = 50 : ClAr(5) = 65 : ClAr(6) = 65 : ClAr(7) = 50 : ClAr(8) = 80
        'ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        TxtHgt = 17.5   '  18.5    19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        vLine_Pen = New Pen(Color.Black, 2)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                Printing_GST_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetMxIndx > 0 Then

                    Do While prn_DetIndx <= prn_DetMxIndx

                        If prn_PageNo <= 1 Then
                            'If prn_DetIndx = prn_DetDt.Rows.Count - 1 Then
                            '    NoofItems_PerPage = 20
                            'Else
                            NoofItems_PerPage = 18 ' 10
                            'End If

                        Else
                            'If prn_DetIndx = prn_DetDt.Rows.Count - 1 Then
                            '    NoofItems_PerPage = 50
                            'Else
                            NoofItems_PerPage = 38 '20 ' 30
                            'End If

                        End If

                        If vNoofHsnCodes = 0 Then
                            NoofItems_PerPage = NoofItems_PerPage + 5
                        Else
                            If vNoofHsnCodes > 1 Then NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                        End If

                        If prn_DetIndx >= (prn_DetMxIndx - 2) Then
                            Debug.Print(prn_DetIndx)
                        End If


                        If prn_DetIndx >= (prn_DetMxIndx - 2) Then

                            If (CurY + (19 * TxtHgt) + ((vNoofHsnCodes + 4) * (TxtHgt + 3))) >= (PageHeight - TxtHgt) Then

                                If CurY < (PageHeight - TxtHgt - TxtHgt) Then
                                    CurY = PageHeight - TxtHgt - TxtHgt
                                End If

                                CurY = CurY + 10 ' TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)

                                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                                e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                                e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))

                                'NoofDets = NoofDets + 1
                                'Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False, vLine_Pen)

                                e.HasMorePages = True
                                Return

                            End If


                        ElseIf CurY >= (PageHeight - TxtHgt - TxtHgt) Then

                            If CurY < (PageHeight - TxtHgt - TxtHgt) Then
                                CurY = PageHeight - TxtHgt - TxtHgt
                            End If

                            CurY = CurY + 10 ' TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)

                            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

                            'NoofDets = NoofDets + 1
                            'Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False, vLine_Pen)

                            e.HasMorePages = True
                            Return

                        End If

                        CurY = CurY + TxtHgt - 5

                        If Trim(prn_DetAr(prn_DetIndx, 2)) <> "" And Trim(prn_DetAr(prn_DetIndx, 9)) = "SERIALNO" Then
                            CurY = CurY - 3
                            p1Font = New Font("Calibri", 8, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 2), LMargin + ClAr(1) + 25, CurY, 0, 0, p1Font)

                        ElseIf Trim(prn_DetAr(prn_DetIndx, 2)) <> "" And Trim(prn_DetAr(prn_DetIndx, 9)) = "ITEM_2ND_LINE" Then
                            CurY = CurY - 3
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 2), LMargin + ClAr(1) + 25, CurY, 0, 0, pFont)

                        Else

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 1), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 3), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 10), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 2, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 7), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 8), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)

                        End If

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1


                    Loop


                End If

                Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageHeight, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True, vLine_Pen)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, C2, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_PanNo As String
        Dim CurY1 As Single = 0, CurX As Single = 0
        Dim Cmp_StateCap As String = "", Cmp_StateNm As String = "", Cmp_StateCode As String = "", Cmp_GSTIN_Cap As String = "", Cmp_GSTIN_No As String = ""

        Try

            PageNo = PageNo + 1

            CurY = TMargin

            'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothSales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
            'da2.Fill(dt2)
            'If dt2.Rows.Count > NoofItems_PerPage Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            'End If
            'dt2.Clear()

            'prn_Count = prn_Count + 1

            'prn_OriDupTri = ""
            'If Trim(prn_InpOpts) <> "" Then
            '    If prn_Count <= Len(Trim(prn_InpOpts)) Then

            '        S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

            '        If Val(S) = 1 Then
            '            prn_OriDupTri = "ORIGINAL"
            '        ElseIf Val(S) = 2 Then
            '            prn_OriDupTri = "DUPLICATE"
            '        ElseIf Val(S) = 3 Then
            '            prn_OriDupTri = "TRIPLICATE"
            '        End If

            '    End If
            'End If

            'If Trim(prn_OriDupTri) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            'End If

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            ' If PageNo <= 1 Then


            Desc = ""
            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
            Cmp_StateCode = "" : Cmp_GSTIN_No = "" : Cmp_StateNm = ""

            Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If PageNo <= 1 Then

                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

                If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                    Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
                End If
                If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
                    Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
                End If
                If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                    Cmp_PhNo = "Phone: " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
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

                p1Font = New Font("Calibri", 15, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "SALES RETURN", LMargin, CurY, 2, PrintWidth, p1Font)
                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

                CurY = CurY + TxtHgt
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then '---- JENO TEX (SOMANUR)
                    If InStr(1, Trim(UCase(Cmp_Name)), "JENO") > 0 Then                                    '---- Jeno Textile Logo
                        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Jeno_Garments, Drawing.Image), LMargin + 20, CurY, 120, 85)
                    ElseIf InStr(1, Trim(UCase(Cmp_Name)), "ANNAI") > 0 Then                                          '---- Annai Tex Logo
                        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_AnnaiTex, Drawing.Image), LMargin + 20, CurY, 120, 85)
                    End If
                End If

            End If

            p1Font = New Font("Calibri", 17, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight - 10
            Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)
            If PageNo <= 1 Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

                CurY = CurY + TxtHgt

                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
                strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "  " & Cmp_StateCode & "    " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width

                If PrintWidth > strWidth Then
                    CurX = LMargin + (PrintWidth - strWidth) / 2
                Else
                    CurX = LMargin
                End If

                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)

                strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
                CurX = CurX + strWidth
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm & "  " & Cmp_StateCode, CurX, CurY, 0, 0, pFont)

                strWidth = e.Graphics.MeasureString(Cmp_StateNm & "  " & Cmp_StateCode, pFont).Width

                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                CurX = CurX + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)

                strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width

                CurX = CurX + strWidth

                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin + 10, CurY, 2, PrintWidth, pFont)
            End If



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY


            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("ELECTRONIC REF. NO. ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("Date and Time of Supply ", pFont).Width
            S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width

            CurY1 = CurY

            '-Left side

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " PHONE : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            'Right Side

            CurY1 = CurY1 + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, p1Font)

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 20, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RETURN NO", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "RETURN DATE", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 20, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "BILL NO ", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + W1 + 20, CurY1, 0, 0, pFont)

                CurY1 = CurY1 + TxtHgt

                Common_Procedures.Print_To_PrintDocument(e, "BILL DATE", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_date").ToString, LMargin + C1 + W1 + 20, CurY1, 0, 0, pFont)



            End If

            '  CurY1 = CurY1 + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "No.Of Bundles", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bags").ToString, LMargin + C1 + W1 + 20, CurY1, 0, 0, pFont)
            'End If

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N)", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "N", LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)

            'p1Font = New Font("Calibri", 11, FontStyle.Bold)
            'CurY1 = CurY1 + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Due Days", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)
            'End If

            'If prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString <> "" Then
            '    CurY1 = CurY1 + TxtHgt

            '    Common_Procedures.Print_To_PrintDocument(e, "EWAY BILLNO.", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)

            'End If

            'If CurY1 > CurY Then CurY = CurY1


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

            LnAr(3) = CurY

            CurY1 = CurY
            '-Left Side
            'If PageNo <= 1 Then
            '    CurY = CurY + 10
            '    Common_Procedures.Print_To_PrintDocument(e, "Agent Name ", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("agent_name").ToString, LMargin + S2 + 20, CurY, 0, 0, pFont)

            '    CurY = CurY + TxtHgt
            '    If prn_HdDt.Rows(0).Item("Order_No").ToString <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Order No ", LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + S2 + 20, CurY, 0, 0, pFont)
            '        If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
            '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + S2 + 30 + e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width, CurY, 0, 0, pFont)
            '        End If
            '    End If

            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Doc.Through ", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Document_Through").ToString, LMargin + S2 + 20, CurY, 0, 0, pFont)



            '    'Right Side

            '    CurY1 = CurY1 + 10
            '    Common_Procedures.Print_To_PrintDocument(e, "Transport Mode ", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W2 + 20, CurY1, 0, 0, pFont)

            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W2 + 20, CurY1, 0, 0, pFont)

            '    'CurY1 = CurY1 + TxtHgt
            '    'Common_Procedures.Print_To_PrintDocument(e, "Date and Time of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C2 + W2 + 20, CurY1, 0, 0, pFont)

            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Lr.No  ", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C2 + W2 + 20, CurY1, 0, 0, pFont)
            '    If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C2 + W2 + 30 + e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, p1Font).Width, CurY1, 0, 0, pFont)
            '    End If


            '    If CurY1 > CurY Then CurY = CurY1

            '    CurY = CurY + TxtHgt
            '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            '    LnAr(4) = CurY

            'End If

            LnAr(4) = CurY


            CurY = CurY + 10

            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "STYLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)


            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY - 5, 2, ClAr(7) + ClAr(8), pFont)

            LnAr(9) = CurY + TxtHgt - 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(9), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(9))

            Common_Procedures.Print_To_PrintDocument(e, "BOXS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt + 3, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt + 3, 2, ClAr(8), pFont)


            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            If Trim(prn_HdDt.Rows(0).Item("Pcs_or_Box").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "/" & Trim(prn_HdDt.Rows(0).Item("Pcs_or_Box").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'CurY = CurY + 10
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, PageHeight As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vLine_Pen As Pen)
        Dim p1Font As Font
        Dim I As Integer
        Dim K As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BankNm5 As String = ""
        Dim NetBilTxt As String = ""
        Dim W2 As Single = 0
        Dim CurY1 As Single = 0
        Dim BnkDetAr() As String
        Dim BInc As Integer
        Dim PageClm_Width As Single = 0
        Dim vNoofHsnCodes As Integer = 0
        'Dim vLine_Pen As Pen
        Dim vGST_PERC_AMT_FOR_PRNT As String = ""
        Dim ar_GSTDET() As String, ar_GSTAMT() As String


        Try

            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            CurY = PageHeight - (20 * TxtHgt) - ((vNoofHsnCodes + 4) * (TxtHgt + 3))

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(6) = CurY

            CurY = CurY + 10

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_TOTBOX), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))

            If is_LastPage = True Then

                W2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + (ClAr(9) \ 2)

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

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        BankNm5 = Trim(BnkDetAr(BInc))
                    End If

                End If


                CurY = CurY + 5   ' TxtHgt
                If is_LastPage = True Then
                    If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString) & "%", LMargin + W2 - 20, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ ", LMargin + W2 - 20, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Extra_Copies").ToString) <> 0 Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Trade Discount @ " & Trim(prn_HdDt.Rows(0).Item("Extra_Charges").ToString) & "%", LMargin + W2 - 20, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "Trade Discount @ ", LMargin + W2 - 20, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Extra_Charges").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Extra_Copies").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                    End If

                End If


                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + W2 - 20, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Add/Less Amount", LMargin + W2 - 20, CurY, 1, 0, pFont)
                        If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) < 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Total_Extra_Copies").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                        CurY = CurY + TxtHgt
                        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, PageWidth, CurY)
                        CurY = CurY - 15
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString) <> 0 Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + W2 - 20, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                    End If

                    vGST_PERC_AMT_FOR_PRNT = get_GSTPercentage_and_GSTAmount_For_Printing(EntryCode)
                    'S = Trim(Dt1.Rows(I).Item("gsttaxcaption").ToString) & " " & Trim(Val(Dt1.Rows(I).Item("gstperc").ToString)) & "$^$" & Trim(Format(Val(Dt1.Rows(I).Item("gstamount").ToString), "##########0.00"))
                    'vRETSTR = Trim(vRETSTR) & IIf(Trim(vRETSTR) <> "", "#^#", "") & Trim(S)
                    If Trim(vGST_PERC_AMT_FOR_PRNT) <> "" Then

                        ar_GSTDET = Split(vGST_PERC_AMT_FOR_PRNT, "#^#")

                        For K = 0 To UBound(ar_GSTDET)
                            If Trim(ar_GSTDET(K)) <> "" Then
                                ar_GSTAMT = Split(ar_GSTDET(K), "$^$")
                                If Val(ar_GSTAMT(1)) <> 0 Then
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ar_GSTAMT(0)), LMargin + W2 - 20, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(ar_GSTAMT(1)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                                End If

                            End If
                        Next K

                    End If

                End If


                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 11, FontStyle.Bold)


                If is_LastPage = True Then
                    If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + W2 - 20, CurY, 1, 0, pFont)
                        If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) < 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Round_Off").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                    End If
                End If



                p1Font = New Font("Calibri", 9, FontStyle.Regular Or FontStyle.Underline)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY1, 0, 0, p1Font)
                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "1.Payment Should Be Made Within Due Date.", LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "2.Payment Should Be Paid By Cheque Or Draft Payeable at Coimbatore", LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "3.Overdue Interest will be charged at 24% from The invoice date.", LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "4.Subject to Coimbatore jurisdiction Only ", LMargin + 10, CurY1, 0, 0, p1Font)

                If CurY1 > CurY Then CurY = CurY1

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, PageWidth, CurY)
                LnAr(8) = CurY

                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                CurY = CurY + 10

                Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 20, CurY, 0, 0, pFont)


                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + W2 - 20, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(9))
                LnAr(9) = CurY

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
                CurY = CurY + 5

                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & StrConv(BmsInWrds, VbStrConv.ProperCase) & " ", LMargin + 10, CurY, 0, 0, p1Font)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(10) = CurY

                CurY = CurY + TxtHgt - 10


                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1197" Then '---- SATHIS TEXTILES (VAGARAYAMPALAYAM)
                '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
                '    'CurY1 = CurY1 + 10
                '    'Common_Procedures.Print_To_PrintDocument(e, "No.of Bundles : " & Trim(Val(prn_HdDt.Rows(0).Item("Noof_Bundles").ToString)), LMargin + 10, CurY1, 0, 0, pFont)
                '    CurY1 = CurY1 + TxtHgt + 10
                '    Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p1Font)
                '    CurY1 = CurY1 + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p1Font)
                '    CurY1 = CurY1 + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p1Font)
                '    CurY1 = CurY1 + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p1Font)
                'End If


                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
                If vNoofHsnCodes <> 0 Then
                    Printing_GST_Format1_HSN_Details(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), vLine_Pen)
                End If
                LnAr(12) = CurY


                PageClm_Width = PrintWidth / 3

                CurY = CurY + 5

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)


                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

                CurY = CurY + TxtHgt

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)


                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(prn_HdDt.Rows(0).Item("Prepared_By").ToString) & ")", LMargin + 250, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm5, LMargin + 10, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 250, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 400, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(11) = CurY

                p1Font = New Font("Calibri", 12, FontStyle.Bold)


                e.Graphics.DrawLine(Pens.Black, LMargin + 235, CurY, LMargin + 235, LnAr(12))


                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            Else


                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub Printing_GST_Format1_HSN_Details(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByRef CurY As Single, ByRef TopLnYAxis As Single, ByVal vLine_Pen As Pen)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim I As Integer = 0
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim SNo As Integer = 0
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String = ""
        Dim vINDX As Integer = 0

        Try

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 100 : SubClAr(2) = 100 : SubClAr(3) = 45 : SubClAr(4) = 90 : SubClAr(5) = 45 : SubClAr(6) = 90 : SubClAr(7) = 45 : SubClAr(8) = 90
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "HSN", LMargin, CurY, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + SubClAr(1), CurY, 2, SubClAr(2), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin, CurY + 15, 2, SubClAr(1), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 15, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY)
            LnAr2 = CurY
            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin, CurY, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1), CurY, 2, SubClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TAX AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            Da = New SqlClient.SqlDataAdapter("select * from Garments_Sales_Return_GST_Tax_Details where Sales_Code = '" & Trim(EntryCode) & "' order by HSN_Code, CGST_Percentage, SGST_Percentage, IGST_Percentage", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then




                vINDX = 0

                CurY = CurY - 15

                Do While vINDX <= Dt.Rows.Count - 1

                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(Dt.Rows(vINDX).Item("HSN_Code").ToString), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(vINDX).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(vINDX).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(vINDX).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(vINDX).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(vINDX).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(vINDX).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(vINDX).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(vINDX).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(vINDX).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(vINDX).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(vINDX).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(vINDX).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(vINDX).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(vINDX).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Dt.Rows(vINDX).Item("SGST_Amount").ToString) + Val(Dt.Rows(vINDX).Item("SGST_Amount").ToString) + Val(Dt.Rows(vINDX).Item("IGST_Amount").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(vINDX).Item("Taxable_Amount").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(vINDX).Item("CGST_Amount").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(vINDX).Item("SGST_Amount").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(vINDX).Item("IGST_Amount").ToString)

                    vINDX = vINDX + 1

                Loop

            End If

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

            'CurY = CurY + 5
            'BmsInWrds = ""
            'If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
            '    BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            'End If

            'p1Font = New Font("Calibri", 10, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Function get_GSTPercentage_and_GSTAmount_For_Printing(ByVal EntryCode As String) As String
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vRETSTR As String = ""
        Dim S As String = ""
        Dim Nr As Long

        Cmd.Connection = con

        Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSubTable) & " "
        Nr = Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " (Int1, Name1, Meters1, Currency1) select 1, 'CGST @', CGST_Percentage, CGST_Amount from Garments_Sales_Return_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "' and CGST_Amount <> 0"
        Nr = Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " (Int1, Name1, Meters1, Currency1) select 2, 'SGST @', SGST_Percentage, SGST_Amount from Garments_Sales_Return_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "' and SGST_Amount <> 0"
        Nr = Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " (Int1, Name1, Meters1, Currency1) select 3, 'IGST @', IGST_Percentage, IGST_Amount from Garments_Sales_Return_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "' and IGST_Amount <> 0"
        Nr = Cmd.ExecuteNonQuery()

        vRETSTR = ""
        Da = New SqlClient.SqlDataAdapter("Select Int1, Name1 as gsttaxcaption, Meters1 as gstperc, sum(Currency1) as gstamount from " & Trim(Common_Procedures.EntryTempSubTable) & " Group by Int1, Name1, Meters1 Having sum(Currency1) <> 0 Order  by Meters1, Int1, Name1  ", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                If Val(Dt1.Rows(i).Item("gstamount").ToString) <> 0 Then

                    S = Trim(Dt1.Rows(i).Item("gsttaxcaption").ToString) & " " & Trim(Val(Dt1.Rows(i).Item("gstperc").ToString)) & " % " & "$^$" & Trim(Format(Val(Dt1.Rows(i).Item("gstamount").ToString), "##########0.00"))

                    vRETSTR = Trim(vRETSTR) & IIf(Trim(vRETSTR) <> "", "#^#", "") & Trim(S)

                End If
            Next i
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da.Dispose()

        get_GSTPercentage_and_GSTAmount_For_Printing = Trim(vRETSTR)

    End Function

    Private Sub txt_PreparedBy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PreparedBy.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If txt_LessFor.Visible = False Then
                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
            Else
                txt_LessFor.Focus()
            End If

        End If
    End Sub

    Private Sub get_Item_Details()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim item_id As Integer
        item_id = 0

        item_id = Common_Procedures.Item_NameToIdNo1(con, cbo_ItemName.Text)

        Da = New SqlClient.SqlDataAdapter("select a.*, c.Size_name , sh.Style_Name from item_head a LEFT OUTER JOIN Size_Head c on a.Item_Size_IdNo = c.Size_idno LEFT OUTER JOIN Style_Head sh ON a.Item_Style_IdNo = sh.Style_IdNo where a.item_idno = " & Str(Val(item_id)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)("Size_name").ToString) = False Then
                cbo_Size.Text = Dt1.Rows(0)("Size_name").ToString
            End If
            cbo_Style.Text = Dt1.Rows(0)("Style_Name").ToString
        End If


    End Sub

    ''Private Sub btn_OrderSelection_Click(sender As Object, e As EventArgs) Handles btn_OrderSelection.Click


    ''    Dim Da As New SqlClient.SqlDataAdapter
    ''    Dim Dt1 As New DataTable
    ''    Dim Dt2 As New DataTable
    ''    Dim I As Integer, J As Integer, n As Integer, SNo As Integer
    ''    Dim LedIdNo As Integer
    ''    Dim NewCode As String
    ''    Dim CompIDCondt As String = ""
    ''    Dim Ent_OrdCd As String = ""
    ''    Dim Ent_Qty As Single = 0
    ''    Dim Ent_rte As Single = 0
    ''    Dim Ent_amt As Single = 0
    ''    Dim Ent_Mtrs As Single = 0
    ''    Dim nr As Single = 0

    ''    If Trim(UCase(cbo_Type.Text)) <> "ORDER" Then
    ''        MessageBox.Show("Invalid Invoice Type", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
    ''        If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
    ''        Exit Sub
    ''    End If

    ''    LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
    ''    If LedIdNo = 0 Then
    ''        MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
    ''        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
    ''        Exit Sub
    ''    End If

    ''    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    ''    With dgv_OrderSelection

    ''        .Rows.Clear()

    ''        SNo = 0


    ''        '---1

    ''        Da = New SqlClient.SqlDataAdapter("select a.*, c.Item_Name  from Garments_Item_PackingSlip_Head a INNER JOIN Garments_Item_PackingSlip_Details b on a.item_Packingslip_code = b.item_Packingslip_code  LEFT OUTER JOIN item_head c ON c.Item_IdNo <> 0 and c.Item_IdNo = b.Item_Idno  Where a.Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
    ''        Dt1 = New DataTable
    ''        Da.Fill(Dt1)


    ''        'Ent_OrdCd = "'0'"

    ''        If Dt1.Rows.Count > 0 Then

    ''            For I = 0 To Dt1.Rows.Count - 1


    ''                n = .Rows.Add()

    ''                ' Ent_OrdCd = Trim(Ent_OrdCd) & IIf(Trim(Ent_OrdCd) <> "", ", ", "") & "'" & Dt1.Rows(I).Item("Item_PackingSlip_Code").ToString & "'"

    ''                SNo = SNo + 1
    ''                .Rows(n).Cells(0).Value = Val(SNo)
    ''                .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Item_PackingSlip_No").ToString
    ''                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(I).Item("Item_PackingSlip_Date").ToString), "dd-MM-yyyy")
    ''                .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Order_No").ToString
    ''                .Rows(n).Cells(4).Value = Dt1.Rows(I).Item("Item_Name").ToString
    ''                .Rows(n).Cells(5).Value = (Val(Dt1.Rows(I).Item("Total_Quantity").ToString) - Val(Dt1.Rows(I).Item("Invoice_Quantity").ToString)) 'Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
    ''                .Rows(n).Cells(6).Value = 0 'Val(Dt1.Rows(I).Item("Balance_Qty").ToString) + Val(Dt1.Rows(I).Item("Ent_Qty").ToString)
    ''                .Rows(n).Cells(7).Value = "1"

    ''                .Rows(n).Cells(8).Value = Dt1.Rows(I).Item("Item_PackingSlip_Code").ToString

    ''                For J = 0 To .ColumnCount - 1
    ''                    .Rows(I).Cells(J).Style.ForeColor = Color.Red
    ''                Next

    ''            Next

    ''        End If
    ''        Dt1.Clear()


    ''        '---2

    ''        Da = New SqlClient.SqlDataAdapter("select a.* , c.Item_Name from Garments_Item_PackingSlip_Head a INNER JOIN Garments_Item_PackingSlip_Details b on a.item_Packingslip_code = b.item_Packingslip_code  LEFT OUTER JOIN item_head c ON c.Item_IdNo <> 0 and c.Item_IdNo = b.Item_Idno Where (a.Invoice_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & ")  order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
    ''        Dt1 = New DataTable
    ''        Da.Fill(Dt1)

    ''        If Dt1.Rows.Count > 0 Then

    ''            For I = 0 To Dt1.Rows.Count - 1

    ''                n = .Rows.Add()

    ''                SNo = SNo + 1
    ''                .Rows(n).Cells(0).Value = Val(SNo)
    ''                .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Item_PackingSlip_No").ToString
    ''                .Rows(n).Cells(2).Value = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
    ''                .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(I).Item("Total_Meters").ToString), "#########0.00")
    ''                .Rows(n).Cells(4).Value = ""
    ''                .Rows(n).Cells(5).Value = Dt1.Rows(I).Item("Item_PackingSlip_Code").ToString

    ''            Next


    ''        End If
    ''        Dt1.Clear()

    ''        If .Rows.Count = 0 Then .Rows.Add()

    ''        pnl_OrderSelection.Visible = True
    ''        pnl_Back.Enabled = False

    ''        .Focus()
    ''        .CurrentCell = .Rows(0).Cells(0)
    ''        .CurrentCell.Selected = True

    ''    End With

    ''End Sub

    ''Private Sub btn_Close_OrderSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_OrderSelection.Click


    ''    Dim Da2 As New SqlClient.SqlDataAdapter
    ''    Dim Dt2 As New DataTable
    ''    Dim n As Integer, i As Integer, j As Integer
    ''    Dim SNo As Integer
    ''    Dim NewCode As String

    ''    dgv_Details.Rows.Clear()

    ''    txt_OrderDate.Text = ""
    ''    txt_OrderNo.Text = ""
    ''    txt_DcNo.Text = ""

    ''    txt_OrderDate.Enabled = False
    ''    txt_OrderNo.Enabled = False
    ''    txt_DcNo.Enabled = False

    ''    pnl_Back.Enabled = True


    ''    dgv_Details.Rows.Clear()

    ''    For i = 0 To dgv_OrderSelection.RowCount - 1

    ''        If Val(dgv_OrderSelection.Rows(i).Cells(7).Value) = 1 Then

    ''            txt_OrderNo.Text = Trim(txt_OrderNo.Text) & IIf(Trim(txt_OrderNo.Text) <> "", ",", "") & Trim(dgv_OrderSelection.Rows(i).Cells(3).Value)
    ''            txt_OrderDate.Text = dgv_OrderSelection.Rows(i).Cells(2).Value
    ''            txt_DcNo.Text = dgv_OrderSelection.Rows(i).Cells(3).Value


    ''            n = dgv_Details.Rows.Add()
    ''            SNo = SNo + 1
    ''            dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
    ''            dgv_Details.Rows(n).Cells(1).Value = dgv_OrderSelection.Rows(i).Cells(4).Value

    ''            'dgv_Details.Rows(n).Cells(2).Value = dgv_OrderSelection.Rows(i).Cells(4).Value
    ''            dgv_Details.Rows(n).Cells(4).Value = dgv_OrderSelection.Rows(i).Cells(5).Value
    ''            dgv_Details.Rows(n).Cells(15).Value = dgv_OrderSelection.Rows(i).Cells(8).Value

    ''            '      Amount_Calculation(n, 7)

    ''        End If

    ''    Next



    ''    pnl_Back.Enabled = True
    ''    pnl_OrderSelection.Visible = False
    ''    If txt_DcNo.Enabled And txt_DcNo.Visible Then txt_DcNo.Focus()


    ''End Sub

    'Private Sub dgv_OrderSelection_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_OrderSelection.CellClick
    '    If dgv_OrderSelection.Rows.Count > 0 And e.RowIndex >= 0 Then
    '        Select_Order(e.RowIndex)
    '    End If
    'End Sub

    'Private Sub dgv_OrderSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_OrderSelection.KeyDown
    '    Dim n As Integer = 0

    '    Try
    '        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
    '            If dgv_OrderSelection.CurrentCell.RowIndex >= 0 Then

    '                n = dgv_OrderSelection.CurrentCell.RowIndex

    '                Select_Order(n)

    '                e.Handled = True

    '            End If
    '        End If

    '    Catch ex As Exception
    '        '---

    '    End Try
    'End Sub

    'Private Sub Select_Order(ByVal RwIndx As Integer)
    '    Dim i As Integer = 0

    '    With dgv_OrderSelection

    '        If .RowCount > 0 And RwIndx >= 0 Then

    '            .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2

    '            If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
    '                Next

    '            Else

    '                .Rows(RwIndx).Cells(7).Value = ""

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
    '                Next

    '            End If

    '        End If


    '    End With

    'End Sub

    '-------------------------------*****************************

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, dtp_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub btn_PackSlip_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PackSlip_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_Type.Text)) <> "PACKING" Then
            MessageBox.Show("Invalid Type", "DOES NOT SELECT Packing Slip...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Packing_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.* from Garments_Item_PackingSlip_Head a Where a.Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Item_PackingSlip_No").ToString
                    .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Quantity").ToString)
                    .Rows(n).Cells(3).Value = 0 'Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(4).Value = "1"
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Item_PackingSlip_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.* from Garments_Item_PackingSlip_Head a Where a.Invoice_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Item_PackingSlip_No").ToString
                    .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Quantity").ToString)
                    .Rows(n).Cells(3).Value = 0 'Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(4).Value = ""
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Item_PackingSlip_Code").ToString

                Next

            End If
            Dt1.Clear()

        End With

        pnl_BaleSelection.Visible = True
        pnl_BaleSelection.BringToFront()
        pnl_Back.Enabled = False

        If txt_BaleNo_Selection.Enabled And txt_BaleNo_Selection.Visible Then txt_BaleNo_Selection.Focus()

    End Sub

    Private Sub btn_Close_PackSlip_Selection_Click(sender As Object, e As EventArgs) Handles btn_Close_PackSlip_Selection.Click

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Q As Single = 0
        Dim Rt As Single = 0
        Dim Rt_Disc_Per As Single = 0
        Dim Rt_Disc_Amt As Single = 0
        Dim NewCode As String = ""
        Dim FsNo As Single = 0, LsNo As Single = 0
        Dim FsBlNo As String = "", LsBlNo As String = ""
        Dim vBl_No As String = ""

        ' txt_NoofBundles.Text = ""

        pnl_Back.Enabled = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        dgv_Details.Rows.Clear()
        dgv_BaleDetails.Rows.Clear()

        NoCalc_Status = True
        sno = 0

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        Cmd.ExecuteNonQuery()

        sno = 0




        For i = 0 To dgv_Packing_Selection.RowCount - 1

            If Val(dgv_Packing_Selection.Rows(i).Cells(4).Value) = 1 Then

                n = dgv_BaleDetails.Rows.Add()

                sno = sno + 1


                dgv_BaleDetails.Rows(n).Cells(0).Value = Val(sno)
                dgv_BaleDetails.Rows(n).Cells(1).Value = dgv_Packing_Selection.Rows(i).Cells(1).Value
                dgv_BaleDetails.Rows(n).Cells(2).Value = Val(dgv_Packing_Selection.Rows(i).Cells(2).Value)
                dgv_BaleDetails.Rows(n).Cells(3).Value = Format(Val(dgv_Packing_Selection.Rows(i).Cells(3).Value), "#########0.00")
                dgv_BaleDetails.Rows(n).Cells(4).Value = dgv_Packing_Selection.Rows(i).Cells(5).Value


                Cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & " (Int1, Int2,Int3, Weight1, Meters1) Select Company_Idno, Item_IdNo,Ledger_Idno, Quantity, 0 from Garments_Item_PackingSlip_Details where Item_PackingSlip_Code = '" & Trim(dgv_Packing_Selection.Rows(i).Cells(5).Value) & "'"
                Cmd.ExecuteNonQuery()

                'Cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & " (Int1, Int2,Int3, Weight1, Meters1) Select Company_Idno, Item_IdNo,Ledger_Idno, Quantity, Meters from Garments_Item_PackingSlip_Details where Item_PackingSlip_Code = '" & Trim(dgv_Packing_Selection.Rows(i).Cells(5).Value) & "'"
                'Cmd.ExecuteNonQuery()

                Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Name1 ) values ('" & Trim(dgv_Packing_Selection.Rows(i).Cells(5).Value) & "')"
                Cmd.ExecuteNonQuery()

                'txt_NoofBundles.Text = Val(txt_NoofBundles.Text) + 1
            End If

        Next i

        Da = New SqlClient.SqlDataAdapter("select a.Int1 as Company_IdNo, a.Int2 as Item_IdNo,a.Int3 as LedgerIdno, b.Item_Name ,  c.Unit_Name, b.Sales_Rate, sum(a.Weight1) as qty, sum(a.Meters1) as meters from " & Trim(Common_Procedures.EntryTempTable) & " a INNER JOIN Item_Head b ON a.Int2 = b.Item_IdNo LEFT OUTER JOIN Unit_Head c ON b.Unit_IdNo = c.Unit_IdNo   group by a.int1, a.Int2,INT3, b.Item_Name, c.Unit_Name,  b.Sales_Rate Order by b.Item_Name, a.int1, a.Int2, c.Unit_Name, b.Sales_Rate", con)
        'Da = New SqlClient.SqlDataAdapter("select a.Int1 as Company_IdNo, a.Int2 as Item_IdNo,a.Int3 as LedgerIdno, b.Processed_Item_Name , e.Party_ItemName AS Salesname, c.Unit_Name, b.Sales_Rate, sum(a.Weight1) as qty, sum(a.Meters1) as meters from " & Trim(Common_Procedures.EntryTempTable) & " a INNER JOIN Processed_Item_Head b ON a.Int2 = b.Processed_Item_IdNo LEFT OUTER JOIN Unit_Head c ON b.Unit_IdNo = c.Unit_IdNo LEFT OUTER JOIN Ledger_ItemName_Details e ON   a.Int3=e.Ledger_Idno  and a.int2 = e.Item_IdNo  group by a.int1, a.Int2,INT3, b.Processed_Item_Name, c.Unit_Name, e.Party_ItemName, b.Sales_Rate Order by b.Processed_Item_Name, a.int1, a.Int2,e.Party_ItemName, c.Unit_Name, b.Sales_Rate", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        sno = 0

        If Dt1.Rows.Count > 0 Then

            For i = 0 To Dt1.Rows.Count - 1

                Rt = 0

                Da = New SqlClient.SqlDataAdapter("Select a.* from Garments_Sales_Return_Details a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Item_IdNo = " & Str(Val(Dt1.Rows(i).Item("Item_IdNo").ToString)) & " Order by a.sl_no", con)
                ' Da = New SqlClient.SqlDataAdapter("Select a.* from FinishedProduct_Invoice_Details a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.FinishedProduct_IdNo = " & Str(Val(Dt1.Rows(i).Item("Item_IdNo").ToString)) & " Order by a.sl_no", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)

                If Dt2.Rows.Count > 0 Then
                    If IsDBNull(Dt2.Rows(0).Item("Rate").ToString) = False Then
                        Rt = Val(Dt2.Rows(0).Item("Rate").ToString)
                    End If
                End If
                Dt2.Clear()
                Da.Dispose()

                Da = New SqlClient.SqlDataAdapter("Select a.* from Ledger_ItemName_Details a Where  a.Ledger_IdNo = " & Str(Val(Dt1.Rows(i).Item("LedgerIdno").ToString)) & " and a.Item_Idno = " & Str(Val(Dt1.Rows(i).Item("Item_IdNo").ToString)) & "", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                Rt_Disc_Per = 0
                Rt_Disc_Amt = 0
                If Dt2.Rows.Count > 0 Then
                    If IsDBNull(Dt2.Rows(0).Item("Rate_Disc_Percentage").ToString) = False Then
                        Rt_Disc_Per = Val(Dt2.Rows(0).Item("Rate_Disc_Percentage").ToString)
                    End If

                    If IsDBNull(Dt2.Rows(0).Item("Rate_Disc_Amount").ToString) = False Then
                        Rt_Disc_Amt = Val(Dt2.Rows(0).Item("Rate_Disc_Amount").ToString)
                    End If

                End If
                Dt2.Clear()
                Da.Dispose()
                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Item_Name").ToString
                dgv_Details.Rows(n).Cells(2).Value = "" 'Dt1.Rows(i).Item("Salesname").ToString
                dgv_Details.Rows(n).Cells(3).Value = 0 'Val(Dt1.Rows(i).Item("qty").ToString)
                dgv_Details.Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("qty").ToString) 'Format(Val(Dt1.Rows(i).Item("meters").ToString), "#########0.00")
                dgv_Details.Rows(n).Cells(5).Value = "" 'Dt1.Rows(i).Item("Unit_Name").ToString

                If Rt = 0 Then
                    Rt = Val(Dt1.Rows(i).Item("Sales_Rate").ToString)
                End If

                If Rt_Disc_Per <> 0 Then
                    Rt = Val(Rt) - (Val(Rt) * Val(Rt_Disc_Per) / 100)
                Else
                    Rt = Val(Rt) - Val(Rt_Disc_Amt)
                End If


                dgv_Details.Rows(n).Cells(6).Value = Format(Val(Rt), "#########0.00")

                If InStr(1, Trim(UCase(Dt1.Rows(i).Item("Unit_Name").ToString)), "MTR") > 0 Or InStr(1, Trim(UCase(Dt1.Rows(i).Item("Unit_Name").ToString)), "METER") > 0 Or InStr(1, Trim(UCase(Dt1.Rows(i).Item("Unit_Name").ToString)), "METRE") > 0 Then
                    Q = Val(Dt1.Rows(i).Item("meters").ToString)
                Else
                    Q = Val(Dt1.Rows(i).Item("qty").ToString)
                End If

                dgv_Details.Rows(n).Cells(7).Value = Format(Val(Q) * Val(Rt), "#########0.00")

            Next

        End If

        vBl_No = ""
        FsNo = 0 : LsNo = 0
        FsBlNo = "" : LsBlNo = ""

        Da = New SqlClient.SqlDataAdapter("Select b.Item_PackingSlip_No, b.For_OrderBy from " & Trim(Common_Procedures.ReportTempTable) & " a, Garments_Item_PackingSlip_Head b where a.Name1 = b.Item_PackingSlip_Code order by b.For_OrderBy, b.Item_PackingSlip_No", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            FsNo = Dt1.Rows(0).Item("For_OrderBy").ToString
            LsNo = Dt1.Rows(0).Item("For_OrderBy").ToString

            FsBlNo = Trim(UCase(Dt1.Rows(0).Item("Item_PackingSlip_No").ToString))
            LsBlNo = Trim(UCase(Dt1.Rows(0).Item("Item_PackingSlip_No").ToString))

            For i = 1 To Dt1.Rows.Count - 1
                If LsNo + 1 = Val(Dt1.Rows(i).Item("For_OrderBy").ToString) Then
                    LsNo = Val(Dt1.Rows(i).Item("For_OrderBy").ToString)
                    LsBlNo = Trim(UCase(Dt1.Rows(i).Item("Item_PackingSlip_No").ToString))

                Else
                    If FsNo = LsNo Then
                        vBl_No = vBl_No & Trim(FsBlNo) & ","
                    Else
                        vBl_No = vBl_No & Trim(FsBlNo) & "-" & Trim(LsBlNo) & ","
                    End If
                    FsNo = Dt1.Rows(i).Item("For_OrderBy").ToString
                    LsNo = Dt1.Rows(i).Item("For_OrderBy").ToString

                    FsBlNo = Trim(UCase(Dt1.Rows(i).Item("Item_PackingSlip_No").ToString))
                    LsBlNo = Trim(UCase(Dt1.Rows(i).Item("Item_PackingSlip_No").ToString))

                End If

            Next

            If FsNo = LsNo Then vBl_No = vBl_No & Trim(FsBlNo) Else vBl_No = vBl_No & Trim(FsBlNo) & "-" & Trim(LsBlNo)

        End If
        Dt1.Clear()

        'lbl_BaleNos.Text = Trim(vBl_No)
        txt_DcNo.Text = Trim(vBl_No)

        NoCalc_Status = False
        ' Total_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_BaleSelection.Visible = False

        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()

            dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
            dgv_Details.Columns(5).ReadOnly = False

            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
            dgv_Details.CurrentCell.Selected = True

        Else
            txt_CashDiscPerc.Focus()

        End If



    End Sub

    Private Sub dgv_BaleDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BaleDetails.CellEnter
        dgv_ActCtrlName = dgv_BaleDetails.Name
    End Sub


    Private Sub dgv_BaleDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BaleDetails.KeyDown
        On Error Resume Next

        With dgv_BaleDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True

                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
                        dgv_Details.CurrentCell.Selected = True

                    Else
                        cbo_Transport.Focus()

                    End If
                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    txt_CashDiscPerc.Focus()
                End If
            End If

        End With

    End Sub

    Private Sub dgv_BaleDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BaleDetails.LostFocus
        On Error Resume Next
        dgv_BaleDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Packing_Selection_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Packing_Selection.CellClick
        Grid_Pack_Selection(e.RowIndex)
    End Sub



    Private Sub Grid_Pack_Selection(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Packing_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(4).Value = (Val(.Rows(RwIndx).Cells(4).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(4).Value) = 0 Then

                    .Rows(RwIndx).Cells(4).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next

                Else
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                End If

            End If



        End With

    End Sub

    Private Sub dgv_Packing_Selection_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_Packing_Selection.KeyDown

        Dim n As Integer

        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Packing_Selection.CurrentCell.RowIndex >= 0 Then

                    n = dgv_Packing_Selection.CurrentCell.RowIndex

                    Grid_Pack_Selection(n)

                    e.Handled = True

                End If
            End If

        Catch ex As Exception
            '---

        End Try


    End Sub

    Private Sub btn_SelectBale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_SelectBale.Click
        Dim BlNo As String
        Dim i As Integer

        If Trim(txt_BaleNo_Selection.Text) <> "" Then

            BlNo = Trim(txt_BaleNo_Selection.Text)

            For i = 0 To dgv_Packing_Selection.Rows.Count - 1
                If Trim(UCase(BlNo)) = Trim(UCase(dgv_Packing_Selection.Rows(i).Cells(1).Value)) Then
                    Call Grid_Pack_Selection(i)
                    Exit For
                End If
            Next

            txt_BaleNo_Selection.Text = ""

        End If

    End Sub

    Private Sub cbo_Type_TextChanged(sender As Object, e As EventArgs) Handles cbo_Type.TextChanged
        If (Trim(UCase(cbo_Type.Text)) <> "DIRECT") Then
            cbo_ItemName.Enabled = True
            cbo_Style.Enabled = False
            cbo_Size.Enabled = False
            txt_NoofPcs.Enabled = False
        Else
            cbo_ItemName.Enabled = True
            cbo_Style.Enabled = True
            cbo_Size.Enabled = True
            txt_NoofPcs.Enabled = True
        End If
    End Sub


End Class
